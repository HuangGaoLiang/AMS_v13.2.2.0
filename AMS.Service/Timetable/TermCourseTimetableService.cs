using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Anticorrosion.HRS;
using AMS.Core;
using AMS.Dto;
using AMS.Storage.Models;
using AMS.Storage.Repository;

namespace AMS.Service
{
    /// <summary>
    /// 学期排课表
    /// </summary>
    public class TermCourseTimetableService : BService
    {
        private readonly Lazy<TblDatRoomCourseRepository> _datRoomCourseRepository = new Lazy<TblDatRoomCourseRepository>();
        private readonly Lazy<TblDatTermRepository> _termRepository = new Lazy<TblDatTermRepository>();
        private readonly Lazy<TblTimClassTimeRepository> _classTimeRepository = new Lazy<TblTimClassTimeRepository>();
        private readonly Lazy<TblDatClassRepository> _classRepository = new Lazy<TblDatClassRepository>();

        //负责处理审批事项
        private readonly Lazy<TermCourseTimetableAuditService> _auditService = null;

        private readonly long _termId;                  //学期Id
        private TermTimetableResponse _termTimetable;   //学期的课程表信息

        /// <summary>
        /// 初始化一个学期的课表实例
        /// </summary>
        /// <param name="termId">所属学期</param>
        public TermCourseTimetableService(long termId)
        {
            this._termId = termId;
            this.TblDatTerm1 = _termRepository.Value.Load(_termId);
            _auditService = new Lazy<TermCourseTimetableAuditService>(() =>
            {
                return new TermCourseTimetableAuditService(termId);
            });
        }

        private TblDatTerm _tblDatTerm;                 //当前学期的基础信息
        /// <summary>
        /// 学期信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-30</para>
        /// </summary>
        public TblDatTerm TblDatTerm { get { return TblDatTerm1; } }

        /// <summary>
        /// 学期排课数据(已审核)
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-30</para>
        /// </summary>
        public TermTimetableResponse TermTimetable
        {
            get
            {
                if (_termTimetable == null)
                {
                    _termTimetable = this.GetTermCourseTimetable();
                }
                return _termTimetable;
            }
        }

        /// <summary>
        /// 学期排课数据(设置中的数据)
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-30</para>
        /// </summary>
        public TermTimetableResponse TermTimetableSetting
        {
            get
            {
                if (_auditService.Value.TblAutAudit != null)
                {
                    //1、检查是否有审核中的数据
                    if (_auditService.Value.NoPass)
                    {
                        return _auditService.Value.TermTimetable;
                    }
                }
                //正式数据
                return this.TermTimetable;
            }
        }

        public TblDatTerm TblDatTerm1 { get => _tblDatTerm; set => _tblDatTerm = value; }

        /// <summary>
        /// 查询构造器
        /// </summary>
        /// <returns></returns>
        public static TermCourseTimetableSearchBuilder CreateSearchBuilder()
        {
            return new TermCourseTimetableSearchBuilder();
        }

        /// <summary>
        /// 删除学期课表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-30</para>
        /// </summary>
        internal void Remove()
        {
            List<TblDatClass> list = DefaultClassService.GetClasssByTermId(this._termId);
            List<long> classId = list.Select(t => t.ClassId).ToList();

            //1、删除时间课表
            _classTimeRepository.Value.DeleteByClassId(classId);

            //2、删除班级数据
            DefaultClassService.DeleteByTermId(this._termId);
        }

        #region GetTermCourseTimetable 获取学期排课表

        /// <summary>
        /// 获取学期排课表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-30</para>
        /// </summary>
        private TermTimetableResponse GetTermCourseTimetable()
        {
            TermTimetableResponse res = new TermTimetableResponse();

            res.CourseTimetableSixty = this.GetTermCourseTimetable(TimeType.Sixty);
            res.CourseTimetableNinety = this.GetTermCourseTimetable(TimeType.Ninety);
            res.TermBeginDate = this.TblDatTerm1.BeginDate.ToString("yyyy-MM-dd");
            res.TermEndDate = this.TblDatTerm1.EndDate.ToString("yyyy-MM-dd");
            res.TermTimetableAuditResponse = null;

            TblAutAudit tblAutAudit = _auditService.Value.TblAutAudit;
            if (tblAutAudit != null)
            {
                res.TermTimetableAuditResponse = new TermTimetableAuditResponse
                {
                    AuditName = tblAutAudit.AuditUserName,
                    Auditime = tblAutAudit.AuditDate.ToString("yyyy-MM-dd"),
                    AuditStatus = (AuditStatus)tblAutAudit.AuditStatus,
                    Remark = tblAutAudit.DataExt
                };
            }

            return res;
        }

