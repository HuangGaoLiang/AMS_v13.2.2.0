using System;
using System.Collections.Generic;
using System.Linq;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;

/*
 * 取消转校之后重新排课
 */

namespace AMS.Service
{
    /// <summary>
    /// 取消转校之后获取转校前销毁的课次
    /// </summary>
    public class ChangeSchoolLessonCreator : ChangeSchoolLessonProvider, ILessonCreator
    {
        //课次过程记录表
        private readonly Lazy<TblTimLessonProcessRepository> _lessonProcessRepository = new Lazy<TblTimLessonProcessRepository>();
        //课次信息表
        private readonly Lazy<TblTimLessonRepository> _lessonRepository = new Lazy<TblTimLessonRepository>();
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// 转校课次产生者
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-15</para>
        /// </summary>
        /// <param name="refundOrderId">转校订单Id</param>
        /// <param name="unitOfWork">事物</param>
        public ChangeSchoolLessonCreator(long refundOrderId, UnitOfWork unitOfWork) : base(refundOrderId)
        {
            this._unitOfWork = unitOfWork;
            _lessonProcessRepository = new Lazy<TblTimLessonProcessRepository>(unitOfWork.GetCustomRepository<TblTimLessonProcessRepository, TblTimLessonProcess>());
            _lessonRepository = new Lazy<TblTimLessonRepository>(unitOfWork.GetCustomRepository<TblTimLessonRepository, TblTimLesson>());
        }

        /// <summary>
        /// 排课之后回调函数
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-15</para>
        /// </summary>
        public void AfterLessonCreate()
        {

        }

        /// <summary>
        /// 获取课次信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-15</para>
        /// </summary>
        /// <returns>构造完成课次信息</returns>
        public List<LessonCreatorInfo> GetLessonCreatorInfo()
        {
            //1、根据转校订单Id获取转校销毁的课次
            //2、根据销毁的课次Id获取课次信息
            //3、重新创建

            List<TblTimLessonProcess> lessonProcesses = _lessonProcessRepository.Value.GetByBusinessId(_refundOrderId);

            List<TblTimLesson> lessons = _lessonRepository.Value.GetByLessonIdTask(lessonProcesses.Select(x => x.LessonId)).Result;

            List<LessonCreatorInfo> res = lessons.Select(x => new LessonCreatorInfo
            {
                BusinessId = x.BusinessId,
                BusinessType = (byte)x.BusinessType,
                ClassBeginTime = x.ClassBeginTime,
                ClassDate = x.ClassDate,
                ClassEndTime = x.ClassEndTime,
                ClassId = x.ClassId,
                ClassRoomId = x.ClassRoomId,
                CourseId = x.CourseId,
                CourseLevelId = x.CourseLevelId,
                EnrollOrderItemId = x.EnrollOrderItemId,
                LessonCount = x.LessonCount,
                LessonType = (LessonType)x.LessonType,
                SchoolId = x.SchoolId,
                StudentId = x.StudentId,
                TeacherId = x.TeacherId,
                TermId = x.TermId
            }).ToList();

            return res;
        }
    }
}
