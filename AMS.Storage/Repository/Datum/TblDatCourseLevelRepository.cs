/*此代码由生成工具字段生成，生成时间2018/9/14 10:04:46 */
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描    述: TblDatCourseLevel仓储
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18 </para>
    /// </summary>
    public class TblDatCourseLevelRepository : BaseRepository<TblDatCourseLevel>
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public TblDatCourseLevelRepository()
        {
        }
        /// <summary>
        /// 带上下文的构造函数
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public TblDatCourseLevelRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 获取课程级别设置列表
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-09-04  </para>
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns>返回课程级别列表</returns>
        public async Task<List<TblDatCourseLevel>> Get(string companyId)
        {
            return await this.LoadLisTask(m => m.CompanyId == companyId);
        }


        /// <summary>
        /// 根据课程等级id查询课程等级信息
        /// <para>作     者:Huang GaoLiang   </para>
        /// <para>创建时间：2018-09-04  </para>
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="courseLeaveId">课程级别id</param>
        /// <returns>返回课程级别</returns>
        public TblDatCourseLevel GetCourseLevelById(string companyId, long courseLeaveId)
        {
            return Load(m => m.CourseLevelId == courseLeaveId);
        }

        /// <summary>
        /// 异步删除
        /// <para>作     者:Huang GaoLiang   </para>
        /// <para>创建时间：2018-09-04  </para>
        /// </summary>
        /// <param name="courseLeave">课程等级</param>
        /// <returns>返回删除行数</returns>
        public async Task<int> DeleteAsync(TblDatCourseLevel courseLeave)
        {
            return await DeleteTask(courseLeave);
        }

        /// <summary>
        /// 根据课程等级Id获取课程级别
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-20</para>
        /// </summary>
        /// <param name="courseLevelId">课程等级Id</param>
        /// <returns>课程级别信息</returns>
        public List<TblDatCourseLevel> GetById(IEnumerable<long> courseLevelId)
        {
            return base.LoadList(x => courseLevelId.Contains(x.CourseLevelId));
        }
    }
}
