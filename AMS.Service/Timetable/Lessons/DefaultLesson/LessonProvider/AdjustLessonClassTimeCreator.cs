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
    /// 描述：班级上课时间课次调整生产者
    /// <para>作者：瞿琦</para>
    /// <para>创建时间：2019-3-8</para>
    /// </summary>
    public class AdjustLessonClassTimeCreator : AdjustLessonClassTimeProvider, ILessonCreator, IReplenishLessonCreator
    {
        private readonly AdjustClassTimeRequest _request;            //要调整的时间段信息
        private readonly List<TblTimAdjustLesson> _adjustLessonList;//课次调整信息表信息
        private readonly List<TblTimLesson> _lessonList;            //要创建的总的课次
        private readonly List<TblTimLesson> _normalLessonList;      //正常课次要创建的课次集合
        private readonly List<TblTimLesson> _replenishLessonList;   //补课调课要创建的课次集合
        private readonly List<TblDatSchoolTime> _classTimeList;     //班级上课时间段集合

        /// <summary>
        /// 描述：实例化一个班级上课时间课次调整生产者
        /// </summary>
        /// <param name="request">要调整的时间段信息</param>
        /// <param name="adjustLessonList">课次调整信息表信息</param>
        /// <param name="lessonList">要销毁的课次</param>
        /// <param name="classTimeList">班级上课时间段</param>
        public AdjustLessonClassTimeCreator(AdjustClassTimeRequest request, List<TblTimAdjustLesson> adjustLessonList, List<TblTimLesson> lessonList, List<TblDatSchoolTime> classTimeList)
        {
            _request = request;
            _adjustLessonList = adjustLessonList;
            _lessonList = lessonList;
            _normalLessonList = this.GetNormalLessonList();
            _replenishLessonList = this.GetRelenishLessonList();
            _classTimeList = classTimeList;
        }
        /// <summary>
        /// 业务类型
        /// </summary>
        public override int BusinessType { get => (int)LessonBusinessType.AdjustLessonClassTime; set { } }

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
        /// 补课调课课次是否有数据
        /// </summary>
        internal bool IsReplenishLessonData { get { return this._replenishLessonList.Any(); } }




        #region GetNormalLessonList  获取要创建的正常的课次集合
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

        #region GetLessonCreatorInfo 获取班级上课时间课次调整要创建的课次（正常）
        /// <summary>
        /// 描述：获取班级上课时间课次调整要创建的课次（正常）
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-8</para>
        /// </summary>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常Id:44,异常描述：找不到课次调整业务Id
        /// </exception>
        /// <returns>要创建的课次列表</returns>
        public List<LessonCreatorInfo> GetLessonCreatorInfo()
        {
            ////验证：学生上课时间冲突
            var viewTimAttendLessonRepository = new ViewTimAttendLessonRepository();
            //获取某天某个时间段的课次信息
            //var classTimeLessonList = viewTimAttendLessonRepository.GetClassTimeTimAttendLessonList(_normalLessonList.FirstOrDefault().SchoolId, _request.NewClassDate, _request.NewClassBeginTime, ""); //_request.ClassEndTime
            var classTimeLessonList = viewTimAttendLessonRepository.GetClassDateTimAttendLessonList(_normalLessonList.FirstOrDefault().SchoolId, _request.NewClassDate);

            var result = new List<LessonCreatorInfo>();
            var lessonList = from x1 in _normalLessonList
                             join x2 in _adjustLessonList on x1.LessonId equals x2.FromLessonId
                             select new LessonAdjustOutDto
                             {
                                 AdjustLessonId = x2.AdjustLessonId,
                                 EnrollOrderItemId = x1.EnrollOrderItemId,
                                 ClassBeginTime = x2.ClassBeginTime,
                                 ClassEndTime = x2.ClassEndTime,
                                 ClassDate = x2.ClassDate,
                                 ClassId = x1.ClassId,
                                 ClassRoomId = x1.ClassRoomId,
                                 CourseId = x1.CourseId,
                                 CourseLevelId = x1.CourseLevelId,
                                 LessonCount = x1.LessonCount,
                                 LessonType = x1.LessonType,
                                 SchoolId = x1.SchoolId,
                                 StudentId = x1.StudentId,
                                 TeacherId = x1.TeacherId,
                                 TermId = x1.TermId
                             };

            //验证要创建的课次是否冲突
            this.ValidationLesson(lessonList, classTimeLessonList);

            foreach (var item in lessonList)
            {

                var lessonEntity = new LessonCreatorInfo
                {
                    BusinessId = item.AdjustLessonId,
                    BusinessType = this.BusinessType,
                    EnrollOrderItemId = item.EnrollOrderItemId,
                    ClassBeginTime = item.ClassBeginTime,   //调整后的上课时间
                    ClassEndTime = item.ClassEndTime,       //调整后的下课时间
                    ClassDate = item.ClassDate,             //调整后的上课日期 
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
                result.Add(lessonEntity);
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

        #region GetReplenishLessonCreatorInfo 获取班级上课时间课次调整要创建的课次（补课，调课，补课周补课）
        /// <summary>
        /// 描述：获取班级上课时间课次调整要创建的课次（补课，调课，补课周补课）
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-14</para>
        /// </summary>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常Id:44,异常描述：找不到课次调整业务Id
        /// </exception>
        /// <returns>补课，调课，补课周补课课次</returns>
        public List<ReplenishLessonCreatorInfo> GetReplenishLessonCreatorInfo()
        {
            var viewTimAttendLessonRepository = new ViewTimAttendLessonRepository();
            //获取某天某个时间段的课次信息
            var classTimeLessonList = viewTimAttendLessonRepository.GetClassDateTimAttendLessonList(_normalLessonList.FirstOrDefault().SchoolId, _request.NewClassDate);

            var result = new List<ReplenishLessonCreatorInfo>();

            var lessonList = from x1 in _normalLessonList
                             join x2 in _adjustLessonList on x1.LessonId equals x2.FromLessonId
                             select new LessonAdjustOutDto
                             {
                                 AdjustLessonId = x2.AdjustLessonId,
                                 EnrollOrderItemId = x1.EnrollOrderItemId,
                                 ClassBeginTime = x2.ClassBeginTime,
                                 ClassEndTime = x2.ClassEndTime,
                                 ClassDate = x2.ClassDate,
                                 ClassId = x1.ClassId,
                                 ClassRoomId = x1.ClassRoomId,
                                 CourseId = x1.CourseId,
                                 CourseLevelId = x1.CourseLevelId,
                                 LessonCount = x1.LessonCount,
                                 LessonType = x1.LessonType,
                                 SchoolId = x1.SchoolId,
                                 StudentId = x1.StudentId,
                                 TeacherId = x1.TeacherId,
                                 TermId = x1.TermId
                             };
            //验证要创建的课次是否冲突
            this.ValidationLesson(lessonList, classTimeLessonList);

            foreach (var item in lessonList)
            {
                var lessonEntity = new ReplenishLessonCreatorInfo
                {
                    BusinessId = item.AdjustLessonId,
                    BusinessType = (LessonBusinessType)this.BusinessType,
                    EnrollOrderItemId = item.EnrollOrderItemId,
                    ClassBeginTime = item.ClassBeginTime,                    //调整后的上课日期
                    ClassEndTime = item.ClassEndTime,//_request.ClassEndTime, //调整后的下课日期
                    ClassDate = item.ClassDate,             //调整后的上课日期
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
                result.Add(lessonEntity);
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
        /// <param name="classTimeLessonList">调整日期当天的所有课次</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常Id:45,异常描述：教师上课时间冲突
        /// 异常Id:54,异常描述：学生上课时间冲突
        /// </exception>
        private void ValidationLesson(IEnumerable<LessonAdjustOutDto> lessonList, List<ViewTimAttendLesson> classTimeLessonList)
        {
            //获取老师在某个时间段的课次信息
            var teacherIdList = lessonList.Select(x => x.TeacherId).Distinct();
            var teacherLessonList = classTimeLessonList.Where(x => teacherIdList.Contains(x.TeacherId));
            foreach (var itemTeacherLesson in teacherLessonList)  //老师在当前时间段的课次信息
            {
                //获取老师要创建的所属课次
                var teacherCreateLessonList = lessonList.Where(x => x.TeacherId.Trim() == itemTeacherLesson.TeacherId.Trim());
                foreach (var itemLesson in teacherCreateLessonList)     //要创建的课次
                {
                    ////当前老师所属课次时间
                    //var teacherBeginDate = DateTime.Parse($"{itemTeacherLesson.ClassDate:yyyy-MM-dd} {itemTeacherLesson.ClassBeginTime}");
                    //var teacherEndDate = DateTime.Parse($"{itemTeacherLesson.ClassDate:yyyy-MM-dd} {itemTeacherLesson.ClassEndTime}");
                    ////要更换课次的上课时间
                    //var createBeginDate = DateTime.Parse($"{itemTeacherLesson.ClassDate:yyyy-MM-dd} {itemLesson.ClassBeginTime}");
                    //var createEndDate = DateTime.Parse($"{itemTeacherLesson.ClassDate:yyyy-MM-dd} {itemLesson.ClassEndTime}");

                    //if (itemTeacherLesson.ClassDate == itemLesson.ClassDate && ((createBeginDate >= teacherBeginDate && createBeginDate <= teacherEndDate) || (createEndDate >= teacherBeginDate && createEndDate <= teacherEndDate)))
                    //{
                    //    throw new BussinessException((byte)ModelType.Timetable, 10);
                    //}

                    var (adjustBeginDate, adjustEndDate, oldBeginDate, oldEndDate) = this.GetValidationDate(itemTeacherLesson.ClassDate, itemTeacherLesson.ClassBeginTime, itemTeacherLesson.ClassEndTime, itemLesson.ClassBeginTime, itemLesson.ClassEndTime);
                    if (itemTeacherLesson.ClassDate == itemLesson.ClassDate && ((oldBeginDate >= adjustBeginDate || oldBeginDate <= adjustEndDate) || (oldEndDate >= adjustBeginDate || oldEndDate <= adjustEndDate)))
                    {
                        throw new BussinessException((byte)ModelType.Timetable, 54);
                    }
                }
            }

            var studentIdList = lessonList.Select(x => x.StudentId).Distinct();
            //获取学生在某个时间段的课次信息
            var studentLessonList = classTimeLessonList.Where(x => studentIdList.Contains(x.StudentId));
            foreach (var itemStudentLesson in studentLessonList)    //学生在当前时间段的课次信息
            {
                //获取学生要创建的课次
                var studentCreateLessonList = lessonList.Where(x => x.StudentId == itemStudentLesson.StudentId);
                foreach (var itemLesson in studentCreateLessonList)
                {
                    ////当前老师所属课次时间
                    //var studentBeginDate = DateTime.Parse($"{itemStudentLesson.ClassDate:yyyy-MM-dd} {itemStudentLesson.ClassBeginTime}");
                    //var studentEndDate = DateTime.Parse($"{itemStudentLesson.ClassDate:yyyy-MM-dd} {itemStudentLesson.ClassEndTime}");
                    ////要更换课次的上课时间
                    //var createBeginDate = DateTime.Parse($"{itemStudentLesson.ClassDate:yyyy-MM-dd} {itemLesson.ClassBeginTime}");
                    //var createEndDate = DateTime.Parse($"{itemStudentLesson.ClassDate:yyyy-MM-dd} {itemLesson.ClassEndTime}");
                    //if (itemStudentLesson.ClassDate == itemLesson.ClassDate && ((createBeginDate >= studentBeginDate || createBeginDate <= studentEndDate) || (createEndDate >= studentBeginDate || createEndDate <= studentEndDate)))
                    //{
                    //    throw new BussinessException((byte)ModelType.Timetable, 54);
                    //}

                    var (adjustBeginDate, adjustEndDate, oldBeginDate, oldEndDate) = this.GetValidationDate(itemStudentLesson.ClassDate, itemStudentLesson.ClassBeginTime, itemStudentLesson.ClassEndTime, itemLesson.ClassBeginTime, itemLesson.ClassEndTime);
                    if (itemStudentLesson.ClassDate == itemLesson.ClassDate && ((oldBeginDate >= adjustBeginDate || oldBeginDate <= adjustEndDate) || (oldEndDate >= adjustBeginDate || oldEndDate <= adjustEndDate)))
                    {
                        throw new BussinessException((byte)ModelType.Timetable, 54);
                    }
                }
            }
        }
        #endregion

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
        private (DateTime adjustBeginDate, DateTime adjustEndDate, DateTime oldBeginDate, DateTime oldEndDate) GetValidationDate(DateTime adjustDate, string adjustBeginTime, string adjustEndTime, string oldBeginTime, string oldEndTime)
        {
            //当前日期所属课次时间
            var adjustBeginDate = DateTime.Parse($"{adjustDate:yyyy-MM-dd} {adjustBeginTime}");
            var adjustEndDate = DateTime.Parse($"{adjustDate:yyyy-MM-dd} {adjustEndTime}");
            //要创建课次的上课时间
            var oldBeginDate = DateTime.Parse($"{adjustDate:yyyy-MM-dd} {oldBeginTime}");
            var oldEndDate = DateTime.Parse($"{adjustDate:yyyy-MM-dd} {oldEndTime}");
            return (adjustBeginDate, adjustEndDate, oldBeginDate, oldEndDate);
        }
    }
}
