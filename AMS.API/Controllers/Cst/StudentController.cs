using AMS.Dto;
using AMS.Service;
using AMS.Storage.Models;
using Jerrisoft.Platform.Public.PageExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 描    述: 学生信息资源
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    [Produces("application/json"), Route("api/AMS/Student")]
    [ApiController]
    public class StudentController : BaseController
    {
        /// <summary>
        /// 实例化学生服务
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2018-11-12 </para>
        /// </summary>
        private StudentService Service
        {
            get
            {
                return new StudentService(base.SchoolId);
            }
        }


        /// <summary>
        /// 根据学生编号获取学生信息
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-11-02 </para>
        /// </summary>
        /// <param name="studentId">学生编号</param>
        [HttpGet]
        public StudentDetailResponse Get(long studentId)
        {
            return Service.GetStudent(studentId);
        }

        /// <summary>
        /// 根据学生姓名或手机号码查询学生 学生注册功能
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-11-02 </para>
        /// </summary>
        /// <param name="keyWord">学生信息</param>
        /// <returns>返回学生信息数据</returns>
        [HttpGet, Route("GetList")]
        public List<StudentSearchResponse> GetList(string keyWord)
        {
            return StudentService.GetListBykeyWord(keyWord, base.CurrentUser.CompanyId);
        }

        /// <summary>
        /// 获取本校区的学生列表 退费功能
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-11-02</para>
        /// </summary>
        /// <param name="keyWord">学生信息</param>
        /// <returns>返回学生信息数据</returns>
        [HttpGet, Route("GetSchoolStudentList")]
        public List<StudentSearchResponse> GetSchoolStudentList(string keyWord)
        {
            return Service.GetSchoolStudentList(keyWord);
        }

        /// <summary>
        /// 根据条件查询学员信息 学生管理列表
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-11-02</para>
        /// </summary>
        /// <param name="req">查询条件</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>返回分页列表数据</returns>
        [HttpGet, Route("GetStudentList")]
        public PageResult<StudentListResponse> GetStudentList([FromQuery]StudentListSearchRequest req, int pageIndex, int pageSize)
        {
            return new StudentService(base.SchoolId).GetStudentPageList(req, pageIndex, pageSize);
        }

        /// <summary>
        /// 学生注册
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-10-08 </para>
        /// </summary>
        /// <param name="request">注册学生请求数据</param>
        /// <returns>返回学生编号</returns>
        [HttpPost]
        public string Post(StudentRegisterRequest request)
        {
            return Service.Register(request);
        }

        /// <summary>
        /// 修改学生信息
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-11-02</para>
        /// </summary>
        /// <param name="studentId">学生编号</param>
        /// <param name="request">学生修改请求数据</param>
        /// <returns>返回收影响的行数</returns>
        [HttpPut]
        public void Put(long studentId, [FromBody]StudentRegisterRequest request)
        {
            request.SchoolId = base.SchoolId;
            request.UserId = base.CurrentUser.UserId;
            request.UserName = base.CurrentUser.UserName;
            StudentService.Modify(studentId, request);
        }

        /// <summary>
        /// 更新最新头像 修改学生信息
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-11-02</para>
        /// </summary>
        /// <param name="studentId">学生编号</param>
        /// <param name="newHeadFace">头像地址</param>
        [HttpPost, Route("ModifyHeadFace")]
        public void ModifyHeadFace(long studentId, string newHeadFace)
        {
            StudentService.ModifyHeadFace(studentId, newHeadFace);
        }

        /// <summary>
        /// 根据学生编号、学生名称、学生联系电话检查学生是否存在 
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-11-02</para>
        /// </summary>
        /// <param name="studentId">学生编号</param>
        /// <param name="studentName">学生名称</param>
        /// <param name="linkMobile">电话号码</param>
        /// <returns>返回学生编号</returns>
        [HttpGet, Route("ExistStudent")]
        public long ExistStudent(long studentId, string studentName, string linkMobile)
        {
            return StudentService.ExistStudent(studentId, studentName, linkMobile);
        }

        /// <summary>
        /// 统计本校区的学生数据
        /// <para>作    者: 黄高亮 </para>
        /// <para>创建时间: 2019-03-06</para>
        /// </summary>
        /// <returns>返回本校的学生总数量</returns>
        [HttpGet, Route("CountSchoolStudent")]
        public long CountSchoolStudent()
        {
            return Service.CountSchoolStudent();
        }

        /// <summary>
        /// 获取学生卡打印信息
        /// <para>作    者:Huang GaoLiang </para>
        /// <para>创建时间:2019-03-06 </para>
        /// </summary>
        /// <param name="studentId">学生编号</param>
        /// <returns>返回学生信息</returns>
        [HttpGet, Route("GetStudentCard")]
        public StudentCardResponse GetStudentCard(long studentId)
        {
            return Service.GetStudentCard(studentId, base.SchoolId);
        }
    }
}
