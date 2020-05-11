/*此代码由生成工具字段生成，生成时间2018/9/10 16:44:45 */
using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblAutClass仓储
    /// </summary>
    public class TblAutClassRepository : BaseRepository<TblAutClass>
    {
        public TblAutClassRepository(DbContext context) : base(context)
        {

        }

        public TblAutClassRepository()
        {

        }

        /// <summary>
        /// 根据审核Id获取所有班级课表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-21</para>
        /// </summary>
        /// <param name="auditId">审核Id</param>
        /// <returns>审核中班级课表集合</returns>
        public async Task<List<TblAutClass>> GetByAuditId(long auditId)
        {
            return await base.LoadLisTask(x => x.AuditId == auditId);
        }

        /// <summary>
        /// 获取审核中班级课表数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-25</para>
        /// </summary>
        /// <param name="auditId">审核Id</param>
        /// <param name="classId">班级Id</param>
        /// <returns>审核中班级课表</returns>
        public async Task<TblAutClass> GetAsync(long auditId, long classId)
        {
            return await base.LoadTask(x => x.AuditId == auditId && x.ClassId == classId);
        }

        /// <summary>
        /// 删除审核中的一条班级数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-25</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <param name="auditId">审核Id</param>
        public void DeleteByClassId(long auditId, long classId)
        {
            base.Delete(x => x.AuditId == auditId && x.ClassId == classId);
        }

        /// <summary>
        /// 根据学期Id查找班级信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="auditId">审核Id</param>
        /// <param name="termIds">学期ID</param>
        /// <returns>审核中班级课表</returns>
        public List<TblAutClass> GetTermIdByClass(long auditId, List<long> termIds)
        {
            return base.LoadList(x => termIds.Contains(x.TermId));
        }

        /// <summary>
        /// 教室课程是否已使用
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="auditId">一组审核Id</param>
        /// <param name="roomCourseId">教室课程ID</param>
        /// <returns>true:已使用 false:未使用</returns>
        public bool RoomCourseIdIsUse(List<long> auditId, long roomCourseId)
        {
            return IsExist(x => auditId.Contains(x.AuditId) && x.RoomCourseId == roomCourseId);
        }
    }
}
