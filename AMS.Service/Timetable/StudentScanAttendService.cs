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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Anticorrosion.HRS;
using AMS.Core;
using AMS.Core.Constants;
using AMS.Dto;
using AMS.Dto.Enum;
using AMS.Service.Hss;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using Jerrisoft.Platform.Log;

namespace AMS.Service
{
    /// <summary>
    /// 描述：学生扫码考勤
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-06</para>
    /// </summary>
    internal class StudentScanAttendService
    {
        private readonly Lazy<TblTimLessonStudentRepository> _lessonStudentRepository = new Lazy<TblTimLessonStudentRepository>();
        private readonly Lazy<TblTimReplenishLessonRepository> _replenishLessonRepository = new Lazy<TblTimReplenishLessonRepository>();


        private readonly ViewStudentScanCodeAttendRepository _repository; //学生考勤仓储
        private readonly StudentService _studentService;//学生服务

        private readonly string _schoolId;      //校区ID
        private readonly string _teacherId;     //老师ID
        private readonly long _studentId;       //学生ID
        private static int _tblSource1 => 1; //TblTimLessonStudent表    
        private static int _tblSource2 => 2; //TblTimReplenishLesson 表

        /// <summary>
        /// 构造一个学生扫码考勤的服务
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <param name="schoolId">校区ID</param>
        /// <param name="teacherId">校区ID</param>
        /// <param name="studentId">学生ID</param>
        public StudentScanAttendService(string schoolId, string teacherId, long studentId)
        {
            this._schoolId = schoolId;
            this._teacherId = teacherId;
            this._studentId = studentId;
            this._repository = new ViewStudentScanCodeAttendRepository();
            this._studentService = new StudentService(this._schoolId);
        }

        /// <summary>
        /// 学生信息
        /// </summary>
        private StudentDetailResponse _student;

        /// <summary>
        /// 学生信息
        /// </summary>
        private StudentDetailResponse Student
        {
            get
            {
                if (_student == null)
                {
                    _student = _studentService.GetStudent(this._studentId);
                }
                return _student;
            }
        }

        /// <summary>
        /// 扫🐴考勤
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <param name="classIds">班级Id列表</param>
        /// <returns>老师使用K信扫描学生二维码考勤输出</returns>
        public StudentScanCodeAttendResponse ScanCode(List<long> classIds)
        {
            //返回信息
            StudentScanCodeAttendResponse res = new StudentScanCodeAttendResponse
            {
                AttendStatus = ScanCodeAttendStatusResponse.SUCCESS,
                Message = string.Empty,
                ClassItem = new List<ScanCodeClassInfo>()
            };

            try
            {
                //1.校验二维码
                this.VerifyQrCode();

                //2.校验当前时间是否在有效的扫码考勤范围内
                this.VerifyScanCodePeriod();

                //3.校验学生是否绑定家校互联
                this.VerifyStudentBindWeiXin();

                //4.获取学生今天的考勤数据
                List<ViewStudentScanCodeAttend> stuAttendList = this._repository.GetDayAttendList(
                        this._schoolId, this._teacherId, this._studentId, DateTime.Today);

                //5.校验该学生当前时间在这个老师这是否有课
                this.VerifyStudentHaveClass(stuAttendList);

                //6.校验是否已考勤
                this.VerifyStudentHaveAttend(stuAttendList);

                if (classIds == null || !classIds.Any())
                {
                    //7.判断是否有多节课
                    (bool multi, List<ScanCodeClassInfo> scanCodeInfoList) = VerifyMultipleAttend(stuAttendList);
                    if (multi)
                    {
                        res.AttendStatus = ScanCodeAttendStatusResponse.MULTIPLE;
                        res.ClassItem = scanCodeInfoList;
                        return res;
                    }
                }

                //8.正常考勤
                this.NormalStudentAttend(stuAttendList, classIds);
            }
            catch (BussinessException e)//错误
            {
                switch ((ScanCodeAttendStatusResponse)e.InnerExceptionId)
                {
                    case ScanCodeAttendStatusResponse.SUCCESS:   //成功
                        res.AttendStatus = ScanCodeAttendStatusResponse.SUCCESS;
                        break;
                    case ScanCodeAttendStatusResponse.FAIL:      //失败
                        res.AttendStatus = ScanCodeAttendStatusResponse.FAIL;
                        break;
                    case ScanCodeAttendStatusResponse.MULTIPLE:  //多项
                        res.AttendStatus = ScanCodeAttendStatusResponse.MULTIPLE;
                        break;
                    case ScanCodeAttendStatusResponse.SYSWARNING://系统提示
                        res.AttendStatus = ScanCodeAttendStatusResponse.SYSWARNING;
                        break;
                    case ScanCodeAttendStatusResponse.WARNING:   //温馨提示
                        res.AttendStatus = ScanCodeAttendStatusResponse.WARNING;
                        break;
                    default:
                        res.AttendStatus = ScanCodeAttendStatusResponse.FAIL;
                        break;
                }
                res.Message = e.Message;
            }
            catch (Exception e) //错误
            {
                LogWriter.Write("ScanCode", $"扫码考勤{e.Message}{e}", LoggerType.Error);
                res.AttendStatus = ScanCodeAttendStatusResponse.FAIL;
                res.Message = ScanCodeAttendConstants.SysError;
            }
            return res;
        }

