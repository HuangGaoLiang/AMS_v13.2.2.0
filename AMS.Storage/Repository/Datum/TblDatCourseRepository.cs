/*此代码由生成工具字段生成，生成时间2018/9/14 10:04:46 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AMS.Core;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblDatCourse仓储
    /// </summary>
    public class TblDatCourseRepository : BaseRepository<TblDatCourse>
    {
        /// <summary>
        /// TblDatCourse 无参构造函数
        /// </summary>
        public TblDatCourseRepository()
        {
        }

        /// <summary>
        /// TblDatCourse 带上下文构造函数
        /// </summary>
        /// <param name="dbContext">上下文</param>
        public TblDatCourseRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 根据课程类型获取课程数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-14</para>
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="courseType">课程类型</param>
        /// <param name="courseId">课程Id courseId!=null 排除此数据</param>
        /// <param name="isDisabled">true:禁用 false:启用</param>
        /// <returns></returns>
        public async Task<List<TblDatCourse>> GetByCourseType(string companyId, int? courseType, long? courseId = null, bool? isDisabled = false)
        {
            Expression<Func<TblDatCourse, bool>> where = x => x.CompanyId == companyId;

            if (courseType.HasValue)
            {
                where = where.And(x => x.CourseType == courseType.Value);
            }

            if (courseId.HasValue)
            {
                where = where.And(x => x.CourseId != courseId.Value);
            }

            if (isDisabled.HasValue)
            {
                where = where.And(x => x.IsDisabled == isDisabled.Value);
            }

            return await base.LoadLisTask(where);
        }

        /// <summary>
        /// 获取课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-18</para>
        /// </summary>
        /// <param name="courseId">一组课程Id</param>
        /// <returns>课程列表</returns>
        public async Task<List<TblDatCourse>> GetCoursesAsync(IEnumerable<long> courseId)
        {
            return await base.LoadLisTask(x => courseId.Contains(x.CourseId));
        }

        /// <summary>
        /// 根据课程编号获取课程信息
        /// <para>作    者：Huang GaoLiang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="courseId">一组课程Id</param>
        /// <returns>课程列表</returns>
        public List<TblDatCourse> GetCourseListBycourseIds(IEnumerable<long> courseId)
        {
            return base.LoadList(m => courseId.Contains(m.CourseId));
        }

        /// <summary>
        /// 获取所有课程信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-18</para>
        /// </summary>
        /// <returns>课程列表</returns>
        public async Task<List<TblDatCourse>> GetCoursesAsync()
        {
            return await base.LoadLisTask(x => true);
        }


        /// <summary>
        /// 获取所有课程信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-18</para>
        /// </summary>
        /// <returns>课程列表</returns>
        public List<TblDatCourse> GetAll()
        {
            return base.LoadList(x => true);
        }

        /// <summary>
        /// 根据课程编号获取所有课程信息
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-05</para>
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="courseId">课程编号</param>
        /// <returns>返回课程信息</returns>
        public Task<TblDatCourse> LoadTaskByCourseId(string companyId, long courseId)
        {
            return base.LoadTask(m => m.CompanyId == companyId && m.CourseId == courseId);
        }

        /// <summary>
        /// 根据课程编号获取所有课程信息
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-05</para>
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="courseId">课程编号</param>
        /// <returns>返回课程信息</returns>
        public TblDatCourse GetCoursesByCourseId(string companyId, long courseId)
        {
            return base.Load(m => m.CompanyId == companyId && m.CourseId == courseId);
        }

        /// <summary>
        /// 获取课程信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <returns>返回课程信息</returns>
        public TblDatCourse GetByCourseId(long courseId)
        {
            return base.Load(x => x.CourseId == courseId);
        }
    }
}
