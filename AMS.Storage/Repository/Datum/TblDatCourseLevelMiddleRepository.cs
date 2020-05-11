/*此代码由生成工具字段生成，生成时间2018/9/14 10:04:46 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblDatCourseLevelMiddle仓储
    /// </summary>
    public class TblDatCourseLevelMiddleRepository : BaseRepository<TblDatCourseLevelMiddle>
    {
        public TblDatCourseLevelMiddleRepository(DbContext context) : base(context)
        {

        }

        public TblDatCourseLevelMiddleRepository()
        {

        }

        /// <summary>
        /// 课程级别是否已使用
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-14</para>
        /// </summary>
        /// <param name="courseLevelId">级别Id</param>
        /// <returns>true:已使用 false:未使用</returns>
        public async Task<bool> CourseLevelIsUse(long courseLevelId)
        {
            return await base.IsExistAsync(x => x.CourseLevelId == courseLevelId);
        }

        /// <summary>
        /// 根据课程Id获取级别数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-14</para>
        /// </summary>
        /// <param name="courseId">一组课程Id</param>
        /// <returns>课程级别设置中间表集合</returns>
        public async Task<List<TblDatCourseLevelMiddle>> GetByCourseId(IEnumerable<long> courseId)
        {
            return await base.LoadLisTask(x => courseId.ToList().Contains(x.CourseId));
        }

        /// <summary>
        /// 根据课程Id批量删除
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-14</para>
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <returns></returns>
        public async Task BatchDeleteByCourseId(long courseId)
        {
            Expression<Func<TblDatCourseLevelMiddle, bool>> where = x => x.CourseId == courseId;
            await base.DeleteTask(where, where);
        }
    }
}
