using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 统计班级学生异常状态(异常状态表示学生在指定时间未打卡,未进行补签，未调课，未补课)
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-11</para>
    /// </summary>
    public class ViewTotalClassStudentAbnormalStateRepository : BaseRepository<ViewTotalClassStudentAbnormalState>
    {
        #region  统计班级学生异常状态 SQL
        private const string Sql = @"
WITH StuAttend --学生考勤基础数据
AS (SELECT A.AttendStatus,
           B.*
    FROM
    (
        SELECT Stu.LessonId,
               Stu.AttendStatus,
               Stu.AdjustType
        FROM dbo.TblTimLessonStudent AS Stu
        UNION ALL
        SELECT StuR.LessonId,
               StuR.AttendStatus,
               StuR.AdjustType
        FROM dbo.TblTimReplenishLesson AS StuR
    ) A
        LEFT JOIN dbo.TblTimLesson AS B
            ON B.LessonId = A.LessonId
    WHERE B.SchoolId = @SchoolId
          AND B.TeacherId = @TeacherId
          AND B.ClassDate = @ClassDate
          AND A.AdjustType IN (0,4)),
     BeforeA --常规课次当前时间之前学生是否缺勤
AS (SELECT B.ClassId,
           COUNT(1) AS Total
    FROM StuAttend AS B
    WHERE B.AttendStatus = 0
          AND B.LessonType = 1
          AND CONVERT(DATETIME, (CONVERT(VARCHAR(32), B.ClassDate, 23) + ' ' + B.ClassEndTime), 101) < GETDATE()
    GROUP BY B.ClassId),
     AfterA --常规课次当前时间后悔学生是否有请假
AS (SELECT B.ClassId,
           COUNT(1) AS Total
    FROM StuAttend AS B
    WHERE B.AttendStatus = 2
          AND B.LessonType = 1
    GROUP BY B.ClassId),
     BeforeB --写生课次当前时间之前学生是否缺勤
AS (SELECT B.ClassId,
           COUNT(1) AS Total
    FROM StuAttend AS B
    WHERE B.AttendStatus = 0
          AND B.LessonType = 2
          AND B.ClassEndTime < GETDATE()
    GROUP BY B.ClassId),
     AfterB --写生课次当前时间后悔学生是否有请假
AS (SELECT B.ClassId,
           COUNT(1) AS Total
    FROM StuAttend AS B
    WHERE B.AttendStatus = 2
          AND B.LessonType = 2
    GROUP BY B.ClassId)
SELECT ClassId,
       SUM(A.Total) AS Total,
       1 AS LessonType
FROM
(SELECT * FROM BeforeA UNION ALL SELECT * FROM AfterA) AS A
GROUP BY A.ClassId
UNION ALL
SELECT ClassId,
       SUM(A.Total) AS Total,
       1 AS LessonType
FROM
(SELECT * FROM BeforeB UNION ALL SELECT * FROM AfterB) AS A
GROUP BY A.ClassId;
";
        #endregion

        #region 统计班级学生异常状态 Get
        /// <summary>
        /// 统计班级学生异常状态(异常状态表示学生在指定时间未打卡,请假,未进行补签，未调课，未补课)
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="classDate">上课时间</param>
        /// <param name="schoolId">校区ID</param>
        /// <param name="teacherId">老师ID</param>
        /// <returns>班级学生异常状态列表</returns>
        public List<ViewTotalClassStudentAbnormalState> Get(string schoolId, string teacherId, DateTime classDate)
        {
            SqlParameter[] sqlParameters = {
                new SqlParameter("@SchoolId",schoolId),
                new SqlParameter("@TeacherId",teacherId),
                new SqlParameter("@ClassDate",classDate)
            };

            return base.CurrentContext.ViewTotalClassStudentAbnormalState
                .FromSql<ViewTotalClassStudentAbnormalState>(Sql, sqlParameters)
                .AsNoTracking()
                .ToList();
        }
        #endregion
    }
}
