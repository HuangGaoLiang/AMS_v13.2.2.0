/*此代码由生成工具字段生成，生成时间2018/10/27 16:52:22 */

using AMS.Dto;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描    述: TblOdrDepositOrder仓储
    /// <para>作    者: Huang GaoLiang</para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class TblOdrDepositOrderRepository : BaseRepository<TblOdrDepositOrder>
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public TblOdrDepositOrderRepository()
        {
        }

        /// <summary>
        /// 带上下文的构造函数
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public TblOdrDepositOrderRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 根据订金编号查询订金信息
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-11-26</para>
        /// </summary>
        /// <param name="depositOrderId">订金编号</param>
        /// <returns>返回订金详情数据</returns>
        public async Task<TblOdrDepositOrder> GetOdrDepositOrderById(long depositOrderId)
        {
            return await base.LoadTask(depositOrderId);
        }

        /// <summary>
        /// 根据收银人获取定金信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-15</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="payee">招生员Id</param>
        /// <returns>定金订单列表</returns>
        public async Task<List<TblOdrDepositOrder>> GetDepositOrderByPayee(string schoolId, string payee)
        {
            return await LoadLisTask(x => x.SchoolId == schoolId && x.PayeeId == payee);
        }

        /// <summary>
        /// 更新定金订单状态
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-26</para>
        /// </summary>
        /// <param name="orderIdList">订单Id列表</param>
        /// <param name="status">状态</param>
        /// <returns>无</returns>
        public async Task UpdateOrderStatus(List<long> orderIdList, OrderStatus status)
        {
            await UpdateTask(x => orderIdList.Contains(x.DepositOrderId), w => new TblOdrDepositOrder() { OrderStatus = (int)status });
        }

        /// <summary>
        /// 根据订金编号查询订金信息
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-11-26</para>
        /// </summary>
        /// <param name="depositOrderId">订金编号</param>
        /// <returns>返回订金详情数据</returns>
        public TblOdrDepositOrder GetOdrDepositOrder(long depositOrderId)
        {
            return base.Load(depositOrderId);
        }

    }
}
