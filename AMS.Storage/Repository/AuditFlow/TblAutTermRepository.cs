/*此代码由生成工具字段生成，生成时间2018/9/10 16:44:45 */
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AMS.Storage.Context;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描述：TblAutTerm仓储
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018.9.19</para>
    /// </summary>
    public class TblAutTermRepository : BaseRepository<TblAutTerm>
    {
        /// <summary>
        /// 描述：实例化一个TblAutTerm仓储对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.19</para>
        /// </summary>
        /// <param name="context">数据上下文</param>
        public TblAutTermRepository(DbContext context) : base(context)
        {

        }
        /// <summary>
        /// 描述：实例化一个TblAutTerm仓储对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.19</para>
        /// </summary>
        public TblAutTermRepository()
        {

        }

        /// <summary>
        /// 描述：根据审核Id获取审核中的学期列表
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.19</para>
        /// </summary>
        /// <returns>审核中的学期信息集合</returns>
        public List<TblAutTerm> GetTblAutTermList(long auditId)
        {
            return base.LoadList(x => x.AuditId == auditId);
        }


        /// <summary>
        /// 描述：根据审核ID和学期ID删除审核中的学期
        ///  <para>作    者：瞿琦</para>
        ///  <para>创建时间：2018.9.19</para>
        /// </summary>
        /// <param name="auditId">审核Id</param>
        /// <param name="termId">学期Id</param>
        /// <returns>无</returns>
        public void DeleteByAutTermId(long auditId, long termId)
        {
            base.Delete(x => x.AuditId == auditId && x.TermId == termId);
        }

        /// <summary>
        /// 描述：根据校区和年度删除审核中的学期
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.19</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="year">年度</param>
        public void DeleteByAutTermId(string schoolId, int year)
        {
            base.Delete(x => x.SchoolId.Trim() == schoolId && x.Year == year);
        }
    }
}
