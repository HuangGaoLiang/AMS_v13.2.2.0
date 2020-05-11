using AMS.Anticorrosion.HRS;
using AMS.Core;
using AMS.Dto;
using AMS.Models;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using AMS.Storage.Repository.Orders;
using AutoMapper;
using Jerrisoft.Platform.Public.PageExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Storage;
using System.Data;
using AMS.Core.Locks;

namespace AMS.Service
{
    /// <summary>
    /// 描    述: 报名订单
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class EnrollOrderService : BaseOrderService
    {
        #region 仓储/字段/构造函数
        private readonly Lazy<TblOdrEnrollOrderRepository> _enrollOrderRepository = new Lazy<TblOdrEnrollOrderRepository>();
        private readonly Lazy<TblOdrEnrollOrderItemRepository> _enrollOrderItemRepository = new Lazy<TblOdrEnrollOrderItemRepository>();

        /// <summary>
        /// 报名订单类型
        /// </summary>
        protected override string OrderNoTag => "B";

        /// <summary>
        /// 校区编号
        /// </summary>
        private readonly string _schoolId;

        /// <summary>
        /// 实例化一个校区的订单
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        public EnrollOrderService(string schoolId)
        {
            _schoolId = schoolId;
        }

        #endregion

        #region 根据招生专员Id获取学生报名信息
        /// <summary>
        /// 根据招生专员Id获取学生报名信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-14</para>
        /// </summary>
        /// <param name="creatorId">招生专员Id</param>
        /// <returns>报名订单信息列表</returns>
        internal async Task<List<TblOdrEnrollOrder>> GetEnrollOrderByCreatorId(string creatorId)
        {
            return await _enrollOrderRepository.Value.GetEnrollOrderByCreatorId(_schoolId, creatorId);
        }
        #endregion

        #region GetEnrollOrderItemByEnrollOrderId 根据报名订单Id获取报名的课程
        /// <summary>
        /// 根据报名订单Id获取报名的课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="enrollOrderId">报名订单集合</param>
        /// <returns></returns>
        internal async Task<List<TblOdrEnrollOrderItem>> GetEnrollOrderItemByEnrollOrderId(IEnumerable<long> enrollOrderId)
        {
            return await _enrollOrderItemRepository.Value.GetByEnrollOrderId(enrollOrderId);
        }
        #endregion

        #region Enrolling 报名并排课

        /// <summary>
        /// 报名并排课
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间:2018-11-02 </para>
        /// </summary>
        /// <param name="dto">报名提交请求参数</param>
        /// <param name="userId">操作人编号</param>
        /// <param name="userName">操作人名称</param>
        /// <param name="companyId">归属公司编号</param>
        /// <exception cref="BussinessException">
        /// 异常ID：15.排课班级不能重复，请重新排课
        /// 异常ID：13.排课的班级有误，请重新选择
        /// 异常ID：7.数据异常
        /// 异常ID：14.请输入正确的付款金额
        /// </exception>
        public string Enrolling(EnrollOrderRequest dto, string userId, string userName, string companyId)
        {
            long enrollOrderId = IdGenerator.NextId();
            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_AMSSCHOOLSTUDENT, this._schoolId, dto.StudentId.ToString()))
            {
                // 1、数据验证
                SignUpCheck(dto);

                // 2、报名订单
                TblOdrEnrollOrder order = this.SetEnrollOrder(dto, userId, userName, enrollOrderId);

                // 3、报名订单课程明细
                List<MakeLessonOrderItemResponse> itemList = AddOrderItem(dto, enrollOrderId);

                // 4、获取排课数据
                List<OrderItemResponse> makeList = GetMakeLesson(itemList);
                List<TblOdrEnrollOrderItem> orderItemList = Mapper.Map<List<MakeLessonOrderItemResponse>, List<TblOdrEnrollOrderItem>>(itemList);

                // 5、报名业绩
                List<TblOdrEnrollOrderAchieve> addAchievementList = AddAchievement(dto.AchievementList, enrollOrderId, _schoolId);

                // 6、学生校区归属表
                TblCstSchoolStudent schoolStudent = AddSchoolStudent(dto.StudentId, _schoolId, companyId);

                // 7、资金处理
                EnrollOrderTrade orderTrade = new EnrollOrderTrade(order);

                // 8排课会总动更新课次
                List<MakeLessonRequest> makeLessonList = MakeLess(dto.FirstClassTime.Value, makeList);

                //添加事务
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.BeginTransaction(IsolationLevel.Snapshot);

                        //报名订单优惠
                        List<TblOdrEnrollOrderDiscount> addDiscounts = AddDiscounts(dto, enrollOrderId, unitOfWork);

                        unitOfWork.GetCustomRepository<TblOdrEnrollOrderRepository, TblOdrEnrollOrder>().Add(order);
                        unitOfWork.GetCustomRepository<TblOdrEnrollOrderItemRepository, TblOdrEnrollOrderItem>().Add(orderItemList);
                        unitOfWork.GetCustomRepository<TblOdrEnrollOrderAchieveRepository, TblOdrEnrollOrderAchieve>().Add(addAchievementList);
                        unitOfWork.GetCustomRepository<TblOdrEnrollOrderDiscountRepository, TblOdrEnrollOrderDiscount>().Add(addDiscounts);

                        // 如果学生信息不为空
                        if (schoolStudent != null)
                        {
                            unitOfWork.GetCustomRepository<TblCstSchoolStudentRepository, TblCstSchoolStudent>().Add(schoolStudent);
                        }

                        // 如果有排课
                        if (makeList.Any())
                        {
                            new MakeLessonService(_schoolId, dto.StudentId).Make(makeLessonList, unitOfWork);
                        }
                        else
                        {
                            new StudentService(_schoolId).UpdateStudentStatusById(dto.StudentId, unitOfWork, StudyStatus.Reading);//更新学生剩余课次
                        }
                        TradeService tradeService = new TradeService(orderTrade, unitOfWork);
                        tradeService.Trade();

                        unitOfWork.CommitTransaction();

                        return enrollOrderId.ToString();
                    }
                    catch (Exception ex)
                    {
                        unitOfWork.RollbackTransaction();
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 获取需要排课的信息
        ///  <para>作     者:Huang GaoLiang </para>
        ///  <para>创建时间:2018-11-26 </para>
        /// </summary>
        /// <param name="list">报名课程明细</param>
        /// <returns>返回报名课程明细数据</returns>
        private List<OrderItemResponse> GetMakeLesson(List<MakeLessonOrderItemResponse> list)
        {
            List<OrderItemResponse> orderItemList = new List<OrderItemResponse>();

            // 如果报名课程明细存在
            if (list.Any())
            {
                foreach (var item in list)
                {
                    // 如果排课班级编号小于等于O
                    if (item.ClassId <= 0)
                    {
                        continue;
                    }
                    OrderItemResponse o = new OrderItemResponse
                    {
                        EnrollOrderItemId = item.EnrollOrderItemId,
                        ClassId = item.ClassId,
                        ClassTimes = item.ClassScheduling
                    };
                    orderItemList.Add(o);
                }
                return orderItemList;
            }

            return orderItemList;
        }

        /// <summary>
        /// 组装报名订单
        /// <para>作     者:HuangGaoLiang </para>
        /// <para>创建时间:2018-11-16 </para>
        /// </summary>
        /// <param name="dto">报名排课请求数据</param>
        /// <param name="userId">用户编号</param>
        /// <param name="userName">用户名称</param>
        /// <param name="enrollOrderId">订单主键</param>
        /// <returns>返回订单信息</returns>
        private TblOdrEnrollOrder SetEnrollOrder(EnrollOrderRequest dto, string userId, string userName, long enrollOrderId)
        {
            // 根据学生编号，查询该学生最早的一条报名记录
            TblOdrEnrollOrder orderInfo = _enrollOrderRepository.Value.GetOrderByStudentId(dto.StudentId);

            OrderNewType orderNewType = OrderNewType.NewStu;

            // 判断新生订单还是老生订单
            if (orderInfo != null && ((DateTime.Now - orderInfo.CreateTime).Days >= 30))
            {
                orderNewType = OrderNewType.OldStu;
            }

            TblOdrEnrollOrder order = new TblOdrEnrollOrder
            {
                EnrollOrderId = enrollOrderId,
                OrderNo = CreateOrderNo(enrollOrderId),
                OrderNewType = (int)orderNewType,
                StudentId = dto.StudentId,
                SchoolId = _schoolId,
                TotalClassTimes = dto.TotalClassTimes,
                TotalTuitionFee = dto.TotalTuitionFee,
                TotalMaterialFee = dto.TotalMaterialFee,
                TotalDiscountFee = dto.TotalDiscountFee + dto.TotalScholarshipFee, //优惠总额 = 赠与奖学金+ 奖学金券
                PayType = dto.PayType,
                TotalTradeAmount = dto.PayAmount + dto.UseBalanceAmount,
                PayAmount = dto.PayAmount,
                UseBalance = dto.UseBalanceAmount,
                CreateId = userId,
                CreateName = userName,
                CreateTime = DateTime.Now,
                CancelUserId = "",
                CancelRemark = "",
                OrderStatus = (int)OrderStatus.Paid,
                Remark = dto.Remark
            };
            return order;
        }

        /// <summary>
        /// 排课数据构造
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间:2018-11-06 </para>
        /// </summary>
        /// <param name="firstClassTime">首次上课时间</param>
        /// <param name="orderItem">报班明细</param>
        private List<MakeLessonRequest> MakeLess(DateTime firstClassTime, List<OrderItemResponse> orderItem)
        {
            List<MakeLessonRequest> makeLessonList = new List<MakeLessonRequest>();

            foreach (OrderItemResponse item in orderItem)
            {
                MakeLessonRequest make = new MakeLessonRequest
                {
                    ClassId = item.ClassId,
                    ClassTimes = item.ClassTimes,
                    EnrollOrderItemId = item.EnrollOrderItemId,
                    FirstClassTime = firstClassTime
                };
                makeLessonList.Add(make);
            }
            return makeLessonList;
        }

        /// <summary>
        /// 添加报名订单课程明细
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间:2018-11-05 </para>
        /// </summary>
        /// <param name="dto">报名订单课信息</param>
        /// <param name="enrollOrderId">报名订单编号</param>
        /// <returns>返回排课集合信息</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：7,数据异常
        /// </exception>
        private List<MakeLessonOrderItemResponse> AddOrderItem(EnrollOrderRequest dto, long enrollOrderId)
        {
            decimal totalDiscountFee = dto.TotalDiscountFee;//优惠总额
            decimal totalAmount = 0;

            List<EnrollOrderItemRequest> items = dto.EnrollOrderItem;//报名课程明细

            List<MakeLessonOrderItemResponse> orderItem = new List<MakeLessonOrderItemResponse>();//报名订单课程明细集合

            foreach (EnrollOrderItemRequest item in items)
            {
                // 根据学期类型查询学期信息
                TermDetailResponse term = new TermService(_schoolId, item.Year).GetTermList().Where(m => m.TermTypeId == item.TermTypeId)?.FirstOrDefault();
                if (term == null)
                {
                    throw new BussinessException((byte)ModelType.SignUp, 7);
                }

                totalAmount += (term.TuitionFee + term.MaterialFee) * item.ClassTimes;//计算报名课程订单课程的总额

                long enrollOrderItemId = IdGenerator.NextId();//报名订单课程明细编号

                MakeLessonOrderItemResponse orderItems = SetEnrollOrderItem(enrollOrderId, enrollOrderItemId, item, term);
                orderItems.ClassId = item.ClassId;

                orderItem.Add(orderItems);
            }

            // 计算学费单价(实价)和杂费单价(实价)
            GetFee(orderItem, totalDiscountFee, totalAmount);
            return orderItem;
        }

        /// <summary>
        /// 设置报名订单课程明细
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间:2018-11-16 </para>
        /// </summary>
        /// <param name="enrollOrderId">报名订单编号</param>
        /// <param name="enrollOrderItemId">报名订单课程明细</param>
        /// <param name="item">订单课程明细类</param>
        /// <param name="term">学期数据</param>
        /// <returns>返回排课信息</returns>
        private MakeLessonOrderItemResponse SetEnrollOrderItem(long enrollOrderId, long enrollOrderItemId,
            EnrollOrderItemRequest item, TermDetailResponse term)
        {
            MakeLessonOrderItemResponse orderItems = new MakeLessonOrderItemResponse
            {
                EnrollOrderItemId = enrollOrderItemId,
                SchoolId = _schoolId,
                EnrollOrderId = enrollOrderId,
                EnrollType = item.EnrollType,
                TermTypeId = item.TermTypeId,
                Year = item.Year,
                CourseId = item.CourseId,
                CourseType = item.CourseType,
                CourseLevelId = item.CourseLevelId,
                Duration = item.Duration,
                ClassTimes = item.ClassTimes,
                ClassScheduling = item.ClassScheduling,
                ClassTimesUse = 0,
                TuitionFee = term.TuitionFee, //学费单价
                MaterialFee = term.MaterialFee, //杂费单价
                Status = (int)OrderItemStatus.Enroll
            };
            return orderItems;
        }

        /// <summary>
        /// 计算学费单价(实价)和杂费单价(实价)
        /// <para>作     者：Huang GaoLiang </para>
        /// <para>创建时间:2018-11-19 </para>
        /// </summary>
        /// <param name="orderItem">报名订单课程明细</param>
        /// <param name="totalDiscountFee">优惠总额</param>
        /// <param name="totalAmount">报名课程订单课程的总额</param>
        private static void GetFee(List<MakeLessonOrderItemResponse> orderItem, decimal totalDiscountFee, decimal totalAmount)
        {
            foreach (MakeLessonOrderItemResponse item in orderItem)
            {
                //没有优惠
                if (totalDiscountFee == 0)
                {
                    item.TuitionFeeReal = item.TuitionFee;
                    item.MaterialFeeReal = item.MaterialFee;
                    item.DiscountFee = 0;
                }
                else
                {
                    decimal feeReal = Math.Round(totalDiscountFee * ((item.TuitionFee + item.MaterialFee) * item.ClassTimes / totalAmount) / item.ClassTimes, 4); //单次课程的优惠

                    if (feeReal > item.TuitionFee)
                    {
                        item.TuitionFeeReal = 0;
                        item.MaterialFeeReal = item.MaterialFee - (feeReal - item.TuitionFee);
                    }
                    else if (feeReal == item.TuitionFee)
                    {
                        item.TuitionFeeReal = item.TuitionFee;
                        item.MaterialFeeReal = item.MaterialFee;
                    }
                    else if (feeReal < item.TuitionFee)
                    {
                        item.TuitionFeeReal = item.TuitionFee - feeReal;
                        item.MaterialFeeReal = item.MaterialFee;
                    }

                    item.DiscountFee = Math.Round(item.DiscountFee * ((item.TuitionFee + item.MaterialFee) * item.ClassTimes / totalAmount), 4);//优惠金额=总的优惠金额*（（课程原价+杂费原价）*课次/报名的总费用）
                }
            }
        }

        /// <summary>
        /// 添加业绩归属人
        /// <para>作     者:Huang GaoLiang  </para>
        /// <para>创建时间: 2018-11-05 </para>
        /// </summary>
        /// <param name="achievements">业绩人</param>
        /// <param name="enrollOrderId">所属订单</param>
        /// <param name="schoolId">校区编号</param>
        /// <returns>返回业绩归属人集合</returns>
        private List<TblOdrEnrollOrderAchieve> AddAchievement(List<AchievementRequest> achievements, long enrollOrderId, string schoolId)
        {
            List<TblOdrEnrollOrderAchieve> achieveList = new List<TblOdrEnrollOrderAchieve>();
            foreach (var item in achievements)
            {
                TblOdrEnrollOrderAchieve a = new TblOdrEnrollOrderAchieve()
                {
                    EnrollOrderAchieveId = IdGenerator.NextId(),
                    SchoolId = schoolId,
                    EnrollOrderId = enrollOrderId,
                    PersonalId = item.PersonalId,
                    Proportion = item.Proportion
                };
                achieveList.Add(a);
            }
            return achieveList;
        }

        /// <summary>
        /// 添加报名订单优惠
        /// <para>作     者：Huang GaoLiang </para>
        /// <para>创建时间:2018-11-15 </para>
        /// </summary>
        /// <param name="dto">奖学金券</param>
        /// <param name="enrollOrderId">所属订单编号</param>
        /// <param name="unitOfWork">事务</param>
        /// <returns>返回优惠信息集合</returns>
        private List<TblOdrEnrollOrderDiscount> AddDiscounts(EnrollOrderRequest dto, long enrollOrderId, UnitOfWork unitOfWork)
        {
            List<EnrollDiscountRequest> discounts = dto.EnrollDiscounts;
            decimal money = dto.TotalTuitionFee + dto.TotalMaterialFee;//报名总额（学费总额+杂费总额）                                                                   
            var couponIdList = discounts.Select(m => m.CouponId).Distinct().ToList();

            //根据学生编号获取学生的转介绍人编号
            long? parentId = null;

            //根据学生编号查询学生信息
            var info = StudentService.GetStudentInfo(dto.StudentId);
            if (info != null && info.ParentId > 0)
            {
                parentId = info.ParentId;
            }

            List<CouponGenerateResponse> list = new CouponRuleEnrollProducer(_schoolId, money).Create(couponIdList, dto.StudentId, parentId, dto.RecommendCouponRuleId, dto.FullCouponRulId, enrollOrderId, unitOfWork);

            List<TblOdrEnrollOrderDiscount> discountList = new List<TblOdrEnrollOrderDiscount>();
            foreach (var item in list)
            {
                TblOdrEnrollOrderDiscount a = new TblOdrEnrollOrderDiscount
                {
                    EnrollOrderDiscountId = IdGenerator.NextId(),
                    SchoolId = _schoolId,
                    EnrollOrderId = enrollOrderId,
                    CouponId = item.CouponId,
                    CouponAmount = item.CouponRuleAmount,
                    CouponType = (int)item.CouponType
                };
                discountList.Add(a);
            }
            return discountList;
        }

        /// <summary>
        /// 添加学生校区归属表
        /// <para>作     者：Huang GaoLiang </para>
        /// <para>创建时间:2018-11-05 </para>
        /// </summary>
        /// <param name="studentId">学生编号</param>
        /// <param name="schoolId">校区编号</param>
        /// <param name="companyId">归属公司编号</param>
        /// <returns>返回校区学生信息</returns>
        private TblCstSchoolStudent AddSchoolStudent(long studentId, string schoolId, string companyId)
        {
            if (studentId <= 0)
            {
                throw new BussinessException((byte)ModelType.SignUp, 7);
            }

            // 根据学生编号和校区编号查询校区学生信息
            TblCstSchoolStudent stu = new TblCstSchoolStudentRepository().GetStudentById(_schoolId, studentId).Result;
            if (stu == null)
            {
                TblCstSchoolStudent schoolStudent = new TblCstSchoolStudent
                {
                    SchoolStudentId = IdGenerator.NextId(),
                    CompanyId = companyId,
                    SchoolId = schoolId,
                    StudentId = studentId,
                    StudyStatus = (byte)StudyStatus.Reading,
                    RemindClassTimes = 0,//默认剩余课次为0
                    CreateTime = DateTime.Now
                };
                return schoolStudent;
            }
            return null;
        }

        /// <summary>
        /// 报名数据校验
        /// <para>作     者：Huang GaoLiang </para>
        /// <para>创建时间:2018-11-02 </para>
        /// </summary>
        /// <param name="dto">报名提交请求参数</param>
        /// <exception cref="BussinessException">
        /// 异常ID： 15.排课班级不能重复，请重新排课
        /// 异常ID： 7.数据异常
        /// 异常ID： 13.排课的班级有误，请重新选择
        /// 异常ID： 14.请输入正确的付款金额
        /// </exception>
        private void SignUpCheck(EnrollOrderRequest dto)
        {
            DataValidate(dto);

            // 校验排课的信息与报名的信息是否一致
            List<EnrollOrderItemRequest> enrollOrderItemList = dto.EnrollOrderItem;

            // 1、获取报名提交数据中的排课信息
            var courseList = enrollOrderItemList.Where(m => m.ClassId > 0).Select(m => m.ClassId).GroupBy(m => m).Select(x => new
            {
                key = x.Key,
                count = x.Count()

            }).ToList();

            // 判断是否重复排课
            if (courseList.Any(m => m.count > 1))
            {
                throw new BussinessException((byte)ModelType.SignUp, 15);
            }

            List<TblDatTerm> datTermList = TermService.GetSchoolIdTermList(_schoolId);

            decimal totalAmount = 0;//报名课程订单课程的总额

            foreach (EnrollOrderItemRequest orderItem in enrollOrderItemList)
            {
                TblDatClass datClass = new TblDatClass();

                // 2、校验报名的课程信息是否正确
                datTermList = datTermList.Where(m => m.Year == orderItem.Year && m.TermTypeId == orderItem.TermTypeId).ToList();
                if (orderItem.ClassId > 0)
                {
                    datClass = new DefaultClassService(orderItem.ClassId).TblDatClass;

                    // 判断班级信息是否存在
                    if (datClass == null)
                    {
                        throw new BussinessException((byte)ModelType.SignUp, 7);
                    }

                    List<long> termIds = datTermList.Select(m => m.TermId).Distinct().ToList();
                    datTermList = datTermList.Where(m => termIds.Contains(datClass.TermId)).ToList();

                    // 判断学期是否存在
                    if (!datTermList.Any())
                    {
                        throw new BussinessException((byte)ModelType.SignUp, 13);
                    }
                }

                // 3、检查报名的课程费用
                TermDetailResponse term = new TermService(_schoolId, orderItem.Year).GetTermList().FirstOrDefault(m => m.TermTypeId == orderItem.TermTypeId);
                if (term != null)
                {
                    totalAmount += (term.TuitionFee + term.MaterialFee) * orderItem.ClassTimes;
                }
            }

            decimal money = dto.TotalDiscountFee + dto.UseBalanceAmount + dto.PayAmount;
            // 检验付款金额+优惠总额与所报课程的金额是否相等
            if (totalAmount != money)
            {
                throw new BussinessException((byte)ModelType.SignUp, 14);
            }
        }


        /// <summary>
        /// 报名提交，数据校验
        /// <para>作     者：Huang GaoLiang </para>
        /// <para>创建时间:2018-11-11 </para>
        /// </summary>
        /// <param name="dto">报名提交数据</param>
        private void DataValidate(EnrollOrderRequest dto)
        {
            // 排课那边需要数据处理
            if (!dto.FirstClassTime.HasValue)
            {
                dto.FirstClassTime = DateTime.Now;
            }

            // 处理支付方式
            if (dto.PayType == 0)
            {
                dto.PayType = (int)PayType.Other;
            }

            //判断报名总课次
            if (dto.TotalClassTimes < 1) //报名总课次不能小于0
            {
                throw new BussinessException((byte)ModelType.SignUp, 3);
            }

            //业绩归属人不能为空
            if (!dto.AchievementList.Any())
            {
                throw new BussinessException((byte)ModelType.SignUp, 4);
            }

            //支付金额不能小于0
            if (dto.PayAmount < 0)
            {
                throw new BussinessException((byte)ModelType.SignUp, 16);
            }

            //验证余额够不够
            if (dto.UseBalanceAmount > 0)
            {
                //根据校区编号和学生编号判断学生使用的余额
                bool useBalance = WalletService.IsWalletSufficient(_schoolId, dto.StudentId, dto.UseBalanceAmount);
                if (!useBalance)
                {
                    throw new BussinessException((byte)ModelType.SignUp, 17);
                }
            }
        }

        #endregion

        #region  GetPageList 获取报班订单分页列表
        /// <summary>
        /// 获取报班订单分页列表
        /// <para>作     者：Huang GaoLiang </para>
        /// <para>创建时间:2018-11-04 </para>
        /// </summary>
        /// <param name="dto">报班查询请求数据</param>
        /// <returns>返回报班分页数据</returns>
        public PageResult<EnrollOrderListResponse> GetPageList(EnrollOrderListSearchRequest dto)
        {
            PageResult<ViewOdrEnrollOrder> orderList = new ViewOdrEnrollOrderRepository().GetOrderList(dto);
            PageResult<EnrollOrderListResponse> list = Mapper.Map<PageResult<ViewOdrEnrollOrder>, PageResult<EnrollOrderListResponse>>(orderList);

            List<EnrollOrderListResponse> resultList = list.Data;
            foreach (var item in resultList)
            {
                if (item.UseBalance > 0 && item.PayTypeId == (int)PayType.Other && item.PayAmount <= 0)
                {
                    item.PayType = "余额";
                }
                else if (item.UseBalance > 0 && item.PayTypeId > 0)
                {
                    item.PayType = $"余额、{item.PayType}";
                }
            }
            return list;
        }

        #endregion


        #region GetOrderList 获取报班订单分页列表
        /// <summary>
        /// 获取报班订单分页列表
        /// <para>作     者：Huang GaoLiang </para>
        /// <para>创建时间:2018-11-04 </para>
        /// </summary>
        /// <param name="searcher"></param>
        /// <returns></returns>
        public override LeaveSchoolOrderListResponse GetOrderList(IOrderListSearchRequest searcher)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region GetOrderDetail 获取招生订单详情

        /// <summary>
        /// 获取招生订单详情 
        /// <para>作     者：Huang GaoLiang </para>
        /// <para>创建时间:2018-11-01 </para>
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>返回订单详情信息</returns>
        public override IOrderDetailResponse GetOrderDetail(long orderId, string companyId)
        {
            EnrollOrderDetailResponse orderDetail = (EnrollOrderDetailResponse)GetOdrEnrollOrderDetail(orderId, companyId);
            FinOrderHandoverResponse r = new OrderHandoverService(_schoolId).GetFinOrderHandoverDetailByOrderId(orderId, OrderTradeType.EnrollOrder);
            orderDetail.FinOrderHandoverList = r ?? new FinOrderHandoverResponse();

            return orderDetail;
        }

        /// <summary>
        /// 获取订单详情
        /// <para>作     者：Huang GaoLiang </para>
        /// <para>创建时间:2018-11-10 </para>
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>返回订单详情信息</returns>
        /// <exception cref="BussinessException">
        /// 异常ID：7,数据异常
        /// </exception>
        private IOrderDetailResponse GetOdrEnrollOrderDetail(long orderId, string companyId)
        {
            // 根据订单编号查询订单信息
            TblOdrEnrollOrder order = _enrollOrderRepository.Value.GetOrderById(orderId);
            if (order == null)
            {
                throw new BussinessException((byte)ModelType.SignUp, 7);
            }
            EnrollOrderDetailResponse orderDetail = new EnrollOrderDetailResponse();

            //1、报名订单
            orderDetail.EnrollOrderId = order.EnrollOrderId;
            orderDetail.StudentId = order.StudentId;
            orderDetail.OrderNo = order.OrderNo;
            orderDetail.CreateTime = order.CreateTime;
            orderDetail.CreateName = order.CreateName;
            orderDetail.PayType = order.PayType;
            orderDetail.PayTypeName = EnumName.GetDescription(typeof(PayType), order.PayType);
            orderDetail.OrderNewTypeName = EnumName.GetDescription(typeof(OrderNewType), order.OrderNewType);
            orderDetail.OrderStatus = order.OrderStatus;
            orderDetail.OrderStatuName = EnumName.GetDescription(typeof(OrderStatus), order.OrderStatus);
            orderDetail.Remark = order.Remark;
            orderDetail.UseBalance = order.UseBalance;
            orderDetail.TotalClassTimes = order.TotalClassTimes;
            orderDetail.TotalTuitionFee = order.TotalTuitionFee;
            orderDetail.TotalMaterialFee = order.TotalMaterialFee;
            orderDetail.PayAmount = order.PayAmount;
            orderDetail.TotalTradeAmount = order.TotalTradeAmount;
            orderDetail.SchoolId = order.SchoolId;
            orderDetail.CancelUserId = order.CancelUserId;
            orderDetail.CancelUserName = order.CancelUserName;
            orderDetail.CancelDate = order.CancelDate;
            orderDetail.CancelRemark = order.CancelRemark;


            // 2、业绩归属人
            List<TblOdrEnrollOrderAchieve> achieves = new TblOdrEnrollOrderAchieveRepository().LoadList(m => m.EnrollOrderId == orderId);

            // 2 业绩人名称
            List<Anticorrosion.HRS.EmployeeResponse> employList = EmployeeService.GetPerformanceOwnerBySchoolId(this._schoolId);

            // 3、组合数据
            List<AMS.Dto.EmployeeResponse> employeeList = (from a in achieves
                                                           join e in employList on a.PersonalId equals e.EmployeeId
                                                           select new AMS.Dto.EmployeeResponse
                                                           {
                                                               EmployeeId = a.PersonalId,
                                                               EmployeeName = e.EmployeeName,
                                                               Proportion = a.Proportion
                                                           }).ToList();

            orderDetail.PersonalInfo = employeeList;

            // 4、报名订单课程明细
            this.SetOrderItem(orderId, orderDetail, companyId);

            // 5、订单使用的优惠券
            List<TblOdrEnrollOrderDiscount> discount = new TblOdrEnrollOrderDiscountRepository().LoadList(m => m.EnrollOrderId == orderId);
            orderDetail.Scholarship = discount.Where(m => m.CouponType == (int)CouponType.FullReduce).Sum(m => m.CouponAmount); //满减
            orderDetail.ScholarshipVoucher = discount.Where(m => m.CouponType != (int)CouponType.FullReduce).Sum(m => m.CouponAmount);//转介绍+校长奖学金

            // 6、根据学生编号获取学生信息
            StudentDetailResponse s = new StudentService(_schoolId).GetStudent(orderDetail.StudentId);
            if (s == null)
            {
                throw new BussinessException((byte)ModelType.SignUp, 7);
            }

            orderDetail.StudentId = s.StudentId;
            orderDetail.StudentNo = s.StudentNo;
            orderDetail.StudentName = s.StudentName;
            orderDetail.HeadFaceUrl = s.HeadFaceUrl;
            orderDetail.Sex = s.Sex;
            orderDetail.SexName = s.SexName;
            orderDetail.StudentNo = s.StudentNo;
            orderDetail.Age = s.Age;
            orderDetail.Birthday = s.Birthday;
            orderDetail.IDNumber = s.IDNumber;
            orderDetail.IDType = s.IDType;
            orderDetail.IdTypeName = s.IdTypeName;
            orderDetail.LinkMail = s.LinkMail;
            orderDetail.LinkMobile = s.LinkMobile;
            orderDetail.ContactPerson = s.ContactPerson;
            orderDetail.SchoolName = new OrgService().GetAllSchoolList().FirstOrDefault(m => m.SchoolId == orderDetail.SchoolId)?.SchoolName;

            return orderDetail;
        }


        /// <summary>
        /// 获取报名订单详情
        /// <para>作     者：Huang GaoLiang </para>
        /// <para>创建时间:2018-11-08 </para>
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="result">详情信息</param>
        /// <param name="companyId">公司编号</param>
        private void SetOrderItem(long orderId, EnrollOrderDetailResponse result, string companyId)
        {
            List<TblOdrEnrollOrderItem> orderItemItem = _enrollOrderItemRepository.Value.LoadList(m => m.EnrollOrderId == orderId);

            // 1、获取学期
            List<int> years = orderItemItem.Select(m => m.Year).Distinct().ToList();
            List<long> termTypeIds = orderItemItem.Select(m => m.TermTypeId).ToList();
            List<TblDatTerm> datTermList = TermService.GetSchoolIdTermList(_schoolId)
                .Where(m => termTypeIds.Contains(m.TermTypeId)
                            && years.Contains(m.Year))
                .ToList();
            // 学期类型相同的要过滤
            datTermList = datTermList.Where((x, i) => datTermList.FindIndex(z => z.TermTypeId == x.TermTypeId) == i)
                .ToList();

            // 2、获取学期类型名称
            List<TermTypeResponse> termTypeList = new TermTypeService().GetAll();

            // 3、课程信息
            List<TblDatCourse> courseList = CourseService.GetAllAsync().Result;

            // 4、获取等级
            List<CourseLevelResponse> courseLeave = new CourseLevelService(companyId).GetList().Result;

            // 5、组合数据
            List<EnrollOrderItemResponse> orderItemList = this.CombinOrderItemList(result, orderItemItem, datTermList, courseList, courseLeave, termTypeList);

            // 6、统计课次
            foreach (var item in orderItemList)
            {
                item.ClassesCount = orderItemList.Where(m => m.TermTypeId == item.TermTypeId && m.CourseType == item.CourseType && m.Year == item.Year).Sum(m => m.ClassTimes);
            }
        }

        /// <summary>
        /// 组合报名单课程明细数据
        /// <para>作     者：Huang GaoLiang </para>
        /// <para>创建时间:2018-11-23 </para>
        /// </summary>
        /// <param name="result">返回数据集</param>
        /// <param name="orderItemItem">报名单课程明细</param>
        /// <param name="datTermList">学期集合</param>
        /// <param name="courseList">课程集合</param>
        /// <param name="courseLeave">课程等级集合</param>
        /// <param name="termTypeList">学期类型</param>
        /// <returns>返回订单明细数据</returns>
        private List<EnrollOrderItemResponse> CombinOrderItemList(EnrollOrderDetailResponse result, List<TblOdrEnrollOrderItem> orderItemItem, List<TblDatTerm> datTermList,
            List<TblDatCourse> courseList, List<CourseLevelResponse> courseLeave, List<TermTypeResponse> termTypeList)
        {
            List<EnrollOrderItemResponse> orderItemList = (from i in orderItemItem
                                                           join t in datTermList on new { i.TermTypeId, i.Year } equals new { t.TermTypeId, t.Year } into term
                                                           from tt in term.DefaultIfEmpty()

                                                           join c in courseList on i.CourseId equals c.CourseId into cour
                                                           from cc in cour.DefaultIfEmpty()

                                                           join l in courseLeave on i.CourseLevelId equals l.CourseLevelId into courLev
                                                           from ll in courLev.DefaultIfEmpty()

                                                           select new EnrollOrderItemResponse
                                                           {
                                                               EnrollOrderItemId = i.EnrollOrderItemId,
                                                               TermTypeId = i.TermTypeId,
                                                               TermName = tt == null ? "" : tt.TermName,
                                                               TermTypeName = GetTermTypeNameById(i.TermTypeId, termTypeList),
                                                               ClassesCount = i.ClassTimes,
                                                               Year = i.Year,
                                                               CourseId = i.CourseId,
                                                               CourseName = cc == null ? "" : cc.CourseCnName,
                                                               ClassTimes = i.ClassTimes,
                                                               CourseLevelId = i.CourseLevelId,
                                                               CourseLeveName = ll == null ? "" : ll.LevelCnName,
                                                               CourseType = i.CourseType,
                                                               CourseTypeName = EnumName.GetDescription(typeof(EnrollOrderCouseType), i.CourseType),
                                                           }
                                                         ).OrderBy(m => m.Year).ToList();
            List<OrderYear> glist = orderItemList.GroupBy(m => m.Year).Select(m => new OrderYear
            {
                Year = m.Key,
                OrderItemList = m.ToList()
            }).ToList();
            result.YearList = glist;
            return orderItemList;
        }

        /// <summary>
        /// 根据学期类型编号获取学期信息
        /// <para>作     者：Huang GaoLiang </para>
        /// <para>创建时间:2018-11-23 </para>
        /// </summary>
        /// <param name="termTypeId">学期类型编号</param>
        /// <param name="termTypeList">学期集合</param>
        /// <returns>返回学期信息</returns>
        private static string GetTermTypeNameById(long termTypeId, List<TermTypeResponse> termTypeList)
        {
            return termTypeList.FirstOrDefault(m => m.TermTypeId == termTypeId)?.TermTypeName;
        }
        #endregion

        #region GetPrintOrderDetail 打印收据
        /// <summary>
        /// 打印收据 
        /// <para>作     者：Huang GaoLiang </para>
        /// <para>创建时间:2018-11-01 </para>
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>返回订单详情</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：7,数据异常
        /// </exception>
        public EnrollOrderDetailResponse GetPrintOrderDetail(long orderId, string companyId)
        {
            // 根据订单编号查询订单信息
            EnrollOrderDetailResponse detail = (EnrollOrderDetailResponse)GetOdrEnrollOrderDetail(orderId, companyId);
            if (detail == null)
            {
                throw new BussinessException((byte)ModelType.SignUp, 7);
            }
            detail.SchoolName = new OrgService().GetAllSchoolList().FirstOrDefault(m => m.SchoolId == detail.SchoolId)?.SchoolName;
            detail.PrintNumber = PrintCounterService.Print(_schoolId, PrintBillType.SingUp);
            return detail;
        }

        #endregion

        #region GetPrintEnrollOrder 打印报名表
        /// <summary>
        /// 打印报名表
        /// <para>作     者：Huang GaoLiang </para>
        /// <para>创建时间:2018-11-09 </para>
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>返回订单详情</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：7,数据异常
        /// </exception>
        public EnrollPrintOrderDetailResponse GetPrintEnrollOrder(long orderId, string companyId)
        {
            // 根据订单编号查询订单信息
            EnrollOrderDetailResponse orderInfo = (EnrollOrderDetailResponse)GetOdrEnrollOrderDetail(orderId, companyId);
            if (orderInfo == null)
            {
                throw new BussinessException((byte)ModelType.SignUp, 7);
            }
            EnrollPrintOrderDetailResponse detail = Mapper.Map<EnrollPrintOrderDetailResponse>(orderInfo);

            //上课时间
            List<long> orderItemIds = new List<long>();
            foreach (var item in orderInfo.YearList)
            {
                orderItemIds.AddRange(item.OrderItemList.Select(m => m.EnrollOrderItemId));
            }

            orderItemIds = orderItemIds.Distinct().ToList();
            detail.ContactPersonList = orderInfo.ContactPerson;
            detail.MakeLessonList = MakeLessonService.GetConfirmedMakeLesson(orderItemIds, companyId);
            detail.PrintNumber = PrintCounterService.Print(_schoolId, PrintBillType.SingUp);
            return detail;
        }

        #endregion

        #region GetEnrollOrderMonthCancelList 获取本月要取消订单作废的订单列表

        /// <summary>
        /// 获取本月要取消订单作废的订单列表
        ///  <para>作     者：Huang GaoLiang </para>
        /// <para>创建时间:2018-11-09 </para>
        /// </summary>
        /// <param name="stuName">订单编号</param>
        /// <param name="schoolId">校区编号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>返回月作废分页列表数据</returns>
        public PageResult<EnrollOrderMonthCancelListResponse> GetEnrollOrderMonthCancelList(string stuName, string schoolId, int pageIndex, int pageSize)
        {
            PageResult<ViewOrderObsoleteMonth> orders = new ViewOrderObsoleteMonthResponse().GetEnrollOrderPageList(stuName, schoolId, pageIndex, pageSize);
            PageResult<EnrollOrderMonthCancelListResponse> list = Mapper.Map<PageResult<ViewOrderObsoleteMonth>, PageResult<EnrollOrderMonthCancelListResponse>>(orders);
            foreach (var item in list.Data)
            {
                // 如果使用余额小于或者等于0,并且支付方式为其他
                if (item.UseBalance > 0 && item.PayTypeId == (int)PayType.Other && item.PayAmount <= 0)
                {
                    item.PayType = "余额";
                }
                // 如果余额大于O，并且支付方式大于0
                else if (item.UseBalance > 0 && item.PayTypeId > 0)
                {
                    item.PayType = $"余额、{item.PayType}";
                }
            }
            return list;
        }
        #endregion

        #region Cancel 作废报名订单

        /// <summary>
        /// 作废报名订单－－当天作废
        /// <para>作     者：Huang GaoLiang </para>
        /// <para>创建时间:2018-11-05 </para>
        /// </summary>
        /// <param name="enrollOrderId">订单编号</param>
        /// <param name="userId">操作人</param>
        /// <param name="cancelUserName">操作人名称</param>
        /// <param name="cancelRemark">作废备注</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：7,数据异常
        /// </exception>
        public void CancelToday(long enrollOrderId, string userId, string cancelUserName, string cancelRemark)
        {
            // 根据订单编号查询订单信息
            TblOdrEnrollOrder order = new TblOdrEnrollOrderRepository().Load(m => m.EnrollOrderId == enrollOrderId);
            if (order == null)
            {
                throw new BussinessException((byte)ModelType.SignUp, 7);
            }

            // 作废
            Cancel(order, userId, cancelUserName, cancelRemark, "Day");
        }

        /// <summary>
        /// 作废报名订单－－当月作废
        /// <para>作     者：Huang GaoLiang </para>
        /// <para>创建时间:2018-11-02 </para>
        /// </summary>
        /// <param name="enrollOrderId">订单编号</param>
        /// <param name="userId">操作人</param>
        /// <param name="cancelUserName">作废操作人名称</param>
        /// <param name="cancelRemark">作废备注呢</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：7,数据异常
        /// </exception>
        public void CancelCurrentMonth(long enrollOrderId, string userId, string cancelUserName, string cancelRemark)
        {
            TblOdrEnrollOrder order = new TblOdrEnrollOrderRepository().Load(m => m.EnrollOrderId == enrollOrderId);
            if (order == null)
            {
                throw new BussinessException((byte)ModelType.SignUp, 7);
            }

            // 作废
            Cancel(order, userId, cancelUserName, cancelRemark, "Month");
        }

        /// <summary>
        /// 作废校验
        /// <para>作     者：Huang GaoLiang </para>
        /// <para>创建时间:2018-11-08 </para>
        /// </summary>
        /// <param name="order">订单信息</param>
        /// <param name="dayOrMonth">当天或者当月</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：18,已作废的订单不能再作废
        /// 异常ID：19,已完成的订单不能再作废
        /// 异常ID：20,已退费的订单不能再作废
        /// 异常ID：21,已休学的订单不能再作废
        /// 异常ID：10,只能当天作废
        /// 异常ID：11,只能当月作废
        /// </exception>
        private void CancelCheck(TblOdrEnrollOrder order, string dayOrMonth)
        {
            if (order.OrderStatus == (int)OrderStatus.Cancel)
            {
                throw new BussinessException((byte)ModelType.SignUp, 18);
            }
            if (order.OrderStatus == (int)OrderStatus.Finish)
            {
                throw new BussinessException((byte)ModelType.SignUp, 19);
            }

            List<int> list = new TblOdrEnrollOrderItemRepository().LoadList(m => m.EnrollOrderId == order.EnrollOrderId).Select(m => m.Status).ToList();

            if (list.Any(m => m == (int)OrderItemStatus.Cancel))
            {
                throw new BussinessException((byte)ModelType.SignUp, 18);
            }

            if (list.Any(m => m == (int)OrderItemStatus.LeaveClass))
            {
                throw new BussinessException((byte)ModelType.SignUp, 20);
            }
            if (list.Any(m => m == (int)OrderItemStatus.LeaveSchool))
            {
                throw new BussinessException((byte)ModelType.SignUp, 21);
            }
            if (list.Any(m => m == (int)OrderItemStatus.ChangeSchool))
            {
                throw new BussinessException((byte)ModelType.SignUp, 22);
            }

            if (dayOrMonth == "Day" && order.CreateTime.ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd"))
            {
                throw new BussinessException((byte)ModelType.SignUp, 10);
            }

            if (dayOrMonth == "Month" && order.CreateTime.Year == DateTime.Now.Year && order.CreateTime.Month != DateTime.Now.Month)
            {
                throw new BussinessException((byte)ModelType.SignUp, 11);
            }
        }

        /// <summary>
        /// 作废报名订单
        /// <para>作     者：Huang GaoLiang </para>
        /// <para>创建时间:2018-11-01 </para>
        /// </summary>
        /// <param name="order">订单编号</param>
        /// <param name="userId">操作人</param>
        /// <param name="cancelUserName">操作人名称</param>
        /// <param name="cancelRemark">作废备注</param>
        /// <param name="dayOrMonth">天/月</param>   
        private void Cancel(TblOdrEnrollOrder order, string userId, string cancelUserName, string cancelRemark, string dayOrMonth)
        {
            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_AMSSCHOOLSTUDENT, this._schoolId, order.StudentId.ToString()))
            {
                this.CancelCheck(order, dayOrMonth);

                List<long> enrollOrderItemIds = new TblOdrEnrollOrderItemRepository().GetByEnrollOrderId(order.EnrollOrderId).Result.Select(m => m.EnrollOrderItemId).ToList();

                List<long> couponIds = new TblOdrEnrollOrderDiscountRepository().LoadList(m => m.EnrollOrderId == order.EnrollOrderId).Select(m => m.CouponId).ToList();//获取使用优惠券的id

                order.OrderStatus = (int)OrderStatus.Cancel;
                EnrollOrderTrade orderTrade = new EnrollOrderTrade(order);

                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.BeginTransaction();

                        // 1、报名订单作废
                        this.CancelOrder(order.EnrollOrderId, userId, cancelUserName, cancelRemark, unitOfWork);

                        // 2、更新报名订单课程明细
                        unitOfWork.GetCustomRepository<TblOdrEnrollOrderItemRepository, TblOdrEnrollOrderItem>().UpdateEnrollOrderItemStatus(enrollOrderItemIds, OrderItemStatus.Cancel).Wait();

                        // 3、优惠返回
                        CouponRuleEnrollProducer.RefundCoupon(couponIds, order.StudentId, unitOfWork);

                        // 4、排课表返回
                        MakeLessonService.Cancel(order.EnrollOrderId, unitOfWork);

                        // 5、学生状态更改
                        new StudentService(_schoolId).UpdateStudentStatusById(order.StudentId, unitOfWork);

                        // 6、资金交易返回
                        TradeService tradeService = new TradeService(orderTrade, unitOfWork);
                        tradeService.Invalid();

                        // 7、收款交接 
                        new OrderHandoverService(_schoolId).DeleteHandleOver(order.EnrollOrderId, OrderTradeType.EnrollOrder, unitOfWork);

                        unitOfWork.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        unitOfWork.RollbackTransaction();
                        throw ex;
                    }
                }

            }
        }

        /// <summary>
        /// 作废报名单
        /// <para>作     者：Huang GaoLiang </para>
        /// <para>创建时间:2018-11-15 </para>
        /// </summary>
        /// <param name="enrollOrderId">报名订单编号</param>
        /// <param name="userId">作废人编号</param>
        /// <param name="cancelUserName">作废人名称</param>
        /// <param name="cancelRemark">作废人名称</param>
        /// <param name="unitOfWork">事务</param>
        /// <exception cref = "BussinessException" >
        /// 异常ID：7,数据异常
        /// </exception>
        private void CancelOrder(long enrollOrderId, string userId, string cancelUserName, string cancelRemark, UnitOfWork unitOfWork)
        {

            TblOdrEnrollOrder enrollOrder = unitOfWork.GetCustomRepository<TblOdrEnrollOrderRepository, TblOdrEnrollOrder>().Load(m => m.EnrollOrderId == enrollOrderId);
            if (enrollOrder == null)
            {
                throw new BussinessException((byte)ModelType.SignUp, 7);
            }
            enrollOrder.OrderStatus = (int)OrderStatus.Cancel;
            enrollOrder.CancelDate = DateTime.Now;
            enrollOrder.CancelUserId = userId;
            enrollOrder.CancelUserName = cancelUserName;
            enrollOrder.CancelRemark = cancelRemark;

            unitOfWork.GetCustomRepository<TblOdrEnrollOrderRepository, TblOdrEnrollOrder>().Update(enrollOrder);
        }

        #endregion

        #region AddClassTimesUse 增加或减少已排课次
        /// <summary>
        /// 增加或减少已排课次
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-08</para>
        /// </summary>
        /// <param name="enrollOrderItemId">报名课程订单Id</param>
        /// <param name="addClassTime">增加或减少的课次</param>
        /// <param name="unitOfWork">事物单元</param>
        internal void AddClassTimesUse(long enrollOrderItemId, int addClassTime, UnitOfWork unitOfWork)
        {
            TblOdrEnrollOrderItemRepository enrollOrderItemRepository =
                unitOfWork.GetCustomRepository<TblOdrEnrollOrderItemRepository, TblOdrEnrollOrderItem>();

            TblOdrEnrollOrderItem enrollOrderItem = enrollOrderItemRepository.Load(enrollOrderItemId);
            enrollOrderItem.ClassTimesUse += addClassTime;
            enrollOrderItemRepository.Update(enrollOrderItem);
        }
        #endregion

        #region ChangeToLeaveSchool 休学
        /// <summary>
        /// 描述：修改报名订单课程明细表状态为休学
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        internal async Task ChangeToLeaveSchool(List<long> enrollOrderItemId, UnitOfWork unitOfWork)
        {
            var enrollOrderItemRepository = unitOfWork.GetCustomRepository<TblOdrEnrollOrderItemRepository, TblOdrEnrollOrderItem>();

            await enrollOrderItemRepository.UpdateEnrollOrderItemStatus(enrollOrderItemId, OrderItemStatus.LeaveSchool);
        }
        #endregion

        #region ChangeToChangeSchool 转校
        /// <summary>
        /// 转校
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-12-10</para>
        /// </summary>
        internal void ChangeToChangeSchool(long enrollOrderItemId, UnitOfWork unitOfWork)
        {
            var enrollOrderItemRepository = unitOfWork.GetCustomRepository<TblOdrEnrollOrderItemRepository, TblOdrEnrollOrderItem>();

            var orderItem = enrollOrderItemRepository.Load(enrollOrderItemId);
            orderItem.Status = (int)OrderItemStatus.ChangeSchool;
            enrollOrderItemRepository.Update(orderItem);
        }
        #endregion

        #region ChangeToLeaveClass 退班
        /// <summary>
        /// 描述：修改报名订单课程明细表状态为退班
        /// <para>作   者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        internal async Task ChangeToLeaveClass(List<long> enrollOrderItemId, UnitOfWork unitOfWork)
        {
            var tblOdrEnrollOrderItemRepository = unitOfWork.GetCustomRepository<TblOdrEnrollOrderItemRepository, TblOdrEnrollOrderItem>();

            await tblOdrEnrollOrderItemRepository.UpdateEnrollOrderItemStatus(enrollOrderItemId, OrderItemStatus.LeaveClass);
        }
        #endregion

        #region GetStudentEnroOrderItem 获取学生的报名课程明细
        /// <summary>
        /// 获取学生的报名课程明细
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-07</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <param name="time">转校、转班、休学时间</param>
        /// <returns>返回报名信息数据</returns>
        internal List<EnrollOrderResponse> GetStudentEnroOrderItem(long studentId, DateTime time)
        {
            List<EnrollOrderResponse> result = new List<EnrollOrderResponse>();

            // 获取当前时间之后的学期数据
            List<TblDatTerm> terms = TermService.GetFutureTerm(_schoolId, time)
                .Distinct(new Compare<TblDatTerm>(
                    (x, y) =>
                    (x != null && y != null && x.TermTypeId == y.TermTypeId && x.Year == y.Year)))
                .ToList();

            // 1、根据学生编号和校区编号获取学生的报名信息
            var orderStatusList = new List<int> { (int)OrderStatus.Paid, (int)OrderStatus.Finish };
            List<long> enrollOrderIds = _enrollOrderRepository
                .Value.GetStudentOrderList(_schoolId, studentId, orderStatusList)
                .Result
                //.Where(x => x.OrderStatus == (int)OrderStatus.Paid || x.OrderStatus == (int)OrderStatus.Finish)
                .Select(m => m.EnrollOrderId)
                .Distinct()
                .ToList();

            // 2、根据报名编号报名订单课程明细
            List<TblOdrEnrollOrderItem> orderItems = _enrollOrderItemRepository
                .Value.GetByEnrollOrderId(enrollOrderIds)
                .Result
                .Where(m => m.Status == (int)OrderItemStatus.Enroll)
                .ToList();

            // 3.关联过滤已过学期
            orderItems = (from a in orderItems
                          join b in terms
                          on new { a.TermTypeId, a.Year } equals new { b.TermTypeId, b.Year }
                          select a).ToList();

            if (orderItems.Any())
            {
                foreach (var item in orderItems)
                {
                    var res = new EnrollOrderResponse
                    {
                        EnrollOrderItemId = item.EnrollOrderItemId,
                        Year = item.Year,
                        TermTypeId = item.TermTypeId,
                        CourseId = item.CourseId,
                        CourseLevelId = item.CourseLevelId,
                        ClassTimes = item.ClassTimes,
                        //实收金额=(学费单价+杂费单价)*报名课次
                        PayAmount = (item.TuitionFeeReal + item.MaterialFeeReal) * item.ClassTimes,
                        TuitionFee = item.TuitionFee,
                        MaterialFee = item.MaterialFee,
                        DiscountFee = item.DiscountFee,
                        TuitionFeeReal = item.TuitionFeeReal,
                        MaterialFeeReal = item.MaterialFeeReal,
                        Status = (OrderItemStatus)item.Status,
                        ClassTimesUse = item.ClassTimesUse
                    };
                    result.Add(res);
                }
            }

            return result;
        }
        #endregion

        #region GetEnrollOrderItemIdByEnroOrderList 根据报名订单课程明细Id获取报名订单课程信息
        /// <summary>
        /// 根据报名订单课程明细Id获取报名订单课程信息
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        /// <param name="enrollOrderItemId">报名订单课程明细Id</param>
        /// <returns>报名订单课程列表</returns>
        internal static List<EnrollOrderResponse> GetEnrollOrderItemIdByEnroOrderList(IEnumerable<long> enrollOrderItemId)
        {
            var query = new TblOdrEnrollOrderItemRepository().GetByEnrollOrderItemId(enrollOrderItemId).Result.Select(x => new EnrollOrderResponse
            {
                EnrollOrderItemId = x.EnrollOrderItemId,
                Year = x.Year,
                TermTypeId = x.TermTypeId,
                CourseId = x.CourseId,
                CourseLevelId = x.CourseLevelId,
                ClassTimes = x.ClassTimes,
                PayAmount = (x.TuitionFeeReal + x.MaterialFeeReal) * x.ClassTimes,
                TuitionFee = x.TuitionFee,
                MaterialFee = x.MaterialFee,
                DiscountFee = x.DiscountFee,
                Status = (OrderItemStatus)x.Status,
                ClassTimesUse = x.ClassTimesUse
            }).ToList();

            return query;
        }

        /// <summary>
        /// 根据报名订单课程明细Id列表，获取报名订单课程明细信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="enrollOrderItemIdList">报名订单课程明细Id列表</param>
        /// <returns>报名订单课程明细列表</returns>
        internal static List<TblOdrEnrollOrderItem> GetEnrollOrderItemByItemId(IEnumerable<long> enrollOrderItemIdList)
        {
            return new TblOdrEnrollOrderItemRepository().GetByEnrollOrderItemId(enrollOrderItemIdList).Result;
        }

        public override Task Cancel(long orderId, string cancelUserId, string cancelUserName, string cancelRemark)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region GetStudentOrders 获取一个学生的订单集合
        /// <summary>
        /// 获取一个学生的订单集合
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <returns></returns>
        internal async Task<List<TblOdrEnrollOrder>> GetStudentOrders(long studentId)
        {
            //这个校区学生所有的订单
            return await _enrollOrderRepository.Value.GetStudentOrders(_schoolId, studentId);
        }
        #endregion


        #region GetEnrollOrderItemById 根据订单明细编号获取订单明细
        /// <summary>
        /// 根据报名订单Id获取报名的课程
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-20</para>
        /// </summary>
        /// <param name="enrollOrderItemId">订单明细编号</param>
        /// <returns>返回订单明细</returns>
        internal TblOdrEnrollOrderItem GetEnrollOrderItemById(long enrollOrderItemId)
        {
            return _enrollOrderItemRepository.Value.Load(enrollOrderItemId);
        }
        #endregion
    }
}
