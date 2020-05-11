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
    /// 描    述：班级的人数信息
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-12</para>
    /// </summary>
    public class ViewTimLifeClassStudentRepository : BaseRepository<ViewTimLifeClassStudent>
    {
        /// <summary>
        /// 根据校区Id、学期Id和课程Id等获取班级的人数信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="request">写生课学生请求对象</param>
        /// <returns>班级学生人数列表</returns>
        public async Task<List<ViewTimLifeClassStudent>> GetLifeClassStudentListAsync(string schoolId, LifeClassStudentRequest request)
        {
            #region 获取学生记录SQL语句
            string querySql = @"WITH ta
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
                                     SELECT ta.*, 
                                            td.ClassNo, 
                                            (td.WeekDay + td.Times) WeekTimes
                                     FROM
                                     (
                                         SELECT aa.SchoolId, 
                                                aa.ClassId, 
                                                aa.TermId, 
                                                COUNT(aa.StudentId) PersonNumber
                                         FROM
                                         (
                                            {0}
                                         ) aa
                                         GROUP BY aa.SchoolId, 
                                                  aa.ClassId, 
                                                  aa.TermId
                                     ) ta
                                     JOIN td ON td.ClassId = ta.ClassId ";

            string subSql = @"SELECT DISTINCT 
                                    a.SchoolId, 
                                    a.StudentId, 
                                    b.ClassId, 
                                    b.TermId
                                FROM TblTimLessonStudent a
                                    JOIN TblTimLesson b ON a.LessonId = b.LessonId
                                    JOIN TblOdrEnrollOrderItem c ON c.EnrollOrderItemId = b.EnrollOrderItemId
                                WHERE b.STATUS = 1 
                                  AND b.SchoolId = @SchoolId
                                  AND b.TermId = @TermId ";
            #endregion

            List<SqlParameter> parameterList = new List<SqlParameter>() {
                new SqlParameter("@SchoolId", schoolId),
                new SqlParameter("@TermId", request.TermId)
            };
            if (request.CourseId.HasValue)
            {
                subSql += " AND b.CourseId = @CourseId ";
                parameterList.Add(new SqlParameter("@CourseId", request.CourseId));
            }
            if (request.CourseLevelId.HasValue)
            {
                subSql += " AND b.CourseLevelId = @CourseLevelId ";
                parameterList.Add(new SqlParameter("@CourseLevelId", request.CourseLevelId));
            }
            return await CurrentContext.ViewTimLifeClassStudent.FromSql(string.Format(querySql, subSql), parameterList.ToArray()).ToListAsync();
        }
    }
}
