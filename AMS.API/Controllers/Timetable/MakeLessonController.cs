using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Dto;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 课程排课
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2018-11-05</para> 
    /// </summary>
    [Route("api/AMS/MakeLesson")]
    public class MakeLessonController : BsnoController
    {
        /// <summary>
        /// 保存排课
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-05</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <param name="requests">报名的排课信息</param>
        [HttpPost]
        public void Post(long studentId, List<MakeLessonRequest> requests)
        {
            new MakeLessonService(base.SchoolId, studentId).Save(requests);
        }

        /// <summary>
        /// 排课确认
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-05</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <param name="enrollOrderItemId">报名课程订单Id</param>
        [HttpPost, Route("Confirm")]
        public void Confirm(long studentId, long enrollOrderItemId)
        {
            new MakeLessonService(base.SchoolId, studentId).Confirm(enrollOrderItemId);
        }

        /// <summary>
        /// 获取学生排课列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-05</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        [HttpGet, Route("GetMakeLessonList")]
        public List<MakeLessonListResponse> GetMakeLessonList(long studentId)
        {
            return new MakeLessonService(base.SchoolId, studentId).GetMakeLessonList();
        }

        /// <summary>
        /// 获取的排课详情
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-01</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <param name="enrollOrderItemId">报名订单课程明细Id</param>
        /// <returns></returns>
        [HttpGet, Route("GetMakeLessonDetail")]
        public MakeLessonDetailResponse GetMakeLessonDetail(long studentId, long enrollOrderItemId)
        {
            return new MakeLessonService(base.SchoolId, studentId).GetMakeLessonDetail(enrollOrderItemId);
        }

        /// <summary>
        /// 获取学生已排课列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-05</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <param name="enrollOrderItemId">报名课程Id</param>
        [HttpGet, Route("GetHasMakeLessonList")]
        public async Task<List<StudentMakeLessonResponse>> GetHasMakeLessonList(long studentId, long enrollOrderItemId)
        {
            return await new StudentTimetableService(base.SchoolId, studentId).GetHasMakeLessonListAsync(enrollOrderItemId);
        }

        /// <summary>
        /// 获取获取学生学习记录信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-07</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <returns>学习记录列表</returns>
        [HttpGet, Route("GetStudentStudyRecordList")]
        public List<StudentStudyRecordResponse> GetStudentStudyRecordList(long studentId)
        {
            return new MakeLessonService(base.SchoolId, studentId).GetStudentStudyRecordList();
        }
    }
}
