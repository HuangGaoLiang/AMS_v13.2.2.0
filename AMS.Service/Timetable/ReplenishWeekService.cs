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
using AMS.Core;
using AMS.Core.Constants;
using AMS.Core.Locks;
using AMS.Dto;
using AMS.Models;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using AutoMapper;
using Jerrisoft.Platform.Public.PageExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMS.Service
{
    /// <summary>
    /// 描述：补课周业务服务
    /// <para>作    者：Huang GaoLiang</para>
    /// <para>创建时间：2019-03-12</para>
    /// </summary>
    public class ReplenishWeekService
    {
        #region 仓储/字段/构造函数

        private readonly Lazy<TblDatClassReplenishWeekRepository> _repository = new Lazy<TblDatClassReplenishWeekRepository>();//补课周补课
        private readonly Lazy<TblTimAdjustLessonRepository> _timAdjustLessonRepository = new Lazy<TblTimAdjustLessonRepository>();//调整课次业务

        private readonly string _schoolId;//校区编号
        private readonly string _teacherId;//老师编号
        private readonly string _teacherName;//老师编号

        private const string Sixty = "S";
        private const string Ninety = "M";
        private const string HundredEight = "L";
        private const string WeekClassNo = "X";


        /// <summary>
        /// 实例化一个教师在指定校区补课周服务
        ///<para>作    者：Huang GaoLiang</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <param name="teacherId">用户编号</param>
        /// <param name="userName">用户名称</param>
        public ReplenishWeekService(string schoolId, string teacherId, string userName)
        {
            this._schoolId = schoolId;
            this._teacherId = teacherId;
            this._teacherName = userName;
        }

        #endregion

        #region GetReplenishWeekList 补课周补课分页列表

        /// <summary>
        /// 补课周补课分页列表
        /// <para>作    者：Huang GaoLiang</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="request">查询补课周补课列表条件</param>
        /// <returns>返回补课周补课分页数据</returns>
        public PageResult<ReplenishWeekListResponse> GetReplenishWeekList(ReplenishWeekListSearchRequest request)
        {
            // 1、根据条件查询补课周补课列表
            PageResult<ViewReplenishWeek> replenishWeekList = new ViewReplenishWeekRepository().GetReplenishWeekList(request);
            PageResult<ReplenishWeekListResponse> list = Mapper.Map<PageResult<ViewReplenishWeek>, PageResult<ReplenishWeekListResponse>>(replenishWeekList);

            // 2、获取补课时间
            var classIds = list.Data.Select(m => m.ClassId).ToList();

            // 3、根据上课班级编号获取

            List<ViewTimAdjustLesson> timAdjustLessonList = new ViewTimAdjustLessonRepository().GetTimeLessonClassList(classIds);

            // 3、组合数据
            foreach (var item in list.Data)
            {
                item.MakeUpTimeList = timAdjustLessonList.Where(m => m.ClassId == item.ClassId && m.StudentId == item.StudentId)
                    .Select(m => new MakeUpTime
                    {
                        ClassDate = m.ClassDate,
                        ClassBeginTime = m.ClassBeginTime,
                        ClassEndTime = m.ClassEndTime
                    })
                    .OrderBy(m => m.ClassDate)
                    .ToList();
            }

            return list;
        }

        #endregion

        #region ModifyRemark 补课周补课添加备注
        /// <summary>
        /// 补课周补课添加备注
        /// <para>作    者：Huang GaoLiang</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="request">添加补课周补课备注</param>
        public void ModifyRemark(ReplenishWeekModifyRemarkRequest request)
        {
            // 先根据条件查询备注信息
            var datClassReplenishWeek = _repository.Value.GetClassReplenishWeek(this._schoolId, this._teacherId, request.StudentId, request.ClassId);
            if (datClassReplenishWeek == null)
            {
                TblDatClassReplenishWeek week = new TblDatClassReplenishWeek
                {
                    ReplenishWeekId = IdGenerator.NextId(),
                    SchoolId = this._schoolId,
                    TeacherId = this._teacherId,
                    ClassId = request.ClassId,
                    Remark = request.Remark,
                    StudentId = request.StudentId,
                    CreateTime = DateTime.Now
                };
                _repository.Value.Add(week);
            }
            else
            {
                datClassReplenishWeek.Remark = request.Remark;
                _repository.Value.Update(datClassReplenishWeek);
            }
        }
        #endregion

        #region GetClassTimeList 根据学生编号和班级编号获取安排的补课记录

        /// <summary>
        /// 根据学生编号和班级编号获取安排的补课记录
        /// <para>作    者：Huang GaoLiang</para>
        /// <para>创建时间：2019-03-12</para> 
        /// </summary>
        /// <param name="studentId">学生编号</param>
        /// <param name="classId">班级编号</param>
        /// <returns>返回安排的补课记录</returns>
        public List<ReplenishWeekClassTimeListResponse> GetClassTimeList(long studentId, long classId)
        {
            var classTimeList = GetTimAdjustLessonList(classId, studentId);
            return classTimeList;
        }

        #endregion

        #region RemoveClassTime 删除已安排的补课周课次

        /// <summary>
        /// 删除已安排的补课周课次
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-12 </para>
        /// </summary>
        /// <param name="adjustLessonIds">安排补课班级记录编号</param>
        /// <exception>
        /// 异常ID：1,未找到数据
        /// </exception>
        public void RemoveClassTime(List<long> adjustLessonIds)
        {
            var adjustLesson = _timAdjustLessonRepository.Value.GetTimAdjustLessonList(adjustLessonIds);
            if (!adjustLesson.Any())
            {
                throw new BussinessException((byte)ModelType.Default, 1);
            }

            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_AMSSCHOOLSTUDENT, this._schoolId, adjustLesson.FirstOrDefault().StudentId.ToString()))
            {
                // 获取班级信息
                TimAdjustLessonInDto inDto = new TimAdjustLessonInDto
                {
                    SchoolId = this._schoolId,
                    ToTeacherId = this._teacherId,
                    BusinessType = ProcessBusinessType.C_AdjustLessonReplenishWeek
                };
                long classId = _timAdjustLessonRepository.Value.Load(adjustLesson.FirstOrDefault().AdjustLessonId).ClassId;
                TblDatClass datClass = new ReplenishWeekClassService(classId).TblDatClass;

                // 操作日志
                TblDatOperationLog log = new TblDatOperationLog()
                {
                    OperationLogId = IdGenerator.NextId(),
                    SchoolId = this._schoolId,
                    BusinessType = (int)LogBusinessType.ReplenishWeekCancel,
                    BusinessId = adjustLesson.FirstOrDefault().AdjustLessonId,
                    FlowStatus = (int)OperationFlowStatus.Cancel,
                    OperatorId = this._teacherId,
                    OperatorName = this._teacherName,
                    Remark = JsonConvert.SerializeObject($"ID是{adjustLesson.FirstOrDefault().StudentId}学生，被删除班级ID是{adjustLesson.FirstOrDefault().ClassId}，上课时间是{adjustLesson.FirstOrDefault().ClassDate.ToString("yyyy-MM-dd")}     {adjustLesson.FirstOrDefault().ClassBeginTime}"),
                    CreateTime = DateTime.Now,
                };

                // 要判断该课次是否该老师的课次,防止别人乱传ID
                if (adjustLesson.FirstOrDefault().ToTeacherId == this._teacherId)
                {
                    adjustLesson.FirstOrDefault().Status = (int)TimAdjustLessonStatus.Invalid;

                    using (var unitOfWork = new UnitOfWork())
                    {
                        try
                        {
                            unitOfWork.BeginTransaction();

                            // 调用课次服务
                            var adjustLessonWeekRevokeFinisher = new AdjustLessonReplenishWeekFinisher(this._schoolId, adjustLesson.FirstOrDefault().StudentId, adjustLesson, datClass, log, unitOfWork);

                            LessonService lessonService = new LessonService(unitOfWork);
                            lessonService.Finish(adjustLessonWeekRevokeFinisher);

                            unitOfWork.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            unitOfWork.RollbackTransaction();
                            throw ex;
                        }
                    }
                }
            }
        }
        #endregion

        #region AddClassTime 添加补课周课次

        /// <summary>
        /// 添加补课周课次
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-12 </para>
        /// </summary>
        ///  <param name="replenishWeekClassTime">安排补课参数信息</param>
        public void AddClassTime(ReplenishWeekClassTimeAddRequest replenishWeekClassTime)
        {
            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_AMSSCHOOLSTUDENT, this._schoolId, replenishWeekClassTime.StudentId.ToString()))
            {
                // 1、根据编辑编号查询原班级信息
                TblDatClass oldDatClass = new DefaultClassService(replenishWeekClassTime.ClassId).TblDatClass;

                // 2、获取学生缺课的课次信息
                List<ViewStudentTimeLess> studentTimeLessList = GetStudentMissClassList(replenishWeekClassTime);

                // 3、数据校验
                CheckDatClass(oldDatClass, replenishWeekClassTime.WeekClassTimeList.Count, studentTimeLessList.Count);

                // 获取补课日期中该老师所有的课次
                var classDates = replenishWeekClassTime.WeekClassTimeList.Select(m => m.ClassDate).Distinct().ToList();
                var teacherClassList = new ViewTimAttendLessonRepository().GetClassDateTimAttendLessonList(this._schoolId, this._teacherId, classDates);

                this.ValidationLesson(replenishWeekClassTime.WeekClassTimeList, teacherClassList);


                // 4、获取所有补课周补课中的补课数据
                List<TblTimAdjustLesson> timAdjustLessonList = GetTimAdjustLessonList();

                // 用户补课周补课班级编号生成
                var classNo = string.Empty;
                classNo = oldDatClass.ClassNo.Substring(0, oldDatClass.ClassNo.IndexOf('-') + 1);

                // 5、根据上课教室编号，查询教室信息
                var roomIds = replenishWeekClassTime.WeekClassTimeList.Select(m => m.ClassRoomId).Distinct().ToList();
                List<TblDatClassRoom> datClassRooms = ClassRoomService.GetClassRoomListByIds(roomIds);

                List<TblTimAdjustLesson> adjustLessonList = new List<TblTimAdjustLesson>();//业务调整表数据集合
                List<TblDatClass> newClassList = new List<TblDatClass>();//补课周班级集合

                List<WeekClassTime> replenishWeekList = new List<WeekClassTime>();

                int index = 0;

                var batchNo = IdGenerator.NextId();
                foreach (var m in replenishWeekClassTime.WeekClassTimeList)
                {
                    var studentTimeLess = studentTimeLessList[index];
                    replenishWeekClassTime.EnrollOrderItemId = studentTimeLess.EnrollOrderItemId;
                    replenishWeekClassTime.CourseId = studentTimeLess.CourseId;
                    replenishWeekClassTime.CourseLevelId = studentTimeLess.CourseLevelId;
                    replenishWeekClassTime.TeacherId = studentTimeLess.TeacherId;

                    // 补课周补课班级编号生成规则
                    var newClassNo = $"{classNo}{datClassRooms.FirstOrDefault(x => x.ClassRoomId == m.ClassRoomId).RoomNo }{WeekDayConvert.DayOfWeekToInt(m.ClassDate)}{GetTimeNumNo(m.ClassBeginTime, m.ClassEndTime, oldDatClass.ClassNo)}{WeekClassNo}";

                    SetDatClass(replenishWeekClassTime, oldDatClass, timAdjustLessonList, newClassNo, newClassList, adjustLessonList, m, replenishWeekList, studentTimeLess, batchNo);

                    WeekClassTime week = new WeekClassTime
                    {
                        ClassDate = m.ClassDate,
                        ClassRoomId = m.ClassRoomId
                    };
                    replenishWeekList.Add(week);

                    index++;
                }

                using (var unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.BeginTransaction();

                        // 1、写入调整课次业务表
                        unitOfWork.GetCustomRepository<TblTimAdjustLessonRepository, TblTimAdjustLesson>().Add<TblTimAdjustLesson>(adjustLessonList);

                        // 2、创建补课周补课班级
                        unitOfWork.GetCustomRepository<TblDatClassRepository, TblDatClass>().Add(newClassList);

                        // 3、调用课次服务
                        var adjustLessonReplenishWeekCreator = new AdjustLessonReplenishWeekCreator(replenishWeekClassTime, adjustLessonList, studentTimeLessList, replenishWeekClassTime.StudentId, replenishWeekClassTime.TermId, this._schoolId, this._teacherId);

                        ReplenishLessonService replenishLessonService = new ReplenishLessonService(unitOfWork);
                        replenishLessonService.Create(adjustLessonReplenishWeekCreator);

                        unitOfWork.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        unitOfWork.RollbackTransaction();
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        ///  <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-26 </para>
        /// </summary>
        /// <param name="weekClassTimeList">补课周补课信息</param>
        /// <param name="classDateTimAttendLessonList">老师课表信息</param>
        /// <exception cref="BussinessException">
        /// 异常Id:45,异常描述：教师上课时间冲突
        /// </exception>
        private void ValidationLesson(List<WeekClassTime> weekClassTimeList, List<ViewTimAttendLesson> classDateTimAttendLessonList)
        {
            foreach (var teacherLesson in classDateTimAttendLessonList)  //老师在当前时间段的课次信息
            {

                foreach (var itemLesson in weekClassTimeList)     //要创建的课次
                {
                    //当前老师所属课次时间
                    var teacherBeginDate = DateTime.Parse($"{teacherLesson.ClassDate:yyyy-MM-dd} {teacherLesson.ClassBeginTime}");
                    var teacherEndDate = DateTime.Parse($"{teacherLesson.ClassDate:yyyy-MM-dd} {teacherLesson.ClassEndTime}");

                    //要补课课次的上课时间
                    var createBeginDate = DateTime.Parse($"{teacherLesson.ClassDate:yyyy-MM-dd} {itemLesson.ClassBeginTime}");
                    var createEndDate = DateTime.Parse($"{teacherLesson.ClassDate:yyyy-MM-dd} {itemLesson.ClassEndTime}");

                    if (teacherLesson.ClassDate == itemLesson.ClassDate && ((createBeginDate >= teacherBeginDate && createBeginDate <= teacherEndDate) || (createEndDate >= teacherBeginDate && createEndDate <= teacherEndDate)))
                    {
                        throw new BussinessException((byte)ModelType.Timetable, 10);
                    }
                }
            }
        }

        /// <summary>
        /// 获取所有补课周补课中的补课数据
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-12 </para>
        /// </summary>
        /// <returns>返回补课周业务调整表中的数据</returns>
        private List<TblTimAdjustLesson> GetTimAdjustLessonList()
        {
            TimAdjustLessonInDto inDto = new TimAdjustLessonInDto
            {
                SchoolId = this._schoolId,
                ToTeacherId = this._teacherId,
                BusinessType = ProcessBusinessType.C_AdjustLessonReplenishWeek,
                Status = TimAdjustLessonStatus.Normal
            };
            List<TblTimAdjustLesson> timAdjustLessonList = new TblTimAdjustLessonRepository().GetTimAdjustLessonList(inDto);
            return timAdjustLessonList;
        }

        /// <summary>
        /// 获取学生缺课的课次信息
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-12 </para>
        /// </summary>
        /// <param name="replenishWeekClassTime">学生缺课查询条件</param>
        /// <returns>返回学生缺课信息</returns>
        private List<ViewStudentTimeLess> GetStudentMissClassList(ReplenishWeekClassTimeAddRequest replenishWeekClassTime)
        {
            StudentLessonInDto search = new StudentLessonInDto
            {

                StudentId = replenishWeekClassTime.StudentId,
                ClassId = replenishWeekClassTime.ClassId,
                SchoolId = this._schoolId,
                TeacherId = this._teacherId,
                TermId = replenishWeekClassTime.TermId,
                CourseId = replenishWeekClassTime.CourseId
            };
            List<ViewStudentTimeLess> studentTimeLessList = new ViewStudentTimeLessRepository().GetStudentTimeLessList(search);
            return studentTimeLessList;
        }

        /// <summary>
        /// 生成补课周补课班级信息
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-12 </para>
        /// </summary>
        /// <param name="replenishWeekClassTime"></param>
        /// <param name="oldDatClass">常规课班级信息</param>
        /// <param name="timAdjustLessonList">补课周补课集合</param>
        /// <param name="classNo">补课周班级生成规则</param>
        /// <param name="newClassList">新班级集合</param>
        /// <param name="adjustLessonList">业务调整数据集合</param>
        /// <param name="m">补课周补课信息</param>
        /// <param name="replenishWeekList">即将生成补课周的信息</param>
        /// <param name="studentTimeLessList">学生考勤数据</param>
        /// <param name="batchNo">批次号</param>
        private void SetDatClass(ReplenishWeekClassTimeAddRequest replenishWeekClassTime, TblDatClass oldDatClass, List<TblTimAdjustLesson> timAdjustLessonList, string classNo, List<TblDatClass> newClassList, List<TblTimAdjustLesson> adjustLessonList, WeekClassTime m, List<WeekClassTime> replenishWeekList, ViewStudentTimeLess studentTimeLessList, long batchNo)
        {
            // 检查该班级是否存在（上课日期/上课时间/下课时间/教室）
            var adjustLesson = timAdjustLessonList.FirstOrDefault(j => j.ClassDate == m.ClassDate
                                && j.ClassBeginTime == m.ClassBeginTime
                                && j.ClassEndTime == m.ClassEndTime
                                && j.ClassRoomId == m.ClassRoomId);

            // 如果补课周补课班级不存在，则需要生成一个补课周补课班级
            long weekClassId = 0;

            // 如果不是连着上的，则判断补课周班级存不存在
            if (!oldDatClass.ClassNo.Contains(HundredEight) && null == adjustLesson)
            {
                weekClassId = IdGenerator.NextId();
            }
            // 如果是连着上的
            else if (oldDatClass.ClassNo.Contains(HundredEight))
            {
                adjustLesson = timAdjustLessonList.FirstOrDefault(j => j.ClassDate == m.ClassDate && j.ClassRoomId == m.ClassRoomId);
                if (adjustLesson == null && adjustLessonList.Any())
                {
                    weekClassId = adjustLessonList.FirstOrDefault().ClassId;
                }
                else
                {
                    weekClassId = IdGenerator.NextId();
                }

            }
            else
            {
                weekClassId = adjustLesson.ClassId;
                replenishWeekClassTime.NewClassId = adjustLesson.ClassId;
            }

            CreateDatClass(weekClassId, replenishWeekClassTime, oldDatClass, classNo, newClassList, m, studentTimeLessList, batchNo);

            var adjust = new TblTimAdjustLesson
            {
                AdjustLessonId = IdGenerator.NextId(),
                BatchNo = batchNo,
                BusinessType = (int)LessonBusinessType.AdjustLessonReplenishWeek,
                ClassBeginTime = m.ClassBeginTime,
                ClassDate = m.ClassDate,
                ClassEndTime = m.ClassEndTime,
                ClassId = weekClassId,
                ClassRoomId = m.ClassRoomId,
                FromLessonId = studentTimeLessList.LessonId,
                FromTeacherId = studentTimeLessList.TeacherId,
                ToTeacherId = studentTimeLessList.TeacherId,
                Remark = LessonProcessConstants.WeekRemark,
                SchoolId = this._schoolId,
                SchoolTimeId = 0,//补课周补课，无需添加上课时间段编号
                StudentId = replenishWeekClassTime.StudentId,
                Status = (int)TimAdjustLessonStatus.Normal,
                CreateTime = DateTime.Now
            };

            adjustLessonList.Add(adjust);
        }

        /// <summary>
        /// 创建补课周补课
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-12 </para>
        /// </summary>
        /// <param name="weekClassId">班级主键编号</param>
        /// <param name="replenishWeekClassTime"></param>
        /// <param name="oldDatClass">常规课班级信息</param>
        /// <param name="classNo">补课周班级生成规则</param>
        /// <param name="datClassList">补课周补课班级</param>
        /// <param name="m">补课周补课信息</param>
        /// <param name="studentTimeLessList">学生考勤数据</param>
        /// <param name="batchNo">批次号</param>
        private void CreateDatClass(long weekClassId, ReplenishWeekClassTimeAddRequest replenishWeekClassTime, TblDatClass oldDatClass, string classNo, List<TblDatClass> datClassList, WeekClassTime m, ViewStudentTimeLess studentTimeLessList, long batchNo)
        {
            replenishWeekClassTime.NewClassId = weekClassId;

            if (!datClassList.Any(c => c.ClassId == weekClassId))
            {
                TblDatClass weekClass = new TblDatClass
                {
                    ClassId = weekClassId,
                    SchoolId = this._schoolId,
                    ClassNo = classNo,
                    TermId = oldDatClass.TermId,
                    RoomCourseId = oldDatClass.RoomCourseId,
                    ClassRoomId = m.ClassRoomId,
                    CourseId = oldDatClass.CourseId,
                    CourseLeveId = oldDatClass.CourseLeveId,
                    TeacherId = oldDatClass.TeacherId,
                    CourseNum = oldDatClass.CourseNum,
                    StudentsNum = oldDatClass.StudentsNum,
                    ClassType = (int)ClassType.ReplenishWeek,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                datClassList.Add(weekClass);
            }

        }

        /// <summary>
        /// 检查补课周补课数据
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-12 </para>
        /// </summary>
        /// <param name="datClass">班级信息</param>
        /// <param name="AddlessonCount">添加补课记录数</param>
        /// <param name="lessonCount">缺勤总课次</param>
        /// <exception>
        /// 异常ID：21,未找到班级信息
        /// 异常ID：57,补课课次不能大于缺勤课次
        /// </exception>
        private static void CheckDatClass(TblDatClass datClass, int AddlessonCount, int lessonCount)
        {
            if (datClass == null)
            {
                throw new BussinessException((byte)ModelType.Datum, 21);
            }

            if (AddlessonCount > lessonCount)
            {
                throw new BussinessException((byte)ModelType.Timetable, 57);
            }
        }

        /// <summary>
        /// 获取时间编号是S/L/M
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2019-03-21 </para>
        /// </summary>
        /// <param name="start">上课时间</param>
        /// <param name="end">下课时间</param>
        /// <param name="classNo">班级编号</param>
        /// <returns>时间编号</returns>
        ///  <exception>
        /// 异常ID：62,安排补课周补课时间有误
        /// </exception>
        private static string GetTimeNumNo(string start, string end, string classNo)
        {
            string timeNo = string.Empty;

            if (classNo.Contains(HundredEight))
            {
                timeNo = HundredEight;
            }
            else
            {
                DateTime sTime = Convert.ToDateTime(start);
                DateTime endTime = Convert.ToDateTime(end);

                double time = (endTime - sTime).TotalMinutes;

                if (time == ((int)TimeType.Sixty))
                {
                    timeNo = Sixty;
                }
                else if ((endTime - sTime).TotalMinutes == (int)TimeType.Ninety)
                {
                    timeNo = Ninety;
                }
                else
                {
                    throw new BussinessException((byte)ModelType.Timetable, 64);
                }
            }
            return timeNo;
        }

        #endregion

        #region  GetTimAdjustLessonList 获取学生补课周补课信息
        /// <summary>
        /// 获取学生补课周补课信息
        /// <para>作    者：HuangGaoLiang </para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="classId">班级编号</param>
        /// <param name="studentId">学生编号</param>
        /// <returns>返回学生补课周补课安排信息</returns>
        private List<ReplenishWeekClassTimeListResponse> GetTimAdjustLessonList(long classId, long studentId)
        {
            // 1、根据条件获取学生的补课周补课信息
            List<ViewTimAdjustLesson> timAdjustLessonList = new ViewTimAdjustLessonRepository().GetTimeLessonClassList(new List<long> { classId }).Where(m => m.StudentId == studentId).ToList();

            // 2、组合数据
            var list = (from t in timAdjustLessonList

                        join r in ClassRoomService.GetClassRoomListBySchoolId(this._schoolId) on t.ClassRoomId equals r.ClassRoomId into cr
                        from room in cr.DefaultIfEmpty()

                        select new ReplenishWeekClassTimeListResponse
                        {
                            AdjustLessonId = t.AdjustLessonId,
                            Data = t.ClassDate,
                            StartTime = t.ClassBeginTime,
                            EndTime = t.ClassEndTime,
                            ClassRoomId = room == null ? 0 : room.ClassRoomId,
                            RoomNo = room == null ? "" : room.RoomNo
                        }).ToList();

            return list;
        }


        #endregion
    }
}
