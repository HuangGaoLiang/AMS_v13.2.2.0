using AMS.Core;
using AMS.Dto;
using AMS.Dto.Enum;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using Jerrisoft.Platform.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Service
{
    /// <summary>
    /// 描述：报名优惠产生者
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-2</para>
    /// </summary>
    public class CouponRuleEnrollProducer : BService
    {
        private readonly string _schoolId;                                            //校区Id
        private readonly decimal _totalAmount;                                        //总的交易金额
        private readonly Lazy<TblDctCouponRuleRepository> _tblDctCouponRuleRepository;//赠与奖学金列表
        private bool _IsFreeAll = false;                                              //是否全免

        /// <summary>
        /// 描述：实例化一个报名优惠产生者对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-2</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="totalAmount">总金额</param>
        public CouponRuleEnrollProducer(string schoolId, decimal totalAmount)
        {
            _schoolId = schoolId;
            _totalAmount = totalAmount;
            _tblDctCouponRuleRepository = new Lazy<TblDctCouponRuleRepository>();
        }

        /// <summary>
        /// 描述：预览生成优惠数据（满减金额+转介绍）
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-2</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <returns>优惠信息集合</returns>
           
        public async Task<List<CouponGenerateResponse>> Peek(long studentId)
        {
            var result = new List<CouponGenerateResponse>();

            //1.满减信息
            var resultFullReduce = new CouponGenerateResponse();
            //获取赠与奖学金金额
            var query = (await _tblDctCouponRuleRepository.Value.GetAmountByCouponRule(_schoolId, _totalAmount)).FirstOrDefault();
            resultFullReduce.CouponRuleId = query?.CouponRuleId ?? 0;
            resultFullReduce.CouponRuleAmount = query?.CouponAmount ?? 0;
            resultFullReduce.CouponType = CouponType.FullReduce;
            result.Add(resultFullReduce);

            //2.转介绍信息
            //获取学生是否有转介绍Id
            var studentService = new StudentService(_schoolId);
            var studenInfo = studentService.GetStudent(studentId);
            //获取学生是否有报名信息
            var enrollOrderService = new EnrollOrderService(_schoolId);
            var studentOrder = enrollOrderService.GetStudentOrders(studentId).Result;

            if (!studentOrder.Any()) //没有报过名，并且有转介绍人 才返回转介绍信息
            {
                if (studenInfo.ParentId <= 0) return result;

                var couponRuleService = new CouponRuleService(_schoolId);
                var couponRuleResult = couponRuleService.GetTblDctCouponRuleInfo();

                var resultRecommend = new CouponGenerateResponse
                {
                    CouponRuleId = couponRuleResult?.CouponRuleId ?? 0,
                    CouponRuleAmount = couponRuleResult?.CouponAmount ?? 0,
                    CouponType = CouponType.Recommend
                };
                result.Add(resultRecommend);
            }
            return result;
        }



        /// <summary>
        /// 描述：生成优惠数据并占用优惠名额.生成优惠券(转介绍)
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-2</para>
        /// </summary>
        /// <param name="couponIdList">优惠券主键Id，没有可用的优惠券集合为空</param>
        /// <param name="enrollStudentId">报名学生</param>
        /// <param name="recommendStudentId">被介绍人（没有转介绍时为空）</param>
        /// <param name="recommendCouponRuleId">转介绍的规则Id（没有勾选时传空）</param>
        /// <param name="fullCouponRulId">满减规则Id</param>
        /// <param name="enrollOrderId">订单表主键</param>
        /// <param name="unitOfWork">事务</param>
        /// <returns>使用的优惠券集合</returns>
           
        public List<CouponGenerateResponse> Create(List<long> couponIdList, long enrollStudentId, long? recommendStudentId, long? recommendCouponRuleId, long? fullCouponRulId, long enrollOrderId, UnitOfWork unitOfWork)
        {
            TblDctCouponRepository tblDctCouponRepository = unitOfWork.GetCustomRepository<TblDctCouponRepository, TblDctCoupon>();
            TblDctCouponRuleRepository tblDctCouponRuleRepository = unitOfWork.GetCustomRepository<TblDctCouponRuleRepository, TblDctCouponRule>();

            //1.报名时先使用优惠券，并修改优惠券状态
            var result = UseCoupon(couponIdList, enrollOrderId, tblDctCouponRepository);

            //2.添加转介绍优惠券，并修改优惠券的状态 
            result = OperationRecommendCoupon(result, enrollStudentId, recommendStudentId, recommendCouponRuleId, enrollOrderId, tblDctCouponRepository, tblDctCouponRuleRepository);

            #region  暂时不需要用到，防止变更先注释
            ////筛选出转介绍的优惠券
            //var recommendList = result.Where(x => x.CouponType == CouponType.Recommend).Select(x => x.CouponId);
            ////根据优惠券Id找到赠与奖学金表Id,并修改使用人数和状态
            //var couponRuleIdList = (await tblDctCouponRepository.GetCouponInfo(recommendList)).Select(x => x.FromId);

            ////如果有转介绍的名额，则修改使用人数, 使用人数+1,
            ////修改赠与奖学金表名额(转介绍)
            //await tblDctCouponRuleRepository.UpdateUseQuotaAsync(couponRuleIdList, 1);

            //2.添加转介绍优惠券，并修改优惠券的状态  
            //if (recommendStudentId.HasValue)   //介绍人不为空时才有转介绍优惠
            //{
            //    var couponRuleService = new CouponRuleService(this._schoolId);
            //    TblDctCouponRule recommendRuleModel;
            //    if (recommendCouponRuleId.HasValue)  //如果报名页面勾选转介绍 ，则根据id查询转介绍信息
            //    {
            //        recommendRuleModel = couponRuleService.GetCouponRuleIdByRuleList(recommendCouponRuleId.Value);
            //        if (recommendRuleModel == null)
            //        {
            //            throw new BussinessException(ModelType.Discount, 15);
            //        }
            //    }
            //    else //如果报名页面没有勾选转介绍 ，则获取本校区转介绍最新优惠信息
            //    {
            //        recommendRuleModel = couponRuleService.GetTblDctCouponRuleInfo();
            //    }

            //    if (recommendRuleModel != null)  //为空代表没有转介绍优惠,有转介绍的话则添加两张优惠券
            //    {
            //        var enrollStuCouponId = await AddCouponInfo(recommendRuleModel, enrollStudentId, CouponType.Recommend, enrollOrderId, Dto.Enum.CouponStatus.NoUse, unitOfWork);  //同时添加2张优惠券
            //        await AddCouponInfo(recommendRuleModel, recommendStudentId.Value, CouponType.Recommend, enrollOrderId, Dto.Enum.CouponStatus.NoUse, unitOfWork);

            //        recommendRuleModel.UseQuota = recommendRuleModel.UseQuota + 2;
            //        await tblDctCouponRuleRepository.UpdateTask(recommendRuleModel);  //修改使用人数

            //        if (recommendCouponRuleId.HasValue)   //转介绍规则Id不为空，则使用该优惠券
            //        {
            //            await tblDctCouponRepository.UpdateCouponAsync(enrollStuCouponId, Dto.Enum.CouponStatus.HasUse, DateTime.Now, null);  //将当前报名学生的优惠券使用掉 修改优惠券状态

            //            var recommendRuleEntity = new CouponGenerateResponse
            //            {
            //                CouponId = enrollStuCouponId,
            //                CouponRuleAmount = recommendRuleModel.CouponAmount,
            //                CouponType = CouponType.Recommend
            //            };
            //            result.Add(recommendRuleEntity);
            //        }
            //    }
            //}
            //3.使用满减优惠，并添加优惠券表数据
            //if (_totalAmount > 0)   //使用满减优惠
            //{
            //    var query = (await tblDctCouponRuleRepository.GetAmountByCouponRule(this._schoolId, this._totalAmount)).FirstOrDefault();
            //    if (query != null)
            //    {
            //        //添加优惠券并修改使用人数
            //        var fullReduceId = await this.AddCouponInfo(query, enrollStudentId, CouponType.FullReduce, enrollOrderId, Dto.Enum.CouponStatus.HasUse, unitOfWork);   //添加满减优惠券

            //        query.UseQuota = query.UseQuota + 1;
            //        await tblDctCouponRuleRepository.UpdateTask(query);  //修改使用人数

            //        var fullReduceEntity = new CouponGenerateResponse
            //        {
            //            CouponId = fullReduceId,
            //            CouponRuleAmount = query.CouponAmount,
            //            CouponType = CouponType.FullReduce
            //        };
            //        result.Add(fullReduceEntity);
            //    }
            //}
            #endregion

            //3.使用满减优惠，并添加优惠券表数据
            if (_IsFreeAll) return result;   //如果是全免则没有满减优惠
            result =  OperationFullReduceCoupon(fullCouponRulId, result, enrollStudentId, enrollOrderId, tblDctCouponRepository, tblDctCouponRuleRepository);

            return result;
        }

        /// <summary>
        /// 描述：报名时使用优惠券，并修改优惠券状态
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-24</para>
        /// </summary>
        /// <param name="couponIdList">优惠券Id集合</param>
        /// <param name="enrollOrderId">报名订单Id</param>
        /// <param name="tblDctCouponRepository">优惠券仓储</param>
        /// <returns>使用的满减和转介绍的优惠券集合</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：13, 异常描述:全免时不可选择多张优惠券
        /// </exception>
        private List<CouponGenerateResponse> UseCoupon(List<long> couponIdList, long enrollOrderId, TblDctCouponRepository tblDctCouponRepository)
        {
            //此方法其实只有转介绍和校长奖学金两种优惠券进来，满减在另外一个方法
            var result = new List<CouponGenerateResponse>();
            if (!couponIdList.Any()) return result;

            //优惠券ID集合不为空，则使用优惠券
            var couponInfoList = tblDctCouponRepository.GetCouponInfo(couponIdList).Result;

            //有使用全免优惠券时，不可选择多张优惠券
            if (couponInfoList.Count > 1 && couponInfoList.Any(x => x.CouponType == (int)CouponType.HeadmasterBonus && x.IsFreeAll))
            {
                throw new BussinessException(ModelType.Discount, 13);
            }

            var fullReduceOrRecommendList = new List<long>(); //满减或者转介绍优惠券Id集合
            var headmasterBonusList = new List<long>();       //校长奖学金优惠券Id集合
            foreach (var item in couponInfoList)
            {
                switch (item.CouponType)
                {
                    case (int)CouponType.FullReduce:  //满减或者转介绍
                        fullReduceOrRecommendList.Add(item.CouponId);
                        break;
                    case (int)CouponType.Recommend:
                        fullReduceOrRecommendList.Add(item.CouponId);
                        break;
                    //校长奖学金需重新绑定订单Id
                    case (int)CouponType.HeadmasterBonus:
                        if (item.IsFreeAll)  //校长奖学金是全免的时候则减免全部学费
                        {
                            item.Amount = this._totalAmount;
                            _IsFreeAll = true;
                        }
                        headmasterBonusList.Add(item.CouponId);
                        break;
                }
                var entity = new CouponGenerateResponse
                {
                    CouponId = item.CouponId,
                    CouponRuleAmount = item.Amount,    //校长奖学金是全免的时候则减免全部学费
                    CouponType = (CouponType)item.CouponType
                };
                result.Add(entity);
            }

             tblDctCouponRepository.UpdateCouponAsync(fullReduceOrRecommendList, Dto.Enum.CouponStatus.HasUse, DateTime.Now, enrollOrderId).Wait();
             tblDctCouponRepository.UpdateCouponAsync(headmasterBonusList, Dto.Enum.CouponStatus.HasUse, DateTime.Now, enrollOrderId).Wait();  //使用校长奖学金时需要绑定订单Id

            return result;
        }

        /// <summary>
        /// 描述：添加转介绍优惠券，并修改优惠券的状态
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-1-10</para>
        /// </summary>
        /// <param name="useCouponList">已使用的优惠券信息集合</param>
        /// <param name="enrollStudentId">报名学生Id</param>
        /// <param name="recommendStudentId">转介绍人Id（没有转介绍时为空）</param>
        /// <param name="recommendCouponRuleId">转介绍的规则Id（没有勾选时传空）</param>
        /// <param name="enrollOrderId">订单表主键</param>
        /// <param name="tblDctCouponRepository">优惠券表仓储</param>
        /// <param name="tblDctCouponRuleRepository">优惠信息规则表仓储</param>
        /// <returns>已使用的转介绍优惠券集合</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：15, 异常描述:该转介绍奖学金信息不存在
        /// </exception>
        private List<CouponGenerateResponse> OperationRecommendCoupon(List<CouponGenerateResponse> useCouponList, long enrollStudentId, long? recommendStudentId, long? recommendCouponRuleId, long enrollOrderId, TblDctCouponRepository tblDctCouponRepository, TblDctCouponRuleRepository tblDctCouponRuleRepository)
        {
           
            if (recommendStudentId.HasValue && recommendStudentId > 0)   //介绍人不为空时才有转介绍优惠
            {
                var couponRuleService = new CouponRuleService(this._schoolId);
                TblDctCouponRule recommendRuleModel;
                if (recommendCouponRuleId.HasValue && recommendCouponRuleId > 0)  //如果报名页面勾选转介绍 ，则根据id查询转介绍信息
                {
                    recommendRuleModel = couponRuleService.GetCouponRuleIdByRuleList(recommendCouponRuleId.Value);
                    if (recommendRuleModel == null)
                    {
                        throw new BussinessException(ModelType.Discount, 15);
                    }
                }
                else //如果报名页面没有勾选转介绍 ，则获取本校区转介绍最新优惠信息
                {
                    recommendRuleModel = couponRuleService.GetTblDctCouponRuleInfo();
                }

                if (recommendRuleModel != null)  //为空代表没有转介绍优惠,有转介绍的话则添加两张优惠券
                {
                    var enrollStuCouponId =  AddCouponInfo(recommendRuleModel, enrollStudentId, CouponType.Recommend, enrollOrderId, Dto.Enum.CouponStatus.NoUse, tblDctCouponRepository).Result;  //同时添加2张优惠券
                     AddCouponInfo(recommendRuleModel, recommendStudentId.Value, CouponType.Recommend, enrollOrderId, Dto.Enum.CouponStatus.NoUse, tblDctCouponRepository).Wait();

                    recommendRuleModel.UseQuota = recommendRuleModel.UseQuota + 2;
                     tblDctCouponRuleRepository.Update(recommendRuleModel);  //修改使用人数

                    LogWriter.Write(this, "测试转介绍:" + recommendCouponRuleId);
                    if (recommendCouponRuleId.HasValue && recommendCouponRuleId > 0)   //转介绍规则Id不为空，则使用该优惠券
                    {
                        tblDctCouponRepository.UpdateCouponAsync(enrollStuCouponId, Dto.Enum.CouponStatus.HasUse, DateTime.Now, null).Wait();  //将当前报名学生的优惠券使用掉 修改优惠券状态
                        var recommendRuleEntity = new CouponGenerateResponse
                        {
                            CouponId = enrollStuCouponId,
                            CouponRuleAmount = recommendRuleModel.CouponAmount,
                            CouponType = CouponType.Recommend
                        };
                        useCouponList.Add(recommendRuleEntity);
                    }
                }
            }
            return useCouponList;
        }

        /// <summary>
        /// 描述：使用满减优惠，并添加优惠券表数据
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-24</para>
        /// </summary>
        /// <param name="fullCouponRulId">满减规则Id</param>
        /// <param name="useCouponList">已使用的优惠券信息集合</param>
        /// <param name="enrollStudentId">报名学生Id</param>
        /// <param name="enrollOrderId">订单表主键</param>
        /// <param name="tblDctCouponRepository">优惠券表仓储</param>
        /// <param name="tblDctCouponRuleRepository">优惠信息规则表仓储</param>
        /// <returns>使用的满减优惠券的集合</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：16, 异常描述:该满减奖学金信息不存在
        /// </exception>
        private List<CouponGenerateResponse> OperationFullReduceCoupon(long? fullCouponRulId, List<CouponGenerateResponse> useCouponList, long enrollStudentId, long enrollOrderId, TblDctCouponRepository tblDctCouponRepository, TblDctCouponRuleRepository tblDctCouponRuleRepository)
        {
            //使用满减优惠
            if (!fullCouponRulId.HasValue || fullCouponRulId <= 0) { return useCouponList; }
            var query = tblDctCouponRuleRepository.GetCouponRuleIdByRuleList(fullCouponRulId.Value).Result;
            if (query == null)  //如果满减不存在
            {
                throw new BussinessException(ModelType.Discount, 16);
            }
            //优惠券不能过期
            var currentDate = DateTime.Now;
            if (query.IsDisabled == false && ((query.MaxQuota > 0 && query.UseQuota < query.MaxQuota) || query.MaxQuota == 0) && query.BeginDate <= currentDate && currentDate <= query.EndDate.AddDays(1))
            {
                //添加优惠券并修改使用人数
                var fullReduceId = this.AddCouponInfo(query, enrollStudentId, CouponType.FullReduce, enrollOrderId, Dto.Enum.CouponStatus.HasUse, tblDctCouponRepository).Result;   //添加满减优惠券

                query.UseQuota = query.UseQuota + 1;
                tblDctCouponRuleRepository.Update(query);  //修改使用人数

                var fullReduceEntity = new CouponGenerateResponse
                {
                    CouponId = fullReduceId,
                    CouponRuleAmount = query.CouponAmount,
                    CouponType = CouponType.FullReduce
                };
                useCouponList.Add(fullReduceEntity);
            }
            else
            {
                throw new BussinessException(ModelType.Discount, 16);
            }
            return useCouponList;
        }


        /// <summary>
        /// 描述：添加优惠券信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间： 2018-11-2</para>
        /// </summary>
        /// <param name="couponRuleEntity">赠与奖学金信息</param>
        /// <param name="studentId">学生Id</param>
        /// <param name="couponType">优惠券类型</param>
        /// <param name="enrollOrderId">来源订单Id</param>
        /// <param name="couponStatus">优惠券状态</param>
        /// <param name="tblDctCouponRepository">优惠券表仓储</param>
        /// <returns>优惠券Id</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：8, 异常描述:添加优惠券失败
        /// </exception>
        private async Task<long> AddCouponInfo(TblDctCouponRule couponRuleEntity, long studentId, CouponType couponType, long enrollOrderId, Dto.Enum.CouponStatus couponStatus, TblDctCouponRepository tblDctCouponRepository)
        {
            try
            {
                var recommendEntity = new TblDctCoupon
                {
                    CouponId = IdGenerator.NextId(),
                    SchoolId = _schoolId,
                    CouponNo = CreateCouponNo.GetCouponCode(),
                    CouponType = (int)couponType,
                    Amount = couponRuleEntity.CouponAmount,
                    Status = (int)couponStatus,
                    ExpireTime = DateTime.Now.AddMonths(6),
                    StudentId = studentId,
                    UseTime = couponStatus == Dto.Enum.CouponStatus.HasUse ? DateTime.Now : (DateTime?)null,
                    CreateTime = DateTime.Now,
                    FromId = couponRuleEntity.CouponRuleId,
                    EnrollOrderId = enrollOrderId,
                    Remark = couponRuleEntity.Remark,
                    IsFreeAll = false,
                    CreatorId = base.CurrentUserId,
                };
                var flag = await tblDctCouponRepository.AddTask(recommendEntity);
                if (flag)
                {
                    return recommendEntity.CouponId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            throw new BussinessException(ModelType.Discount, 8);
        }

        /// <summary>
        /// 描述：订单作废，退回优惠券
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间： 2018-11-2</para>
        /// </summary>
        /// <param name="couponIds">退回优惠券Id</param>
        /// <param name="signUpStudengId">报名学生Id</param>
        /// <param name="unitOfWork">事务</param>
        /// <returns>无</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：9, 异常描述:修改赠与奖学金使用人数失败
        /// </exception>
        public static void RefundCoupon(IEnumerable<long> couponIds, long signUpStudengId, UnitOfWork unitOfWork)
        {
            TblDctCouponRepository tblDctCouponRepository = unitOfWork.GetCustomRepository<TblDctCouponRepository, TblDctCoupon>();
            TblDctCouponRuleRepository tblDctCouponRuleRepository = unitOfWork.GetCustomRepository<TblDctCouponRuleRepository, TblDctCouponRule>();

            //获取到优惠券的信息
            var couponInfoList = tblDctCouponRepository.GetCouponInfo(couponIds).Result;
            var fullReduceOrRecommendIdList = new List<long>();

            var enrollOrderIdLists = couponInfoList.Select(x => x.EnrollOrderId);
            //根据订单号获取优惠券
            var couponList = tblDctCouponRepository.GetEnrollOrderByCoupon(enrollOrderIdLists.Distinct());

            foreach (var item in couponList)
            {
                if (item.CouponType == (int)CouponType.Recommend && item.StudentId != signUpStudengId && item.Status == (int)Dto.Enum.CouponStatus.HasUse && item.UseTime.HasValue)   //如果已使用则不作废
                    continue;

                if (item.CouponType == (int)CouponType.FullReduce || item.CouponType == (int)CouponType.Recommend)    //满减奖学金或者 转介绍 ,将已发放优惠券改为作废
                {
                    item.Status = (int)Dto.Enum.CouponStatus.Invalid;
                    fullReduceOrRecommendIdList.Add(item.FromId);
                }
                else if (item.CouponType == (int)CouponType.HeadmasterBonus)  //校长奖学金,将已发放优惠券改为未使用
                {
                    item.Status = (int)Dto.Enum.CouponStatus.NoUse;
                }
                tblDctCouponRepository.Update(item);
            }

            if (fullReduceOrRecommendIdList.Any())
            {
                var flag = tblDctCouponRuleRepository.UpdateUseQuotaAsync(fullReduceOrRecommendIdList, -1).Result;  //使用人数-1  //修改赠与奖学金表的使用人数
                if (!flag)
                {
                    throw new BussinessException(ModelType.Discount, 9);
                }
            }
        }
        
    }
}
