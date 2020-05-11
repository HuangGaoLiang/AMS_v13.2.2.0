using AMS.Dto;
using AMS.Storage.Models;
using Jerrisoft.Platform.Public.PageExtensions;
using Jerrisoft.Platform.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描    述：写生课学生列表相关业务逻辑类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-09</para>
    /// </summary>
    public class ViewTimLifeClassLessonRepository : BaseRepository<ViewTimLifeClass>
    {
        /// <summary>
        /// 获取写生课的学生列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-09</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="request">请求参数</param>
        /// <returns>写生课的学生分页列表</returns>
        public PageResult<ViewTimLifeClass> GetStudentListOfLifeClass(string schoolId, LifeClassLessonListSearchRequest request)
        {
            var result = GetTimLifeClassResult(schoolId, request.LifeTimeId, request.ClassId, request.Keyword);
            return result.OrderBy(a => a.StudentName).ToPagerSource(request.PageIndex, request.PageSize);
        }

        /// <summary>
        /// 获取写生课的学生列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-21</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="request">写生课学生查询条件</param>
        /// <returns>写生课的学生列表</returns>
        public List<ViewTimLifeClass> GetLifeClassStudentList(string schoolId, LifeClassLessonStudentSearchRequest request)
        {
            var result = GetTimLifeClassResult(schoolId, request.LifeTimeId, request.ClassId, request.Keyword);
            return result.ToList();
        }

        /// <summary>
        /// 根据校区Id、写生课Id、班级Id和学生名称/手机号获取写生课信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-12-29</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="lifeClassId">写生课Id</param>
        /// <param name="classId">班级Id</param>
        /// <param name="keyword">学生名称/手机号</param>
        /// <returns>写生课的学生列表</returns>
        private IQueryable<ViewTimLifeClass> GetTimLifeClassResult(string schoolId, long? lifeClassId, long? classId, string keyword)
        {
            return from a in CurrentContext.TblTimLesson
                   join b in CurrentContext.TblCstStudent on a.StudentId equals b.StudentId
                   join c in CurrentContext.TblDatClass on a.ClassId equals c.ClassId into c_join
                   from c in c_join.DefaultIfEmpty()
                   let mobile = (b.ContactPersonMobile.IndexOf(",") >= 0 ? b.ContactPersonMobile.Remove(b.ContactPersonMobile.IndexOf(",")) : b.ContactPersonMobile)
                   where a.SchoolId == schoolId
                   && (!lifeClassId.HasValue || a.BusinessId == lifeClassId)
                   && a.Status == (int)LessonUltimateStatus.Normal
                   && (!classId.HasValue || a.ClassId == classId)//班级Id可为空
                   && (string.IsNullOrEmpty(keyword) || (b.StudentName.Contains(keyword) || mobile.Contains(keyword)))//关键字可为空
                   select new ViewTimLifeClass()
                   {
                       BusinessId = a.BusinessId,
                       BusinessType = a.BusinessType,
                       ClassId = a.ClassId,
                       ClassRoomId = a.ClassRoomId,
                       CreateTime = a.CreateTime,
                       EnrollOrderItemId = a.EnrollOrderItemId,
                       LessonCount = a.LessonCount,
                       LessonId = a.LessonId,
                       LessonType = a.LessonType,
                       SchoolId = a.SchoolId,
                       Status = a.Status,
                       StudentId = a.StudentId,
                       StudentName = b.StudentName,
                       Sex = b.Sex,
                       Birthday = b.Birthday,
                       TeacherId = a.TeacherId,
                       TermId = a.TermId,
                       ClassNo = c.ClassNo,
                       IDNumber = b.IDNumber,
                       IDType = b.IDType,
                       ContactPersonMobile = b.ContactPersonMobile
                   };
        }
    }
}