        /// <summary>
        /// 校验二维码
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <exception cref="BussinessException">
        /// 异常ID:7  异常描述:无效二维码
        /// </exception>
        private void VerifyQrCode()
        {
            if (Student == null)
            {
                //无效二维码
                throw new BussinessException(ModelType.Default,
                    (ushort)ScanCodeAttendStatusResponse.FAIL, ScanCodeAttendConstants.QrCodeInvalid);
            }
        }

        /// <summary>
        /// 校验当前时间是否在有效的扫码考勤范围内
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <exception cref="BussinessException">
        /// 异常ID:6  异常描述:扫码考勤时间范围配置错误
        /// 异常ID:37 异常描述:未到考勤时间
        /// </exception>
        private void VerifyScanCodePeriod()
        {
            //开始时间
            bool sTimeIsDateTime = DateTime.TryParse(ClientConfigManager.AppsettingsConfig.ScanCodeAttend.StartTime, out DateTime sTime);
            //结束时间
            bool eTimeIsDateTime = DateTime.TryParse(ClientConfigManager.AppsettingsConfig.ScanCodeAttend.EndTime, out DateTime eTime);

            if (!sTimeIsDateTime || !eTimeIsDateTime)
            {
                //扫码考勤时间范围配置错误
                throw new BussinessException(ModelType.Default,
                    (ushort)ScanCodeAttendStatusResponse.FAIL, ScanCodeAttendConstants.AttendError);
            }

            if (sTime > DateTime.Now || eTime < DateTime.Now)
            {
                //未到考勤时间
                throw new BussinessException(ModelType.Default,
                    (ushort)ScanCodeAttendStatusResponse.FAIL, ScanCodeAttendConstants.AttendError1);
            }
        }

        /// <summary>
        /// 校验学生是否绑定公众号
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <exception cref="BussinessException">
        /// 异常ID:38 ->学生未绑定家校互联
        /// </exception>
        private void VerifyStudentBindWeiXin()
        {
            List<string> phones = StudentService.GetMobiles(this._studentId);

            PassportService service = new PassportService();
            bool isMobileBind = service.IsMobileBind(phones);
            if (!isMobileBind)
            {
                //学生未绑定家校互联
                throw new BussinessException(ModelType.Default,
                    (ushort)ScanCodeAttendStatusResponse.WARNING, ScanCodeAttendConstants.Unbound);
            }
        }

        /// <summary>
        /// 上课时间开始时间结束时间转换
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="time">时间</param>
        /// <param name="isNegate">取反</param>
        /// <returns>DateTime</returns>
        /// <exception cref="BussinessException">
        /// 异常ID:41 异常描述:时间转换失败
        /// </exception>
        internal static DateTime ConvDateTime(DateTime date, string time, bool isNegate)
        {
            int bufferTime = ClientConfigManager.AppsettingsConfig.ScanCodeAttend.BufferTime;

            bool isDateTime = DateTime.TryParse(date.ToString("yyyy-MM-dd") + " " + time, out DateTime dt);
            if (!isDateTime)
            {
                //时间转换失败
                throw new BussinessException(ModelType.Timetable, 41);
            }

            return isNegate ? dt.AddMinutes(-bufferTime) : dt.AddMinutes(bufferTime);
        }

