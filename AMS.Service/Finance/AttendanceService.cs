/****************************************************************************\
所属系统：招生系统
所属模块：财务模块
创建时间：2019-03-04
作   者：郭伟佳
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/

using AMS.Core;
using AMS.Dto;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Service
{
    /// <summary>
    /// 描    述：财务考勤核对业务服务类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-04</para>
    /// </summary>
    public class AttendanceService
    {
        private readonly Lazy<ViewStudentAttendanceRepository> _attendanceRepository = new Lazy<ViewStudentAttendanceRepository>();                           //学生课程考勤信息仓储
        private readonly Lazy<TblTimLessonStudentRepository> _lessonStudentRepository = new Lazy<TblTimLessonStudentRepository>();                             //学生课次信息仓储
        private readonly Lazy<TblFinAttendanceConfirmRepository> _confirmRepository = new Lazy<TblFinAttendanceConfirmRepository>();                             //考勤确认信息仓储
        private readonly Lazy<ViewTimReplenishLessonStudentRepository> _vReplenishRepository = new Lazy<ViewTimReplenishLessonStudentRepository>();//考勤确认信息仓储
        private readonly Lazy<TblTimLessonRepository> _lessonRepository = new Lazy<TblTimLessonRepository>();                                                                   //课次基础信息仓储

        private readonly string _schoolId;//校区Id

        /// <summary>
        /// 带校区Id参数的构造函数
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-04</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        public AttendanceService(string schoolId)
        {
            this._schoolId = schoolId;
        }

        #region GetStudentAttendanceList 根据学期Id和班级Id等查询条件获取学生考勤核对信息
        /// <summary>
        /// 根据学期Id和班级Id等查询条件获取学生考勤核对信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="searchRequest">考勤查询条件</param>
        /// <returns>获取学生考勤核对信息列表</returns>
        public async Task<StudentAttendCheckResponse> GetStudentAttendanceList(StudentAttendSearchRequest searchRequest)
        {
            var result = new StudentAttendCheckResponse();
            //获取校区学生的当前学期的课程信息
            var studentLessonList = await _attendanceRepository.Value.GetStudentAttendListAsync(_schoolId, searchRequest);
            if (studentLessonList != null && studentLessonList.Count > 0)
            {
                result.DateInfos = GetLessonDateInfo(studentLessonList);       //获取课程日期集合
                result.StudentAttendList = await GetStudentAttendList(studentLessonList);                                                                       //获取班级学生的考勤信息
            }
            return result;
        }

        /// <summary>
        /// 获取班级学生的考勤信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="studentLessonList">学生课程考勤列表信息</param>
        /// <returns>班级学生的考勤信息</returns>
        private async Task<List<StudentAttendInfoResponse>> GetStudentAttendList(List<ViewStudentAttendance> studentLessonList)
        {
            List<StudentAttendInfoResponse> result = new List<StudentAttendInfoResponse>();
            //获取学生信息
            var studentIdList = studentLessonList.GroupBy(g => g.StudentId).Select(a => a.Key.Value);
            var studentInfoList = await StudentService.GetStudentByIds(studentIdList);
            //未考勤集合
            List<int> notAttendStatusList = new List<int>() { (int)LessonStatus.None, (int)LessonStatus.NotCheckIn, (int)LessonStatus.Absent, (int)LessonStatus.Leave };
            //获取学生某班级的课程考勤详情信息
            foreach (var studentId in studentIdList)
            {
                //学生信息
                var studentInfo = studentInfoList.FirstOrDefault(a => a.StudentId == studentId);
                //获取学生课程考勤信息（不包括补课、调课）
                var studentLessonInfo = studentLessonList.Where(a => a.StudentId == studentId && a.AdjustType == (int)AdjustType.DEFAULT);
                if (studentLessonInfo.Any())
                {
                    var attendInfo = GetStudentAttendInfo(studentLessonInfo, studentInfo, notAttendStatusList);
                    attendInfo.StudentId = studentId;
                    result.Add(attendInfo);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取学生考勤信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="studentLessonList">学生课次信息</param>
        /// <param name="studentInfo">学生信息</param>
        /// <param name="notAttendStatusList">未考勤集合</param>
        /// <returns></returns>
        private StudentAttendInfoResponse GetStudentAttendInfo(IEnumerable<ViewStudentAttendance> studentLessonList, TblCstStudent studentInfo, List<int> notAttendStatusList)
        {
            int attendStatus = 0;
            var lessonInfo = studentLessonList.FirstOrDefault();
            return new StudentAttendInfoResponse()
            {
                StudentName = studentInfo?.StudentName,
                StudentNo = studentInfo?.StudentNo,
                TotalLessonCount = lessonInfo.ClassLessonCount,
                AttendLessonCount = studentLessonList.Where(a => a.AttendStatus == (int)LessonStatus.Attended).Sum(a => a.LessonCount),                                 //出勤总计
                LessonDetails = studentLessonList.OrderBy(x => x.LessonType).ThenBy(y => y.ClassDate)
                        .GroupBy(g => new { g.ClassDate, g.LessonType, g.AttendStatus, g.AttendUserType, g.AdjustType })
                        .Select(b =>
                        {
                            //考勤状态attendStatus：-1：不显示；0：表示未考勤；1：表示已考勤；2：表示财务已考勤；
                            if (notAttendStatusList.Contains(b.Key.AttendStatus ?? 0))
                            {
                                attendStatus = (b.Key.AttendUserType == 9 ? 0 : (b.Key.AdjustType == 0 ? 0 : -1));
                            }
                            else
                            {
                                attendStatus = (b.Key.AttendUserType == 9 ? 2 : 1);
                            }

                            return new StudentAttendLessonResponse()
                            {
                                LessonDate = b.Key.ClassDate.ToString("MM/dd"),                                                                                                                                                    //课次日期
                                LessonStudentIds = string.Join(",", studentLessonList.Where(x => x.ClassDate == b.Key.ClassDate).Select(y => y.StudentLessonId.Value)),       //学生课次Id
                                LessonType = (LessonType)b.Key.LessonType,                                                                                                                                                          //1表示常规课；2表示写生课
                                LessonCount = studentLessonList.Where(x => x.ClassDate == b.Key.ClassDate).Sum(y => y.LessonCount),                                                         //课次：一般情况下常规课是1节，写生课是4节
                                AttendStatus = attendStatus,                                                                                                                                                                                        //考勤状态
                                StatusName = b.Key.AttendStatus == (int)LessonStatus.Attended ? "1" : ""                                                                                                                //状态名称
                            };
                        }).ToList()
            };
        }

        /// <summary>
        /// 获取课程日期集合
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="studentLessonList">学生课程考勤列表信息</param>
        /// <returns>课程日期集合</returns>
        private List<StudentAttendDateInfoResponse> GetLessonDateInfo(List<ViewStudentAttendance> studentLessonList)
        {
            //如果不存在学生课程信息，则返回课程日期的空集合
            if (studentLessonList == null || studentLessonList.Count == 0)
            {
                return new List<StudentAttendDateInfoResponse>();
            }
            //获取课程日期集合
            var classLessonInfo = studentLessonList.OrderBy(x => x.LessonType)
                                                .ThenBy(o => o.ClassDate)
                                                .ThenBy(t => t.ClassBeginTime)
                                                .GroupBy(g => new { g.ClassDate, g.LessonType, g.ClassBeginTime, g.LessonCount });
            return classLessonInfo.GroupBy(g => new { g.Key.ClassDate, g.Key.LessonType }).Select(a => new StudentAttendDateInfoResponse()
            {
                LessonCount = classLessonInfo.Where(b => b.Key.ClassDate == a.Key.ClassDate).Sum(c => c.Key.LessonCount),
                LessonType = a.Key.LessonType,
                LessonDate = a.Key.ClassDate.ToString("MM/dd")
            }).ToList();
        }
        #endregion

        #region GetStudentAttendanceReport 根据学期Id和班级Id等查询条件获取学生考勤统计表信息
        /// <summary>
        /// 根据学期Id和班级Id等查询条件获取学生考勤统计表信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-04</para>
        /// </summary>
        /// <param name="searchRequest">考勤查询条件</param>
        /// <returns>获取学生考勤统计表</returns>
        public async Task<StudentAttendReportResponse> GetStudentAttendanceReport(StudentAttendSearchRequest searchRequest)
        {
            var result = new StudentAttendReportResponse();
            //获取校区学生的当前学期的课程信息
            var studentLessonList = await _attendanceRepository.Value.GetAttendListByFinanceAsync(_schoolId, searchRequest);
            if (studentLessonList != null && studentLessonList.Count > 0)
            {
                //获取所有老师信息
                var teacherInfoList = TeachService.GetTeachers();
                var lessonList = studentLessonList.Where(a => (searchRequest.Month > 0 && a.ClassDate.Year == searchRequest.Year && a.ClassDate.Month == searchRequest.Month)
                                                    || searchRequest.Month == 0).ToList();
                result.DateInfos = GetLessonDateInfo(lessonList);                               //获取课程日期集合
                result.StudentAttendList = await GetStudentAttendList(studentLessonList, teacherInfoList, searchRequest.Year, searchRequest.Month);           //获取班级学生的考勤信息
                result.TeacherAttendList = GetTeacherAttendList(studentLessonList, teacherInfoList, searchRequest.Year, searchRequest.Month);              //获取班级老师的考勤信息
            }
            return result;
        }

        #region GetStudentAttendList 获取班级学生的考勤信息
        /// <summary>
        /// 获取班级学生的考勤信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="studentLessonList">学生课程考勤列表信息</param>
        /// <param name="teacherInfoList">老师信息</param>
        /// <param name="year">年份</param>
        /// <param name="month">0表示学期，其他数字表示月份</param>
        /// <returns>班级学生的考勤信息</returns>
        private async Task<List<StudentAttendInfoResponse>> GetStudentAttendList(List<ViewStudentAttendance> studentLessonList,
            List<ClassTimetableTeacherResponse> teacherInfoList,
            int year,
            int month)
        {
            List<StudentAttendInfoResponse> result = new List<StudentAttendInfoResponse>();
            //获取学生信息
            var studentIdList = studentLessonList.GroupBy(g => g.StudentId).Select(a => a.Key.Value);
            var studentInfoList = await StudentService.GetStudentByIds(studentIdList);

            //获取学生补课信息
            var replenishLessonList = await _vReplenishRepository.Value.GetLessonListByRootLessonIdAsync(studentLessonList.Select(a => a.LessonId).ToList());

            //定义未考勤集合
            List<int> notAttendStatusList = new List<int>() { (int)LessonStatus.None, (int)LessonStatus.NotCheckIn, (int)LessonStatus.Absent, (int)LessonStatus.Leave };

            //获取学生某班级的课程考勤详情信息
            foreach (var studentId in studentIdList)
            {
                //学生信息
                var studentInfo = studentInfoList.FirstOrDefault(a => a.StudentId == studentId);
                //获取学生课程考勤信息（不包括补课、调课）
                var studentLessonInfo = studentLessonList.Where(a => a.StudentId == studentId);
                //获取学生补课考勤信息
                var replenishLessonInfo = replenishLessonList.Where(a => a.StudentId == studentId);
                if (studentLessonInfo.Any())
                {
                    var attendInfo = GetStudentAttendInfo(studentLessonInfo, studentInfo, teacherInfoList, notAttendStatusList, replenishLessonInfo, year, month);
                    result.Add(attendInfo);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取学生考勤课程信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="studentLessonList">学生课次信息</param>
        /// <param name="studentInfo">学生信息</param>
        /// <param name="teacherInfoList">老师信息</param>
        /// <param name="notAttendStatusList">未考勤集合</param>
        /// <param name="replenishLessonList">学生补课信息</param>
        /// <param name="year">年份</param>
        /// <param name="month">0表示学期，其他数字表示月份</param>
        /// <returns>学生考勤信息</returns>
        private StudentAttendInfoResponse GetStudentAttendInfo(IEnumerable<ViewStudentAttendance> studentLessonList,
            TblCstStudent studentInfo,
            List<ClassTimetableTeacherResponse> teacherInfoList,
            List<int> notAttendStatusList,
            IEnumerable<ViewTimReplenishLessonStudent> replenishLessonList,
            int year,
            int month)
        {
            var studentLessonInfo = studentLessonList.FirstOrDefault();
            var lessonList = studentLessonList.Where(a => (month > 0 && a.ClassDate.Year == year && a.ClassDate.Month == month) || month == 0);
            var result = new StudentAttendInfoResponse()
            {
                StudentId = studentLessonInfo?.StudentId,                                                                                                                                                                         //学生Id
                StudentName = studentInfo?.StudentName,                                                                                                                                                                       //学生名称
                StudentNo = studentInfo?.StudentNo,                                                                                                                                                                                 //学生学号
                TotalLessonCount = studentLessonInfo?.ClassLessonCount ?? 0,                                                                                                                                    //报名课次
                AttendLessonCount = studentLessonList.Where(a => a.AttendStatus == (int)LessonStatus.Attended).Sum(a => a.LessonCount),                                 //出勤总计
                LessonDetails = GetStudentAttendLessonList(lessonList, teacherInfoList, notAttendStatusList, replenishLessonList)                                                      //学生考勤课程信息
            };
            result.LeftLessonCount = studentLessonList.Sum(a => a.LessonCount) - result.AttendLessonCount;                                                                                  //剩余课次
            result.CurrentMonthLessonCount = month == 0
                                ? "--"
                                : lessonList.Where(a => a.AttendStatus == (int)LessonStatus.Attended).Sum(a => a.LessonCount).ToString();                                  //当月出勤
            return result;
        }

        /// <summary>
        /// 获取学生考勤课程信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="studentLessonList">学生课次信息</param>
        /// <param name="teacherInfoList">老师信息</param>
        /// <param name="notAttendStatusList">未考勤集合</param>
        /// <param name="replenishLessonList">学生补课信息</param>
        /// <returns>学生考勤课程信息</returns>
        private List<StudentAttendLessonResponse> GetStudentAttendLessonList(IEnumerable<ViewStudentAttendance> studentLessonList,
            List<ClassTimetableTeacherResponse> teacherInfoList,
            List<int> notAttendStatusList,
            IEnumerable<ViewTimReplenishLessonStudent> replenishLessonList)
        {
            string classTime = string.Empty;//上课时间
            string teacherName = string.Empty;//老师名称
            string replenishTips = string.Empty;//补课/调课提示 
            List<StudentAttendLessonResponse> lessonList = studentLessonList.OrderBy(x => x.LessonType).ThenBy(y => y.ClassDate)
                    .GroupBy(g => new { g.ClassDate, g.LessonType, g.AttendStatus, g.AdjustType, g.BusinessType, g.ProcessStatus })
                    .Select(b =>
                    {
                        var studentLessonDateList = studentLessonList.Where(x => x.ClassDate == b.Key.ClassDate);//上课日期对应的课程信息
                        var replenhishList = replenishLessonList.Where(a => studentLessonDateList.Select(s => s.LessonId).Contains(a.RootLessonId));//补课信息
                        replenishTips = string.Empty;//补课/调课提示 
                        if (replenhishList.Any())
                        {
                            var replenishLesson = replenhishList.FirstOrDefault();
                            classTime = string.Join("，", replenhishList.OrderBy(o => o.ClassBeginTime).Select(x => $"{x.ClassBeginTime}-{x.ClassEndTime}").Distinct());
                            teacherName = teacherInfoList.FirstOrDefault(a => a.TeacherId == replenishLessonList.FirstOrDefault().TeacherId)?.TeacherName;
                            replenishTips = $"{replenishLesson?.ClassDate.ToString("yyyy-MM-dd")} {WeekDayConvert.DayOfWeekToString(replenishLesson.ClassDate.DayOfWeek)}（{classTime}）{teacherName}老师补课";
                        }
                        var lessonCount = studentLessonList.Where(x => x.ClassDate == b.Key.ClassDate).Sum(y => y.LessonCount);
                        return new StudentAttendLessonResponse()
                        {
                            LessonDate = b.Key.ClassDate.ToString("MM/dd"),                                                                                                                                                 //课次日期
                            LessonType = (LessonType)b.Key.LessonType,                                                                                                                                                      //1表示常规课；2表示写生课
                            LessonCount = lessonCount,                                                                                                                                                                                    //课次：一般情况下常规课是1节，写生课是4节
                            AttendStatus = notAttendStatusList.Contains(b.Key.AttendStatus ?? 0) ? 0 : 1,                                                                                                      //考勤状态：0表示学生未考勤；1表示学生已考勤；
                            ReplenishStatus = (b.Key.AdjustType == (int)AdjustType.DEFAULT ? 0 : 1),                                                                                                          //是否补课调课
                            ReplenishLessonTips = replenishTips,
                            StatusName = (b.Key.AttendStatus == (int)LessonStatus.Attended ? lessonCount.ToString()
                            : (b.Key.ProcessStatus == (int)ProcessStatus.End ? GetStatusNameByProcessStatus(b.Key.BusinessType ?? 0) : ""))                                      //状态名称
                        };
                    }).ToList();
            return lessonList;
        }

        /// <summary>
        /// 根据课程业务类型，获取作废课程的相关课程状态
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="businessType">课程业务类型</param>
        /// <returns>课程状态</returns>
        public string GetStatusNameByProcessStatus(int businessType)
        {
            switch ((ProcessBusinessType)businessType)
            {
                case ProcessBusinessType.F_ChangeSchool:
                    return EnumName.GetDescription(typeof(FinanceLessonStatus), FinanceLessonStatus.ChangeSchool);
                case ProcessBusinessType.F_ChangeClass:
                    return EnumName.GetDescription(typeof(FinanceLessonStatus), FinanceLessonStatus.ChangeClass);
                case ProcessBusinessType.F_LeaveSchool:
                    return EnumName.GetDescription(typeof(FinanceLessonStatus), FinanceLessonStatus.LeaveSchool);
                case ProcessBusinessType.F_Refund:
                    return EnumName.GetDescription(typeof(FinanceLessonStatus), FinanceLessonStatus.Refund);
                default:
                    return "";
            }
        }
        #endregion

        #region GetTeacherAttendList 获取班级老师的考勤信息
        /// <summary>
        /// 获取班级老师的考勤信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        /// <param name="studentLessonList">学生课程考勤列表信息</param>
        /// <param name="teacherInfoList">老师信息</param>
        /// <param name="year">年份</param>
        /// <param name="month">0表示学期，其他数字表示月份</param>
        /// <returns>班级老师的考勤信息</returns>
        private List<StudentAttendTeacherInfoResponse> GetTeacherAttendList(List<ViewStudentAttendance> studentLessonList,
            List<ClassTimetableTeacherResponse> teacherInfoList,
            int year,
            int month)
        {
            var result = new List<StudentAttendTeacherInfoResponse>();

            //获取班级老师考勤信息
            var teacherIdList = studentLessonList.Select(a => a.TeacherId).Distinct().ToList();
            if (teacherIdList.Any())
            {
                //获取老师考勤确认信息
                var attendConfirmList = _confirmRepository.Value.GetAttendanceConfirmList(_schoolId, teacherIdList, studentLessonList.Select(a => a.ClassId).Distinct().ToList());
                foreach (var teacherId in teacherIdList)
                {
                    var attendConfirmInfo = attendConfirmList.FirstOrDefault(a => a.TeacherId == teacherId);//老师考勤签名确认信息
                    var teacherInfo = teacherInfoList.FirstOrDefault(b => b.TeacherId == teacherId);//老师信息
                    StudentAttendTeacherInfoResponse teacherAttendInfo = GetTeacherAttendInfo(teacherId, studentLessonList, teacherInfo, attendConfirmInfo, year, month);
                    if (teacherAttendInfo != null)
                    {
                        result.Add(teacherAttendInfo);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取班级老师考勤信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        /// <param name="teacherId">老师Id</param>
        /// <param name="studentLessonList">学生课程考勤列表信息</param>
        /// <param name="teacherInfo">老师信息</param>
        /// <param name="attendConfirmInfo">老师考勤确认信息</param>
        /// <param name="year">年份</param>
        /// <param name="month">0表示学期，其他数字表示月份</param>
        /// <returns></returns>
        private StudentAttendTeacherInfoResponse GetTeacherAttendInfo(string teacherId,
            List<ViewStudentAttendance> studentLessonList,
            ClassTimetableTeacherResponse teacherInfo,
            TblFinAttendanceConfirm attendConfirmInfo,
            int year,
            int month)
        {
            //获取班级老师考勤信息（不包括补课、调课）
            var teacherLessonInfoList = studentLessonList.Where(a => a.TeacherId == teacherId && a.AdjustType == (int)AdjustType.DEFAULT
                     && a.ClassBeginDate <= DateTime.Now)
                    .OrderBy(x => x.LessonType)
                    .ThenBy(y => y.ClassBeginDate)
                    .GroupBy(g => new { g.ClassDate, g.LessonType, g.ClassBeginTime, g.LessonCount });
            if (teacherLessonInfoList.Any())
            {
                var teacherLessonList = teacherLessonInfoList.Where(a => (month > 0 && a.Key.ClassDate.Year == year && a.Key.ClassDate.Month == month) || month == 0);
                return new StudentAttendTeacherInfoResponse()
                {
                    TeacherId = teacherId,
                    TeacherName = teacherInfo?.TeacherName,
                    AttendLessonCount = teacherLessonInfoList.Sum(c => c.Key.LessonCount),
                    CurrentMonthLessonCount = (month == 0
                                ? "--"
                                : teacherLessonList.Sum(c => c.Key.LessonCount).ToString()),                      //当月出勤
                    SignatureUrl = attendConfirmInfo?.TeacherSignUrl,
                    //获取老师考勤课程
                    TeacherLessons = teacherLessonList.Select(c => new StudentAttendTeacherLessonResponse()
                    {
                        LessonDate = c.Key.ClassDate.ToString("MM/dd"),
                        LessonCount = teacherLessonList.Where(d => d.Key.ClassDate == c.Key.ClassDate).Sum(e => e.Key.LessonCount),
                        LessonType = (LessonType)c.Key.LessonType,
                        StatusName = teacherLessonList.Where(d => d.Key.ClassDate == c.Key.ClassDate).Sum(e => e.Key.LessonCount).ToString()
                    }).ToList()
                };
            }
            return null;
        }
        #endregion

        #endregion

        #region UpdateAttendStatus 更新学生对应的课次考勤状态为已考勤
        /// <summary>
        /// 更新学生对应的课次考勤状态为已考勤
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="LessonStudentIds">学生课次Id</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：
        /// 48、未找到该学生的课次信息
        /// 55、该课程已补课/调课，不可请假
        /// 70、上课未结束不能补签
        /// </exception>
        public async Task UpdateAttendStatus(List<long> LessonStudentIds)
        {
            //1.获取学生课次信息
            var lessonStudentList = await _lessonStudentRepository.Value.GetListByLessonStudentIdAsnyc(LessonStudentIds);

            //2. 财务考勤之前，进行数据校验
            await CheckDataBeforeUpdate(lessonStudentList, LessonStudentIds);

            //3. 更新学生考勤课次表考勤数据
            await UpdateAttendStatus(LessonStudentIds, lessonStudentList);
        }

        /// <summary>
        /// 更新学生考勤课次表考勤数据
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="LessonStudentIds">学生课次Id集合</param>
        /// <param name="lessonStudentList"> 学生课次课程考勤信息</param>
        /// <returns></returns>
        private async Task UpdateAttendStatus(List<long> LessonStudentIds, List<TblTimLessonStudent> lessonStudentList)
        {
            AttendStatus newAttendStatus = AttendStatus.Normal;
            AttendStatus oldAttendStatus = (AttendStatus)lessonStudentList.FirstOrDefault().AttendStatus;
            DateTime? attendTime = DateTime.Now;
            switch (oldAttendStatus)
            {
                case AttendStatus.Normal:
                    newAttendStatus = AttendStatus.NotClockIn;
                    attendTime = null;
                    break;
                case AttendStatus.NotClockIn:
                case AttendStatus.Leave:
                    newAttendStatus = AttendStatus.Normal;
                    attendTime = DateTime.Now;
                    break;
            }
            if (!await _lessonStudentRepository.Value.UpdateAttendStatusAsync(LessonStudentIds, newAttendStatus, oldAttendStatus,
                    attendTime, attendUserType: AttendUserType.FINANCE))
            {
                throw new BussinessException(ModelType.Timetable, 56);
            }
        }

        /// <summary>
        /// 财务考勤之前，进行数据校验
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="lessonStudentList">学生课次课程考勤信息</param>
        /// <param name="lessonStudentIdList">学生课次Id集合</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：
        /// 48、未找到该学生的课次信息
        /// 55、该课程已补课/调课，不可请假
        /// 70、上课未结束不能补签
        /// </exception>
        private async Task CheckDataBeforeUpdate(List<TblTimLessonStudent> lessonStudentList, List<long> lessonStudentIdList)
        {
            //检查课次信息是否存在
            if (lessonStudentIdList.Exists(lessonId => !lessonStudentList.Any(b => b.LessonStudentId == lessonId)))
            {
                throw new BussinessException(ModelType.Timetable, 48);
            }
            //课程是否上课已结束，如果没有结束，不允许考勤
            var lessonList = await _lessonRepository.Value.GetByLessonIdTask(lessonStudentList.Select(a => a.LessonId));
            if (lessonList.Any(a => a.ClassEndDate >= DateTime.Now))
            {
                throw new BussinessException(ModelType.Timetable, 70);
            }
            //补课、调课集合
            List<AdjustType> adjustTypesList = new List<AdjustType>()
            {
                AdjustType.ADJUST,
                AdjustType.MAKEUP
            };
            //检查是否存在已经补课、调课，如果存在，则不允许考勤
            if (lessonStudentList.Any(a => adjustTypesList.Contains((AdjustType)a.AdjustType)))
            {
                throw new BussinessException((byte)ModelType.Timetable, 55);
            }
        }
        #endregion

        #region SignConfirmAsync 老师签字确认
        /// <summary>
        /// 老师签字确认
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="request">老师签字确认信息请求对象</param>
        public async Task SignConfirmAsync(StudentAttendConfirmRequest request)
        {
            var attendConfirm = await _confirmRepository.Value.GetAttendConfirmInfo(_schoolId, request.TeacherId, request.ClassId, request.Month);
            if (attendConfirm != null)
            {
                attendConfirm.TeacherSignUrl = request.TeacherSignUrl;
                await _confirmRepository.Value.UpdateTask(attendConfirm);
            }
            else
            {
                await AddAttendConfirmAsync(request);
            }
        }

        /// <summary>
        /// 新增考勤确认信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="request">老师签字确认信息请求对象</param>
        private async Task AddAttendConfirmAsync(StudentAttendConfirmRequest request)
        {
            TblFinAttendanceConfirm attendanceConfirm = new TblFinAttendanceConfirm()
            {
                AttendanceConfirmId = IdGenerator.NextId(),
                SchoolId = _schoolId,
                TeacherId = request.TeacherId,
                ClassId = request.ClassId,
                Month = request.Month,
                TeacherSignUrl = request.TeacherSignUrl
            };
            await _confirmRepository.Value.AddTask(attendanceConfirm);
        }
        #endregion
    }
}
