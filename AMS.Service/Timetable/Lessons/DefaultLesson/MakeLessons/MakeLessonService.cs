using System;
using System.Collections.Generic;
using System.Linq;
using AMS.Anticorrosion.HRS;
using AMS.Core;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;

namespace AMS.Service
{
    /// <summary>
    /// 对报名课程排课处理
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2018-11-12</para>
    /// </summary>
    public class MakeLessonService
    {
        private TblTimMakeLessonRepository _makeLessonRepository = new TblTimMakeLessonRepository();
        private TblOdrEnrollOrderItemRepository _enrollOrderItemRepository = new TblOdrEnrollOrderItemRepository();
        private TblOdrEnrollOrderRepository _enrollOrderRepository = new TblOdrEnrollOrderRepository();
        private ViewTimStudentRecordRepository _studentRecordRepository = new ViewTimStudentRecordRepository();

        private readonly string _schoolId; //校区ID
        private readonly long _studentId;  //学生ID

        /// <summary>
        /// 构造一个校区下一个学生的排课对象
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-19</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        public MakeLessonService(string schoolId, long studentId)
        {
            this._schoolId = schoolId;
            this._studentId = studentId;
        }

        /// <summary>
        /// 初始化工作单元
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-19</para>
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        private void InitUnitOfWork(UnitOfWork unitOfWork)
        {
            _makeLessonRepository = unitOfWork.GetCustomRepository<TblTimMakeLessonRepository, TblTimMakeLesson>();
            _enrollOrderItemRepository = unitOfWork.GetCustomRepository<TblOdrEnrollOrderItemRepository, TblOdrEnrollOrderItem>();
            _enrollOrderRepository = unitOfWork.GetCustomRepository<TblOdrEnrollOrderRepository, TblOdrEnrollOrder>();
            _studentRecordRepository = unitOfWork.GetCustomRepository<ViewTimStudentRecordRepository, ViewTimStudentStudyRecord>();
        }

        #region GetMakeLessonList 获取学生排课列表

        /// <summary>
        /// 获取学生排课列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <remarks>排课列表</remarks>
        public List<MakeLessonListResponse> GetMakeLessonList()
        {
            //1、获取学生订单
            List<TblOdrEnrollOrder> studentOrders = this.GetStudentOrders();
            if (studentOrders.Count == 0)
            {
                return new List<MakeLessonListResponse>();
            }

            //2、获取学生报名订单课程明细
            List<TblOdrEnrollOrderItem> studentOrderItems =
                this.GetStudentOrderItems(studentOrders.Select(x => x.EnrollOrderId));

            //3、获取学生排课
            List<TblTimMakeLesson> studentMakeLessons =
                this.GetStudentMakeLessons(studentOrderItems.Select(x => x.EnrollOrderItemId));

            //4、基础数据
            this._courseBasicData = this.GetCourseBasicData(studentMakeLessons.Select(x => x.ClassId));

            //5、组合数据
            return GetMakeLessonListResponse(studentOrderItems, studentMakeLessons);
        }

        /// <summary>
        /// 获取排课返回数据列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-10</para>
        /// </summary>
        /// <param name="studentOrderItems">学生报名订单课程明细</param>
        /// <param name="studentMakeLessons">学生排课</param>
        /// <returns>排课列表集合</returns>
        private List<MakeLessonListResponse> GetMakeLessonListResponse(
            List<TblOdrEnrollOrderItem> studentOrderItems,
            List<TblTimMakeLesson> studentMakeLessons)
        {
            List<MakeLessonListResponse> res = new List<MakeLessonListResponse>();

            List<int> years = studentOrderItems.GroupBy(x => x.Year).Select(x => x.Key).OrderBy(x => x).ToList();

            foreach (var year in years)
            {
                MakeLessonListResponse makeLessonRes = new MakeLessonListResponse
                {
                    Year = year,
                    MakeLessonItem = new List<MakeLessonItemListResponse>()
                };

                var orderItems = studentOrderItems.Where(x => x.Year == year).ToList();

                foreach (var orderItem in orderItems)
                {
                    //报名数等于排课数则不显示数据
                    if (orderItem.ClassTimes == orderItem.ClassTimesUse)
                    {
                        continue;
                    }

                    //未与家长确认的排课
                    var unconfirmedMakeLesson =
                        studentMakeLessons.Where(x => x.EnrollOrderItemId == orderItem.EnrollOrderItemId && !x.IsConfirm);

                    if (unconfirmedMakeLesson.Any())
                    {
                        makeLessonRes.MakeLessonItem.AddRange(UnconfirmedMakeLesson(orderItem, unconfirmedMakeLesson));
                        continue;
                    }

                    //未排课
                    makeLessonRes.MakeLessonItem.Add(NotMakeLesson(orderItem));
                }

                if (makeLessonRes.MakeLessonItem.Count > 0)
                {
                    res.Add(makeLessonRes);
                }
            }

            return res;
        }

