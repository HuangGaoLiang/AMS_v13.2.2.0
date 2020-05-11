/*此代码由生成工具字段生成，生成时间14/11/2018 20:16:09 */
using AMS.Dto;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描    述：收款交接明细仓储
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-15</para>
    /// </summary>
    public class TblFinOrderHandoverDetailRepository : BaseRepository<TblFinOrderHandoverDetail>
    {
        /// <summary>
        /// 收款交接明细仓储实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-15</para>
        /// </summary>
        public TblFinOrderHandoverDetailRepository()
        {
        }

        /// <summary>
        /// 带有数据库上下文的收款交接明细仓储实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-15</para>
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        public TblFinOrderHandoverDetailRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 根据校区Id和招生专员Id获取订单交接核对明细信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-15</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="personalId">招生员Id</param>
        /// <returns>订单交接核对明细列表</returns>
        public async Task<List<TblFinOrderHandoverDetail>> GetDetailsByPersonalId(string schoolId, string personalId)
        {
            return await LoadLisTask(x => x.SchoolId == schoolId && x.PersonalId == personalId);
        }

        /// <summary>
        /// 根据订单编号获取收款交接详情
        /// 作     者:Huang GaoLiang 2018年11月16日
        /// </summary>
        /// <param name="orderIdList">订单编号列表</param>
        /// <returns>收款交接明细信息列表</returns>
        public List<TblFinOrderHandoverDetail> GetDetailsByOrderId(List<long> orderIdList)
        {
            return LoadList(m => orderIdList.Contains(m.OrderId));
        }

        /// <summary>
        /// 根据订单编号获取收款交接详情
        /// 作     者:Huang GaoLiang 2018年11月23日
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="orderType">订单类型</param>
        /// <returns>收款交接明细信息</returns>
        public TblFinOrderHandoverDetail GetDetailById(long orderId, OrderTradeType orderType)
        {
            return Load(m => m.OrderId == orderId && m.OrderTradeType == (int)orderType);
        }

        /// <summary>
        /// 根据订单交接Id获取收款交接列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-17</para>
        /// </summary>
        /// <param name="handoverId">订单交接Id</param>
        /// <returns>订单交接核对明细列表</returns>
        public async Task<List<TblFinOrderHandoverDetail>> GetDetailsByHandoverId(long handoverId)
        {
            return await LoadLisTask(m => m.OrderHandoverId == handoverId);
        }

        /// <summary>
        /// 更新收款交接明细的状态为已交接状态
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-21</para>
        /// </summary>
        /// <param name="orderHandover">收款交接单信息</param>
        public async Task UpdateDetailsByHandoverId(TblFinOrderHandover orderHandover)
        {
            await UpdateTask(s => s.PersonalId == orderHandover.PersonalId && s.SchoolId == orderHandover.SchoolId && s.HandoverStatus == (int)HandoverStatus.Checked,
                        w => new TblFinOrderHandoverDetail() { OrderHandoverId = orderHandover.OrderHandoverId, HandoverDate = orderHandover.HandoverDate, HandoverStatus = (int)HandoverStatus.Handover });
        }

        /// <summary>
        /// 删除对应的收款交接明细
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-12-20</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="orderId">收款交接明细对象</param>
        /// <param name="tradeType">收款交接明细对象</param>
        public void DeleteDetailByOrderId(string schoolId, long orderId, OrderTradeType tradeType)
        {
            this.Delete(a => a.SchoolId == schoolId && a.OrderId == orderId && a.OrderTradeType == (int)tradeType);
        }
    }
}
