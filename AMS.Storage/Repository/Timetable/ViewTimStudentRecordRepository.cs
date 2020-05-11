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
    /// 描    述：学生记录信息
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public class ViewTimStudentRecordRepository : BaseRepository<ViewTimStudentStudyRecord>
    {
        /// <summary>
        /// 学生记录信息仓储实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        public ViewTimStudentRecordRepository()
        {
        }

        /// <summary>
        /// 学生记录信息仓储实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        public ViewTimStudentRecordRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 获取学生学习记录列表信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-07</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <param name="schoolId">校区Id</param>
        /// <param name="yearList">年度列表</param>
        /// <returns>学生学习记录列表</returns>
        public async Task<List<ViewTimStudentStudyRecord>> GetStudentRecordListAsync(string schoolId, long studentId, List<int> yearList)
        {
            #region 获取学生记录SQL语句
            string querySql = $@"WITH ta
                                         AS (SELECT c.ClassId, 
                                                    c.ClassNo, 
                                                    (CASE j.WeekDay
                                                         WHEN 0
                                                         THEN '周日'
                                                         WHEN 1
                                                         THEN '周一'
                                                         WHEN 2
                                                         THEN '周二'
                                                         WHEN 3
                                                         THEN '周三'
                                                         WHEN 4
                                                         THEN '周四'
                                                         WHEN 5
                                                         THEN '周五'
                                                         WHEN 6
                                                         THEN '周六'
                                                         WHEN 7
                                                         THEN '周日'
                                                         ELSE ''
                                                     END) WeekDay, 
                                                    (CASE j.WeekDay
                                                         WHEN 0
                                                         THEN 7
                                                         ELSE j.WeekDay
                                                     END) weekOrder, 
                                                    (j.BeginTime+'-'+j.EndTime) Times
                                             FROM TblDatClass c
                                                  JOIN TblTimClassTime i ON c.ClassId = i.ClassId
                                                  JOIN TblDatSchoolTime j ON j.SchoolTimeId = i.SchoolTimeId),
                                         tb
                                         AS (SELECT DISTINCT 
                                                    ClassId, 
                                                    weekOrder, 
                                                    WeekDay
                                             FROM ta),
                                         tc
                                         AS (SELECT DISTINCT 
                                                    ClassId, 
                                                    Times
                                             FROM ta),
                                         td
                                         AS (SELECT a.ClassId, 
                                                    a.ClassNo, 
                                                    '（'+STUFF(
                                             (
                                                 SELECT '，'+Times
                                                 FROM tc
                                                 WHERE ClassId = a.ClassId FOR XML PATH('')
                                             ), 1, 1, '')+'）' Times, 
                                                    STUFF(
                                             (
                                                 SELECT '、'+WeekDay
                                                 FROM tb
                                                 WHERE ClassId = a.ClassId
                                                 ORDER BY weekOrder FOR XML PATH('')
                                             ), 1, 1, '') WeekDay
                                             FROM ta a
                                             GROUP BY a.ClassId, 
                                                      a.ClassNo)
                                         SELECT a.EnrollOrderItemId,
                                                (CASE a.BusinessType
                                                    WHEN 4
                                                    THEN i.LifeClassCode
                                                    ELSE c.ClassNo
                                                END) ClassNo, --班级代码
                                               (CASE a.BusinessType
                                                    WHEN 4
                                                    THEN(CONVERT(VARCHAR(16), i.ClassBeginTime, 120)+'-'+CONVERT(VARCHAR(16), i.ClassEndTime, 120))
                                                    ELSE(c.WeekDay + c.Times)
                                                END) ClassDate, --上课时间
                                               (CASE a.BusinessType
                                                    WHEN 4
                                                    THEN i.TeacherId
                                                    ELSE a.TeacherId
                                                END) TeacherId, --上课教师Id
                                               a.SchoolId, --校区Id
                                               a.StudentId, --学生Id
                                               a.ArrangedClassTimes, --排课课次
                                               ISNULL(b.AttendedClassTimes, 0) AttendedClassTimes, --已上课次
                                               (a.ArrangedClassTimes - ISNULL(b.AttendedClassTimes, 0)) RemainingClassTimes, --剩余课次
                                               (CASE a.BusinessType
                                                    WHEN 4
                                                    THEN i.Title
                                                    ELSE e.ShortName+replace(replace(f.LevelCnName, '无', ''), '级', '')
                                                END) CourseName, --课程名称
                                               d.Year, --年度
                                               d.ClassTimes, --报名课次
                                               d.TermTypeId, --学期类型Id
                                               g.TermName, --学期
                                               (CASE a.BusinessType
                                                    WHEN 4
                                                    THEN i.Place
                                                    ELSE h.RoomNo
                                                END) RoomNo   --上课教室
                                         FROM
                                         (
                                             SELECT k.EnrollOrderItemId, 
                                                    k.ClassId, 
                                                    k.TeacherId, 
                                                    k.TermId, 
                                                    k.SchoolId, 
                                                    k.StudentId, 
                                                    k.ClassRoomId, 
                                                    k.CourseId, 
                                                    k.CourseLevelId, 
                                                    k.BusinessId, 
                                                    k.BusinessType,
                                                    --排课课次
                                                    SUM(k.LessonCount) ArrangedClassTimes
                                             FROM TblTimLessonStudent j
                                                  JOIN TblTimLesson k ON j.LessonId = k.LessonId
                                             WHERE k.STATUS = @Status
                                             GROUP BY k.EnrollOrderItemId, 
                                                      k.ClassId, 
                                                      k.TeacherId, 
                                                      k.TermId, 
                                                      k.SchoolId, 
                                                      k.StudentId, 
                                                      k.ClassRoomId, 
                                                      k.CourseId, 
                                                      k.BusinessId, 
                                                      k.BusinessType, 
                                                      k.CourseLevelId
                                         ) a
                                         LEFT JOIN
                                         (
                                             SELECT k.EnrollOrderItemId, 
                                                    k.BusinessId, 
                                                    k.BusinessType,
                                                    --已上课次
                                                    SUM(k.LessonCount) attendedClassTimes
                                             FROM TblTimLessonStudent j
                                                  JOIN TblTimLesson k ON j.LessonId = k.LessonId
                                             WHERE k.STATUS = @Status
                                                   AND k.ClassDate < GETDATE()
                                             GROUP BY k.EnrollOrderItemId, 
                                                      k.BusinessId, 
                                                      k.BusinessType
                                         ) b ON a.EnrollOrderItemId = b.EnrollOrderItemId
                                                AND a.BusinessId = b.BusinessId
                                                AND a.BusinessType = b.BusinessType
                                         JOIN TblOdrEnrollOrderItem d ON d.EnrollOrderItemId = a.EnrollOrderItemId
                                         JOIN td c ON c.ClassId = a.ClassId
                                         LEFT JOIN TblDatCourse e ON e.CourseId = a.CourseId
                                         LEFT JOIN TblDatCourseLevel f ON f.CourseLevelId = a.CourseLevelId
                                         JOIN TblDatTerm g ON g.TermId = a.TermId
                                         LEFT JOIN TblDatClassRoom h ON h.ClassRoomId = a.ClassRoomId
                                         LEFT JOIN dbo.TblTimLifeClass i ON i.LifeClassId = a.BusinessId
                                                                            AND a.BusinessType = 4
                                         WHERE a.StudentId = @StudentId
                                               AND a.SchoolId = @SchoolId
                                        ORDER BY d.Year, 
                                                  a.TermId, 
                                                  a.EnrollOrderItemId ";
            #endregion

            return await base.CurrentContext.ViewTimStudentRecord.FromSql(querySql, new SqlParameter[] {
                new SqlParameter("@Status",(int)LessonUltimateStatus.Normal),
                new SqlParameter("@StudentId", studentId),
                new SqlParameter("@SchoolId", schoolId)
            }).ToListAsync();
        }
    }
}
