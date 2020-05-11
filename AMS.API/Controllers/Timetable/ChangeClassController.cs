using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.API.Filter;
using AMS.Dto;
using AMS.Service;
using Jerrisoft.Platform.Public.PageExtensions;
using Microsoft.AspNetCore.Mvc;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 描述：转班资源
    /// <para>作    者:瞿琦</para>
    /// <para>创建时间：2018-11-5</para>
    /// </summary>
    [Produces("application/json"), Route("api/AMS/ChangeClass")]
    [ApiController]
    [SchoolIdValidator]
    public class ChangeClassController : BaseController
    {
        /// <summary>
        /// 描述:获取转班明细列表
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-3-6</para>
        /// </summary>
        /// <param name="searcher">列表筛选条件</param>
        /// <returns>转班明细分页列表</returns>
        [HttpGet, Route("GetChangeClassList")]
        public PageResult<ChangeClassListResponse> GetChangeClassList([FromQuery]ChangeClassListSearchRequest searcher)
        {
            var service = new ChangeClassService(base.SchoolId);
            var result=  service.GetChangeClassList(searcher);
            return result;
        }


        /// <summary>
        /// 描述：获取可转班的学期和班级数据
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-5</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <returns>可以转班的学期和班级</returns>
        [HttpGet, Route("GetChangeClassTermClass")]
        public List<StudentTermResponse> GetChangeClassTermClass(long studentId)
        {
            var service = new ChangeClassService(base.SchoolId,studentId);
            return  service.GetChangeClassTermClass();
        }

        /// <summary>
        /// 描述：获取转出的课次
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-5</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <param name="outClassId">转出班级</param>
        /// <param name="outDate">停课日期</param>
        /// <returns>可以转出的课次</returns>
        [HttpGet, Route("GetChangeOutClassCount")]
        public int GetChangeOutClassCount(long studentId, long outClassId, DateTime outDate)
        {
            var service = new ChangeClassService(base.SchoolId, studentId);
            var result=  service.GetChangeOutClassCount(outClassId, outDate);
            return result;
        }

        /// <summary>
        /// 描述：获取转入的课次
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-5</para>
        /// </summary>
        ///<param name="studentId">学生Ids</param>
        /// <param name="inClassId">转入班级</param>
        /// <param name="inDate">上课日期</param>
        /// <returns>可以转入的课次</returns>
        [HttpGet, Route("GetChangeInClassCount")]
        public int GetChangeInClassCount(long studentId,long inClassId, DateTime inDate)
        {
            var service = new ChangeClassService(base.SchoolId, studentId);
            var result= service.GetChangeInClassCount(inClassId, inDate);
            
            return result;
        }


        /// <summary>
        /// 描述：保存转班办理
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-5</para>
        /// </summary>
        /// <param name="dto">要添加的转班信息</param>
        [HttpPost]
        public void Post(ChangeClassAddRequest dto)
        {
            var service = new ChangeClassService(base.SchoolId, dto.StudentId);
            service.ChangeIn(dto);
        }
    }
}
