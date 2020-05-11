using AMS.Dto;
using AMS.Models;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描述：老师未上课课次仓储
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2019-3-7</para>
    /// </summary>
    public class ViewTeacherClassTimetableRepository : BaseRepository<ViewTeacherNoAttendLesson>
    {
        /// <summary>
        /// 描述：获取老师未上课课次列表
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-7</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="request">老师上课列表筛选条件</param>
        /// <returns>老师未上课课次列表</returns>
        public List<ViewTeacherNoAttendLesson> GetTeacherClassTimetable(string schoolId, TeacherClassTimetableRequest request)
        {
            //获取的课次只显示常规课，并且不包含补课周的班级，且上课时间大于当前时间
            var querySql = $@"---基本课次信息
                              declare @currentDate datetime=getdate();
							  with teacherLessonList as ( select grl.ClassId,
																 grl.ClassDate,
																 (grl.ClassBeginTime+'-'+grl.ClassEndTime) as ClassTime,
																 grl.TeacherId
														 from	(select ClassId,ClassDate,
																		ClassBeginTime,
																		ClassEndTime,
																		b.TeacherId from (
																							(select LessonId from TblTimLessonStudent where SchoolId=@SchoolId and AttendStatus=0 and AdjustType=0) 
																							union 
																							(select LessonId from TblTimReplenishLesson where SchoolId=@SchoolId and AttendStatus=0 and AdjustType=0)  
																						  ) as a 
																					left join TblTimLesson as b on a.LessonId=b.LessonId 
																					where b.LessonType=1 and b.BusinessType!=5 and CAST(CAST(ClassDate as varchar )+' '+ClassBeginTime as datetime)>=@currentDate
                              group by ClassId,ClassDate,ClassBeginTime,ClassEndTime,b.TeacherId) as grl   --
                              --where CAST(CAST(grl.ClassDate as varchar )+' '+grl.ClassBeginTime as datetime)>=@currentDate
							  ) 
							  ---拼接时间段
							  select les.ClassId,
									les.ClassDate,
									les.ClassTime,
									les.TeacherId,
									tdc.ClassNo
							  from (select tl.ClassId,
										   tl.ClassDate,
										 stuff(
												(select distinct '，'+ ClassTime from teacherLessonList where teacherLessonList.ClassId=tl.ClassId and teacherLessonList.ClassDate=tl.ClassDate for xml path('')) 
												,1,1,''
											  )
										 as ClassTime,
										  tl.TeacherId from teacherLessonList as tl 
							  group by tl.ClassId,tl.ClassDate,tl.TeacherId) as les
							  left join [dbo].[TblDatClass] as tdc on les.ClassId=tdc.ClassId 
                              where 1=1 ";

            if (!string.IsNullOrWhiteSpace(request.TeacherId))  //老师
            {
                querySql += "and les.TeacherId='" + request.TeacherId+"'";
            }
            if (request.BeginClassDate.HasValue)      //上课开始日期
            {
                querySql += " and les.ClassDate >='" + request.BeginClassDate+"'";
            }
            if (request.EndClassDate.HasValue)      //上课结束日期
            {
                querySql += " and les.ClassDate <='" + request.EndClassDate + "'";
            }
            if (request.ClassDate.HasValue)      //上课日期
            {
                querySql += " and les.ClassDate ='" + request.ClassDate + "'";
            }

            var teacherLessonList = base.CurrentContext.ViewTeacherNoAttendLesson.FromSql(querySql, new object[] {
               new SqlParameter("@SchoolId",schoolId.Trim())
            });
            var teacherLessonQuery = teacherLessonList
                                    .OrderBy(x => x.ClassDate)
                                    //.WhereIf(!string.IsNullOrWhiteSpace(request.TeacherId), x => x.TeacherId.Trim() == request.TeacherId)   //老师
                                    //.WhereIf(request.BeginClassDate.HasValue, x => x.ClassDate >= request.BeginClassDate)               //开始日期
                                    //.WhereIf(request.EndClassData.HasValue, x => x.ClassDate <= request.EndClassData.Value.AddDays(1))  //结束日期
                                    //.WhereIf(request.ClassData.HasValue, x => x.ClassDate == request.ClassData)
                                    .ToList();
            return teacherLessonQuery;
        }
    }
}
