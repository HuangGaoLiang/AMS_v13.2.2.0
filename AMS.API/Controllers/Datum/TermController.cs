using AMS.API.Filter;
using AMS.Dto;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 描述：学期资源 REST
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-7</para>
    /// </summary>
    [Route("api/AMS/Term")]
    [ApiController]
    public class TermController : BaseController
    {
        /// <summary>
        /// 描述：获取设置中的数据,该数据可能包含审核中及已生效数据
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <returns>学期列表</returns>
        [HttpGet, Route("GetTermAuditList")]
        [SchoolIdValidator]
        public TermListResponse GetTermAuditList()
        {
            TermService service = new TermService(base.SchoolId, base.Year);
            return service.TermList;
        }

        /// <summary>
        /// 描述：获取已生效的收费标准数据
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <returns>已生效的学期列表</returns>
        [HttpGet]
        [SchoolIdValidator]
        public List<TermDetailResponse> Get()
        {
            TermService service = new TermService(base.SchoolId, base.Year);
            var result = service.GetTermList();
            return result;
        }

        /// <summary>
        /// 描述：获取审核中的学期收费标准(平台审核页面使用)
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="auditId">审核Id</param>
        /// <returns>审核中的学期列表</returns>
        [HttpGet, Route("GetAuditingTermList")]
        [SchoolIdValidator]
        public List<TermDetailResponse> GetAuditingTermList(long auditId)
        {
            TermService service = new TermService(base.SchoolId, base.Year);
            var result = service.GetAuditTermList(auditId);
            return result;
        }

        /// <summary>
        /// 描述：提交审核
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="data">要提交审核的学期信息</param>
        /// <returns>无</returns>
        [HttpPost]
        [SchoolIdValidator]
        public void Post(TermAuditRequest data)
        {
            TermService service = new TermService(base.SchoolId, base.Year);
            data.CreateUserName = base.CurrentUser.UserName;
            service.Audit(data);
        }

        /// <summary>
        /// 描述：删除学期
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="termId">学期表主键</param>
        /// <returns>无</returns>
        [HttpDelete]
        [SchoolIdValidator]
        public void Delete(long termId)
        {
            TermService service = new TermService(base.SchoolId, base.Year);
            service.DeleteTerm(termId);
        }
        /// <summary>
        /// 描述：获取可预见的年份
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <returns>年度列表</returns>
        [HttpGet, Route("GetPredictYears")]
        public List<int> GetPredictYears()
        {
            var schoolNo = base.SchoolId;
            return TermService.GetPredictYears(schoolNo);
        }

        /// <summary>
        /// 根据学期，获取学期开始日期至结束日期之间的所有月份
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="termId">学期Id</param>
        /// <returns>学期所有月份</returns>
        [HttpGet, Route("GetMonthListByTerm")]
        public async Task<List<string>> GetMonthListByTerm(long termId)
        {
            return await TermService.GetMonthListByTerm(termId);
        }
    }
}
