/*此代码由生成工具字段生成，生成时间2018/9/7 14:28:29 */
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblDatHoliday仓储
    /// </summary>
    public class TblDatHolidayRepository : BaseRepository<TblDatHoliday>
    {
        public TblDatHolidayRepository()
        {
        }

        public TblDatHolidayRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 批量插入
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-11</para>
        /// </summary>
        /// <param name="datHolidays">听课表实体</param>
        public async Task BatchInsertsAsync(List<TblDatHoliday> datHolidays)
        {
            await this.SaveTask(datHolidays);
        }

        /// <summary>
        /// 删除
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-10</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <param name="year">年份</param>
        public async Task DeleteAsync(string schoolId, int year)
        {
            Expression<Func<TblDatHoliday, bool>> whereLambda = m => m.SchoolId == schoolId && m.Year == year;

            await this.DeleteTask(whereLambda, whereLambda);
        }

        /// <summary>
        /// 获取停课日列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-12</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <param name="year">年度</param>
        /// <returns>停课日列表</returns>
        public async Task<List<TblDatHoliday>> Get(string schoolId, int year)
        {
            return await this.LoadLisTask(m => m.SchoolId == schoolId && m.Year == year);
        }

        /// <summary>
        /// 获取停课日列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-12</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <returns>停课日列表</returns>
        public async Task<List<TblDatHoliday>> Get(string schoolId)
        {
            return await this.LoadLisTask(m => m.SchoolId == schoolId);
        }
    }
}
