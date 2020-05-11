/*此代码由生成工具字段生成，生成时间2018/9/10 16:44:45 */
using AMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblAutClassTime仓储
    /// </summary>
    public class TblAutClassTimeRepository : BaseRepository<TblAutClassTime>
    {
        public TblAutClassTimeRepository(DbContext context) : base(context)
        {

        }

        public TblAutClassTimeRepository()
        {

        }

        /// <summary>
        /// 根据上课时间表Id获取班级上课时间
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-21</para>
        /// </summary>
        /// <param name="schoolTimeId">上课时间表Id</param>
        /// <param name="auditId">审核Id</param>
        /// <returns>班级上课时间表</returns>
        public async Task<List<TblAutClassTime>> GetBySchoolTimeId(IEnumerable<long> schoolTimeId, long auditId)
        {
            return await base.LoadLisTask(x => schoolTimeId.Contains(x.SchoolTimeId) && x.AuditId == auditId);
        }

        /// <summary>
        /// 根据审核Id获取数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-25</para>
        /// </summary>
        /// <param name="auditId">审核Id</param>
        /// <returns>班级上课时间表</returns>
        public async Task<List<TblAutClassTime>> GetByAuditIdAsync(long auditId)
        {
            return await base.LoadLisTask(x => x.AuditId == auditId);
        }

        /// <summary>
        /// 删除审核中的一条班级课表数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-25</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <param name="auditId">审核Id</param>
        public void DeleteByClassId(long auditId, long classId)
        {
            base.Delete(x => x.ClassId == classId && x.AuditId == auditId);
        }

        /// <summary>
        /// 获取当前审核当前班级上课时间段
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-26</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <param name="auditId">审核Id</param>
        /// <returns>班级上课时间表</returns>
        public async Task<List<TblAutClassTime>> GetByClassId(long auditId, long classId)
        {
            return await base.LoadLisTask(x => x.AuditId == auditId && x.ClassId == classId);
        }

        /// <summary>
        /// 获取当前审核当前班级上课时间段
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-26</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <param name="auditId">审核Id</param>
        /// <returns>班级上课时间表</returns>
        public async Task<List<TblAutClassTime>> GetByClassId(long auditId, IEnumerable<long> classId)
        {
            return await base.LoadLisTask(x => x.AuditId == auditId && classId.Contains(x.ClassId));
        }
    }
}