        /// <summary>
        /// 获取学生正常的订单
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-09</para>
        /// </summary>
        /// <returns>报名订单集合</returns>
        private List<TblOdrEnrollOrder> GetStudentOrders()
        {
            EnrollOrderService enrollOrderService = new EnrollOrderService(_schoolId);

            List<TblOdrEnrollOrder> studentAllOrders = enrollOrderService.GetStudentOrders(_studentId).Result;

            int paid = (int)OrderStatus.Paid;
            int finish = (int)OrderStatus.Finish;

            List<TblOdrEnrollOrder> studentNormalOrders =
                studentAllOrders.Where(x => x.OrderStatus == paid || x.OrderStatus == finish).ToList();

            return studentNormalOrders;
        }

        /// <summary>
        /// 获取学生报名订单课程明细
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-09</para>
        /// </summary>
        /// <param name="enrollOrderId">报名订单ID</param>
        /// <returns>报名订单课程明细集合</returns>
        private List<TblOdrEnrollOrderItem> GetStudentOrderItems(IEnumerable<long> enrollOrderId)
        {
            EnrollOrderService enrollOrderService = new EnrollOrderService(_schoolId);

            //已报名
            int enroll = (int)OrderItemStatus.Enroll;

            List<TblOdrEnrollOrderItem> studentOrderItems =
                enrollOrderService.GetEnrollOrderItemByEnrollOrderId(enrollOrderId)
                    .Result
                    .Where(x => x.Status == enroll)
                    .ToList();

            return studentOrderItems;
        }

        /// <summary>
        /// 获取学生排课
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-09</para>
        /// </summary>
        /// <param name="enrollOrderItemId">报名订单课程明细Id</param>
        /// <returns>排课集合</returns>
        private List<TblTimMakeLesson> GetStudentMakeLessons(IEnumerable<long> enrollOrderItemId)
        {
            List<TblTimMakeLesson> studentMakeLessons =
                _makeLessonRepository.GetByEnrollOrderItemId(enrollOrderItemId).Result;

            return studentMakeLessons;
        }

        /// <summary>
        /// 未跟家长确认的排课
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="orderItem">报名订单课程明细项</param>
        /// <param name="unconfirmedMakeLesson">未确认排课集合</param>
        /// <remarks>排课列表</remarks>
        private List<MakeLessonItemListResponse> UnconfirmedMakeLesson(
            TblOdrEnrollOrderItem orderItem,
            IEnumerable<TblTimMakeLesson> unconfirmedMakeLesson)
        {
            List<MakeLessonItemListResponse> res = new List<MakeLessonItemListResponse>();

            foreach (var item in unconfirmedMakeLesson)
            {
                //班级代码
                string classNo = _courseBasicData.Classs.FirstOrDefault(x => x.ClassId.Equals(item.ClassId))?.ClassNo ?? string.Empty;

                //学期名称
                string termName = (from a in _courseBasicData.Classs
                                   join b in _courseBasicData.Terms on a.TermId equals b.TermId
                                   where a.ClassId == item.ClassId
                                   select b.TermName).FirstOrDefault() ?? string.Empty;

                //课程简称
                string shortName = (from a in _courseBasicData.Classs
                                    join c in _courseBasicData.Courses on a.CourseId equals c.CourseId
                                    where a.ClassId == item.ClassId
                                    select c.ShortName).FirstOrDefault() ?? string.Empty;

                //课程等级名称
                string levelName = (from a in _courseBasicData.Classs
                                    join d in _courseBasicData.CourseLvs on a.CourseLeveId equals d.CourseLevelId
                                    where a.ClassId == item.ClassId
                                    select d.LevelCnName).FirstOrDefault() ?? string.Empty;

                //老师名称
                var teacherName = (from a in _courseBasicData.Classs
                                   join f in _courseBasicData.Teachers on a.TeacherId equals f.TeacherId
                                   where a.ClassId == item.ClassId
                                   select f.TeacherName).FirstOrDefault() ?? string.Empty;

                res.Add(new MakeLessonItemListResponse
                {
                    TermTypeName = TermTypeService.GetTermTypeName(orderItem.TermTypeId),
                    ClassTimes = orderItem.ClassTimes,
                    ClassTimesUse = item.ClassTimes,
                    EnrollOrderItemId = orderItem.EnrollOrderItemId,
                    ShortName = shortName,
                    LevelName = levelName,
                    ClassNo = classNo,
                    TeacherName = teacherName,
                    TermName = termName,
                    Status = 1,
                });
            }

            return res;
        }

