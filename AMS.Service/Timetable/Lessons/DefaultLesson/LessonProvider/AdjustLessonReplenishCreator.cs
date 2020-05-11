using System;
using System.Collections.Generic;
using System.Linq;
using AMS.Anticorrosion.HRS;
using AMS.Core;
using AMS.Core.Constants;
using AMS.Dto;
using AMS.Service.Hss;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;

namespace AMS.Service
{
    /// <summary>
    /// 课程调整--补课
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-14</para>
    /// </summary>
    public class AdjustLessonReplenishCreator : ILessonProvider, IReplenishLessonCreator
    {
        private readonly long _batchNo;                  //批次号
        private readonly string _schoolId;               //校区ID
        private readonly AdjustReplenishRequest _request;//补课输入参数

        private List<ViewCompleteStudentAttendance> _outLessonList; //转出课次信息
        private List<ViewCompleteStudentAttendance> _inLessonList;  //转入课次信息

        /// <summary>
        /// 业务调整记录
        /// </summary>
        private readonly List<TblTimAdjustLesson> _adjustLessons = new List<TblTimAdjustLesson>();

        private readonly TblTimLessonRepository _lessonRepository;
        private readonly ViewCompleteStudentAttendanceRepository _viewCompleteStudentAttendanceRepository;
        private readonly TblTimAdjustLessonRepository _tblTimAdjustLessonRepository;

        /// <summary>
        /// 创建补课需要的数据源服务
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="request">补课,对缺勤或请假的进行补课入参</param>
        /// <param name="unitOfWork">事物工作单元</param>
        public AdjustLessonReplenishCreator(string schoolId, AdjustReplenishRequest request, UnitOfWork unitOfWork)
        {
            this._batchNo = IdGenerator.NextId();
            this._schoolId = schoolId;
            this._request = request;

            _lessonRepository = unitOfWork.GetCustomRepository<TblTimLessonRepository, TblTimLesson>();
            _viewCompleteStudentAttendanceRepository = unitOfWork.GetCustomRepository<ViewCompleteStudentAttendanceRepository, ViewCompleteStudentAttendance>();
            _tblTimAdjustLessonRepository = unitOfWork.GetCustomRepository<TblTimAdjustLessonRepository, TblTimAdjustLesson>();
        }

        /// <summary>
        /// 补课的业务类型
        /// </summary>
        public int BusinessType => (int)LessonBusinessType.RepairLesson;

        /// <summary>
        /// 课次创建之后的回调
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        public void AfterReplenishLessonCreate()
        {
            //1.写入业务记录表
            this._tblTimAdjustLessonRepository.Add(_adjustLessons);

            //2.微信推送
            this.PushWeChatNotice();
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
            this._outLessonList = this.GetOutLessonList();

            //2.校验转出课次
            VerifyOutLesson(this._outLessonList);

            //3.转入课次信息
            this._inLessonList = this.GetInLessonList();

            //4.校验转入课次
            VerifyInLesson(this._outLessonList, this._inLessonList);

            int count = this._inLessonList.Count;

            for (int i = 0; i < count; i++)
            {
                var inLesson = this._inLessonList[i];
                var outLesson = this._outLessonList[i];
                //1.获取业务记录实体
                var adjustLesson = GetAdjustLesson(inLesson, outLesson);
                _adjustLessons.Add(adjustLesson);

                //2.获取创建课次实体
                var creatorInfo = GetReplenishLessonCreatorInfo(inLesson, outLesson, adjustLesson);
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

                StudentId = outLesson.StudentId,
                OutLessonId = outLesson.LessonId,
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
        private static void VerifyOutLesson(IReadOnlyList<ViewCompleteStudentAttendance> outLessons)
        {
            if (!outLessons.Any())
            {
                //未找到课次信息
                throw new BussinessException(ModelType.Timetable, 60);
            }

            foreach (var item in outLessons)
            {
                if ((AttendStatus)item.AttendStatus == AttendStatus.Normal)
                {
                    //课程已正常考勤不能进行补课操作
                    throw new BussinessException(ModelType.Timetable, 58);
                }

                if ((AdjustType)item.AdjustType != AdjustType.DEFAULT)
                {
                    //课程已被调整不能进行补课操作
                    throw new BussinessException(ModelType.Timetable, 59);
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
        private static void VerifyInLesson(
            IReadOnlyList<ViewCompleteStudentAttendance> outLessons,
            IReadOnlyList<ViewCompleteStudentAttendance> inLessons)
        {
            if (outLessons.Count != inLessons.Count)
            {
                //安排补课的课次与原课次不一致
                throw new BussinessException(ModelType.Timetable, 63);
            }

            var except = outLessons.Select(x => x.TermId).Except(inLessons.Select(x => x.TermId));
            if (except.Any())
            {
                //不能跨学期安排补课
                throw new BussinessException(ModelType.Timetable, 62);
            }
        }

        /// <summary>
        /// 微信通知
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        private void PushWeChatNotice()
        {
            var stuInfo = StudentService.GetStudentInfo(_request.StudentId);
            var inLesson = this._inLessonList.FirstOrDefault();

            string pushClassTime = this._outLessonList
                .Select(x => DateTime.Parse(x.ClassDate.ToString("yyyy-MM-dd") + " " + x.ClassBeginTime))
                .Min().ToString("yyyy.MM.dd HH:mm");

            //first={0}家长，您好！因{1}于{2}请假，为了保证孩子的学习效果，我们特别为孩子安排了补课，具体时间如下：
            string title =
                string.Format(
                ClientConfigManager.HssConfig.WeChatTemplateTitle.MakeupNotice,
                stuInfo.StudentName, stuInfo.StudentName, pushClassTime);

            //keyword1=上课时间
            var classTimeList = this._inLessonList.Select(x => x.ClassBeginTime + "-" + x.ClassEndTime);
            string classTime = $"{inLesson.ClassDate.ToString("yyyy.MM.dd")} {string.Join("、", classTimeList)}";

            //keyword2=班级名称
            string className = CourseService.GetByCourseId(inLesson.CourseId)?.ClassCnName ?? string.Empty;

            //keyword3=教室
            string classRoom = new ClassRoomService(inLesson.ClassRoomId)?.ClassRoomInfo?.RoomNo ?? string.Empty;

            //keyword4=老师名称
            string teacherName = TeachService.GetTeacher(inLesson.TeacherId)?.TeacherName ?? string.Empty;

            //remark=校区名称
            string schoolName = OrgService.GetSchoolBySchoolId(_schoolId)?.SchoolName ?? string.Empty;

            WxNotifyProducerService wxNotifyProducerService = WxNotifyProducerService.Instance;
            WxNotifyInDto wxNotify = new WxNotifyInDto
            {
                Data = new List<WxNotifyItemInDto> {
                    new WxNotifyItemInDto{ DataKey="first", Value=title},
                    new WxNotifyItemInDto{ DataKey="keyword1", Value=classTime},
                    new WxNotifyItemInDto{ DataKey="keyword2", Value=className},
                    new WxNotifyItemInDto{ DataKey="keyword3", Value=classRoom},
                    new WxNotifyItemInDto{ DataKey="keyword4", Value=teacherName },
                    new WxNotifyItemInDto{ DataKey="remark", Value=schoolName}
                },
                ToUser = StudentService.GetWxOpenId(stuInfo),
                TemplateId = WeChatTemplateConstants.MakeupNotice,
                Url = string.Empty
            };

            wxNotifyProducerService.Publish(wxNotify);
        }
    }
}
