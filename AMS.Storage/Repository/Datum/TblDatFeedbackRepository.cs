/*此代码由生成工具字段生成，生成时间2018/11/5 16:44:29 */
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AMS.Storage.Repository;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;
using Jerrisoft.Platform.Public.PageExtensions;
using AMS.Dto;
using Jerrisoft.Platform.Storage;

namespace AMS.Storage
{
    /// <summary>
    /// TblDatFeedbackRepository仓储
    /// </summary>
    public class TblDatFeedbackRepository : BaseRepository<TblDatFeedback>
    {
        /// <summary>
        /// TblDatFeedbackRepository无参构造函数
        /// </summary>
        public TblDatFeedbackRepository()
        {
        }

        /// <summary>
        /// TblDatFeedbackRepository带上下文参数的构造函数
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public TblDatFeedbackRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 根据条件查询投诉与建议分页列表数据
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-07 </para>
        /// </summary>
        /// <param name="search">投诉与建议查询条件</param>
        /// <returns>返回信息投诉与建议信息列表</returns>
        public PageResult<TblDatFeedback> GetFeedBackList(FeedbackSearchRequest search)
        {
            var list = base.LoadQueryable()
               .Where(m => m.CompanyId == search.CompanyId)
               .WhereIf(search.FeedbackProcessStatus != null, m => m.ProcessStatus == (int)search.FeedbackProcessStatus)
               .WhereIf(search.StartTime != null, m => m.CreateTime >= search.StartTime)
               .WhereIf(search.EndtTime != null, m => m.CreateTime.AddDays(-1) < search.EndtTime)
               .WhereIf(!string.IsNullOrWhiteSpace(search.SchoolId), m => m.SchoolId == search.SchoolId)
               .WhereIf(!string.IsNullOrWhiteSpace(search.CreatorName), m => m.CreatorName == search.CreatorName)
               .OrderBy(m => m.ProcessStatus)
               .ThenByDescending(m => m.CreateTime)
               .ToPagerSource(search.PageIndex, search.PageSize);

            return list;
        }

        /// <summary>
        /// 根据主键编号查询投诉与建议详情
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-07 </para>
        /// </summary>
        /// <param name="feedback">处理意见信息</param>
        /// <returns>返回受影响的行数</returns>
        public async Task<int> UpdateFeedbackById(TblDatFeedback feedback)
        {
            return await base.UpdateTask(feedback);
        }

    }
}
