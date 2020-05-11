using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Dto;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 学生安排了调课、补课仓储
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-11</para>
    /// </summary>
    public class ViewTimReplenishLessonStudentRepository : BaseRepository<ViewTimReplenishLessonStudent>
    {
        /// <summary>
        /// 学生课次仓储
        /// </summary>
        /// <param name="context">上下文</param>
        public ViewTimReplenishLessonStudentRepository(DbContext context) : base(context)
        {

        }

        /// <summary>
        /// 学生课次仓储
        /// </summary>
        public ViewTimReplenishLessonStudentRepository()
        {

        }

        #region 构建课次查询器 QueryableViewTimLessonStudent

        /// <summary>
        /// 构建课次查询器
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <returns>学生课次构造条件</returns>
        private IQueryable<ViewTimReplenishLessonStudent> QueryableViewTimLessonStudent()
        {
            return (from a in base.CurrentContext.TblTimReplenishLesson
                    join b in base.CurrentContext.TblTimLesson on a.LessonId equals b.LessonId
                    select new ViewTimReplenishLessonStudent
                    {
                        AttendStatus = a.AttendStatus,
                        BusinessId = b.BusinessId,
                        StudentId = a.StudentId,
                        SchoolId = a.SchoolId,
                        BusinessType = b.BusinessType,
                        ClassBeginTime = b.ClassBeginTime,
                        ClassDate = b.ClassDate,
                        ClassEndTime = b.ClassEndTime,
                        ClassId = b.ClassId,
                        ClassRoomId = b.ClassRoomId,
                        CourseId = b.CourseId,
                        CourseLevelId = b.CourseLevelId,
                        EnrollOrderItemId = b.EnrollOrderItemId,
                        LessonCount = b.LessonCount,
                        LessonId = b.LessonId,
                        LessonStudentId = a.ReplenishLessonId,
                        LessonType = b.LessonType,
                        Status = b.Status,
                        TeacherId = b.TeacherId,
                        TermId = b.TermId,
                        AdjustType = a.AdjustType,
                        AttendDate = a.AttendDate,
                        ParentLessonId = a.ParentLessonId,
                        RootLessonId = a.RootLessonId,
                        ReplenishCreateTime = a.CreateTime
                    });
        }
        #endregion

        #region 获取课次列表数据 GetLessonList
        /// <summary>
        /// 获取课次列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="schoolId">校区ID</param>
        /// <param name="classId">班级Id</param>
        /// <param name="date">日期</param>
        /// <param name="lessonType">课次类型</param>
        /// <param name="adjustTypes">调整状态</param>
        /// <returns>课次列表</returns>
        public List<ViewTimReplenishLessonStudent> GetLessonList(
            string schoolId, long classId, DateTime date, LessonType lessonType,
            List<int> adjustTypes)
        {

            return this.QueryableViewTimLessonStudent()
                .Where(x => x.SchoolId == schoolId && x.ClassId == classId &&
                x.ClassDate == date && x.LessonType == (int)lessonType &&
                adjustTypes.Contains(x.AdjustType))
                .ToList();
        }
        #endregion

        #region 获取某时间段课次列表数据 GetReplenishLessonsByTimeDuration
        /// <summary>
        /// 根据获取课次列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="schoolId">校区ID</param>
        /// <param name="studentId">学生Id</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>某时间段补课/调课课次列表</returns>
        public List<ViewTimReplenishLessonStudent> GetReplenishLessonsByTimeDuration(string schoolId, long studentId, DateTime beginDate, DateTime endDate)
        {
            return this.QueryableViewTimLessonStudent()
                .Where(x => x.SchoolId == schoolId && x.StudentId == studentId
                && x.ClassDate >= beginDate
                && x.ClassDate <= endDate)
                .ToList();
        }
        #endregion

        #region 根据上课日期获取补课/调课课次列表 GetReplenishLessonsByClassDate
        /// <summary>
        /// 根据上课日期获取补课/调课课次列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="schoolId">校区ID</param>
        /// <param name="studentId">学生Id</param>
        /// <param name="classDate">上课日期</param>
        /// <returns>补课/调课课次列表</returns>
        public List<ViewTimReplenishLessonStudent> GetReplenishLessonsByClassDate(string schoolId, long studentId, DateTime classDate)
        {
            return this.QueryableViewTimLessonStudent()
                .Where(x => x.SchoolId == schoolId && x.StudentId == studentId && x.ClassDate == classDate)
                .ToList();
        }
        #endregion

        #region 获取所有RootLessonId下的调整课次信息

        /// <summary>
        /// 获取根课次下面的所有课次信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="rootLessonId">根课次ID</param>
        /// <returns>根课次下面的所有课次信息列表</returns>
        public List<ViewTimReplenishLessonStudent> GetLessonListByRootLessonId(long rootLessonId)
        {
            return this.QueryableViewTimLessonStudent().Where(x => x.RootLessonId == rootLessonId).ToList();
        }

        /// <summary>
        /// 获取父课次下面的所有课次信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="lessonId">课次ID</param>
        /// <returns>根课次下面的所有课次信息列表</returns>
        public ViewTimReplenishLessonStudent GetLessonListByParentLessonId(long lessonId)
        {
            return this.QueryableViewTimLessonStudent().Where(x => x.ParentLessonId == lessonId).FirstOrDefault();
        }

        /// <summary>
        /// 获取父课次下面的所有课次信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="lessonId">课次ID</param>
        /// <returns>根课次下面的所有课次信息列表</returns>
        public List<ViewTimReplenishLessonStudent> GetLessonListByParentLessonId(IEnumerable<long> lessonId)
        {
            return this.QueryableViewTimLessonStudent().Where(x => lessonId.Contains(x.ParentLessonId)).ToList();
        }

        /// <summary>
        /// 根据根课程Id集合获取根课次下面的所有课次信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="rootLessonIdList">根课次ID集合</param>
        /// <returns>根课次下面的所有课次信息列表</returns>
        public async Task<List<ViewTimReplenishLessonStudent>> GetLessonListByRootLessonIdAsync(IEnumerable<long> rootLessonIdList)
        {
            return await this.QueryableViewTimLessonStudent().Where(x => rootLessonIdList.Contains(x.RootLessonId)).ToListAsync();
        }
        #endregion

        #region 获取课次信息 GetByLessonId

        /// <summary>
        /// 获取课次信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="lesssonId">课次ID</param>
        /// <returns>课次信息</returns>
        public ViewTimReplenishLessonStudent GetByLessonId(long lesssonId)
        {
            return this.QueryableViewTimLessonStudent().FirstOrDefault(x => x.LessonId == lesssonId);
        }

        #endregion

        #region 获取课次信息 GetByLessonId

        /// <summary>
        /// 获取课次信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="schoolId">课次ID</param>
        /// <param name="classId">班级ID</param>
        /// <param name="date">上课日期</param>
        /// <param name="studentId">学生ID</param>
        /// <returns>课次信息</returns>
        public List<ViewTimReplenishLessonStudent> GetLessonList(string schoolId, long classId, DateTime date, long studentId)
        {
            return this.QueryableViewTimLessonStudent()
                .Where(x => x.SchoolId == schoolId && x.StudentId == studentId && x.ClassId == classId && x.ClassDate == date)
                .ToList();
        }

        #endregion
    }
}