        /// <summary>
        /// 校验学生当前时间段是否有课
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <exception cref="BussinessException">
        /// 异常ID:39 异常描述:{studentName}当前没有要上的课程
        /// </exception>
        private void VerifyStudentHaveClass(List<ViewStudentScanCodeAttend> stuAttends)
        {
            List<ViewStudentScanCodeAttend> nowStuAttendList = GetNowSutdentAttend(stuAttends);

            if (!nowStuAttendList.Any())
            {
                //当前没有要上的课程
                throw new BussinessException(ModelType.Default,
                    (ushort)ScanCodeAttendStatusResponse.WARNING, ScanCodeAttendConstants.CourseError);
            }
        }

        /// <summary>
        /// 获取当前时间考勤
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="stuAttends">学生考勤列表</param>
        /// <returns>学生考勤列表</returns>
        private static List<ViewStudentScanCodeAttend> GetNowSutdentAttend(List<ViewStudentScanCodeAttend> stuAttends)
        {
            //常规时间需要ClassDate+ClassBeginTime 并 ClassDate+ClassEndTime 组合判断
            //写生课直接判断ClassBeginTime、ClassEndTime
            DateTime now = DateTime.Now;

            //常规课
            var regularAttends = stuAttends.Where(x =>
                                 x.LessonType == (int)LessonType.RegularCourse &&
                                 ConvDateTime(x.ClassDate, x.ClassBeginTime, true) <= now && now <=
                                 ConvDateTime(x.ClassDate, x.ClassEndTime, false)).ToList();

            //写生课
            int bufferTime = ClientConfigManager.AppsettingsConfig.ScanCodeAttend.BufferTime;
            var sketchAttends = stuAttends.Where(x =>
                                x.LessonType == (int)LessonType.SketchCourse &&
                                DateTime.Parse(x.ClassBeginTime).AddMinutes(-bufferTime) <= now && now <=
                                DateTime.Parse(x.ClassEndTime).AddMinutes(bufferTime)).ToList();

            return regularAttends.Union(sketchAttends).ToList();
        }

        /// <summary>
        /// 校验考勤是否有多个时间段
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="stuAttends">学生考勤列表</param>
        /// <returns>第一参数：true有多个时间段 false 没有  第二参数：有多个时间段的课程信息 </returns>
        /// <exception cref="BussinessException">
        /// 异常ID:21 异常描述:未找到班级信息
        /// 异常ID:22 异常描述:未找到课程信息
        /// </exception>
        private (bool, List<ScanCodeClassInfo>) VerifyMultipleAttend(List<ViewStudentScanCodeAttend> stuAttends)
        {
            bool multiAttend = false;
            List<ScanCodeClassInfo> classInfoList = new List<ScanCodeClassInfo>();

            //筛选未考勤的数据
            var noAttendances = GetNowSutdentAttend(stuAttends)
                .Where(x => x.AttendStatus != (int)AttendStatus.Normal)
                .ToList();

            //常规课未考勤的数据
            var regularAttends = noAttendances.Where(x => x.LessonType == (int)LessonType.RegularCourse).ToList();

            //写生课未考勤的数据
            var sketchAttends = noAttendances.Where(x => x.LessonType == (int)LessonType.SketchCourse).ToList();

            //统计当前时间常规课跟写生课总数
            int count = regularAttends.Select(x => x.ClassId).Union(sketchAttends.Select(x => x.BusinessId)).Count();

            if (count > 1)
            {
                multiAttend = true;
                classInfoList = GetMultiAttend(regularAttends, sketchAttends);
            }

            return (multiAttend, classInfoList);
        }

        /// <summary>
        /// 获取多个时间段上课信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="regularAttends">常规课</param>
        /// <param name="sketchAttends">写生课</param>
        /// <returns>选择的班级信息</returns>
        private static List<ScanCodeClassInfo> GetMultiAttend(
            List<ViewStudentScanCodeAttend> regularAttends,
            List<ViewStudentScanCodeAttend> sketchAttends)
        {
            List<ScanCodeClassInfo> res = new List<ScanCodeClassInfo>();

            if (regularAttends.Any())
            {
                GetMultiAttendRegular(regularAttends, res);
            }

            if (sketchAttends.Any())
            {
                GetMultiAttendSketch(sketchAttends, res);
            }

            return res.OrderBy(x => x.ClassTime).ToList();
        }

