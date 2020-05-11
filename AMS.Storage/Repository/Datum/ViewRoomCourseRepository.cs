using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 教室与课程关联视图
    /// </summary>
    public class ViewRoomCourseRepository : BaseRepository<ViewRoomCourse>
    {
        public ViewRoomCourseRepository()
        {
        }

        public ViewRoomCourseRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 获取学期的所有教室与课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-11</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <returns>教室课程列表</returns>
        public List<ViewRoomCourse> Get(string schoolId)
        {

            if (string.IsNullOrWhiteSpace(schoolId))
            {
                throw new ArgumentNullException(nameof(schoolId));
            }

            string querySql = @"
                                SELECT  A.RoomCourseId,
		                                A.CourseId,
		                                A.MaxWeekStage,
		                                A.MaxStageStudents,
		                                A.IsDisabled,
		                                B.ClassRoomId,
		                                B.RoomNo,
                                        ISNULL(C.CourseCnName,'') AS CourseCnName,
										ISNULL(C.ClassCnName,'') AS ClassCnName
                                FROM TblDatRoomCourse AS A
                                LEFT JOIN TblDatClassRoom AS B ON B.ClassRoomId=A.ClassRoomId
                                LEFT JOIN TblDatCourse AS C ON C.CourseId=A.CourseId
                                WHERE B.SchoolId=@SchoolId
                                ORDER BY IsDisabled DESC,RoomNo ASC
                                ";

            return base.CurrentContext.ViewRoomCourse.FromSql(querySql, new SqlParameter[] {
                new SqlParameter("@SchoolId", schoolId)
            }).ToList();
        }
    }
}
