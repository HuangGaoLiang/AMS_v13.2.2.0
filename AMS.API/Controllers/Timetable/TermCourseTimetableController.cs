using AMS.Dto;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 学期课表资源
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2018-09-22</para>
    /// </summary>
    [Route("api/AMS/TermCourseTimetable")]
    [ApiController]
    public class TermCourseTimetableController : BaseController
    {
        #region Get 学期排课数据
        /// <summary>
        /// 学期排课数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-22</para>
        /// </summary>
        /// <returns>学期排课</returns>
        [HttpGet]
        public TermTimetableResponse Get(long termId)
        {
            TermCourseTimetableService service = new TermCourseTimetableService(termId);
            return service.TermTimetable;
        }
        #endregion

        #region GetTermTimetableSetting 设置中的学期排课数据
        /// <summary>
        /// 设置中的学期排课数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-22</para>
        /// </summary>
        /// <param name="termId">学期Id</param>
        /// <returns>设置中的学期排课</returns>
        [HttpGet, Route("GetTermTimetableSetting")]
        public TermTimetableResponse GetTermTimetableSetting(long termId)
        {
            TermCourseTimetableService service = new TermCourseTimetableService(termId);
            return service.TermTimetableSetting;
        }
        #endregion

        #region GetTeacherByTermId 获取本学期的上课老师
        /// <summary>
        /// 获取本学期的上课老师
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-22</para>
        /// </summary>
        /// <param name="termId">学期Id</param>
        /// <returns>上课老师</returns>
        [HttpGet, Route("GetTeacherByTermId")]
        public List<ClassTimetableTeacherResponse> GetTeacherByTermId(long termId)
        {
            return new TermCourseTimetableService(termId).GetTeacherByTermId();
        }
        #endregion

        /// <summary>
        /// 描述：获取审核中的排课表数据
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.10.14</para>
        /// </summary>
        /// <param name="auditId">审核主表Id</param>
        /// <returns>审核中的排课表</returns>
        [HttpGet, Route("GetAuditTermTimetableSetting")]
        public TermTimetableResponse GetAuditTermTimetableSetting(long auditId)
        {
            var result = TermCourseTimetableAuditService.CreateByAutitId(auditId).TermTimetable;
            return result;
        }

        /// <summary>
        /// 描述：获取排课列表的筛选条件数据
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.25</para>
        /// </summary>
        /// <returns>校区年度下的学期数据</returns>
        [HttpGet, Route("GetSearchData")]
        public ClassCourseSearchSchoolResponse GetSearchData()
        {
            var schoolId = base.SchoolId;
            return TermCourseTimetableService.CreateSearchBuilder().GetSearchData(schoolId,base.CurrentUser.CompanyId);
        }

        /// <summary>
        /// 描述：获取老师的当前学期上哪些课程
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.25</para>
        /// </summary>
        /// <returns>学期下的班级数据</returns>
        [HttpGet, Route("GetTeacherCourse")]
        public List<ClassCourseSearchTeacherResponse> GetTeacherCourse(long termId)
        {
            return TermCourseTimetableService.CreateSearchBuilder().GetTermIdByTeacherList(termId);
        }

        /// <summary>
        /// 提交审核
        /// 作     者:Huang GaoLiang 2018年9月19日16:26:28
        /// </summary>
        /// <param name="termId">学期编号</param>
        /// <param name="auditId">审核人编号</param>
        /// <param name="auditName">审核人名称</param>
        [HttpPost, Route("Audit")]
        public void Audit(long termId, string auditId, string auditName)
        {
            TermCourseTimetableService service = new TermCourseTimetableService(termId);
            service.SubmitAudit(auditId, auditName,base.CurrentUser.UserId,base.CurrentUser.UserName);
        }

        /// <summary>
        /// 复制课表
        /// 作     者:Huang GaoLiang 2018年9月26日10:37:49
        /// </summary>
        /// <param name="termId">当前学期Id</param>
        /// <param name="toTermId">Copy到的学期id</param>
        /// <returns></returns>
        [HttpPost, Route("CopyTermTimetable")]
        public async Task CopyTermTimetable(long termId, long toTermId)
        {
            TermCourseTimetableService service = new TermCourseTimetableService(termId);
            await service.CopyToTermTimetable(toTermId);
        }
    }
}
