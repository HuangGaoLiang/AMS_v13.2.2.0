/*此代码由生成工具字段生成，生成时间2018/9/7 12:42:58 */
using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 教室仓储
    /// </summary>
    public class TblDatClassRoomRepository : BaseRepository<TblDatClassRoom>
    {
        public TblDatClassRoomRepository(DbContext context) : base(context)
        {

        }

        public TblDatClassRoomRepository()
        {

        }

        /// <summary>
        /// 根据条件获取教室
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-09</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <param name="roomNo">门牌号</param>
        /// <returns>教室列表</returns>
        public async Task<TblDatClassRoom> GetClassRoomByWhere(string schoolId, string roomNo)
        {
            return await base.LoadTask(m => m.SchoolId == schoolId && m.RoomNo == roomNo);
        }

        /// <summary>
        /// 根据条件获取教室
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-09</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <returns>教室列表</returns>
        public async Task<List<TblDatClassRoom>> GetClassRoomByWhere(string schoolId)
        {
            return await base.LoadLisTask(m => m.SchoolId == schoolId);
        }

        /// <summary>
        /// 获取校区下所有的教室信息
        /// <para>作    者：Huang GaoLiang</para>
        /// <para>创建时间：2019-03-18</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <returns>返回教室列表</returns>
        public List<TblDatClassRoom> GetClassRoomBySchoolId(string schoolId)
        {
            return base.LoadList(m => m.SchoolId == schoolId);
        }

        /// <summary>
        /// 根据教室编号集合获取教室信息
        /// <para>作    者：Huang GaoLiang</para>
        /// <para>创建时间：2019-03-21 </para>
        /// </summary>
        /// <param name="roomIds">教室编号</param>
        /// <returns>返回教室信息</returns>
        public List<TblDatClassRoom> GetClassRoomByRoomId(List<long> roomIds)
        {
            return base.LoadList(m => roomIds.Contains(m.ClassRoomId));
        }
    }
}
