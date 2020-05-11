using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Core;
using AMS.Dto;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using Jerrisoft.Platform.Storage;

namespace AMS.Service
{
    /// <summary>
    /// 班级服务
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-02-20</para>
    /// </summary>
    public class DefaultClassService : BaseClassService
    {
        /// <summary>
        /// 常规课
        /// </summary>
        protected override ClassType ClassType => ClassType.Default;


        #region 仓储/服务/属性/构造函数
        private readonly Lazy<TblDatClassRepository> _classRepository = new Lazy<TblDatClassRepository>();
        private readonly Lazy<TblDatCourseRepository> _courseRepository = new Lazy<TblDatCourseRepository>();
        private readonly Lazy<TblTimClassTimeRepository> _classTimeRepository = new Lazy<TblTimClassTimeRepository>();
        private readonly Lazy<TblDatSchoolTimeRepository> _schoolTimeRepository = new Lazy<TblDatSchoolTimeRepository>();
        /// <summary>
        /// 班级Id
        /// </summary>
        private readonly long _classId;

        /// <summary>
        /// 根据班级ID构建一个班级对象
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        public DefaultClassService(long classId)
        {
            this._classId = classId;
        }
        #endregion

        #region TblDatClass 获取班级对象 
        private TblDatClass _tblDatClass;

        /// <summary>
        /// 班级信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        public TblDatClass TblDatClass
        {
            get
            {
                if (_tblDatClass == null)
                {
                    _tblDatClass = this.GetTblDatClass();
                }
                return _tblDatClass;
            }
        }

        /// <summary>
        /// 获取班级信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <returns>班级信息</returns>
        /// <exception cref="BussinessException">
        /// 异常ID：1,异常描述：班级信息为空
        /// </exception>
        private TblDatClass GetTblDatClass()
        {
            return _classRepository.Value.Load(_classId);
        }
        #endregion

        #region ClassSchoolTimes 获取班级上课时间
        private List<TblDatSchoolTime> _tblDatSchoolTimes;
        /// <summary>
        /// 该班级的所有上课时间
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        public List<TblDatSchoolTime> ClassSchoolTimes
        {
            get
            {
                if (_tblDatSchoolTimes == null)
                {
                    _tblDatSchoolTimes = this.GetClassSchoolTimes();
                }
                return _tblDatSchoolTimes;
            }
        }

        /// <summary>
        /// 上课时间段
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <returns>上课时间段集合</returns>
        private List<TblDatSchoolTime> GetClassSchoolTimes()
        {
            //班级关联的时间段Id
            List<TblTimClassTime> tblTimClassTimes = _classTimeRepository.Value.GetByClassId(_classId);

            //获取上课时间基础信息
            List<TblDatSchoolTime> termSchoolTimes = _schoolTimeRepository.Value.GetSchoolTimeByTermId(TblDatClass.TermId);//本学期所有上课时间

            return (from a in tblTimClassTimes
                    join b in termSchoolTimes on a.SchoolTimeId equals b.SchoolTimeId
                    orderby b.WeekDay ascending
                    select b).ToList();
        }

        #endregion

        #region GetClasssByTermId 根据学期Id获取所有班级
        /// <summary>
        /// 根据学期Id获取所有班级
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="termId">学期Id</param>
        /// <returns>班级信息</returns>
        internal static List<TblDatClass> GetClasssByTermId(long termId)
        {
            return new TblDatClassRepository().GetTermIdByClass(termId);
        }

        /// <summary>
        /// 根据学期Id获取班级代码
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-19</para>
        /// </summary>
        /// <param name="termId">学期Id</param>
        /// <returns>班级信息列表</returns>
        public static List<ClassInfoListResponse> GetClassInfoListByTermId(long termId)
        {
            //班级信息
            var classInfoList = new TblDatClassRepository().GetTermIdByClass(termId);
            //课程信息
            var courseInfo = new TblDatCourseRepository().GetCoursesAsync().Result;

            var classCourseInfo = from a in classInfoList
                                  join b in courseInfo on a.CourseId equals b.CourseId
                                  select new ClassInfoListResponse()
                                  {
                                      ClassId = a.ClassId,
                                      ClassNo = a.ClassNo,
                                      ClassName = b.ClassCnName
                                  };

            return classCourseInfo.ToList();
        }
        #endregion

        #region GetClassTimeByClassId 获取班级上课时间段
        /// <summary>
        /// 获取班级上课时间段
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-08</para>
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <returns>班级上课时间表集合</returns>
        internal static List<TblTimClassTime> GetClassTimeByClassId(IEnumerable<long> classId)
        {
            return new TblTimClassTimeRepository().GetByClassId(classId).Result;
        }
        #endregion

