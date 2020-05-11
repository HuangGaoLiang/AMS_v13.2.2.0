/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using System.Linq;
using AMS.Dto;
using AMS.Storage.Models;
using Jerrisoft.Platform.Public.PageExtensions;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblCashWalletTrade仓储
    /// </summary>
    public class TblCashWalletTradeRepository : BaseRepository<TblCashWalletTrade>
    {
        /// <summary>
        /// 余额交易实例化
        /// </summary>
        /// <param name="context"></param>
        public TblCashWalletTradeRepository(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// 余额交易实例化
        /// </summary>
        public TblCashWalletTradeRepository()
        {
        }

        /// <summary>
        /// 分页获取余额交易列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-09</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        /// <param name="request">余额交易请求对象</param>
        /// <returns>余额交易列表</returns>
        public PageResult<TblCashWalletTrade> GetWalletTradeList(string schoolId, long studentId, WalletTradeListRequest request)
        {
            var result = base.LoadQueryable()
                        .Where(x => x.SchoolId == schoolId && x.StudentId == studentId)
                        .OrderByDescending(x => x.TransDate);

            return result.ToPagerSource(request.PageIndex, request.PageSize);
        }
    }
}
