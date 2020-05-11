/*此代码由生成工具字段生成，生成时间2019/3/14 16:29:38 */
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AMS.Storage.Context;
using AMS.Storage.Models;
using System;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描    述：考勤确认表仓储
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-14</para>
    /// </summary>
    public class TblFinAttendanceConfirmRepository : BaseRepository<TblFinAttendanceConfirm>
    {
        /// <summary>
        /// 根据校区Id、老师Id集合和班级Id集合，获取老师考勤确认列表信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        /// <param name="schooId">校区Id</param>
        /// <param name="teacherIdList">老师Id集合</param>
        /// <param name="classIdList">班级Id集合</param>
        /// <returns>老师考勤确认列表信息</returns>
        public List<TblFinAttendanceConfirm> GetAttendanceConfirmList(string schooId, List<string> teacherIdList, List<long> classIdList)
        {
            return LoadList(a => a.SchoolId == schooId && teacherIdList.Contains(a.TeacherId) && classIdList.Contains(a.ClassId));
        }

        /// <summary>
        /// 根据校区Id、老师Id、班级Id和月份，获取对应的考勤确认信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="teacherId">老师Id</param>
        /// <param name="classId">班级Id</param>
        /// <param name="month">月份（0表示学期，其他表示月份）</param>
        /// <returns>考勤确认信息</returns>
        public async Task<TblFinAttendanceConfirm> GetAttendConfirmInfo(string schoolId, string teacherId, long classId, int? month)
        {
            return await LoadTask(a => a.SchoolId == schoolId && a.TeacherId == teacherId && a.ClassId == classId && a.Month == month);
        }
    }
}
