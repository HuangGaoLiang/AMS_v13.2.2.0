using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using AMS.Dto;
using AMS.Models;
using AMS.Storage.Models;
using Jerrisoft.Platform.Public.PageExtensions;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;


namespace AMS.Storage.Repository
{
    /// <summary>
    /// 学生缺勤视图仓储
    /// </summary>
    public class ViewStudentTimeLessRepository : BaseRepository<ViewStudentTimeLess>
    {
        /// <summary>
        /// 学生缺勤仓储 无参构造函数
        /// </summary>
        public ViewStudentTimeLessRepository()
        {
        }

        /// <summary>
        /// 学生缺勤仓储 构造函数
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public ViewStudentTimeLessRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 根据条件查询补课周补课分页列表
        /// <para>作    者：Huang GaoLiang</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="search">补课周补课分页查询条件</param>
        /// <returns>返回补课周补课分页列表</returns>
        public List<ViewStudentTimeLess> GetStudentTimeLessList(StudentLessonInDto search)
        {
            #region sql

            string querySql = @"SELECT 
	                            ts.LessonId,
                                ts.StudentId,
	                            tl.ClassId,
                                c.ClassNo,
                                c.TeacherId,
                                tl.TermId,
                                tl.CourseId,
	                            tl.EnrollOrderItemId,
	                            tl.CourseLevelId,
                                tl.ClassDate,
                                tl.ClassBeginTime
                        FROM dbo.TblTimLessonStudent ts
                            INNER JOIN TblTimLesson tl ON tl.LessonId = ts.LessonId
                            INNER JOIN dbo.TblDatClass c ON tl.ClassId = c.ClassId
                            INNER JOIN dbo.TblDatTerm t ON tl.TermId = t.TermId
                        WHERE ts.StudentId=@StudentId
	                          AND ts.SchoolId = @SchoolId
                              AND c.TeacherId = @TeacherId
                              AND t.TermId = @TermId
                              AND ts.AttendStatus IN ( 0, 2 )
                              AND ts.AdjustType IN ( 0 )
                              AND tl.ClassDate <= GETDATE()
	                          AND tl.CourseId=@CourseId
                              AND tl.ClassId=@ClassId
                        UNION
                        SELECT tl.LessonId,
                                tl.StudentId,
	                            tl.ClassId,
                                c.ClassNo,
                                c.TeacherId,
                                tl.TermId,
                                tl.CourseId,
	                            tl.EnrollOrderItemId,
	                            tl.CourseLevelId,
                                tl.ClassDate,
                                tl.ClassBeginTime
                        FROM TblTimLesson tl
                            INNER JOIN TblTimReplenishLesson rl ON tl.LessonId = rl.LessonId
                            INNER JOIN dbo.TblDatClass c ON tl.ClassId = c.ClassId
                            INNER JOIN dbo.TblDatTerm t ON tl.TermId = t.TermId
                        WHERE tl.StudentId=@StudentId
	                        AND rl.SchoolId = @SchoolId
                            AND c.TeacherId = @TeacherId
                            AND t.TermId = @TermId
                            AND rl.AttendStatus IN ( 0, 2 )
                            AND rl.AdjustType =0
                            AND tl.ClassDate <= GETDATE()
	                        AND tl.CourseId=@CourseId
                            AND tl.ClassId=@ClassId";
            #endregion

            var studentTimeLess = base.CurrentContext.ViewStudentTimeLess.FromSql(querySql, new SqlParameter[] {
                new SqlParameter("@StudentId", search.StudentId),
                new SqlParameter("@ClassId", search.ClassId),
                new SqlParameter("@SchoolId", search.SchoolId),
                new SqlParameter("@TeacherId", search.TeacherId),
                new SqlParameter("@TermId", search.TermId),
                new SqlParameter("@CourseId", search.CourseId)
            }).AsNoTracking()
            .OrderBy(m => m.ClassDate)
            .ThenBy(m => m.ClassBeginTime)
            .ToList();

            return studentTimeLess;
        }

    }
}