        /// <summary>
        /// 获取不同时间段的课表信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-30</para>
        /// </summary>
        /// <param name="duration">60分钟或90分钟</param>
        /// <returns>课程表明细</returns>
        private TermCourseTimetableResponse GetTermCourseTimetable(TimeType duration)
        {
            TermCourseTimetableResponse res = new TermCourseTimetableResponse();

            var schTimeRes = this.GetTimetableSchoolTimeResponse(duration);

            res.SchoolTimes = schTimeRes.timetableSchoolTime;                 //上课时间表
            res.ClassRooms = new List<TimetableClassRoomResponse>();          //教室信息

            //班级上课时间表
            var classTimes = new TblTimClassTimeRepository()
                .GetBySchoolTimeId(schTimeRes.schoolTimes.Select(x => x.SchoolTimeId))
                .Result;

            //班级
            List<TblDatClass> classes = DefaultClassService.GetClasssByTermId(this._termId);

            //获取校区教室
            var durationClassId = classTimes.Select(x => x.ClassId).ToList();
            SchoolClassRoomService schoolClassRoomService = new SchoolClassRoomService(this.TblDatTerm1.SchoolId);

            List<RoomCourseResponse> roomCourseResponses = schoolClassRoomService.RoomCourseList;

            List<long> classRoomIds = classes.Where(x => durationClassId.Contains(x.ClassId)).Select(x => x.ClassRoomId).ToList();
            classRoomIds.AddRange(roomCourseResponses.Where(x => !x.IsDisabled).Select(x => x.ClassRoomId));

            List<TblDatClassRoom> classRooms = schoolClassRoomService
                .ClassRoomList
                .Where(x => classRoomIds.Contains(x.ClassRoomId))
                .OrderBy(o => o.RoomNo, new NaturalStringComparer())
                .ToList();

            //获取老师用户信息
            var teachers = TeachService.GetTeachers();

            //所有的课程
            var courses = CourseService.GetAllAsync().Result;

            //课程等级
            var courseLv = CourseLevelService.GetCourseLevelList().Result;

            //渲染教室
            foreach (var classRoom in classRooms)
            {
                TimetableClassRoomResponse ttcrr = new TimetableClassRoomResponse
                {
                    ClassRoomId = classRoom.ClassRoomId,
                    RoomNo = classRoom.RoomNo,
                    Classes = new List<TimetableClassResponse>()
                };

                int lessonIndex = 0;

                int weekDay = -1;

                //教室对应的上课时间段的班级
                foreach (var schoolTime in schTimeRes.schoolTimes)
                {
                    if (weekDay != schoolTime.WeekDay)
                    {
                        weekDay = schoolTime.WeekDay;
                        lessonIndex = 0;
                    }

                    lessonIndex++;

                    TimetableClassResponse ttcr = new TimetableClassResponse
                    {
                        SchoolTimeId = schoolTime.SchoolTimeId,
                        ClassId = 0,
                        CourseId = 0,
                        CourseName = string.Empty,
                        CourseLeveId = 0,
                        CourseLevelName = string.Empty,
                        TeacherId = string.Empty,
                        TeacherName = string.Empty,
                        WeekDay = schoolTime.WeekDay,
                        LessonIndex = lessonIndex
                    };

                    var classIds = from a in classTimes
                                   join b in classes on a.ClassId equals b.ClassId
                                   where a.SchoolTimeId == schoolTime.SchoolTimeId && b.ClassRoomId == classRoom.ClassRoomId
                                   select a.ClassId;

                    if (classIds.Any())
                    {
                        var classInfo = classes.FirstOrDefault(x => x.ClassId == classIds.First());
                        ttcr.ClassId = classInfo.ClassId;
                        ttcr.CourseId = classInfo.CourseId;
                        ttcr.CourseName = courses.FirstOrDefault(x => x.CourseId == classInfo.CourseId)?.ShortName;
                        ttcr.CourseLeveId = classInfo.CourseLeveId;
                        ttcr.CourseLevelName = courseLv.FirstOrDefault(x => x.CourseLevelId == classInfo.CourseLeveId)?.LevelCnName;
                        ttcr.TeacherId = classInfo.TeacherId;
                        ttcr.TeacherName = teachers.FirstOrDefault(x => x.TeacherId == classInfo.TeacherId)?.TeacherName;
                    }

                    ttcrr.Classes.Add(ttcr);
                }

                res.ClassRooms.Add(ttcrr);
            }

            return res;
        }

