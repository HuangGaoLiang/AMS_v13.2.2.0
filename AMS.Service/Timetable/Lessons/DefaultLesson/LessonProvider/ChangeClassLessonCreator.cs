using AMS.Core;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMS.Service
{
    /// <summary>
    /// 描述：转班课次产生者
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-7</para>
    /// </summary>
    public class ChangeClassLessonCreator : ChangeClassLessonProvider, ILessonCreator
    {
        private readonly long _enrollOrderItemId;     //报名订单课程明细Id
        private int _actualLesson;                    //实际排课课次
        private readonly ViewClassTeacherDateRepository _viewClassTeacherDateRepository;  //班级老师上课时间仓储
        private readonly ViewChangeClassTimeRepository _viewChangeClassTimeRepository;    //查询班级上课时间变化仓促
        private readonly List<ViewClassTeacherDate> _viewClassTeacherDates;   // //存储班级老师变更的数据
        private readonly List<ViewChangeClassTime> _viewChangeClassTimeDates;//存储当前班级上课时间发生变化的数据


        /// <summary>
        /// 描述：实例化转班课次产生者
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="entity">转班信息</param>
        /// <param name="enrollOrderItemId">报名订单课程明细Id</param>
        /// <param name="unitOfWork">工作单元事务</param>
        public ChangeClassLessonCreator(TblTimChangeClass entity, long enrollOrderItemId, UnitOfWork unitOfWork) : base(entity, unitOfWork)
        {
            _enrollOrderItemId = enrollOrderItemId;
            _viewClassTeacherDateRepository = unitOfWork.GetCustomRepository<ViewClassTeacherDateRepository, ViewClassTeacherDate>();
            _viewChangeClassTimeRepository = unitOfWork.GetCustomRepository<ViewChangeClassTimeRepository, ViewChangeClassTime>();
            _viewClassTeacherDates = _viewClassTeacherDateRepository.Get(entity.InClassId);
            _viewChangeClassTimeDates = _viewChangeClassTimeRepository.Get(entity.InClassId);
        }

        /// <summary>
        /// 描述：课次创建完成后的回调方法
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        public void AfterLessonCreate()
        {
            //已排课次的影响
            if (base._entity.ClassTimes != _actualLesson)
            {
                EnrollOrderService service = new EnrollOrderService(base._entity.SchoolId);
                service.AddClassTimesUse(_enrollOrderItemId, base._entity.ClassTimes - _actualLesson, base._unitOfWork);
            }

        }

        /// <summary>
        /// 描述：获取可转入的课次信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常Id:1,异常描述：未找到数据
        /// 异常Id:71，异常描述：转入班级时间段和课次信息调整时间段不一致
        /// </exception>
        /// <returns>可转入课次信息列表</returns>
        public List<LessonCreatorInfo> GetLessonCreatorInfo()
        {
            var result = new List<LessonCreatorInfo>();
            //获取转入班级信息
            var classService = new DefaultClassService(base._entity.InClassId);
            var firstTime = base._entity.InDate;
            //1.获取学期信息
            var termInfo = TermService.GetTermByTermId(classService.TblDatClass.TermId);
            if (termInfo == null)
            {
                throw new BussinessException(ModelType.Default, 1);
            }
            //2.获取停课日
            var schoolHoliday = TermService.GetSchoolHoliday(termInfo);
            //首次上课时间大于学期结束时间则没有课次
            if (firstTime > termInfo.EndDate){return result;}
            //首次上课时间小于学期开始时间，则首次上课时间等于学期开始时间
            if (firstTime < termInfo.BeginDate) { firstTime = termInfo.BeginDate; }

            while (firstTime <= termInfo.EndDate)
            {
                //上课时间大于学期结束时间或者预计排课课次等于实际排课课次，则停止排课
                if (firstTime > termInfo.EndDate || _entity.ClassTimes == _actualLesson) { break; }

                var isClosed = SchoolHolidayService.TodayIsSuspendClasses(schoolHoliday, firstTime);
                //当前时间是否是停课日，如果是则加1
                if (isClosed)
                {
                    firstTime = firstTime.AddDays(1);
                    continue;
                }
                //星期转换
                var week = WeekDayConvert.DayOfWeekToInt(firstTime);
                //获取与当前时间匹配的班级上课时间
                var schoolTimes = classService.ClassSchoolTimes.Where(x => x.WeekDay == week).OrderBy(x => x.BeginTime).ToList();

                //获取班级是否有课程信息调整（上课日期和上课时间段 修改）
                var classDateChangeList = _viewChangeClassTimeDates.Where(x => x.OldClassDate == firstTime).OrderBy(x => x.NewClassBeginTime).ToList();
                if (classDateChangeList.Count > 0 && classDateChangeList.Count != schoolTimes.Count)  //如果班级设置的上课时间段和课程调整的时间段不一致则抛出异常
                {
                    throw new BussinessException(ModelType.Timetable, 71);
                }
                var lessonCount = 0;
                foreach (var time in schoolTimes)
                {
                    var lesson = new LessonCreatorInfo
                    {
                        BusinessId = base._entity.ChangeClassId,
                        BusinessType = base.BusinessType,
                        ClassId = base._entity.InClassId,
                        ClassDate = firstTime,
                        ClassBeginTime = time.BeginTime,
                        ClassEndTime = time.EndTime,
                        ClassRoomId = classService.TblDatClass.ClassRoomId,
                        CourseId = classService.TblDatClass.CourseId,
                        CourseLevelId = classService.TblDatClass.CourseLeveId,
                        EnrollOrderItemId = _enrollOrderItemId,
                        SchoolId = termInfo.SchoolId,
                        StudentId = base._entity.StudentId,
                        TeacherId = classService.TblDatClass.TeacherId,
                        TermId = termInfo.TermId
                    };
                    //获取是否有老师调课
                    var teacherLessonChangeInfo = _viewClassTeacherDates.FirstOrDefault(x => x.ClassId == lesson.ClassId && x.ClassDate == lesson.ClassDate);
                    if (teacherLessonChangeInfo != null && teacherLessonChangeInfo.TeacherId.Trim() != lesson.TeacherId.Trim())
                    {
                        lesson.TeacherId = teacherLessonChangeInfo.TeacherId;
                    }
                    //获取班级是否有修改上课日期和上课时间段,并赋值
                    if (classDateChangeList.Any())
                    {
                        lesson.ClassDate = classDateChangeList[lessonCount].NewClassDate;
                        lesson.ClassBeginTime = classDateChangeList[lessonCount].NewClassBeginTime;
                        lesson.ClassEndTime = classDateChangeList[lessonCount].NewClassEndTime;
                    }
                    result.Add(lesson);
                    lessonCount++;
                    //实际排课课次+1
                    _actualLesson++;
                }
                firstTime = firstTime.AddDays(1);
            }
            return result;
        }
    }
}
