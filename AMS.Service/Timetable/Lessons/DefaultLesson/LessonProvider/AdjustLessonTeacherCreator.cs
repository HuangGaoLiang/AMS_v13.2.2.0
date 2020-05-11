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
    /// 描述：老师代课生产者
    /// <para>作者：瞿琦</para>
    /// <para>创建时间：2019-3-8</para>
    /// </summary>
    public class AdjustLessonTeacherCreator : AdjustLessonTeacherProvider, ILessonCreator, IReplenishLessonCreator
    {
        private readonly string _teacherId;                          //要更换的老师Id
        private readonly List<TblTimAdjustLesson> _adjustLessonList; //课次调整业务表信息
        private readonly List<TblTimLesson> _lessonList;             //要更换老师的总的课次信息
        private readonly List<TblTimLesson> _normalLessonList;       //正常课次要创建的课次集合
        private readonly List<TblTimLesson> _replenishLessonList;    //补课调课要创建的课次集合


        /// <summary>
        /// 描述：实例化一个老师代课生产者
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-8</para>
        /// </summary>
        /// <param name="teacherId">要更换的老师Id</param> 
        /// <param name="adjustLessonList">课次调整业务表的Id</param>
        /// <param name="lessonList">要更换老师的课次信息</param>
        public AdjustLessonTeacherCreator(string teacherId, List<TblTimAdjustLesson> adjustLessonList, List<TblTimLesson> lessonList)
        {
            _teacherId = teacherId;
            _adjustLessonList = adjustLessonList;
            _lessonList = lessonList;
            _normalLessonList = this.GetNormalLessonList();
            _replenishLessonList = this.GetRelenishLessonList();
        }
        /// <summary>
        /// 业务类型
        /// </summary>
        public override int BusinessType { get => (int)LessonBusinessType.AdjustLessonTeacher; set { } }

        /// <summary>
        /// 正常课次是否有数据
        /// </summary>
        internal bool IsLessonData { get { return this._normalLessonList.Any(); } }
        /// <summary>
        /// 补课调课课次是否有数据
        /// </summary>
        internal bool IsReplenishLessonData { get { return this._replenishLessonList.Any(); } }

        /// <summary>
        /// 暂不实现
        /// </summary>
        public void AfterLessonCreate()
        {

        }

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

        #region GetLessonCreatorInfo  获取老师代课要重新生成的课次（正常的）
        /// <summary>
        /// 描述：获取老师代课要重新生成的课次（正常的）
        /// <para>作者:瞿琦</para>
        /// <para>创建时间：2019-3-8</para>
        /// </summary>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：44，异常描述：找不到课次调整业务Id
        /// 异常ID：45，异常描述：教师上课时间冲突
        /// </exception>
        /// <returns>重新生成的课次的列表</returns>
        public List<LessonCreatorInfo> GetLessonCreatorInfo()
        {
            //验证：教师上课时间冲突
            var viewTimAttendLessonRepository = new ViewTimAttendLessonRepository();
            //获取更换老师的已排课次
            var attendLessonList = viewTimAttendLessonRepository.GetTeacherTimAttendLessonList(_normalLessonList.FirstOrDefault().SchoolId, _teacherId);
            //验证要创建的课次是否冲突
            this.ValidationLesson(attendLessonList, _normalLessonList);
            var result = new List<LessonCreatorInfo>();
            foreach (var item in _normalLessonList)
            {
                var adjustLessonEntity = _adjustLessonList.FirstOrDefault(k => k.FromLessonId == item.LessonId);
                if (adjustLessonEntity == null)  //如果课次调整信息为空，则提示找不到课次调整信息
                {
                    throw new BussinessException((byte)ModelType.Timetable, 44);
                }
                var lessonEntity = new LessonCreatorInfo
                {
                    BusinessId = adjustLessonEntity.AdjustLessonId,
                    BusinessType = this.BusinessType,
                    EnrollOrderItemId = item.EnrollOrderItemId,
                    ClassBeginTime = item.ClassBeginTime,
                    ClassEndTime = item.ClassEndTime,
                    ClassDate = item.ClassDate,
                    ClassId = item.ClassId,
                    ClassRoomId = item.ClassRoomId,
                    CourseId = item.CourseId,
                    CourseLevelId = item.CourseLevelId,
                    LessonCount = item.LessonCount,
                    LessonType = (LessonType)item.LessonType,
                    SchoolId = item.SchoolId,
                    StudentId = item.StudentId,
                    TeacherId = _teacherId,
                    TermId = item.TermId
                };
                result.Add(lessonEntity);
            }
            
            return result;
        }
        #endregion

       

        /// <summary>
        /// 暂不实现
        /// </summary>
        public void AfterReplenishLessonCreate()
        {

        }

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

        #region GetReplenishLessonCreatorInfo 获取老师代课要重新生成的课次（补课、调课、补课周）
        /// <summary>
        /// 描述：获取老师代课要重新生成的课次（补课、调课、补课周）
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-14</para>
        /// </summary>
        /// <returns>补课、调课、补课周要创建的课次</returns>
        public List<ReplenishLessonCreatorInfo> GetReplenishLessonCreatorInfo()
        {
            var viewTimAttendLessonRepository = new ViewTimAttendLessonRepository();
            var attendLessonList = viewTimAttendLessonRepository.GetTeacherTimAttendLessonList(_replenishLessonList.FirstOrDefault().SchoolId, _teacherId);

            //验证要创建的课次是否冲突
            this.ValidationLesson(attendLessonList, _replenishLessonList);

            var result = new List<ReplenishLessonCreatorInfo>();
            foreach (var item in _replenishLessonList)
            {
                var adjustLessonEntity = _adjustLessonList.FirstOrDefault(k => k.FromLessonId == item.LessonId);
                if (adjustLessonEntity == null)  //如果课次调整信息为空，则提示找不到课次调整信息
                {
                    throw new BussinessException((byte)ModelType.Timetable, 44);
                }
                var lessonEntity = new ReplenishLessonCreatorInfo
                {
                    OutLessonId = item.LessonId,
                    BusinessId = adjustLessonEntity.AdjustLessonId,
                    BusinessType = (LessonBusinessType)this.BusinessType,
                    EnrollOrderItemId = item.EnrollOrderItemId,
                    ClassBeginTime = item.ClassBeginTime,
                    ClassEndTime = item.ClassEndTime,
                    ClassDate = item.ClassDate,
                    ClassId = item.ClassId,
                    ClassRoomId = item.ClassRoomId,
                    CourseId = item.CourseId,
                    CourseLevelId = item.CourseLevelId,
                    LessonCount = item.LessonCount,
                    LessonType = (LessonType)item.LessonType,
                    SchoolId = item.SchoolId,
                    StudentId = item.StudentId,
                    TeacherId = _teacherId,
                    TermId = item.TermId,
                };
                result.Add(lessonEntity);
            }
            return result;
        }
        #endregion

        #region ValidationLesson 验证课次是否冲突
        /// <summary>
        /// 描述：验证课次是否冲突
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-14</para>
        /// </summary>
        /// <param name="attendLessonList">获取要更换老师的所有课次</param>
        /// <param name="lessonList">要生成的课次</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常Id:10,异常描述：教师上课时间冲突
        /// </exception>
        private void ValidationLesson(List<ViewTimAttendLesson> attendLessonList, List<TblTimLesson> lessonList)
        {
            foreach (var itemAttendLesson in attendLessonList)  //更换老师的已排未上的课次
            {
                foreach (var itemLesson in lessonList)         //要创建给当前老师的课次
                {
                    //当前老师所属课次时间
                    var attendBeginDate= DateTime.Parse($"{itemLesson.ClassDate:yyyy-MM-dd} {itemAttendLesson.ClassBeginTime}");
                    var attendEndDate = DateTime.Parse($"{itemLesson.ClassDate:yyyy-MM-dd} {itemAttendLesson.ClassEndTime}");
                    //要更换课次的上课时间
                    var itemBeginDate = DateTime.Parse($"{itemLesson.ClassDate:yyyy-MM-dd} {itemLesson.ClassBeginTime}");
                    var itemEndDate = DateTime.Parse($"{itemLesson.ClassDate:yyyy-MM-dd} {itemLesson.ClassEndTime}");
                    //如果当前老师的已排课次和要添加的课次上课日期和时间冲突，则抛出异常  （后续思考此处是否可以优化）
                    if (itemAttendLesson.ClassDate == itemLesson.ClassDate && ((itemBeginDate>= attendBeginDate&& itemBeginDate<= attendEndDate) ||(itemEndDate >= attendBeginDate && itemEndDate <= attendEndDate)))
                    {
                        throw new BussinessException((byte)ModelType.Timetable, 10);
                    }
                }
            }
        }
        #endregion
    }
}
