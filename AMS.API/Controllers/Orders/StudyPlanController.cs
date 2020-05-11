using AMS.Dto;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 描    述：课程学习计划控制器
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-10-29</para>
    /// </summary>
    [Route("api/AMS/StudyPlan")]
    [ApiController]
    public class StudyPlanController : BsnoController
    {
        /// <summary>
        /// 打开报名，根据出生日期推荐课程学习计划
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="birthday">出生日期</param>
        /// <returns>学习课程计划列表</returns>
        [HttpGet]
        public List<StudyPlanResponse> Get(DateTime birthday)
        {
            return new StudyPlanService(base.SchoolId).GetStudyPlanList(birthday);
        }

        /// <summary>
        /// 根据选择课程级别，获取课程学习计划
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="birthday">出生日期</param>
        /// <param name="courseId">课程Id</param>
        /// <param name="courseLevelId">级别Id</param>
        /// <returns>学习课程计划列表</returns>
        [HttpGet, Route("GetStudyPlan")]
        public List<StudyPlanResponse> GetStudyPlan(DateTime birthday, long courseId, long courseLevelId)
        {
            return new StudyPlanService(base.SchoolId).GetStudyPlanList(birthday, courseId, courseLevelId);
        }
    }
}
