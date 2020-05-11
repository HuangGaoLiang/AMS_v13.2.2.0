/*此代码由生成工具字段生成，生成时间2018/10/27 17:55:53 */
using AMS.Storage.Repository;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using YMM.Storage.Models;

namespace YMM.Test.Storage.Repository
{
    /// <summary>
    /// 描述：TblAutCouponRule仓储
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-9-19</para>
    /// </summary>
    public class TblAutCouponRuleRepository : BaseRepository<TblAutCouponRule>
    {
        /// <summary>
        /// 描述：实例化一个TblAutCouponRule仓储对象
        /// <para>作    者:瞿琦</para>
        /// <para>创建时间：2019-9-19</para>
        /// </summary>
        /// <param name="context">数据上下文</param>
        public TblAutCouponRuleRepository(DbContext context) : base(context)
        {

        }
        /// <summary>
        /// 描述：实例化一个TblAutCouponRule仓储对象
        /// <para>作    者:瞿琦</para>
        /// <para>创建时间：2019-9-19</para>
        /// </summary>
        public TblAutCouponRuleRepository()
        {

        }

        /// <summary>
        /// 描述：根据审核Id获取审核中的信息
        /// <para>作    者:瞿琦</para>
        /// <para>创建时间：2019-9-19</para>
        /// </summary>
        /// <param name="auditId">审核Id</param>
        /// <returns>赠与奖学金信息集合</returns>
        public List<TblAutCouponRule> GetAuditingCouponRule(long auditId)
        {
           var result=  base.LoadList(x=>x.AuditId==auditId);
            return result;
        }
    }
}
