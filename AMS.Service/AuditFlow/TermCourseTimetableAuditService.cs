using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Core;
using AMS.Dto;
using AMS.Models;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using FP3.Core.ServiceInput;
using Jerrisoft.Platform.Log;

namespace AMS.Service
{
    /// <summary>
    /// 表示一个排课表的审核实例
    /// </summary>
    public class TermCourseTimetableAuditService : BaseAuditService
    {
        private readonly Lazy<TblAutClassRepository> _tblAutClassRepository = new Lazy<TblAutClassRepository>();            //审核班级表
        private readonly Lazy<TblAutClassTimeRepository> _tblAutClassTimeRepository = new Lazy<TblAutClassTimeRepository>();//审核班级上课时表
        private readonly long _termId;                                                                                      //学期Id

        /// <summary>
        /// 描述：表示一个学期最近的课表审核数据
        /// </summary>
        /// <param name="termId">学期主键</param>
        public TermCourseTimetableAuditService(long termId)
        {
            this._termId = termId;
            base.TblAutAudit = base._tblAutAuditRepository.GetTblAutAuditByExtField(AuditBusinessType.TermCourseTimetable, termId.ToString(), null);
        }

        /// <summary>
        /// 描述：根据审核主表实例化学期审核数据
        /// </summary>
        /// <param name="tblAutAudit">审核实体</param>
        private TermCourseTimetableAuditService(TblAutAudit tblAutAudit)
        {
            base.TblAutAudit = tblAutAudit;
            this._termId = long.Parse(base.TblAutAudit.ExtField1);
        }

        /// <summary>
        /// 描述：根据审核主健创建一个实例
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-1-2</para>
        /// </summary>
        /// <param name="auditId">审核主表Id</param>
        /// <returns>排课表的审核实例</returns>        
        public static TermCourseTimetableAuditService CreateByAutitId(long auditId)
        {
            TblAutAuditRepository repository = new TblAutAuditRepository();
            var entity = repository.Load(auditId);
            return new TermCourseTimetableAuditService(entity);
        }

        /// <summary>
        /// 最新一条学期排课还未通过审核
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-26</para>
        /// </summary>
        /// <remarks>true:通过 false:未通过</remarks>
        public bool NoPass
        {
            get
            {
                if (base.TblAutAudit == null)
                {
                    return false;
                }
                AuditStatus autAudit = (AuditStatus)base.TblAutAudit.AuditStatus;

                return autAudit != AuditStatus.Success;
            }
        }

        #region GetNoPassClassTimetable 获取未通过审核的班级课表详情

        /// <summary>
        /// 获取未通过审核的班级课表详情
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-22</para>
        /// <para></para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <returns>一个班级的课程表详细信息</returns>
        /// <exception cref="BussinessException">
        /// 异常ID：1,异常描述：班级信息对象为空
        /// </exception>
        public async Task<ClassTimetableResponse> GetNoPassClassTimetable(long classId)
        {
            //获取班级
            var classInfo = await _tblAutClassRepository.Value.GetAsync(base.TblAutAudit.AuditId, classId);

            if (classInfo == null)
            {
                throw new BussinessException(ModelType.Default, 1);
            }

            //基础数据
            ClassTimetableResponse res = new ClassTimetableResponse()
            {
                ClassNo = classInfo.ClassNo,
                CourseId = classInfo.CourseId,
                CourseLevelId = classInfo.CourseLeveId,
                CourseNum = classInfo.CourseNum,
                StudentsNum = classInfo.StudentsNum,
                TeaherId = classInfo.TeacherId,
                CanClassJoin = false,
                WeekDaySchoolTimes = new List<ClassTimetableSchoolTimeResponse>()
            };

            SchoolTimeService schoolTimeService = new SchoolTimeService(_termId);
            //获取学期信息
            TblDatTerm term = schoolTimeService.TblDatTerm;
            //这个学期所有的上课时间
            var allTermClassTime = schoolTimeService.TblDatSchoolTime.OrderBy(x => x.BeginTime).ToList();
            //校区老师
            res.Teacher = TeachService.GetIncumbentTeachers(term.SchoolId, classInfo.TeacherId);

            //课程信息与课程等级
            res.ClassTimetableCourse = ClassCourseTimetableService.GetClassTimetableCourseResponse(term.SchoolId, classInfo.ClassRoomId, classInfo.CourseId);

            //当前教室门牌号
            res.RoomNo = new SchoolClassRoomService(term.SchoolId).GetClassRoom(classInfo.ClassRoomId)?.RoomNo;

            var timClassTime = await _tblAutClassTimeRepository.Value.GetByClassId(base.TblAutAudit.AuditId, classInfo.ClassId);

            //当前班级对应的上课时间段
            var currentClassTimes = allTermClassTime
                .Where(m => timClassTime.Select(x => x.SchoolTimeId).Contains(m.SchoolTimeId))
                .OrderBy(x => x.BeginTime).ToList();

            //班级第一个时间段
            var firstClassTime = currentClassTimes[0];
            res.Time1 = new List<string> { firstClassTime.BeginTime, firstClassTime.EndTime };
            res.Time2 = new List<string>();
            var isJoinClass = currentClassTimes.Where(x => x.WeekDay == firstClassTime.WeekDay).OrderBy(x => x.BeginTime).ToList();

            //班级第二个时间段
            if (isJoinClass.Count > 1)
            {
                var secondClassTime = isJoinClass[1];
                res.Time2 = new List<string> { secondClassTime.BeginTime, secondClassTime.EndTime };
                res.IsClassJoin = true;
            }

            //星期几与上课时间的数据
            for (int i = 1; i <= 7; i++)
            {
                var classTime = currentClassTimes.Where(x => x.WeekDay == i).ToList();
                ClassTimetableSchoolTimeResponse ctst = new ClassTimetableSchoolTimeResponse
                {
                    HasSchoolTime1 = false,
                    SchoolTimeId1 = 0,
                    HasSchoolTime2 = false,
                    SchoolTimeId2 = 0,
                    IsChecked = false,
                    WeekDay = i
                };
                if (classTime.Any())
                {
                    ctst.HasSchoolTime1 = true;
                    ctst.SchoolTimeId1 = classTime[0].SchoolTimeId;
                    ctst.IsChecked = true;

                    if (classTime.Count > 1)
                    {
                        ctst.HasSchoolTime2 = true;
                        ctst.SchoolTimeId2 = classTime[1].SchoolTimeId;
                    }
                }
                res.WeekDaySchoolTimes.Add(ctst);
            }
            return res;
        }
        #endregion

