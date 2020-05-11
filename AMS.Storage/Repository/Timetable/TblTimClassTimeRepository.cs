/*此代码由生成工具字段生成，生成时间2018/9/7 12:42:58 */
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AMS.Storage.Context;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblCousClass仓储
    /// </summary>
    public class TblTimClassTimeRepository : BaseRepository<TblTimClassTime>
    {

        public TblTimClassTimeRepository(DbContext context) : base(context)
        {

        }
        public TblTimClassTimeRepository()
        {

        }


        /// <summary>
        /// 根据上课时间表Id获取班级上课时间
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-19</para>
        /// </summary>
        /// <param name="schoolTimeId">上课时间表Id</param>
        /// <returns>班级上课信息列表</returns>
        public async Task<List<TblTimClassTime>> GetBySchoolTimeId(IEnumerable<long> schoolTimeId)
        {
            return await base.LoadLisTask(x => schoolTimeId.Contains(x.SchoolTimeId));
        }

        /// <summary>
        /// 根据班级获取上课时间段
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-21</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <returns>班级上课信息列表</returns>
        public List<TblTimClassTime> GetByClassId(long classId)
        {
            return base.LoadList(x => x.ClassId == classId);
        }

        /// <summary>
        /// 根据班级获取上课时间段
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-25</para>
        /// </summary>
        /// <param name="classId">一组班级Id</param>
        /// <returns>班级上课信息列表</returns>
        public async Task<List<TblTimClassTime>> GetByClassId(IEnumerable<long> classId)
        {
            return await base.LoadLisTask(x => classId.Contains(x.ClassId));
        }

        /// <summary>
        /// 根据班级ID删除课程时间
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="classIds">班级Id</param>
        public void DeleteByClassId(List<long> classIds)
        {
            base.Delete(t => classIds.Contains(t.ClassId));
        }
    }
}
