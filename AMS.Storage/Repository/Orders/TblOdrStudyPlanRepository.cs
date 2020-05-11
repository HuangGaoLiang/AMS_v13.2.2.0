/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AMS.Storage.Context;
using AMS.Storage.Models;
using AMS.Core;
using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using AMS.Dto;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描    述：学习计划课程仓储
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-02</para>
    /// </summary>
    public class TblOdrStudyPlanRepository : BaseRepository<TblOdrStudyPlan>
    {
        /// <summary>
        /// 学习计划仓储实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        public TblOdrStudyPlanRepository()
        {
        }

        /// <summary>
        /// 学习计划仓储实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        public TblOdrStudyPlanRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 根据校区Id集合获取对应的课程级别相关信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="schoolIdList">校区Id集合</param>
        /// <returns>报名学习计划列表</returns>
        public List<TblOdrStudyPlan> GetStudyPlanList(List<string> schoolIdList)
        {
            List<TblOdrStudyPlan> studyPlanList = new List<TblOdrStudyPlan>();
            var result = from a in CurrentContext.TblDatSchoolCourse
                         join b in CurrentContext.TblDatCourse on a.CourseId equals b.CourseId
                         join c in CurrentContext.TblDatCourseLevelMiddle on a.CourseId equals c.CourseId
                         join d in CurrentContext.TblDatCourseLevel on c.CourseLevelId equals d.CourseLevelId
                         where !d.IsDisabled && schoolIdList.Contains(a.SchoolId) && b.CourseType == (int)CourseType.Compulsory
                         select new
                         {
                             a.SchoolId,
                             a.CourseId,
                             b.ShortName,
                             d.CourseLevelId,
                             d.LevelCnName,
                             c.BeginAge,
                             c.EndAge,
                             c.Duration
                         };
            DateTime currTime = DateTime.Now;
            if (result.Any())
            {
                int priorityLevel;
                foreach (var schoolId in schoolIdList)
                {
                    priorityLevel = 0;
                    result.Where(a => a.SchoolId == schoolId).AsQueryable().OrderBy(o => o.BeginAge).ToList().ForEach(a =>
                    {
                        for (int i = a.BeginAge; i <= a.EndAge; i++)
                        {
                            studyPlanList.Add(new TblOdrStudyPlan()
                            {
                                StudyPlanId = IdGenerator.NextId(),
                                SchoolId = a.SchoolId,
                                CourseId = a.CourseId,
                                CourseLevelId = a.CourseLevelId,
                                CourseName = a.ShortName,
                                CourseLevelName = a.LevelCnName,
                                Age = i,
                                Duration = a.Duration,
                                PriorityLevel = priorityLevel,
                                CreateTime = currTime
                            });
                            priorityLevel++;
                        }
                    });
                }
            }
            return studyPlanList;
        }

        /// <summary>
        /// 根据校区Id获取校区课程信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-05</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <returns>报名学习计划列表</returns>
        public async Task<List<TblOdrStudyPlan>> GetList(string schoolId)
        {
            return await LoadLisTask(a => a.SchoolId == schoolId);
        }

        /// <summary>
        /// 根据校区Id、课程Id和课程级别Id获取校区课程信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-05</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="courseId">课程Id</param>
        /// <param name="courseLevelId">课程级别Id</param>
        /// <returns>报名学习计划列表</returns>
        public async Task<List<TblOdrStudyPlan>> GetListAsync(string schoolId, long courseId, long courseLevelId)
        {
            return await LoadLisTask(a => a.SchoolId == schoolId && a.CourseId == courseId && a.CourseLevelId == courseLevelId);
        }

        /// <summary>
        /// 添加校区课程级别信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-05</para>
        /// </summary>
        /// <param name="studyPlanList">报名学习计划列表</param>
        public async Task AddStudyPlansAsync(List<TblOdrStudyPlan> studyPlanList)
        {
            await this.SaveTask<TblOdrStudyPlan>(studyPlanList);
        }

        /// <summary>
        /// 根据课程Id更新学习计划的课程名称
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-12-04</para>
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <param name="courseName">课程名称</param>
        public async Task UpdateCourseNameAsync(long courseId, string courseName)
        {
            await UpdateTask(s => s.CourseId == courseId, w => new TblOdrStudyPlan() { CourseName = courseName });
        }

        /// <summary>
        /// 根据课程级别Id更新学习计划的课程级别名称
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-12-04</para>
        /// </summary>
        /// <param name="courseLevelId">课程级别Id</param>
        /// <param name="courseLevelName">课程级别名称</param>
        public async Task UpdateCourseLevelNameAsync(long courseLevelId, string courseLevelName)
        {
            await UpdateTask(s => s.CourseLevelId == courseLevelId, w => new TblOdrStudyPlan() { CourseLevelName = courseLevelName });
        }

        /// <summary>
        /// 批量删除校区课程信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="schoolIds">校区Id集合</param>
        public async Task DeleteStudyPlanAsync(List<string> schoolIds)
        {
            Expression<Func<TblOdrStudyPlan, bool>> where = x => schoolIds.Contains(x.SchoolId);
            await base.DeleteTask(where, where);
        }
    }
}