        #region AddClass/ModifyClassAsync/DeleteClass 学期课程管理

        /// <summary>
        /// 添加班级排课
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-25</para>
        /// </summary>
        /// <param name="classRoomId">教室Id</param>
        /// <param name="schoolTimeId">时间段Id</param>
        /// <param name="request">班级排课数据源</param>
        /// <param name="createUserName">创建人用户名称</param>
        /// <returns></returns>
        public static void AddClass(long classRoomId, long schoolTimeId, ClassScheduleRequest request, string createUserName)
        {
            TblAutClass autClass = GetTblAutClass(request, classRoomId, schoolTimeId, createUserName);

            List<long> hasBeenClassTimeIds = GetHasBeenClassTimeIds(autClass.AuditId, classRoomId);

            List<TblAutClassTime> autClassTimes = GetTblAutClassTimes(request, autClass, hasBeenClassTimeIds, schoolTimeId);

            //校验老师是否重复
            VerifyTeacherOccupy(autClassTimes.Select(x => x.SchoolTimeId), request.TeacherId, autClass.TermId);

            Task[] tasks = new Task[2];
            tasks[0] = new TblAutClassRepository().AddTask(autClass);
            tasks[1] = new TblAutClassTimeRepository().SaveTask(autClassTimes);
            Task.WaitAll(tasks);
        }

        /// <summary>
        /// 待添加到审核中上课时间段
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-23</para>
        /// </summary>
        /// <param name="request">待添加的班级信息</param>
        /// <param name="autClass">审核中的班级</param>
        /// <param name="hasBeenClassTimeIds">一组已排课的上课时间Id</param>
        /// <param name="schoolTimeId">要添加的上课时间段Id</param>
        /// <returns>班级上课时间列表</returns>
        /// <exception cref="BussinessException">
        /// 异常ID:2,异常描述：没有此时间段;时间段已占用;不能与下一个时间段连上;下一个时间段已排课;
        /// </exception>
        private static List<TblAutClassTime> GetTblAutClassTimes(ClassScheduleRequest request, TblAutClass autClass, List<long> hasBeenClassTimeIds, long schoolTimeId)
        {
            List<TblAutClassTime> res = new List<TblAutClassTime>();

            List<TblDatSchoolTime> schoolTimes = new SchoolTimeService(autClass.TermId)
                .TblDatSchoolTime
                .OrderBy(x => x.BeginTime)
                .ToList();

            TblDatSchoolTime schoolTime = schoolTimes.FirstOrDefault(x => x.SchoolTimeId == schoolTimeId);
            ValidateObject(schoolTime);

            foreach (var weekDay in request.WeekDay)
            {
                var dayAllClassTime = schoolTimes
                    .Where(x => x.WeekDay == weekDay && x.Duration == schoolTime.Duration)
                    .OrderBy(x => x.BeginTime)
                    .ToList();

                var firstClassTime = dayAllClassTime.FirstOrDefault(x => x.BeginTime == schoolTime.BeginTime && x.EndTime == schoolTime.EndTime);

                if (firstClassTime == null)
                {
                    throw new BussinessException(ModelType.Default, 2, $"{WeekDayConvert.IntToString(weekDay)}没有此时间段");
                }

                //判断是否有排课
                if (hasBeenClassTimeIds.Any(x => x == firstClassTime.SchoolTimeId))
                {
                    throw new BussinessException(ModelType.Default, 2, $"{WeekDayConvert.IntToString(weekDay)}时间段已占用");
                }

                res.Add(new TblAutClassTime
                {
                    AutClassTimeId = IdGenerator.NextId(),
                    SchoolId = autClass.SchoolId,
                    ClassTimeId = IdGenerator.NextId(),
                    AuditId = autClass.AuditId,
                    ClassId = autClass.ClassId,
                    CreateTime = DateTime.Now,
                    DataStatus = 0,
                    SchoolTimeId = firstClassTime.SchoolTimeId
                });

                if (schoolTime.Duration == (int)TimeType.Ninety && request.IsLink)
                {
                    int firstIndex = dayAllClassTime.IndexOf(firstClassTime);
                    int secondIndex = firstIndex + 1;

                    //最后一个时间段
                    if (secondIndex >= dayAllClassTime.Count)
                    {
                        throw new BussinessException(ModelType.Default, 2, $"{WeekDayConvert.IntToString(weekDay)}不能与下一个时间段连上");
                    }

                    var secondClassTime = dayAllClassTime[secondIndex];

                    //判断是否有排课
                    if (hasBeenClassTimeIds.Any(x => x == secondClassTime.SchoolTimeId))
                    {
                        throw new BussinessException(ModelType.Default, 2, $"{WeekDayConvert.IntToString(weekDay)}下一个时间段已排课");
                    }

                    res.Add(new TblAutClassTime
                    {
                        AutClassTimeId = IdGenerator.NextId(),
                        ClassTimeId = IdGenerator.NextId(),
                        AuditId = autClass.AuditId,
                        ClassId = autClass.ClassId,
                        CreateTime = DateTime.Now,
                        DataStatus = 0,
                        SchoolId = autClass.SchoolId,
                        SchoolTimeId = secondClassTime.SchoolTimeId
                    });
                }
            }
            return res;
        }

