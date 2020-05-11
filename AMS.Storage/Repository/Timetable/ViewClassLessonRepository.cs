using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using AMS.Dto;
using AMS.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 家校互联－老师要上课的班级考勤列表仓储
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-08</para>
    /// </summary>
    public class ViewClassLessonRepository : BaseRepository<ViewClassLesson>
    {
        public ViewClassLessonRepository() { }

        public ViewClassLessonRepository(DbContext context) : base(context) { }

        #region 获取老师要上课的班级考勤列表 GetClassLessons
        /// <summary>
        /// 获取老师要上课的班级考勤列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-08</para>
        /// </summary>
        /// <param name="schoolId">校区ID</param>
        /// <param name="teacherId">老师ID</param>
        /// <param name="classDate">上课日期</param>
        /// <returns>获取老师要上课的班级考勤列表</returns>
        public List<ViewClassLesson> GetClassLessons(string schoolId, string teacherId, DateTime classDate)
        {
            #region 老师上课班级统计查询SQL
            const string SQL = @"
WITH BasisLesson
AS (SELECT L.*,
           D.ClassNo
    FROM
    (
        SELECT Stu.LessonId
        FROM dbo.TblTimLessonStudent AS Stu
        UNION
        SELECT StuR.LessonId
        FROM dbo.TblTimReplenishLesson AS StuR
    ) AS Basis
        INNER JOIN dbo.TblTimLesson AS L
            ON L.LessonId = Basis.LessonId
        LEFT JOIN dbo.TblDatClass AS D
            ON D.ClassId = L.ClassId
    WHERE L.SchoolId = @SchoolId
          AND L.TeacherId = @TeacherId
          AND L.ClassDate = @ClassDate),
     RegularClass
AS (SELECT DISTINCT
        A.ClassDate,
        A.ClassId,
        B.ClassCnName AS ClassName,
        C.RoomNo AS ClassRoom,
        A.LessonType,
        (A.ClassBeginTime + '-' + A.ClassEndTime) AS ClassTime,
        A.ClassNo,
        A.TeacherId
    FROM BasisLesson AS A
        LEFT JOIN dbo.TblDatCourse AS B
            ON B.CourseId = A.CourseId
        LEFT JOIN dbo.TblDatClassRoom AS C
            ON C.ClassRoomId = A.ClassRoomId
    WHERE A.LessonType = 1),
     LifeClass
AS (SELECT DISTINCT
        A.ClassDate,
        A.BusinessId AS ClassId,
        B.Title AS ClassName,
        B.Place AS ClassRoom,
        A.LessonType,
        (A.ClassBeginTime + '-' + A.ClassEndTime) AS ClassTime,
        B.LifeClassCode AS ClassNo,
        A.TeacherId
    FROM BasisLesson AS A
        LEFT JOIN dbo.TblTimLifeClass AS B
            ON B.LifeClassId = A.BusinessId
    WHERE A.LessonType = 2)
SELECT DISTINCT
    B.ClassDate,
    B.ClassId,
    B.ClassName,
    B.ClassRoom,
    B.LessonType,
    A.ClassTime,
    B.ClassNo,
    B.TeacherId
FROM
(
    SELECT t_u.ClassId,
           STUFF(
           (
               SELECT RegularClass.ClassTime + ' '
               FROM RegularClass
               WHERE t_u.ClassId = RegularClass.ClassId
               ORDER BY RegularClass.ClassId
               FOR XML PATH('')
           ),
           1,
           0,
           ''
                ) AS ClassTime
    FROM RegularClass AS t_u
    GROUP BY t_u.ClassId
) AS A
    LEFT JOIN RegularClass AS B
        ON B.ClassId = A.ClassId
UNION
SELECT *
FROM LifeClass;
";
            #endregion
            SqlParameter[] sqlParameters = {
                new SqlParameter("@SchoolId",schoolId),
                new SqlParameter("@TeacherId",teacherId),
                new SqlParameter("@ClassDate",classDate)
            };

            return base.CurrentContext.ViewClassLesson.FromSql(SQL, sqlParameters).ToList();
        }
        #endregion

        #region 获取补课/调课课程列表 GetReplenishClassLessonList

        /// <summary>
        /// 获取补课/调课课程列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        /// <param name="schoolId">校区ID</param>
        /// <param name="courseId">课程ID</param>
        /// <param name="classDate">上课时间</param>
        /// <param name="termId">学期ID</param>
        /// <returns>补课/调课课程列表</returns>
        public List<ViewClassLesson> GetReplenishClassLessonList(
            string schoolId, long courseId, DateTime classDate, long termId)
        {
            #region SQL
            const string sql = @"
WITH BasisLesson
AS (SELECT L.*,
           D.ClassNo
    FROM
    (
        SELECT Stu.LessonId
        FROM dbo.TblTimLessonStudent AS Stu
        UNION
        SELECT StuR.LessonId
        FROM dbo.TblTimReplenishLesson AS StuR
    ) AS Basis
        INNER JOIN dbo.TblTimLesson AS L
            ON L.LessonId = Basis.LessonId
        LEFT JOIN dbo.TblDatClass AS D
            ON D.ClassId = L.ClassId
    WHERE L.SchoolId = @SchoolId
          AND L.CourseId = @CourseId
          AND L.ClassDate = @ClassDate
		  AND L.LessonType=1
		  AND CONVERT(DATETIME, (CONVERT(VARCHAR(32), L.ClassDate, 23) + ' ' + L.ClassBeginTime), 101) > GETDATE()
          AND L.TermId=@TermId),
     RegularClass
AS (SELECT DISTINCT
        A.ClassDate,
        A.ClassId,
        B.ClassCnName AS ClassName,
        C.RoomNo AS ClassRoom,
        A.LessonType,
        (A.ClassBeginTime + '-' + A.ClassEndTime) AS ClassTime,
        A.ClassNo,
		A.TeacherId
    FROM BasisLesson AS A
        LEFT JOIN dbo.TblDatCourse AS B
            ON B.CourseId = A.CourseId
        LEFT JOIN dbo.TblDatClassRoom AS C
            ON C.ClassRoomId = A.ClassRoomId)
SELECT DISTINCT
    B.ClassDate,
    B.ClassId,
    B.ClassName,
    B.ClassRoom,
	B.TeacherId,
    B.LessonType,
    A.ClassTime,
    B.ClassNo
FROM
(
    SELECT t_u.ClassId,
           STUFF(
           (
               SELECT RegularClass.ClassTime + ' '
               FROM RegularClass
               WHERE t_u.ClassId = RegularClass.ClassId
               ORDER BY RegularClass.ClassId
               FOR XML PATH('')
           ),
           1,
           0,
           ''
                ) AS ClassTime
    FROM RegularClass AS t_u
    GROUP BY t_u.ClassId
) AS A
    LEFT JOIN RegularClass AS B
        ON B.ClassId = A.ClassId;
";
            #endregion

            SqlParameter[] sqlParameters = {
                new SqlParameter("@SchoolId",schoolId),
                new SqlParameter("@CourseId",courseId),
                new SqlParameter("@ClassDate",classDate),
                 new SqlParameter("@TermId",termId)
            };

            return base.CurrentContext.ViewClassLesson.FromSql(sql, sqlParameters).AsNoTracking().ToList();
        }

        #endregion
    }
}
