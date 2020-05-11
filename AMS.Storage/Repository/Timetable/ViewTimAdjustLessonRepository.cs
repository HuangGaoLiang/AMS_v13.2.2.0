using AMS.Dto;
using AMS.Storage.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AMS.Models;
using System.Data.SqlClient;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描    述：补课周补课记录仓储
    /// <para>作    者：HuangGaoLiang</para>
    /// <para>创建时间：2019-03-22</para>
    /// </summary>
    public class ViewTimAdjustLessonRepository : BaseRepository<ViewTimAdjustLesson>
    {
        /// <summary>
        /// 获取补课周补课记录
        /// <para>作    者：HuangGaoLiang</para>
        /// <para>创建时间：2019-03-22</para>
        /// </summary>
        /// <param name="classIds">课次编号</param>
        /// <returns>返回补课周补课记录</returns>
        public List<ViewTimAdjustLesson> GetTimeLessonClassList(List<long> classIds)
        {
            #region querySql
            string querySql = @"SELECT a.AdjustLessonId,
                                       t.ClassId,
                                       t.StudentId,
                                       t.ClassRoomId,
                                       t.LessonId,
                                       a.ClassDate,
                                       a.ClassBeginTime,
                                       a.ClassEndTime
                                FROM TblTimLesson t
                                    LEFT JOIN TblTimAdjustLesson a ON t.LessonId = a.FromLessonId
                                WHERE t.Status = @Status
                                      AND a.BusinessType = @BusinessType
                                      AND a.Status = @Status";
            #endregion
            var result = base.CurrentContext.ViewTimAdjustLesson.FromSql(querySql, new SqlParameter[] {
                new SqlParameter("@BusinessType",(int)LessonBusinessType.AdjustLessonReplenishWeek),
                new SqlParameter("@Status",(int)LessonUltimateStatus.Normal)
            }).Where(m => classIds.Contains(m.ClassId))
             .OrderBy(m => m.ClassDate)
             .ThenBy(m => m.ClassBeginTime)
             .ToList();
            return result;
        }
    }
}
