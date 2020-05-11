using System;
using System.Collections.Generic;
using System.Linq;
using AMS.Core;
using AMS.Core.Locks;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;

namespace AMS.Service
{
    /// <summary>
    /// 课次操作服务
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-02-20</para>
    /// </summary>
    public class LessonService : BaseLessonService
    {
        private readonly Lazy<TblTimLessonRepository> _lessonRepository;                //课次基础数据仓储
        private readonly Lazy<TblTimLessonProcessRepository> _lessonProcessRepository;  //课次变更记录过程数据仓储
        private readonly Lazy<TblTimLessonStudentRepository> _lessonStudentRepository;  //学生考勤仓储
        private readonly Lazy<TblTimReplenishLessonRepository> _replenishLessonRepository;

        /// <summary>
        /// 课次操作服务
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        public LessonService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
            _lessonRepository = new Lazy<TblTimLessonRepository>(unitOfWork.GetCustomRepository<TblTimLessonRepository, TblTimLesson>);
            _lessonProcessRepository = new Lazy<TblTimLessonProcessRepository>(unitOfWork.GetCustomRepository<TblTimLessonProcessRepository, TblTimLessonProcess>);
            _lessonStudentRepository = new Lazy<TblTimLessonStudentRepository>(unitOfWork.GetCustomRepository<TblTimLessonStudentRepository, TblTimLessonStudent>);
            _replenishLessonRepository = new Lazy<TblTimReplenishLessonRepository>(unitOfWork.GetCustomRepository<TblTimReplenishLessonRepository, TblTimReplenishLesson>);
        }

