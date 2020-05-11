using AMS.Anticorrosion.HRS;
using AMS.Core;
using AMS.Dto;
using AMS.Dto.Hss;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jerrisoft.Platform.Public.PageExtensions;
using AMS.Service.Hss;
using Jerrisoft.MsgPush.Sdk;
using Jerrisoft.MsgPush.Core;
using Jerrisoft.MsgPush.Core.Enum;
using AMS.Core.Constants;

namespace AMS.Service
{
    /// <summary>
    /// 学生课表服务
    /// </summary>
    public class StudentTimetableService : BService
    {
        #region  仓储/构造函数
        private Lazy<TblTimLessonStudentRepository> _studentRepository = new Lazy<TblTimLessonStudentRepository>();                                        //学生课次时间仓储
        private readonly Lazy<ViewTimLessonStudentRepository> _viewTimLessonStudentRepository = new Lazy<ViewTimLessonStudentRepository>();          //学生课次仓储
        private readonly Lazy<TblTimLessonRepository> _lessonRepository = new Lazy<TblTimLessonRepository>();                                                                   //课次基础信息仓储
        private readonly Lazy<ViewStudentAttendanceRepository> _attendanceRepository = new Lazy<ViewStudentAttendanceRepository>();                           //学生课程考勤信息仓储
        private Lazy<TblTimReplenishLessonRepository> _replenishRepository = new Lazy<TblTimReplenishLessonRepository>();                              // 补课信息仓储
        private readonly Lazy<ViewTimReplenishLessonStudentRepository> _viewReplenishLessonRepository = new Lazy<ViewTimReplenishLessonStudentRepository>();             // 补课信息视图仓储
        private readonly Lazy<ViewStudentReplenishLessonRepsoitory> _viewStudentReplenishLesson = new Lazy<ViewStudentReplenishLessonRepsoitory>();

        private readonly string _schoolId;
        private readonly long _studentId;

        /// <summary>
        /// 实例化一个学生的课表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        public StudentTimetableService(string schoolId, long studentId)
        {
            this._schoolId = schoolId;
            this._studentId = studentId;
        }

        #endregion

        #region GetStudentClass 获取学生上课班级
        /// <summary>
        /// 获取学生上课班级
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <returns>学生上课的学期数据</returns> 
        internal List<StudentTermResponse> GetStudentClass()
        {
            List<StudentTermResponse> res = new List<StudentTermResponse>();

            //1.获取学生排课课次信息
            List<ViewTimLessonStudent> stuLessonList = _viewTimLessonStudentRepository.Value.GetStudentLessonList(_schoolId, _studentId);
            if (!stuLessonList.Any())
            {
                return res;
            }

            //2.获取学期基本信息
            List<long> termIdList = stuLessonList.Select(x => x.TermId).Distinct().ToList();

            //3.获取所有的学期类型数据
            var termTypeList = new TermTypeService().GetAll();

            //组合学期基础数据
            //获取学期类型名称
            //学生上课的班级信息
            res = TermService.GetTermByTermId(termIdList).Select(term => new StudentTermResponse
            {
                TermId = term.TermId,
                TermName = term.TermName,
                TermTypeId = term.TermTypeId,
                TermTypeName = termTypeList.FirstOrDefault(x => x.TermTypeId == term.TermTypeId)?.TermTypeName,
                Year = term.Year,
                StudentClassList = this.GetStudentClassList(stuLessonList, term.TermId)
            }).ToList();

            return res;
        }

        /// <summary>
        /// 获取学生上课的班级数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-21</para>
        /// </summary>
        /// <param name="stuLessonList">学生的课次信息</param>
        /// <param name="termId">学期Id</param>
        /// <returns>学生上课的班级数据</returns>
        private List<StudentClassResponse> GetStudentClassList(
            IReadOnlyList<ViewTimLessonStudent> stuLessonList,
            long termId)
        {
            //3.获取班级基本信息
            List<long> classIds = stuLessonList.Select(x => x.ClassId).Distinct().ToList();
            List<TblDatClass> classList = DefaultClassService.GetClassByClassIdAsync(classIds).Result;

            var res = (from a in stuLessonList
                       join b in classList on a.ClassId equals b.ClassId
                       where a.TermId == termId
                       select new StudentClassResponse
                       {
                           ClassId = a.ClassId,
                           ClassNo = b.ClassNo
                       })
                       .Distinct(new Compare<StudentClassResponse>((x, y) => x.ClassId == y.ClassId && x.ClassNo == y.ClassNo))
                       .ToList();

            return res;
        }

        /// <summary>
        /// 获取学生转出班级课次数
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-02-22</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <param name="stopTime">停课日期</param>
        /// <param name="lessonTypes">LessonType课次类型枚举值</param>
        /// <returns>转出课次数量</returns>
        internal int GetStudentTransferOutClassLessonCount(long classId, DateTime stopTime, List<int> lessonTypes)
        {
            return _viewTimLessonStudentRepository.Value
                .GetStudentTransferOutClassLessonCount(_schoolId, _studentId, classId, stopTime, lessonTypes);
        }

        /// <summary>
        /// 获取学生转出班级课次列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-22</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <param name="stopTime">停课日期</param>
        /// <param name="lessonTypes">LessonType课次类型枚举值</param>
        /// <param name="unitOfWork">事物单元</param>
        /// <returns>学生课次信息列表</returns>
        internal List<ViewTimLessonStudent> GetStudentTransferOutClassLessonList(long classId, DateTime stopTime, List<int> lessonTypes, UnitOfWork unitOfWork)
        {
            ViewTimLessonStudentRepository repository = unitOfWork.GetCustomRepository<ViewTimLessonStudentRepository, ViewTimLessonStudent>();

            return repository.GetStudentTransferOutClassLessonList(_schoolId, _studentId, classId, stopTime, lessonTypes);
        }

        #endregion

