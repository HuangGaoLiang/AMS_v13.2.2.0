using AMS.Core.Constants;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AMS.Service
{
    /// <summary>
    /// 描述：删除补课周补课
    /// <para>作    者：Huang GaoLiang </para>
    /// <para>创建时间：2019-03-18 </para>
    /// </summary>
    public class AdjustLessonReplenishWeekFinisher : ILessonFinisher
    {
        #region 构造函数

        private readonly string _schoolId;// 校区编号
        private readonly long _studentId;// 学生编号
        private readonly UnitOfWork _unitOfWork;// 事务单元
        private List<TblTimAdjustLesson> _adjustLesson;// 课次调整业务表
        private TblDatClass _datClass;// 班级
        private TblDatOperationLog _log;// 操作日志

        /// <summary>
        /// 描述：删除补课周补课
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <param name="studentId">学生编号</param>
        /// <param name="adjustLesson">课次调整业务表</param>
        /// <param name="datClass">班级信息</param>
        /// <param name="log">操作日志</param>
        /// <param name="unitOfWork">事务单元</param>
        public AdjustLessonReplenishWeekFinisher(string schoolId, long studentId, List<TblTimAdjustLesson> adjustLesson, TblDatClass datClass, TblDatOperationLog log, UnitOfWork unitOfWork)
        {
            this._schoolId = schoolId;
            this._studentId = studentId;
            this._adjustLesson = adjustLesson;
            this._datClass = datClass;
            this._log = log;
            this._unitOfWork = unitOfWork;
        }

        #endregion

        /// <summary>
        /// 删除补课周补课之后
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-18</para>
        /// </summary>
        public void AfterLessonFinish()
        {
            // 更新课次业务调整记录表
            Expression<Func<TblTimAdjustLesson, TblTimAdjustLesson>> updateLambda = t => new TblTimAdjustLesson
            {
                Status = (int)TimAdjustLessonStatus.Invalid
            };
            var adjustLessonIds = this._adjustLesson.Select(m => m.AdjustLessonId).ToList();
            _unitOfWork.GetCustomRepository<TblTimAdjustLessonRepository, TblTimAdjustLesson>().Update(m => adjustLessonIds.Contains(m.AdjustLessonId), updateLambda);

            if (this._datClass.ClassType == (int)ClassType.ReplenishWeek)
            {
                // 删除补课周班级
                _unitOfWork.GetCustomRepository<TblDatClassRepository, TblDatClass>().Delete(this._datClass);
            }

            // 添加日志记录
            new OperationLogService().Add(this._log, _unitOfWork);

        }


        /// <summary>
        /// 获取课次业务调整表需要删除的课次信息
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-18</para>
        /// </summary>
        /// <returns>返回要删除的课次信息</returns>
        public List<LessonFinisherInfo> GetLessonFinisherInfo()
        {

            List<LessonFinisherInfo> lessonFinisherInfo = new List<LessonFinisherInfo>();
            var fromLessonIds = this._adjustLesson.Select(m => m.FromLessonId).ToList();

            var timReplenishLessonList = _unitOfWork.GetCustomRepository<TblTimReplenishLessonRepository, TblTimReplenishLesson>().LoadList(m => fromLessonIds.Contains(m.ParentLessonId));


            lessonFinisherInfo = this._adjustLesson.Select(m => new LessonFinisherInfo
            {
                BusinessId = m.AdjustLessonId,
                BusinessType = m.BusinessType,
                LessonId = timReplenishLessonList.FirstOrDefault(k => k.ParentLessonId == m.FromLessonId).LessonId,
                Remark = LessonProcessConstants.WeekDeleteRemark
            }).ToList();

            return lessonFinisherInfo;
        }
    }
}
