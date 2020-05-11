/*此代码由生成工具字段生成，生成时间2018/9/7 12:42:58 */
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AMS.Storage.Context;
using AMS.Storage.Models;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;
using System;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描述：TblDatTerm仓储
    /// <para>作  者：瞿琦</para>
    /// <para>创建时间:2018.9.19</para>
    /// </summary>
    public class TblDatTermRepository : BaseRepository<TblDatTerm>
    {
        /// <summary>
        /// 描述：实例化一个TblDatTerm仓储对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间:2018.9.19</para>
        /// </summary>
        public TblDatTermRepository()
        {
        }

        /// <summary>
        /// 描述：实例化一个TblDatTerm仓储对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间:2018.9.19</para>
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public TblDatTermRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 描述：根据校区获取学期列表
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.19</para>
        /// </summary>
        /// <returns>已生效学期列表</returns>
        public List<TblDatTerm> GetShoolNoByTblDatTerm(string schoolId)
        {
            var query = base.LoadQueryable().Where(x => x.SchoolId.Trim() == schoolId.Trim()).ToList();
            return query;
        }

        /// <summary>
        /// 根据校区Id集合获取学期列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="schoolIdList">校区Id集合</param>
        /// <param name="yearList">年份集合</param>
        /// <returns>学期列表</returns>
        public List<TblDatTerm> GetTermListBySchoolIds(List<string> schoolIdList, List<int> yearList = null)
        {
            var query = base.LoadQueryable().Where(a => schoolIdList.Contains(a.SchoolId))
                .WhereIf(yearList != null && yearList.Count > 0, a => yearList.Contains(a.Year)).ToList();
            return query;
        }

        /// <summary>
        /// 描述：根据校区+年度获取已生效学期列表
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.19</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="year">年度</param>
        /// <returns>已生效学期列表</returns>
        public List<TblDatTerm> GetTblDatTremList(string schoolId, int year)
        {
            return base.LoadList(x => x.SchoolId.Trim() == schoolId.Trim() && x.Year == year);
        }

        /// <summary>
        /// 描述：根据校区+多个年度获取已生效学期列表
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.19</para>
        /// </summary>
        /// <returns>已生效学期列表</returns>
        public List<TblDatTerm> GetTblDatTremList(string schoolId, List<int> yearList)
        {
            return base.LoadList(x => x.SchoolId.Trim() == schoolId.Trim() && yearList.Contains(x.Year));
        }

        /// <summary>
        /// 根据学期编号获取学期信息
        /// 作     者:Huang GaoLiang 2018年9月23日18:40:27
        /// </summary>
        /// <param name="termId">学期编号</param>
        /// <returns></returns>
        public async Task<TblDatTerm> GetTblDatTermByTermId(long termId)
        {
            return await base.LoadTask(m => m.TermId == termId);
        }

        /// <summary>
        /// 描述：根据学期编号获取学期信息
        /// <para>作    者:Caiyakang</para> 
        /// <para>创建时间：2018年11月7</para>
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="termTypeId">学期类型ID</param>
        /// <param name="schoolId">校区编号</param>
        /// <returns></returns>
        public List<TblDatTerm> GetTblDatTermByTermTypeId(int year, long termTypeId, string schoolId)
        {
            return base.LoadList(m => m.TermTypeId == termTypeId && m.SchoolId == schoolId && m.Year == year);
        }

        /// <summary>
        /// 根据学期编号获取学期信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="termId">一组学期Id</param>
        /// <returns>学期信息列表</returns>
        public async Task<List<TblDatTerm>> GetTblDatTermByTermId(IEnumerable<long> termId)
        {
            return await base.LoadLisTask(m => termId.Contains(m.TermId));
        }


        /// <summary>
        /// 描述：删除学期数据
        /// <para>作    者：caiyakang</para>
        /// <para>创建时间：2018-09-28</para>
        /// </summary>
        /// <param name="schoolId">校区</param>
        /// <param name="year">年度</param>
        /// <returns>无</returns>
        public void DeleteBySchoolAndYear(string schoolId, int year)
        {
            base.Delete(m => m.SchoolId == schoolId && m.Year == year);
        }

        /// <summary>
        ///  描述：根据校区获取所有的学期
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <returns>已生效学期列表</returns>
        public List<TblDatTerm> GetSchoolIdTermList(string schoolId)
        {
            return base.LoadQueryable(x => x.SchoolId.Trim() == schoolId.Trim(), false).OrderByDescending(x => x.Year).ToList();
        }

        /// <summary>
        /// 描述：获取当前时间之后的学期数据
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="currentDate">当前时间</param>
        /// <returns>已生效校区列表</returns>
        public List<TblDatTerm> GetFutureTerm(string schoolId, DateTime currentDate)
        {
            var result = base.LoadList(x => x.SchoolId.Trim() == schoolId.Trim() && x.EndDate >= currentDate);
            return result;
        }

        /// <summary>
        /// 描述：获取日期所属的学期
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-3-14</para>
        /// </summary>
        /// <param name="day">选择的日期</param>
        /// <returns>学期列表</returns>
        public List<TblDatTerm> GetDateByTermList(DateTime day)
        {
            return base.LoadList(x => x.BeginDate <= day && x.EndDate >= day);
        }
    }
}
