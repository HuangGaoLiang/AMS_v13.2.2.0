/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using System.Collections.Generic;
using AMS.Dto;
using AMS.Storage.Models;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblTimLessonProcess仓储
    /// </summary>
    public class TblTimLessonProcessRepository : BaseRepository<TblTimLessonProcess>
    {
        /// <summary>
        /// 带数据库上下文参数的构造函数
        /// </summary>
        /// <param name="context">数据库上下文</param>
        public TblTimLessonProcessRepository(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// 无参数的构造函数
        /// </summary>
        public TblTimLessonProcessRepository()
        {
        }

        /// <summary>
        /// 根据课程Id集合获取课次变更记录列表信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-04</para>
        /// </summary>
        /// <param name="lessonIdList">课程Id集合</param>
        /// <returns>课次变更记录列表信息</returns>
        public List<TblTimLessonProcess> GetListByLessonId(List<long> lessonIdList)
        {
            return base.LoadList(a => lessonIdList.Contains(a.LessonId));
        }

        /// <summary>
        /// 根据业务Id获取变更记录
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-14</para>
        /// </summary>
        /// <param name="businessId">业务Id</param>
        /// <returns>课次变更记录表列表</returns>
        public List<TblTimLessonProcess> GetByBusinessId(long businessId)
        {
            return base.LoadList(x => x.BusinessId == businessId);
        }
    }
}
