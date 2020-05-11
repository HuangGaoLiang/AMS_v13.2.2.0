using System.Collections.Generic;
using System.Linq;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 上课时间变化
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-22</para>
    /// </summary>
    public class ViewChangeClassTimeRepository : BaseRepository<ViewChangeClassTime>
    {
        public ViewChangeClassTimeRepository() { }
        public ViewChangeClassTimeRepository(DbContext context) : base(context) { }

        /// <summary>
        /// 获取班级上课时间发生变化的数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-22</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <returns>班级上课时间发生变化的数据</returns>
        public List<ViewChangeClassTime> Get(List<long> classId)
        {
            string where = string.Join(",", classId);

            string sql = $@"
SELECT DISTINCT
    A.ClassId,
    B.ClassDate AS NewClassDate,
    B.ClassBeginTime AS NewClassBeginTime,
    B.ClassEndTime AS NewClassEndTime,
    C.ClassDate AS OldClassDate,
    C.ClassBeginTime AS OldClassBeginTime,
    C.ClassEndTime AS OldClassEndTime
FROM
(
    SELECT L.ClassId,
           L.BusinessId
    FROM
    (
        SELECT S.LessonId
        FROM dbo.TblTimLessonStudent AS S
        UNION ALL
        SELECT R.LessonId
        FROM dbo.TblTimReplenishLesson AS R
    ) AS Attend
        INNER JOIN dbo.TblTimLesson L
            ON L.LessonId = Attend.LessonId
               AND L.BusinessType IN ( 9, 10 )
               AND L.ClassId IN ({where})
) AS A
    INNER JOIN TblTimAdjustLesson AS B
        ON B.AdjustLessonId = A.BusinessId
    INNER JOIN dbo.TblTimLesson AS C
        ON C.LessonId = B.FromLessonId;
";

            return base.CurrentContext.ViewChangeClassTime.FromSql(sql).AsNoTracking().ToList();
        }

        /// <summary>
        /// 获取班级上课时间发生变化的数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-22</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <returns>班级上课时间发生变化的数据</returns>
        public List<ViewChangeClassTime> Get(long classId)
        {
            return this.Get(new List<long> { classId });
        }

    }
}
