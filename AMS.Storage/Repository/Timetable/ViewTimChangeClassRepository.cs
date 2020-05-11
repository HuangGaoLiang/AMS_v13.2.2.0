using AMS.Dto;
using AMS.Models;
using Jerrisoft.Platform.Public.PageExtensions;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描述：转班明细列表仓储
    /// <para>作者：瞿琦</para>
    /// <para>创建时间：2019-3-6</para>
    /// </summary>
    public class ViewTimChangeClassRepository : BaseRepository<ViewTimChangeClass>
    {
        /// <summary>
        /// 描述：获取转班明细列表
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-6</para>
        /// </summary>
        /// <returns>转班明细列表分页</returns>
        public PageResult<ViewTimChangeClass> GetTimeChangeClassList(string schoolId, ChangeClassListSearchRequest searcher)
        {
            var querySql = $@"select a.SchoolId,
	                                 a.StudentId,
	                                 b.StudentNo,
	                                 b.StudentName,
	                                 d.CreateTime as EnrollTime,
	                                 d.TotalTradeAmount,
	                                 c.ClassTimes as EnrollClassTimes, 
	                                 c.Year,
	                                 c.TermTypeId,
	                                 a.OutClassId,
	                                 a.OutDate,
	                                 a.CreateTime as ChangeClassSubmitTime,
	                                 a.ClassTimes as ChangeClassTimes,
	                                 a.InClassId,
	                                 a.InDate
                             from (select * from  [TblTimChangeClass] where SchoolId=@SchoolId ) as a
                                     left join [TblCstStudent] as b on a.StudentId=b.StudentId
                                     left join [dbo].[TblOdrEnrollOrderItem] as c on c.EnrollOrderItemId=a.EnrollOrderItemId
                                     left join [dbo].[TblOdrEnrollOrder] as d on c.EnrollOrderId =d.EnrollOrderId 
                             where 1=1 ";
            if (searcher.Year > 0)   //年度
            {
                querySql += " and c.Year= " + searcher.Year;
            }
            if (searcher.TermTypeId > 0)   //学期类型
            {
                querySql += " and c.TermTypeId= " + searcher.TermTypeId;
            }
            if (searcher.ChangeClassTimeStart.HasValue)   //转班日期开始时间
            {
                querySql += " and a.CreateTime >= '" + searcher.ChangeClassTimeStart + "'";
            }
            if (searcher.ChangeClassTimeEnd.HasValue)   //转班日期结束时间
            {
                querySql += " and a.CreateTime <= '" + searcher.ChangeClassTimeEnd + "'";
            }
            if (!string.IsNullOrWhiteSpace(searcher.StudentName))
            {
                querySql += " and b.StudentName like '%" + searcher.StudentName + "%'";
            }
            var timChangeClassList = base.CurrentContext.ViewTimChangeClass.FromSql(querySql, new object[] {
                new SqlParameter("@SchoolId",schoolId.Trim())
            });

            var leaveSchoolQuery = timChangeClassList
                //.WhereIf(searcher.Year > 0, x => x.Year == searcher.Year)                                                             //年度
                //.WhereIf(searcher.TermTypeId > 0, x => x.TermTypeId == searcher.TermTypeId)                                           //学期类型
                //.WhereIf(searcher.ChangeClassTimeStart.HasValue, x => searcher.ChangeClassTimeStart <= x.ChangeClassSubmitTime)          //转班日期开始时间
                //.WhereIf(searcher.ChangeClassTimeEnd.HasValue, x => x.ChangeClassSubmitTime <= searcher.ChangeClassTimeEnd.Value.AddDays(1)) //转班日期结束时间
                //.WhereIf(!string.IsNullOrWhiteSpace(searcher.StudentName), x => x.StudentName.Contains(searcher.StudentName))
                .OrderByDescending(x => x.ChangeClassSubmitTime)
                .ToPagerSource(searcher.PageIndex, searcher.PageSize);
            return leaveSchoolQuery;
        }
    }
}
