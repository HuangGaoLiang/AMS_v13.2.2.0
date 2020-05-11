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
    /// 描    述: TblOdrEnrollOrder仓储
    /// <para>作    者: Huang GaoLiang</para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class TblOdrEnrollOrderRepository : BaseRepository<TblOdrEnrollOrder>
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public TblOdrEnrollOrderRepository()
        {
        }

        /// <summary>
        /// 带上下文的构造函数
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public TblOdrEnrollOrderRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 获取校区一个学生的订单集合
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        /// <returns>报名订单列表</returns>
        public async Task<List<TblOdrEnrollOrder>> GetStudentOrders(string schoolId, long studentId)
        {
            return await LoadLisTask(x => x.SchoolId == schoolId && x.StudentId == studentId);
        }

        /// <summary>
        /// 描述：获取校区一个学生指定状态的订单集合
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-2</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        /// <param name="orderStatusList">订单状态集合</param>
        /// <returns>报名订单列表</returns>
        public async Task<List<TblOdrEnrollOrder>> GetStudentOrderList(string schoolId, long studentId, List<int> orderStatusList)
        {
            return await LoadLisTask(x => x.SchoolId == schoolId && x.StudentId == studentId && orderStatusList.Contains(x.OrderStatus));
        }

        /// <summary>
        /// 根据校区Id和招生员Id获取报名信息
        /// <para>作       者：郭伟佳</para>
        /// <para>创建时间：2018-11-14</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="creatorId">招生员Id</param>
        /// <returns>报名订单列表</returns>
        public async Task<List<TblOdrEnrollOrder>> GetEnrollOrderByCreatorId(string schoolId, string creatorId)
        {
            return await LoadLisTask(x => x.SchoolId == schoolId && x.CreateId == creatorId);
        }

        /// <summary>
        /// 根据学生编号获取报名信息
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-11-26</para>
        /// </summary>
        /// <param name="studentId">学生编号</param>
        /// <param name="orderStatus">订单状态</param>
        /// <returns>返回订单集合</returns>
        public async Task<List<TblOdrEnrollOrder>> GetEnrollOrderByStudentId(long studentId, List<int> orderStatus)
        {
            return await LoadLisTask(x => x.StudentId == studentId && orderStatus.Contains(x.OrderStatus));
        }

        /// <summary>
        /// 根据订单编号获取订单信息
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-11-26</para>
        /// </summary>
        /// <param name="enrollOrderId">订单编号</param>
        /// <returns>返回订单详细信息</returns>
        public TblOdrEnrollOrder GetOrderById(long enrollOrderId)
        {
            return Load(m => m.EnrollOrderId == enrollOrderId);
        }

        /// <summary>
        /// 更新报班订单状态
        /// <param>作       者：郭伟佳</param>
        /// </summary>
        /// <param name="orderIdList">订单Id列表</param>
        /// <param name="status">状态</param>
        /// <returns>无</returns>
        public async Task UpdateOrderStatus(List<long> orderIdList, OrderStatus status)
        {
            await UpdateTask(x => orderIdList.Contains(x.EnrollOrderId), w => new TblOdrEnrollOrder() { OrderStatus = (int)status });
        }

        /// <summary>
        /// 根据学生编号获取订单信息
        ///  <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-11-16</para>
        /// </summary>
        /// <param name="studentId">学生编号</param>
        /// <returns></returns>
        public TblOdrEnrollOrder GetOrderByStudentId(long studentId)
        {
            return LoadList(m => m.StudentId == studentId).OrderByDescending(m => m.CreateTime).FirstOrDefault();
        }
    }
}