        /// <summary>
        /// 未排课
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="orderItem">报名订单课程明细</param>
        /// <returns>排课项</returns>
        private MakeLessonItemListResponse NotMakeLesson(TblOdrEnrollOrderItem orderItem)
        {
            MakeLessonItemListResponse res = new MakeLessonItemListResponse
            {
                TermTypeName = TermTypeService.GetTermTypeName(orderItem.TermTypeId),
                ClassTimes = orderItem.ClassTimes,
                EnrollOrderItemId = orderItem.EnrollOrderItemId,
                ClassTimesUse = 0,
                ShortName = string.Empty,
                ClassNo = string.Empty,
                TeacherName = string.Empty,
                TermName = string.Empty,
                LevelName = string.Empty,
                Status = 0,
            };

            return res;
        }

        /// <summary>
        /// 排课基础数据
        /// </summary>
        private CourseBasicData _courseBasicData;

        /// <summary>
        /// 获取排课基础数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-19</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <remarks>排课基础数据</remarks>
        private CourseBasicData GetCourseBasicData(IEnumerable<long> classId)
        {
            CourseBasicData courseBasicData = new CourseBasicData
            {
                Classs = DefaultClassService.GetClassByClassIdAsync(classId).Result //班级
            };

            //学期
            courseBasicData.Terms = TermService.GetTermByTermId(courseBasicData.Classs.Select(x => x.TermId));

            //所有课程
            courseBasicData.Courses = CourseService.GetAllAsync().Result;

            //所有课程等级
            courseBasicData.CourseLvs = CourseLevelService.GetAll();

            //老师
            courseBasicData.Teachers = TeachService.GetTeachers();

            return courseBasicData;
        }

        /// <summary>
        /// 排课基础数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-19</para>
        /// </summary>
        private class CourseBasicData
        {
            public List<TblDatClass> Classs { get; set; }

            public List<TblDatTerm> Terms { get; set; }

            public List<TblDatCourse> Courses { get; set; }

            public List<TblDatCourseLevel> CourseLvs { get; set; }

            public List<ClassTimetableTeacherResponse> Teachers { get; set; }
        }

        #endregion

        #region GetMakeLessonDetail 获取的排课详情

