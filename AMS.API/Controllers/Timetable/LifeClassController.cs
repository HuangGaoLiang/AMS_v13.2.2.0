using AMS.Core;
using AMS.Dto;
using AMS.Service;
using AMS.Storage.Models;
using Jerrisoft.Platform.Public.PageExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AMS.API.Controllers.Internal.Timetable
{
    /// <summary>
    /// 描    述：写生课
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-09</para>
    /// </summary>
    [Produces("application/json"), Route("api/AMS/LifeClass")]
    [ApiController]
    public class LifeClassController : BsnoController
    {
        #region 获取写生课/写生课学生列表
        /// <summary>
        /// 获取写生课列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-09</para>
        /// </summary>
        /// <param name="searcher">写生列表查询条件</param>
        /// <returns>写生课分页列表</returns>
        [HttpGet]
        public PageResult<LifeClassListResponse> Get([FromQuery]LifeClassListSearchRequest searcher)
        {
            return new LifeClassService(base.SchoolId).GetPagerList(searcher);
        }

        /// <summary>
        /// 获取写生课的学生列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-09</para>
        /// </summary>
        /// <param name="searcher">学生排课查询条件</param>
        /// <returns>写生课学生分页列表</returns>
        [HttpGet, Route("GetStudentListOfLifeClass")]
        public PageResult<LifeClassLessonListResponse> GetStudentListOfLifeClass([FromQuery]LifeClassLessonListSearchRequest searcher)
        {
            return new LifeClassService(base.SchoolId).GetPagerLessonList(searcher);
        }
        #endregion

        #region 获取班级人数
        /// <summary>
        /// 获取班级的人数信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="request">写生课学生列表请求对象</param>
        /// <returns>班级学生人数列表</returns>
        [HttpGet, Route("GetLifeClassStudentList")]
        public List<TimeClassStudentResponse> GetLifeClassStudentList([FromQuery]LifeClassStudentRequest request)
        {
            return new LifeClassService(base.SchoolId).GetLifeClassStudentList(request);
        }
        #endregion

        #region 获取写生课主题列表
        /// <summary>
        /// 根据学期Id获取写生课主题列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-20</para>
        /// </summary>
        /// <param name="termId">学期Id</param>
        /// <returns>学期写生课列表</returns>
        [HttpGet, Route("GetLifeClassByTermId")]
        public List<LifeClassTitleListResponse> GetLifeClassByTermId(long termId)
        {
            return new LifeClassService(base.SchoolId).GetLifeClassTitleList(termId);
        }
        #endregion

        #region 导出写生排课的学生列表
        /// <summary>
        /// 获取写生排课的学生排课导出列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-20</para>
        /// </summary>
        /// <param name="request">写生课学生查询条件</param>
        /// <returns>文件流</returns>
        [HttpGet, Route("GetStudentLessonsExport")]
        public FileResult GetStudentLessonsExport([FromQuery]LifeClassLessonStudentSearchRequest request)
        {
            LifeClassService service = new LifeClassService(base.SchoolId);
            List<LifeClassLessonStudentExportResponse> list = service.GetStudentLessonsExport(request);
            string[] header = new string[] { "学生姓名", "性别", "出生日期", "证件类型", "证件号码", "班级代码" };
            NPOIExcelExport excelExport = new NPOIExcelExport();
            excelExport.Add(list, header);
            return excelExport.DownloadToFile(this, "写生排课");
        }
        #endregion

        #region 获取写生课的班级代码
        /// <summary>
        /// 根据学期Id获取班级代码
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-19</para>
        /// </summary>
        /// <param name="lifeTimeId">写生课Id</param>
        /// <returns>班级信息列表</returns>
        [HttpGet, Route("GetLifeClassList")]
        public List<ClassInfoListResponse> GetLifeClassList(long lifeTimeId)
        {
            return new LifeClassService(base.SchoolId).GetLifeClassList(lifeTimeId);
        }
        #endregion

        #region 写生课排课
        /// <summary>
        /// 学生写生排课
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="lifeClassId">写生课Id</param>
        /// <param name="request">学期班级学生请求对象</param>
        /// <returns>写生排课信息</returns>
        [HttpPost, Route("MakeLifeClassLesson")]
        public LifeClassResultResponse MakeLifeClassLesson(long lifeClassId, [FromBody] List<TimeLessonClassRequest> request)
        {
            return new LifeClassService(base.SchoolId).MakeLesson(lifeClassId, request);
        }

        /// <summary>
        /// 选择班级写生排课
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-02</para>
        /// </summary>
        /// <param name="lifeClassId">写生课Id</param>
        /// <param name="request">写生排课（班级选择）请求对象</param>
        /// <returns>课次不够的学生信息</returns>
        [HttpPost, Route("SelectClassToMake")]
        public List<LifeClassLackTimesListResponse> SelectClassToMake(long lifeClassId, [FromBody] List<LifeClassSelectClassRequest> request)
        {
            return new LifeClassService(base.SchoolId).SelectClassToMake(lifeClassId, request);
        }
        #endregion

        #region 取消写生课
        /// <summary>
        /// 根据写生课Id取消对应的所有学生的写生课次
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-10</para>
        /// </summary>
        /// <param name="lifeClassId">写生课Id</param>
        [HttpPut, Route("Cancel")]
        public void Cancel(long lifeClassId)
        {
            new LifeClassService(base.SchoolId).Cancel(lifeClassId);
        }

        /// <summary>
        /// 根据写生课Id和课次Id列表取消对应的学生写生课次
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-10</para>
        /// </summary>
        /// <param name="lifeClassId">写生课Id</param>
        /// <param name="lessonIdList">课次Id列表</param>
        [HttpPut, Route("CancelByLessonIdList")]
        public void CancelByLessonIdList(long lifeClassId, [FromBody] List<long> lessonIdList)
        {
            new LifeClassService(base.SchoolId).Cancel(lifeClassId, lessonIdList);
        }
        #endregion

        #region 添加写生课
        /// <summary>
        /// 添加写生课
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="request">写生课请求对象</param>
        [HttpPost]
        public void Post(LifeClassAddRequest request)
        {
            new LifeClassService(base.SchoolId).Add(request);
        }
        #endregion

        #region 修改写生课
        /// <summary>
        /// 修改写生课
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="lifeClassId">写生课Id</param>
        /// <param name="request">写生课请求对象</param>
        [HttpPut]
        public void Put(long lifeClassId, LifeClassAddRequest request)
        {
            new LifeClassService(base.SchoolId).Modify(lifeClassId, request);
        }
        #endregion
    }
}
