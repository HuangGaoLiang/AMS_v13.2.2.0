using System;
using System.Collections.Generic;
using System.Linq;
using AMS.Core;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;

namespace AMS.Service
{
    /// <summary>
    /// 写生排课产生者
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public class LifeClassLessonCreator : LifeClassLessonProvider, ILessonCreator
    {
        private readonly List<LifeClassLessonMakeRequest> _data;                //写生课排课列表

        /// <summary>
        /// 写生排课产生者实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <param name="entity">写生课信息</param>
        /// <param name="creatorData">要创建的课次相关信息</param>
        /// <param name="unitOfWork">事务单元</param>
        public LifeClassLessonCreator(TblTimLifeClass entity, List<LifeClassLessonMakeRequest> creatorData, UnitOfWork unitOfWork) : base(entity, unitOfWork)
        {
            _entity = entity;
            _unitOfWork = unitOfWork;
            _data = creatorData;            
        }

        /// <summary>
        /// 生成写生课之后的操作
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-13</para>
        /// </summary>
        public void AfterLessonCreate()
        {
            if (this._data != null && this._data.Count > 0)
            {
                //对报名的课程明细的已排课次进行累加
                EnrollOrderService service = new EnrollOrderService(_entity.SchoolId);
                foreach (var itemId in _data.Select(a => a.EnrollOrderItemId).Distinct())
                {
                    service.AddClassTimesUse(itemId, _entity.UseLessonCount, _unitOfWork);
                }
            }
        }

        /// <summary>
        /// 获取写生课的课次信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-10</para>
        /// </summary>
        /// <returns>课程信息列表</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：
        /// 1. 未找到数据
        /// </exception>
        public List<LessonCreatorInfo> GetLessonCreatorInfo()
        {
            List<LessonCreatorInfo> result = new List<LessonCreatorInfo>();

            // 1.获取学期信息
            var termInfo = TermService.GetTermByTermId(_entity.TermId);
            if (termInfo == null)
            {
                throw new BussinessException(ModelType.Default, 1);
            }
            // 2.添加课程信息
            foreach (var item in _data)
            {
                LessonCreatorInfo lesson = new LessonCreatorInfo
                {
                    BusinessId = _entity.LifeClassId,
                    BusinessType = base.BusinessType,
                    ClassId = item.ClassId,
                    ClassDate = _entity.ClassBeginTime.Value,
                    ClassBeginTime = _entity.ClassBeginTime.Value.ToString("yyyy.MM.dd HH:mm"),
                    ClassEndTime = _entity.ClassEndTime.Value.ToString("yyyy.MM.dd HH:mm"),
                    EnrollOrderItemId = item.EnrollOrderItemId,
                    SchoolId = _entity.SchoolId,
                    StudentId = item.StudentId,
                    TeacherId = _entity.TeacherId,
                    TermId = _entity.TermId,
                    LessonCount = _entity.UseLessonCount,
                    LessonType = LessonType.SketchCourse
                };
                result.Add(lesson);
            }

            return result;
        }
    }
}
