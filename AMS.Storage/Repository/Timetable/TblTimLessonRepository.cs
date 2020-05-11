/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AMS.Core;
using AMS.Dto;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblTimLesson仓储
    /// </summary>
    public class TblTimLessonRepository : BaseRepository<TblTimLesson>
    {


        public TblTimLessonRepository(DbContext context) : base(context)
        {

        }
        public TblTimLessonRepository()
        {

        }

        /// <summary>
        /// 统计班级排课数
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <returns>班级排课数</returns>
        public int TotalClassScheduleNum(long classId)
        {
            int queryableCount = base.LoadQueryable(
                x => x.ClassId == classId &&
                x.Status == (int)LessonUltimateStatus.Normal, false)
                .Select(x => x.StudentId)
                .Distinct()
                .Count();

            return queryableCount;
        }

        /// <summary>
        /// 描述：根据班级Id集合获取课次信息集合
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-10</para>
        /// </summary>
        /// <param name="classIds">班级Id集合</param>
        /// <param name="classDateList">班级上课日期集合</param>
        /// <param name="lessonUltimateStatus">课次最终状态</param>
        /// <returns>课次信息集合</returns>
        public List<TblTimLesson> GetTimLessonByClassIdList(IEnumerable<long> classIds, IEnumerable<DateTime> classDateList, LessonUltimateStatus lessonUltimateStatus)
        {
            var result = base.LoadList(x => classIds.Contains(x.ClassId) && classDateList.Contains(x.ClassDate) && x.Status == (int)lessonUltimateStatus);
            return result;
        }

        /// <summary>
        /// 根据LessonId获取
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-05</para>
        /// </summary>
        /// <param name="lessonId">一组课次Id</param>
        /// <returns>课次基础信息列表</returns>
        public async Task<List<TblTimLesson>> GetByLessonIdTask(IEnumerable<long> lessonId)
        {
            return await base.LoadLisTask(x => lessonId.Contains(x.LessonId));
        }

        /// <summary>
        /// 根据LessonId获取
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-05</para>
        /// </summary>
        /// <param name="lessonId">一组课次Id</param>
        /// <returns>课次基础信息列表</returns>
        public List<TblTimLesson> GetByLessonId(IEnumerable<long> lessonId)
        {
            return base.LoadList(x => lessonId.Contains(x.LessonId));
        }

        /// <summary>
        /// 根据校区Id、学生Id和学期Id集合，获取学生课程基础信息列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        /// <param name="termIdList">学期Id集合</param>
        /// <returns>学生课程基础信息列表</returns>
        public List<TblTimLesson> GetLessonListByTermId(string schoolId, long studentId, List<long> termIdList)
        {
            return base.LoadList(x => x.SchoolId == schoolId
                                            && x.StudentId == studentId
                                            && termIdList.Contains(x.TermId)
                                            && x.Status == (int)LessonUltimateStatus.Normal);
        }

        /// <summary>
        /// 根据报名课程明细Id获取
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-08</para>
        /// </summary>
        /// <param name="enrollOrderItemId">报名课程明细Id</param>
        /// <returns>课次基础信息列表</returns>
        public async Task<List<TblTimLesson>> GetByEnrollOrderItemIdAsync(long enrollOrderItemId)
        {
            return await base.LoadLisTask(x => x.EnrollOrderItemId == enrollOrderItemId);
        }

        /// <summary>
        /// 根据报名课程明细Id获取
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-07</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        /// <param name="enrollOrderItemId">报名课程明细Id</param>
        /// <returns>课次基础信息列表</returns>
        public async Task<List<TblTimLesson>> GetByEnrollOrderItemIdAsync(long enrollOrderItemId, string schoolId, long studentId)
        {
            return await base.LoadLisTask(
                x => x.SchoolId == schoolId &&
                     x.StudentId == studentId &&
                     x.EnrollOrderItemId == enrollOrderItemId &&
                     x.Status == (int)LessonUltimateStatus.Normal);
        }

        /// <summary>
        /// 根据学生Id获取
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        /// <returns>课次基础信息列表</returns>
        public async Task<List<TblTimLesson>> GetByStudentIdAsync(string schoolId, long studentId)
        {
            return await base.LoadLisTask(x => x.SchoolId == schoolId
                                            && x.StudentId == studentId
                                            && x.Status == (int)LessonUltimateStatus.Normal);
        }

        /// <summary>
        /// 在日期时间之后课次
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-26</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        /// <param name="time">时间</param>
        /// <param name="classId">班级Id</param>
        /// <returns>课次基础信息列表</returns>
        private IQueryable<TblTimLesson> GetByAfterDateTimeQueryable(string schoolId, long studentId, DateTime time, long? classId = null)
        {
            var query = base.LoadQueryable(
                x => x.SchoolId == schoolId &&
                     x.StudentId == studentId &&
                     x.Status == (int)LessonUltimateStatus.Normal &&
                     x.ClassDate >= time, false);

            if (classId.HasValue)
            {
                query = query.Where(x => x.ClassId == classId.Value);
            }

            return query;
        }

        ///// <summary>
        ///// 获取转出班级最大课次数
        ///// <para>作    者：zhiwei.Tang</para>
        ///// <para>创建时间：2018-11-06</para>
        ///// </summary>
        ///// <param name="schoolId">校区Id</param>
        ///// <param name="studentId">学生Id</param>
        ///// <param name="classId">班级Id</param>
        ///// <param name="stopTime">停课日期</param>
        ///// <returns>转出班级最大课次数</returns>
        //public int GetRollOutClassMaxLessons(string schoolId, long studentId, long classId, DateTime stopTime)
        //{
        //    var querey = this.GetByAfterDateTimeQueryable(schoolId, studentId, stopTime, classId).Where(x => x.LessonType == (int)LessonType.RegularCourse);

        //    return querey.Sum(x => x.LessonCount);
        //}

        /// <summary>
        /// 获取转出班级最大课次
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        /// <param name="classId">班级Id</param>
        /// <param name="stopTime">停课日期</param>
        /// <returns>课次基础信息列表</returns>
        public List<TblTimLesson> GetRollOutClassLessons(string schoolId, long studentId, long classId, DateTime stopTime)
        {
            return base.LoadList(m => m.SchoolId == schoolId && m.StudentId == studentId && m.ClassId == classId && m.ClassDate == stopTime);
        }

        ///// <summary>
        ///// 获取休学课次
        ///// <para>作    者：zhiwei.Tang</para>
        ///// <para>创建时间：2018-11-01</para>
        ///// </summary>
        ///// <param name="schoolId">校区Id</param>
        ///// <param name="studentId">学期Id</param>
        ///// <param name="leaveTime">休学时间</param>
        ///// <returns>休学课次列表</returns>
        //public List<LeaveSchoolLessonResponse> GetLeaveSchoolLesson(string schoolId, long studentId, DateTime leaveTime)
        //{
        //    var queryable = this.GetByAfterDateTimeQueryable(schoolId, studentId, leaveTime);

        //    var res = queryable
        //        .GroupBy(g => g.EnrollOrderItemId)
        //        .Select(m => new LeaveSchoolLessonResponse
        //        {
        //            EnrollOrderItemId = m.Key,
        //            Count = m.Sum(x => x.LessonCount)
        //        })
        //        .ToList();

        //    return res;
        //}

        ///// <summary>
        ///// 获取休学课次
        ///// <para>作    者：zhiwei.Tang</para>
        ///// <para>创建时间：2018-11-08</para>
        ///// </summary>
        ///// <param name="schoolId">校区Id</param>
        ///// <param name="studentId">学生Id</param>
        ///// <param name="leaveTime">休学时间</param>
        ///// <returns>课次基础信息列表</returns>
        //public List<TblTimLesson> GetLeaveSchoolLessonsList(string schoolId, long studentId, DateTime leaveTime)
        //{
        //    var queryable = this.GetByAfterDateTimeQueryable(schoolId, studentId, leaveTime);

        //    return queryable.ToList();
        //}

        /// <summary>
        /// 获取报名课程明细下的正常课次信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-01-17</para>
        /// </summary>
        /// <param name="enrollOrderItemIds">一组报名课程明细Id</param>
        /// <returns>课次基础信息列表</returns>
        public List<TblTimLesson> GetLessonsListByEnrollOrderItemId(List<long> enrollOrderItemIds)
        {
            var res = base.LoadList(
                x =>
                enrollOrderItemIds.Contains(x.EnrollOrderItemId) &&
                x.Status == (int)LessonUltimateStatus.Normal);

            return res;
        }


        /// <summary>
        /// 根据获取课次基础信息数据
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-10</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="businessIdList">业务Id列表</param>
        /// <returns>课次基础信息列表</returns>
        public List<TblTimLesson> GetStudentLessonListAsync(string schoolId, List<long> businessIdList)
        {
            Expression<Func<TblTimLesson, bool>> where = (x => x.SchoolId == schoolId);
            if (businessIdList != null && businessIdList.Count > 0)
            {
                where = where.And(x => businessIdList.Contains(x.BusinessId)
                    && x.BusinessType == (int)LessonBusinessType.LifeClassMakeLesson
                    && x.Status == (int)LessonUltimateStatus.Normal);
            }
            return base.LoadList(where);
        }

        /// <summary>
        /// 根据lessonId批量更新课次终极状态
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-24</para>
        /// </summary>
        /// <param name="lessonIds">一组课次Id</param>
        /// <param name="lessonUltimateStatus">课次终极状态</param>
        /// <returns>true:成功  false:失败</returns>
        public void UpdateLessonStatus(IEnumerable<long> lessonIds, LessonUltimateStatus lessonUltimateStatus)
        {
            base.Update(
              x => lessonIds.Contains(x.LessonId),
              m => new TblTimLesson
              {
                  Status = (int)LessonUltimateStatus.Invalid,
                  UpdateTime = DateTime.Now
              });
        }

        /// <summary>
        /// 更新班级上课老师
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-21</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <param name="teacherId">老师Id</param>
        /// <param name="timeLater">这个时间节点之后的排课数据</param>
        /// <param name="lessonUltimateStatusList">课次的终极状态</param>
        public void UpdateClassTeacher(
            long classId, string teacherId, DateTime timeLater, List<int> lessonUltimateStatusList)
        {
            Expression<Func<TblTimLesson, bool>> sourceLambda =
                x =>
                x.ClassId == classId &&
                lessonUltimateStatusList.Contains(x.Status) &&
                x.ClassDate.AddDays(1) > timeLater;

            Expression<Func<TblTimLesson, TblTimLesson>> whereLambda =
                x => new TblTimLesson
                {
                    TeacherId = teacherId,
                    UpdateTime = DateTime.Now
                };

            base.Update(sourceLambda, whereLambda);
        }

        ///// <summary>
        ///// 更新课次班级上课老师
        ///// <para>作    者：zhiwei.Tang</para>
        ///// <para>创建时间：2018-12-04</para>
        ///// </summary>
        ///// <param name="classId">班级Id</param>
        ///// <param name="teacherId">老师Id</param>
        ///// <param name="time">此时间段之后</param>
        ///// <returns>true:成功  false:失败</returns>
        //public bool UpdateClassTeacher(long classId, string teacherId, DateTime time)
        //{
        //    Expression<Func<TblTimLesson, bool>> sourceLambda =
        //        x =>
        //        x.ClassId == classId &&
        //        x.Status == (int)LessonUltimateStatus.Normal &&
        //        x.ClassDate.AddDays(1) > time;


        //    return base.Update(sourceLambda,
        //       m => new TblTimLesson
        //       {
        //           TeacherId = teacherId,
        //           UpdateTime = DateTime.Now
        //       });
        //}

        /// <summary>
        /// 获取一个校区学生上课时间
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        /// <returns>学生上课时间列表</returns>
        public List<StudentTimetableDto> GetStudentTimetable(string schoolId, long studentId)
        {
            return base.LoadQueryable(
                 x =>
                 x.SchoolId == schoolId &&
                 x.StudentId == studentId &&
                 x.LessonType == (int)LessonType.RegularCourse &&
                 x.Status == (int)LessonUltimateStatus.Normal, false)
             .Select(m => new StudentTimetableDto
             {
                 ClassId = m.ClassId,
                 ClassBeginTime = DateTime.Parse($"{m.ClassDate.ToString("yyyy-MM-dd")} {m.ClassBeginTime}"),
                 ClassEndTime = DateTime.Parse($"{m.ClassDate.ToString("yyyy-MM-dd")} {m.ClassEndTime}")
             }).ToList();
        }
    }
}
