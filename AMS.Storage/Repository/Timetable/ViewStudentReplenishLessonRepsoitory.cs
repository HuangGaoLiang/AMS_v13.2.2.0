using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 学生可补课/或调课
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-14</para>
    /// </summary>
    public class ViewStudentReplenishLessonRepsoitory : BaseRepository<ViewStudentReplenishLesson>
    {
        public ViewStudentReplenishLessonRepsoitory()
        {
        }

        public ViewStudentReplenishLessonRepsoitory(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// 获取学生可补课/或调课列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        /// <param name="schoolId">校区ID</param>
        /// <param name="studentId">学生ID</param>
        /// <param name="teacherId">学生Id</param>
        /// <param name="page">当前页</param>
        /// <param name="rows">当前行</param>
        /// <param name="total">总数量</param>
        /// <returns></returns>
        public List<ViewStudentReplenishLesson> GetList(string schoolId,
            long studentId,string teacherId, int page, int rows, out int total)
        {
            #region SQL查询
            string sql = @"
WITH Fill --缺勤、请假
AS (SELECT B.ClassId,
           B.ClassDate,
           (B.ClassBeginTime + '-' + B.ClassEndTime) AS ClassTime,
           B.TeacherId,
           B.CourseId,
           B.ClassRoomId,
           B.LessonId,
           CASE A.AttendStatus
               WHEN 2 THEN
                   1
               ELSE
                   2
           END AS [Type]
    FROM
    (
        SELECT LessonId,
               AttendStatus,
               AdjustType,
               AttendDate
        FROM [dbo].[TblTimLessonStudent]
        WHERE StudentId = @StudentId
              AND SchoolId = @SchoolId
              AND AdjustType = 0
              AND AttendStatus IN ( 0, 2 )
        UNION ALL
        SELECT LessonId,
               AttendStatus,
               AdjustType,
               AttendDate
        FROM [dbo].[TblTimReplenishLesson]
        WHERE StudentId = @StudentId
              AND SchoolId = @SchoolId
              AND AdjustType = 0
              AND AttendStatus IN ( 0, 2 )
    ) AS A
        LEFT JOIN [dbo].[TblTimLesson] AS B
            ON A.LessonId = B.LessonId
    WHERE CONVERT(DATETIME, (CONVERT(VARCHAR(32), B.ClassDate, 23) + ' ' + B.ClassEndTime), 101) < GETDATE()
          AND B.LessonType = 1
          AND B.TeacherId = @TeacherId),
     Tune --未来可调的课
AS (SELECT B.ClassId,
           B.ClassDate,
           (B.ClassBeginTime + '-' + B.ClassEndTime) AS ClassTime,
           B.TeacherId,
           B.CourseId,
           B.ClassRoomId,
           B.LessonId,
           3 AS [Type]
    FROM
    (
        SELECT LessonId,
               AttendStatus,
               AdjustType,
               AttendDate
        FROM [dbo].[TblTimLessonStudent]
        WHERE StudentId = @StudentId
              AND SchoolId = @SchoolId
              AND AdjustType = 0
              AND AttendStatus IN ( 0, 2 )
        UNION ALL
        SELECT LessonId,
               AttendStatus,
               AdjustType,
               AttendDate
        FROM [dbo].[TblTimReplenishLesson]
        WHERE StudentId = @StudentId
              AND SchoolId = @SchoolId
              AND AdjustType = 0
              AND AttendStatus IN ( 0, 2 )
    ) AS A
        LEFT JOIN [dbo].[TblTimLesson] AS B
            ON A.LessonId = B.LessonId
    WHERE B.ClassDate > GETDATE()
          AND B.LessonType = 1
          AND B.TeacherId = @TeacherId),
     StuClass
AS (SELECT *
    FROM Fill
    UNION
    SELECT *
    FROM Tune),
     ClassTime
AS (SELECT t_u.ClassId,
           t_u.ClassDate,
           STUFF(
           (
               SELECT StuClass.ClassTime + ' '
               FROM StuClass
               WHERE t_u.ClassId = StuClass.ClassId
                     AND t_u.ClassDate = StuClass.ClassDate
               ORDER BY StuClass.ClassId,
                        t_u.ClassDate
               FOR XML PATH('')
           ),
           1,
           0,
           ''
                ) AS ClassTime
    FROM StuClass AS t_u
    GROUP BY t_u.ClassId,
             t_u.ClassDate)
SELECT DISTINCT
    ROW_NUMBER() OVER (ORDER BY ClassDate, ClassTime) AS RowId,
    Temp.*
FROM
(
    SELECT DISTINCT
        ClassTime.ClassId,
        ClassTime.ClassDate,
        ClassTime.ClassTime,
        StuClass.TeacherId,
        StuClass.CourseId,
        StuClass.ClassRoomId,
        StuClass.[Type]
    FROM ClassTime
        INNER JOIN StuClass
            ON StuClass.ClassId = ClassTime.ClassId
               AND StuClass.ClassDate = ClassTime.ClassDate
) AS Temp;
";
            #endregion

            SqlParameter[] sqlParameters = {
                new SqlParameter("@SchoolId",schoolId),
                new SqlParameter("@StudentId",studentId),
                new SqlParameter("@TeacherId",teacherId)
            };
            var query = base.CurrentContext.ViewStudentReplenishLessons.FromSql(sql, sqlParameters).AsNoTracking().AsQueryable();

            total = query.Count();

            return query.OrderBy(x => x.RowId).Skip((page - 1) * rows).Take(rows).ToList();
        }
    }
}
