using AMS.Dto;
using AMS.Storage.Models;
using Jerrisoft.Platform.Public.PageExtensions;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描    述：订单交接业务逻辑封装类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-16</para>
    /// </summary>
    public class ViewFinOrderHandoverRepository : BaseRepository<ViewFinOrderHandover>
    {
        /// <summary>
        /// 订单交接业务逻辑封装类实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-16</para>
        /// </summary>
        public ViewFinOrderHandoverRepository()
        {
        }

        /// <summary>
        /// 订单交接业务逻辑封装类实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-16</para>
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        public ViewFinOrderHandoverRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 获取订单交接列表信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-16</para>
        /// </summary>
        /// <param name="request">订单已交接查询条件</param>
        /// <returns>交接记录分页列表</returns>
        public PageResult<ViewFinOrderHandover> GetOrderHandoverList(OrderHandoverListSearchRequest request)
        {
            var result = from a in CurrentContext.TblFinOrderHandover
                         join b in (from d in CurrentContext.TblFinOrderHandoverDetail
                                    group d by new
                                    {
                                        d.OrderHandoverId
                                    } into g
                                    select new
                                    {
                                        g.Key.OrderHandoverId,
                                        HandoverNumber = g.Count(),
                                        HandoverAmount = g.Sum(p => p.PayAmount)
                                    }) on a.OrderHandoverId equals b.OrderHandoverId
                         where a.SchoolId == request.SchoolId
                         && (string.IsNullOrEmpty(request.PersonalName) || a.PersonalName.Contains(request.PersonalName))
                         && (!request.HandoverBeginDate.HasValue || a.HandoverDate.Date >= request.HandoverBeginDate.Value.Date)
                         && (!request.HandoverEndDate.HasValue || a.HandoverDate.Date <= request.HandoverEndDate.Value.Date)
                         orderby a.HandoverDate descending
                         select new ViewFinOrderHandover
                         {
                             OrderHandoverId = a.OrderHandoverId,
                             SchoolId = a.SchoolId,
                             PersonalId = a.PersonalId,
                             Remark = a.Remark,
                             HandoverDate = a.HandoverDate,
                             CreatorId = a.CreatorId,
                             CreatorName = a.CreatorName,
                             PersonalName = a.PersonalName,
                             HandoverAmount = b.HandoverAmount,
                             HandoverNumber = b.HandoverNumber
                         };

            return result.ToPagerSource(request.PageIndex, request.PageSize);
        }
    }
}