        /// <summary>
        /// 获取已上课的上课时间段Id
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-23</para>
        /// </summary>
        /// <param name="auditId">审核Id</param>
        /// <param name="classRoomId">教室Id</param>
        /// <returns></returns>
        private static List<long> GetHasBeenClassTimeIds(long auditId, long classRoomId)
        {
            var autClasss = new TblAutClassRepository().GetByAuditId(auditId).Result;
            var autClassTimes = new TblAutClassTimeRepository().GetByAuditIdAsync(auditId).Result;

            var hasBeenClassTimeId = from a in autClasss                                        //班级
                                     join b in autClassTimes on a.ClassId equals b.ClassId      //班级上课时间
                                     where a.ClassRoomId == classRoomId
                                     select b.SchoolTimeId;

            return hasBeenClassTimeId.ToList();
        }

        /// <summary>
        /// 待添加到审核中班级
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-19</para>
        /// </summary>
        /// <param name="input">待添加的班级信息</param>
        /// <param name="classRoomId">教室Id</param>
        /// <param name="schoolTimeId">上课时间段Id</param>
        /// <param name="createUserName">创建人</param>
        /// <returns>审核中的班级课表信息</returns>
        private static TblAutClass GetTblAutClass(ClassScheduleRequest input, long classRoomId, long schoolTimeId, string createUserName)
        {
            //教室信息
            var classRoom = new ClassRoomService(classRoomId).ClassRoomInfo;
            ValidateObject(classRoom);

            //上课时间信息
            var schoolTime = SchoolTimeService.GetBySchoolTimeId(schoolTimeId);
            ValidateObject(schoolTime);

            //根据学期编号获取学期信息
            TblDatTerm term = TermService.GetTermByTermId(schoolTime.TermId);
            ValidateObject(term);

            //获取课程
            TblDatRoomCourse roomCourse = new ClassRoomService(classRoomId).GetByCourseId(input.CourseId).Result;
            ValidateObject(roomCourse);

            //复制课表到审核中得到审核主表id
            long auditId = new TermCourseTimetableAuditService(schoolTime.TermId).VerifyGetAuditId(classRoom.SchoolId);

            TblAutClass autClass = new TblAutClass
            {
                AutClassId = IdGenerator.NextId(),
                SchoolId = classRoom.SchoolId,
                ClassId = IdGenerator.NextId(),
                AuditId = auditId,
                ClassNo = input.ClassNo,
                TermId = term.TermId,
                RoomCourseId = roomCourse.RoomCourseId,
                ClassRoomId = classRoomId,
                CourseId = roomCourse.CourseId,
                CourseLeveId = input.CourseLevelId,
                TeacherId = input.TeacherId,
                CourseNum = input.CourseNum,
                StudentsNum = input.StudentsNum,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                DataStatus = 0
            };

            return autClass;
        }

        /// <summary>
        /// 校验对象是否为空
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-19</para>
        /// </summary>
        /// <exception cref="BussinessException">
        /// 异常ID：1,异常描述：校验的对象为空
        /// </exception>
        private static void ValidateObject(object value)
        {
            if (value == null)
            {
                throw new BussinessException(ModelType.Default, 2);
            }
        }

        /// <summary>
        /// 修改班级课表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-25</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <param name="request">学期班级排课课表修改</param>
        /// <param name="createUserName">创建人用户名称</param>
        /// <returns></returns>
        public async Task ModifyClassAsync(long classId, EditClassScheduleRequest request, string createUserName)
        {
            //1.校验并获取审核Id
            long auditId = VerifyGetAuditId(base.TblAutAudit.SchoolId);

            //2.获取待修改的班级
            var waitEditClass = await _tblAutClassRepository.Value.GetAsync(auditId, classId);
            if (waitEditClass == null)
            {
                throw new BussinessException(ModelType.Default, 1);
            }

            //3.判断老师是否修改,有则校验老师上课时间段是否冲突
            if (waitEditClass.TeacherId != request.TeacherId)
            {
                var classTime = _tblAutClassTimeRepository.Value.GetByClassId(auditId, classId).Result.Select(x => x.SchoolTimeId);

                TblDatSchoolTime schoolTime = SchoolTimeService.GetBySchoolTimeId(classTime.FirstOrDefault());

                //校验老师是否重复
                VerifyTeacherOccupy(classTime, request.TeacherId, schoolTime.TermId);
            }

            //4.准备数据
            waitEditClass.CourseId = request.CourseId;
            waitEditClass.CourseLeveId = request.CourseLevelId;
            waitEditClass.ClassNo = request.ClassNo;
            waitEditClass.TeacherId = request.TeacherId;
            waitEditClass.CourseNum = request.CourseNum;
            waitEditClass.StudentsNum = request.StudentsNum;

            //5.写入更新的班级数据
            await _tblAutClassRepository.Value.UpdateTask(waitEditClass);
        }

