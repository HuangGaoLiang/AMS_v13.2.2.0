using System.Collections.Generic;
using System.Linq;
using AMS.Core.Constants;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;

namespace AMS.Service
{
    /// <summary>
    /// 描述：转班课次结束者
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-7</para>
    /// </summary>
    public class ChangeClassLessonFinisher : ChangeClassLessonProvider, ILessonFinisher
    {
       
        /// <summary>
        /// 描述：实例化提供转班销毁课次对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="entity">转班信息</param>
        /// <param name="unitOfWork">工作单元事务</param>
        public ChangeClassLessonFinisher(TblTimChangeClass entity, UnitOfWork unitOfWork) : base(entity, unitOfWork)
        {
        }


        /// <summary>
        /// 描述：转班时的的所属报名项(报名课程明细Id)
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        public long EnrollOrderItemId
        {
            get; private set;
        }

        /// <summary>
        /// 暂不实现
        /// </summary>
        public void AfterLessonFinish()
        {
        }

        /// <summary>
        /// 描述：查询出转班时需要销毁的课次
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <returns>要销毁的课次集合</returns>
        public List<LessonFinisherInfo> GetLessonFinisherInfo()
        {
            //根据转出班级ID，查询出要销毁的课次
            var service = new StudentTimetableService(base._entity.SchoolId, base._entity.StudentId);

            //var outLessonNum = service.GetRollOutClassLessons(base._entity.OutClassId, base._entity.OutDate, base._unitOfWork);

            List<int> lessonTypeList = new List<int> { (int)LessonType.RegularCourse };

            //获取学生转出班级课次列表
            var stuTransferOutClassLessonList =
                service.GetStudentTransferOutClassLessonList(base._entity.OutClassId,
                base._entity.OutDate, lessonTypeList, base._unitOfWork);

            var outLessonNumEntity = stuTransferOutClassLessonList.FirstOrDefault();
            if (outLessonNumEntity != null)
            {
                this.EnrollOrderItemId = outLessonNumEntity.EnrollOrderItemId;
            }

            var result = stuTransferOutClassLessonList.Select(x => new LessonFinisherInfo
            {
                LessonId = x.LessonId,
                BusinessId = base._entity.ChangeClassId,
                BusinessType = BusinessType,
                Remark = LessonProcessConstants.ChangeClassLessonReamrk

            }).ToList();

            return result;

        }
    }
}
