/*此代码由生成工具字段生成，生成时间2019/3/6 16:11:58 */
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AMS.Storage.Context;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;
using System;
using AMS.Dto;
using Jerrisoft.Platform.Storage;
using System.Linq.Expressions;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblTimAdjustLesson仓储
    /// </summary>
    public class TblTimAdjustLessonRepository : BaseRepository<TblTimAdjustLesson>
    {
        /// <summary>
        /// TblTimAdjustLesson 无参构造函数
        /// </summary>
        public TblTimAdjustLessonRepository()
        {

        }
        /// <summary>
        /// TblTimAdjustLesson 带上下文构造函数
        /// </summary>
        /// <param name="context">上下文</param>
        public TblTimAdjustLessonRepository(DbContext context) : base(context)
        {

        }

        /// <summary>
        /// 根据条件查询补课周补课信息
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-11-22</para>
        /// </summary>
        /// <param name="adjustLessonIds">补课记录编号</param>
        /// <returns>返回补课周补课信息</returns>
        public List<TblTimAdjustLesson> GetTimAdjustLessonList(List<long> adjustLessonIds)
        {
            var list = LoadList(m => adjustLessonIds.Contains(m.AdjustLessonId));

            return list;
        }


        /// <summary>
        /// 根据条件查询补课周补课信息
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-11-22</para>
        /// </summary>
        /// <param name="search">补课周补课信息查询参数</param>
        /// <returns>返回补课周补课信息</returns>
        public List<TblTimAdjustLesson> GetTimAdjustLessonList(TimAdjustLessonInDto search)
        {
            var list = LoadList(m => m.SchoolId == search.SchoolId
                                            && m.FromTeacherId == search.ToTeacherId
                                            && m.BusinessType == (int)search.BusinessType)
                                            .WhereIf(search.ClassId > 0, m => m.ClassId == search.ClassId)
                                            .WhereIf(search.Status.HasValue, m => m.Status == (int)search.Status)
                                            .ToList();

            return list;
        }


        /// <summary>
        /// 删除补课周补课记录
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-11-22</para>
        /// </summary>
        /// <param name="adjustLessonIds">主键编号</param>
        /// <param name="status">状态</param>
        /// <param name="unitOfWork">事务</param>
        public void UpdateRoySalesPayLetterId(List<long> adjustLessonIds, int status, UnitOfWork unitOfWork)
        {
            Expression<Func<TblTimAdjustLesson, TblTimAdjustLesson>> updateLambda = t => new TblTimAdjustLesson
            {
                Status = status
            };
            unitOfWork.GetCustomRepository<TblTimAdjustLessonRepository, TblTimAdjustLesson>().Update(m => adjustLessonIds.Contains(m.AdjustLessonId), updateLambda);
        }
    }
}
