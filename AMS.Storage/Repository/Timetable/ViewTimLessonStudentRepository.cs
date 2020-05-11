using System;
using System.Collections.Generic;
using System.Linq;
using AMS.Dto;
using AMS.Storage.Models;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 学生课次仓储
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-02-21</para>
    /// </summary>
    public class ViewTimLessonStudentRepository : BaseRepository<ViewTimLessonStudent>
    {
        /// <summary>
        /// 学生课次仓储
        /// </summary>
        /// <param name="context">上下文</param>
        public ViewTimLessonStudentRepository(DbContext context) : base(context)
        {

        }

        /// <summary>
        /// 学生课次仓储
        /// </summary>
        public ViewTimLessonStudentRepository()
        {

        }

        #region SQL 基础查询 (补课和调课合并的课次信息) 
        const string SQL = @"
SELECT A.AttendStatus,
       A.StudentId,
       A.SchoolId,
       A.AdjustType,
       A.AttendDate,
       A.CreateTime,
       A.LessonStudentId,
       B.ClassBeginTime,
       B.ClassDate,
       B.ClassEndTime,
       B.ClassId,
       B.ClassRoomId,
       B.CourseId,
       B.CourseLevelId,
       B.EnrollOrderItemId,
       B.LessonCount,
       B.LessonId,
       B.LessonType,
       B.Status,
       B.TeacherId,
       B.TermId,
       B.BusinessId,
       B.BusinessType
FROM (select AttendStatus,StudentId,SchoolId,AdjustType,AttendDate,CreateTime,LessonStudentId,LessonId from [dbo].[TblTimLessonStudent] where AttendStatus=0 and AdjustType=0
          union all
          select AttendStatus,StudentId,SchoolId,AdjustType,AttendDate,CreateTime,ReplenishLessonId as LessonStudentId,LessonId from [dbo].[TblTimReplenishLesson] where AttendStatus=0 and AdjustType=0
         )  AS A
-----TblTimLessonStudent AS A
    INNER JOIN dbo.TblTimLesson AS B
        ON B.LessonId = A.LessonId
";
       

        /// <summary>
        /// 构建一个基础查询
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-03-15</para> 
        /// </summary>
        private IQueryable<ViewTimLessonStudent> BuildBasisQueryable
        {
            get
            {
                var queryable = base.CurrentContext
                    .ViewTimLessonStudent
                    .FromSql<ViewTimLessonStudent>(SQL)
                    .AsNoTracking()
                    .AsQueryable();

                return queryable;
            }
        }
        #endregion

        #region BuildBasisQueryable 构建基础查询 (不包含补课信息)
        private const string querySql = @"SELECT A.AttendStatus,
       A.StudentId,
       A.SchoolId,
       A.AdjustType,
       A.AttendDate,
       A.CreateTime,
       A.LessonStudentId,
       B.ClassBeginTime,
       B.ClassDate,
       B.ClassEndTime,
       B.ClassId,
       B.ClassRoomId,
       B.CourseId,
       B.CourseLevelId,
       B.EnrollOrderItemId,
       B.LessonCount,
       B.LessonId,
       B.LessonType,
       B.Status,
       B.TeacherId,
       B.TermId,
       B.BusinessId,
       B.BusinessType
FROM TblTimLessonStudent AS A
    INNER JOIN dbo.TblTimLesson AS B
        ON B.LessonId = A.LessonId";

        /// <summary>
        /// 描述：获取基础课次
        /// <para>作者：唐志伟</para>
        /// <para>创建时间：2019-3-19</para>
        /// </summary>
        private IQueryable<ViewTimLessonStudent> StudentBasisQueryable
        {
            get
            {
                var queryable = base.CurrentContext
                    .ViewTimLessonStudent
                    .FromSql<ViewTimLessonStudent>(querySql)
                    .AsNoTracking()
                    .AsQueryable();

                return queryable;
            }
        }
        #endregion

        /// <summary>
        /// 获取学生课次信息列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-21</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        /// <returns>学生课次列表</returns>
        public List<ViewTimLessonStudent> GetStudentLessonList(string schoolId, long studentId)
        {
            List<ViewTimLessonStudent> res = this.BuildBasisQueryable
                        .Where(x => x.SchoolId == schoolId && x.StudentId == studentId)
                        .ToList();

            return res;
        }

        /// <summary>
        /// 获取学生课次信息列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-21</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        /// <param name="enrollOrderItemId">报名订单课程明细Id</param>
        /// <returns>学生课次列表</returns>
        public List<ViewTimLessonStudent> GetStudentLessonList(string schoolId, long studentId, long enrollOrderItemId)
        {
            List<ViewTimLessonStudent> res = this.BuildBasisQueryable
                        .Where(x =>
                               x.SchoolId == schoolId &&
                               x.StudentId == studentId &&
                               x.EnrollOrderItemId == enrollOrderItemId)
                        .OrderBy(x => x.ClassDate).ThenBy(x => x.ClassBeginTime)
                        .ToList();

            return res;
        }

        /// <summary>
        /// 获取学生转出班级课次列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-22</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        /// <param name="classId">班级Id</param>
        /// <param name="stopTime">停课日期</param>
        /// <param name="lessonTypes">LessonType课次类型枚举值</param>
        /// <returns>学生课次信息列表</returns>
        public List<ViewTimLessonStudent> GetStudentTransferOutClassLessonList(
            string schoolId, long studentId, long classId, DateTime stopTime, List<int> lessonTypes)
        {
            IQueryable<ViewTimLessonStudent> query = this.QueryStudentTransferOutClassLesson(
                schoolId, studentId, classId, stopTime, lessonTypes);

            return query.ToList();
        }

        /// <summary>
        /// 获取学生转出班级课次数
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-22</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        /// <param name="classId">班级Id</param>
        /// <param name="stopTime">停课日期</param>
        /// <param name="lessonTypes">LessonType课次类型枚举值</param>
        /// <returns>转出课次数量</returns>
        public int GetStudentTransferOutClassLessonCount(
            string schoolId, long studentId, long classId, DateTime stopTime, List<int> lessonTypes)
        {
            IQueryable<ViewTimLessonStudent> query = this.QueryStudentTransferOutClassLesson(
                schoolId, studentId, classId, stopTime, lessonTypes);

            return query.Sum(x => x.LessonCount);
        }

        /// <summary>
        /// 学生转出课次查询构造
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-22</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        /// <param name="classId">班级Id</param>
        /// <param name="stopTime">停课日期</param>
        /// <param name="lessonTypes">LessonType课次类型枚举值</param>
        /// <returns>构造条件</returns>
        private IQueryable<ViewTimLessonStudent> QueryStudentTransferOutClassLesson(
            string schoolId,
            long studentId,
            long classId,
            DateTime stopTime,
            List<int> lessonTypes)
        {
            return this.BuildBasisQueryable
                       .Where(x =>
                              x.SchoolId == schoolId &&
                              x.StudentId == studentId &&
                              x.ClassId == classId &&
                              x.ClassDate >= stopTime &&
                              lessonTypes.Contains(x.LessonType));
        }


        /// <summary>
        /// 获取学生转出课次数
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-22</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学期Id</param>
        /// <param name="stopTime">停课日期</param>
        /// <returns>学生转出课次数</returns>
        public List<LeaveSchoolLessonResponse> GetStudentTransferOutLessonCount(
            string schoolId, long studentId, DateTime stopTime)
        {
            IQueryable<ViewTimLessonStudent> query = QueryStudentTransferOut(schoolId, studentId, stopTime);

            var res = query.GroupBy(g => g.EnrollOrderItemId)
                            .Select(m => new LeaveSchoolLessonResponse
                            {
                                EnrollOrderItemId = m.Key,
                                Count = m.Sum(x => x.LessonCount)
                            })
                            .ToList();

            return res;
        }

        /// <summary>
        /// 获取学生转出课次列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-22</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学期Id</param>
        /// <param name="stopTime">停课日期</param>
        /// <returns>学生转出课次列表</returns>
        public List<ViewTimLessonStudent> GetStudentTransferOutLessonList(string schoolId, long studentId, DateTime stopTime)
        {
            IQueryable<ViewTimLessonStudent> query = QueryStudentTransferOut(schoolId, studentId, stopTime);

            return query.ToList();
        }

        /// <summary>
        /// 学生转出课次查询构造器
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-22</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学期Id</param>
        /// <param name="stopTime">停课日期</param>
        /// <returns>学生转出课次查询条件</returns>
        private IQueryable<ViewTimLessonStudent> QueryStudentTransferOut(
            string schoolId, long studentId, DateTime stopTime)
        {
            return this.BuildBasisQueryable
                       .Where(x =>
                              x.SchoolId == schoolId &&
                              x.StudentId == studentId &&
                              x.ClassDate >= stopTime);
        }

        #region GetLessonsByTimeDuration 获取某时间段课次列表数据 
        /// <summary>
        /// 根据某时间段获取常规课课次列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="schoolId">校区ID</param>
        /// <param name="studentId">学生Id</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>某时间段常规课课次列表</returns>
        public List<ViewTimLessonStudent> GetLessonsByTimeDuration(string schoolId, long studentId, DateTime beginDate, DateTime endDate)
        {
            return this.StudentBasisQueryable
                .Where(x => x.SchoolId == schoolId && x.StudentId == studentId
                && x.ClassDate >= beginDate
                && x.ClassDate <= endDate)
                .ToList();
        }
        #endregion

        #region GetLessonsByClassDate 根据上课日期获取常规课课次列表 
        /// <summary>
        /// 根据上课日期获取常规课课次列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="schoolId">校区ID</param>
        /// <param name="studentId">学生Id</param>
        /// <param name="classDate">上课日期</param>
        /// <returns>某时间段常规课课次列表</returns>
        public List<ViewTimLessonStudent> GetLessonsByClassDate(string schoolId, long studentId, DateTime classDate)
        {
            return this.StudentBasisQueryable
                .Where(x => x.SchoolId == schoolId && x.StudentId == studentId && x.ClassDate == classDate)
                .ToList();
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
        public ViewTimLessonStudent GetByLessonId(long lesssonId)
        {
            return this.StudentBasisQueryable.FirstOrDefault(x => x.LessonId == lesssonId);
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
        public List<ViewTimLessonStudent> GetLessonList(string schoolId, long classId, DateTime date, long studentId)
        {
            return this.StudentBasisQueryable
                .Where(x => x.SchoolId == schoolId && x.StudentId == studentId && x.ClassId == classId && x.ClassDate == date)
                .ToList();
        }

        #endregion
    }
}
