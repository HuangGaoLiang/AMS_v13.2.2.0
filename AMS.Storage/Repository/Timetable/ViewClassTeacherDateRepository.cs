using System;
using System.Collections.Generic;
using System.Text;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AMS.Storage.Repository
{
    /// <summary>
    ///  班级老师上课时间
    ///  <para>作    者：zhiwei.Tang</para>
    ///  <para>创建时间：2019-03-21</para>
    /// </summary>
    public class ViewClassTeacherDateRepository : BaseRepository<ViewClassTeacherDate>
    {
        public ViewClassTeacherDateRepository() { }

        public ViewClassTeacherDateRepository(DbContext context) : base(context) { }

        /// <summary>
        /// 获取班级老师上课时间
        ///  <para>作    者：zhiwei.Tang</para>
        ///  <para>创建时间：2019-03-21</para> 
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <returns>班级老师上课时间列表</returns>
        public List<ViewClassTeacherDate> Get(List<long> classId)
        {
            string where = string.Join(",", classId);
            string sql = $@"
WITH StuAtten
AS (SELECT B.ClassId,
           B.TeacherId,
           B.ClassDate
    FROM
    (
        SELECT A.LessonId
        FROM dbo.TblTimLessonStudent AS A
        UNION ALL
        SELECT B.LessonId
        FROM dbo.TblTimReplenishLesson AS B
    ) AS A
        INNER JOIN dbo.TblTimLesson B
            ON B.LessonId = A.LessonId
    WHERE B.ClassId IN ({where}))
SELECT StuAtten.ClassId,
       StuAtten.TeacherId,
       StuAtten.ClassDate
FROM StuAtten
GROUP BY StuAtten.ClassId,
         StuAtten.TeacherId,
         StuAtten.ClassDate
ORDER BY StuAtten.ClassId,
         StuAtten.ClassDate;

";
            return base.CurrentContext.ViewClassTeacherDate.FromSql(sql).AsNoTracking().ToList();
        }

        /// <summary>
        /// 获取班级老师上课时间
        ///  <para>作    者：zhiwei.Tang</para>
        ///  <para>创建时间：2019-03-21</para> 
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <returns>班级老师上课时间列表</returns>
        public List<ViewClassTeacherDate> Get(long classId)
        {
            return this.Get(new List<long> { classId });
        }
    }
}
