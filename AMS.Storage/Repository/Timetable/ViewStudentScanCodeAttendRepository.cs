using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using AMS.Dto;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 学生扫码考勤仓储
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-06</para>
    /// </summary>
    public class ViewStudentScanCodeAttendRepository : BaseRepository<ViewStudentScanCodeAttend>
    {
        #region 获取当天考勤列表 SQL
        private const string SQL = @"
SELECT A.*,
       B.ClassId,
       B.TeacherId,
       B.ClassDate,
       B.ClassBeginTime,
       B.ClassEndTime,
	   B.StudentId,
	   B.SchoolId,
       B.LessonType,
	   B.BusinessId,
	   B.CourseId
FROM
(
    SELECT LessonStudentId AS Id,
           AttendStatus,
           AdjustType,
           AttendDate,
           LessonId,
           1 AS TblSource
    FROM TblTimLessonStudent
    UNION ALL
    SELECT ReplenishLessonId AS Id,
           AttendStatus,
           AdjustType,
           AttendDate,
           LessonId,
           2 AS TblSource
    FROM TblTimReplenishLesson
) AS A
    INNER JOIN TblTimLesson AS B
        ON B.LessonId = A.LessonId
		WHERE B.SchoolId=@SchoolId
        AND B.StudentId=@StudentId
		AND B.TeacherId=@TeacherId
		AND A.AdjustType=@AdjustType
		AND B.ClassDate=@ClassDate
		ORDER BY B.ClassDate ASC,B.ClassBeginTime ASC";
        #endregion

        #region 获取当天考勤列表 GetDayAttendList
        /// <summary>
        /// 获取当天考勤列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <param name="schoolId">校区ID</param>
        /// <param name="teacherId">老师ID</param>
        /// <param name="studentId">学生ID</param>
        /// <param name="classDate">考勤日期 如:2019-03-06</param>
        /// <returns>学生考勤列表</returns>
        public List<ViewStudentScanCodeAttend> GetDayAttendList(
            string schoolId, string teacherId, long studentId, DateTime classDate)
        {
            SqlParameter[] sqlParameters = {
                new SqlParameter("@SchoolId",schoolId),
                new SqlParameter("@StudentId",studentId),
                new SqlParameter("@TeacherId",teacherId),
                new SqlParameter("@AdjustType",AdjustType.DEFAULT),
                new SqlParameter("@ClassDate",classDate)
            };

            return base.CurrentContext.ViewStudentScanCodeAttend
                .FromSql<ViewStudentScanCodeAttend>(SQL, sqlParameters)
                .ToList();
        }
        #endregion
    }
}
