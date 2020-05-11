using System;
using System.Collections.Generic;
using System.Linq;
using AMS.Core;
using AMS.Core.Constants;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;

namespace AMS.Service
{

    /// <summary>
    /// 描述：撤销排课
    /// <para>作    者：Huang GaoLiang </para>
    /// <para>创建时间：2019-03-11</para>
    /// </summary>
    public class AdjustLessonRevokeFinisher : ILessonFinisher
    {
        #region 构造函数

        private readonly string _schoolId;// 校区编号
        private readonly long _studentId;// 学生编号
        private readonly long _lessonId;// 课次编号
        private int _nums;//撤销排课的课次
        private readonly UnitOfWork _unitOfWork;// 事务单元
        private TblTimMakeLesson _makelesson;// 排课信息
        private TblOdrEnrollOrderItem _enrollOrderItem;// 报名课程明细

        private readonly Lazy<ViewCompleteStudentAttendanceRepository> _viewCompleteStudentAttendanceRepository = new Lazy<ViewCompleteStudentAttendanceRepository>();

        //学生调整课次考勤仓储
        private readonly Lazy<ViewTimReplenishLessonStudentRepository> _viewTimReplenishLessonStudentRepository = new Lazy<ViewTimReplenishLessonStudentRepository>();


        /// <summary>
        /// 描述：撤销排课构造函数
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <param name="studentId">学生编号</param>
        /// <param name="lessonId">课次编号</param>
        /// <param name="unitOfWork">事务单元</param>
        public AdjustLessonRevokeFinisher(string schoolId, long studentId, long lessonId, UnitOfWork unitOfWork)
        {
            this._schoolId = schoolId;
            this._studentId = studentId;
            this._lessonId = lessonId;
            this._unitOfWork = unitOfWork;
        }

        #endregion

        /// <summary>
        /// 课次废弃之后
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        public void AfterLessonFinish()
        {
            // 1、更新排课表
            _makelesson.ClassTimes = _makelesson.ClassTimes - this._nums;

            // 2、更新报名订单课程明细表
            _enrollOrderItem.ClassTimesUse = _enrollOrderItem.ClassTimesUse - this._nums;

            _unitOfWork.GetCustomRepository<TblTimMakeLessonRepository, TblTimMakeLesson>().Update(_makelesson);
            _unitOfWork.GetCustomRepository<TblOdrEnrollOrderItemRepository, TblOdrEnrollOrderItem>().Update(_enrollOrderItem);

        }


        /// <summary>
        /// 获取班级上课时间课次调整要撤销的课次
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <returns>返回要撤销的课次信息</returns>
        public List<LessonFinisherInfo> GetLessonFinisherInfo()
        {

            // 1、根据课次ID查询课次基础信息
            var studentAttendance = _viewCompleteStudentAttendanceRepository.Value.GetLesson(this._lessonId);
            if (studentAttendance == null)
            {
                //未找到该学生的补课信息
                throw new BussinessException(ModelType.Timetable, 47);
            }

            if (studentAttendance.StudentId != this._studentId)
            {
                //未找到该学生的补课信息
                throw new BussinessException(ModelType.Timetable, 47);
            }

            _makelesson = new StudentTimetableService(this._schoolId, this._studentId).GetTimMakeLessonById(studentAttendance.BusinessId);
            if (_makelesson == null)
            {
                throw new ArgumentNullException(nameof(_makelesson));
            }

            _enrollOrderItem = new EnrollOrderService(this._schoolId).GetEnrollOrderItemById(studentAttendance.EnrollOrderItemId);

            // 2、获取一个班级学生的课次信息
            var stuDayAttendances = _viewCompleteStudentAttendanceRepository.Value.GetStudetnDayLessonList(studentAttendance.SchoolId, studentAttendance.ClassId, studentAttendance.ClassDate, this._studentId, LessonType.RegularCourse);

            // 3、获取父课次下面的所有课次信息
            var lessonIds = stuDayAttendances.Select(x => x.LessonId).ToList();
            List<ViewTimReplenishLessonStudent> stuLessons = _viewTimReplenishLessonStudentRepository.Value.GetLessonListByParentLessonId(lessonIds);

            if (stuLessons == null || !stuLessons.Any())
            {
                //未找到该学生的补课信息
                throw new BussinessException(ModelType.Timetable, 47);
            }

            List<LessonFinisherInfo> lessonList = stuLessons.Select(m => new LessonFinisherInfo
            {
                BusinessId = m.BusinessId,
                BusinessType = m.BusinessType,
                LessonId = m.LessonId,
                Remark = LessonProcessConstants.Remark

            }).ToList();

            this._nums = stuLessons.Count;

            return lessonList;
        }
    }
}