        /// <summary>
        /// 获取的排课详情
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="enrollOrderItemId">报名课程订单Id</param>
        /// <returns>学生报名一个课程的排课详细信息</returns>
        public MakeLessonDetailResponse GetMakeLessonDetail(long enrollOrderItemId)
        {
            MakeLessonDetailResponse res = new MakeLessonDetailResponse
            {
                CourseInfos = new List<CourseInformation>()
            };

            TblOdrEnrollOrderItem enrollOrderItem = _enrollOrderItemRepository.Load(enrollOrderItemId);
            TblOdrEnrollOrder enrollOrder = _enrollOrderRepository.Load(enrollOrderItem.EnrollOrderId);

            TblDatCourse course = CourseService.GetByCourseId(enrollOrderItem.CourseId);

            res.RegisterInfo = new RegisterInformation()
            {
                ClassTimes = enrollOrderItem.ClassTimes,
                ClassTimesUse = enrollOrderItem.ClassTimesUse,
                CourseName = string.Empty,
                LevelName = CourseLevelService.GetById(enrollOrderItem.CourseLevelId)?.LevelCnName ?? string.Empty,
                Year = enrollOrderItem.Year,
                CourseType = CourseType.Elective,
                TermTypeId = enrollOrderItem.TermTypeId,
                TermTypeName = TermTypeService.GetTermTypeName(enrollOrderItem.TermTypeId),
                EnrollDate = enrollOrder.CreateTime
            };

            if (course != null)
            {
                res.RegisterInfo.CourseName = course.ShortName;
                res.RegisterInfo.CourseType = CourseType.Compulsory;
            }

            List<TblTimMakeLesson> makeLessons =
                _makeLessonRepository.GetUnconfirmedMakeLessonList(enrollOrderItem.EnrollOrderItemId);

            if (makeLessons.Any())
            {
                List<long> classIds = makeLessons.Select(x => x.ClassId).ToList();

                List<TblDatClass> classes = DefaultClassService.GetClassByClassIdAsync(classIds).Result;

                List<ViewRoomCourse> classRooms = ClassRoomService.GetClassRoomBySchoolId(enrollOrder.SchoolId);

                //老师
                var teacherList = TeachService.GetTeachers();

                foreach (var makeLesson in makeLessons)
                {
                    var classInfo = classes.FirstOrDefault(x => x.ClassId == makeLesson.ClassId);
                    var classSchoolTimes = new DefaultClassService(classInfo.ClassId).ClassSchoolTimes;
                    //老师信息
                    var teacher = teacherList.FirstOrDefault(x => x.TeacherId == classInfo.TeacherId);

                    CourseInformation courseInformation = new CourseInformation
                    {
                        ClassId = classInfo.ClassId,
                        Year = enrollOrderItem.Year,
                        ClassNo = classInfo.ClassNo,
                        ClassTimesUse = makeLesson.ClassTimes,
                        CourseName = course?.ShortName ?? string.Empty,
                        FirstClassTime = makeLesson.FirstClassTime,
                        RoomNo = classRooms.FirstOrDefault(x => x.ClassRoomId == classInfo.ClassRoomId)?.RoomNo ?? string.Empty,
                        LevelName = CourseLevelService.GetById(classInfo.CourseLeveId)?.LevelCnName ?? string.Empty,
                        TeacherName = teacher?.TeacherName ?? string.Empty,
                        TermName = TermService.GetTermByTermId(classInfo.TermId)?.TermName ?? string.Empty,
                        Week = classSchoolTimes.Select(x => x.WeekDay)
                                .Distinct()
                                .OrderBy(x => x)
                                .Select(x => WeekDayConvert.IntToString(x))
                                .ToList(),
                        PeriodTime = new List<string>()
                    };

                    foreach (var item in classSchoolTimes)
                    {
                        string time = item.BeginTime + "-" + item.EndTime;
                        if (courseInformation.PeriodTime.Any(x => x == time))
                        {
                            continue;
                        }
                        courseInformation.PeriodTime.Add(time);
                    }

                    res.CourseInfos.Add(courseInformation);
                }
            }

            return res;
        }

        #endregion

        #region Confirm 排课确认

        /// <summary>
        /// 排课确认
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-05</para>
        /// </summary>
        /// <param name="enrollOrderItemId">报名订单课程明细ID</param>
        /// <exception cref="BussinessException">
        /// 异常ID：33,异常描述：订单状态已改变不能进行操作
        /// </exception>
        /// <exception cref="Exception">系统异常</exception>
        public void Confirm(long enrollOrderItemId)
        {
            var orderItem = _enrollOrderItemRepository.Load(enrollOrderItemId);

            if (orderItem == null)
            {
                throw new BussinessException(ModelType.Default, 1);
            }

            if (orderItem.Status != (int)OrderItemStatus.Enroll)
            {
                throw new BussinessException(ModelType.Timetable, 33, $"当前课程{EnumName.GetDescription(typeof(OrderItemStatus), orderItem.Status)}");
            }

            List<TblTimMakeLesson> makeLessons =
                _makeLessonRepository.GetUnconfirmedMakeLessonList(enrollOrderItemId);

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                InitUnitOfWork(unitOfWork);

                unitOfWork.BeginTransaction();
                try
                {
                    foreach (var item in makeLessons)
                    {
                        this.MakeInternal(item.MakeLessonId, unitOfWork);
                    }
                    unitOfWork.CommitTransaction();
                }
                catch
                {
                    unitOfWork.RollbackTransaction();
                    throw;
                }
            }
        }

        #endregion

        #region Save 保存课次信息,但不排课