        /// <summary>
        /// 删除班级
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-25</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <param name="createUserName">创建人名称</param>
        /// <exception cref="BussinessException">
        /// 异常ID：20,异常描述：班级已被排课不能进行删除
        /// </exception>
        public void DeleteClass(long classId, string createUserName)
        {
            //验证班级是否排课
            bool classIsUse = LessonService.ClassIsUse(classId);
            if (classIsUse)
            {
                //班级已使用,不能删除
                throw new BussinessException(ModelType.Datum, 20);
            }

            //审核Id
            long auditId = VerifyGetAuditId(base.TblAutAudit.SchoolId);

            var autClass = _tblAutClassRepository.Value.GetAsync(auditId, classId).Result;

            //删除班级课表数据
            _tblAutClassRepository.Value.DeleteByClassId(auditId, autClass.ClassId);
            //删除班级上课时间
            _tblAutClassTimeRepository.Value.DeleteByClassId(auditId, autClass.ClassId);

            //校验是否为最后一个班级
            this.VerifyDeleteClassFallback(auditId);
        }

        /// <summary>
        /// 校验审核班级记录表是否存在班级
        /// 没有班级则删除主表审核记录
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-11</para>
        /// </summary>
        /// <param name="auditId">审核Id</param>
        private void VerifyDeleteClassFallback(long auditId)
        {
            //1、是否还存在班级
            List<TblAutClass> autClasses = _tblAutClassRepository.Value.GetByAuditId(auditId).Result;
            if (autClasses.Any())
            {
                return;
            }

            //2、是否存在正式数据
            bool existClass = new TermCourseTimetableService(this._termId).ExistClassCourse();
            if (existClass)
            {
                return;
            }

            //3、删除审核主表数据
            var autAudit = _tblAutAuditRepository.Load(auditId);
            if (autAudit != null)
            {
                _tblAutAuditRepository.Delete(autAudit);
            }
        }

        #endregion

        #region VerifyGetAuditId 校验并获取审核Id
        /// <summary>
        /// 校验并获取审核Id
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-25</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <returns>审核Id</returns>
        private long VerifyGetAuditId(string schoolId)
        {
            TblAutAudit audit = base.TblAutAudit;
            if (audit == null)
            {
                audit = new TblAutAudit
                {
                    AuditId = IdGenerator.NextId(),
                    SchoolId = schoolId,
                    CreateTime = DateTime.Now,
                    AuditDate = DateTime.Now,
                    AuditStatus = (int)AuditStatus.WaitAudit,
                    AuditUserId = string.Empty,
                    AuditUserName = string.Empty,
                    BizType = (int)AuditBusinessType.TermCourseTimetable,
                    DataExt = string.Empty,
                    DataExtVersion = string.Empty,
                    CreateUserId = "",
                    CreateUserName = "",
                    ExtField1 = _termId.ToString(),
                    ExtField2 = string.Empty,
                    FlowNo = string.Empty,
                    UpdateTime = DateTime.Now
                };
                _tblAutAuditRepository.Add(audit);
            }

            switch ((AuditStatus)audit.AuditStatus)
            {
                case AuditStatus.WaitAudit: //待审核
                    break;
                case AuditStatus.Auditing:  //审核中
                    throw new BussinessException(ModelType.Timetable, 5);//课表正在审核中,不可操作
                case AuditStatus.Success:   //审核通过
                    audit.AuditId = CopyClassToAutClass();
                    break;
                case AuditStatus.Return:    //审核不通过
                    audit.AuditId = AuditFailResetPendingAudit(audit.AuditId);
                    break;
            }
            return audit.AuditId;
        }

        /// <summary>
        /// 审核不通过重置待审核
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-10</para>
        /// </summary>
        /// <returns>审核Id</returns>
        private long AuditFailResetPendingAudit(long auditId)
        {
            TblAutAudit autAudit = _tblAutAuditRepository.Load(auditId);
            if (autAudit == null)
            {
                throw new ArgumentNullException(nameof(autAudit));
            }
            autAudit.AuditStatus = (int)AuditStatus.WaitAudit;
            autAudit.UpdateTime = DateTime.Now;

            _tblAutAuditRepository.Update(autAudit);

            return autAudit.AuditId;
        }

