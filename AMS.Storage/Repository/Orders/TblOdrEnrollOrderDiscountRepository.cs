/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描    述: TblOdrEnrollOrderDiscount仓储
    /// <para>作    者: Huang GaoLiang</para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class TblOdrEnrollOrderDiscountRepository : BaseRepository<TblOdrEnrollOrderDiscount>
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public TblOdrEnrollOrderDiscountRepository()
        {
        }

        /// <summary>
        /// 带上下文的构造函数
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public TblOdrEnrollOrderDiscountRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