        /// <summary>
        /// 获取上课时间表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-30</para>
        /// </summary>
        /// <param name="duration">上课时间枚举</param>
        private (List<TimetableSchoolTimeResponse> timetableSchoolTime, List<TblDatSchoolTime> schoolTimes) GetTimetableSchoolTimeResponse(TimeType duration)
        {
            var resSchoolTimes = new SchoolTimeService(this.TblDatTerm1.TermId)
                .TblDatSchoolTime
                .Where(x => x.Duration == (int)duration)
                .OrderBy(x => x.WeekDay)
                .ThenBy(x => x.BeginTime)
                .ToList();

            List<TimetableSchoolTimeResponse> resTimetableSchoolTime = new List<TimetableSchoolTimeResponse>();

            if (!resSchoolTimes.Any())
            {
                return (resTimetableSchoolTime, resSchoolTimes);
            }

            for (int i = 1; i <= 7; i++)
            {
                TimetableSchoolTimeResponse schoolTime = new TimetableSchoolTimeResponse
                {
                    WeekDay = WeekDayConvert.IntToString(i),
                    Times = new List<SchoolTimePeriodResponse>()
                };

                var weekDayTime = resSchoolTimes
                    .Where(x => x.WeekDay == i)
                    .Select(x => new SchoolTimePeriodResponse
                    {
                        SchoolTimeId = x.SchoolTimeId,
                        BeginTime = x.BeginTime,
                        EndTime = x.EndTime
                    })
                     .OrderBy(x => x.BeginTime)
                    .ToList();

                schoolTime.Times.AddRange(weekDayTime);

                resTimetableSchoolTime.Add(schoolTime);
            }

            return (resTimetableSchoolTime, resSchoolTimes);
        }

        #endregion

        #region  Audit 提交审核
        /// <summary>
        /// 提交审核
        /// 作     者:Huang GaoLiang 2018年9月19日17:20:36
        /// </summary>
        /// <param name="auditorId">审核人编号</param>
        /// <param name="auditName">审核人名称</param>
        /// <param name="createUserId">创建人</param>
        /// <param name="createUserName"></param>
        public void SubmitAudit(string auditorId, string auditName, string createUserId, string createUserName)
        {
            //1.数据校验
            this.ValidateAudit(auditorId, auditName);

            OrgService orgService = new OrgService();
            var schoolList = orgService.GetAllSchoolList().FirstOrDefault(x => x.SchoolId.Trim() == this.TblDatTerm1.SchoolId.Trim());
            if (schoolList == null)
            {
                throw new BussinessException(ModelType.Default, 4);
            }

            var applyTitle = $"{schoolList.SchoolName}{this.TblDatTerm1.Year}{this.TblDatTerm1.TermName}";
            //2.调用审核类
            _auditService.Value.SubmitAudit(auditorId, auditName, applyTitle, createUserId, createUserName);

        }

        /// <summary>
        /// 提交审核数据校验
        /// 作     者:Huang GaoLiang 2018年10月9日09:40:53
        /// </summary>
        /// <param name="auditorId">审核人编号</param>
        /// <param name="auditorName">审核人名称</param>
        private void ValidateAudit(string auditorId, string auditorName)
        {
            if (string.IsNullOrWhiteSpace(auditorId) || string.IsNullOrWhiteSpace(auditorName))
            {
                throw new BussinessException((byte)ModelType.Audit, 9);
            }

            //判断待审核的数据有没有
            int auditStatus = this._auditService.Value.TblAutAudit.AuditStatus;
            if (auditStatus != (int)AuditStatus.WaitAudit)
            {
                throw new BussinessException((byte)ModelType.Audit, 5);
            }

            TblAutAudit autAudit = _auditService.Value.TblAutAudit;
            if (autAudit == null)
            {
                throw new BussinessException((byte)ModelType.Audit, 8);
            }
        }


        #endregion

        #region GetAllClassTime 获取学期所有班级时间
        /// <summary>
        /// 获取学期所有班级时间
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-30</para>
        /// </summary>
        /// <returns>班级课程表</returns>
        private async Task<List<TblTimClassTime>> GetAllClassTime()
        {
            List<long> classIds = DefaultClassService.GetClasssByTermId(this._termId).Select(t => t.ClassId).ToList();
            return await _classTimeRepository.Value.GetByClassId(classIds);
        }
        #endregion

