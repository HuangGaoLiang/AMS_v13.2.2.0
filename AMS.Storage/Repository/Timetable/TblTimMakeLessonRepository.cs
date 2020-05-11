/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblTimMakeLesson仓储
    /// </summary>
    public class TblTimMakeLessonRepository : BaseRepository<TblTimMakeLesson>
    {

        public TblTimMakeLessonRepository(DbContext context) : base(context)
        {

        }
        public TblTimMakeLessonRepository()
        {

        }

        /// <summary>
        /// 根据报名订单明细Id获取排了哪些课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="enrollOrderItemId">订单明细Id集合</param>
        /// <returns>排课列表</returns>
        public async Task<List<TblTimMakeLesson>> GetByEnrollOrderItemId(IEnumerable<long> enrollOrderItemId)
        {
            return await LoadLisTask(x => enrollOrderItemId.Contains(x.EnrollOrderItemId));
        }

        /// <summary>
        /// 根据主键编号获取排课记录信息
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-08</para>
        /// </summary>
        /// <param name="makeLessonId">主键编号</param>
        /// <returns>排课信息</returns>
        public TblTimMakeLesson GetTimMakeLesson(long makeLessonId)
        {
            return Load(x => x.MakeLessonId == makeLessonId);
        }

        /// <summary>
        /// 根据报名订单明细Id获取排了哪些课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="enrollOrderItemId">订单明细Id集合</param>
        /// <returns>排课列表</returns>
        public async Task<List<TblTimMakeLesson>> GetByEnrollOrderItemId(long enrollOrderItemId)
        {
            return await LoadLisTask(x => x.EnrollOrderItemId == enrollOrderItemId);
        }

        /// <summary>
        /// 获取未跟家长确认的报名排课
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-05</para> 
        /// </summary>
        /// <param name="orderItemId">报名订单课程Id</param>
        /// <returns>排课列表</returns>
        public List<TblTimMakeLesson> GetUnconfirmedMakeLessonList(long orderItemId)
        {
            return base.LoadList(x => x.EnrollOrderItemId == orderItemId && !x.IsConfirm);
        }

        /// <summary>
        /// 清空未确认排课
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-16</para> 
        /// </summary>
        /// <param name="enrollOrderItemId">报名课程订单Id</param>
        /// <returns></returns>
        public async Task ClearUnconfirmedClass(long enrollOrderItemId)
        {
            Expression<Func<TblTimMakeLesson, bool>> whereLambda = x => !x.IsConfirm && x.EnrollOrderItemId == enrollOrderItemId;
            await base.DeleteTask(whereLambda, whereLambda);
        }
    }
}