        /// <summary>
        /// 复制正式学期排课数据到草稿
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-25</para>
        /// </summary>
        /// <returns>得到审核Id</returns>
        private long CopyClassToAutClass()
        {
            //获取所有班级信息
            var classes = DefaultClassService.GetClasssByTermId(_termId);
            //获取班级所有上课时间段
            var classTime = DefaultClassService.GetClassTimeByClassId(classes.Select(x => x.ClassId));

            TblAutAudit autAudit = new TblAutAudit
            {
                AuditId = IdGenerator.NextId(),
                CreateTime = DateTime.Now,
                AuditDate = DateTime.Now,
                AuditStatus = (int)AuditStatus.WaitAudit,
                AuditUserId = string.Empty,
                AuditUserName = string.Empty,
                BizType = (int)AuditBusinessType.TermCourseTimetable,
                DataExt = string.Empty,
                DataExtVersion = string.Empty,
                ExtField1 = _termId.ToString(),
                ExtField2 = string.Empty,
                FlowNo = string.Empty,
                UpdateTime = DateTime.Now,
                SchoolId = base.TblAutAudit.SchoolId,
                CreateUserId = string.Empty,
                CreateUserName = string.Empty
            };

            List<TblAutClass> autClasses = classes.Select(x => new TblAutClass
            {
                AutClassId = IdGenerator.NextId(),
                AuditId = autAudit.AuditId,
                ClassId = x.ClassId,
                ClassNo = x.ClassNo,
                ClassRoomId = x.ClassRoomId,
                CourseId = x.CourseId,
                CourseLeveId = x.CourseLeveId,
                CourseNum = x.CourseNum,
                CreateTime = DateTime.Now,
                DataStatus = 0,
                RoomCourseId = x.RoomCourseId,
                StudentsNum = x.StudentsNum,
                TeacherId = x.TeacherId,
                TermId = x.TermId,
                UpdateTime = DateTime.Now,
                SchoolId = base.TblAutAudit.SchoolId
            }).ToList();

            List<TblAutClassTime> autClassTime = classTime.Select(x => new TblAutClassTime
            {
                AutClassTimeId = IdGenerator.NextId(),
                AuditId = autAudit.AuditId,
                ClassId = x.ClassId,
                ClassTimeId = x.ClassTimeId,
                CreateTime = DateTime.Now,
                DataStatus = 0,
                SchoolTimeId = x.SchoolTimeId,
                SchoolId = base.TblAutAudit.SchoolId
            }).ToList();

            //TODO:事物
            Task[] tasks = new Task[3];
            tasks[0] = _tblAutAuditRepository.AddTask(autAudit);
            tasks[1] = _tblAutClassRepository.Value.SaveTask(autClasses);
            tasks[2] = _tblAutClassTimeRepository.Value.SaveTask(autClassTime);

            Task.WaitAll(tasks);

            return autAudit.AuditId;
        }
        #endregion

        #region GetAlreadyClassTime 获取已排课时间段
        /// <summary>
        /// 获取已排课的上课时间段
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="classRoomId">教室Id</param>
        /// <returns>上课时间段Id集合</returns>
        public List<long> GetAlreadyClassTime(long classRoomId)
        {
            List<TblAutClass> autClass = _tblAutClassRepository.Value.GetByAuditId(base.TblAutAudit.AuditId)
                .Result
                .Where(x => x.ClassRoomId == classRoomId)
                .ToList();

            List<long> classIds = autClass.Select(x => x.ClassId).ToList();

            List<TblAutClassTime> autClassTime = _tblAutClassTimeRepository.Value.GetByAuditIdAsync(base.TblAutAudit.AuditId)
                .Result
                .Where(x => classIds.Contains(x.ClassId))
                .ToList();

            return autClassTime.Select(x => x.SchoolTimeId).Distinct().ToList();
        }
        #endregion

        #region VerifyTeacherOccupy 校验老师在同一时间段不同教室是否存在

        /// <summary>
        /// 校验老师在同一时间段不同教室是否存在
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="schoolTimeId">时间段</param>
        /// <param name="notClassId">排除的班级Id</param>
        /// <param name="duration">时长</param>
        /// <param name="teacherId">老师Id</param>
        /// <param name="termId">学期Id</param>
        /// <exception cref="BussinessException">
        /// 异常ID：10,异常描述：老师上课时间冲突
        /// </exception>
        private static void VerifyTeacherOccupy(IEnumerable<long> inputSchoolTimeId, string teacherId, long termId)
        {
            //1.获取老师上课段
            TermCourseTimetableAuditService termCourseTimetableAuditService = new TermCourseTimetableAuditService(termId);

            List<long> teacherClassTimes = termCourseTimetableAuditService.GetAutClassTime(teacherId)
                .Select(x => x.SchoolTimeId)
                .ToList();

            if (!teacherClassTimes.Any())
            {
                return; //该老师未排课
            }

            //2.获取重叠的上课时间段
            var termSchoolTimes = new SchoolTimeService(termCourseTimetableAuditService._termId).TblDatSchoolTime.ToList();

            List<long> overlappingSchoolTime = OverlappingSchoolTime(termSchoolTimes, inputSchoolTimeId);

            //3.比较
            var intersectedList = teacherClassTimes.Intersect(overlappingSchoolTime);

            if (intersectedList.Any())
            {
                throw new BussinessException(ModelType.Timetable, 10);  //老师上课时间冲突
            }
        }

