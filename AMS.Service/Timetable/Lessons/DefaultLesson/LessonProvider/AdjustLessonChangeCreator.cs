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
    /// 调课，课次创建
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-15</para>
    /// </summary>
    public class AdjustLessonChangeCreator : AdjustLessonChangeProvider, IReplenishLessonCreator
    {
        private readonly long _batchNo;                 //批次号
        private readonly string _schoolId;              //校区Id
        private readonly AdjustChangeRequest _request;  //调课参数

        private readonly List<TblTimAdjustLesson> _adjustLessons = new List<TblTimAdjustLesson>(); //业务记录表数据

        private readonly TblTimLessonRepository _lessonRepository;
        private readonly ViewCompleteStudentAttendanceRepository _viewCompleteStudentAttendanceRepository;
        private readonly TblTimAdjustLessonRepository _tblTimAdjustLessonRepository;

        /// <summary>
        /// 调课，课次创建服务
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="request">调课参数</param>
        /// <param name="unitOfWork">事物单元</param>
        public AdjustLessonChangeCreator(string schoolId, AdjustChangeRequest request, UnitOfWork unitOfWork)
        {
            this._batchNo = IdGenerator.NextId();
            this._schoolId = schoolId;
            this._request = request;

            _lessonRepository = unitOfWork.GetCustomRepository<TblTimLessonRepository, TblTimLesson>();
            _viewCompleteStudentAttendanceRepository = unitOfWork.GetCustomRepository<ViewCompleteStudentAttendanceRepository, ViewCompleteStudentAttendance>();
            _tblTimAdjustLessonRepository = unitOfWork.GetCustomRepository<TblTimAdjustLessonRepository, TblTimAdjustLesson>();
        }

        /// <summary>
        /// 课次创建之后的回调
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        public void AfterReplenishLessonCreate()
        {
            //写入业务记录表
            _tblTimAdjustLessonRepository.Add(_adjustLessons);
        }

        /// <summary>
        ///准备补课课次的数据源
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <returns>准备创建补课课次数据源列表</returns>
        public List<ReplenishLessonCreatorInfo> GetReplenishLessonCreatorInfo()
        {
            List<ReplenishLessonCreatorInfo> res = new List<ReplenishLessonCreatorInfo>();

            //1.获取转出课次信息
            var outLessonList = this.GetOutLessonList();

            //2.校验转出课次
            this.VerifyOutLesson(outLessonList);

            //3.转入课次信息
            var inLessonList = this.GetInLessonList();

            //4.校验转入课次
            this.VerifyInLesson(outLessonList, inLessonList);

            for (int i = 0; i < inLessonList.Count; i++)
            {
                var inLesson = inLessonList[i];
                var outLesson = outLessonList[i];
                //1.获取业务记录实体
                var adjustLesson = GetAdjustLesson(inLesson, outLesson);
                //2.获取创建课次实体
                var creatorInfo = GetReplenishLessonCreatorInfo(inLesson, outLesson, adjustLesson);

                _adjustLessons.Add(adjustLesson);
                res.Add(creatorInfo);
            }

            return res;
        }

        /// <summary>
        /// 获取调整课次信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="inLesson">转入课次信息</param>
        /// <param name="outLesson">转出课次信息</param>
        /// <param name="adjustLesson">课次业务信息</param>
        /// <returns>调整课次信息</returns>
        private ReplenishLessonCreatorInfo GetReplenishLessonCreatorInfo(
            ViewCompleteStudentAttendance inLesson,
            ViewCompleteStudentAttendance outLesson,
            TblTimAdjustLesson adjustLesson)
        {
            return new ReplenishLessonCreatorInfo
            {
                BusinessId = adjustLesson.AdjustLessonId,
                BusinessType = (LessonBusinessType)this.BusinessType,
                TeacherId = inLesson.TeacherId,
                ClassBeginTime = inLesson.ClassBeginTime,
                ClassDate = inLesson.ClassDate,
                ClassEndTime = inLesson.ClassEndTime,
                ClassId = inLesson.ClassId,
                ClassRoomId = inLesson.ClassRoomId,
                CourseId = inLesson.CourseId,
                CourseLevelId = inLesson.CourseLevelId,
                EnrollOrderItemId = inLesson.EnrollOrderItemId,
                LessonCount = inLesson.LessonCount,
                LessonType = (LessonType)inLesson.LessonType,
                SchoolId = inLesson.SchoolId,
                TermId = inLesson.TermId,

                OutLessonId = outLesson.LessonId,
                StudentId = outLesson.StudentId,
            };
        }

        /// <summary>
        /// 获取课次业务表数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="inLesson">转入课次信息</param>
        /// <param name="outLesson">转出课次信息</param>
        /// <returns>课次业务表数据</returns>
        private TblTimAdjustLesson GetAdjustLesson(
            ViewCompleteStudentAttendance inLesson,
            ViewCompleteStudentAttendance outLesson)
        {
            return new TblTimAdjustLesson
            {
                AdjustLessonId = IdGenerator.NextId(),
                BatchNo = this._batchNo,
                BusinessType = this.BusinessType,
                ClassDate = inLesson.ClassDate,
                ClassId = inLesson.ClassId,
                ClassBeginTime = inLesson.ClassBeginTime,
                ClassEndTime = inLesson.ClassEndTime,
                ClassRoomId = inLesson.ClassRoomId,
                CreateTime = DateTime.Now,
                FromLessonId = outLesson.LessonId,
                FromTeacherId = outLesson.TeacherId,
                Remark = string.Empty,
                SchoolId = inLesson.SchoolId,
                SchoolTimeId = 0,
                Status = (int)TimAdjustLessonStatus.Normal,
                StudentId = outLesson.StudentId,
                ToTeacherId = inLesson.TeacherId
            };
        }

        /// <summary>
        /// 转入课次信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <returns>转入课次列表</returns>
        private List<ViewCompleteStudentAttendance> GetInLessonList()
        {
            //获取转入课次信息
            var inLessonList = _viewCompleteStudentAttendanceRepository.GetInLessonList(
                this._schoolId, this._request.ToClassId, this._request.ToClassDate, LessonType.RegularCourse);

            if (!inLessonList.Any())
            {
                return new List<ViewCompleteStudentAttendance>();
            }

            return inLessonList;
        }

        /// <summary>
        /// 转出课次信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <returns>转出课次信息列表</returns>
        private List<ViewCompleteStudentAttendance> GetOutLessonList()
        {
            //获取今天转出课次信息
            var outLessonList = _viewCompleteStudentAttendanceRepository.GetClassInfoTheDayList(
                this._schoolId, this._request.ClassId, this._request.ClassDate, LessonType.RegularCourse);

            if (!outLessonList.Any())
            {
                return new List<ViewCompleteStudentAttendance>();
            }

            outLessonList = outLessonList.Where(
                x => x.StudentId == this._request.StudentId &&
                     x.AdjustType == (int)AdjustType.DEFAULT &&
                     x.AttendStatus != (int)AttendStatus.Normal)
                     .ToList();

            return outLessonList;
        }

        /// <summary>
        /// 校验转出课次
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        /// <param name="outLessons">转出课次信息</param>
        /// <exception cref="BussinessException">
        /// 异常ID：60->未找到课次信息
        /// 异常ID：65->课程已正常考勤,不能再进行调课
        /// 异常ID：66->课程已被调整,不能再进行调课
        /// </exception>
        private void VerifyOutLesson(List<ViewCompleteStudentAttendance> outLessons)
        {
            if (!outLessons.Any())
            {
                //未找到课次信息
                throw new BussinessException(ModelType.Timetable, 60);
            }

            foreach (var item in outLessons)
            {
                AttendStatus status = (AttendStatus)item.AttendStatus;
                AdjustType type = (AdjustType)item.AdjustType;
                if (status == AttendStatus.Normal)
                {
                    //课程已正常考勤,不能再进行调课
                    throw new BussinessException(ModelType.Timetable, 65);
                }

                if (type != AdjustType.DEFAULT)
                {
                    //课程已被调整,不能再进行调课
                    throw new BussinessException(ModelType.Timetable, 66);
                }
            }
        }

        /// <summary>
        /// 校验转入课次
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="outLessons">转出课次信息</param>
        /// <param name="inLessons">转入课次信息</param>
        /// <exception cref="BussinessException">
        /// 异常ID：62->不能跨学期安排补课
        /// 异常ID：63->安排补课的课次与原课次不一致
        /// </exception>
        private void VerifyInLesson(List<ViewCompleteStudentAttendance> outLessons,
            List<ViewCompleteStudentAttendance> inLessons)
        {
            if (outLessons.Count != inLessons.Count)
            {
                //安排调课的课次与原课次不一致
                throw new BussinessException(ModelType.Timetable, 67);
            }

            var except = outLessons.Select(x => x.TermId).Except(inLessons.Select(x => x.TermId));
            if (except.Any())
            {
                //不能跨学期安排调课
                throw new BussinessException(ModelType.Timetable, 68);
            }
        }
    }
}