        #region GetHasMakeLessonListAsync 获取学生已排课列表
        /// <summary>
        /// 获取学生已排课列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-07</para>
        /// </summary>
        /// <param name="enrollOrderItemId">报名课程Id</param>
        /// <returns>学生排课列表</returns>
        public async Task<List<StudentMakeLessonResponse>> GetHasMakeLessonListAsync(long enrollOrderItemId)
        {
            List<StudentMakeLessonResponse> res = new List<StudentMakeLessonResponse>();

            //获取学生课次信息
            List<ViewTimLessonStudent> stuLessonList = _viewTimLessonStudentRepository.Value
                .GetStudentLessonList(_schoolId, _studentId, enrollOrderItemId);

            //获取班级信息
            List<TblDatClass> classList = await DefaultClassService.GetClassByClassIdAsync(stuLessonList.Select(x => x.ClassId));

            //获取校区下的教室信息
            List<ViewRoomCourse> classRoomList = ClassRoomService.GetClassRoomBySchoolId(_schoolId);
            //获取所有课程信息
            List<TblDatCourse> courseList = await CourseService.GetAllAsync();
            //获取所有课程等级信息
            List<TblDatCourseLevel> courseLvList = CourseLevelService.GetAll();
            //获取所有老师信息
            List<ClassTimetableTeacherResponse> teachers = TeachService.GetTeachers();

            //写生课Id
            List<long> lifeTimeIdList = stuLessonList
                .Where(x => x.LessonType == (int)LessonType.SketchCourse)
                .Select(x => x.BusinessId)
                .ToList();
            //写生排课信息
            List<TblTimLifeClass> lifeClassList = await new LifeClassService(_schoolId)
                .GetLifeClassListById(lifeTimeIdList);

            foreach (var item in stuLessonList)
            {
                var classInfo = (from a in classList                                                     //班级
                                 join b in courseList on a.CourseId equals b.CourseId                    //课程
                                 join c in courseLvList on a.CourseLeveId equals c.CourseLevelId         //课程等级
                                 join d in classRoomList on a.ClassRoomId equals d.ClassRoomId           //教室
                                 where a.ClassId == item.ClassId
                                 select new { a.ClassNo, b.ShortName, c.LevelCnName, d.RoomNo }).FirstOrDefault();
                if (classInfo == null)
                {
                    continue;
                }

                StudentMakeLessonResponse response = null;

                if (item.LessonType == (int)LessonType.SketchCourse)
                {
                    TblTimLifeClass lifeClass = lifeClassList.FirstOrDefault(x => x.LifeClassId == item.BusinessId);
                    if (lifeClass != null)
                    {
                        response = new StudentMakeLessonResponse
                        {
                            ClassDate = item.ClassDate,
                            Week = WeekDayConvert.DayOfWeekToString(item.ClassDate.DayOfWeek),
                            ClassBeginTime = item.ClassBeginTime,
                            ClassEndTime = item.ClassEndTime,
                            TeacherName = teachers.FirstOrDefault(x => x.TeacherId == item.TeacherId)?.TeacherName,
                            ClassName = lifeClass.Title,
                            LevelName = string.Empty,
                            ClassNo = classInfo.ClassNo,
                            RoomNo = lifeClass.Place
                        };
                    }
                }
                else
                {
                    response = new StudentMakeLessonResponse
                    {
                        ClassDate = item.ClassDate,
                        Week = WeekDayConvert.IntToString((int)item.ClassDate.DayOfWeek),
                        ClassBeginTime = item.ClassBeginTime,
                        ClassEndTime = item.ClassEndTime,
                        TeacherName = teachers.FirstOrDefault(x => x.TeacherId == item.TeacherId)?.TeacherName,
                        ClassName = classInfo.ShortName,
                        LevelName = classInfo.LevelCnName,
                        ClassNo = classInfo.ClassNo,
                        RoomNo = classInfo.RoomNo
                    };
                }

                res.Add(response);
            }
            return res;
        }
        #endregion

