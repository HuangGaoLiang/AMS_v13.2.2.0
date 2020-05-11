using System;
using System.Collections.Generic;
using System.Linq;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;

namespace AMS.Service
{
    /// <summary>
    /// 课表作废结束者
    /// </summary>
    public class CancelMakeLessonFinisher : MakeLessonProvider, ILessonFinisher
    {
        private readonly Lazy<TblTimMakeLessonRepository> _makeLessonRepository;
        private readonly Lazy<TblTimLessonRepository> _lessonRepository;

        private readonly TblTimMakeLesson _makeLesson;
        private readonly UnitOfWork _unitOfWork;
        public CancelMakeLessonFinisher(long makeLessonId, UnitOfWork unitOfWork) : base(makeLessonId)
        {
            this._unitOfWork = unitOfWork;
            _makeLessonRepository = new Lazy<TblTimMakeLessonRepository>(unitOfWork.GetCustomRepository<TblTimMakeLessonRepository, TblTimMakeLesson>());
            _lessonRepository = new Lazy<TblTimLessonRepository>(unitOfWork.GetCustomRepository<TblTimLessonRepository, TblTimLesson>());
            _makeLesson = _makeLessonRepository.Value.Load(makeLessonId);
        }

        public List<LessonFinisherInfo> GetLessonFinisherInfo()
        {
            if (_makeLesson == null)
            {
                throw new ArgumentNullException(nameof(_makeLesson));
            }

            List<TblTimLesson> lessons = _lessonRepository.Value.GetByEnrollOrderItemIdAsync(_makeLesson.EnrollOrderItemId).Result;

            List<LessonFinisherInfo> res = lessons.Select(m => new LessonFinisherInfo { LessonId = m.LessonId }).ToList();

            return res;
        }

        public void AfterLessonFinish()
        {
        }

    }
}
