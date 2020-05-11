using AMS.Dto;
using AMS.Storage.Models;
using System.Collections.Generic;
using System.Linq;

namespace AMS.Service
{
    /// <summary>
    /// 描述：补课周补课
    /// <para>作    者：Huang GaoLiang </para>
    /// <para>创建时间：2019-03-12</para>
    /// </summary>
    public class AdjustLessonReplenishWeekCreator : AdjustLessonReplenishWeekProvider, IReplenishLessonCreator
    {
        private readonly long _studentId;// 学生编号
        private readonly long _termId;//学期编号
        private string _schoolId;// 校区编号
        private string _teacherId;//老师编号
        private ReplenishWeekClassTimeAddRequest _replenishWeekClassTime;//补课周补课信息
        private List<TblTimAdjustLesson> _adjustLessonList;//课次调整业务信息
        private List<ViewStudentTimeLess> _studentTimeLessList;//学生缺课信息集合

        /// <summary>
        /// 补课周补课构造函数
        /// </summary>
        /// <param name="replenishLessonList">添加补课周补课集合</param>
        /// <param name="adjustLessonList">课次调整业务信息</param>
        /// <param name="studentTimeLessList">学生缺课信息集合</param>
        /// <param name="studentId">学生编号</param>
        /// <param name="termId">学期编号</param>
        /// <param name="schoolId">校区编号</param>
        /// <param name="teacherId">教师编号</param>
        public AdjustLessonReplenishWeekCreator(ReplenishWeekClassTimeAddRequest replenishLessonList, List<TblTimAdjustLesson> adjustLessonList, List<ViewStudentTimeLess> studentTimeLessList, long studentId, long termId, string schoolId, string teacherId)
        {
            this._studentId = studentId;
            this._termId = termId;
            this._schoolId = schoolId;
            this._teacherId = teacherId;
            this._replenishWeekClassTime = replenishLessonList;
            this._adjustLessonList = adjustLessonList;
            this._studentTimeLessList = studentTimeLessList;
        }

        /// <summary>
        /// 暂时无需实现
        /// </summary>
        public void AfterReplenishLessonCreate()
        {

        }

        /// <summary>
        /// 获取补课周补课信息
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-13 </para>
        /// </summary>
        /// <returns>返回补课周补课信息</returns>
        public List<ReplenishLessonCreatorInfo> GetReplenishLessonCreatorInfo()
        {
            List<ReplenishLessonCreatorInfo> replenishLessonList = new List<ReplenishLessonCreatorInfo>();

            // 根据课次编号获取课次基础信息
            int index = 0;
            foreach (var m in _replenishWeekClassTime.WeekClassTimeList)
            {
                var timeLess = _studentTimeLessList[index];
                var adjustLesson = _adjustLessonList[index];

                ReplenishLessonCreatorInfo info = new ReplenishLessonCreatorInfo
                {
                    OutLessonId = timeLess.LessonId,
                    SchoolId = this._schoolId,
                    EnrollOrderItemId = this._replenishWeekClassTime.EnrollOrderItemId,
                    StudentId = this._studentId,
                    ClassDate = m.ClassDate,
                    ClassBeginTime = m.ClassBeginTime,
                    ClassEndTime = m.ClassEndTime,
                    TermId = this._termId,
                    ClassId = this._replenishWeekClassTime.NewClassId,
                    CourseId = this._replenishWeekClassTime.CourseId,
                    CourseLevelId = this._replenishWeekClassTime.CourseLevelId,
                    TeacherId = this._replenishWeekClassTime.TeacherId,
                    ClassRoomId = m.ClassRoomId,
                    LessonType = LessonType.RegularCourse,
                    BusinessId = adjustLesson.AdjustLessonId,
                    BusinessType = (LessonBusinessType)base.BusinessType
                };
                index++;
                replenishLessonList.Add(info);
            }


            return replenishLessonList;
        }
    }
}
