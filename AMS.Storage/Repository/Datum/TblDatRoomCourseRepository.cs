/*此代码由生成工具字段生成，生成时间2018/9/7 12:42:58 */
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AMS.Storage.Context;
using AMS.Storage.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblDatRoomCourse仓储
    /// </summary>
    public class TblDatRoomCourseRepository : BaseRepository<TblDatRoomCourse>
    {
        public TblDatRoomCourseRepository()
        {
        }

        public TblDatRoomCourseRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 验证(教室+班级名称) 不允许重复。
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-10</para>
        /// </summary>
        /// <param name="classId">教室Id</param>
        /// <param name="courseId">课程Id(别名:班级Id)</param>
        /// <returns>true:通过 false:不通过</returns>
        public async Task<bool> VerificationCourseAsync(long classId, long courseId)
        {
            return await base.IsExistAsync(m => m.ClassRoomId == classId && m.CourseId == courseId);
        }

        /// <summary>
        /// 验证(教室+班级名称) 不允许重复。
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-10</para>
        /// </summary>
        /// <param name="roomCourseId">班级课程id</param>
        /// <param name="classId">教室Id</param>
        /// <param name="courseId">课程Id(别名:班级Id)</param>
        /// <returns>true:通过 false:不通过</returns>
        public async Task<bool> VerificationCourseAsync(long roomCourseId, long classId, long courseId)
        {
            return await base.IsExistAsync(m => m.ClassRoomId == classId && m.CourseId == courseId && roomCourseId != m.RoomCourseId);
        }

        /// <summary>
        /// 课程是否被使用
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-15</para>
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <returns>true:已使用 false:未使用</returns>
        public async Task<bool> CourseIsUse(long courseId)
        {
            return await base.IsExistAsync(x => x.CourseId == courseId);
        }

        /// <summary>
        /// 根据教室主键和课程主键查找教室课程信息
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-23 </para>
        /// </summary>
        /// <param name="classRoomId">教室主键</param>
        /// <param name="courseId">课程主键</param>
        /// <returns>获取教室课程信息</returns>
        public async Task<TblDatRoomCourse> GetDatRoomCourseByWhere(long classRoomId, long courseId)
        {
            return await base.LoadTask(m => m.ClassRoomId == classRoomId && m.CourseId == courseId);
        }

        /// <summary>
        /// 获取教室下的所有课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-09</para>
        /// </summary>
        /// <param name="classRoomId">教室Id</param>
        /// <returns>教室课程列表</returns>
        public List<TblDatRoomCourse> GetByClassRoomId(long classRoomId)
        {
            return base.LoadList(x => x.ClassRoomId == classRoomId);
        }
    }
}