        #region DeleteByTermId 根据学期删除班级信息
        /// <summary>
        /// 根据学期删除班级信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="termId">学期</param>
        internal static void DeleteByTermId(long termId)
        {
            new TblDatClassRepository().DeleteByTermId(termId);
        }
        #endregion

        #region GetClassList 查询班级列表
        /// <summary>
        /// 查询班级列表
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="searcher">班级查询条件</param>
        /// <returns>返回班级集合</returns>
        public static List<ClassListResponse> GetClassList(ClassListSearchRequest searcher)
        {
            // 根据条件查询获取此学期的班级信息
            List<TblDatClass> datClassList = new TblDatClassRepository().GetTermIdByClass(searcher.TermId, searcher.CourseId, searcher.CourseLeaveId, searcher.RoomId, ClassType.Default);

            List<ClassListResponse> classListResult = new List<ClassListResponse>();

            // 组合数据
            foreach (TblDatClass datClass in datClassList)
            {
                ClassListResponse classList = new ClassListResponse
                {
                    ClassId = datClass.ClassId,
                    ClassNo = datClass.ClassNo,
                    ClassTime = GetSchoolTime(datClass.ClassId),
                    SurplusNum = datClass.StudentsNum - MakeLessonService.GetClassScheduleNum(datClass.ClassId)
                };
                classListResult.Add(classList);
            }
            return classListResult;
        }
        #endregion

        #region GetClassDetail 根据编辑编号查询班级详情信息

        /// <summary>
        /// 根据班级信息查询班级详情信息
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="classInfo">班级查询信息</param>
        /// <returns>返回班级详情信息</returns>
        public static ClassDetailResponse GetClassDetail(ClassInfo classInfo)
        {
            // 获取当前学期下该编辑编码下的班级信息
            TblDatClassRepository repository = new TblDatClassRepository();
            List<long> termIds = TermService.GetTermByTermTypeId(classInfo.Year, classInfo.TermTypeId, classInfo.SchoolId).Select(t => t.TermId).ToList(); //获取学期类型下的所有学期

            // 根据学期编号、班级编号、校区编号获取班级信息
            TblDatClass datClass = repository.GetByClassNo(termIds, classInfo.classNo, classInfo.SchoolId);
            if (datClass == null)
            {
                return null;
            }
            return GetClassInfo(datClass, classInfo.CompanyId);
        }

        #endregion

        #region GetClassDetail 查询班级详情信息

        /// <summary>
        /// 查询班级详情信息
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="classId">班级编号</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>返回班及详情信息</returns>
        public static ClassDetailResponse GetClassDetail(long classId, string companyId)
        {
            // 获取当前学期下该编辑编码下的班级信息
            TblDatClassRepository repository = new TblDatClassRepository();
            TblDatClass datClass = repository.Load(classId);
            return GetClassInfo(datClass, companyId);
        }


        #endregion

        #region GetClassInfo 获取班级信息
        /// <summary>
        /// 获取班级信息
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="datClass">班级信息</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>返回班级详细信息</returns>
        /// <exception cref="BussinessException">
        /// 异常ID：7,数据异常
        /// </exception> 
        private static ClassDetailResponse GetClassInfo(TblDatClass datClass, string companyId)
        {
            ClassDetailResponse dto = new ClassDetailResponse
            {
                ClassId = datClass.ClassId,
                ClassNo = datClass.ClassNo,
                RoomCourseId = datClass.RoomCourseId,
                ClassRoomId = datClass.ClassRoomId,
                CourseId = datClass.CourseId,
                CourseLeveId = datClass.CourseLeveId,
                TermId = datClass.TermId,
                CourseNum = datClass.CourseNum,
                StudentsNum = datClass.StudentsNum,
                SurplusNum = datClass.StudentsNum,
            };
            if (dto == null)
            {
                throw new BussinessException((byte)ModelType.SignUp, 7);
            }

            //1、课程名称，课程等级
            CourseResponse course = new CourseService(companyId).GetCourseDetailsAsync(dto.CourseId).Result;
            if (course != null)
            {
                dto.CourseName = course.CourseCnName;
                dto.CourseLeveName = course.CourseLevels.FirstOrDefault(m => m.CourseLevelId == dto.CourseLeveId)
                    ?.CourseLevelName;
            }

            dto.TeachName = TeachService.GetTeacher(datClass.TeacherId)?.TeacherName;
            dto.TermName = TermService.GetTermByTermId(dto.TermId)?.TermName;

            //2、门牌号
            dto.ClassRoomName = new ClassRoomService(dto.ClassRoomId).ClassRoomInfo?.RoomNo;

            //、计算出剩余学位数
            dto.SurplusNum = dto.StudentsNum - MakeLessonService.GetClassScheduleNum(dto.ClassId);

            //3、获取上课时间段数据
            dto.SchoolTimeList = GetDatSchoolTimeInfo(dto.ClassId);

            dto.ClassTime = GetSchoolTime(datClass.ClassId);

            return dto;
        }
        #endregion

