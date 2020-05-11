/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using AMS.Dto;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描    述: TblOdrEnrollOrderItem仓储
    /// <para>作    者: Huang GaoLiang</para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class TblOdrEnrollOrderItemRepository : BaseRepository<TblOdrEnrollOrderItem>
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public TblOdrEnrollOrderItemRepository()
        {
        }

        /// <summary>
        /// 带上下文的构造函数
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public TblOdrEnrollOrderItemRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 根据报名订单id获取
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="enrollOrderId">报名订单Id集合</param>
        /// <returns>报名课程明细列表</returns>
        public async Task<List<TblOdrEnrollOrderItem>> GetByEnrollOrderId(IEnumerable<long> enrollOrderId)
        {
            return await LoadLisTask(x => enrollOrderId.Contains(x.EnrollOrderId));
        }

        /// <summary>
        /// 根据报名订单id和校区编号获取订单明细数据
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-12-08</para>
        /// </summary>
        /// <param name="enrollOrderId">报名订单Id集合</param>
        /// <param name="schoolId">校区编号</param>
        /// <returns>返回订单明细数据</returns>
        public async Task<List<TblOdrEnrollOrderItem>> GetByOrderId(IEnumerable<long> enrollOrderId, string schoolId)
        {
            return await LoadLisTask(x => enrollOrderId.Contains(x.EnrollOrderId) && x.SchoolId == schoolId);
        }

        /// <summary>
        /// 根据报名订单id获取
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="enrollOrderId">报名订单Id</param>
        /// <returns>报名课程明细列表</returns>
        public async Task<List<TblOdrEnrollOrderItem>> GetByEnrollOrderId(long enrollOrderId)
        {
            return await LoadLisTask(x => x.EnrollOrderId == enrollOrderId);
        }

        /// <summary>
        /// 根据报名订单课程明细主键查询
        /// ---瞿琦  2018-11-12
        /// </summary>
        /// <returns></returns>
        public async Task<List<TblOdrEnrollOrderItem>> GetByEnrollOrderItemId(IEnumerable<long> enrollOrderItemId)
        {
            return await LoadLisTask(x => enrollOrderItemId.Contains(x.EnrollOrderItemId));
        }

        /// <summary>
        /// 修改报名订单课程明细表状态
        /// ----瞿琦 2018-11-26
        /// </summary>
        /// <param name="enrollOrderItemId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task UpdateEnrollOrderItemStatus(IEnumerable<long> enrollOrderItemId, OrderItemStatus status)
        {
            await base.UpdateTask(x => enrollOrderItemId.Contains(x.EnrollOrderItemId), k => new TblOdrEnrollOrderItem { Status = (int)status });
        }
    }
}
