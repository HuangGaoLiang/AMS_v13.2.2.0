using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMS.Service
{
    /// <summary>
    /// 写生排课结束者
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public class LifeClassLessonFinisher : LifeClassLessonProvider, ILessonFinisher
    {
        private readonly List<long> _lessonIdList;                                          //如果不为0则退此课次
        private Lazy<TblTimLessonRepository> _lessonRepository;             //课次仓储        

        /// <summary>
        /// 写生排课结束者业务类型
        /// </summary>
        public override int BusinessType => (int)ProcessBusinessType.F_LifeClassMakeLesson;

        /// <summary>
        /// 写生排课结束者实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-10</para>
        /// </summary>
        /// <param name="entity">写生课信息</param>
        /// <param name="lessonIdList">课程Id列表</param>
        /// <param name="unitOfWork">事务单元</param>
        public LifeClassLessonFinisher(TblTimLifeClass entity, List<long> lessonIdList, UnitOfWork unitOfWork = null) : base(entity, unitOfWork)
        {
            _entity = entity;
            _lessonIdList = lessonIdList;
            _unitOfWork = unitOfWork;

            //初始化仓储
            InitUnitOfWork();
        }

        /// <summary>
        /// 初始化仓储
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-10</para>
        /// </summary>
        /// <param name="unitOfWork"></param>
        private void InitUnitOfWork()
        {
            _lessonRepository = _unitOfWork != null
                ? new Lazy<TblTimLessonRepository>(() =>
                {
                    return _unitOfWork.GetCustomRepository<TblTimLessonRepository, TblTimLesson>();
                }) : new Lazy<TblTimLessonRepository>();
        }

        /// <summary>
        /// 取消之后的操作
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-10</para>
        /// </summary>
        public void AfterLessonFinish()
        {
            var lessonInfoList = _lessonRepository.Value.GetByLessonIdTask(_lessonIdList).Result;
            if (lessonInfoList != null && lessonInfoList.Count > 0)
            {
                //对报名的课程明细的已排课次进行扣减
                EnrollOrderService service = new EnrollOrderService(_entity.SchoolId);
                foreach (var itemId in lessonInfoList.Select(a => a.EnrollOrderItemId).Distinct())
                {
                    service.AddClassTimesUse(itemId, -_entity.UseLessonCount, _unitOfWork);
                }
            }
        }

        /// <summary>
        /// 获取写生课需要取消的课次
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-10</para>
        /// </summary>
        /// <returns>课程取消信息列表</returns>
        public List<LessonFinisherInfo> GetLessonFinisherInfo()
        {
            var result = _lessonIdList.Select(x => new LessonFinisherInfo
            {
                LessonId = x,
                BusinessId = _entity.LifeClassId,
                BusinessType = BusinessType,
            }).ToList();
            return result;
        }
    }
}