        #region GetDatSchoolTimeInfo 根据班级编号，获取班级上课时间信息

        /// <summary>
        /// 根据班级编号，获取班级上课时间信息
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-11-02 </para>
        /// </summary>
        /// <param name="classId">班级编号</param>
        /// <returns>返回班级上课信息集合</returns>
        private static List<SchoolTime> GetDatSchoolTimeInfo(long classId)
        {
            List<SchoolTime> tiems = new List<SchoolTime>();

            //1、根据班级编号，获取班级上课时间表中的上课时间段
            List<long> schoolTimeIds = new TimClassTimeService(classId).GetSchoolTimeIds();

            //2、根据上课时间段编号，获取上课时间表
            List<TblDatSchoolTime> schoolTimeList = SchoolTimeService.GetBySchoolTimeIds(schoolTimeIds);

            //3、返回数据
            foreach (TblDatSchoolTime s in schoolTimeList)
            {
                SchoolTime time = new SchoolTime();

                time.SchoolTimeId = s.SchoolTimeId;
                time.WeekDay = s.WeekDay;
                time.BeginTime = s.BeginTime;
                time.EndTime = s.EndTime;
                tiems.Add(time);
            }
            return tiems;
        }

        /// <summary>
        /// 获取拼接好的上课时间数据
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-11-02 </para>
        /// </summary>
        /// <param name="classId">编辑编号</param>
        /// <returns>获取上课时间字符串</returns>
        private static string GetSchoolTime(long classId)
        {
            // 1、根据班级编号，获取班级中的上课时间编号
            List<long> schoolTimeIds = new TimClassTimeService(classId).GetSchoolTimeIds();

            // 2、根据上课时间段编号，获取上课时间表
            List<TblDatSchoolTime> schoolTimeList = SchoolTimeService.GetBySchoolTimeIds(schoolTimeIds).Select(m => new TblDatSchoolTime
            {
                WeekDay = m.WeekDay,
                BeginTime = m.BeginTime,
                EndTime = m.EndTime
            }).ToList();

            // 星期
            string week = string.Join("、", schoolTimeList.Select(m => EnumName.GetDescription(typeof(Week), m.WeekDay)).Distinct());

            schoolTimeList = schoolTimeList.Where((x, i) => schoolTimeList.FindIndex(z => z.BeginTime == x.BeginTime) == i).ToList();

            string classTime = string.Empty;

            foreach (TblDatSchoolTime s in schoolTimeList)
            {
                classTime += $"{s.BeginTime}-{s.EndTime}，";
            }
            classTime = $"{week}（{classTime.TrimEnd('，')}）";

            return classTime;
        }
        #endregion

        #region GetClasssByClassId 根据班级Id获取班级信息
        /// <summary>
        /// 根据班级Id获取班级信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="classId">一组班级Id</param>
        /// <returns>班级信息</returns>
        internal static async Task<List<TblDatClass>> GetClassByClassIdAsync(IEnumerable<long> classId)
        {
            return await new TblDatClassRepository().GetByClassIdAsync(classId);
        }

        /// <summary>
        /// 根据班级Id获取班级信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="classId">一组班级Id</param>
        /// <returns>班级信息</returns>
        internal static List<TblDatClass> GetClassByClassId(IEnumerable<long> classId)
        {
            return new TblDatClassRepository().GetByClassId(classId);
        }
        #endregion

        #region GetMaximumLessonByFirstTime 根据首次上课时间获取排课最大课次

