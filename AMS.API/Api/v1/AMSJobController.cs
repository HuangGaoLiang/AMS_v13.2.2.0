using AMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AMS.API.Api.v1
{
    /// <summary>
    /// 招生系统任务调用业务接口
    /// </summary>
    [Produces("application/json"), Route("api/AMS/AMSJob/v1")]
    [ApiController]
    public class AMSJobController : ControllerBase //DefaultScheduleApiController
    {

        /// <summary>
        /// 学生在读状态更新任务
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2018-11-12 </para>
        /// </summary>
        [HttpPost, Route("RunStudentStatusJob")]
        public async Task RunStudentStatusJob()
        {
            await StudentService.RunStudentStatusJob();
        }

        /// <summary>
        /// 更新学习计划任务
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("RunStudyPlanJob")]
        public async Task RunStudyPlanJob()
        {
            await StudyPlanService.StartAsync();
        }

        /// <summary>
        /// 更新学生联系人至学生联系人表
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2019-03-06 </para>
        /// </summary>
        /// <param name="passWord">密码</param>
        [HttpPost, Route("RunStudentInfoJob")]
        public void RunStudentInfoJob(string passWord)
        {
            StudentService.RunStudentInfoJob(passWord);
        }

    }
}
