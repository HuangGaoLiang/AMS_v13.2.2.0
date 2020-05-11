using AMS.Dto;
using AMS.Storage.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描    述：学期班级学生课次信息仓储
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-13</para>
    /// </summary>
    public class ViewTimeLessonClassRepository : BaseRepository<ViewTimeLessonClass>
    {
        /// <summary>
        /// 获取写生课的学生列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-13</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="request">学期班级学生实体请求条件</param>
        /// <returns>学期班级学生实体列表</returns>
        public async Task<List<ViewTimeLessonClass>> GetTimeLessonClassList(string schoolId, List<TimeLessonClassRequest> request)
        {
            var result = from a in CurrentContext.TblTimLesson
                         join b in CurrentContext.TblTimLessonStudent on a.LessonId equals b.LessonId
                         where a.SchoolId == schoolId && request.Select(x => x.TermId).Contains(a.TermId)
                         && request.Select(y => y.ClassId).Contains(a.ClassId)
                         select new ViewTimeLessonClass()
                         {
                             SchoolId = a.SchoolId,
                             StudentId = a.StudentId,
                             EnrollOrderItemId = a.EnrollOrderItemId,
                             TermId = a.TermId,
                             ClassId = a.ClassId
                         };
            return await result.Distinct().ToListAsync();
        }
    }
}