        #region CopyToTermTimetable 复制学期班级
        /// <summary>
        /// 复制学期班级 
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>2018-09-26日 </para>
        /// </summary>
        /// <param name="toTermId">复制到学期编号</param>
        public async Task CopyToTermTimetable(long toTermId)
        {
            //FROM A 学期: 从A学期复制
            //TO B 学期:   复制到B学期

            // 1、校验 是否可以复制以及数据是否正确
            CheckTermData(this._termId, toTermId);


            // 2、将数据保存到审核草稿表
            long auditId = IdGenerator.NextId();
            // 2.1 处理审核主表数据
            TblAutAudit autAudit = GetAutAudit(auditId, toTermId);

            // 2.2 获取上课时间段表数据(B 学期)
            List<DatSchoolTimResponse> datSchoolTimeList = GetDatSchoolTimeList(toTermId).Select(m => new DatSchoolTimResponse
            {
                SchoolTimeId = m.SchoolTimeId,
                NewSchoolTimeId = m.NewSchoolTimeId,
                ToTermId = m.ToTermId,
                TermId = m.TermId,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                BeginTime = m.BeginTime,
                ClassTimeNo = m.ClassTimeNo,
                Duration = m.Duration,
                EndTime = m.EndTime,
                WeekDay = m.WeekDay
            }).ToList();


            // 2.3 获取已生效的班级表
            List<TblDatClass> classList = GetDatClassList();
            List<TblTimClassTime> timClassTimeList = GetAutClassTimeList();

            List<TblAutClass> autClassList = new List<TblAutClass>();
            List<TblAutClassTime> autClassTimeList = new List<TblAutClassTime>();

            // 2.4 处理审核中数据-班级课表和班级上课时间表数据
            foreach (TblDatClass item in classList)
            {
                var classId = IdGenerator.NextId();
                TblAutClass tblAutClass = new TblAutClass
                {
                    AutClassId = IdGenerator.NextId(),
                    ClassId = classId,
                    AuditId = auditId,
                    ClassNo = item.ClassNo,
                    TermId = toTermId,
                    RoomCourseId = item.RoomCourseId,
                    ClassRoomId = item.ClassRoomId,
                    CourseId = item.CourseId,
                    CourseLeveId = item.CourseLeveId,
                    TeacherId = item.TeacherId,
                    CourseNum = item.CourseNum,
                    StudentsNum = item.StudentsNum,
                    DataStatus = 0,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    SchoolId = item.SchoolId
                };

                autClassList.Add(tblAutClass);

                List<TblAutClassTime> list = timClassTimeList.Where(k => k.ClassId == item.ClassId)
                    .Select(m => new TblAutClassTime
                    {
                        AutClassTimeId = IdGenerator.NextId(),
                        ClassTimeId = IdGenerator.NextId(),
                        AuditId = auditId,
                        ClassId = classId,
                        SchoolTimeId = datSchoolTimeList.Where(u => u.SchoolTimeId == m.SchoolTimeId).Select(p => p.NewSchoolTimeId).FirstOrDefault(),
                        DataStatus = 0,
                        CreateTime = DateTime.Now,
                        SchoolId = m.SchoolId
                    }).ToList();

                autClassTimeList.AddRange(list);
            }

            List<TblDatSchoolTime> schoolTimeList = datSchoolTimeList.Select(m => new TblDatSchoolTime
            {
                SchoolTimeId = m.NewSchoolTimeId,
                TermId = toTermId,
                WeekDay = m.WeekDay,
                Duration = m.Duration,
                BeginTime = m.BeginTime,
                EndTime = m.EndTime,
                ClassTimeNo = m.ClassTimeNo,
                CreateTime = m.CreateTime,
                UpdateTime = m.UpdateTime,
                SchoolId = autAudit.SchoolId

            }).ToList();

            // 2.4提交数据
            await TermCourseTimetableAuditService.CreateTermTimetable(autAudit, schoolTimeList, autClassList, autClassTimeList, toTermId);
        }
        /// <summary>
        /// 获取班级上课时间集合
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>2018-09-26日 </para>
        /// </summary>
        /// <returns>返回班级上课时间集合</returns>
        private List<TblTimClassTime> GetAutClassTimeList()
        {
            return this.GetAllClassTime().Result.ToList();
        }

