using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using AMS.Dto;
using AMS.Storage.Models;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// ViewCancelMakeLesson仓储
    /// <para>作    者：Huang GaoLiang</para>
    /// <para>创建时间：2019-03-07</para>
    /// </summary>
    public class ViewCancelMakeLessonRepository : BaseRepository<ViewCancelMakeLesson>
    {
        /// <summary>
        /// ViewCancelMakeLesson构造函数
        /// </summary>
        /// <param name="context">上下文</param>
        public ViewCancelMakeLessonRepository(DbContext context) : base(context)
        {

        }

        /// <summary>
        /// ViewCancelMakeLesson构造函数
        /// </summary>
        public ViewCancelMakeLessonRepository()
        {

        }

        /// <summary>
        /// 获取学生课次信息列表
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-07 </para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        /// <param name="termId">学期编号</param>
        /// <param name="courseId">课程编号</param>
        /// <returns>学生课次列表</returns>
        public List<ViewCancelMakeLesson> GetStudentLessonList(string schoolId, long studentId, long termId, long courseId)
        {
            #region sql

            string sql = @"SELECT s.AttendStatus,
                                   l.BusinessType,
                                   s.StudentId,
                                   s.SchoolId,
                                   l.BusinessId,
                                   l.ClassBeginTime,
                                   l.ClassDate,
                                   l.ClassEndTime,
                                   l.ClassId,
                                   c.ClassNo,
                                   l.CourseId,
                                   d.CourseCnName as CourseName,
                                   s.LessonId,
                                   s.LessonStudentId,
                                   l.Status,
                                   l.TeacherId,
                                   l.TermId
                            FROM dbo.TblTimLessonStudent s
                                INNER JOIN dbo.TblTimLesson l ON l.LessonId = s.LessonId
                                INNER JOIN dbo.TblDatClass c ON l.ClassId = c.ClassId
                                INNER JOIN dbo.TblDatCourse d ON d.CourseId = c.CourseId
                                AND s.SchoolId=@SchoolId
	                            AND s.StudentId=@StudentId
	                            AND s.AttendStatus IN(0,2)
                                AND s.AdjustType=0
                          UNION 
                            SELECT s.AttendStatus,
                                    l.BusinessType,
                                    s.StudentId,
                                    s.SchoolId,
                                    l.BusinessId,
                                    l.ClassBeginTime,
                                    l.ClassDate,
                                    l.ClassEndTime,
                                    l.ClassId,
                                    c.ClassNo,
                                    l.CourseId,
                                    d.CourseCnName,
                                    s.LessonId,
                                    s.ReplenishLessonId,
                                    l.Status,
                                    l.TeacherId,
                                    l.TermId
                            FROM dbo.TblTimReplenishLesson s
                                INNER JOIN dbo.TblTimLesson l ON l.LessonId = s.LessonId
                                INNER JOIN dbo.TblDatClass c ON l.ClassId = c.ClassId
                                INNER JOIN dbo.TblDatCourse d ON d.CourseId = c.CourseId
	                            AND s.SchoolId=@SchoolId
	                            AND s.StudentId=@StudentId
	                            AND s.AttendStatus IN(0,2)
                                AND s.AdjustType=0";
            #endregion

            List<ViewCancelMakeLesson> res = base.CurrentContext.ViewCancelMakeLesson.FromSql(sql, new SqlParameter[] {
                new SqlParameter("@SchoolId", schoolId),
                new SqlParameter("@StudentId", studentId)
            })
           .WhereIf(termId > 0, m => m.TermId == termId)
           .WhereIf(courseId > 0, m => m.CourseId == courseId)
           .OrderBy(m => m.ClassDate)
           .ThenBy(m => m.ClassBeginTime)
           .ToList();
            return res;
        }
    }
}