        /// <summary>
        /// 课次生产
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-08</para>
        /// </summary>
        /// <param name="creator">课次的产生者接口</param>
        /// <exception cref="BussinessException">
        /// 异常ID：35 异常描述：创建的课次为空
        /// </exception>
        public void Create(ILessonCreator creator)
        {
            List<LessonCreatorInfo> lessonCreatorInfos = creator.GetLessonCreatorInfo();
            if (lessonCreatorInfos == null || !lessonCreatorInfos.Any())
            {
                //创建的课次为空
                throw new BussinessException(ModelType.Timetable, 35);
            }

            List<TblTimLesson> lessons = new List<TblTimLesson>();//课次
            List<TblTimLessonProcess> lessonProceses = new List<TblTimLessonProcess>();//课次过程
            List<TblTimLessonStudent> lessonStudents = new List<TblTimLessonStudent>();//学生课次时间表

            foreach (var m in lessonCreatorInfos)
            {
                DateTime classBeginDate; //上课开始时间
                DateTime classEndDate;   //上课结束时间
                if (m.LessonType == LessonType.RegularCourse)
                {
                    classBeginDate = DateTime.Parse($"{m.ClassDate:yyyy-MM-dd} {m.ClassBeginTime}");
                    classEndDate = DateTime.Parse($"{m.ClassDate:yyyy-MM-dd} {m.ClassEndTime}");
                }
                else
                {
                    classBeginDate = DateTime.Parse(m.ClassBeginTime);
                    classEndDate = DateTime.Parse(m.ClassEndTime);
                }

                //课次
                TblTimLesson lesson = new TblTimLesson
                {
                    BusinessId = m.BusinessId,
                    BusinessType = m.BusinessType,
                    ClassBeginTime = m.ClassBeginTime,
                    ClassDate = m.ClassDate,
                    ClassEndTime = m.ClassEndTime,
                    ClassId = m.ClassId,
                    ClassRoomId = m.ClassRoomId,
                    CourseId = m.CourseId,
                    CourseLevelId = m.CourseLevelId,
                    EnrollOrderItemId = m.EnrollOrderItemId,
                    SchoolId = m.SchoolId,
                    StudentId = m.StudentId,
                    TeacherId = m.TeacherId,
                    TermId = m.TermId,
                    LessonId = IdGenerator.NextId(),
                    LessonType = (int)m.LessonType,
                    LessonCount = m.LessonCount,
                    Status = (int)LessonUltimateStatus.Normal,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    ClassBeginDate = classBeginDate,
                    ClassEndDate = classEndDate
                };

                //课次过程
                TblTimLessonProcess lessonProcess = new TblTimLessonProcess
                {
                    SchoolId = lesson.SchoolId,
                    BusinessId = lesson.BusinessId,
                    BusinessType = lesson.BusinessType,
                    LessonId = lesson.LessonId,
                    LessonProcessId = IdGenerator.NextId(),
                    ProcessStatus = (int)ProcessStatus.Create,
                    Remark = string.Empty,
                    ProcessTime = DateTime.Now,
                    CreateTime = DateTime.Now
                };

                //学生课次时间
                TblTimLessonStudent lessonStudent = new TblTimLessonStudent
                {
                    LessonId = lesson.LessonId,
                    LessonStudentId = IdGenerator.NextId(),
                    StudentId = lesson.StudentId,
                    SchoolId = lesson.SchoolId,
                    AttendStatus = (int)AttendStatus.NotClockIn,
                    CreateTime = DateTime.Now,
                    AdjustType = (int)AdjustType.DEFAULT,
                    AttendUserType = (int)AttendUserType.DEFAULT,
                    ReplenishCode = string.Empty,
                };

                lessons.Add(lesson);
                lessonProceses.Add(lessonProcess);
                lessonStudents.Add(lessonStudent);
            }

            var lessonCreatorInfo = lessonCreatorInfos.FirstOrDefault();
            string schoolId = lessonCreatorInfo.SchoolId;
            string studentId = lessonCreatorInfo.StudentId.ToString();

            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_AMSSCHOOLSTUDENT, schoolId, studentId))
            {
                //校验排课时间是否冲突
                base.VerifyClassTimeCross(
                    schoolId, lessonCreatorInfo.StudentId,
                    this.GetVerifyClassTimeList(lessonCreatorInfos),
                    lessonCreatorInfo.LessonType);

                _lessonRepository.Value.Add(lessons);
                _lessonProcessRepository.Value.Add(lessonProceses);
                _lessonStudentRepository.Value.Add(lessonStudents);
            }

            //排课之后处理的业务
            creator.AfterLessonCreate();
        }

        /// <summary>
        /// 课次结束
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-08</para>
        /// </summary>
        /// <param name="finisher">课次结束者接口</param>
        /// <exception cref="BussinessException">
        /// 异常ID：36 异常描述：销毁的课次为空
        /// </exception>
        public void Finish(ILessonFinisher finisher)
        {
            List<LessonFinisherInfo> lessonFinisherInfo = finisher.GetLessonFinisherInfo();
            if (lessonFinisherInfo == null || !lessonFinisherInfo.Any())
            {
                //销毁的课次为空
                throw new BussinessException(ModelType.Timetable, 36);
            }

            List<long> lessonIds = lessonFinisherInfo.Select(x => x.LessonId).ToList();

            //课次基础数据作废
            List<TblTimLesson> lessons = _lessonRepository.Value.GetByLessonIdTask(lessonIds).Result;

            var info = lessonFinisherInfo.FirstOrDefault();
            long businessId = info.BusinessId;    //业务Id
            int businessType = info.BusinessType;//业务类型
            string remark = string.IsNullOrWhiteSpace(info.Remark) ? EnumName.GetDescription(typeof(LessonBusinessType), businessType) : info.Remark;

            //产生课次变更记录表
            List<TblTimLessonProcess> lessonProceses = lessons.Select(m => new TblTimLessonProcess
            {
                LessonProcessId = IdGenerator.NextId(),
                SchoolId = m.SchoolId,
                BusinessId = businessId,
                BusinessType = businessType,
                LessonId = m.LessonId,
                ProcessStatus = (int)ProcessStatus.End,
                Remark = remark,
                ProcessTime = DateTime.Now,
                CreateTime = DateTime.Now
            }).ToList();

            //1.课次作废
            _lessonRepository.Value.UpdateLessonStatus(lessonIds, LessonUltimateStatus.Invalid);
            //2.课次销毁记录
            _lessonProcessRepository.Value.Add(lessonProceses);
            //3.补课周课次调整状态回退
            this.ReplenishLessonGoBack(lessonIds);
            //4.销毁调课表课次
            _lessonStudentRepository.Value.DeleteByLessonId(lessonIds);
            //5.销毁正常学生考勤表课次
            _replenishLessonRepository.Value.DeleteByLessonId(lessonIds);
            
            finisher.AfterLessonFinish();
        }


        /// <summary>
        /// 补课周课次销毁，调整状态回退
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-25</para>
        /// </summary>
        /// <param name="lessonId">课次ID</param>
        private void ReplenishLessonGoBack(IEnumerable<long> lessonId)
        {
            var list = _replenishLessonRepository.Value.GetLessonListByLessonId(lessonId);
            if (!list.Any())
            {
                return;
            }
            var goBackStudentAttends = list.Where(x => x.ParentLessonId == x.RootLessonId)
                .Select(x => x.ParentLessonId).ToList();
            if (goBackStudentAttends.Any())
            {
                _lessonStudentRepository.Value.UpdateAdjustTypeByLessonId(
                    goBackStudentAttends, AdjustType.SUPPLEMENTARYWEEK, AdjustType.DEFAULT);
            }

            var goBackReplenishAttends = list.Where(x => x.ParentLessonId != x.RootLessonId)
                .Select(x => x.ParentLessonId).ToList();
            if (goBackReplenishAttends.Any())
            {
                _replenishLessonRepository.Value.UpdateAdjustTypeByLessonId(
                    goBackReplenishAttends, AdjustType.SUPPLEMENTARYWEEK, AdjustType.DEFAULT);
            }
        }

        /// <summary>
        /// 班级是否被排课
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        internal static bool ClassIsUse(long classId)
        {
            ViewCompleteStudentAttendanceRepository repository = new ViewCompleteStudentAttendanceRepository();

            return repository.ClassIsUse(classId);
        }

        /// <summary>
        /// 上课时间转换
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="lessonCreatorInfoList">课次调整列表</param>
        /// <returns>上课时间段列表</returns>
        private List<VerifyClassTime> GetVerifyClassTimeList(IEnumerable<LessonCreatorInfo> lessonCreatorInfoList)
        {
            return lessonCreatorInfoList.Select(x => new VerifyClassTime
            {
                ClassDate = x.ClassDate,
                ClassBeginTime = x.ClassBeginTime,
                ClassEndTime = x.ClassEndTime
            }).ToList();
        }
    }
}