        /// <summary>
        /// 获取写生课多个时间段上课信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="sketchAttends">写生课次信息</param>
        /// <param name="res">out 常规课多个上课信息</param>
        private static void GetMultiAttendSketch(
            List<ViewStudentScanCodeAttend> sketchAttends,
            List<ScanCodeClassInfo> res)
        {
            var lifeClassIdList = sketchAttends.Select(x => x.BusinessId).Distinct().ToList();
            var lifeClassList = new TblTimLifeClassRepository().GetLifeClassList(lifeClassIdList);
            foreach (var lifeClassId in lifeClassIdList)
            {
                var lifeClassInfo = lifeClassList.FirstOrDefault(x => x.LifeClassId == lifeClassId);
                var sketchAttend = sketchAttends.FirstOrDefault(x => x.BusinessId == lifeClassId);

                string classTime;
                if (sketchAttend != null)
                {
                    classTime = DateTime.Parse(sketchAttend.ClassBeginTime).ToString("HH:mm") +
                                "-" +
                                DateTime.Parse(sketchAttend.ClassBeginTime).ToString("yyyy.MM.dd HH:mm");
                }
                else if (lifeClassInfo != null)
                {
                    classTime = lifeClassInfo.ClassBeginTime.Value.ToString("HH:mm") +
                                "-" +
                                lifeClassInfo.ClassBeginTime.Value.ToString("yyyy.MM.dd HH:mm");
                }
                else
                {
                    classTime = string.Empty;
                }

                ScanCodeClassInfo scanCode = new ScanCodeClassInfo
                {
                    ClassId = lifeClassId,
                    ClassName = lifeClassInfo?.Title ?? string.Empty,
                    ClassTime = classTime
                };

                res.Add(scanCode);
            }
        }

        /// <summary>
        /// 获取常规课多个时间段上课信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="regularAttends">常规课次信息</param>
        /// <param name="res">out 常规课多个上课信息</param>
        private static void GetMultiAttendRegular(
            List<ViewStudentScanCodeAttend> regularAttends,
            List<ScanCodeClassInfo> res)
        {
            var classIdList = regularAttends.Select(x => x.ClassId).Distinct().ToList();  //班级ID列表
            var courseIdList = regularAttends.Select(x => x.CourseId).Distinct().ToList();//课程ID列表

            //获取课程信息列表
            var courseList = CourseService.GetByCourseId(courseIdList);

            foreach (var classId in classIdList)
            {
                var regularAttend = regularAttends.FirstOrDefault(x => x.ClassId == classId);
                var courseInfo = courseList.FirstOrDefault(x => x.CourseId == regularAttend?.CourseId);

                //上课时间
                var classTimeList = regularAttends
                    .OrderBy(x => x.ClassBeginTime)
                    .Select(x => x.ClassBeginTime + '-' + x.ClassEndTime)
                    .ToList();

                ScanCodeClassInfo scanCode = new ScanCodeClassInfo
                {
                    ClassId = classId,
                    ClassName = courseInfo?.ClassCnName ?? string.Empty,
                    ClassTime = string.Join(" ", classTimeList)
                };

                res.Add(scanCode);
            }
        }

        /// <summary>
        /// 校验学生这个时间点是否已签到
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-03</para>
        /// </summary>
        /// <param name="stuAttends">学生考勤列表</param>
        /// <exception cref="BussinessException">
        /// 异常ID:1 异常描述:XXX于HH:mm已签到
        /// </exception>
        private void VerifyStudentHaveAttend(List<ViewStudentScanCodeAttend> stuAttends)
        {
            var stuAttendList = GetNowSutdentAttend(stuAttends);

            List<int> asList = new List<int> {
                 (int)AttendStatus.NotClockIn,      //未考勤
                 (int)AttendStatus.Leave            //请假
            };

            var isAttend = stuAttendList.Any(x => asList.Contains(x.AttendStatus));

            if (!isAttend)
            {
                DateTime attendDate = stuAttendList.Select(x => x.AttendDate.Value).Max();

                //$"{this.Student.StudentName}同学在{attendDate:HH:mm}完成考勤"
                throw new BussinessException(ModelType.Default, (ushort)ScanCodeAttendStatusResponse.SUCCESS,
                    string.Format(ScanCodeAttendConstants.AttendError2, this.Student.StudentName, attendDate.ToString("HH:mm")));
            }
        }

