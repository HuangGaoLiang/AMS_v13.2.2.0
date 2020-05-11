using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Anticorrosion.HRS;
using AMS.Dto;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;
using YMM.HRS.SDK;
using YMM.HRS.SDK.Dto;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 班级课表资源
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2018-09-22</para>
    /// </summary>
    [Route("api/AMS/ClassCourseTimetable")]
    [ApiController]
    public class ClassCourseTimetableController : BaseController
    {
        #region Get 班级课表详情
        /// <summary>
        /// 班级课表详情
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-22</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <param name="termId">学期Id</param>
        /// <returns>课表详情</returns>
        [HttpGet]
        public ClassTimetableResponse Get(long classId, long termId)
        {
            return ClassCourseTimetableService.GetClassTimetable(classId, termId);
        }
        #endregion

        #region GetNoClassTimetable 获取未安排课表的班级课表信息
        /// <summary>
        /// 获取未安排课表的班级课表信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-22</para>
        /// </summary>
        /// <param name="classRoomId">教室Id</param>
        /// <param name="schoolTimeId">上课时间段Id</param>
        /// <returns>班级课表信息</returns>
        [HttpGet, Route("GetNoClassTimetable")]
        public ClassTimetableResponse GetNoClassTimetable(long classRoomId, long schoolTimeId)
        {
            return ClassCourseTimetableService.GetNoClassTimetable(classRoomId, schoolTimeId);
        }
        #endregion

        #region Post 添加班级排课
        /// <summary>
        /// 添加班级排课
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-22</para>
        /// </summary>
        /// <param name="classRoomId">教室Id</param>
        /// <param name="schoolTimeId">上课时间段Id</param>
        /// <param name="input">班级排课信息</param>
        [HttpPost]
        public void Post(long classRoomId, long schoolTimeId, [FromBody]ClassScheduleRequest input)
        {
            TermCourseTimetableAuditService.AddClass(classRoomId, schoolTimeId, input,base.CurrentUser.UserName);
        }
        #endregion

        #region Delete 删除班级
        /// <summary>
        /// 删除班级
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-22</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <param name="termId">学期Id</param>
        [HttpDelete]
        public void Delete(long classId, long termId)
        {
            new TermCourseTimetableAuditService(termId).DeleteClass(classId, base.CurrentUser.UserName);
        }
        #endregion

        #region Put 修改校区班级课表排课
        /// <summary>
        /// 修改校区班级课表排课
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-22</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <param name="termId">学期Id</param>
        /// <param name="input">班级排课属性</param>
        [HttpPut]
        public async Task Put(long classId, long termId, [FromBody]EditClassScheduleRequest input)
        {
            await new TermCourseTimetableAuditService(termId).ModifyClassAsync(classId, input,base.CurrentUser.UserName);
        }
        #endregion

        /// <summary>
        /// 获取人员及部门信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetPersonnelOfDeptData")]
        public List<PersonnelOfDeptData> GetPersonnelOfDeptData()
        {
            HrSystem hrSystem = new HrSystem();

            //未知查询条件,查询所有转换再处理
            var personList = hrSystem.GetPersonnelDataByCompanyId(new List<string>());


            return personList;
        }
    }
}
