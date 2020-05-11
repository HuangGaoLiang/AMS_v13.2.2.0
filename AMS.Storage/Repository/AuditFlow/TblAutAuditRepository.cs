/*此代码由生成工具字段生成，生成时间2018/9/10 16:44:45 */
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AMS.Storage.Context;
using AMS.Storage.Models;
using Jerrisoft.Platform.Storage;
using AMS.Dto;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描述：TblAutAudit仓储
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-9-25 </para>
    /// </summary>
    public class TblAutAuditRepository : BaseRepository<TblAutAudit>
    {
        /// <summary>
        /// 描述：数据上下文对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-9-25 </para>
        /// </summary>
        public AMSContext CurrentContext
        {
            get { return (AMSContext)base.Context; }
        }

        /// <summary>
        /// 描述：实例化一个TblAutAudit表的仓储对象
        ///  <para>作    者：瞿琦</para>
        ///  <para>创建时间：2018-9-25 </para>
        /// </summary>
        /// <param name="context">数据上下文对象</param>
        public TblAutAuditRepository(DbContext context) : base(context)
        {

        }
        /// <summary>
        ///  描述：实例化一个TblAutAudit表的仓储对象
        ///  <para>作    者：瞿琦</para>
        ///  <para>创建时间：2018-9-25 </para>
        /// </summary>
        public TblAutAuditRepository()
        {
        }

        /// <summary>
        /// 描述：根据业务扩展字段获取审核信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.19</para>
        /// </summary>
        /// <param name="businessType">业务类型</param>
        /// <param name="extField1">扩展字段1</param>
        /// <param name="extField2">扩展字段2</param>
        /// <returns>审核表对象</returns>
        public TblAutAudit GetTblAutAuditByExtField(AuditBusinessType businessType, string extField1, string extField2 = null)
        {
            var queryWhere = base.LoadQueryable(x => x.BizType == (int)businessType, false)
                            .Where(x => x.ExtField1.Trim() == extField1.Trim())
                            .WhereIf(!string.IsNullOrWhiteSpace(extField2), x => x.ExtField2 == extField2);
            var ss = queryWhere.ToList();
            return queryWhere.OrderByDescending(x => x.CreateTime).FirstOrDefault();
        }
        /// <summary>
        /// 描述：获取审核主表信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.19</para>
        /// </summary>
        /// <param name="flowNo">流程编号</param>
        /// <returns>审核信息集合</returns>
        public async Task<List<TblAutAudit>> GetAutAuditByFlowNo(string flowNo)
        {
            return await base.LoadLisTask(m => m.FlowNo == flowNo);
        }

        /// <summary>
        /// 描述：跟据扩展字段获取批量审核信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.19</para>
        /// </summary>
        /// <param name="extList">扩展字段集合</param>
        /// <returns>审核信息集合</returns>
        public List<TblAutAudit> GetAuditList(List<string> extList)
        {
            var result = base.LoadList(x => extList.Contains(x.ExtField1));
            return result;
        }

        /// <summary>
        /// 获取学期待审核，审核中 的审核Id
        /// </summary>
        /// <returns></returns>
        public List<long> GetByWhere()
        {
            int bizType = (int)AuditBusinessType.TermCourseTimetable;
            int waitAudit = (int)AuditStatus.WaitAudit;
            int auditing = (int)AuditStatus.Auditing;

            return base.LoadList(x => x.BizType == bizType && (x.AuditStatus == waitAudit || x.AuditStatus == auditing)).Select(x => x.AuditId).ToList();
        }
    }
}