        /// <summary>
        /// 保存课次信息,但不排课
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-08</para> 
        /// </summary>
        /// <param name="req">报名的排课信息</param>
        /// <returns>排课表主键ID</returns>
        public List<long> Save(List<MakeLessonRequest> req)
        {
            List<long> makeLessonIds = new List<long>();

            //过滤掉排课课次为0的数据
            req = req.Where(x => x.ClassTimes > 0).ToList();

            var reqGroup = req.GroupBy(x => x.EnrollOrderItemId).ToList();

            var enrollOrderItemIds = reqGroup.Select(m => m.Key);

            var orderItems = _enrollOrderItemRepository.GetByEnrollOrderItemId(enrollOrderItemIds).Result;

            foreach (var item in reqGroup)
            {
                var orderItem = orderItems.FirstOrDefault(x => x.EnrollOrderItemId == item.Key);
                if (orderItem == null)
                {
                    continue;
                }

                if (orderItem.Status != (int)OrderItemStatus.Enroll)
                {
                    throw new BussinessException(ModelType.Timetable, 33,
                        $"当前课程{EnumName.GetDescription(typeof(OrderItemStatus), orderItem.Status)}");
                }

                List<TblTimMakeLesson> makeLessons = req.Where(x => x.EnrollOrderItemId == item.Key)
                    .Select(x => new TblTimMakeLesson
                    {
                        ClassId = x.ClassId,
                        ClassTimes = GetClassTimes(x.ClassId, x.ClassTimes, x.FirstClassTime),
                        EnrollOrderItemId = x.EnrollOrderItemId,
                        FirstClassTime = x.FirstClassTime,
                        IsConfirm = false,
                        MakeLessonId = IdGenerator.NextId(),
                        CreateTime = DateTime.Now,
                        SchoolId = _schoolId
                    }).ToList();

                int classTimesCount = makeLessons.Where(x => x.EnrollOrderItemId == item.Key).Sum(x => x.ClassTimes);
                if (classTimesCount + orderItem.ClassTimesUse > orderItem.ClassTimes)
                {
                    throw new BussinessException(ModelType.SignUp, 8); //未排课次不足
                }

                //清空未确认的排课
                _makeLessonRepository.ClearUnconfirmedClass(orderItem.EnrollOrderItemId).Wait();
                _makeLessonRepository.SaveTask(makeLessons).Wait();

                makeLessonIds.AddRange(makeLessons.Select(x => x.MakeLessonId));
            }

            return makeLessonIds;
        }

        /// <summary>
        /// 处理排课课次
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-08</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <param name="classTimes">预计排课课次</param>
        /// <param name="firstTime">首次上课时间</param>
        /// <returns>实际需要排多少次课</returns>
        private int GetClassTimes(long classId, int classTimes, DateTime firstTime)
        {
            int maximumLesson = new DefaultClassService(classId).GetMaximumLessonByFirstTime(firstTime);

            return maximumLesson < classTimes ? maximumLesson : classTimes;
        }

        #endregion

        /// <summary>
        /// 获取班级排课数
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <returns>班级排课数量</returns>
        internal static int GetClassScheduleNum(long classId)
        {
            return new TblTimLessonRepository().TotalClassScheduleNum(classId);
        }

        /// <summary>
        /// 保存并排课
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-08</para>
        /// </summary>
        /// <param name="req">报名的排课信息</param>
        /// <param name="unitOfWork">工作单元</param>
        public void Make(List<MakeLessonRequest> req, UnitOfWork unitOfWork)
        {
            InitUnitOfWork(unitOfWork);

            //1、排课处理
            List<long> makeLessonIds = this.Save(req);

            //2、课次操作处理
            foreach (long id in makeLessonIds)
            {
                this.MakeInternal(id, unitOfWork);
            }
        }

        /// <summary>
        /// 课表作废,订单作废时会用到
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-08</para>
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="unitOfWork">工作单元</param>
        public static void Cancel(long orderId, UnitOfWork unitOfWork)
        {
            List<TblOdrEnrollOrderItem> enrollOrderItems = new TblOdrEnrollOrderItemRepository().GetByEnrollOrderId(orderId).Result;

            var enrollOrderItemIds = enrollOrderItems.Select(x => x.EnrollOrderItemId);

            List<TblTimMakeLesson> makeLessons = new TblTimMakeLessonRepository().GetByEnrollOrderItemId(enrollOrderItemIds).Result;

            foreach (var makeLesson in makeLessons)
            {
                CancelMakeLessonFinisher cancelMakeLesson = new CancelMakeLessonFinisher(makeLesson.MakeLessonId, unitOfWork);
                LessonService lessonService = new LessonService(unitOfWork);
                lessonService.Finish(cancelMakeLesson);
            }
        }