        #region  GetLessonStudentList 获取学生考勤信息
        /// <summary>
        /// 获取学生考勤信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="request">学生考勤请求对象</param>
        /// <returns>学生考勤列表</returns>
        public List<StudentTimeLessonResponse> GetLessonStudentList(StudentTimeLessonRequest request)
        {
            var result = new List<StudentTimeLessonResponse>();
            //获取校区学生的考勤信息
            var studentLessonList = _attendanceRepository.Value.GetLessonStudentListAsync(this._schoolId, this._studentId, request).Result;
            if (studentLessonList != null && studentLessonList.Count > 0)
            {
                //按班级名称和课程名称进行排序，生成学生每个班级课程的考勤列表信息
                var classInfoList = studentLessonList.Where(a => !string.IsNullOrEmpty(a.ClassName))
                                                                        .GroupBy(g => new { g.ClassNo, g.ClassName, g.ClassTeacherId })
                                                                        .Select(a => new { a.Key.ClassNo, a.Key.ClassName, a.Key.ClassTeacherId });
                //获取校区的所有老师信息
                var teacherList = EmployeeService.GetAllBySchoolId(this._schoolId);
                foreach (var classInfo in classInfoList)
                {
                    //获取班级课程对应的学生考勤信息
                    var studentLessonInfo = studentLessonList.Where(a => a.ClassNo == classInfo.ClassNo);
                    if (studentLessonInfo.Any())
                    {
                        var studentLesson = new StudentTimeLessonResponse()
                        {
                            ClassNo = classInfo.ClassNo,
                            ClassName = $"{classInfo.ClassName}（{classInfo.ClassNo}）",
                            TeacherName = teacherList.FirstOrDefault(t => t.EmployeeId == classInfo.ClassTeacherId)?.EmployeeName,
                            TotalLessonCount = studentLessonInfo.Sum(a => a.LessonCount),
                            AttendedLessonCount = studentLessonInfo.Where(a => a.AttendStatus == (int)LessonStatus.Attended).Sum(a => a.LessonCount),
                            NormalLessons = GetStudentLessonsByType(studentLessonInfo, LessonType.RegularCourse),            //常规课
                            LifeClassLessons = GetStudentLessonsByType(studentLessonInfo, LessonType.SketchCourse)           //写生课
                        };
                        studentLesson.LeftLessonCount = studentLesson.TotalLessonCount - studentLesson.AttendedLessonCount;//剩余课次
                        result.Add(studentLesson);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 根据课程类型获取学生课程考勤信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="studentLessonList">学生考勤列表信息</param>
        /// <param name="lessonType">课程类型</param>
        /// <returns>常规课/写生课考勤列表</returns>
        private List<StudentTimeLessonListResponse> GetStudentLessonsByType(IEnumerable<ViewStudentAttendance> studentLessonList, LessonType lessonType)
        {
            var classDateInfo = studentLessonList.Where(w => w.LessonType == (int)lessonType)
                .OrderBy(o => o.ClassDate).GroupBy(g => new { g.ClassDate, g.AttendStatus, g.LessonStatus });
            return classDateInfo.Select(a => new StudentTimeLessonListResponse()
            {
                ClassDate = string.Format("{0} {1}（{2}）",
                                                            a.Key.ClassDate.ToString("yyyy-MM-dd"),
                                                            WeekDayConvert.IntToString((int)a.Key.ClassDate.DayOfWeek),
                                                            string.Join("，", studentLessonList.Where(b => b.ClassDate == a.Key.ClassDate).Select(c => c.ClassBeginTime + "-" + c.ClassEndTime).ToList())),
                StudentLessonIds = string.Join(",", studentLessonList.Where(b => b.ClassDate == a.Key.ClassDate).Select(c => c.LessonId)),
                LessonCount = studentLessonList.Where(b => b.ClassDate == a.Key.ClassDate).Sum(c => c.LessonCount),
                AttendStatus = a.Key.AttendStatus ?? 0,
                AttendStatusName = EnumName.GetDescription(typeof(LessonStatus), a.Key.AttendStatus),
                LessonStatus = a.Key.LessonStatus,
                LessonStatusName = EnumName.GetDescription(typeof(LessonStatus), a.Key.LessonStatus)
            }).ToList();
        }
        #endregion

        #region  GetStudentLessonDetailAsync 获取学生考勤详情信息
        /// <summary>
        /// 根据学生课程考勤Id获取学生课程考勤信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="lessonIdList">学生课程考勤Id集合</param>
        /// <returns>学生课程考勤信息</returns>
        public async Task<List<StudentTimeLessonDetailResponse>> GetStudentLessonDetailAsync(List<long> lessonIdList)
        {
            var result = new List<StudentTimeLessonDetailResponse>();
            if (lessonIdList == null || lessonIdList.Count == 0)
            {
                return result;
            }
            //获取校区学生的学生课程考勤信息
            var lessonInfoList = await _attendanceRepository.Value.GetStudentLessonDetailAsync(_schoolId, _studentId, lessonIdList, null, null);
            if (lessonInfoList != null && lessonInfoList.Count > 0)
            {
                //获取老师信息
                var teacherInfoList = TeachService.GetTeachers();

                //常规课信息
                result.AddRange(GetStudentLessonDetail(lessonInfoList.Where(a => a.AdjustType == 0), teacherInfoList));
                //补课/调课信息
                result.AddRange(GetStudentLessonDetail(lessonInfoList.Where(a => a.AdjustType != 0).OrderBy(x => x.ClassDate).ThenBy(y => y.ClassBeginTime), teacherInfoList));
            }
            return result;
        }

        /// <summary>
        /// 获取学生课程考勤详情信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="lessonInfoList">学生考勤信息</param>
        /// <param name="teacherInfoList">老师信息</param>
        /// <returns>学生课程考勤详情信息</returns>
        private List<StudentTimeLessonDetailResponse> GetStudentLessonDetail(IEnumerable<ViewStudentAttendance> lessonInfoList, List<ClassTimetableTeacherResponse> teacherInfoList)
        {
            //课程信息不存在，则直接返回学生课程考勤详情
            if (!lessonInfoList.Any())
            {
                return new List<StudentTimeLessonDetailResponse>();
            }
            var lessonList = lessonInfoList.GroupBy(g => new { g.ClassDate, g.ClassName, g.TeacherId, g.RoomNo, g.AttendStatus, g.AttendDate });
            return lessonList.Select(a => new StudentTimeLessonDetailResponse
            {
                ClassDate = string.Format("{0} {1}（{2}）",
                                                            a.Key.ClassDate.ToString("yyyy-MM-dd"),
                                                            WeekDayConvert.IntToString((int)a.Key.ClassDate.DayOfWeek),
                                                            string.Join("，", lessonInfoList.Where(b => b.ClassDate == a.Key.ClassDate).Select(c => c.ClassBeginTime + "-" + c.ClassEndTime).ToList())),
                ClassName = a.Key.ClassName,
                RoomNo = a.Key.RoomNo,
                TeacherName = teacherInfoList.FirstOrDefault(b => b.TeacherId == a.Key.TeacherId)?.TeacherName,
                Status = a.Key.AttendStatus ?? -1,
                StatusName = EnumName.GetDescription(typeof(LessonStatus), a.Key.AttendStatus),
                AttendDate = a.Key.AttendDate.HasValue ? a.Key.AttendDate.Value.ToString("yyyy.MM.dd HH:mm") : ""
            }).ToList();
        }
        #endregion

        #region GetStudentLessons 获取学生按周、月、学期的课程表--家校互联考勤列表
        /// <summary>
        /// 获取学生按周、月、学期的课程表--家校互联考勤列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <returns>家校互联学生课表列表信息</returns>
        public List<StudentLessonResponse> GetStudentLessons(PeriodType periodType, long? courseId)
        {
            //获取校区当前学期信息
            var termList = TermService.GetFutureTerm(_schoolId, DateTime.Now).Where(a => a.BeginDate <= DateTime.Now);
            if (termList.Any())
            {
                //获取校区学生的当前学期的课程信息
                var studentLessonList = _attendanceRepository.Value.GetLessonStudentListAsync(this._schoolId, this._studentId,
                                                        new StudentTimeLessonRequest() { TermIdList = termList.Select(a => a.TermId).ToList() }).Result;
                if (studentLessonList != null && studentLessonList.Count > 0)
                {
                    //获取校区的所有老师信息
                    var teacherList = EmployeeService.GetAllBySchoolId(this._schoolId);

                    //获取学生上课的最新一节课程
                    var studentLastLesson = studentLessonList.Where(a => DateTime.Parse(a.LessonType == (int)LessonType.RegularCourse
                                                                                                                ? $"{a.ClassDate.ToString("yyyy-MM-dd")} {a.ClassEndTime}"
                                                                                                                : a.ClassEndTime) >= DateTime.Now)
                                                                                        .OrderBy(a => a.ClassDate).ThenBy(b => b.ClassBeginTime).FirstOrDefault();
                    switch (periodType)
                    {
                        case PeriodType.CurrentWeek://本周学生课程
                            return GetStudentLessonOfCurrentWeek(studentLessonList, teacherList, studentLastLesson);
                        case PeriodType.CurrentMonth://本月学生课程
                            return GetStudentLessonOfCurrentMonth(studentLessonList, teacherList, studentLastLesson);
                        case PeriodType.CurrentTerm://本学期学生课程
                            return GetStudentLessonOfCurrentTerm(studentLessonList, teacherList, studentLastLesson, courseId ?? 0);
                        default:
                            return new List<StudentLessonResponse>();
                    }
                }
            }
            return new List<StudentLessonResponse>();
        }

        /// <summary>
        /// 根据学生课程列表、老师信息、最新上课课程，获取本周学生课程信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="studentLessonList">学生课程列表</param>
        /// <param name="teacherList">老师信息列表</param>
        /// <param name="studentLastLesson">最新上课课程</param>
        /// <returns>本周学生课程信息</returns>
        private List<StudentLessonResponse> GetStudentLessonOfCurrentWeek(List<ViewStudentAttendance> studentLessonList,
            List<Anticorrosion.HRS.EmployeeResponse> teacherList,
            ViewStudentAttendance studentLastLesson)
        {
            //获本周的开始日期与结束日期
            DateUtil.GetPeriod(Period.Week, out DateTime startTimeOfCurrentWeek, out DateTime endTimeOfCurrentWeek);
            var studentLessonListOfCurrentWeek = studentLessonList.Where(a => a.ClassDate >= startTimeOfCurrentWeek && a.ClassDate < endTimeOfCurrentWeek);
            return GetStudentLessonDetail(studentLessonListOfCurrentWeek, teacherList, studentLastLesson);
        }

        /// <summary>
        /// 根据学生课程列表、老师信息、最新上课课程，获取本月学生课程信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="studentLessonList">学生课程列表</param>
        /// <param name="teacherList">老师信息列表</param>
        /// <param name="studentLastLesson">最新上课课程</param>
        /// <returns></returns>
        private List<StudentLessonResponse> GetStudentLessonOfCurrentMonth(List<ViewStudentAttendance> studentLessonList,
            List<Anticorrosion.HRS.EmployeeResponse> teacherList,
            ViewStudentAttendance studentLastLesson)
        {
            //获取本月的开始日期和结束日期
            DateUtil.GetPeriod(Period.Month, out DateTime startTimeOfCurrentMonth, out DateTime endTimeOfCurrentMonth);
            var studentLessonListOfCurrentMonth = studentLessonList.Where(a => a.ClassDate >= startTimeOfCurrentMonth && a.ClassDate < endTimeOfCurrentMonth);
            return GetStudentLessonDetail(studentLessonListOfCurrentMonth, teacherList, studentLastLesson);
        }

        /// <summary>
        /// 根据学生课程列表、老师信息、最新上课课程，获取本学期学生课程信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="studentLessonList">学生课程列表</param>
        /// <param name="teacherList">老师信息列表</param>
        /// <param name="studentLastLesson">最新上课课程</param>
        /// <param name="courseId">课程Id</param>
        /// <returns>本学期学生课程信息</returns>
        private List<StudentLessonResponse> GetStudentLessonOfCurrentTerm(List<ViewStudentAttendance> studentLessonList,
            List<Anticorrosion.HRS.EmployeeResponse> teacherList,
            ViewStudentAttendance studentLastLesson,
            long courseId)
        {
            var studentLessonListOfCurrentCourse = studentLessonList.Where(a => a.CourseId == courseId);
            return GetStudentLessonDetail(studentLessonListOfCurrentCourse, teacherList, studentLastLesson);
        }

        /// <summary>
        /// 根据学生课程列表、老师信息、最新上课课程，获取学生课程信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="studentLessonList">学生课程列表</param>
        /// <param name="teacherList">老师信息列表</param>
        /// <param name="studentLastLesson">最新上课课程</param>
        /// <returns>学生课程详情列表</returns>
        private List<StudentLessonResponse> GetStudentLessonDetail(IEnumerable<ViewStudentAttendance> studentLessonList,
            List<Anticorrosion.HRS.EmployeeResponse> teacherList,
            ViewStudentAttendance studentLastLesson)
        {
            //不存在学生课程信息，则直接返回空的学生课程列表
            if (!studentLessonList.Any())
            {
                return new List<StudentLessonResponse>();
            }
            string timeString = string.Empty;
            int timePastFlag = 0;
            return studentLessonList.OrderBy(a => a.ClassDate).ThenBy(b => b.ClassBeginTime).Select(c =>
                {
                    //获取时间字符串格式的上课结束时间
                    timeString = (c.LessonType == (int)LessonType.RegularCourse ? $"{c.ClassDate.ToString("yyyy-MM-dd")} {c.ClassEndTime}" : c.ClassEndTime);
                    //上课结束时间小于当前时间，表示上课时间已经过去，反之则时间还没过去
                    timePastFlag = (DateTime.TryParse(timeString, out DateTime tempTime) && tempTime < DateTime.Now) ? 1 : 0;
                    //返回学生考勤信息
                    return new StudentLessonResponse()
                    {
                        ClassDate = c.ClassDate,
                        ClassName = c.ClassName,
                        LastFlag = (studentLastLesson.StudentLessonId == c.StudentLessonId ? 1 : 0),
                        TimePastFlag = timePastFlag,
                        ClassTime = $"{c.ClassBeginTime}-{c.ClassEndTime}",
                        RoomNo = c.RoomNo,
                        TeacherName = teacherList.FirstOrDefault(t => t.EmployeeId == c.TeacherId)?.EmployeeName
                    };
                }).ToList();
        }
        #endregion

        #region GetStudentClassInfos 获取学生按周、月、学期的课程表--学期与班级信息

        /// <summary>
        /// 获取学生按周、月、学期的课程表--学期与班级信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <returns>家校互联学生课表列表信息</returns>
        public List<StudentTermClassInfoResponse> GetStudentClassInfos()
        {
            var result = new List<StudentTermClassInfoResponse>();
            //获取校区当前学期信息
            var termList = TermService.GetFutureTerm(_schoolId, DateTime.Now).Where(a => a.BeginDate <= DateTime.Now);
            if (termList.Any())
            {
                //获取校区学生的当前学期的课程信息
                var lessonInfoList = _lessonRepository.Value.GetLessonListByTermId(_schoolId, _studentId, termList.Select(a => a.TermId).ToList());
                if (lessonInfoList != null && lessonInfoList.Count > 0)
                {
                    //获取课程信息
                    var courseInfoList = CourseService.GetAllAsync().Result;

                    //获取学期班级信息
                    result = lessonInfoList.GroupBy(g => g.TermId).Select(a => new StudentTermClassInfoResponse()
                    {
                        TermName = termList.FirstOrDefault(b => b.TermId == a.Key)?.TermName,
                        ClassList = lessonInfoList.Where(x => x.LessonType == (int)LessonType.RegularCourse)
                                            .GroupBy(g => new { g.TermId, g.CourseId }).Select(l => new StudentClassInfoResponse()
                                            {
                                                CourseId = l.Key.CourseId,
                                                ClassName = courseInfoList.FirstOrDefault(c => c.CourseId == l.Key.CourseId)?.CourseCnName
                                            }).ToList()
                    }).ToList();
                }
            }
            return result;
        }

        #endregion

        #region  GetStudentAttendanceLessonList 获取缺勤+请假+未上课的课程列表
        /// <summary>
        /// 获取缺勤+请假+未上课的课程列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        /// <param name="teacherId">老师ID</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">当前行</param>
        /// <returns>学生缺勤，请假，未上课列表</returns>
        public PageResult<StudentAttendanceLessonSkResponse> GetStudentAttendanceLessonList(
            string teacherId, int pageIndex, int pageSize)
        {
            PageResult<StudentAttendanceLessonSkResponse> res = new PageResult<StudentAttendanceLessonSkResponse>();
            res.CurrentPage = pageIndex;
            res.PageSize = pageSize;

            int total;//总数

            //获取学生缺勤，请假，未上课列表
            var stuClassList = _viewStudentReplenishLesson.Value.GetList(this._schoolId, this._studentId,
                teacherId, pageIndex, pageSize, out total);

            res.TotalData = total;

            res.Data = new List<StudentAttendanceLessonSkResponse>();

            //获取所有课程信息
            var courseList = CourseService.GetAllAsync().Result;

            //获取班级信息
            var classList = DefaultClassService.GetClassByClassIdAsync(stuClassList.Select(x => x.ClassId)).Result;

            //var teachrList = TeachService.GetTeachers();
            var classRooms = ClassRoomService.GetClassRoomBySchoolId(this._schoolId);

            foreach (var item in stuClassList)
            {
                StudentAttendanceLessonSkResponse stuAttend = new StudentAttendanceLessonSkResponse
                {
                    ClassDate = item.ClassDate.ToString("yyyy-MM-dd"),
                    ClassId = item.ClassId,
                    ClassName = courseList.FirstOrDefault(x => x.CourseId == item.CourseId)?.ClassCnName,
                    ClassNo = classList.FirstOrDefault(x => x.ClassId == item.ClassId)?.ClassNo,
                    ClassTime = item.ClassTime,
                    Status = item.Type,
                    TeacherName = string.Empty, //teachrList.FirstOrDefault(x => x.TeacherId == item.TeacherId)?.TeacherName,
                    Week = WeekDayConvert.DayOfWeekToString(item.ClassDate.DayOfWeek),
                    ClassRoom = classRooms.FirstOrDefault(x => x.ClassRoomId == item.ClassRoomId)?.RoomNo ?? string.Empty
                };

                res.Data.Add(stuAttend);
            }

            return res;
        }
        #endregion

        #region  GetStudentNoAttendance 获取学生未上课的考勤列表--用于家校互联请假
        /// <summary>
        /// 描述：获取学生未上课的考勤列表--用于家校互联请假
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="classDate">上课日期</param>
        /// <returns></returns>
        public async Task<List<StudentAttendanceLeaveLessonResponse>> GetStudentNoAttendance(DateTime classDate)
        {
            //学生未上课的考勤列表
            var result = new List<StudentAttendanceLeaveLessonResponse>();
            //获取学生常规课的课程信息
            var lessonStudentsList = await _attendanceRepository.Value.GetStudentLessonDetailAsync(_schoolId, _studentId, null, classDate, (int)LessonUltimateStatus.Normal);
            if (lessonStudentsList != null && lessonStudentsList.Count > 0)
            {
                var lessonList = AutoMapper.Mapper.Map<List<TimeLessonStudentResponse>>(lessonStudentsList);
                result.AddRange(GetStudentAttendanceLesson(lessonList));
            }
            result = result.OrderBy(a => a.ClassDate).ThenBy(b => b.ClassBeginTime).ToList();
            return result;
        }

        /// <summary>
        /// 获取学生上课课程信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="lessonInfoList">学生课程列表信息</param>
        /// <returns>学生的考勤课次列表信息</returns>
        private List<StudentAttendanceLeaveLessonResponse> GetStudentAttendanceLesson(List<TimeLessonStudentResponse> lessonInfoList)
        {
            string timeString = string.Empty;//获取时间字符串格式的上课结束时间
            DateTime endDate = DateTime.Now;//结束日期
            var teacherInfo = TeachService.GetTeachers();//老师信息
            var classDataList = lessonInfoList.GroupBy(g => new { g.ClassDate, g.ClassName, g.LessonType, g.AttendStatus, g.ClassTeacherId });
            return classDataList.Select(a =>
            {
                //连着上课，取上课最后时间
                var classLessonInfo = lessonInfoList.Where(b => b.ClassDate == a.Key.ClassDate).OrderByDescending(c => c.ClassEndTime).FirstOrDefault();
                timeString = (a.Key.LessonType == (int)LessonType.RegularCourse ? $"{a.Key.ClassDate.ToString("yyyy-MM-dd")} {classLessonInfo?.ClassEndTime}" : classLessonInfo?.ClassEndTime);
                return new StudentAttendanceLeaveLessonResponse()
                {
                    LessonIds = string.Join(",", lessonInfoList.Where(x => x.ClassDate == a.Key.ClassDate).Select(y => y.LessonId)),
                    ClassDate = a.Key.ClassDate.ToString("yyyy-MM-dd"),
                    ClassBeginTime = timeString,
                    ClassName = $"{a.Key.ClassName}（{string.Join("，", lessonInfoList.Where(b => b.ClassDate == a.Key.ClassDate).Select(c => c.ClassBeginTime + "-" + c.ClassEndTime))}）",
                    CourseName = a.Key.ClassName,
                    TimePastFlag = (DateTime.TryParse(timeString, out endDate) && endDate < DateTime.Now) ? 1 : 0,
                    TeacherId = a.Key.ClassTeacherId,
                    TeacherName = teacherInfo.FirstOrDefault(x => x.TeacherId == a.Key.ClassTeacherId)?.TeacherName,
                    AttendStatus = a.Key.AttendStatus == (int)LessonStatus.Normal ? 1 : (a.Key.AttendStatus == (int)LessonStatus.Absent ? 0 : a.Key.AttendStatus)
                };
            }).ToList();
        }

        #endregion

        #region GetNoAttendanceDates 获取可以请假的课次时间集合--用于家校互联请假日历控件
        /// <summary>
        /// 获取几个月之内可以请假的的课次集合--用于家校互联请假日历控件
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="monthsCount">月数</param>
        /// <returns>上课日期集合</returns>
        public List<string> GetNoAttendanceDates(int monthsCount)
        {
            List<string> result = new List<string>();
            DateTime beginDate = DateTime.Now;
            DateTime endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(monthsCount).AddDays(-1);
            //获取学生的课程信息
            var lessonList = _viewTimLessonStudentRepository.Value.GetLessonsByTimeDuration(_schoolId, _studentId, beginDate, endDate);
            if (lessonList != null && lessonList.Count > 0)
            {
                result.AddRange(lessonList.Select(a => a.ClassDate.ToString("yyyy-MM-dd")).ToList());
            }
            result = result.Distinct().OrderBy(o => DateTime.Parse(o)).ToList();
            return result;
        }
        #endregion

        #region  GetStudentNoAttendance 获取学生未上课的考勤列表--用于撤销排课
        /// <summary>
        /// 描述：获取学生未上课的考勤列表--用于撤销排课
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-08 </para>
        /// </summary>
        /// <param name="searcher">撤销排课查询条件</param>
        /// <returns>返回学生未上课的考勤列表</returns>
        public List<StudentAttendanceLessonResponse> GetStudentNoAttendance(CancelMakeLessonSearchRequest searcher)
        {
            // 1、从学生课次时间表中获取学生的课次信息

            var lessonStudentList = new ViewCancelMakeLessonRepository().GetStudentLessonList(_schoolId, searcher.StudentId, searcher.TermId, searcher.CourseId);

            // 2、获取所有的老师信息
            var teacharList = EmployeeService.GetAll();

            // 3、组合撤销排课列表数据
            var studentAttendanceLessonList = (from l in lessonStudentList
                                               join s in teacharList on l.TeacherId equals s.EmployeeId
                                               select new StudentAttendanceLessonResponse
                                               {
                                                   LessonId = l.LessonId,
                                                   ClassTime = l.ClassDate,
                                                   ClassDate = l.ClassDate.ToString("yyyy-MM-dd"),
                                                   ClassBeginTime = GetMakeLessonClassDate(l, lessonStudentList),
                                                   ClassEndTime = l.ClassEndTime,
                                                   ClassId = l.ClassId,
                                                   ClassNo = l.ClassNo,
                                                   CourseId = l.CourseId,
                                                   CourseName = l.CourseName,
                                                   TeacherId = s.EmployeeId,
                                                   Week = WeekDayConvert.IntToString((int)l.ClassDate.DayOfWeek),
                                                   TeacherName = s.EmployeeName,
                                               }).ToList();
            studentAttendanceLessonList = studentAttendanceLessonList.Where((x, i) => studentAttendanceLessonList.FindIndex(z => z.ClassDate == x.ClassDate) == i).ToList();

            return studentAttendanceLessonList;
        }

        /// <summary>
        /// 描述：获取上课时间信息
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-20 </para>
        /// </summary>
        /// <param name="lesson">排课信息</param>
        /// <param name="makeLessonList">学生排课记录集合</param>
        /// <returns>返回学生上课时间</returns>
        private static string GetMakeLessonClassDate(ViewCancelMakeLesson lesson, List<ViewCancelMakeLesson> makeLessonList)
        {
            var classTime = string.Empty;

            if (lesson.ClassNo.Contains("L"))//两节课连着上
            {
                var lessonList = makeLessonList.Where(m => m.ClassDate == lesson.ClassDate && m.ClassId == lesson.ClassId && m.CourseId == lesson.CourseId).ToList();
                classTime = $"{lesson.ClassDate.ToString("yyyy-MM-dd")} {WeekDayConvert.IntToString((int)lesson.ClassDate.DayOfWeek)}（";

                var time = string.Empty;
                foreach (var item in lessonList)
                {
                    time += $"{item.ClassBeginTime}-{item.ClassEndTime}，";
                }

                classTime += $"{time.TrimEnd('，')}）";

            }
            else
            {
                classTime = $"{lesson.ClassDate.ToString("yyyy-MM-dd")} {WeekDayConvert.IntToString((int)lesson.ClassDate.DayOfWeek)}（{lesson.ClassBeginTime} - {lesson.ClassEndTime}）";
            }

            return classTime;
        }




        #endregion

        #region Leave [家校互联]--学生请假
        /// <summary>
        /// 描述：[家校互联]--学生请假
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="lessonDic"></param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：
        /// 51. 课程已结束不能请假
        /// 60. 未找到课次信息
        /// </exception>
        public void Leave(Dictionary<string, string> lessonDic)
        {
            var lessonIdList = new List<long>();
            lessonDic.Keys.ToList().ForEach(a => lessonIdList.AddRange(a.Split(',', StringSplitOptions.RemoveEmptyEntries).Select<string, long>(b => Convert.ToInt64(b))));
            var lessonInfoList = _lessonRepository.Value.GetByLessonId(lessonIdList);
            if (lessonInfoList != null && lessonInfoList.Count > 0)
            {
                List<LessonBusinessType> replenishTypeList = new List<LessonBusinessType>()
                {
                    LessonBusinessType.RepairLesson,//补课
                    LessonBusinessType.AdjustLessonReplenishWeek,//补课周
                    LessonBusinessType.AdjustLessonChange//调课
                };
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.BeginTransaction(System.Data.IsolationLevel.Snapshot);
                        foreach (var lessonIds in lessonDic.Keys)
                        {
                            var idList = lessonIds.Split(',', StringSplitOptions.RemoveEmptyEntries).Select<string, long>(a => Convert.ToInt64(a));
                            var lessons = lessonInfoList.Where(a => idList.Contains(a.LessonId)).ToList();
                            if (lessons.Exists(a => !idList.Any(b => b == a.LessonId)))
                            {
                                throw new BussinessException(ModelType.Timetable, 60);
                            }
                            //更新学生课程的考勤状态为请假，如果更新考勤状态失败，则直接不成功，并回滚事务
                            if (!UpdateLessonAttendStatus(lessons, replenishTypeList, AttendStatus.NotClockIn, AttendStatus.Leave, unitOfWork))
                            {
                                throw new BussinessException(ModelType.Timetable, 51);
                            }
                            else
                            {
                                //推送消息
                                SendMessage(lessonInfoList, lessonDic[lessonIds]);
                            }
                        }
                        unitOfWork.CommitTransaction();
                    }
                    catch
                    {
                        unitOfWork.RollbackTransaction();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 发送通知
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="lessonInfoList">学生课程基础信息</param>
        /// <param name="courseName">课程名称</param>
        public void SendMessage(List<TblTimLesson> lessonInfoList, string courseName)
        {
            //获取学生信息
            var studentInfo = StudentService.GetStudentInfo(this._studentId);

            //发送请假信息微信到家长公众号
            SendWxMessage(lessonInfoList, courseName, studentInfo);

            //发送请假信息到K信通知老师
            SendKpcMessage(lessonInfoList, courseName, studentInfo);
        }

        /// <summary>
        /// 发送请假信息微信到家长公众号
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="lessonInfoList">学生课程基础信息</param>
        /// <param name="courseName">课程名称</param>
        /// <param name="studentInfo">学生信息</param>
        private void SendWxMessage(List<TblTimLesson> lessonInfoList, string courseName, TblCstStudent studentInfo)
        {
            //不存在课程信息，则直接退出
            var lessonInfo = lessonInfoList.FirstOrDefault();
            if (lessonInfo == null)
            {
                return;
            }
            //课室信息
            var classRoomInfo = new ClassRoomService(lessonInfo.ClassRoomId).ClassRoomInfo;

            //发送请假信息
            WxNotifyInDto wxNotify = new WxNotifyInDto
            {
                Data = new List<WxNotifyItemInDto> {
                    new WxNotifyItemInDto{ DataKey="first", Value=string.Format(ClientConfigManager.HssConfig.WeChatTemplateTitle.LessonStudentLeaveNotice,studentInfo.StudentName) },
                    new WxNotifyItemInDto{ DataKey="keyword1", Value=courseName },
                    new WxNotifyItemInDto{ DataKey="keyword2", Value=classRoomInfo.RoomNo },
                    new WxNotifyItemInDto{ DataKey="keyword3",
                        Value = string.Format("{0}（{1}）{2}",lessonInfo.ClassDate.ToString("yyyy-MM-dd"),
                                                            WeekDayConvert.DayOfWeekToString(lessonInfo.ClassDate.DayOfWeek),
                                                            string.Join("，",lessonInfoList.OrderBy(o=>o.ClassBeginDate).Select(a=> $"{a.ClassBeginTime}-{a.ClassEndTime}")))},
                    new WxNotifyItemInDto{ DataKey="remark", Value=WeChatTemplateContentConstants.PleaseNoteReplenishLessonMsg }
                },
                ToUser = StudentService.GetWxOpenId(studentInfo),
                TemplateId = WeChatTemplateConstants.LessonStudentLeaveTemplateId,
                Url = string.Empty
            };

            WxNotifyProducerService.Instance.Publish(wxNotify);
        }

        /// <summary>
        /// 发送请假信息到K信通知老师
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="lessonInfoList">学生课程基础信息</param>
        /// <param name="courseName">课程名称</param>
        /// <param name="studentInfo">学生信息</param>
        private void SendKpcMessage(List<TblTimLesson> lessonInfoList, string courseName, TblCstStudent studentInfo)
        {
            //不存在课程信息，则直接退出
            var lessonInfo = lessonInfoList.FirstOrDefault();
            if (lessonInfo == null)
            {
                return;
            }
            var ret = RabbitMqSdk.SendMessage<Cfp>(new MsgBodyDto
            {
                Msg = KpcTemplateContentConstants.LeaveRemindTitle,
                SystemCode = BusinessConfig.BussinessCode,
                Ext = string.Format(KpcTemplateContentConstants.LeaveRemindContent, studentInfo.StudentName,
                                                lessonInfo.ClassDate.ToString("yyyy-MM-dd"),
                                                WeekDayConvert.DayOfWeekToString(lessonInfo.ClassDate.DayOfWeek),
                                                string.Join("，", lessonInfoList.Select(a => $"{a.ClassBeginTime}-{a.ClassEndTime}")),
                                                courseName),
                MsgType = MsgContentType.Remid,
                ExtType = (int)RemidType.Leave,
                ToUserId = lessonInfo.TeacherId,
                UserId = string.Empty
            });
        }

        /// <summary>
        /// 更新学生课程考勤状态
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="lessonInfoList">课程基础信息</param>
        /// <param name="replenishTypeList">补课/调课/补课周类型集合</param>
        /// <param name="oldAttendStatus">旧的考勤状态</param>
        /// <param name="newAttendStatus">更新的考勤状态</param>
        /// <param name="unitOfWork">事务单元</param>
        /// <returns></returns>
        private bool UpdateLessonAttendStatus(List<TblTimLesson> lessonInfoList, List<LessonBusinessType> replenishTypeList,
            AttendStatus oldAttendStatus, AttendStatus newAttendStatus, UnitOfWork unitOfWork)
        {
            if (unitOfWork != null)
            {
                _replenishRepository = new Lazy<TblTimReplenishLessonRepository>(unitOfWork.GetCustomRepository<TblTimReplenishLessonRepository, TblTimReplenishLesson>);
                _studentRepository = new Lazy<TblTimLessonStudentRepository>(unitOfWork.GetCustomRepository<TblTimLessonStudentRepository, TblTimLessonStudent>);
            }
            //如果课程为补课/补课周/调课，则更新补课表的考勤状态
            if (replenishTypeList.Contains((LessonBusinessType)lessonInfoList.FirstOrDefault().BusinessType))
            {
                return _replenishRepository.Value.UpdateAttendStatus(lessonInfoList.Select(a => a.LessonId), newAttendStatus, oldAttendStatus);
            }
            //否则更新学生课次表的考勤状态
            else
            {
                return _studentRepository.Value.UpdateAttendStatus(lessonInfoList.Select(a => a.LessonId), newAttendStatus, oldAttendStatus);
            }
        }
        #endregion

        #region ReplenishLesson 补课,对缺勤或请假的进行补课
        /// <summary>
        /// 补课,对缺勤或请假的进行补课
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        /// <param name="request">补课,对缺勤或请假的进行补课</param>
        public void ReplenishLesson(AdjustReplenishRequest request)
        {
            AdjustLessonReplenishService service = new AdjustLessonReplenishService(this._schoolId);
            service.Adjust(request);
        }
        #endregion

        #region ChangeLesson 调课,对未上的课次进行调用
        /// <summary>
        /// 调课,对未上的课次进行调用
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="request">调课参数</param>
        public void ChangeLesson(AdjustChangeRequest request)
        {
            AdjustLessonChangeService service = new AdjustLessonChangeService(this._schoolId);
            service.Adjust(request);
        }
        #endregion

        #region GetLeaveSchoolLessonCount 获取休学课次数

        /// <summary>
        /// 获取休学课次数
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-08</para>
        /// </summary>
        /// <param name="stopTime">停课日期</param>
        /// <returns></returns>
        internal List<LeaveSchoolLessonResponse> GetLeaveSchoolLessonCount(DateTime stopTime)
        {
            return _viewTimLessonStudentRepository.Value
                .GetStudentTransferOutLessonCount(_schoolId, _studentId, stopTime);
        }
        #endregion

        #region GetLeaveSchoolLessonsList 获取休学课次

        /// <summary>
        /// 获取休学课次
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-08</para>
        /// </summary>
        /// <param name="leaveTime">休学时间</param>
        /// <returns></returns>
        internal List<ViewTimLessonStudent> GetLeaveSchoolLessonsList(DateTime leaveTime, UnitOfWork unitOfWork)
        {
            ViewTimLessonStudentRepository repository =
                unitOfWork.GetCustomRepository<ViewTimLessonStudentRepository, ViewTimLessonStudent>();

            return repository.GetStudentTransferOutLessonList(_schoolId, _studentId, leaveTime);
        }
        #endregion

        #region GetStudentRemindClassTimes 获取未排课和已排课的剩余课次
        /// <summary>
        /// 获取未排课和已排课的剩余课次
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018年11月14日 </para>
        /// </summary>
        /// <param name="unitOfWork">事务单元</param>
        /// <returns>返回剩余课次数量</returns>
        internal int GetStudentRemindClassTimes(UnitOfWork unitOfWork = null)
        {
            TblOdrEnrollOrderRepository enrollOrderRepository = null;
            TblOdrEnrollOrderItemRepository enrollOrderItemRepository = null;
            ViewTimLessonStudentRepository viewTimLessonStudentRepository = null;
            if (unitOfWork == null)
            {
                enrollOrderRepository = new TblOdrEnrollOrderRepository();
                enrollOrderItemRepository = new TblOdrEnrollOrderItemRepository();
                viewTimLessonStudentRepository = new ViewTimLessonStudentRepository();
            }
            else
            {
                enrollOrderRepository = unitOfWork.GetCustomRepository<TblOdrEnrollOrderRepository, TblOdrEnrollOrder>();
                enrollOrderItemRepository = unitOfWork.GetCustomRepository<TblOdrEnrollOrderItemRepository, TblOdrEnrollOrderItem>();
                viewTimLessonStudentRepository = unitOfWork.GetCustomRepository<ViewTimLessonStudentRepository, ViewTimLessonStudent>();
            }

            // 1、获取这个学生所有校区的订单编号信息
            List<int> orderStatus = new List<int>
            {
                (int)OrderStatus.Refuse,
                (int)OrderStatus.Confirm,
                (int)OrderStatus.Paid,
                (int)OrderStatus.Finish,
            };
            List<long> enrollOrderIds = enrollOrderRepository.GetEnrollOrderByStudentId(_studentId, orderStatus).Result.Select(m => m.EnrollOrderId).ToList();

            // 2、根据该校区订单编号获取报名订单课程信息
            List<TblOdrEnrollOrderItem> enrollOrderItems = enrollOrderItemRepository.GetByOrderId(enrollOrderIds, _schoolId).Result.Where(m => m.Status == (int)OrderItemStatus.Enroll).ToList();

            int totalClass = enrollOrderItems.Select(m => m.ClassTimes).Sum();//总课次
            int classTimesUse = enrollOrderItems.Select(m => m.ClassTimesUse).Sum();//已排课次
            int unscheduled = totalClass - classTimesUse;//统计未排课次

            // 3、获取排课未上课次
            int noClass = viewTimLessonStudentRepository.GetStudentTransferOutLessonList(_schoolId, _studentId, DateTime.Now).Count;//已排课未上课次

            return unscheduled + noClass;//统计该学生的剩余课次(未排课次+未上课次)
        }
        #endregion

        #region GetTimLessonById 根据课次Id查询课次基础信息

        /// <summary>
        /// 根据课次Id查询课次基础信息
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2019年03月11日 </para>
        /// </summary>
        /// <param name="lessonId">课次编号</param>
        /// <returns>返回课次信息</returns>
        internal TblTimLesson GetTimLessonById(long lessonId)
        {
            return _lessonRepository.Value.Load(lessonId);
        }
        #endregion

        #region GetimLessonStudentById 根据课次Id获取学生课次信息
        /// <summary>
        /// 描述：根据课次Id获取学生课次信息
        /// <para>作者：Huang GaoLiang</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="lessonId">课次编号</param>
        /// <returns></returns>
        internal TblTimLessonStudent GetStudentTimLessonByLessId(long lessonId)
        {
            return _studentRepository.Value.GetTblTimLessonStudent(this._schoolId, this._studentId, lessonId);
        }
        #endregion

        #region  ConfirmReplenishLesson 补签家长确认
        /// <summary>
        /// 补签家长确认
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="replenishCode">补签码</param>
        public static void ConfirmReplenishLesson(string replenishCode)
        {
            var lessonStudentRepository = new TblTimLessonStudentRepository();
            var stuAttend = lessonStudentRepository.GetByReplenishCode(replenishCode);
            //考勤表
            if (stuAttend != null)
            {
                if (stuAttend.AdjustType == (int)AdjustType.SUPPLEMENTNOTCONFIRMED)
                {
                    //更新家长确认补签
                    lessonStudentRepository.UpdateAdjustTypeByReplenishCode(
                        replenishCode, AdjustType.SUPPLEMENTNOTCONFIRMED, AdjustType.SUPPLEMENTCONFIRMED);
                }
                else if (stuAttend.AdjustType == (int)AdjustType.SUPPLEMENTCONFIRMED)
                {
                    return;
                }
                else
                {
                    //通知已失效，请联系您孩子的上课老师。谢谢！
                    throw new BussinessException(ModelType.Timetable, 52);
                }
            }
            //补课/调课考勤
            else
            {
                var replenishLessonRepository = new TblTimReplenishLessonRepository();
                var stuAttendReplenish = replenishLessonRepository.GetByReplenishCode(replenishCode);
                if (stuAttendReplenish == null)
                {
                    //通知已失效，请联系您孩子的上课老师。谢谢！
                    throw new BussinessException(ModelType.Timetable, 52);
                }
                else if (stuAttendReplenish.AdjustType == (int)AdjustType.SUPPLEMENTNOTCONFIRMED)
                {
                    //更新家长确认补签
                    replenishLessonRepository.UpdateAdjustTypeByReplenishCode(
                      replenishCode, AdjustType.SUPPLEMENTNOTCONFIRMED, AdjustType.SUPPLEMENTCONFIRMED);
                    //您已确认，请勿重复提交
                    throw new BussinessException(ModelType.Timetable, 53);
                }
                else if (stuAttendReplenish.AdjustType == (int)AdjustType.SUPPLEMENTCONFIRMED)
                {
                    return;
                }
                else
                {
                    //通知已失效，请联系您孩子的上课老师。谢谢！
                    throw new BussinessException(ModelType.Timetable, 52);
                }
            }
        }
        #endregion

        #region  GetStudentAttendLately 获取学生最近要上课的考勤信息
        /// <summary>
        /// 获取学生最近要上课的考勤信息
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-18</para> 
        /// </summary>
        /// <param name="studentId">要查询的一组学生ID</param>
        /// <returns></returns>
        internal static List<ViewCompleteStudentAttendance> GetStudentAttendLately(List<long> studentId)
        {
            ViewCompleteStudentAttendanceRepository repository = new ViewCompleteStudentAttendanceRepository();
            //查询未考勤或者已考勤的数据
            List<int> attendStatus = new List<int> { (int)AttendStatus.NotClockIn, (int)AttendStatus.Normal };
            return repository.SearchStudentAttendLately(studentId, attendStatus);
        }
        #endregion

        #region GetTimMakeLessonById 根据主键编号获取排课记录信息
        /// <summary>
        /// 根据主键编号获取排课记录信息
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-20 </para>
        /// </summary>
        /// <param name="makeLessonId">主键编号</param>
        /// <returns>排课信息</returns>
        internal TblTimMakeLesson GetTimMakeLessonById(long makeLessonId)
        {
            return new TblTimMakeLessonRepository().GetTimMakeLesson(makeLessonId);
        }
        #endregion

    }
}
