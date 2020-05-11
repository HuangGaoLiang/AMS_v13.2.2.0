using System.Collections.Generic;
using System.Linq;
using AMS.Storage.Models;
using System;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading.Tasks;

namespace AMS.Storage.Repository
{
    /// <summary>s
    /// 课程级别关联视图
    /// </summary>
    public class ViewCourseLevelMiddleRepository : BaseRepository<ViewCourseLevelMiddle>
    {
        public ViewCourseLevelMiddleRepository()
        {
        }

        public ViewCourseLevelMiddleRepository(DbContext dbContext) : base(dbContext)
        {
        }


        /// <summary>
        /// 课程级别
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-14</para>
        /// </summary>
        /// <param name="courseId">一组课程Id</param>
        /// <returns>课程级别关联视图列表</returns>
        public async Task<List<ViewCourseLevelMiddle>> Get(IEnumerable<long> courseId)
        {
            if (courseId == null || !courseId.Any())
            {
                throw new ArgumentNullException(nameof(courseId));
            }

            string joinCourseId = string.Join(",", courseId);

            string querySql = $@"
                                SELECT A.CourseLevelMiddleId,
	                                   A.CourseId,
	                                   A.CourseLevelId,
	                                   A.BeginAge,
	                                   A.EndAge,
	                                   A.Duration,
	                                   B.LevelCode,
	                                   B.LevelCnName,
	                                   B.LevelEnName,
	                                   B.IsDisabled
                                FROM TblDatCourseLevelMiddle AS A
                                LEFT JOIN TblDatCourseLevel AS B ON B.CourseLevelId = A.CourseLevelId
                                WHERE CourseId IN ({joinCourseId})
                                ORDER BY LevelCode ASC
                                ";

            return await base.CurrentContext.ViewCourseLevelMiddles.FromSql(querySql).ToListAsync();
        }
    }
}