        /// <summary>
        /// 正常考勤
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-03</para>
        /// </summary>
        /// <param name="stuAttends">学生考勤列表</param>
        /// <param name="classIds">班级</param>
        private void NormalStudentAttend(List<ViewStudentScanCodeAttend> stuAttends, List<long> classIds)
        {
            //获取当前学生未考勤数据
            var studentNoAttendances = GetNowSutdentAttend(stuAttends)
                .Where(x => x.AttendStatus == (int)AttendStatus.NotClockIn)
                .ToList();

            if (classIds != null && classIds.Any())
            {
                studentNoAttendances = studentNoAttendances
                    .Where(x => classIds.Contains(x.ClassId) || classIds.Contains(x.BusinessId))
                    .ToList();
            }

            DateTime attendTime = DateTime.Now;//考勤时间

            //学生考勤表
            var normalAttends = studentNoAttendances.Where(x => x.TblSource == _tblSource1).Select(x => x.Id).ToList();
            if (normalAttends.Any())
            {
                //更新学生考勤课次表考勤数据
                _lessonStudentRepository.Value.UpdateAttendStatus(
                    normalAttends, AttendStatus.Normal, AttendStatus.NotClockIn,
                    AdjustType.DEFAULT, attendTime, AttendUserType.TEACHER);
            }

            //学生补课表
            var remedyAttends = studentNoAttendances.Where(x => x.TblSource == _tblSource2).Select(x => x.Id).ToList();
            if (remedyAttends.Any())
            {
                //更新学生补课课次表考勤数据
                _replenishLessonRepository.Value.UpdateAttendStatus(
                    remedyAttends, AttendStatus.Normal, AttendStatus.NotClockIn,
                    AdjustType.DEFAULT, attendTime, AttendUserType.TEACHER);
            }

            //微信推送
            this.PushWeChatNoticeStart(stuAttends, attendTime);

            //"{this.Student.StudentName}同学在{attendTime:HH:mm}完成考勤"
            throw new BussinessException(ModelType.Default, (ushort)ScanCodeAttendStatusResponse.SUCCESS,
                string.Format(ScanCodeAttendConstants.AttendError2, this.Student.StudentName, attendTime.ToString("HH:mm")));
        }

        /// <summary>
        /// 微信通知
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        private void PushWeChatNoticeStart(List<ViewStudentScanCodeAttend> stuAttends, DateTime attendTime)
        {
            List<long> cIds = stuAttends.Select(x => x.ClassId).Distinct().ToList();
            foreach (var item in cIds)
            {
                var temp = stuAttends.Where(x => x.ClassId == item).ToList();
                var model = temp.FirstOrDefault();
                var classTimeList = temp.Select(x => x.ClassBeginTime + "-" + x.ClassEndTime);
                string classTime = $"{model.ClassDate:yyyy.MM.dd} {string.Join("、", classTimeList)}"; //上课时间段
                var classService = new DefaultClassService(item);
                var classInfo = classService.TblDatClass;
                var classRoom = new ClassRoomService(classInfo.ClassRoomId).ClassRoomInfo.RoomNo; //教室门牌号

                var className = CourseService.GetByCourseId(classInfo.CourseId)?.ClassCnName; //班级名称

                PushWeChatNotice(className, classTime, classRoom, attendTime);
            }
        }

        /// <summary>
        /// 微信通知
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        private void PushWeChatNotice(string className, string classTime, string classRoom, DateTime attendTime)
        {
            string title = string.Format(ClientConfigManager.HssConfig.WeChatTemplateTitle.ArrivalNotice,
                this.Student.StudentName);

            var schoolName = OrgService.GetSchoolBySchoolId(_schoolId)?.SchoolName ?? string.Empty;

            WxNotifyProducerService wxNotifyProducerService = WxNotifyProducerService.Instance;
            WxNotifyInDto wxNotify = new WxNotifyInDto
            {
                Data = new List<WxNotifyItemInDto> {
                    new WxNotifyItemInDto{ DataKey="first", Value=title },
                    new WxNotifyItemInDto{ DataKey="keyword1", Value=$"{attendTime:yyyy.MM.dd HH:mm}"},
                    new WxNotifyItemInDto{ DataKey="keyword2", Value=className },
                    new WxNotifyItemInDto{ DataKey="keyword3", Value=classRoom },
                    new WxNotifyItemInDto{ DataKey="keyword4", Value=classTime},
                    new WxNotifyItemInDto{ DataKey="remark", Value=schoolName }
                },
                ToUser = StudentService.GetWxOpenId(this.Student.StudentId),
                TemplateId = WeChatTemplateConstants.ArrivalNotice,
                Url = string.Empty
            };

            wxNotifyProducerService.Publish(wxNotify);
        }
    }
}
