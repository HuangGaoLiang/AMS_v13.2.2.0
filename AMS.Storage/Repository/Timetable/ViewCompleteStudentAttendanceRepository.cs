using AMS.Core;
using AMS.Dto;
using AMS.Storage.Models;
using Jerrisoft.Platform.Log;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 完整的学生考勤(包括正常/补课/调课的考勤)
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-15</para>
    /// </summary>
    public class ViewCompleteStudentAttendanceRepository : BaseRepository<ViewCompleteStudentAttendance>
    {
        public ViewCompleteStudentAttendanceRepository()
        {

        }
        public ViewCompleteStudentAttendanceRepository(DbContext context) : base(context)
        {

        }

        #region BuildBasisQueryable 构建基础查询
        #region 基础查询 SQL
        const string SQL = @"
SELECT A.*,
       B.EnrollOrderItemId,
       B.SchoolId,
       B.StudentId,
       B.ClassDate,
       B.ClassBeginTime,
       B.ClassEndTime,
       B.TermId,
       B.ClassId,
       B.CourseId,
       B.CourseLevelId,
       B.ClassRoomId,
       B.TeacherId,
       B.LessonType,
       B.LessonCount,
       B.BusinessId
FROM
(
    SELECT A.LessonStudentId AS Id,
           A.LessonId,
           A.AttendStatus,
           A.AdjustType,
           A.ReplenishCode,
           A.AttendUserType,
           A.AttendDate,
           1 AS TblType
    FROM dbo.TblTimLessonStudent AS A
    UNION ALL
    SELECT B.ReplenishLessonId AS Id,
           B.LessonId,
           B.AttendStatus,
           B.AdjustType,
           B.ReplenishCode,
           B.AttendUserType,
           B.AttendDate,
           2 AS TblType
    FROM dbo.TblTimReplenishLesson AS B
) AS A
    INNER JOIN dbo.TblTimLesson B
        ON B.LessonId = A.LessonId
";
        #endregion

        /// <summary>
        /// 构建一个基础查询
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para> 
        /// </summary>
        private IQueryable<ViewCompleteStudentAttendance> BuildBasisQueryable
        {
            get
            {
                var queryable = base.CurrentContext
                    .ViewCompleteStudentAttendance
                    .FromSql<ViewCompleteStudentAttendance>(SQL)
                    .AsNoTracking()
                    .AsQueryable();

                return queryable;
            }
        }
        #endregion

        /// <summary>
        /// 获取一个班级某一天的上课信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="classId">班级Id</param>
        /// <param name="day">某一天</param>
        /// <param name="lessonType">课次类型</param>
        /// <returns>班级某一天的上课信息列表</returns>
        public List<ViewCompleteStudentAttendance> GetClassInfoTheDayList(
            string schoolId, long classId, DateTime day, LessonType lessonType)
        {
            var query = this.BuildBasisQueryable
                .Where(x => x.SchoolId == schoolId &&
                x.ClassId == classId &&
                x.ClassDate == day &&
                x.LessonType == (int)lessonType)
                .ToList();

            return query;
        }

        /// <summary>
        /// 获取学生某一天的课次信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="classId">班级Id</param>
        /// <param name="day">某一天</param>
        /// <param name="studentId">学生Id</param>
        /// <param name="lessonType">课次类型</param>
        /// <returns>班级某一天的上课信息列表</returns>
        public List<ViewCompleteStudentAttendance> GetStudetnDayLessonList(
            string schoolId, long classId, DateTime day, long studentId, LessonType lessonType)
        {
            var query = this.BuildBasisQueryable
                .Where(x => x.SchoolId == schoolId &&
                x.ClassId == classId &&
                x.ClassDate == day &&
                x.StudentId == studentId &&
                x.LessonType == (int)lessonType)
                .ToList();

            return query;
        }

        /// <summary>
        /// 获取转入课次信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="schoolId">校区ID</param>
        /// <param name="classId">班级Id</param>
        /// <param name="classDate">上课时间</param>
        /// <param name="lessonType">课次类型</param>
        /// <returns>转入课次信息</returns>
        public List<ViewCompleteStudentAttendance> GetInLessonList(
            string schoolId, long classId, DateTime classDate, LessonType lessonType)
        {
            string sql = @"
WITH SourceData AS (SELECT A.*,
       B.EnrollOrderItemId,
       B.SchoolId,
       B.StudentId,
       B.ClassDate,
       B.ClassBeginTime,
       B.ClassEndTime,
       B.TermId,
       B.ClassId,
       B.CourseId,
       B.CourseLevelId,
       B.ClassRoomId,
       B.TeacherId,
       B.LessonType,
       B.LessonCount,
       B.BusinessId
FROM
(
    SELECT A.LessonStudentId AS Id,
           A.LessonId,
           A.AttendStatus,
           A.AdjustType,
           A.ReplenishCode,
           A.AttendUserType,
           A.AttendDate,
           1 AS TblType
    FROM dbo.TblTimLessonStudent AS A
    UNION ALL
    SELECT B.ReplenishLessonId AS Id,
           B.LessonId,
           B.AttendStatus,
           B.AdjustType,
           B.ReplenishCode,
           B.AttendUserType,
           B.AttendDate,
           2 AS TblType
    FROM dbo.TblTimReplenishLesson AS B
) AS A
    INNER JOIN dbo.TblTimLesson B
        ON B.LessonId = A.LessonId
		WHERE B.ClassDate =@ClassDate
		AND B.SchoolId=@SchoolId
		AND B.ClassId=@ClassId
		AND B.LessonType=@LessonType)

SELECT * FROM (SELECT RANK()OVER(PARTITION BY classId ORDER BY studentId) rn,*FROM SourceData ) AS A
WHERE A.rn=1 
";

            SqlParameter[] sqlParameters = {
                new SqlParameter("@SchoolId",schoolId),
                new SqlParameter("@ClassDate",classDate),
                new SqlParameter("@ClassId",classId),
                new SqlParameter("@LessonType",LessonType.RegularCourse)
            };

            var query = base.CurrentContext.ViewCompleteStudentAttendance
                .FromSql(sql, sqlParameters).AsNoTracking().ToList();

            return query;
        }

        /// <summary>
        /// 获取学生考勤(正常/补课/调课)
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        /// <param name="adjustTypes">调整状态</param>
        /// <param name="lessonTypes">课次类型</param>
        /// <returns>学生考勤(正常/补课/调课)</returns>
        public List<ViewCompleteStudentAttendance> GetStudentAttendList(
            string schoolId, long studentId, List<int> adjustTypes, List<int> lessonTypes)
        {
            var query = this.BuildBasisQueryable
                .Where(x =>
                x.SchoolId == schoolId &&
                x.StudentId == studentId &&
                adjustTypes.Contains(x.AdjustType) &&
                lessonTypes.Contains(x.LessonType)).ToList();

            return query;
        }

        /// <summary>
        /// 获取课次列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="schoolId">校区ID</param>
        /// <param name="classId">班级Id</param>
        /// <param name="date">日期</param>
        /// <param name="lessonType">课次类型</param>
        /// <param name="adjustTypes">调整状态类型</param>
        /// <param name="teacherId">老师Id</param>
        /// <returns>课次列表</returns>
        public List<ViewCompleteStudentAttendance> GetLessonList(
            string schoolId, long classId, DateTime date, LessonType lessonType, List<int> adjustTypes,
            string teacherId)
        {
            var query = this.BuildBasisQueryable
                 .Where(x => x.SchoolId == schoolId &&
                             x.ClassDate == date &&
                             x.LessonType == (int)lessonType &&
                             adjustTypes.Contains(x.AdjustType) &&
                             x.TeacherId == teacherId);

            if (lessonType == LessonType.RegularCourse)
            {
                query = query.Where(x => x.ClassId == classId);
            }
            else
            {
                query = query.Where(x => x.BusinessId == classId);
            }
            return query.ToList();
        }

        /// <summary>
        /// 查询学生最近的考勤数据
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-18</para> 
        /// </summary>
        /// <param name="studentId">要查询的学生</param>
        /// <param name="attendStatus">要查询的考勤状态</param>
        /// <returns></returns>
        public List<ViewCompleteStudentAttendance> SearchStudentAttendLately(List<long> studentId, List<int> attendStatus)
        {
            /*
            A1查询：查询正在上课并且离当前时间最近的课程(考勤表)
            A2查询：查询正在上课并且离当前时间最近的课程(考勤补课表)
            B1查询：查询未上课并且离当前时间最近的课程(考勤表)
            B2查询：查询未上课并且离当前时间最近的课程(考勤补课表)
            C查询: 取出离当前最近的课次
            */
            //List<SqlParameter> parameterList = new List<SqlParameter>()
            //{
            //    new SqlParameter("@studentId", string.Join(",",studentId)),
            //    new SqlParameter("@attendStatus", string.Join(",",attendStatus)),
            //};

            if (!studentId.Any())
            {
                return new List<ViewCompleteStudentAttendance>();
            }
            string sql = $@"
WITH A1
AS ( SELECT ROW_NUMBER() OVER ( PARTITION BY ttl.StudentId,ttl.SchoolId
                                ORDER BY ClassEndDate ASC ) AS SN ,
			tts.LessonStudentId AS Id,
            tts.SchoolId,
			tts.AttendStatus,
			tts.AdjustType,
			tts.ReplenishCode,
			tts.AttendUserType,
			tts.AttendDate,
			1 AS TblType,
            ttl.LessonId ,
            ttl.StudentId ,
            ttl.ClassEndDate AS ClassLastDate
     FROM   dbo.TblTimLessonStudent tts
            INNER JOIN dbo.TblTimLesson ttl ON ttl.LessonId = tts.LessonId
     WHERE  tts.StudentId IN ({GetSqlIn(studentId)})
            AND AttendStatus IN({GetSqlIn(attendStatus)})
            AND ClassEndDate >= GETDATE()) ,
     A2
AS ( SELECT ROW_NUMBER() OVER ( PARTITION BY ttl.StudentId,ttl.SchoolId
                                ORDER BY ClassEndDate ASC ) AS SN ,
			tts.ReplenishLessonId AS Id,
            tts.SchoolId,
			tts.AttendStatus,
			tts.AdjustType,
			tts.ReplenishCode,
			tts.AttendUserType,
			tts.AttendDate,
			1 AS TblType,
            ttl.LessonId ,
            ttl.StudentId ,
            ttl.ClassEndDate AS ClassLastDate
     FROM   dbo.TblTimReplenishLesson tts
            INNER JOIN dbo.TblTimLesson ttl ON ttl.LessonId = tts.LessonId
     WHERE  tts.StudentId IN ({GetSqlIn(studentId)})
            AND AttendStatus IN({GetSqlIn(attendStatus)})
            AND ClassEndDate >= GETDATE()) ,
     B1
AS ( SELECT ROW_NUMBER() OVER ( PARTITION BY ttl.StudentId,ttl.SchoolId
                                ORDER BY ClassBeginDate ASC ) AS SN ,
			tts.LessonStudentId AS Id,
            tts.SchoolId,
			tts.AttendStatus,
			tts.AdjustType,
			tts.ReplenishCode,
			tts.AttendUserType,
			tts.AttendDate,
			2 AS TblType,
            ttl.LessonId ,
            ttl.StudentId ,
            ttl.ClassBeginDate
     FROM   dbo.TblTimLessonStudent tts
            INNER JOIN dbo.TblTimLesson ttl ON ttl.LessonId = tts.LessonId
     WHERE  tts.StudentId IN ({GetSqlIn(studentId)})
            AND AttendStatus IN({GetSqlIn(attendStatus)})
            AND ClassBeginDate >= GETDATE()) ,
     B2
AS ( SELECT ROW_NUMBER() OVER ( PARTITION BY ttl.StudentId,ttl.SchoolId
                                ORDER BY ClassBeginDate ASC ) AS SN ,
			tts.ReplenishLessonId AS Id,
            tts.SchoolId,
			tts.AttendStatus,
			tts.AdjustType,
			tts.ReplenishCode,
			tts.AttendUserType,
			tts.AttendDate,
			2 AS TblType,
            ttl.LessonId ,
            ttl.StudentId ,
            ttl.ClassBeginDate
     FROM   dbo.TblTimReplenishLesson tts
            INNER JOIN dbo.TblTimLesson ttl ON ttl.LessonId = tts.LessonId
     WHERE  tts.StudentId IN ({GetSqlIn(studentId)})
            AND AttendStatus IN({GetSqlIn(attendStatus)})
            AND ClassBeginDate >= GETDATE()) ,
     C
AS ( SELECT * FROM   A1 WHERE  A1.SN = 1
     UNION
     SELECT * FROM   A2 WHERE  A2.SN = 1
     UNION
     SELECT * FROM   B1 WHERE  B1.SN = 1
     UNION
	 SELECT * FROM   B2 WHERE  B2.SN = 1 
	) ,
     D
AS ( SELECT ROW_NUMBER() OVER ( PARTITION BY C.StudentId,C.SchoolId
                                ORDER BY ClassLastDate ASC ) AS SN ,
				c.Id,
				AttendStatus,
				AdjustType,
				ReplenishCode,
				AttendUserType,
				AttendDate,
				TblType,
				C.LessonId ,
				C.ClassLastDate
     FROM   C )
SELECT 
        ttl.BusinessId,
		D.Id,
		D.AttendStatus,
		D.AdjustType,
		D.ReplenishCode,
		D.AttendUserType,
		D.AttendDate,
		D.TblType,
	   D.LessonId ,
	   D.ClassLastDate,
	   ttl.EnrollOrderItemId,
	   ttl.TermId,
	   ttl.ClassId,
	   ttl.CourseId,
	   ttl.CourseLevelId,
	   ttl.ClassRoomId,
	   ttl.TeacherId,
	   ttl.LessonType,
	   ttl.LessonCount,
       ttl.SchoolId ,
       ttl.StudentId,
	   ttl.ClassBeginTime,
	   ttl.ClassEndTime,
	   ttl.ClassDate
FROM   D
       INNER JOIN dbo.TblTimLesson ttl ON ttl.LessonId = D.LessonId
WHERE  SN = 1;";

            //LogWriter.Write(this, sql, LoggerType.Info);

            return CurrentContext.ViewCompleteStudentAttendance.FromSql(sql.ToString()).ToList();
        }

        /// <summary>
        /// 班级是否已被排课
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-25</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <returns>true:已使用 false:未使用</returns>
        public bool ClassIsUse(long classId)
        {
            return this.BuildBasisQueryable.Any(x => x.ClassId == classId);
        }

        /// <summary>
        /// 获取课次列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-25</para>
        /// </summary>
        /// <param name="lessonId">课次Id</param>
        /// <returns>课次列表</returns>
        public List<ViewCompleteStudentAttendance> GetLessonList(IEnumerable<long> lessonId)
        {
            return this.BuildBasisQueryable.Where(x => lessonId.Contains(x.LessonId)).ToList();
        }

        /// <summary>
        /// 获取课次列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-25</para>
        /// </summary>
        /// <param name="lessonId">课次Id</param>
        /// <returns>课次列表</returns>
        public ViewCompleteStudentAttendance GetLesson(long lessonId)
        {
            return this.BuildBasisQueryable.Where(x => x.LessonId == lessonId).FirstOrDefault();
        }
    }
}
