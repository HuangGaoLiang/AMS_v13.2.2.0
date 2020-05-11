/*此代码由生成工具字段生成，生成时间2018/9/7 12:42:58 */
using AMS.Dto;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描述： 补课周信息仓储
    /// <para>作    者：Huang GaoLiang </para>
    /// <para>创建时间：2019-02-28</para>
    /// </summary>
    public class TblDatClassReplenishWeekRepository : BaseRepository<TblDatClassReplenishWeek>
    {
        /// <summary>
        /// TblDatClassReplenishWeek 构造函数
        /// </summary>
        /// <param name="context">数据上下文</param>
        public TblDatClassReplenishWeekRepository(DbContext context) : base(context)
        {

        }

        /// <summary>
        ///  TblDatClassReplenishWeek 构造函数
        /// </summary>
        public TblDatClassReplenishWeekRepository()
        {

        }

        /// <summary>
        /// 根据条件查询补课周补课信息
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-11-21</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <param name="teacherId">老师编号</param>
        /// <param name="studentId">学生编号</param>
        /// <param name="classId">班级编号</param>
        /// <returns>返回补课周补课信息</returns>
        public TblDatClassReplenishWeek GetClassReplenishWeek(string schoolId, string teacherId, long studentId, long classId)
        {
            var info = Load(m => m.SchoolId == schoolId && m.TeacherId == teacherId && m.StudentId == studentId && m.ClassId == classId);

            return info;
        }
    }
}
