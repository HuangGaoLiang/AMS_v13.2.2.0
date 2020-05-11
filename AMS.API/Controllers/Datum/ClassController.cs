using System;
using System.Collections.Generic;
using AMS.Dto;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AMS.API.Filter;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 班级资源
    /// </summary>
    [Produces("application/json"), Route("api/AMS/Class")]
    [ApiController]
    public class ClassController : BaseController
    {
        /// <summary>
        /// 查询班级列表
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2018-11-02 </para>
        /// </summary>
        /// <param name="searcher">班级查询条件</param>
        /// <returns>返回班级列表信息</returns>
        [HttpGet, Route("GetClassList")]
        public List<ClassListResponse> GetClassList([FromQuery]ClassListSearchRequest searcher)
        {
            return DefaultClassService.GetClassList(searcher);
        }

        /// <summary>
        /// 根据年度、学期类型编号和班级编码查询班级详情信息
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2018-11-02 </para>
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="termTypeId">学期类型ID</param>
        /// <param name="classNo">班级编码</param>
        /// <returns>返回班级详细信息</returns>
        [HttpGet, SchoolIdValidator, Route("GetClassDetailByClassNo")]
        public ClassDetailResponse GetClassDetailByClassNo(int year, long termTypeId, string classNo)
        {
            ClassInfo classInfo = new ClassInfo
            {
                Year = year,
                TermTypeId = termTypeId,
                classNo = classNo,
                SchoolId = base.SchoolId,
                CompanyId = base.CurrentUser.CompanyId
            };
            return DefaultClassService.GetClassDetail(classInfo);
        }

        /// <summary>
        /// 根据班级ID查询班级详情信息
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2018-11-02 </para>
        /// </summary>
        /// <param name="classId">班级编号</param>
        /// <returns>返回班级详细信息</returns>
        [HttpGet, Route("GetClassDetailById")]
        public ClassDetailResponse GetClassDetailById(long classId)
        {
            return DefaultClassService.GetClassDetail(classId, base.CurrentUser.CompanyId);
        }

        /// <summary>
        /// 根据首次上课时间获取排课最大课次
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <param name="firstTime">首次上课时间</param>
        /// <returns>排课最大课次</returns>
        [HttpGet, Route("GetMaximumLessonByFirstTime")]
        public int GetMaximumLessonByFirstTime(long classId, DateTime firstTime)
        {
            return new DefaultClassService(classId).GetMaximumLessonByFirstTime(firstTime);
        }

        /// <summary>
        /// 获取插班补上本学期已上课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="classId">班级Id (13988012684421120,13988012684421120,13988012684421120)</param>
        /// <param name="firstTime">首次上课时间</param>
        /// <returns>插班数量</returns>
        [HttpGet, Route("GetTransferLessonByFirstTime")]
        public int GetTransferLessonByFirstTime(string classId, DateTime? firstTime)
        {
            int num = 0;

            List<long> cId = classId.Trim(',').Split(',').Select(x => long.Parse(x)).ToList();

            foreach (var item in cId)
            {
                num += new DefaultClassService(item).GetTransferLessonByFirstTime(firstTime);
            }

            return num;
        }

        /// <summary>
        /// 根据学期Id获取班级代码
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-19</para>
        /// </summary>
        /// <param name="termId">学期Id</param>
        /// <returns>班级信息列表</returns>
        [HttpGet, Route("GetClassListByTermId")]
        public List<ClassInfoListResponse> GetClassListByTermId(long termId)
        {
            return DefaultClassService.GetClassInfoListByTermId(termId);
        }
    }
}
