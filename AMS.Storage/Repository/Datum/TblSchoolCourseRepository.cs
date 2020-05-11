/*此代码由生成工具字段生成，生成时间2018/9/14 10:04:46 */
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描述：TblDatSchoolCourse仓储
    /// <para>作    者：Huang GaoLiang </para>
    /// <para>创建时间：2019-02-28</para>
    /// </summary>
    public class TblSchoolCourseRepository : BaseRepository<TblDatSchoolCourse>
    {
        /// <summary>
        /// TblDatSchoolCourse 无参构造函数
        /// </summary>
        public TblSchoolCourseRepository()
        {
        }

        /// <summary>
        /// TblDatSchoolCourse 带上下文构造函数
        /// </summary>
        /// <param name="dbContext">上下文</param>
        public TblSchoolCourseRepository(DbContext dbContext) : base(dbContext)
        {
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
        /// 获取所有的校区课程授权列表
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-18 </para>
        /// </summary>
        /// <returns>返回校区课程授权列表</returns>
        public async Task<List<TblDatSchoolCourse>> GetAllSchoolCourseList()
        {
            return await base.LoadLisTask(m => true);
        }

        /// <summary>
        /// 根据校区编号，获取校区已经授权的课程编号
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-18 </para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <returns>返回校区授权课程编号集合</returns>
        public async Task<List<TblDatSchoolCourse>> GetSchoolCourseListBySchoolId(string schoolId)
        {
            return await base.LoadLisTask(m => m.SchoolId == schoolId);
        }

        /// <summary>
        /// 获取已授权的校区编号
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-18 </para>
        /// </summary>
        /// <returns>获取校区授权课程编号集合</returns>
        public List<string> GetSchools()
        {
            return base.LoadQueryable(m => true, false)
                        .Select(t => t.SchoolId)
                        .Distinct()
                        .ToList();
        }

        /// <summary>
        /// 根据校区编号和课程编号查询有没有授权
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-18 </para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <param name="courseIds">课程编号</param>
        /// <returns>获取没有授权的数据集合</returns>
        public async Task<List<TblDatSchoolCourse>> GetSchoolCourseByWhere(string schoolId, List<long> courseIds)
        {
            return await base.LoadLisTask(m => m.SchoolId == schoolId && courseIds.Contains(m.CourseId));
        }

        /// <summary>
        /// 批量添加
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-18 </para>
        /// </summary>
        /// <param name="schoolCourseList">校区课程集合</param>
        /// <returns>返回受影响的行数</returns>
        public async Task<int> AddTask(List<TblDatSchoolCourse> schoolCourseList)
        {
            return await base.AddTask(schoolCourseList);
        }

        /// <summary>
        /// 根据校区编号批量删除
        ///  <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-19 </para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        public async Task BatchDeleteBySchoolId(string schoolId)
        {
            Expression<Func<TblDatSchoolCourse, bool>> where = x => x.SchoolId == schoolId;
            await base.DeleteTask(where, where);
        }
    }
}
