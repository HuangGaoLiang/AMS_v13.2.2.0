/*此代码由生成工具字段生成，生成时间14/11/2018 20:16:09 */
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AMS.Storage.Context;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描    述：收款交接仓储
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-19</para>
    /// </summary>
    public class TblFinOrderHandoverRepository : BaseRepository<TblFinOrderHandover>
    {
        /// <summary>
        /// 收款交接仓储实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-19</para>
        /// </summary>
        public TblFinOrderHandoverRepository()
        {
        }

        /// <summary>
        /// 收款交接仓储实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-19</para>
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        public TblFinOrderHandoverRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 根据订单交接Id获取收款交接信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-19</para>
        /// </summary>
        /// <param name="handoverId">订单交接Id</param>
        /// <returns>收款交接核对信息</returns>
        public TblFinOrderHandover GetByHandoverId(long handoverId)
        {
            return Load(m => m.OrderHandoverId == handoverId);
        }
    }
}
