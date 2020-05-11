using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using AMS.Dto;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描述：获取课次信息 
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2019-3-12</para>
    /// </summary>
    public class ViewTimAttendLessonRepository : BaseRepository<ViewTimAttendLesson>
    {
        public ViewTimAttendLessonRepository(DbContext context) : base(context) { }

        public ViewTimAttendLessonRepository() { }

        //课次查询链接字符串
        private string strSql = @"select  a.LessonId,
	                                      b.TeacherId,
                                          b.StudentId,
                                          b.ClassId,
                                          b.ClassDate,
                                          b.ClassBeginTime,
                                          b.ClassEndTime
                                   from (
                                           select LessonId from [dbo].[TblTimLessonStudent] where SchoolId=@SchoolId and AttendStatus=0 and AdjustType=0
                                           union all
                                           select LessonId from [dbo].[TblTimReplenishLesson] where SchoolId=@SchoolId and AttendStatus=0 and AdjustType=0
                                        ) as a
                                   left join [dbo].[TblTimLesson] as b on a.LessonId=b.LessonId 
                                   where b.LessonType=1";




        /// <summary>
        /// 描述：获取老师的考勤课次信息
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-12</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="teacherId">老师Id</param>
        /// <returns>考勤课次信息</returns>
        public List<ViewTimAttendLesson> GetTeacherTimAttendLessonList(string schoolId, string teacherId)
        {
            var str = new StringBuilder();
            str.Append(strSql);
            str.Append(" and b.TeacherId=@TeacherId");

            var attendLessonList = base.CurrentContext.ViewTimAttendLesson.FromSql(str.ToString(), new object[] {
                new SqlParameter("@SchoolId",schoolId),
                new SqlParameter("@TeacherId",teacherId)
            }).ToList();
            return attendLessonList;
        }

        /// <summary>
        /// 描述：获取某一天的考勤课次信息
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-12</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="classDate">上课日期</param>
        /// <returns>考勤课次信息</returns>
        public List<ViewTimAttendLesson> GetClassDateTimAttendLessonList(string schoolId, DateTime classDate)
        {
            var str = new StringBuilder();
            str.Append(strSql);
            str.Append(" and b.ClassDate=@ClassDate");

            var attendLessonList = base.CurrentContext.ViewTimAttendLesson.FromSql(str.ToString(), new object[] {
                new SqlParameter("@SchoolId",schoolId),
                new SqlParameter("@ClassDate",classDate)
            }).ToList();
            return attendLessonList;
        }

        /// <summary>
        /// 描述：获取某日期某个时间段的课次信息
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-13</para>
        /// </summary>
        /// <returns>考勤课次信息</returns>
        public List<ViewTimAttendLesson> GetClassTimeTimAttendLessonList(string schoolId, DateTime classDate, string classBeginTime, string classEndTime)
        {
            var str = new StringBuilder();
            str.Append(strSql);
            str.Append(" and b.ClassDate=@ClassDate ");
            str.Append(" and b.ClassBeginTime=@ClassBeginTime ");
            str.Append(" and b.ClassEndTime=@ClassEndTime ");

            var attendLessonList = base.CurrentContext.ViewTimAttendLesson.FromSql(str.ToString(), new object[] {
                new SqlParameter("@SchoolId",schoolId),
                new SqlParameter("@ClassDate",classDate),
                new SqlParameter("@ClassBeginTime",classBeginTime),
                new SqlParameter("@ClassEndTime",classEndTime)
            }).ToList();
            return attendLessonList;
        }


        /// <summary>
        /// 描述：获取某些天老师的课次信息
        /// <para>作者：Huang GaoLiang</para>
        /// <para>创建时间：2019-03-26</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="teacherId">老师编号</param>
        /// <param name="classDates">上课日期</param>
        /// <returns>考勤课次信息</returns>
        public List<ViewTimAttendLesson> GetClassDateTimAttendLessonList(string schoolId, string teacherId, List<DateTime> classDates)
        {
            var str = new StringBuilder();
            str.Append(strSql);

            var attendLessonList = base.CurrentContext.ViewTimAttendLesson.FromSql(str.ToString(), new object[] {
                new SqlParameter("@SchoolId",schoolId)
            })
            .Where(m => classDates.Contains(m.ClassDate) && m.TeacherId == teacherId)
            .ToList();
            return attendLessonList;
        }

    }
}
