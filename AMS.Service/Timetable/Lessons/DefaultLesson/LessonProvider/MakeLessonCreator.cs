using System;
using System.Collections.Generic;
using System.Linq;
using AMS.Core;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;


namespace AMS.Service
{
    /// <summary>
    /// 排课课次产生者
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-02-19</para>
    /// </summary>
    public class MakeLessonCreator : MakeLessonProvider, ILessonCreator
    {
        private readonly TblTimMakeLessonRepository _makeLessonRepository;
        private readonly TblOdrEnrollOrderRepository _enrollOrderRepository;
        private readonly TblOdrEnrollOrderItemRepository _enrollOrderItemRepository;
        private readonly ViewClassTeacherDateRepository _viewClassTeacherDateRepository;
        private readonly ViewChangeClassTimeRepository _viewChangeClassTimeRepository;

        private readonly TblTimMakeLesson _makeLesson;
        private readonly UnitOfWork _unitOfWork;

        private readonly DefaultClassService _classService; //班级服务

        //存储班级老师变更的数据
        private readonly List<ViewClassTeacherDate> _viewClassTeacherDates;
        //存储当前班级上课时间发生变化的数据
        private readonly List<ViewChangeClassTime> _viewChangeClassTimeDates;

        /// <summary>
        /// 根据排课Id构建一个课次生产对象
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-19</para>
        /// </summary>
        /// <param name="makeLessonId">排课Id</param>
        /// <param name="unitOfWork">工作单元</param>
        public MakeLessonCreator(long makeLessonId, UnitOfWork unitOfWork) : base(makeLessonId)
        {
            this._unitOfWork = unitOfWork;
            _makeLessonRepository = unitOfWork.GetCustomRepository<TblTimMakeLessonRepository, TblTimMakeLesson>();
            _enrollOrderRepository = unitOfWork.GetCustomRepository<TblOdrEnrollOrderRepository, TblOdrEnrollOrder>();
            _enrollOrderItemRepository = unitOfWork.GetCustomRepository<TblOdrEnrollOrderItemRepository, TblOdrEnrollOrderItem>();
            _viewClassTeacherDateRepository = unitOfWork.GetCustomRepository<ViewClassTeacherDateRepository, ViewClassTeacherDate>();
            _viewChangeClassTimeRepository = unitOfWork.GetCustomRepository<ViewChangeClassTimeRepository, ViewChangeClassTime>();

            _makeLesson = _makeLessonRepository.Load(makeLessonId);
            _classService = new DefaultClassService(_makeLesson.ClassId);
            _viewClassTeacherDates = _viewClassTeacherDateRepository.Get(_makeLesson.ClassId);
            _viewChangeClassTimeDates = _viewChangeClassTimeRepository.Get(_makeLesson.ClassId);
        }

        /// <summary>
        /// 实际已使用的排课课次
        /// </summary>
        private int _lessonCount;

