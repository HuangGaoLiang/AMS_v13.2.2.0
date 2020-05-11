/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AMS.Storage.Context;
using AMS.Storage.Models;
using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描    述：学习计划课程学期仓储
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public class TblOdrStudyPlanTermRepository : BaseRepository<TblOdrStudyPlanTerm>
    {
        /// <summary>
        /// 学习计划学期仓储实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        public TblOdrStudyPlanTermRepository()
        {
        }

        /// <summary>
        /// 学习计划学期仓储实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        public TblOdrStudyPlanTermRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 根据校区Id获取校区课程信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-05</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <returns>报名学习计划学期列表</returns>
        public async Task<List<TblOdrStudyPlanTerm>> GetList(string schoolId)
        {
            return await LoadLisTask(a => a.SchoolId == schoolId);
        }

        /// <summary>
        /// 添加校区学期类型的课次信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="studyPlanTermList">报名学习计划学期列表</param>
        public async Task AddStudyPlanTermsAsync(List<TblOdrStudyPlanTerm> studyPlanTermList)
        {
            await this.SaveTask<TblOdrStudyPlanTerm>(studyPlanTermList);
        }

        /// <summary>
        /// 批量删除学期相关课次信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="schoolIds">校区Id集合</param>
        public async Task DeleteStudyPlanTermsAsync(List<string> schoolIds)
        {
            Expression<Func<TblOdrStudyPlanTerm, bool>> where = x => schoolIds.Contains(x.SchoolId);
            await base.DeleteTask(where, where);
        }
    }
}