        /// <summary>
        /// 根据学期获取未禁用的班级集合
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>2018-09-26日 </para>
        /// </summary>
        /// <returns>返回班级集合</returns>
        private List<TblDatClass> GetDatClassList()
        {
            List<long> classRoomIds = _datRoomCourseRepository.Value.LoadList(m => true).Where(m => m.IsDisabled == false).Select(m => m.ClassRoomId).ToList();

            //获取正式的班级数据
            List<TblDatClass> classList = DefaultClassService.GetClasssByTermId(this._termId).Where(m => classRoomIds.Contains(m.ClassRoomId)).ToList();
            return classList;
        }

        /// <summary>
        /// 根据学期获取上课时间
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>2018-09-26日 </para>
        /// </summary>
        /// <param name="toTermId">学期编号</param>
        /// <returns>返回上课时间</returns>
        private List<DatSchoolTimResponse> GetDatSchoolTimeList(long toTermId)
        {
            return new SchoolTimeService(this._termId).TblDatSchoolTime.Select(m => new DatSchoolTimResponse
            {
                SchoolTimeId = m.SchoolTimeId,
                NewSchoolTimeId = IdGenerator.NextId(),
                TermId = m.TermId,
                ToTermId = toTermId,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                BeginTime = m.BeginTime,
                ClassTimeNo = m.ClassTimeNo,
                Duration = m.Duration,
                EndTime = m.EndTime,
                WeekDay = m.WeekDay
            }).ToList();
        }

        /// <summary>
        /// 生成审核表数据
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>2018-09-26日 </para>
        /// </summary>
        /// <param name="auditId">审核主键</param>
        /// <param name="toTermId">复制到的学期编号</param>
        /// <returns>返回审核数据</returns>
        private TblAutAudit GetAutAudit(long auditId, long toTermId)
        {
            return new TblAutAudit()
            {
                AuditId = auditId,
                FlowNo = "",
                BizType = (int)AuditBusinessType.TermCourseTimetable,
                ExtField1 = toTermId.ToString(),
                ExtField2 = "",
                AuditStatus = (int)AuditStatus.WaitAudit,
                AuditUserId = "",
                AuditUserName = "",
                AuditDate = DateTime.Now,
                DataExt = "",
                DataExtVersion = "",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                SchoolId = TblDatTerm1.SchoolId,
                CreateUserId = "",
                CreateUserName = ""
            };
        }

        /// <summary>
        /// 校验 是否可以复制以及数据是否正确
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>2018-09-26日 </para>
        /// </summary>
        /// <param name="currentTermId">当前学期编号</param>
        /// <param name="toTermId">复制到的学期编号</param>
        private void CheckTermData(long currentTermId, long toTermId)
        {
            //1.1 校验 A学期 的班级是否存在数据
            bool beIsExistClass = DefaultClassService.GetClasssByTermId(currentTermId).Any();
            if (!beIsExistClass)
            {
                //没有可复制的学期
                throw new BussinessException(ModelType.Timetable, 8);
            }

            //1.2 校验 A学期 是否存在数据，存在则不能复制
            bool isExistClass = DefaultClassService.GetClasssByTermId(toTermId).Any();
            if (isExistClass)
            {
                //已有班级排课数据,不可复制
                throw new BussinessException(ModelType.Timetable, 7);
            }

            //1.3 检查是否有审核中的课表
            TermCourseTimetableAuditService auditService = new TermCourseTimetableAuditService(toTermId);
            if (auditService.IsAuditing || auditService.TblAutAudit != null)
            {
                throw new BussinessException(ModelType.Timetable, 9);
            }
        }
        #endregion

        #region ExistClassCourse 当前学期是否存在排课
        /// <summary>
        /// 当前学期是否存在排课
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-30</para>
        /// </summary>
        /// <returns>true:存在 false:不存在</returns>
        internal bool ExistClassCourse()
        {
            return _classRepository.Value.ExistClass(_termId);
        }
        #endregion

        #region GetTeacherByTermId 获取本学期的上课老师
        /// <summary>
        /// 获取本学期的上课老师
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-30</para>
        /// </summary>
        /// <returns>上课老师</returns>
        public List<ClassTimetableTeacherResponse> GetTeacherByTermId()
        {
            //班级
            List<TblDatClass> classes = DefaultClassService.GetClasssByTermId(this._termId);

            List<string> teacherIds = classes
                .Select(x => x.TeacherId)
                .Distinct()
                .ToList();

            List<ClassTimetableTeacherResponse> allTeacher = TeachService.GetTeachers();

            List<ClassTimetableTeacherResponse> res = allTeacher
                .Where(x => teacherIds.Contains(x.TeacherId))
                .ToList();

            return res;
        }
        #endregion
    }
}