        /// <summary>
        /// 开始真实排课
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-08</para>
        /// </summary>
        /// <param name="makeLessonId">排课Id</param>
        /// <param name="unitOfWork">工作单元</param>
        private void MakeInternal(long makeLessonId, UnitOfWork unitOfWork)
        {
            MakeLessonCreator lessonCreator = new MakeLessonCreator(makeLessonId, unitOfWork);  //实例化课次生产者
            LessonService lessonService = new LessonService(unitOfWork);
            lessonService.Create(lessonCreator);                                    //课次生产

            new StudentService(this._schoolId).UpdateStudentStatusById(this._studentId, unitOfWork, StudyStatus.Reading);
        }

        /// <summary>
        /// 获取获取学生学习记录信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-07</para>
        /// </summary>
        /// <returns>学生学习记录列表</returns>
        public List<StudentStudyRecordResponse> GetStudentStudyRecordList()
        {
            var result = new List<StudentStudyRecordResponse>();
            List<int> yearList = new List<int>() { DateTime.Now.Year, DateTime.Now.AddYears(1).Year, DateTime.Now.AddYears(2).Year };
            var studentRecordList = _studentRecordRepository.GetStudentRecordListAsync(this._schoolId, this._studentId, yearList).Result;
            if (studentRecordList != null && studentRecordList.Count > 0)
            {
                var teacherList = EmployeeService.GetAllBySchoolId(this._schoolId);
                var termTypeList = new TermTypeService().GetAll();
                foreach (var year in yearList)
                {
                    var studentRecord = studentRecordList.Where(a => a.Year == year);
                    if (studentRecord != null && studentRecord.Any())
                    {
                        StudentStudyRecordResponse recordResponse = new StudentStudyRecordResponse();
                        recordResponse.Year = year;
                        recordResponse.Data = studentRecord.Select(a => new StudentStudyRecordDetailResponse()
                        {
                            EnrollOrderItemId = a.EnrollOrderItemId,
                            TermTypeName = termTypeList.FirstOrDefault(t => t.TermTypeId == a.TermTypeId)?.TermTypeName,
                            TermName = a.TermName,
                            TeacherName = teacherList.FirstOrDefault(t => t.EmployeeId == a.TeacherId)?.EmployeeName,
                            RoomNo = a.RoomNo,
                            CourseName = a.CourseName,
                            ClassNo = a.ClassNo,
                            ClassTimes = a.ClassTimes,
                            ClassDate = a.ClassDate,
                            ArrangedclassTimes = a.ArrangedClassTimes,
                            AttendedClassTimes = a.AttendedClassTimes,
                            RemainingClassTimes = a.RemainingClassTimes
                        }).ToList();
                        result.Add(recordResponse);
                    }
                }
            }
            return result;
        }

        #region GetConfirmedMakeLesson 获取确认排课数据
        /// <summary>
        /// 获取确认排课数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-10</para>
        /// </summary>
        /// <param name="enrollOrderItemId">报名订单课程明细Id</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>排课数据</returns>
        internal static List<MakeLessonResponse> GetConfirmedMakeLesson(IEnumerable<long> enrollOrderItemId, string companyId)
        {
            List<MakeLessonResponse> res = new List<MakeLessonResponse>();

            List<TblTimMakeLesson> confirmMakeLessons = new TblTimMakeLessonRepository()
                .GetByEnrollOrderItemId(enrollOrderItemId)
                .Result
                .Where(x => x.IsConfirm)
                .ToList();

            if (confirmMakeLessons.Count == 0)
            {
                return res;
            }

            var basicData = GetMakeLessonResponseBasicData(confirmMakeLessons.FirstOrDefault().SchoolId, confirmMakeLessons.Select(x => x.ClassId), companyId);

            foreach (var makeLesson in confirmMakeLessons)
            {
                var classInfo = GetClassInfo(basicData, makeLesson.ClassId);

                if (classInfo == null)
                {
                    continue;
                }

                var makeLessonResponse = GetMakeLessonResponse(classInfo, companyId);

                res.Add(makeLessonResponse);
            }

            return res;
        }

