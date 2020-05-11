using AMS.Anticorrosion.HRS;
using AMS.Core;
using AMS.Dto;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using AutoMapper;
using Jerrisoft.Platform.Public.PageExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Service
{
    /// <summary>
    /// 写生课信息服务
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public class LifeClassService
    {
        private readonly string _schoolId;                                                                                                                                                                                                        //校区Id
        private readonly Lazy<ViewTimLifeClassLessonRepository> _viewTimLifeClassRepository = new Lazy<ViewTimLifeClassLessonRepository>();                         //写生课的视图仓储
        private readonly Lazy<TblTimLifeClassRepository> _tblTimLifeClassRepository = new Lazy<TblTimLifeClassRepository>();                                                         //写生课仓储
        private readonly Lazy<TblTimLessonRepository> _tblTimLessonRepository = new Lazy<TblTimLessonRepository>();                                                                   //课次基础信息仓储
        private readonly Lazy<ViewTimLifeClassStudentRepository> _viewTimLifeClassStudentRepository = new Lazy<ViewTimLifeClassStudentRepository>();           //班级人数仓储 

        /// <summary>
        /// 校区的写生课信息实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        public LifeClassService(string schoolId)
        {
            this._schoolId = schoolId;
        }

        #region GetPagerList 获取写生课列表
        /// <summary>
        /// 获取写生课列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-17</para>
        /// </summary>
        /// <param name="searcher">写生列表查询条件</param>
        /// <returns>写生课信息分页列表</returns>
        public PageResult<LifeClassListResponse> GetPagerList(LifeClassListSearchRequest searcher)
        {
            var result = new PageResult<LifeClassListResponse>() { Data = new List<LifeClassListResponse>() };
            var lifeClassList = _tblTimLifeClassRepository.Value.GetLifeClassListAsync(this._schoolId, searcher);
            if (lifeClassList != null && lifeClassList.Data != null && lifeClassList.Data.Count > 0)
            {
                var teacherInfo = EmployeeService.GetAllBySchoolId(this._schoolId);
                List<long> businessIdList = lifeClassList.Data.Select(a => a.LifeClassId).ToList();
                var studentLessonInfo = _tblTimLessonRepository.Value.GetStudentLessonListAsync(this._schoolId, businessIdList);
                result.Data = lifeClassList.Data.Select(a => new LifeClassListResponse
                {
                    LifeTimeId = a.LifeClassId,
                    Title = a.Title,
                    ClassBeginDate = a.ClassBeginTime.Value,
                    ClassEndTime = a.ClassEndTime.Value,
                    LifeClassCode = a.LifeClassCode,
                    PersonNumber = studentLessonInfo.Count(s => s.BusinessId == a.LifeClassId && s.BusinessType == (int)LessonBusinessType.LifeClassMakeLesson && s.Status == (int)LessonUltimateStatus.Normal),
                    Place = a.Place,
                    TeacherId = a.TeacherId,
                    TeacherName = teacherInfo.FirstOrDefault(t => t.EmployeeId == a.TeacherId)?.EmployeeName,
                    UseLessonCount = a.UseLessonCount
                }).ToList();
                result.CurrentPage = lifeClassList.CurrentPage;
                result.PageSize = lifeClassList.PageSize;
                result.TotalData = lifeClassList.TotalData;
            }
            return result;
        }
        #endregion

        #region GetPagerLessonList 获取写生课的学生排课列表
        /// <summary>
        /// 获取写生课的学生排课列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-17</para>
        /// </summary>
        /// <param name="searcher">学生排课查询条件</param>
        /// <returns>写生课学生分页列表</returns>
        public PageResult<LifeClassLessonListResponse> GetPagerLessonList(LifeClassLessonListSearchRequest searcher)
        {
            var result = new PageResult<LifeClassLessonListResponse>() { Data = new List<LifeClassLessonListResponse>() };
            var lifeClassList = _viewTimLifeClassRepository.Value.GetStudentListOfLifeClass(this._schoolId, searcher);
            if (lifeClassList != null && lifeClassList.Data != null && lifeClassList.Data.Count > 0)
            {
                result.Data = Mapper.Map<List<LifeClassLessonListResponse>>(lifeClassList.Data);
                result.Data.ForEach(a => a.ContactPersonMobile = a.ContactPersonMobile.Split(",")?[0]);
                result.CurrentPage = lifeClassList.CurrentPage;
                result.PageSize = lifeClassList.PageSize;
                result.TotalData = lifeClassList.TotalData;
            }
            return result;
        }
        #endregion

        #region GetLifeClassStudentList 获取班级的人数信息
        /// <summary>
        /// 获取班级的人数信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="request">写生课学生请求对象</param>
        /// <returns>班级学生人数列表</returns>
        public List<TimeClassStudentResponse> GetLifeClassStudentList(LifeClassStudentRequest request)
        {
            var lifeClassStudentLis = _viewTimLifeClassStudentRepository.Value.GetLifeClassStudentListAsync(this._schoolId, request).Result;
            var result = new List<TimeClassStudentResponse>();
            if (lifeClassStudentLis != null && lifeClassStudentLis.Count > 0)
            {
                result = Mapper.Map<List<TimeClassStudentResponse>>(lifeClassStudentLis);
            }
            return result;
        }
        #endregion

        #region GetLifeClassTitleList 获取写生课主题列表
        /// <summary>
        /// 根据学期Id获取写生课列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-20</para>
        /// </summary>
        /// <param name="termId">学期Id</param>
        /// <returns>学期写生课列表</returns>
        public List<LifeClassTitleListResponse> GetLifeClassTitleList(long termId)
        {
            var lifeClassList = _tblTimLifeClassRepository.Value.GetLifeClassListByTermId(new List<long>() { termId }).Result;
            var result = new List<LifeClassTitleListResponse>();
            if (lifeClassList != null && lifeClassList.Count > 0)
            {
                result = lifeClassList.Select(a => new LifeClassTitleListResponse()
                {
                    LifeTimeId = a.LifeClassId,
                    Title = a.Title + "(" + a.LifeClassCode + ")"
                }).ToList();
            }
            return result;
        }

        /// <summary>
        /// 根据写生课Id集合获取写生课信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <param name="lifeClassIdList">写生课Id集合</param>
        /// <returns>校区写生排课列表</returns>
        internal async Task<List<TblTimLifeClass>> GetLifeClassListById(List<long> lifeClassIdList)
        {
            return await _tblTimLifeClassRepository.Value.GetLifeClassListAsync(lifeClassIdList);
        }
        #endregion

        #region GetStudentLessonsExport 获取写生排课的学生排课导出列表
        /// <summary>
        /// 获取写生排课的学生排课导出列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <param name="request">写生课学生查询条件</param>
        /// <returns>写生课的学生排课数据导出列表</returns>
        public List<LifeClassLessonStudentExportResponse> GetStudentLessonsExport(LifeClassLessonStudentSearchRequest request)
        {
            List<LifeClassLessonStudentExportResponse> result = new List<LifeClassLessonStudentExportResponse>();
            var lifeClassList = _viewTimLifeClassRepository.Value.GetLifeClassStudentList(this._schoolId, request);
            if (lifeClassList != null && lifeClassList.Count > 0)
            {
                result = Mapper.Map<List<LifeClassLessonStudentExportResponse>>(lifeClassList);
            }
            return result;
        }
        #endregion

        #region 获取写生课的班级代码

        /// <summary>
        /// 获取写生课的班级代码
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-22</para>
        /// </summary>
        /// <param name="lifeTimeId">写生课Id</param>
        /// <returns>班级信息列表</returns>
        public List<ClassInfoListResponse> GetLifeClassList(long lifeTimeId)
        {
            var result = new List<ClassInfoListResponse>();
            //获取学期ID对应的写生课列表信息
            var lifeClassList = _viewTimLifeClassRepository.Value.GetLifeClassStudentList(this._schoolId, new LifeClassLessonStudentSearchRequest() { LifeTimeId = lifeTimeId });
            if (lifeClassList != null && lifeClassList.Count > 0)
            {
                result = lifeClassList.GroupBy(g => new { g.ClassId, g.ClassNo }).Select(a => new ClassInfoListResponse()
                {
                    ClassId = a.Key.ClassId,
                    ClassNo = a.Key.ClassNo
                }).ToList();
            }
            return result;
        }

        #endregion

        #region Add 添加一个写生课
        /// <summary>
        /// 添加一个写生课
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-22</para>
        /// </summary>
        /// <param name="request">写生课实体信息</param>
        public void Add(LifeClassAddRequest request)
        {
            //1、数据校验
            Verification(request);

            //2、准备数据
            TblTimLifeClass entity = new TblTimLifeClass
            {
                LifeClassId = IdGenerator.NextId(),
                SchoolId = this._schoolId,
                TermId = request.TermId.Value,
                Title = request.Title,
                Place = request.Place,
                ClassBeginTime = request.ClassBeginTime,
                ClassEndTime = request.ClassEndTime,
                LifeClassCode = _tblTimLifeClassRepository.Value.GenerateLifeClassCode(request.ClassBeginTime.Value),
                TeacherId = request.TeacherId,
                UseLessonCount = request.UseLessonCount.Value,
                CreateTime = DateTime.Now
            };

            //3、写入数据库
            _tblTimLifeClassRepository.Value.Add(entity);
        }

        /// <summary>
        /// 数据校验
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-10</para>
        /// </summary>
        /// <param name="request">写生课实体信息</param>
        /// <param name="source">对象为NULL表示新增，反之表示修改</param>
        /// <exception cref="AMS.Core.BusinessException">
        /// 异常ID：
        /// 20. 消耗课次未选择，不可保存
        /// 21. 上课开始时间不能小于今天，不可保存
        /// 22. 上课结束日期不能小于上课开始日期，不可保存
        /// 24. 学期不可修改
        /// 25. 消耗课次不可修改
        /// </exception>
        private void Verification(LifeClassAddRequest request, TblTimLifeClass source = null)
        {
            //写生课修改的数据检查
            if (source == null)
            {
                //检查耗用课次
                CheckData(request, x => (!x.UseLessonCount.HasValue || x.UseLessonCount == 0), 20);
                //检查上课开始时间不能小于今天
                CheckData(request, x => x.ClassBeginTime.HasValue && x.ClassBeginTime.Value < DateTime.Now, 21);
                //检查上课结束日期不能小于上课开始日期
                CheckData(request, x => x.ClassEndTime.HasValue && x.ClassEndTime.Value < x.ClassBeginTime.Value, 22);
            }
            else
            {
                //学期不允许修改
                CheckData(request, x => x.TermId != source.TermId, 24);
                //消耗课次不允许修改
                CheckData(request, x => x.UseLessonCount != source.UseLessonCount, 25);
                //如果改变上课开始时间，检查上课开始时间不能小于今天
                CheckData(request, x => x.ClassBeginTime != source.ClassBeginTime && x.ClassBeginTime.Value < DateTime.Now, 21);
                //如果改变上课结束时间，检查上课结束日期不能小于上课开始日期
                CheckData(request, x => x.ClassEndTime != source.ClassEndTime && x.ClassEndTime.Value < x.ClassBeginTime.Value, 22);
            }
        }

        /// <summary>
        /// 数据校验
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-10</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="where">表达式</param>
        /// <param name="exceptionId">异常Id</param>
        /// <exception cref="AMS.Core.BusinessException">
        /// 异常ID：exceptionId
        /// </exception>
        private void CheckData(LifeClassAddRequest source, Func<LifeClassAddRequest, bool> where, ushort exceptionId)
        {
            if (source == null) return;
            if (where(source))
            {
                throw new BussinessException((byte)ModelType.Timetable, exceptionId);
            }
        }
        #endregion

        #region Modify 修改一个写生课
        /// <summary>
        /// 修改一个写生课
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-10</para>
        /// </summary>
        /// <param name="lifeClassId">写生课Id</param>
        /// <param name="request">写生课实体信息</param>
        public void Modify(long lifeClassId, LifeClassAddRequest request)
        {
            TblTimLifeClass entity = _tblTimLifeClassRepository.Value.GetLifeClassInfo(lifeClassId);
            //1、数据校验
            Verification(request, entity);

            //2、准备数据            
            entity.Title = request.Title;
            entity.Place = request.Place;
            entity.ClassBeginTime = request.ClassBeginTime;
            entity.ClassEndTime = request.ClassEndTime;
            entity.TeacherId = request.TeacherId;

            //3、写入数据库
            _tblTimLifeClassRepository.Value.Update(entity);
        }
        #endregion

        #region MakeLesson 写生排课
        /// <summary>
        /// 以班级方式进行排课
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="lifeClassId">写生课Id</param>
        /// <param name="request">写生课实体信息</param>
        /// <returns>写生排课结果</returns>
        public LifeClassResultResponse MakeLesson(long lifeClassId, List<TimeLessonClassRequest> request)
        {
            return new LifeClassMakeLessonService(this._schoolId, lifeClassId).Make(request);
        }

        /// <summary>
        /// 以班级方式进行排课
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="lifeClassId">写生课Id</param>
        /// <param name="request">请求参数</param>
        /// <returns>学生课次不够的课程信息列表</returns>
        public List<LifeClassLackTimesListResponse> SelectClassToMake(long lifeClassId, List<LifeClassSelectClassRequest> request)
        {
            return new LifeClassMakeLessonService(this._schoolId, lifeClassId).SelectClassToMake(request);
        }
        #endregion

        #region Cancel 取消写生课
        /// <summary>
        /// 取消写生排课
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="lifeClassId">写生课Id</param>
        public void Cancel(long lifeClassId)
        {
            new LifeClassMakeLessonService(this._schoolId, lifeClassId).Cancel();
        }

        /// <summary>
        /// 取消学生的某个课次
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="lifeClassId">写生课Id</param>
        /// <param name="lessonId">课程Id</param>
        public void Cancel(long lifeClassId, List<long> lessonId)
        {
            new LifeClassMakeLessonService(this._schoolId, lifeClassId).Cancel(lessonId);
        }
        #endregion

        #region Change 写生调课
        /// <summary>
        /// 写生调课
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="lifeClassId">写生课Id</param>
        /// <param name="lessonId">课程Id</param>
        /// <param name="classId">班级Id</param>
        public void Change(long lifeClassId, long lessonId, long classId)
        {
            LifeClassMakeLessonService service = new LifeClassMakeLessonService(this._schoolId, lifeClassId);
            service.Change(lessonId, classId);
        }
        #endregion

    }
}
