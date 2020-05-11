using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Dto;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;
using AMS.API.Filter;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 教室资源
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-02-27</para>
    /// </summary>
    [Produces("application/json"), Route("api/AMS/ClassRoom")]
    [ApiController]
    public class ClassRoomController : BaseController
    {
        /// <summary>
        /// 获取所有教室
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-11</para>
        /// </summary>
        /// <returns>所有教室列表</returns>
        [HttpGet, Route("GetAllClassRoom")]
        [SchoolIdValidator]
        public List<RoomCourseResponse> GetAllClassRoom()
        {
            SchoolClassRoomService service = new SchoolClassRoomService(base.SchoolId);
            return service.RoomCourseList;
        }

        /// <summary>
        /// 获取所有已启用的教室
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-30</para>
        /// </summary>
        /// <returns>所有已启用的教室列表</returns>
        [HttpGet, Route("GetEnableClassRoom")]
        [SchoolIdValidator]
        public List<ClassRoomResponse> GetEnableClassRoom()
        {
            SchoolClassRoomService service = new SchoolClassRoomService(base.SchoolId);
            return service.GetAllEnableClassRoom();
        }

        /// <summary>
        /// 设置启用
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-11</para>
        /// </summary>
        /// <param name="roomCourseId">教室课程Id</param>
        [HttpPost, Route("SetEnable")]
        public async Task SetEnable(long roomCourseId)
        {
            ClassRoomCourseService classRoomService = new ClassRoomCourseService(roomCourseId);
            await classRoomService.SetCourseEnable();
        }

        /// <summary>
        /// 设置禁用
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-11</para>
        /// </summary>
        /// <param name="roomCourseId">教室课程Id</param>
        [HttpPost, Route("SetDisable")]
        public async Task SetDisable(long roomCourseId)
        {
            ClassRoomCourseService classRoomService = new ClassRoomCourseService(roomCourseId);
            await classRoomService.SetCourseDisable();
        }

        /// <summary>
        /// 删除
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-11</para>
        /// </summary>
        /// <param name="roomCourseId">教室课程Id</param>
        [HttpDelete]
        public async Task Delete(long roomCourseId)
        {
            ClassRoomCourseService classRoomService = new ClassRoomCourseService(roomCourseId);
            await classRoomService.Remove();
        }

        /// <summary>
        /// 添加 教室分配及学位设置
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-11</para>
        /// </summary>
        /// <param name="request">教室分配及学位设置Dto</param>
        [HttpPost]
        public async Task Post([FromBody]ClassRoomRequest request)
        {
            await ClassRoomCourseService.AddAsync(base.SchoolId, request);
        }

        /// <summary>
        /// 修改 教室分配及学位设置
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-10</para>
        /// </summary>
        /// <param name="roomCourseId">班级课程Id</param>
        /// <param name="request">教室分配及学位设置Dto</param>
        [HttpPut]
        public async Task Put(long roomCourseId, [FromBody]ClassRoomRequest request)
        {
            ClassRoomCourseService classRoomService = new ClassRoomCourseService(roomCourseId);
            await classRoomService.Modify(request);
        }
    }
}
