using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 老师上课时间统计
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-11</para>
    /// </summary>
    public class ViewTeacherClassDateRepository : BaseRepository<ViewTeacherClassDate>
    {
        #region 老师上课时间统计 SQL
        private const string SQL = @"
WITH StuAttend --学生考勤
AS (SELECT A.AttendStatus,
           A.AdjustType,
           B.ClassDate,
           B.LessonType,
           B.ClassBeginTime,
           B.ClassEndTime,
           B.StudentId
    FROM
    (
        SELECT Stu.LessonStudentId AS Id,
               Stu.LessonId,
               Stu.AttendStatus,
               Stu.AdjustType,
               Stu.AttendDate
        FROM dbo.TblTimLessonStudent AS Stu
        UNION
        SELECT StuR.ReplenishLessonId AS Id,
               StuR.LessonId,
               StuR.AttendStatus,
               StuR.AdjustType,
               StuR.AttendDate
        FROM dbo.TblTimReplenishLesson AS StuR
    ) AS A
        LEFT JOIN dbo.TblTimLesson AS B
            ON B.LessonId = A.LessonId
    WHERE B.SchoolId = @SchoolId
          AND B.TeacherId = @TeacherId
          AND B.ClassDate BETWEEN @SDate AND @EDate),
     TeacharDate --老师上课的日期
AS (SELECT ClassDate,
           0 AS Num
    FROM StuAttend
    GROUP BY StuAttend.ClassDate),
     BeforeA --统计常规课学生的异常情况
AS (SELECT ClassDate,
           COUNT(1) AS Num
    FROM StuAttend AS A
    WHERE (
              (
                  A.AttendStatus = 0
                  AND A.AdjustType = 0
              ) --请假未安排补课/调课
              OR
              (
                  A.AttendStatus = 2
                  AND A.AdjustType = 0
              ) --缺勤但未安排补课/调课
              OR (A.AdjustType = 4) --申请补签家长未确认
          )
          AND A.LessonType = 1
          AND CONVERT(DATETIME, (CONVERT(VARCHAR(32), A.ClassDate, 23) + ' ' + A.ClassEndTime), 101) < GETDATE()
    GROUP BY A.ClassDate),
     AfterA
AS (SELECT ClassDate,
           COUNT(1) AS Num
    FROM StuAttend AS A
    WHERE A.AttendStatus = 2
          AND A.AdjustType = 0
          AND A.LessonType = 1
          AND CONVERT(DATETIME, (CONVERT(VARCHAR(32), A.ClassDate, 23) + ' ' + A.ClassBeginTime), 101) > GETDATE()
    GROUP BY A.ClassDate),
     BeforeB
AS (SELECT ClassDate,
           COUNT(1) AS Num
    FROM StuAttend AS A
    WHERE (
              (
                  A.AttendStatus = 0
                  AND A.AdjustType = 0
              ) --请假未安排补课/调课
              OR
              (
                  A.AttendStatus = 2
                  AND A.AdjustType = 0
              ) --缺勤但未安排补课/调课
              OR (A.AdjustType = 4) --申请补签家长未确认
          )
          AND A.LessonType = 2
          AND A.ClassEndTime < GETDATE()
    GROUP BY A.ClassDate),
     AfterB
AS (SELECT ClassDate,
           COUNT(1) AS Num
    FROM StuAttend AS A
    WHERE A.AttendStatus = 2
          AND A.AdjustType = 0
          AND A.LessonType = 2
          AND A.ClassBeginTime > GETDATE()
    GROUP BY A.ClassDate)
SELECT Total.ClassDate,
       SUM(Total.Num) AS Mark
FROM
(
    SELECT *
    FROM TeacharDate --老师上课时间
    UNION ALL
    SELECT *
    FROM BeforeA --常规课次当前时间之前的异常学生统计
    UNION ALL
    SELECT *
    FROM AfterA --常规课次当前时间之后的异常学生统计
    UNION ALL
    SELECT *
    FROM BeforeB --写生课次当前时间之前的异常学生统计
    UNION ALL
    SELECT *
    FROM AfterB --写生课次当前时间之后的异常学生统计
) AS Total
GROUP BY Total.ClassDate;
";
        #endregion

        #region 老师上课时间统计列表 Get

        /// <summary>
        /// 老师上课时间统计
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="schoolId">校区ID</param>
        /// <param name="teacherId">老师Id</param>
        /// <param name="sTime">开始日期</param>
        /// <param name="eTime">结束日期</param>
        /// <returns>老师上课日期列表</returns>
        public List<ViewTeacherClassDate> Get(string schoolId, string teacherId, DateTime sTime, DateTime eTime)
        {
            SqlParameter[] sqlParameters = {
                new SqlParameter("@SchoolId",schoolId),
                new SqlParameter("@TeacherId",teacherId),
                new SqlParameter("@SDate",sTime),
                new SqlParameter("@EDate",eTime)
            };

            return base.CurrentContext.View_TeacherClassDate
                .FromSql<ViewTeacherClassDate>(SQL, sqlParameters)
                .AsNoTracking()
                .ToList();
        }
        #endregion
    }
}
