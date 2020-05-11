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
    /// ReplenishWeekRepository视图仓储
    /// </summary>
    public class ViewReplenishWeekRepository : BaseRepository<ViewReplenishWeek>
    {
        /// <summary>
        /// ReplenishWeekRepository 无参构造函数
        /// </summary>
        public ViewReplenishWeekRepository()
        {
        }

        /// <summary>
        /// ReplenishWeekRepository 构造函数
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public ViewReplenishWeekRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 根据条件查询补课周补课分页列表
        /// <para>作    者：Huang GaoLiang</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="search">补课周补课分页查询条件</param>
        /// <returns>返回补课周补课分页列表</returns>
        public PageResult<ViewReplenishWeek> GetReplenishWeekList(ReplenishWeekListSearchRequest search)
        {
            #region sql

            string querySql = @"SELECT  s.StudentId,
                                        s.StudentNo,
                                        s.StudentName,
                                        c.ClassNo,
                                        c.TeacherId,
                                        COUNT(0) AS ClassNumber,
                                        w.ReplenishWeekId,
                                        w.Remark,
                                        tl.TermId,
                                        t.TermName,
                                        tl.CourseId,
                                        tl.ClassId
                                FROM dbo.TblTimLessonStudent ts
                                    INNER JOIN TblTimLesson tl ON tl.LessonId = ts.LessonId
                                    INNER JOIN dbo.TblCstStudent s ON s.StudentId = ts.StudentId
                                    INNER JOIN dbo.TblDatClass c ON tl.ClassId = c.ClassId
                                    INNER JOIN dbo.TblDatTerm t ON tl.TermId = t.TermId
                                    LEFT JOIN dbo.TblDatClassReplenishWeek w ON tl.ClassId=w.ClassId  AND  s.StudentId = w.StudentId
                                WHERE ts.SchoolId = @SchoolId
                                        AND c.TeacherId = @TeacherId
                                        AND t.TermId = @TermId
                                        AND ts.AttendStatus IN ( 0, 2 )
                                        AND ts.AdjustType in( 0, 3 )
                                        AND tl.ClassDate<=GETDATE()
                                GROUP BY s.StudentId,
                                         s.StudentNo,
                                         s.StudentName,
                                         c.ClassNo,
                                         c.TeacherId,
                                         w.ReplenishWeekId,
                                         w.Remark,
                                         tl.TermId,
                                         t.TermName,
                                         tl.CourseId,
                                         tl.ClassId
                               HAVING COUNT(0) > 0
                              UNION
                              SELECT  s.StudentId,
                                      s.StudentNo,
                                      s.StudentName,
                                      c.ClassNo,
                                      c.TeacherId,
                                      COUNT(0) AS ClassNumber,
                                      w.ReplenishWeekId,
                                      w.Remark,
                                      tl.TermId,
                                      t.TermName,
                                      tl.CourseId,
                                      tl.ClassId
                              FROM TblTimLesson tl
                                   INNER JOIN TblTimReplenishLesson rl ON tl.LessonId = rl.LessonId
                                   INNER JOIN dbo.TblCstStudent s ON s.StudentId = rl.StudentId
                                   INNER JOIN dbo.TblDatClass c ON tl.ClassId = c.ClassId
                                   INNER JOIN dbo.TblDatTerm t ON tl.TermId = t.TermId
                                   LEFT JOIN dbo.TblDatClassReplenishWeek w ON s.StudentId = w.StudentId AND  c.ClassId =w.ClassId
                              WHERE rl.SchoolId = @SchoolId
                                      AND c.TeacherId = @TeacherId
                                      AND t.TermId = @TermId
                                      AND rl.AttendStatus IN ( 0, 2 )
                                      AND rl.AdjustType in( 0)
                                      AND tl.ClassDate <= GETDATE()
                              GROUP BY s.StudentId,
                                       s.StudentNo,
                                       s.StudentName,
                                       c.ClassNo,
                                       c.TeacherId,
                                       w.ReplenishWeekId,
                                       w.Remark,
                                       tl.TermId,
                                       t.TermName,
                                       tl.CourseId,
                                       tl.ClassId
                              HAVING COUNT(0) > 0 ";
            #endregion
            var replenishWeekQuery = base.CurrentContext.ViewReplenishWeek.FromSql(querySql, new SqlParameter[] {
                new SqlParameter("@SchoolId", search.SchoolId),
                new SqlParameter("@TeacherId", search.TeacherId),
                new SqlParameter("@TermId", search.TermId)
            }).AsNoTracking();
            var result = replenishWeekQuery
                              .WhereIf(search.CourseId > 0, x => x.CourseId == search.CourseId)
                              .WhereIf(!string.IsNullOrWhiteSpace(search.ClassNo), x => x.ClassNo == search.ClassNo)
                              .OrderBy(m => m.StudentNo).ToPagerSource(search.PageIndex, search.PageSize);

            return result;
        }

    }
}