        /// <summary>
        /// 重叠的上课时间段Id
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-19</para>
        /// </summary>
        /// <param name="termSchoolTimes">学期下所有上课时间段信息</param>
        /// <param name="inputSchoolTimeId">需要检测重叠的上课时间Id</param>
        /// <returns>重叠上课时间段Id</returns>
        private static List<long> OverlappingSchoolTime(List<TblDatSchoolTime> termSchoolTimes, IEnumerable<long> inputSchoolTimeId)
        {
            //待返回的重叠上课时间段Id
            List<long> overlappingSchoolTime = new List<long>();

            var inputSchoolTimes = termSchoolTimes.Where(x => inputSchoolTimeId.Contains(x.SchoolTimeId)).ToList();

            foreach (var item in inputSchoolTimes)
            {
                //当天上课所有时间段
                var daySchoolTimes = termSchoolTimes.Where(x => x.WeekDay == item.WeekDay).ToList();

                //当天相同时间段
                var same = daySchoolTimes
                    .Where(x => x.BeginTime == item.BeginTime && x.EndTime == item.EndTime)
                    .Select(x => x.SchoolTimeId);

                //当天交叉时间段
                var cross = new List<long>();

                DateTime sTime = DateTime.Parse(item.BeginTime);
                DateTime eTime = DateTime.Parse(item.EndTime);

                foreach (var x in daySchoolTimes)
                {
                    DateTime xStime = DateTime.Parse(x.BeginTime);
                    DateTime eStime = DateTime.Parse(x.EndTime);

                    //08:00<=08:10 && 08:10<=09:00
                    if (sTime <= xStime && xStime <= eTime)
                    {
                        cross.Add(x.SchoolTimeId);
                    }

                    //08:10<=08:00 && 08:00<=09:30
                    if (xStime <= sTime && sTime <= eStime)
                    {
                        cross.Add(x.SchoolTimeId);
                    }
                }

                overlappingSchoolTime.AddRange(same);  //把相同时间段Id加入重叠集合
                overlappingSchoolTime.AddRange(cross); //把交叉时间段Id加入重叠集合
            }

            return overlappingSchoolTime.Distinct().ToList();
        }
        #endregion

        #region GetAutClassTime 获取老师最新审核中上课时间段
        /// <summary>
        /// 获取老师所有的审核中上课时间段
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="teacherId">老师Id</param>
        /// <returns>班级上课表信息</returns>
        private List<TblAutClassTime> GetAutClassTime(string teacherId)
        {

            //1、查询老师所有班级
            List<TblAutClass> autClassList = _tblAutClassRepository.Value.GetByAuditId(this.TblAutAudit.AuditId).Result;
            List<long> classId = autClassList.Where(t => t.TeacherId == teacherId)
                        .Select(t => t.ClassId)
                        .ToList();

            if (!classId.Any())
            {
                return new List<TblAutClassTime>();
            }

            //2、查询老师所在班级的时间段
            List<TblAutClassTime> resultList = _tblAutClassTimeRepository.Value.GetByClassId(this.TblAutAudit.AuditId, classId).Result;

            return resultList;
        }
        #endregion

        #region TermTimetable 学期排课数据(审核中)
        //当前学期的课程表信息
        private TermTimetableResponse _termTimetable;

        /// <summary>
        /// 学期排课数据(审核中)
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-11</para>
        /// </summary>
        public TermTimetableResponse TermTimetable => _termTimetable ?? (_termTimetable = this.GetTermCourseTimetable());

        /// <summary>
        /// 获取学期排课表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-21</para>
        /// </summary>
        /// <returns>学期排课表数据</returns>
        private TermTimetableResponse GetTermCourseTimetable()
        {
            TermTimetableResponse res = new TermTimetableResponse
            {
                CourseTimetableSixty = this.GetTermCourseTimetable(TimeType.Sixty),
                CourseTimetableNinety = this.GetTermCourseTimetable(TimeType.Ninety),

                TermTimetableAuditResponse = new TermTimetableAuditResponse
                {
                    AuditStatus = (AuditStatus)base.TblAutAudit.AuditStatus,
                    Auditime = base.TblAutAudit.AuditDate.ToString("yyyy-MM-dd HH:mm"),
                    AuditName = base.TblAutAudit.AuditUserName,
                    Remark = GetAuditRemark()
                }
            };

            TblDatTerm term = TermService.GetTermByTermId(_termId);
            res.TermBeginDate = term.BeginDate.ToString("yyyy-MM-dd");
            res.TermEndDate = term.EndDate.ToString("yyyy-MM-dd");

            //如果被退回，退回原因应从DataExt中取
            if (res.TermTimetableAuditResponse.AuditStatus == AuditStatus.Return)
            {
                res.TermTimetableAuditResponse.IsFirstSubmitUser = base.IsFlowSubmitUser;
                res.TermTimetableAuditResponse.CreateUserName = base.TblAutAudit.CreateUserName;
                res.TermTimetableAuditResponse.Remark = base.TblAutAudit.DataExt;
            }
            return res;
        }

        /// <summary>
        /// 获取审核中备注信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <returns>备注信息</returns>
        private string GetAuditRemark()
        {
            string res = string.Empty;
            switch ((AuditStatus)base.TblAutAudit.AuditStatus)
            {
                case AuditStatus.WaitAudit:
                    res = "温馨提示：课表有修改，请提交审核！";
                    break;
                case AuditStatus.Auditing:
                    res = "温馨提示：课表审核中！";
                    break;
                case AuditStatus.Success:
                    res = "温馨提示：审核通过！";
                    break;
                case AuditStatus.Return:
                    res = $"温馨提示：{base.TblAutAudit.DataExt}";
                    break;
            }
            return res;
        }
        #endregion