        /// <summary>
        /// 获取排课课次信息列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-19</para>
        /// </summary>
        /// <returns>课次信息列表</returns>
        public List<LessonCreatorInfo> GetLessonCreatorInfo()
        {
            List<LessonCreatorInfo> res = new List<LessonCreatorInfo>();

            //校验排课信息是否为空
            ValidateObject(_makeLesson);

            if (_makeLesson.IsConfirm)
            {
                throw new BussinessException(ModelType.SignUp, 9);
            }

            //1.获取学期信息
            var termInfo = TermService.GetTermByTermId(_classService.TblDatClass.TermId);
            ValidateObject(termInfo);

            //订单明细
            TblOdrEnrollOrderItem orderItem =
                _enrollOrderItemRepository.Load(_makeLesson.EnrollOrderItemId);
            //订单
            TblOdrEnrollOrder order =
                _enrollOrderRepository.Load(orderItem.EnrollOrderId);

            //2.获取停课日
            List<SchoolHolidayResponse> schoolHoliday = TermService.GetSchoolHoliday(termInfo);

            //上课时间
            DateTime classDate = _makeLesson.FirstClassTime;

            //首次上课日期大于学期结束日期则没有课次
            if (classDate > termInfo.EndDate)
            {
                return res;
            }

            //首次上课日期未到学期开始时间取学期设置开始日期
            if (classDate < termInfo.BeginDate)
            {
                classDate = termInfo.BeginDate;
            }

            bool isStandard180 = this.IsStandard180(_classService.ClassSchoolTimes); //180分钟
            TimeType duration = (TimeType)_classService.ClassSchoolTimes.FirstOrDefault().Duration; //时长

            int standard180 = termInfo.Classes180; //设置180分钟标准
            int makeLessonTotal = _makeLesson.ClassTimes; //设置排课总数

            if (isStandard180)
            {
                //180分钟设置的标准是否是奇数
                bool class180IsOddNumber = Convert.ToBoolean(termInfo.Classes180 % 2);
                standard180 = class180IsOddNumber ? termInfo.Classes180 - 1 : termInfo.Classes180;

                //排课是否是奇数
                bool makeIsOddNumber = Convert.ToBoolean(_makeLesson.ClassTimes % 2);
                //报名课次数量 考虑180分钟排2个课次的完整性
                makeLessonTotal = makeIsOddNumber ? _makeLesson.ClassTimes - 1 : _makeLesson.ClassTimes;
            }

            //上课日期大于学期结束日期停止排课
            while (classDate <= termInfo.EndDate)
            {
                //当天是否停课
                bool isClosed = SchoolHolidayService.TodayIsSuspendClasses(schoolHoliday, classDate);

                if (isClosed)
                {
                    classDate = classDate.AddDays(1);
                    continue;
                }

                int week = WeekDayConvert.DayOfWeekToInt(classDate);

                List<TblDatSchoolTime> classTimes = _classService.ClassSchoolTimes
                    .Where(x => x.WeekDay == week)
                    .OrderBy(x => x.BeginTime)
                    .ToList();

                //处理上课时间发生变化
                var changeTimes = _viewChangeClassTimeDates
                    .Where(x => x.OldClassDate == classDate)
                    .OrderBy(x => x.NewClassBeginTime)
                    .ToList();

                if (!classTimes.Any() && !changeTimes.Any())
                {
                    classDate = classDate.AddDays(1);
                    continue;
                }

                //180分钟课程
                if (duration == TimeType.Ninety && isStandard180 == true)
                {
                    if (!changeTimes.Any())
                    {
                        var ct1 = classTimes[0];
                        var ct2 = classTimes[1];

                        res.Add(this.GetCreatorInfo(classDate, ct1.BeginTime, ct1.EndTime, termInfo.SchoolId, order.StudentId, termInfo.TermId));
                        res.Add(this.GetCreatorInfo(classDate, ct2.BeginTime, ct2.EndTime, termInfo.SchoolId, order.StudentId, termInfo.TermId));
                    }
                    else
                    {
                        var ct1 = changeTimes[0];
                        var ct2 = changeTimes[1];

                        res.Add(this.GetCreatorInfo(ct1.NewClassDate, ct1.NewClassBeginTime, ct1.NewClassEndTime, termInfo.SchoolId, order.StudentId, termInfo.TermId));
                        res.Add(this.GetCreatorInfo(ct2.NewClassDate, ct2.NewClassBeginTime, ct2.NewClassEndTime, termInfo.SchoolId, order.StudentId, termInfo.TermId));
                    }

                    _lessonCount += 2;
                }
                else
                {
                    if (!changeTimes.Any())
                    {
                        var ct1 = classTimes[0];
                        res.Add(this.GetCreatorInfo(classDate, ct1.BeginTime, ct1.EndTime, termInfo.SchoolId, order.StudentId, termInfo.TermId));
                    }
                    else
                    {
                        var ct1 = changeTimes[0];
                        res.Add(this.GetCreatorInfo(ct1.NewClassDate, ct1.NewClassBeginTime, ct1.NewClassEndTime, termInfo.SchoolId, order.StudentId, termInfo.TermId));
                    }

                    _lessonCount += 1;
                }

                //上课时间大于学期结束时间停止排课或者预计排课课次等于实际排课课次停止排课
                if (this._lessonCount == makeLessonTotal)
                {
                    return res;
                }
                //执行60分钟标准
                else if (duration == TimeType.Sixty && this._lessonCount == termInfo.Classes60)
                {
                    return res;
                }
                //执行90分钟标准
                else if (duration == TimeType.Ninety && isStandard180 == false && this._lessonCount == termInfo.Classes90)
                {
                    return res;
                }
                //执行180分钟标准
                else if (duration == TimeType.Ninety && isStandard180 == true && this._lessonCount == standard180)
                {
                    return res;
                }

                classDate = classDate.AddDays(1);
            }

            return res;
        }

