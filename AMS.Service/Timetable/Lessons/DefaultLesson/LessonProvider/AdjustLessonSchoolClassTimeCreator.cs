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
    /// 描述：全校上课日期课次生产者
    /// <para>作者：瞿琦</para>
    /// <para>创建时间：2019-3-8</para>
    /// </summary>
    public class AdjustLessonSchoolClassTimeCreator : AdjustLessonSchoolClassTimeProvider, ILessonCreator, IReplenishLessonCreator
    {
        private readonly DateTime _adjustDate;                       //调整后的时间
        private readonly List<TblTimAdjustLesson> _adjustLessonList; //课次调整业务表信息
        private readonly List<TblTimLesson> _lessonList;             //要更换上课日期的课次信息
        private readonly List<TblTimLesson> _normalLessonList;       //正常课次要创建的课次集合
        private readonly List<TblTimLesson> _replenishLessonList;     //补课调课要创建的课次集合

        /// <summary>
        /// 描述：实例化一个全校上课日期课次生产者
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-3-8</para>
        /// </summary>
        /// <param name="adjustDate">调整后的时间</param>
        /// <param name="adjustLessonList">课次调整业务表信息</param>
        /// <param name="lessonList">要更换上课日期的课次信息</param>
        public AdjustLessonSchoolClassTimeCreator(DateTime adjustDate, List<TblTimAdjustLesson> adjustLessonList, List<TblTimLesson> lessonList)
        {
            _adjustDate = adjustDate;
            _adjustLessonList = adjustLessonList;
            _lessonList = lessonList;
            _normalLessonList = this.GetNormalLessonList();
            _replenishLessonList = this.GetRelenishLessonList();
        }
        /// <summary>
        /// 业务类型
        /// </summary>
        public override int BusinessType { get => (int)LessonBusinessType.AdjustLessonSchoolClassTime; set { } }

        /// <summary>
        /// 暂不实现
        /// </summary>
        public void AfterLessonCreate()
        {

        }
        /// <summary>
        /// 暂不实现
        /// </summary>
        public void AfterReplenishLessonCreate()
        {

        }

        /// <summary>
        /// 正常课次是否有数据
        /// </summary>
        internal bool IsLessonData { get { return this._normalLessonList.Any(); } }
        /// <summary>
        /// 补课/调课 课次是否有数据
        /// </summary>
        internal bool IsReplenishLessonData { get { return this._replenishLessonList.Any(); } }



        #region GetNormalLessonList 获取要创建的正常的课次集合
        /// <summary>
        /// 描述：获取要创建的正常的课次集合
        /// <para>作者：瞿琦</para>
        /// <para>创建时间:2019-3-15</para>
        /// </summary>
        /// <returns>创建的正常的课次集合</returns>
        private List<TblTimLesson> GetNormalLessonList()
        {
            var normalLessonList = _lessonList.Where(x => x.Status != (int)LessonBusinessType.RepairLesson
                                              && x.Status != (int)LessonBusinessType.AdjustLessonReplenishWeek
                                              && x.Status != (int)LessonBusinessType.AdjustLessonChange).ToList();

            return normalLessonList;
        }
        #endregion

        #region GetLessonCreatorInfo 获取要创建的新课次（正常的）
        /// <summary>
        /// 描述：获取要创建的新课次（正常的）
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-8</para>
        /// </summary>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常Id:44,异常描述：找不到课次调整业务Id
        /// 异常Id:45,异常描述：教师上课时间冲突
        /// 异常Id:54,异常描述：学生上课时间冲突
        /// </exception>
        /// <returns>要创建的课次列表</returns>
        public List<LessonCreatorInfo> GetLessonCreatorInfo()
        {
            var viewTimAttendLessonRepository = new ViewTimAttendLessonRepository();
            //获取某天的已排考勤课次
            var attendLessonList = viewTimAttendLessonRepository.GetClassDateTimAttendLessonList(_normalLessonList.FirstOrDefault().SchoolId, _adjustDate);


            //验证要创建的课次是否冲突
            this.ValidationLesson(_normalLessonList, attendLessonList);

            var result = new List<LessonCreatorInfo>();
            foreach (var item in _normalLessonList)
            {
                var adjustLessonEntity = _adjustLessonList.FirstOrDefault(k => k.FromLessonId == item.LessonId);
                if (adjustLessonEntity == null)  //如果课次调整信息为空，则提示找不到课次调整信息
                {
                    throw new BussinessException((byte)ModelType.Timetable, 44);
                }
                var lessonEnity = new LessonCreatorInfo
                {
                    BusinessId = adjustLessonEntity.AdjustLessonId,
                    BusinessType = this.BusinessType,
                    EnrollOrderItemId = item.EnrollOrderItemId,
                    ClassBeginTime = item.ClassBeginTime,
                    ClassEndTime = item.ClassEndTime,
                    ClassDate = _adjustDate,           //调整后的日期
                    ClassId = item.ClassId,
                    ClassRoomId = item.ClassRoomId,
                    CourseId = item.CourseId,
                    CourseLevelId = item.CourseLevelId,
                    LessonCount = item.LessonCount,
                    LessonType = (LessonType)item.LessonType,
                    SchoolId = item.SchoolId,
                    StudentId = item.StudentId,
                    TeacherId = item.TeacherId,
                    TermId = item.TermId
                };
                result.Add(lessonEnity);
            }
            return result;
        }
        #endregion

        #region GetRelenishLessonList 获取要创建的补课调课的课次集合
        /// <summary>
        /// 描述：获取要创建的补课调课的课次集合
        /// <para>作者：瞿琦</para>
        /// <para>创建时间:2019-3-15</para>
        /// </summary>
        /// <returns>创建的补课调课的课次集合</returns>
        private List<TblTimLesson> GetRelenishLessonList()
        {
            var replenishLessonList = _lessonList.Where(x => x.Status == (int)LessonBusinessType.RepairLesson
                                              && x.Status == (int)LessonBusinessType.AdjustLessonReplenishWeek
                                              && x.Status == (int)LessonBusinessType.AdjustLessonChange).ToList();

            return replenishLessonList;
        }
        #endregion

        #region GetReplenishLessonCreatorInfo 获取要创建的新课次（补课、调课、补课周）
        /// <summary>
        /// 描述：获取要创建的新课次（补课、调课、补课周）
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-14</para>
        /// </summary>
        /// <returns>补课、调课、补课周要创建的课次</returns>
        public List<ReplenishLessonCreatorInfo> GetReplenishLessonCreatorInfo()
        {

            var viewTimAttendLessonRepository = new ViewTimAttendLessonRepository();
            //获取某天的已排考勤课次
            var attendLessonList = viewTimAttendLessonRepository.GetClassDateTimAttendLessonList(_replenishLessonList.FirstOrDefault().SchoolId, _adjustDate);
            //验证要创建的课次是否冲突
            this.ValidationLesson(_replenishLessonList, attendLessonList);

            var result = new List<ReplenishLessonCreatorInfo>();
            foreach (var item in _replenishLessonList)
            {
                var adjustLessonEntity = _adjustLessonList.FirstOrDefault(k => k.FromLessonId == item.LessonId);
                if (adjustLessonEntity == null)  //如果课次调整信息为空，则提示找不到课次调整信息
                {
                    throw new BussinessException((byte)ModelType.Timetable, 44);
                }

                var lessonEnity = new ReplenishLessonCreatorInfo
                {
                    OutLessonId = item.LessonId,
                    BusinessId = adjustLessonEntity.AdjustLessonId,
                    BusinessType = (LessonBusinessType)this.BusinessType,
                    EnrollOrderItemId = item.EnrollOrderItemId,
                    ClassBeginTime = item.ClassBeginTime,
                    ClassEndTime = item.ClassEndTime,
                    ClassDate = _adjustDate,           //调整后的日期
                    ClassId = item.ClassId,
                    ClassRoomId = item.ClassRoomId,
                    CourseId = item.CourseId,
                    CourseLevelId = item.CourseLevelId,
                    LessonCount = item.LessonCount,
                    LessonType = (LessonType)item.LessonType,
                    SchoolId = item.SchoolId,
                    StudentId = item.StudentId,
                    TeacherId = item.TeacherId,
                    TermId = item.TermId
                };
                result.Add(lessonEnity);
            }
            return result;
        }
        #endregion

        #region ValidationLesson 验证要创建的课次是否冲突
        /// <summary>
        /// 描述：验证要创建的课次是否冲突
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-14</para>
        /// </summary>
        /// <param name="lessonList">要创建的课次</param>
        /// <param name="attendLessonList">调整日期当天的所有课次</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常Id:45,异常描述：教师上课时间冲突
        /// 异常Id:54,异常描述：学生上课时间冲突
        /// </exception>
        private void ValidationLesson(List<TblTimLesson> lessonList, List<ViewTimAttendLesson> attendLessonList)
        {
            var teacherIdList = lessonList.Select(x => x.TeacherId).Distinct();
            //获取调整日期当天的老师所属课次
            var teacherLessonList = attendLessonList.Where(x => teacherIdList.Contains(x.TeacherId));
            foreach (var itemTeacherLesson in teacherLessonList)
            {
                //获取创建的课次中属于该老师的
                var teacherCreateLessonList = lessonList.Where(x => x.TeacherId.Trim() == itemTeacherLesson.TeacherId.Trim());
                foreach (var itemLesson in teacherCreateLessonList)    //要重新创建的课次
                {
                    ////当前老师所属课次时间
                    //var teacherBeginDate = DateTime.Parse($"{itemTeacherLesson.ClassDate:yyyy-MM-dd} {itemTeacherLesson.ClassBeginTime}");
                    //var teacherEndDate = DateTime.Parse($"{itemTeacherLesson.ClassDate:yyyy-MM-dd} {itemTeacherLesson.ClassEndTime}");
                    ////要更换课次的上课时间
                    //var createBeginDate = DateTime.Parse($"{itemTeacherLesson.ClassDate:yyyy-MM-dd} {itemLesson.ClassBeginTime}");
                    //var createEndDate = DateTime.Parse($"{itemTeacherLesson.ClassDate:yyyy-MM-dd} {itemLesson.ClassEndTime}");

                    //if ((createBeginDate >= teacherBeginDate && createBeginDate <= teacherEndDate) || (createEndDate >= teacherBeginDate && createEndDate <= teacherEndDate))
                    //{
                    //    throw new BussinessException((byte)ModelType.Timetable, 10);
                    //}
                    var (adjustBeginDate, adjustEndDate, oldBeginDate, oldEndDate) = this.GetValidationDate(itemTeacherLesson.ClassDate, itemTeacherLesson.ClassBeginTime, itemTeacherLesson.ClassEndTime, itemLesson.ClassBeginTime, itemLesson.ClassEndTime);
                    if ((oldBeginDate >= adjustBeginDate && oldBeginDate <= adjustEndDate) || (oldEndDate >= adjustBeginDate && oldEndDate <= adjustEndDate))
                    {
                        throw new BussinessException((byte)ModelType.Timetable, 10);
                    }
                }
            }

            var studentIdList = lessonList.Select(x => x.StudentId).Distinct();
            //获取调整日期当天的学生所属课次
            var studentLessonList = attendLessonList.Where(x => studentIdList.Contains(x.StudentId));
            foreach (var itemStudentLesson in studentLessonList)
            {
                //获取学生要创建的课次
                var studentCreateLessonList = lessonList.Where(x => x.StudentId == itemStudentLesson.StudentId);
                foreach (var itemLesson in studentCreateLessonList)
                {
                    ////当前老师所属课次时间
                    //var studentBeginDate = DateTime.Parse($"{itemStudentLesson.ClassDate:yyyy-MM-dd} {itemStudentLesson.ClassBeginTime}");
                    //var studentEndDate = DateTime.Parse($"{itemStudentLesson.ClassDate:yyyy-MM-dd} {itemStudentLesson.ClassEndTime}");
                    ////要创建课次的上课时间
                    //var createBeginDate = DateTime.Parse($"{itemStudentLesson.ClassDate:yyyy-MM-dd} {itemLesson.ClassBeginTime}");
                    //var createEndDate = DateTime.Parse($"{itemStudentLesson.ClassDate:yyyy-MM-dd} {itemLesson.ClassEndTime}");

                    //if ((createBeginDate >= studentBeginDate && createBeginDate <= studentEndDate) || (createEndDate >= studentBeginDate && createEndDate <= studentEndDate))
                    //{
                    //    throw new BussinessException((byte)ModelType.Timetable, 54);
                    //}
                    var (adjustBeginDate, adjustEndDate, oldBeginDate, oldEndDate) = this.GetValidationDate(itemStudentLesson.ClassDate, itemStudentLesson.ClassBeginTime, itemStudentLesson.ClassEndTime, itemLesson.ClassBeginTime, itemLesson.ClassEndTime);
                    if ((oldBeginDate >= adjustBeginDate && oldBeginDate <= adjustEndDate) || (oldEndDate >= adjustBeginDate && oldEndDate <= adjustEndDate))
                    {
                        throw new BussinessException((byte)ModelType.Timetable, 54);
                    }

                }
            }

            
        }
        /// <summary>
        /// 描述：拼接时间段，返回多个时间
        /// <para>作者:瞿琦</para>
        /// <para>创建时间：2019-3-22</para>
        /// </summary>
        /// <param name="adjustDate">调整日期</param>
        /// <param name="adjustBeginTime">调整日期当天的开始时间段</param>
        /// <param name="adjustEndTime">调整日期当天的结束时间段</param>
        /// <param name="oldBeginTime">要创建课次的开始时间段</param>
        /// <param name="oldEndTime">要创建课次的结束时间段</param>
        /// <returns>上课时间</returns>
        private (DateTime adjustBeginDate, DateTime adjustEndDate, DateTime oldBeginDate, DateTime oldEndDate) GetValidationDate(DateTime adjustDate,string adjustBeginTime, string adjustEndTime, string oldBeginTime, string oldEndTime)
        {
            //当前日期所属课次时间
            var adjustBeginDate = DateTime.Parse($"{adjustDate:yyyy-MM-dd} {adjustBeginTime}");
            var adjustEndDate = DateTime.Parse($"{adjustDate:yyyy-MM-dd} {adjustEndTime}");
            //要创建课次的上课时间
            var oldBeginDate = DateTime.Parse($"{adjustDate:yyyy-MM-dd} {oldBeginTime}");
            var oldEndDate = DateTime.Parse($"{adjustDate:yyyy-MM-dd} {oldEndTime}");
            return (adjustBeginDate, adjustEndDate, oldBeginDate, oldEndDate);
        }
        #endregion

    }
}
