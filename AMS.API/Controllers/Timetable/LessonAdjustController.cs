using AMS.Dto;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AMS.API.Controllers
{
    /// <summary>
    /// 描    述：课程调整业务--老师代课
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2019-3-8</para>
    /// </summary>
    [Route("api/AMS/LessonAdjust")]
    [ApiController]
    public class LessonAdjustController : BsnoController
    {
        /// <summary>
        /// 描述：教师代课调整
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-3-8</para>
        /// <param name="request">教师代课调整要保存的信息</param>
        [HttpPost, Route("AdjustTeacher")]
        public void AdjustTeacher([FromBody]AdjustTeacherRequest request)
        {
            var service = new AdjustLessonTeacherService(base.SchoolId);
            service.Adjust(request);
        }


        /// <summary>
        /// 全校上课日期调整
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-3-8</para>
        /// <param name="request">全校上课日期调整要保存的信息</param>
        [HttpPost, Route("AdjustSchoolClassTime")]
        public void AdjustSchoolClassTime([FromBody]AdjustSchoolClassTimeRequest request)
        {
            var service = new AdjustLessonSchoolClassTimeService(base.SchoolId);
            service.Adjust(request);
        }

        /// <summary>
        /// 班级上课时间调整
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-3-8</para>
        /// <param name="request">班级上课时间调整要保存的信息param>
        [HttpPost, Route("AdjustClassTime")]
        public void AdjustClassTime([FromBody]AdjustClassTimeRequest request)
        {
            var service = new AdjustLessonClassTimeService(base.SchoolId);
            service.Adjust(request);
        }
    }
}
