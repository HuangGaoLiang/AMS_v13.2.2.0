using AMS.Core;
using AMS.Dto;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描    述：学生考勤信息
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public class ViewStudentAttendanceRepository : BaseRepository<ViewStudentAttendance>
    {
        #region 考勤信息SQL语句
        private const string LESSON_STUDENT_SQL = @"SELECT B.STUDENTID, --学生ID
       B.LESSONID, --学生课次Id
       A.LESSONSTUDENTID STUDENTLESSONID, --学生考勤课程ID       
       B.CLASSDATE, --上课日期
       B.CLASSBEGINTIME, --上课开始时间
       B.CLASSENDTIME, --下课结束时间
       B.CLASSBEGINDATE, --上课开始日期
       B.CLASSENDDATE, --下课结束日期
       B.LESSONTYPE, --课程类型
       OI.CLASSTIMES CLASSLESSONCOUNT, --报名课次
       B.LESSONCOUNT, --占用课次
       A.ATTENDUSERTYPE, --签到人员类型
       (CASE B.LESSONTYPE
            WHEN 2
            THEN I.TEACHERID
            ELSE B.TEACHERID
        END) TEACHERID, --考勤老师ID
       C.TEACHERID CLASSTEACHERID, --班级老师ID
       B.COURSEID, --课程ID
       D.CLASSCNNAME CLASSNAME, --班级名称
       B.CLASSID, --班级Id
       C.CLASSNO, --班级代码
       (CASE B.LESSONTYPE
            WHEN 2
            THEN I.PLACE
            ELSE F.ROOMNO
        END) ROOMNO, --上课地点       
       (CASE
        --ADJUSTTYPE为5表示补签已确认，则相当于已考勤
            WHEN A.ADJUSTTYPE = 5
            THEN 1
        --未考勤且上课开始日期小于当前时间，则不需要显示状态
            WHEN A.ATTENDSTATUS = 0
                 AND CONVERT(DATETIME, (CASE B.LESSONTYPE
                                            WHEN 2
                                            THEN B.CLASSBEGINTIME
                                            ELSE CONVERT(VARCHAR(100), B.CLASSDATE, 23)+' '+B.CLASSENDTIME
                                        END), 101) > GETDATE()
            THEN-1
        --未考勤且上课结束日期已大于当前时间，则为缺勤
            WHEN A.ATTENDSTATUS = 0
                 AND CONVERT(DATETIME, (CASE B.LESSONTYPE
                                            WHEN 2
                                            THEN B.CLASSENDTIME
                                            ELSE CONVERT(VARCHAR(100), B.CLASSDATE, 23)+' '+B.CLASSENDTIME
                                        END), 101) < GETDATE()
            THEN 8
        --未考勤/缺勤，且已补课/已调课的课程已考勤，则为已考勤
            WHEN A.ATTENDSTATUS = 0
                 AND ISNULL(REPLENISH_COUNT, 0) > 0
            THEN 1
            ELSE A.ATTENDSTATUS
        END) ATTENDSTATUS, --ATTENDSTATUS：0未考勤1已考勤2请假8缺勤9正常
       (CASE
        --已考勤
            WHEN A.ATTENDSTATUS = 1
            THEN 1
        --已安排补课/已经安排补课周补课
            WHEN A.ADJUSTTYPE = 1
                 OR A.ADJUSTTYPE = 3
            THEN 11
        --已安排调课
            WHEN A.ADJUSTTYPE = 2
            THEN 12
            ELSE-1
        END) LESSONSTATUS, --LESSONSTATUS：-1不显示0未考勤1已考勤11已经安排补课12已安排调课
       (CASE
        --ADJUSTTYPE:4补签未确认和5补签已确认都为补签，则ADJUSTTYPE为0表示非(补课/调课)
            WHEN A.ADJUSTTYPE = 4
                 OR A.ADJUSTTYPE = 5
            THEN 0
            ELSE A.ADJUSTTYPE
        END) ADJUSTTYPE, --补课/调课
       A.ATTENDDATE, --考勤时间
       B.BUSINESSTYPE, --课程业务类型
       1 PROCESSSTATUS --课程处理状态:1创建2作废
FROM TBLTIMLESSONSTUDENT A 
     --课程基础信息
     JOIN TBLTIMLESSON B ON A.LESSONID = B.LESSONID
     --班级信息
     LEFT JOIN TBLDATCLASS C ON C.CLASSID = B.CLASSID
     LEFT JOIN TBLDATCOURSE D ON D.COURSEID = B.COURSEID
     --课室信息
     LEFT JOIN TBLDATCLASSROOM F ON F.CLASSROOMID = B.CLASSROOMID
     LEFT JOIN TBLTIMLIFECLASS I ON I.LIFECLASSID = B.BUSINESSID
                                    AND B.LESSONTYPE = 2
     --报名课程明细
     LEFT JOIN TBLODRENROLLORDERITEM OI ON OI.ENROLLORDERITEMID = B.ENROLLORDERITEMID
     --获取对应的补课/调课的课程是否已考勤
     LEFT JOIN
(
    SELECT ROOTLESSONID, 
           COUNT(ROOTLESSONID) REPLENISH_COUNT
    FROM TBLTIMREPLENISHLESSON
    --ATTENDSTATUS：1已考勤和ADJUSTTYPE：5补签已确认
    WHERE ATTENDSTATUS = 1
          OR ADJUSTTYPE = 5
    GROUP BY ROOTLESSONID
) J ON J.ROOTLESSONID = A.LESSONID ";
        #endregion

        #region 考勤统计表SQL语句
        private const string LESSON_STUDENT_FINANCE_SQL = @"SELECT A.STUDENTID, --学生ID
       A.LESSONID, --学生课次ID
       B.LESSONSTUDENTID STUDENTLESSONID, --学生考勤课程ID   
       A.CLASSDATE, --上课日期
       A.CLASSBEGINTIME, --上课开始时间
       A.CLASSENDTIME, --下课结束时间
       A.CLASSBEGINDATE, --上课开始日期
       A.CLASSENDDATE, --下课结束日期
       A.LESSONTYPE, --课程类型
       OI.CLASSTIMES CLASSLESSONCOUNT, --报名课次
       A.LESSONCOUNT, --占用课次
       B.ATTENDUSERTYPE, --签到人员类型
       (CASE A.LESSONTYPE
            WHEN 2
            THEN I.TEACHERID
            ELSE A.TEACHERID
        END) TEACHERID, --考勤老师ID
       C.TEACHERID CLASSTEACHERID, --班级老师ID
       A.COURSEID, --课程ID
       '' CLASSNAME, --班级名称
       A.CLASSID, --班级ID
       C.CLASSNO, --班级代码
       '' ROOMNO, --上课地点       
       (CASE
        --ADJUSTTYPE为5表示补签已确认，则相当于已考勤
            WHEN B.ADJUSTTYPE = 5
            THEN 1
        --未考勤且上课开始日期小于当前时间，则不需要显示状态
            WHEN B.ATTENDSTATUS = 0
                 AND CONVERT(DATETIME, (CASE A.LESSONTYPE
                                            WHEN 2
                                            THEN A.CLASSBEGINTIME
                                            ELSE CONVERT(VARCHAR(100), A.CLASSDATE, 23)+' '+A.CLASSENDTIME
                                        END), 101) > GETDATE()
            THEN-1
        --未考勤且上课结束日期已大于当前时间，则为缺勤
            WHEN B.ATTENDSTATUS = 0
                 AND CONVERT(DATETIME, (CASE A.LESSONTYPE
                                            WHEN 2
                                            THEN A.CLASSENDTIME
                                            ELSE CONVERT(VARCHAR(100), A.CLASSDATE, 23)+' '+A.CLASSENDTIME
                                        END), 101) < GETDATE()
            THEN 8
        --未考勤/缺勤，且已补课/已调课的课程已考勤，则为已考勤
            WHEN B.ATTENDSTATUS = 0
                 AND ISNULL(J.REPLENISH_COUNT, 0) > 0
            THEN 1
            ELSE B.ATTENDSTATUS
        END) ATTENDSTATUS, --ATTENDSTATUS：0未考勤1已考勤2请假8缺勤9正常
       (CASE
        --已考勤
            WHEN B.ATTENDSTATUS = 1
            THEN 1
        --已安排补课/已经安排补课周补课
            WHEN B.ADJUSTTYPE = 1
                 OR B.ADJUSTTYPE = 3
            THEN 11
        --已安排调课
            WHEN B.ADJUSTTYPE = 2
            THEN 12
            ELSE-1
        END) LESSONSTATUS, --LESSONSTATUS：-1不显示0未考勤1已考勤11已经安排补课12已安排调课
       (CASE
        --ADJUSTTYPE:4补签未确认和5补签已确认都为补签，则ADJUSTTYPE为0表示非(补课/调课)
            WHEN B.ADJUSTTYPE = 4
                 OR B.ADJUSTTYPE = 5
            THEN 0
            ELSE ISNULL(B.ADJUSTTYPE, 0)
        END) ADJUSTTYPE, --补课/调课
       B.ATTENDDATE, --考勤时间
       K.BUSINESSTYPE, --课程业务类型
       CAST(K.PROCESSSTATUS AS INT) PROCESSSTATUS --课程处理状态:1创建2作废
FROM
(--常规课课程信息
    SELECT *, 
           1 RN
    FROM TBLTIMLESSON
    WHERE LESSONTYPE = 1
          AND BUSINESSTYPE = 1
    UNION ALL
    --写生课课程信息
    SELECT *
    FROM
    (
        SELECT *, 
               ROW_NUMBER() OVER(PARTITION BY STUDENTID, 
                                              BUSINESSID, 
                                              BUSINESSTYPE
               ORDER BY CREATETIME DESC) RN
        FROM TBLTIMLESSON
        WHERE LESSONTYPE = 2
    ) A
    WHERE RN = 1
) A
--课程基础信息
LEFT JOIN TBLTIMLESSONSTUDENT B ON B.LESSONID = A.LESSONID
--班级信息
LEFT JOIN TBLDATCLASS C ON C.CLASSID = A.CLASSID
LEFT JOIN TBLTIMLIFECLASS I ON I.LIFECLASSID = A.BUSINESSID
                               AND A.LESSONTYPE = 2
--报名课程明细
LEFT JOIN TBLODRENROLLORDERITEM OI ON OI.ENROLLORDERITEMID = A.ENROLLORDERITEMID
--获取对应的补课/调课的课程是否已考勤
LEFT JOIN
(
    SELECT ROOTLESSONID, 
           COUNT(ROOTLESSONID) REPLENISH_COUNT
    FROM TBLTIMREPLENISHLESSON
    --ATTENDSTATUS：1已考勤和ADJUSTTYPE：5补签已确认
    WHERE ATTENDSTATUS = 1
          OR ADJUSTTYPE = 5
    GROUP BY ROOTLESSONID
) J ON J.ROOTLESSONID = A.LESSONID
--获取课程变更记录信息
LEFT JOIN
(
    SELECT LESSONID, 
           BUSINESSTYPE, 
           PROCESSSTATUS
    FROM
    (
        SELECT LESSONID, 
               PROCESSSTATUS, 
               BUSINESSTYPE, 
               ROW_NUMBER() OVER(PARTITION BY LESSONID
               ORDER BY PROCESSSTATUS DESC) RN
        FROM TBLTIMLESSONPROCESS
    ) T
    WHERE T.RN = 1
) K ON K.LESSONID = A.LESSONID ";
        #endregion

        #region 获取学生考勤详情信息（常规课、补课和调课等课程信息）SQL语句
        private const string LESSON_WITH_REPLENISH_SQL = @"SELECT A.STUDENTID, --学生ID
       A.LESSONID, --学生课次Id
       B.LESSONSTUDENTID STUDENTLESSONID, --学生考勤课程ID    
       A.CLASSDATE, --上课日期
       A.CLASSBEGINTIME, --上课开始时间
       A.CLASSENDTIME, --上课结束时间
       A.CLASSBEGINDATE, --上课开始日期
       A.CLASSENDDATE, --下课结束日期
       A.LESSONTYPE, --课程类型
       OI.CLASSTIMES CLASSLESSONCOUNT, --报名课次
       A.LESSONCOUNT, --占用课次
       B.ATTENDUSERTYPE, --签到人员类型
       (CASE A.LESSONTYPE
            WHEN 2
            THEN I.TEACHERID
            ELSE A.TEACHERID
        END) TEACHERID, --考勤老师ID
       C.TEACHERID CLASSTEACHERID, --班级老师ID
       A.COURSEID, --课程ID
       D.CLASSCNNAME CLASSNAME, --班级名称
       A.CLASSID, --班级Id
       C.CLASSNO, --班级代码
       (CASE A.LESSONTYPE
            WHEN 2
            THEN I.PLACE
            ELSE F.ROOMNO
        END) ROOMNO, --上课地点
       (CASE
        --补签相当于已考勤
            WHEN B.ATTENDSTATUS = 1
                 OR B.ADJUSTTYPE = 5
            THEN 1
        --未考勤且上课结束日期已大于当前时间，则为缺勤
            WHEN B.ATTENDSTATUS = 0
                 AND CONVERT(DATETIME, (CASE A.LESSONTYPE
                                            WHEN 2
                                            THEN A.CLASSENDTIME
                                            ELSE CONVERT(VARCHAR(100), A.CLASSDATE, 23)+' '+A.CLASSENDTIME
                                        END), 101) < GETDATE()
            THEN 8
            ELSE B.ATTENDSTATUS
        END) ATTENDSTATUS, --ATTENDSTATUS：0未考勤9正常2请假8缺勤
       (CASE
        --已考勤
            WHEN B.ATTENDSTATUS = 1
            THEN 1
        --已安排补课/已经安排补课周补课
            WHEN B.ADJUSTTYPE = 1
                 OR B.ADJUSTTYPE = 3
            THEN 11
        --已安排调课
            WHEN B.ADJUSTTYPE = 2
            THEN 12
            ELSE-1
        END) LESSONSTATUS, --LESSONSTATUS：0未考勤1已考勤11已经安排补课12已安排调课
       (CASE
        --ADJUSTTYPE:4补签未确认和5补签已确认都为补签，则ADJUSTTYPE为0表示非(补课/调课)
            WHEN B.ADJUSTTYPE = 4
                 OR B.ADJUSTTYPE = 5
            THEN 0
            ELSE B.ADJUSTTYPE
        END) ADJUSTTYPE, --补课/调课
       B.ATTENDDATE, --考勤时间
       A.BUSINESSTYPE, --课程业务类型
       1 PROCESSSTATUS --课程处理状态
FROM TBLTIMLESSON A
     --课程基础信息
     JOIN
(
    SELECT STUDENTID, 
           ATTENDSTATUS, 
           LESSONID ROOTLESSONID, 
           LESSONSTUDENTID, 
           LESSONID, 
           ATTENDDATE, 
           ATTENDUSERTYPE, --签到人员类型
           0 ADJUSTTYPE--0表示常规课
    FROM TBLTIMLESSONSTUDENT
    UNION ALL
    SELECT STUDENTID, 
           ATTENDSTATUS, 
           ROOTLESSONID, 
           REPLENISHLESSONID, 
           LESSONID, 
           ATTENDDATE, 
           ATTENDUSERTYPE, --签到人员类型
           1 ADJUSTTYPE --1表示补课/调课
    FROM TBLTIMREPLENISHLESSON
) B ON B.LESSONID = A.LESSONID
     --班级信息
     LEFT JOIN TBLDATCLASS C ON C.CLASSID = A.CLASSID
     --课程信息
     LEFT JOIN TBLDATCOURSE D ON D.COURSEID = A.COURSEID
     --课室信息
     LEFT JOIN TBLDATCLASSROOM F ON F.CLASSROOMID = A.CLASSROOMID
     --写生课信息
     LEFT JOIN TBLTIMLIFECLASS I ON I.LIFECLASSID = A.BUSINESSID
                                    AND A.LESSONTYPE = 2
     --报名课程明细
     LEFT JOIN TBLODRENROLLORDERITEM OI ON OI.ENROLLORDERITEMID = A.ENROLLORDERITEMID ";
        #endregion

        /// <summary>
        /// 根据校区Id、学生Id和学生考勤查询条件获取学生考勤信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        /// <param name="request">学生考勤请求对象</param>
        /// <returns>学生考勤信息列表</returns>
        public async Task<List<ViewStudentAttendance>> GetLessonStudentListAsync(string schoolId, long studentId, StudentTimeLessonRequest request)
        {
            StringBuilder querySql = new StringBuilder();
            querySql.Append(LESSON_STUDENT_SQL);
            querySql.Append(@" WHERE B.STUDENTID = @StudentId                                                    
                                                    AND B.SCHOOLID = @SchoolId
                                                    AND B.STATUS = @Status ");
            List<SqlParameter> parameterList = new List<SqlParameter>()
            {
                new SqlParameter("@StudentId", studentId),
                new SqlParameter("@SchoolId", schoolId),
                new SqlParameter("@Status", (int)request.LessonStatus)
            };
            if (request.TermIdList != null && request.TermIdList.Count > 0)
            {
                querySql.Append(" AND B.TermId in (select col from FN_SPLIT(@TermId,',')) ");
                parameterList.Add(new SqlParameter("@TermId", string.Join(",", request.TermIdList)));
            }
            if (request.ClassId.HasValue)
            {
                querySql.Append(" AND B.ClassId = @ClassId ");
                parameterList.Add(new SqlParameter("@ClassId", request.ClassId));
            }
            return await CurrentContext.ViewStudentAttendance.FromSql(querySql.ToString(), parameterList.ToArray()).ToListAsync();
        }

        /// <summary>
        /// 根据校区Id、学生Id和学生考勤查询条件获取学生考勤信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="request">学生考勤请求对象</param>
        /// <returns>学生考勤信息列表</returns>
        public async Task<List<ViewStudentAttendance>> GetStudentAttendListAsync(string schoolId, StudentAttendSearchRequest request)
        {
            StringBuilder querySql = new StringBuilder();
            querySql.Append(LESSON_STUDENT_SQL);
            querySql.Append(@" WHERE  B.SCHOOLID = @SchoolId
                                                     AND B.STATUS = @Status ");
            //过滤校区
            List<SqlParameter> parameterList = new List<SqlParameter>()
            {
                new SqlParameter("@SchoolId", schoolId),
                new SqlParameter("@Status", (int)request.LessonStatus)
            };
            //过滤学期
            if (request.TermId.HasValue)
            {
                querySql.Append(" AND B.TERMID = @TermId ");
                parameterList.Add(new SqlParameter("@TermId", request.TermId.Value));
            }
            //过滤班级
            if (request.ClassId.HasValue)
            {
                querySql.Append(" AND B.CLASSID = @ClassId ");
                parameterList.Add(new SqlParameter("@ClassId", request.ClassId.Value));
            }
            return await CurrentContext.ViewStudentAttendance.FromSql(querySql.ToString(), parameterList.ToArray()).ToListAsync();
        }

        /// <summary>
        /// 根据校区Id、学生Id和学生考勤查询条件获取学生考勤信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="request">学生考勤请求对象</param>
        /// <returns>学生考勤信息列表</returns>
        public async Task<List<ViewStudentAttendance>> GetAttendListByFinanceAsync(string schoolId, StudentAttendSearchRequest request)
        {
            StringBuilder querySql = new StringBuilder();
            querySql.Append(LESSON_STUDENT_FINANCE_SQL);
            querySql.Append(@" WHERE  A.SCHOOLID = @SchoolId ");
            //过滤校区
            List<SqlParameter> parameterList = new List<SqlParameter>()
            {
                new SqlParameter("@SchoolId", schoolId)
            };
            //过滤学期
            if (request.TermId.HasValue)
            {
                querySql.Append(" AND A.TERMID = @TermId ");
                parameterList.Add(new SqlParameter("@TermId", request.TermId.Value));
            }
            //过滤班级
            if (request.ClassId.HasValue)
            {
                querySql.Append(" AND A.CLASSID = @ClassId ");
                parameterList.Add(new SqlParameter("@ClassId", request.ClassId.Value));
            }
            return await CurrentContext.ViewStudentAttendance.FromSql(querySql.ToString(), parameterList.ToArray()).ToListAsync();
        }

        /// <summary>
        /// 根据校区Id、学生Id和学生课程考勤Id获取学生课程考勤信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        /// <param name="lessonIdList">学生课程考勤Id</param>
        /// <param name="classDate">上课日期</param>
        /// <param name="lessonStatus">课程状态</param>
        /// <returns>学生考勤列表信息</returns>
        public async Task<List<ViewStudentAttendance>> GetStudentLessonDetailAsync(string schoolId, long studentId, List<long> lessonIdList, DateTime? classDate, int? lessonStatus)
        {
            StringBuilder querySql = new StringBuilder();
            querySql.Append(LESSON_WITH_REPLENISH_SQL);
            querySql.Append(@" WHERE A.STUDENTID = @StudentId
                                                        AND A.SCHOOLID = @SchoolId ");
            List<SqlParameter> parameterList = new List<SqlParameter>()
            {
                new SqlParameter("@StudentId", studentId),
                new SqlParameter("@SchoolId", schoolId)
            };
            //过滤学生课程Id
            if (lessonIdList != null && lessonIdList.Count > 0)
            {
                querySql.Append(" AND B.ROOTLESSONID in (select col from FN_SPLIT(@LessonId,',')) ");
                parameterList.Add(new SqlParameter("@LessonId", string.Join(",", lessonIdList)));
            }
            //过滤上课日期
            if (classDate.HasValue)
            {
                querySql.Append(" AND A.CLASSDATE = @ClassDate ");
                parameterList.Add(new SqlParameter("@ClassDate", classDate.Value));
            }
            //过滤无效的课程
            if (lessonStatus.HasValue)
            {
                querySql.Append(" AND A.STATUS = @Status ");
                parameterList.Add(new SqlParameter("@Status", lessonStatus.Value));
            }
            return await CurrentContext.ViewStudentAttendance.FromSql(querySql.ToString(), parameterList.ToArray()).ToListAsync();
        }
    }
}
