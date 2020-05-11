/*此代码由生成工具字段生成，生成时间2018/9/7 12:42:58 */
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描    述: TblDatSchoolTime仓储 
    /// <para>作    者: Huang GaoLiang</para>
    /// <para>创建时间:2018-09-19</para>
    /// </summary>
    public class TblDatSchoolTimeRepository : BaseRepository<TblDatSchoolTime>
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public TblDatSchoolTimeRepository()
        {
        }

        /// <summary>
        /// 带上下文构造函数
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public TblDatSchoolTimeRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 查询学期时间段数据
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2019-02-14</para>
        /// </summary>
        /// <param name="termId">学期编号</param>
        /// <returns></returns>
        public List<TblDatSchoolTime> GetSchoolTimeByTermId(long termId)
        {
            return base.LoadList(m => m.TermId == termId);
        }

        /// <summary>
        /// 根据学期和星期几获取上课时间段
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-09-12</para>
        /// </summary>
        /// <param name="termId">学期编号</param>
        /// <param name="weekDay">星期几</param>
        /// <returns>返回上课时间段集合</returns>
        public IQueryable<TblDatSchoolTime> GetSchoolTimeByTermAndWeekDayList(long termId, int weekDay)
        {
            IQueryable<TblDatSchoolTime> query = base.LoadQueryable().Where(m => m.TermId == termId && m.WeekDay == weekDay);
            return query;
        }

        /// <summary>
        /// 批量添加上课时间段
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-09-12</para>
        /// </summary>
        /// <param name="schoolTimeList">上课时间段</param>
        public async Task BatchInsertsAsync(List<TblDatSchoolTime> schoolTimeList)
        {
            await this.SaveTask(schoolTimeList);
        }

        /// <summary>
        /// 根据条件删除上课时间段
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-09-10</para>
        /// </summary>
        /// <param name="termId">学期编号</param>
        /// <param name="weekDay">星期几</param>
        public async Task DeleteBytermIdAndWeek(long termId, int weekDay)
        {
            Expression<Func<TblDatSchoolTime, bool>> lam = t => t.TermId == termId && t.WeekDay == weekDay;

            await this.DeleteTask(lam, lam);
        }

        /// <summary>
        /// 根据条件获取上课时间段
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-09-12</para>
        /// </summary>
        /// <param name="termId">学期编号</param>
        /// <param name="weekDay">星期几</param>
        /// <param name="duration">时间60/90分钟</param>
        /// <returns>返回上课时间段数据</returns>
        public async Task<List<TblDatSchoolTime>> Get(long termId, int weekDay, int duration)
        {
            return await this.LoadLisTask(m => m.TermId == termId && m.WeekDay == weekDay && m.Duration == duration);
        }
        /// <summary>
        /// 批量删除上课时间段
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-09-12</para>
        /// </summary>
        /// <param name="termId">学期编号</param>
        /// <param name="weekDay">星期几</param>
        /// <param name="duration">时间</param>
        /// <returns>返回删除成功或失败信息</returns>
        public async Task DeleteTask(long termId, int weekDay, int duration)
        {
            Expression<Func<TblDatSchoolTime, bool>> lam = t => t.TermId == termId && t.WeekDay == weekDay && t.Duration == duration;
            await this.DeleteTask(lam, lam);
        }

        /// <summary>
        /// 根据学期编号删除上课时间段
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-09-12</para>
        /// </summary>
        /// <param name="termId">学期编号</param>
        /// <returns>返回删除成功或失败信息</returns>
        public async Task DeleteTask(long termId)
        {
            Expression<Func<TblDatSchoolTime, bool>> lam = t => t.TermId == termId;
            await this.DeleteTask(lam, lam);
        }

        /// <summary>
        /// 描述：根据校区Id和学期Id集合获取时间段信息
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-14</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="termIds">学期Id集合</param>
        /// <returns></returns>
        public List<TblDatSchoolTime> GetSchoolOrTermBySchoolTimeList(string schoolId,IEnumerable<long> termIds)
        {
            var result = base.LoadList(x=>x.SchoolId.Trim()==schoolId.Trim()&&termIds.Contains(x.TermId));
            return result;
        }
    }
}