        /// <summary>
        /// 获取创建课次信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-19</para>
        /// </summary>
        /// <param name="classDate">上课日期</param>
        /// <param name="beginTime">上课开始时间</param>
        /// <param name="endTime">上课结束时间</param>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        /// <param name="termId">学期Id</param>
        /// <returns>创建的课次信息</returns>
        private LessonCreatorInfo GetCreatorInfo(
            DateTime classDate, string beginTime, string endTime, string schoolId,
            long studentId, long termId)
        {
            //生成新课次需注意老师代课调整、全校上课日期调整、班级上课日期调整

            LessonCreatorInfo lesson = new LessonCreatorInfo
            {
                BusinessId = _makeLesson.MakeLessonId,
                ClassId = _makeLesson.ClassId,
                EnrollOrderItemId = _makeLesson.EnrollOrderItemId,

                ClassRoomId = _classService.TblDatClass.ClassRoomId,
                CourseId = _classService.TblDatClass.CourseId,
                CourseLevelId = _classService.TblDatClass.CourseLeveId,
                TeacherId = _classService.TblDatClass.TeacherId,

                ClassDate = classDate,
                ClassBeginTime = beginTime,
                ClassEndTime = endTime,
                SchoolId = schoolId,
                StudentId = studentId,
                TermId = termId,

                BusinessType = base.BusinessType,
                LessonCount = 1,
                LessonType = LessonType.RegularCourse
            };

            //处理某些上课日期有老师代课的情况
            var temp = _viewClassTeacherDates
                .FirstOrDefault(x => x.ClassDate == classDate && x.ClassId == lesson.ClassId);

            if (temp != null && temp.TeacherId != lesson.TeacherId)
            {
                lesson.TeacherId = temp.TeacherId;
            }

            return lesson;
        }

        /// <summary>
        /// 是否是180分钟的课
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-19</para>
        /// </summary>
        /// <param name="classSchoolTimes">上课时间段列表</param>
        /// <returns>true:是 false:否</returns>
        private bool IsStandard180(List<TblDatSchoolTime> classSchoolTimes)
        {
            var first = classSchoolTimes.FirstOrDefault();

            //180=当天有两节课连上
            return classSchoolTimes.Count(x => x.WeekDay == first.WeekDay) == 2;
        }

        /// <summary>
        /// 排课之后跟家长确认
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-19</para>
        /// </summary>
        public void AfterLessonCreate()
        {
            ValidateObject(_makeLesson);

            //跟家长确认
            _makeLesson.IsConfirm = true;
            _makeLesson.ClassTimes = _lessonCount;
            _makeLessonRepository.Update(_makeLesson);

            EnrollOrderService service = new EnrollOrderService(_makeLesson.SchoolId);
            service.AddClassTimesUse(_makeLesson.EnrollOrderItemId, this._lessonCount, _unitOfWork);
        }

        /// <summary>
        /// 校验对象是否为空
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-19</para>
        /// </summary>
        /// <exception cref="BussinessException">
        /// 异常ID：1，异常描述：对象为空
        /// </exception>
        private static void ValidateObject(object obj)
        {
            if (obj == null)
            {
                throw new BussinessException(ModelType.Default, 1);
            }
        }
    }
}