        /// <summary>
        /// 根据首次上课时间获取排课最大课次
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-05</para>
        /// </summary>
        /// <param name="firstTime">首次上课时间</param>
        /// <returns>获取排课最大课次</returns>
        /// <exception cref="BussinessException">
        /// 异常ID：1 异常描述：学期信息为空
        /// </exception>
        public int GetMaximumLessonByFirstTime(DateTime firstTime)
        {
            int maximumLesson = 0;

            //1.获取学期信息
            var termInfo = TermService.GetTermByTermId(TblDatClass.TermId);

            if (termInfo == null)
            {
                throw new BussinessException(ModelType.Default, 1);
            }

            //获取停课日
            List<SchoolHolidayResponse> schoolHoliday = TermService.GetSchoolHoliday(termInfo);

            //首次上课时间大于学期结束时间则没有课次
            if (firstTime > termInfo.EndDate)
            {
                return maximumLesson;
            }

            //首次上课时间小于学期开始时间则以学期开始时间为准
            if (firstTime < termInfo.BeginDate)
            {
                firstTime = termInfo.BeginDate;
            }

            bool isStandard180 = this.IsStandard180(ClassSchoolTimes); //180分钟
            TimeType duration = (TimeType)ClassSchoolTimes.FirstOrDefault().Duration; //时长

            while (firstTime <= termInfo.EndDate)
            {
                //判断停课日
                bool isClosed = SchoolHolidayService.TodayIsSuspendClasses(schoolHoliday, firstTime);
                if (isClosed)
                {
                    firstTime = firstTime.AddDays(1);
                    continue;
                }

                //执行60分钟标准
                if (duration == TimeType.Sixty && maximumLesson == termInfo.Classes60)
                {
                    return maximumLesson;
                }
                //执行90分钟标准
                else if (duration == TimeType.Ninety && !isStandard180 && maximumLesson == termInfo.Classes90)
                {
                    return maximumLesson;
                }
                //执行180分钟标准
                else if (duration == TimeType.Ninety && isStandard180 && maximumLesson >= termInfo.Classes180)
                {
                    //180分钟标准由90分钟标准构成
                    //180分钟标准设置可能会是奇数
                    //最大课次排课超过标准,则按标准执行
                    if (maximumLesson > termInfo.Classes180)
                    {
                        return termInfo.Classes180;
                    }
                    break;
                }

                int weekDay = WeekDayConvert.DayOfWeekToInt(firstTime);

                int lessonCount = ClassSchoolTimes.Count(x => x.WeekDay == weekDay);

                maximumLesson += lessonCount;

                firstTime = firstTime.AddDays(1);
            }

            return maximumLesson;
        }

        /// <summary>
        /// 当前时间段是否为180分钟标准
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="ClassSchoolTimes">上课时间段</param>
        /// <returns>true:是 false:否</returns>
        private bool IsStandard180(List<TblDatSchoolTime> ClassSchoolTimes)
        {
            var first = ClassSchoolTimes.FirstOrDefault();

            //180=当天有两节课连上
            return ClassSchoolTimes.Count(x => x.WeekDay == first.WeekDay) == 2;
        }
        #endregion

        #region GetTransferLessonByFirstTime 获取插班补上本学期已上课程

        /// <summary>
        /// 获取插班补上本学期已上课程数
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-07</para>
        /// </summary>
        /// <param name="firstTime">首次上课时间</param>
        /// <returns>插班补上本学期已上课程数</returns>
        /// <exception cref="BussinessException">
        /// 异常ID：1 异常描述：学期信息为空
        /// </exception>
        public int GetTransferLessonByFirstTime(DateTime? firstTime)
        {
            int transferLesson = 0;

            //1.获取学期信息
            var termInfo = TermService.GetTermByTermId(TblDatClass.TermId);
            if (termInfo == null)
            {
                throw new BussinessException(ModelType.Default, 1);
            }

            if (!firstTime.HasValue)
            {
                firstTime = termInfo.BeginDate;
            }

            //时间不在学期的开始、结束范围类则没有插班补课
            if (firstTime < termInfo.BeginDate || firstTime > termInfo.EndDate)
            {
                return transferLesson;
            }

            //2.获取停课日
            List<SchoolHolidayResponse> schoolHoliday = TermService.GetSchoolHoliday(termInfo);

            while (termInfo.BeginDate < firstTime)
            {
                //今天
                DateTime today = termInfo.BeginDate;

                termInfo.BeginDate = termInfo.BeginDate.AddDays(1);

                bool isClosed = SchoolHolidayService.TodayIsSuspendClasses(schoolHoliday, today);
                if (isClosed)
                {
                    continue;
                }

                int weekDay = WeekDayConvert.DayOfWeekToInt(today);

                var todayClassTimes = ClassSchoolTimes
                    .Where(x => x.WeekDay == weekDay)
                    .OrderBy(x => x.BeginTime)
                    .ToList();
                if (todayClassTimes.Count == 0)
                {
                    continue;
                }

                var firstClassTime = todayClassTimes.FirstOrDefault();

                if (todayClassTimes.Count == 1)
                {
                    switch ((TimeType)firstClassTime.Duration)
                    {
                        case TimeType.Sixty when termInfo.Classes60 <= transferLesson:
                            return transferLesson;
                        case TimeType.Ninety when termInfo.Classes60 <= transferLesson:
                            return transferLesson;
                    }
                }
                else
                {
                    if (termInfo.Classes180 <= transferLesson)
                    {
                        return transferLesson;
                    }
                }

                transferLesson += todayClassTimes.Count;
            }

            return transferLesson;
        }

        #endregion

        #region GetClassListByTeacherId 根据校区编号和老师编号获取老师的班级信息
        /// <summary>
        /// 根据校区编号和老师编号获取老师的班级信息
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <param name="teacherId">老师编号</param>
        /// <returns>返回班级信息</returns>
        internal static List<TblDatClass> GetClassListByTeacherId(string schoolId, string teacherId)
        {
            return new TblDatClassRepository().GetClassListByTeacherId(schoolId, teacherId);
        }
        #endregion
    }
}