        /// <summary>
        /// 获取排课课次信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-19</para>
        /// </summary>
        /// <param name="classInfo">班级信息</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>排课课次信息</returns>
        private static MakeLessonResponse GetMakeLessonResponse(ClassInfo classInfo, string companyId)
        {
            MakeLessonResponse response = new MakeLessonResponse
            {
                ClassId = classInfo.ClassId,
                ClassName = classInfo.ClassName,
                ClassNo = classInfo.ClassNo,
                CourseLeaveName = classInfo.LevelName,
                RoomNo = classInfo.RoomNo,
                Week = new List<string>(),
                ClassTime = new List<string>()
            };

            DefaultClassService classService = new DefaultClassService(classInfo.ClassId);

            for (int i = 1; i <= 7; i++)
            {
                var schoolTimes = classService.ClassSchoolTimes.Where(x => x.WeekDay == i).ToList();

                if (schoolTimes.Count == 0)
                {
                    continue;
                }

                response.ClassTime.Add(WeekDayConvert.IntToString(i));

                foreach (var item in schoolTimes)
                {
                    var time = item.BeginTime + "-" + item.EndTime;

                    if (response.ClassTime.Any(x => x == time))
                    {
                        continue;
                    }
                    response.ClassTime.Add(time);
                }
            }

            return response;
        }

        /// <summary>
        /// 获取班级信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-19</para>
        /// </summary>
        /// <param name="basicData">返回排课数据组合需要的基础数据</param>
        /// <param name="classId">班级Id</param>
        /// <returns>班级信息</returns>
        private static ClassInfo GetClassInfo(MakeLessonResponseBasicData basicData, long classId)
        {
            var classInfo = (from a in basicData.ClassInfos
                             join b in basicData.Courses on a.CourseId equals b.CourseId
                             join c in basicData.CourseLevels on a.CourseLeveId equals c.CourseLevelId
                             join d in basicData.ClassRooms on a.ClassRoomId equals d.ClassRoomId
                             where a.ClassId == classId
                             select new ClassInfo
                             {
                                 ClassId = a.ClassId,
                                 ClassName = b.ClassCnName,
                                 ClassNo = a.ClassNo,
                                 RoomNo = d.RoomNo,
                                 LevelName = c.LevelCnName
                             }).FirstOrDefault();


            return classInfo;
        }

        /// <summary>
        /// 获取排课数据基础数据源
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-19</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="classIds">班级Id</param>
        ///  <param name="companyId">公司编号</param>
        /// <returns>返回排课数据源组合的基础数据源</returns>
        private static MakeLessonResponseBasicData GetMakeLessonResponseBasicData(string schoolId, IEnumerable<long> classIds, string companyId)
        {
            MakeLessonResponseBasicData res = new MakeLessonResponseBasicData
            {
                ClassInfos = DefaultClassService.GetClassByClassIdAsync(classIds).Result,
                ClassRooms = new SchoolClassRoomService(schoolId).GetAllEnableClassRoom(),
                Courses = CourseService.GetAllAsync().Result,
                CourseLevels = new CourseLevelService(companyId).GetList().Result
            };

            return res;
        }

        /// <summary>
        /// 班级信息
        /// </summary>
        private class ClassInfo
        {
            /// <summary>
            /// 班级Id
            /// </summary>
            public long ClassId { get; set; }

            /// <summary>
            /// 班级名称
            /// </summary>
            public string ClassName { get; set; }

            /// <summary>
            /// 班级代码
            /// </summary>
            public string ClassNo { get; set; }

            /// <summary>
            /// 门牌号
            /// </summary>
            public string RoomNo { get; set; }

            /// <summary>
            /// 登记名称
            /// </summary>
            public string LevelName { get; set; }

        }

        /// <summary>
        /// 返回排课数据组合需要的基础数据
        /// </summary>
        private class MakeLessonResponseBasicData
        {
            /// <summary>
            /// 教室信息
            /// </summary>
            public List<ClassRoomResponse> ClassRooms { get; set; }

            /// <summary>
            /// 班级信息
            /// </summary>
            public List<TblDatClass> ClassInfos { get; set; }

            /// <summary>
            /// 课程信息
            /// </summary>
            public List<TblDatCourse> Courses { get; set; }

            /// <summary>
            /// 课程等级信息
            /// </summary>
            public List<CourseLevelResponse> CourseLevels { get; set; }
        }
        #endregion

        

    }
}