        #region  GetTermCourseTimetable 获取不同时间段的课表信息
        /// <summary>
        /// 获取不同时间段的课表信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-21</para>
        /// </summary>
        /// <param name="duration">60分钟或90分钟</param>
        /// <returns>课表信息</returns>
        private TermCourseTimetableResponse GetTermCourseTimetable(TimeType duration)
        {
            TermCourseTimetableResponse res = new TermCourseTimetableResponse();

            var schTimeRes = this.GetTimetableSchoolTimeResponse(duration);

            res.SchoolTimes = schTimeRes.TimetableSchoolTimeResponses;                //上课时间表
            res.ClassRooms = new List<TimetableClassRoomResponse>();          //教室信息

            //审核中所有班级
            var classes = _tblAutClassRepository.Value.GetByAuditId(base.TblAutAudit.AuditId).Result;

            //校区
            var schoolId = TermService.GetTermByTermId(_termId)?.SchoolId;

            //班级上课时间表
            var classTimes = new TblAutClassTimeRepository()
                .GetBySchoolTimeId(schTimeRes.TblDatSchoolTimes.Select(x => x.SchoolTimeId), base.TblAutAudit.AuditId)
                .Result;

            //获取老师用户信息
            var teachers = TeachService.GetTeachers();

            //所有的课程
            var courses = CourseService.GetAllAsync().Result;

            //课程等级
            var courseLv = CourseLevelService.GetCourseLevelList().Result;

            //获取校区教室
            var durationClassId = classTimes.Select(x => x.ClassId).ToList();
            SchoolClassRoomService schoolClassRoomService = new SchoolClassRoomService(schoolId);
            List<RoomCourseResponse> roomCourseResponses = schoolClassRoomService.RoomCourseList;

            List<long> classRoomIds = classes.Where(x => durationClassId.Contains(x.ClassId)).Select(x => x.ClassRoomId).ToList();
            classRoomIds.AddRange(roomCourseResponses.Where(x => !x.IsDisabled).Select(x => x.ClassRoomId));

            List<TblDatClassRoom> classRooms = schoolClassRoomService
                .ClassRoomList
                .Where(x => classRoomIds.Contains(x.ClassRoomId))
                .OrderBy(o => o.RoomNo, new NaturalStringComparer())
                .ToList();

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
                foreach (var schoolTime in schTimeRes.TblDatSchoolTimes)
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

                    var classIds = (from a in classTimes
                                    join b in classes on a.ClassId equals b.ClassId
                                    where a.SchoolTimeId == schoolTime.SchoolTimeId && b.ClassRoomId == classRoom.ClassRoomId
                                    select a.ClassId).ToList();

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
        /// <para>创建时间：2018-09-19</para>
        /// </summary>
        /// <returns>上课时间段信息</returns>
        private TupleSchoolTime GetTimetableSchoolTimeResponse(TimeType duration)
        {
            TupleSchoolTime res = new TupleSchoolTime
            {
                TblDatSchoolTimes = new SchoolTimeService(this._termId)
                    .TblDatSchoolTime
                    .Where(x => x.Duration == (int)duration)
                    .OrderBy(x => x.WeekDay)
                    .ThenBy(x => x.BeginTime)
                    .ToList(),

                TimetableSchoolTimeResponses = new List<TimetableSchoolTimeResponse>()
            };

            for (int i = 1; i <= 7; i++)
            {
                TimetableSchoolTimeResponse schoolTime = new TimetableSchoolTimeResponse
                {
                    WeekDay = WeekDayConvert.IntToString(i),
                    Times = new List<SchoolTimePeriodResponse>()
                };

                var weekDayTime = res.TblDatSchoolTimes
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

                //如果该星期没有时间，则不加入该数据
                if (schoolTime.Times != null && schoolTime.Times.Count > 0)
                {
                    res.TimetableSchoolTimeResponses.Add(schoolTime);
                }
            }

            return res;
        }

        /// <summary>
        /// 上课时间段信息
        /// </summary>
        private class TupleSchoolTime
        {
            /// <summary>
            /// 排课的上课时间段
            /// </summary>
            public List<TimetableSchoolTimeResponse> TimetableSchoolTimeResponses { get; set; }

            /// <summary>
            /// 定义的上课时间段
            /// </summary>
            public List<TblDatSchoolTime> TblDatSchoolTimes { get; set; }
        }

        #endregion

        #region  CreateTermTimetable 复制课表数据提交
        /// <summary>
        /// 复制课表数据提交
        /// 作     者:Huang GaoLiang 2018年9月26日15:22:05
        /// </summary>
        /// <param name="autAudit">审核表</param>
        /// <param name="datSchoolTimeList">上课时间段集合</param>
        /// <param name="autClassList">审核中数据-班级表集合</param>
        /// <param name="autClassTimeList">审核中数据-班级上课时间表</param>
        /// <param name="toTermId">学期编号</param>
        internal static async Task CreateTermTimetable(TblAutAudit autAudit, List<TblDatSchoolTime> datSchoolTimeList, List<TblAutClass> autClassList, List<TblAutClassTime> autClassTimeList, long toTermId)
        {
            await new TblAutAuditRepository().AddTask(autAudit);//审核表 
            await new SchoolTimeService(toTermId).AddTaskDatSchoolTime(datSchoolTimeList, toTermId);//班级上课时间表
            await new TblAutClassRepository().AddTask(autClassList);//审核中数据-班级课表
            await new TblAutClassTimeRepository().AddTask(autClassTimeList); //审核中数据-班级上课时间表
        }
        #endregion

        #region CourseIsUseByRoomCourseId 审核中的班级是否已被使用
        /// <summary>
        /// 审核中的班级是否已被使用
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="roomCourseId">教室课程Id</param>
        /// <returns>true:已使用 false:未使用</returns>
        public static bool CourseIsUseByRoomCourseId(long roomCourseId)
        {
            List<long> auditIds = new TblAutAuditRepository().GetByWhere();
            return new TblAutClassRepository().RoomCourseIdIsUse(auditIds, roomCourseId);
        }
        #endregion

        /// <summary>
        /// 描述：审核终审通过
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2018-11-2</para>
        /// </summary>
        public override void ProcessAuditSuccess()
        {
            try
            {


                //3、将审核通过数据添加到已生效数据表
                var autClassQuery = _tblAutClassRepository.Value.LoadList(x => x.AuditId == TblAutAudit.AuditId);   //班级课表
                var classTimQuery = _tblAutClassTimeRepository.Value.LoadList(x => x.AuditId == TblAutAudit.AuditId);  //班级上课时间表

                //3.1先删除原来学期的课表数据
                TermCourseTimetableService termTimService = new TermCourseTimetableService(this._termId);
                termTimService.Remove();

                var timeClassList = classTimQuery.Select(x => new TblTimClassTime
                {
                    ClassTimeId = x.ClassTimeId,
                    ClassId = x.ClassId,
                    SchoolTimeId = x.SchoolTimeId,
                    CreateTime = DateTime.Now,
                    SchoolId = x.SchoolId
                }).ToList();
                LogWriter.Write(this, "班级时间段表" + timeClassList.Count);
                if (timeClassList.Any())
                {
                    ClassCourseTimetableService.AddTimClassTime(timeClassList); //添加班级上课时间表数据
                }

                var classList = autClassQuery.Select(x => new TblDatClass
                {
                    ClassId = x.ClassId,
                    ClassNo = x.ClassNo,
                    ClassRoomId = x.ClassRoomId,
                    TermId = x.TermId,
                    RoomCourseId = x.RoomCourseId,
                    CourseId = x.CourseId,
                    CourseLeveId = x.CourseLeveId,
                    TeacherId = x.TeacherId,
                    CourseNum = x.CourseNum,
                    StudentsNum = x.StudentsNum,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    SchoolId = x.SchoolId,
                    ClassType = (int)ClassType.Default
                }).ToList();
                LogWriter.Write(this, "班级表" + classList.Count);
                if (classList.Any())
                {
                    ClassCourseTimetableService.AddDatClass(classList);  //添加班级表数据
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 描述：退回提交人（没有特殊业务要求，暂不实现该方法）
        /// <para>作者：瞿琦</para>
        /// <para>创建时间:2019-11-2</para>
        /// </summary>
        public override void ProcessAuditReturn(AuditCallbackRequest dto)
        {
        }

        /// <summary>
        /// 描述：提交审核
        /// <para>作者：Huang GaoLiang</para>
        /// <para>创建时间：2018-11-2</para>
        /// </summary>
        /// <param name="auditorId">审核人编号</param>
        /// <param name="auditName">审核人名称</param>
        /// <param name="applyTitle">提交给流程平台的显示信息</param>]
        /// <param name="createUserId">操作人编号</param>
        /// <param name="createUserName">操作人名称</param>
        public void SubmitAudit(string auditorId, string auditName, string applyTitle, string createUserId, string createUserName)
        {
            // 1.向流程平台提交审核流程,并得到流程记录Id 
            string businessType = ((int)AuditBusinessType.TermCourseTimetable).ToString();

            var flowModel = new FlowInputDto
            {
                SystemCode = BusinessConfig.BussinessCode,
                BusinessCode = businessType,
                ApplyId = CurrentUserId,
                ApplyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                ApplyCode = this.TblAutAudit.AuditId.ToString(),    //申请单号，没有置空
                ApplyTitle = applyTitle,
                AuditUserId = auditorId,
                FlowConent = string.Empty,
                Describe = string.Empty,
                Number = 0,
                FlowID = this.TblAutAudit != null && this.TblAutAudit.AuditStatus == (int)AuditStatus.Return ? this.TblAutAudit.FlowNo : string.Empty
            };

            // 审核状态
            var auditStatus = TblAutAudit?.AuditStatus ?? 0;  //等于this.TblAutAudit != null ? this.TblAutAudit.AuditStatus : 0

            string flowId = base.SubmitAuditFlow(flowModel, (AuditStatus)auditStatus);

            this.TblAutAudit.FlowNo = flowId;
            this.TblAutAudit.AuditUserId = auditorId;
            this.TblAutAudit.AuditUserName = auditName;
            this.TblAutAudit.AuditStatus = (int)AuditStatus.Auditing;
            this.TblAutAudit.UpdateTime = DateTime.Now;
            this.TblAutAudit.CreateUserId = createUserId;
            this.TblAutAudit.CreateUserName = createUserName;

            //2.提交审核
            _tblAutAuditRepository.Update(this.TblAutAudit);
        }

        /// <summary>
        /// 根据上课时间段编号查询数据
        /// <para>作者：Huang GaoLiang </para>
        /// <para>创建时间：2018-11-2 </para>
        /// </summary>
        /// <param name="schoolTimeId">上课时间段编号</param>
        /// <returns>返回上课时间段集合</returns>
        public List<TblAutClassTime> GetAutClassTimeListById(long schoolTimeId)
        {
            return _tblAutClassTimeRepository.Value.LoadList(m => m.SchoolTimeId == schoolTimeId);
        }
    }
}
