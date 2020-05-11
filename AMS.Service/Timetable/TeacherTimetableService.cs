/****************************************************************************\
所属系统:招生系统
所属模块:课表模块
创建时间:
作   者:
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/
using AMS.Anticorrosion.HRS;
using AMS.Core;
using AMS.Core.Constants;
using AMS.Dto;
using AMS.Service.Hss;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using Jerrisoft.Platform.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMS.Service
{
    /// <summary>
    /// 描述：老师上课课表相关业务
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-08</para>
    /// </summary>
    public class TeacherTimetableService
    {
        //获取老师上课班级仓储
        private readonly Lazy<ViewClassLessonRepository> _viewClassLessonRepository = new Lazy<ViewClassLessonRepository>();

        //统计学生异常状态仓储
        private readonly Lazy<ViewTotalClassStudentAbnormalStateRepository> _totalClassStudentAbnormalStateRepository = new Lazy<ViewTotalClassStudentAbnormalStateRepository>();

        //统计老师上课时间仓储
        private readonly Lazy<ViewTeacherClassDateRepository> _viewTeacherClassDateRepository = new Lazy<ViewTeacherClassDateRepository>();

        //学生考勤仓储
        private readonly Lazy<ViewTimLessonStudentRepository> _viewLessonStudentRepository = new Lazy<ViewTimLessonStudentRepository>();

        //学生调整课次考勤仓储
        private readonly Lazy<ViewTimReplenishLessonStudentRepository> _viewTimReplenishLessonStudentRepository = new Lazy<ViewTimReplenishLessonStudentRepository>();

        //调课、补课仓储
        private readonly Lazy<TblTimReplenishLessonRepository> _replenishLessonRepository = new Lazy<TblTimReplenishLessonRepository>();

        //正常考勤
        private readonly Lazy<TblTimLessonStudentRepository> _lessonStudentRepository = new Lazy<TblTimLessonStudentRepository>();

        private readonly Lazy<ViewCompleteStudentAttendanceRepository> _viewCompleteStudentAttendanceRepository
            = new Lazy<ViewCompleteStudentAttendanceRepository>();

        private readonly string _schoolId;      //校区ID
        private readonly string _teacherId;     //老师ID

        /// <summary>
        /// 老师上课课表服务
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-08</para> 
        /// </summary>
        /// <param name="schoolId">校区ID</param>
        /// <param name="teacherId">老师ID</param>
        public TeacherTimetableService(string schoolId, string teacherId)
        {
            this._schoolId = schoolId;
            this._teacherId = teacherId;
        }

        #region  CancelAttendReplenish 撤回考勤补签
        /// <summary>
        /// 撤回考勤补签
        /// <para>作    者：</para>
        /// <para>创建时间：</para>
        /// <param name="lessonId">课次ID</param>
        public void CancelAttendReplenish(long lessonId)
        {
            var stuLesson = _viewLessonStudentRepository.Value.GetByLessonId(lessonId);

            if (stuLesson != null)
            {
                CancelAttendSign(stuLesson);          //取消补签
            }
            else
            {
                CancelReplenishAttendSign(lessonId); //调课/补课 取消补签
            }

        }

        /// <summary>
        /// 取消考勤补签
        /// </summary>
        private void CancelAttendSign(ViewTimLessonStudent lessonStudent)
        {
            if (lessonStudent == null)
            {
                //未找到考勤信息
                throw new BussinessException(ModelType.Timetable, 48);
            }
            if (!this._teacherId.Equals(lessonStudent.TeacherId, StringComparison.Ordinal))
            {
                //补签老师与上课老师不一致
                throw new BussinessException(ModelType.Timetable, 49);
            }
            if (lessonStudent.AdjustType != (int)AdjustType.SUPPLEMENTNOTCONFIRMED)
            {
                //当前状态不可操作
                throw new BussinessException(ModelType.Timetable, 50);
            }

            //如果是180分钟的课次会有两条课次信息 这里获取下面批量更新
            var lessonList = _viewLessonStudentRepository.Value
                 .GetLessonList(lessonStudent.SchoolId, lessonStudent.ClassId,
                 lessonStudent.ClassDate, lessonStudent.StudentId);

            var stuLessonIdList = lessonList.Select(x => x.LessonStudentId);

            //取消补签
            _lessonStudentRepository.Value.UpdateStuAttend(stuLessonIdList,
                AdjustType.DEFAULT, AttendStatus.NotClockIn,
                DateTime.Now, Guid.NewGuid().ToString().Replace("-", ""));
        }

        /// <summary>
        /// 取消调课、补课的学生考勤补签
        /// </summary>
        private void CancelReplenishAttendSign(long lessonId)
        {
            //确认是否插班补课的考勤
            var stuAdjustLesson = _viewTimReplenishLessonStudentRepository.Value.GetByLessonId(lessonId);

            if (stuAdjustLesson == null)
            {
                //未找到考勤信息
                throw new BussinessException(ModelType.Timetable, 48);
            }
            if (!this._teacherId.Equals(stuAdjustLesson.TeacherId, StringComparison.Ordinal))
            {
                //补签老师与上课老师不一致
                throw new BussinessException(ModelType.Timetable, 49);
            }
            if (stuAdjustLesson.AdjustType != (int)AdjustType.SUPPLEMENTNOTCONFIRMED)
            {
                //当前状态不可补签
                throw new BussinessException(ModelType.Timetable, 50);
            }

            //如果是180分钟的课次会有两条课次信息 这里获取下面批量更新
            var lessonList = _viewTimReplenishLessonStudentRepository.Value
                 .GetLessonList(stuAdjustLesson.SchoolId, stuAdjustLesson.ClassId,
                 stuAdjustLesson.ClassDate, stuAdjustLesson.StudentId);

            var stuLessonIdList = lessonList.Select(x => x.LessonStudentId);

            //更新插班补课考勤课次
            _replenishLessonRepository.Value.UpdateStuAttend(stuLessonIdList,
                AdjustType.DEFAULT, AttendStatus.NotClockIn,
                DateTime.Now, Guid.NewGuid().ToString().Replace("-", ""));
        }

        #endregion

        #region GetClassLessons 获取老师要上课的班级信息
        /// <summary>
        /// 获取老师要上课的班级信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-08</para>
        /// </summary>
        /// <param name="classDate">上课日期</param>
        /// <returns>老师上课班级列表</returns>
        public List<ClassLessonResponse> GetClassLessons(DateTime classDate)
        {
            var abnormalStates = _totalClassStudentAbnormalStateRepository.Value.Get(this._schoolId, this._teacherId, classDate);

            List<ClassLessonResponse> res = _viewClassLessonRepository
                .Value.GetClassLessons(this._schoolId, this._teacherId, classDate)
                .Select(x => new ClassLessonResponse
                {
                    ClassId = x.ClassId,
                    ClassName = x.ClassName,
                    ClassRoom = x.ClassRoom,
                    ClassTime = ConvClassTime(x.ClassTime, x.LessonType),
                    LessonType = (LessonType)x.LessonType,
                    ClassDate = x.ClassDate.ToString("yyyy-MM-dd"),
                    Week = WeekDayConvert.DayOfWeekToString(x.ClassDate.DayOfWeek),
                    ClassNo = x.ClassNo
                }).OrderBy(x => x.ClassTime).ToList();

            res.ForEach(x =>
            {
                x.Mark = false;
                var temp = abnormalStates.FirstOrDefault(m => m.LessonType == (int)x.LessonType && m.ClassId == x.ClassId);
                if (temp != null && temp.Total > 0)
                {
                    x.Mark = true;
                }
            });

            return res;
        }
        #endregion

        /// <summary>
        /// 写生课次上课时间需要处理一下
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-22</para>
        /// </summary>
        /// <param name="classTime">上课时间</param>
        /// <param name="lessonType">课次类型</param>
        /// <returns>处理的时间</returns>
        private static string ConvClassTime(string classTime, int lessonType)
        {
            if (lessonType != (int)LessonType.SketchCourse)
            {
                return classTime.Trim();
            }

            string[] arr = classTime.Split('-');
            DateTime sTime = DateTime.Parse(arr[0]);
            DateTime eTime = DateTime.Parse(arr[1]);

            if (!sTime.ToString("yyyyMMdd").Equals(eTime.ToString("yyyyMMdd"), StringComparison.OrdinalIgnoreCase))
            {
                return $"{sTime:HH:mm}-{eTime:yyyy.MM.dd HH:mm}";
            }

            return $"{sTime:HH:mm}-{eTime:HH:mm}";
        }

        #region GetClassStudentLessons 获取班级下的学生列表
        /// <summary>
        /// 获取班级下的学生列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="classId">上课班级</param>
        /// <param name="classTime">上课时间</param>
        /// <param name="lessonType">课次类型</param>
        /// <param name="teacherId">老师Id</param>
        /// <returns>班级下学生列表</returns>
        public ClassStudentLessonResponse GetClassStudentLessons(
            long classId, LessonType lessonType, DateTime classTime, string teacherId)
        {
            //根据课次类型返回相应的学生列表
            switch (lessonType)
            {
                case LessonType.RegularCourse:  //常规课
                    return this.GetCourseStudentList(classId, classTime, LessonType.RegularCourse, teacherId);
                case LessonType.SketchCourse:   //写生课
                    return this.GetCourseStudentList(classId, classTime, LessonType.SketchCourse, teacherId);
                default:
                    return new ClassStudentLessonResponse();
            }
        }

        /// <summary>
        /// 获取课程学生列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <param name="date">日期</param>
        /// <param name="lessonType">课次类型</param>
        /// <param name="teacherId">老师Id</param>
        /// <returns>学生列表</returns>
        private ClassStudentLessonResponse GetCourseStudentList(long classId, DateTime date, LessonType lessonType,
            string teacherId)
        {
            List<int> adjustTypes = new List<int> {
                 (int)AdjustType.DEFAULT,                  //默认
                 (int)AdjustType.MAKEUP,                   //补课
                 (int)AdjustType.SUPPLEMENTARYWEEK,        //补课周补课
                 (int)AdjustType.SUPPLEMENTCONFIRMED,      //补课已确认
                 (int)AdjustType.SUPPLEMENTNOTCONFIRMED,   //补课未确认
            };

            var lessonList = _viewCompleteStudentAttendanceRepository.Value
                .GetLessonList(this._schoolId, classId, date, lessonType, adjustTypes, teacherId);

            ClassStudentLessonResponse res = new ClassStudentLessonResponse
            {
                //1=正常考勤 2=调课/补课 考勤
                CanAttend = GetCanAttend(lessonList),

                //正常上课安排学生列表
                Students = this.GetLessonStudentList(lessonList, 1),

                //插班补课安排学生列表
                TransferStudents = this.GetLessonStudentList(lessonList, 2)
            };

            return res;
        }

        /// <summary>
        /// 获取当前班级是否可以开始考勤了
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="attendances">考勤列表</param>
        /// <returns>是/否</returns>
        private static bool GetCanAttend(IReadOnlyList<ViewCompleteStudentAttendance> attendances)
        {
            bool res = false;
            LessonType lessonType = (LessonType)attendances.FirstOrDefault().LessonType;
            //获取当前时间考勤
            DateTime now = DateTime.Now;

            if (lessonType == LessonType.RegularCourse)
            {
                res = attendances
                     .Where(x => StudentScanAttendService.ConvDateTime(x.ClassDate, x.ClassBeginTime, true)
                                 <= now && now <=
                                 StudentScanAttendService.ConvDateTime(x.ClassDate, x.ClassEndTime, false)).Any();
            }
            else
            {
                res = attendances
                     .Where(x => DateTime.Parse(x.ClassBeginTime)
                                 <= now && now <=
                                 DateTime.Parse(x.ClassEndTime)).Any();
            }

            return res;
        }

        /// <summary>
        /// 获取正常安排上课学生列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="attendances">考勤列表</param>
        /// <param name="tab">正常考勤表标识</param>
        /// <returns>正常安排上课学生列表</returns>
        private List<ClassStudent> GetLessonStudentList(IReadOnlyList<ViewCompleteStudentAttendance> attendances, int tab)
        {
            var stuLessonList = attendances.Where(x => x.TblType == tab)
               .Select(x => new StudentLesson
               {
                   AdjustType = x.AdjustType,
                   AttendDate = x.AttendDate,
                   AttendStatus = x.AttendStatus,
                   ClassBeginTime = x.ClassBeginTime,
                   ClassDate = x.ClassDate,
                   ClassEndTime = x.ClassEndTime,
                   LessonId = x.LessonId,
                   LessonType = x.LessonType,
                   SchoolId = x.SchoolId,
                   StudentId = x.StudentId
               });

            return this.GetClassStudentList(stuLessonList);
        }

        /// <summary>
        /// 获取上课学生列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="stuLessonList">学生课次信息</param>
        /// <returns>学生列表</returns>
        private List<ClassStudent> GetClassStudentList(IEnumerable<StudentLesson> stuLessonList)
        {
            List<ClassStudent> res = new List<ClassStudent>();

            //2.筛选学生ID
            var stuIdList = stuLessonList.Select(x => x.StudentId).Distinct().ToList();

            //3.获取学生信息
            var studentList = StudentService.GetStudentByIds(stuIdList)
                .Result
                .OrderBy(x => x.StudentName)
                .ToList();

            // 0=未考勤
            // 1=正常签到 显示考勤时间
            // 2=补签未确认(缺勤) 显示撤回补签
            // 3=补签已确认 显示考勤时间
            // 4=请假(当前时间之前) 判断AdjustStatus是否已安排补课
            // 5=缺勤 判断AdjustStatus是否已安排补课
            // 6=请假(当前时间之后) 无需判断AdjustStatus是否已安排补课 显示 --
            // 7=补签未确认(请假) 显示撤回补签

            //4.生成考勤状态
            foreach (var stu in studentList)
            {
                var stuLesson = stuLessonList.FirstOrDefault(x => x.StudentId == stu.StudentId);
                if (stuLesson == null)
                {
                    continue;
                }

                ClassStudent student = new ClassStudent
                {
                    StudentId = stu.StudentId,
                    StudentName = stu.StudentName,
                    LessonId = stuLesson.LessonId,
                    AttendStatus = 0,
                    AdjustStatus = false
                };

                AdjustType stuAdjustType = (AdjustType)stuLesson.AdjustType;
                AttendStatus stuAttendStatus = (AttendStatus)stuLesson.AttendStatus;

                //上课结束时间+缓冲时间
                DateTime eTime;
                if (stuLesson.LessonType == (int)LessonType.RegularCourse)
                {
                    //上课结束时间+缓冲时间
                    eTime = DateTime.Parse(stuLesson.ClassDate.ToString("yyyy-MM-dd") + " " + stuLesson.ClassEndTime);
                }
                else
                {
                    eTime = DateTime.Parse(stuLesson.ClassEndTime);
                }

                //0=未考勤
                if (DateTime.Now < eTime)
                {
                    student.AttendStatus = 0;
                }

                //1=正常签到 显示考勤时间
                if (stuAttendStatus == AttendStatus.Normal)
                {
                    student.AttendStatus = 1;
                    if (stuLesson.AttendDate.Value.Date == stuLesson.ClassDate.Date)
                    {
                        student.AttendTime = stuLesson.AttendDate.Value.ToString("HH:mm");
                    }
                    else
                    {
                        student.AttendTime = stuLesson.AttendDate.Value.ToString("yyyy-MM-dd HH:mm");
                    }
                }

                //2=补签未确认(缺勤) 显示撤回补签
                if (stuAttendStatus != AttendStatus.Leave && stuAdjustType == AdjustType.SUPPLEMENTNOTCONFIRMED)
                {
                    student.AttendStatus = 2;
                }

                //3=补签已确认 显示考勤时间
                else if (stuAdjustType == AdjustType.SUPPLEMENTCONFIRMED)
                {
                    student.AttendStatus = 3;
                    if (stuLesson.AttendDate.Value.Date == stuLesson.ClassDate.Date)
                    {
                        student.AttendTime = stuLesson.AttendDate.Value.ToString("HH:mm");
                    }
                    else
                    {
                        student.AttendTime = stuLesson.AttendDate.Value.ToString("yyyy-MM-dd HH:mm");
                    }
                }

                //4=请假(当前时间之前) 判断AdjustStatus是否已安排补课
                else if (eTime < DateTime.Now && stuAttendStatus == AttendStatus.Leave)
                {
                    student.AttendStatus = 4;
                    if (stuLesson.AdjustType == (int)AdjustType.MAKEUP ||
                        stuLesson.AdjustType == (int)AdjustType.SUPPLEMENTARYWEEK)
                    {
                        student.AdjustStatus = true;
                    }
                }

                //5=缺勤(当前时间之后) 判断AdjustStatus是否已安排补课
                else if (DateTime.Now > eTime && stuAttendStatus == AttendStatus.NotClockIn)
                {
                    student.AttendStatus = 5;
                    if (stuAdjustType == AdjustType.MAKEUP ||
                        stuAdjustType == AdjustType.SUPPLEMENTARYWEEK)
                    {
                        student.AdjustStatus = true;
                    }
                }

                //6=请假(当前时间之后) 无需判断AdjustStatus是否已安排补课 显示 --
                else if (eTime > DateTime.Now && stuAttendStatus == AttendStatus.Leave)
                {
                    student.AttendStatus = 6;
                }

                //7=补签未确认(请假) 显示撤回补签
                else if (stuAttendStatus == AttendStatus.Leave && stuAdjustType == AdjustType.SUPPLEMENTNOTCONFIRMED)
                {
                    student.AttendStatus = 7;
                }

                res.Add(student);
            }

            return res;
        }

        /// <summary>
        /// 学生课次
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        private class StudentLesson
        {
            /// <summary>
            /// 主健(课次基础信息表)课次ID
            /// </summary>
            public long LessonId { get; set; }

            /// <summary>
            /// 所属校区
            /// </summary>
            public string SchoolId { get; set; }

            /// <summary>
            /// 学生ID
            /// </summary>
            public long StudentId { get; set; }

            /// <summary>
            /// 上课日期
            /// </summary>
            public DateTime ClassDate { get; set; }

            /// <summary>
            /// 上课时间
            /// </summary>
            public string ClassBeginTime { get; set; }

            /// <summary>
            /// 下课时间
            /// </summary>
            public string ClassEndTime { get; set; }

            /// <summary>
            /// 课次类型
            /// </summary>
            public int LessonType { get; set; }

            /// <summary>
            /// 考勤状态(1正常 2请假)
            /// </summary>
            public int AttendStatus { get; set; }

            /// <summary>
            /// 1已经安排补课2已安排调课3已经安排补课周补课 4补签未确认5补签已确认
            /// </summary>
            public int AdjustType { get; set; }

            /// <summary>
            /// 考勤时间
            /// </summary>
            public DateTime? AttendDate { get; set; }

        }
        #endregion

        #region ScanCodeAttend 扫码考勤
        /// <summary>
        /// 扫码考勤
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-03</para>
        /// </summary>
        /// <returns>扫码是否成功信息,是否存在多个课次让老师选择?</returns>
        public StudentScanCodeAttendResponse ScanCodeAttend(StudentScanCodeAttendRequest request)
        {
            StudentScanAttendService service = new StudentScanAttendService(this._schoolId, this._teacherId, request.StudentId);
            return service.ScanCode(request.ClassIds);
        }
        #endregion

        #region SignInAttendReplenish 考勤补签到 
        /// <summary>
        /// 描述：考勤补签到
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-12</para>
        /// <param name="lessonId">课次ID</param>
        /// </summary>
        public void SignInAttendReplenish(long lessonId)
        {
            //确认是否正常安排的考勤
            var stuLesson = _viewLessonStudentRepository.Value.GetByLessonId(lessonId);

            if (stuLesson != null)
            {
                UpdateStuAttend(stuLesson);          //正常安排考勤补签
            }
            else
            {
                UpdateStuReplenishAttend(lessonId); //调课/补课考勤补签
            }
        }

        /// <summary>
        /// 正常考勤补签
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="lessonStudent">学生考勤</param>
        /// <exception cref="BussinessException">
        /// 异常ID：48 异常描述：未找到考勤信息
        /// 异常ID：49 异常描述：补签老师与上课老师不一致
        /// 异常ID：50 异常描述：当前状态不可补签
        /// </exception>
        private void UpdateStuAttend(ViewTimLessonStudent lessonStudent)
        {
            if (lessonStudent == null)
            {
                //未找到考勤信息
                throw new BussinessException(ModelType.Timetable, 48);
            }

            //校验是否绑定家校互联
            this.VerifyStudentBindWeiXin(lessonStudent.StudentId);

            if (!this._teacherId.Equals(lessonStudent.TeacherId, StringComparison.Ordinal))
            {
                //补签老师与上课老师不一致
                throw new BussinessException(ModelType.Timetable, 49);
            }
            if (lessonStudent.AttendStatus == (int)AttendStatus.Normal)
            {
                //当前状态不可补签
                throw new BussinessException(ModelType.Timetable, 50);
            }
            if (lessonStudent.AdjustType != (int)AdjustType.DEFAULT)
            {
                //当前状态不可补签
                throw new BussinessException(ModelType.Timetable, 50);
            }

            //如果是180分钟的课次会有两条课次信息 这里获取下面批量更新
            var lessonList = _viewLessonStudentRepository.Value
                 .GetLessonList(lessonStudent.SchoolId, lessonStudent.ClassId,
                 lessonStudent.ClassDate, lessonStudent.StudentId);

            var stuLessonIdList = lessonList.Select(x => x.LessonStudentId);

            string replenishCode = Guid.NewGuid().ToString().Replace("-", "");

            //更新插班补课考勤课次
            _lessonStudentRepository.Value.UpdateStuAttend(stuLessonIdList,
                AdjustType.SUPPLEMENTNOTCONFIRMED, AttendStatus.NotClockIn,
                DateTime.Now, replenishCode);

            //微信通知

            //班级名称
            string className = CourseService.GetByCourseId(lessonStudent.CourseId)?.ClassCnName ?? string.Empty;

            //上课时间段
            var classTimeList = lessonList.Select(x => x.ClassBeginTime + "-" + x.ClassEndTime);
            string classTime = $"{lessonStudent.ClassDate.ToString("yyyy.MM.dd")} {string.Join("、", classTimeList)}";

            //教室
            var roomService = new ClassRoomService(lessonStudent.ClassRoomId);
            string classRoom = roomService.ClassRoomInfo?.RoomNo ?? string.Empty;

            //推送
            this.PushWeChatNotice(lessonStudent.StudentId, className, classRoom, classTime, replenishCode);
        }


        /// <summary>
        /// 校验学生是否绑定公众号
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <exception cref="BussinessException">
        /// 异常ID:38 ->学生未绑定家校互联
        /// </exception>
        private void VerifyStudentBindWeiXin(long studentId)
        {
            List<string> phones = StudentService.GetMobiles(studentId);
            PassportService service = new PassportService();
            bool isMobileBind = service.IsMobileBind(phones);
            if (!isMobileBind)
            {
                //未绑定家校互联
                throw new BussinessException(ModelType.Timetable, 69);
            }
        }

        /// <summary>
        /// 插班补课考勤补签
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="lessonId">课次ID</param>
        /// <exception cref="BussinessException">
        /// 异常ID：48 异常描述：未找到考勤信息
        /// 异常ID：49 异常描述：补签老师与上课老师不一致
        /// 异常ID：50 异常描述：当前状态不可补签
        /// </exception>
        private void UpdateStuReplenishAttend(long lessonId)
        {
            //确认是否插班补课的考勤
            var stuAdjustLesson = _viewTimReplenishLessonStudentRepository.Value.GetByLessonId(lessonId);

            if (stuAdjustLesson == null)
            {
                //未找到考勤信息
                throw new BussinessException(ModelType.Timetable, 48);
            }

            //校验是否绑定家校互联
            this.VerifyStudentBindWeiXin(stuAdjustLesson.StudentId);

            if (!this._teacherId.Equals(stuAdjustLesson.TeacherId, StringComparison.Ordinal))
            {
                //补签老师与上课老师不一致
                throw new BussinessException(ModelType.Timetable, 49);
            }
            if (stuAdjustLesson.AttendStatus != (int)AttendStatus.NotClockIn)
            {
                //当前状态不可补签
                throw new BussinessException(ModelType.Timetable, 50);
            }

            //如果是180分钟的课次会有两条课次信息 这里获取下面批量更新
            var lessonList = _viewTimReplenishLessonStudentRepository.Value
                 .GetLessonList(stuAdjustLesson.SchoolId, stuAdjustLesson.ClassId,
                 stuAdjustLesson.ClassDate, stuAdjustLesson.StudentId);

            var stuLessonIdList = lessonList.Select(x => x.LessonStudentId);

            string replenishCode = Guid.NewGuid().ToString().Replace("-", ""); //补签码

            //更新插班补课考勤课次
            _replenishLessonRepository.Value.UpdateStuAttend(stuLessonIdList,
                AdjustType.SUPPLEMENTNOTCONFIRMED, AttendStatus.NotClockIn,
                DateTime.Now, replenishCode);

            //微信通知

            //班级名称
            string className = CourseService.GetByCourseId(stuAdjustLesson.CourseId)?.ClassCnName ?? string.Empty;

            //上课时间段
            var classTimeList = lessonList.Select(x => x.ClassBeginTime + "-" + x.ClassEndTime);
            string classTime = $"{stuAdjustLesson.ClassDate.ToString("yyyy.MM.dd")} {string.Join("、", classTimeList)}";

            //教室
            var roomService = new ClassRoomService(stuAdjustLesson.ClassRoomId);
            string classRoom = roomService.ClassRoomInfo?.RoomNo ?? string.Empty;

            //推送
            this.PushWeChatNotice(stuAdjustLesson.StudentId, className, classRoom, classTime, replenishCode);
        }

        /// <summary>
        /// 补签-微信通知
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="className">班级名称</param>
        /// <param name="classRoom">教室</param>
        /// <param name="classTime">上课时间</param>
        /// <param name="replenishCode">补签码</param>
        /// <param name="studentId">学生Id</param>
        /// 
        private void PushWeChatNotice(long studentId, string className,
            string classRoom, string classTime, string replenishCode)
        {
            //获取学生信息
            var stuInfo = StudentService.GetStudentInfo(studentId);
            //微信推送标题
            string title = string.Format(ClientConfigManager.HssConfig.WeChatTemplateTitle.SignReplenishNotice, stuInfo.StudentName);
            //家长确认补签地址
            string signUrl = ClientConfigManager.HssConfig.ParentsConfirmSignUrl + "?replenishCode=" + replenishCode;

            WxNotifyProducerService wxNotifyProducerService = WxNotifyProducerService.Instance;
            WxNotifyInDto wxNotify = new WxNotifyInDto
            {
                Data = new List<WxNotifyItemInDto> {
                    new WxNotifyItemInDto{ DataKey="first", Value=title},
                    new WxNotifyItemInDto{ DataKey="keyword1", Value=className },
                    new WxNotifyItemInDto{ DataKey="keyword2", Value=classRoom },
                    new WxNotifyItemInDto{ DataKey="keyword3", Value=classTime },
                    new WxNotifyItemInDto{ DataKey="remark", Value=ScanCodeAttendConstants.Msg ,Color="#FF2500" }
                },
                ToUser = StudentService.GetWxOpenId(stuInfo),
                TemplateId = WeChatTemplateConstants.SignReplenishNotice,
                Url = signUrl
            };

            wxNotifyProducerService.Publish(wxNotify);
        }
        #endregion

        #region  GetTeacherClassTimetable 获取老师上课的课表信息
        /// <summary>
        /// 描述：获取老师上课的课表信息
        /// 调程信息调整功能使用
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-3-7</para>
        /// </summary>
        /// <param name="request">老师上课列表筛选条件</param>
        /// <returns>老师的未上课课次列表</returns>
        public List<TeacherClassTimetableResponse> GetTeacherClassTimetable(TeacherClassTimetableRequest request)
        {
            var teacherAttendLessonRepository = new ViewTeacherClassTimetableRepository();
            var result = teacherAttendLessonRepository.GetTeacherClassTimetable(this._schoolId, request);

            var teacherTimetableList = AutoMapper.Mapper.Map<List<TeacherClassTimetableResponse>>(result);
            foreach (var item in teacherTimetableList)
            {
                string[] weekdays = { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
                item.Week = weekdays[Convert.ToInt32(item.ClassDate.DayOfWeek)];
            }

            return teacherTimetableList;
        }
        #endregion

        #region  GetTeacherClassDates 获取老师在指定时间段内的上课日期集合
        /// <summary>
        /// 获取老师在指定时间段内的上课日期集合
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="beginDate">开始时间段</param>
        /// <param name="endDate">结束时间段</param>
        /// <returns>上课日期集合</returns>
        public List<TeacherClassDateResponse> GetTeacherClassDates(DateTime beginDate, DateTime endDate)
        {
            return _viewTeacherClassDateRepository.Value
                .Get(this._schoolId, this._teacherId, beginDate, endDate)
                .Select(x => new TeacherClassDateResponse
                {
                    ClassDate = x.ClassDate.ToString("yyyy-MM-dd"),
                    Mark = x.Mark > 0
                }).ToList();
        }
        #endregion

        #region GetStudentReplenishLessons 获取已安排补课或调课的详情 
        /// <summary>
        /// 获取已安排补课或调课的详情
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <param name="lessonId">课次ID</param>
        /// <returns>调课详情/补课详情</returns>
        public StudentReplenishLessonsResponse GetStudentReplenishLessons(long studentId, long lessonId)
        {
            var studentAttendance = _viewCompleteStudentAttendanceRepository.Value.GetLesson(lessonId);

            if (studentAttendance == null)
            {
                //未找到该学生的补课信息
                throw new BussinessException(ModelType.Timetable, 47);
            }

            if (studentAttendance.StudentId != studentId)
            {
                //未找到该学生的补课信息
                throw new BussinessException(ModelType.Timetable, 47);
            }

            //获取一个班级学生的课次信息
            var stuDayAttendances = _viewCompleteStudentAttendanceRepository.Value.GetStudetnDayLessonList(
                 studentAttendance.SchoolId, studentAttendance.ClassId, studentAttendance.ClassDate,
                 studentId, LessonType.RegularCourse);

            var lessonIds = stuDayAttendances.Select(x => x.LessonId).ToList();
            var stuLessons = _viewTimReplenishLessonStudentRepository.Value.GetLessonListByParentLessonId(lessonIds);

            if (stuLessons == null || !stuLessons.Any())
            {
                //未找到该学生的补课信息
                throw new BussinessException(ModelType.Timetable, 47);
            }

            var stuLesson = stuLessons.OrderBy(x => x.ClassBeginTime).FirstOrDefault();

            string classTime = string.Join(" ", stuLessons.Select(x => x.ClassBeginTime + "-" + x.ClassEndTime));

            var classInfo = new DefaultClassService(stuLesson.ClassId).TblDatClass;

            StudentReplenishLessonsResponse res = new StudentReplenishLessonsResponse
            {
                ClassId = stuLesson.ClassId,
                ClassName = CourseService.GetByCourseId(stuLesson.CourseId)?.ClassCnName,
                ClassRoom = new ClassRoomService(stuLesson.ClassRoomId).ClassRoomInfo?.RoomNo,
                ClassTime = classTime,
                ClassDate = stuLesson.ClassDate.ToString("yyyy.MM.dd"),
                ClassNo = new DefaultClassService(stuLesson.ClassId).TblDatClass?.ClassNo,
                TeacherName = TeachService.GetTeacher(stuLesson.TeacherId)?.TeacherName,
                Week = WeekDayConvert.DayOfWeekToString(stuLesson.ClassDate.DayOfWeek)
            };
            return res;
        }
        #endregion

        # region GetTeacherYearTearmClass 获取老师的年度学期班级班级代码数据
        /// <summary>
        /// 描述：获取老师的年度学期班级班级代码数据
        /// <para>作    者：Huang GaoLiang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <returns>返回老师的年度学期班级班级代码数据</returns>
        public TeacherYearTearmClassResponse GetTeacherYearTearmClass()
        {
            TeacherYearTearmClassResponse classInfo = new TeacherYearTearmClassResponse();

            // 根据当前登录的教师编号获取老师的考勤信息
            List<TblDatClass> datClassesList = DefaultClassService.GetClassListByTeacherId(this._schoolId, this._teacherId);
            var termIds = datClassesList.Select(m => m.TermId).Distinct().ToList();
            var courseIds = datClassesList.Select(m => m.CourseId).Distinct().ToList();

            // 1、获取学期
            classInfo.TermList = TermService.GetTermByTermId(termIds).Select(m => new TermOutDto
            {
                TermId = m.TermId,
                TermName = m.TermName,
                Year = m.Year
            }).ToList();

            // 2、获取年度
            classInfo.YearList = classInfo.TermList.Select(m => m.Year).Distinct().ToList();

            // 3、获取班级
            classInfo.TeacherCourseList = ClassRoomService.GetClassRoomBySchoolId(this._schoolId).Where(m => courseIds.Contains(m.CourseId))
                .Select(m => new TeacherCourseOutDto
                {
                    CourseId = m.CourseId,
                    ClassCnName = m.ClassCnName,
                    ClassRoomId = m.ClassRoomId
                }).ToList();

            classInfo.TeacherCourseList = classInfo.TeacherCourseList.Where((x, i) => classInfo.TeacherCourseList.FindIndex(z => z.CourseId == x.CourseId) == i).ToList();

            // 4、获取班级代码
            classInfo.TeacherDatClassList = datClassesList.Select(m => new TeacherDatClassOutDto
            {
                ClassId = m.ClassId,
                ClassNo = m.ClassNo,
                CourseId = m.CourseId
            }).ToList();

            // 5、教室
            var roomList = ClassRoomService.GetClassRoomBySchoolId(this._schoolId).Where(m => courseIds.Contains(m.CourseId));
            classInfo.ClassRoomList = roomList.Select(m => new ClassRoomOutDto
            {
                RoomId = m.ClassRoomId,
                RoomName = m.RoomNo
            }).ToList();

            classInfo.ClassRoomList = classInfo.ClassRoomList.Where((x, i) => classInfo.ClassRoomList.FindIndex(z => z.RoomId == x.RoomId) == i).ToList();

            return classInfo;
        }
        #endregion

        #region GetClassTimetableGeneratorList 获取用于补课或调课的老师课程表信息 
        /// <summary>
        /// 获取用于补课或调课的老师课程表信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <param name="classDate">上课时间</param>
        /// <returns>补课或调课的老师课程列表</returns>
        public List<ClassTimetableGeneratorResponse> GetClassTimetableGeneratorList(long classId, DateTime classDate)
        {
            List<ClassTimetableGeneratorResponse> res = new List<ClassTimetableGeneratorResponse>();
            if (classDate < DateTime.Today)
            {
                return res;
            }

            var classInfo = new DefaultClassService(classId).TblDatClass;

            var list = _viewClassLessonRepository.Value
                 .GetReplenishClassLessonList(this._schoolId, classInfo.CourseId, classDate, classInfo.TermId);

            //当前老师的课程
            var currentTeacherList = list.Where(x => x.TeacherId == this._teacherId)
                .OrderBy(x => x.ClassTime)
                .ToList();

            //其它老师的课程
            var notinCurrentTeacherList = list.Where(x => x.TeacherId != this._teacherId)
                .OrderBy(x => x.ClassTime)
                .ToList();

            var teacherList = TeachService.GetTeachers();

            //把当前老师的课程排在前面
            foreach (var item in currentTeacherList)
            {
                var model = new ClassTimetableGeneratorResponse
                {
                    ClassDate = item.ClassDate.ToString("yyyy-MM-dd"),
                    ClassTime = item.ClassTime,
                    ClassId = item.ClassId,
                    ClassName = item.ClassName,
                    ClassNo = item.ClassNo,
                    ClassRoom = item.ClassRoom,
                    TeacherName = teacherList.FirstOrDefault(x => x.TeacherId == item.TeacherId)?.TeacherName ?? string.Empty,
                    Week = WeekDayConvert.DayOfWeekToString(item.ClassDate.DayOfWeek)
                };
                res.Add(model);
            }


            foreach (var item in notinCurrentTeacherList)
            {
                var model = new ClassTimetableGeneratorResponse
                {
                    ClassDate = item.ClassDate.ToString("yyyy-MM-dd"),
                    ClassTime = item.ClassTime,
                    ClassId = item.ClassId,
                    ClassName = item.ClassName,
                    ClassNo = item.ClassNo,
                    ClassRoom = item.ClassRoom,
                    TeacherName = teacherList.FirstOrDefault(x => x.TeacherId == item.TeacherId)?.TeacherName ?? string.Empty,
                    Week = WeekDayConvert.DayOfWeekToString(item.ClassDate.DayOfWeek)
                };
                res.Add(model);
            }

            return res;
        }
        #endregion
    }
}
