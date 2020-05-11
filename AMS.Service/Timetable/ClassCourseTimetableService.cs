using System;
using System.Collections.Generic;
using System.Linq;
using AMS.Core;
using AMS.Dto;
using AMS.Storage.Models;
using AMS.Storage.Repository;

namespace AMS.Service
{
    /// <summary>
    /// 班级课表服务
    /// </summary>
    public class ClassCourseTimetableService : BService
    {
        #region 属性/服务/仓储/构造函数
        /// <summary>
        /// 班级仓储
        /// </summary>
        private readonly Lazy<TblDatClassRepository> _classRepository = new Lazy<TblDatClassRepository>();
        /// <summary>
        /// 班级上课时间段
        /// </summary>
        private readonly Lazy<TblTimClassTimeRepository> _classTimeRepository = new Lazy<TblTimClassTimeRepository>();
        /// <summary>
        /// 班级Id
        /// </summary>
        private long _classId;
        /// <summary>
        /// 实例化一个班级的课表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="classId">班级ID</param>
        public ClassCourseTimetableService(long classId)
        {
            this._classId = classId;
        }
        #endregion

        #region GetClass 班级信息
        /// <summary>
        /// 班级信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <returns>班级信息</returns>
        internal TblDatClass GetClass()
        {
            return _classRepository.Value.Load(_classId);
        }
        #endregion

        #region GetClassTimetable 获取已安排课表的班级课表信息

        /// <summary>
        /// 班级课表详情
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <param name="termId">学期Id</param>
        /// <returns>课程表班级的详情信息</returns>
        public static ClassTimetableResponse GetClassTimetable(long classId, long termId)
        {
            TermCourseTimetableAuditService termCourseTimetableAuditService = new TermCourseTimetableAuditService(termId);

            if (termCourseTimetableAuditService.NoPass)
            {
                return termCourseTimetableAuditService.GetNoPassClassTimetable(classId).Result;
            }

            return new ClassCourseTimetableService(classId).GetClassTimetable();
        }

        /// <summary>
        /// 获取已安排课表的班级课表信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <remarks>课程表班级的详情信息</remarks>
        /// <exception cref="BussinessException">
        /// 异常ID：1，异常描述：班级信息为空
        /// </exception>
        private ClassTimetableResponse GetClassTimetable()
        {
            //获取班级
            var classInfo = this.GetClass();

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

            SchoolTimeService schoolTimeService = new SchoolTimeService(classInfo.TermId);
            //获取学期信息
            TblDatTerm term = schoolTimeService.TblDatTerm;
            //这个学期所有的上课时间
            var allTermClassTime = schoolTimeService.TblDatSchoolTime.OrderBy(x => x.BeginTime).ToList();
            //校区老师
            res.Teacher = TeachService.GetIncumbentTeachers(term.SchoolId, classInfo.TeacherId);

            //课程信息与课程等级
            res.ClassTimetableCourse = GetClassTimetableCourseResponse(term.SchoolId, classInfo.ClassRoomId, classInfo.CourseId);

            //当前教室门牌号
            res.RoomNo = new SchoolClassRoomService(term.SchoolId).GetClassRoom(classInfo.ClassRoomId)?.RoomNo;

            var timClassTime = _classTimeRepository.Value.GetByClassId(classInfo.ClassId);

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

        #region GetClassTimetable 获取未安排课表的班级课表信息
        /// <summary>
        /// 获取未安排课表的班级课表信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="classRoomId">教室ID</param>
        /// <param name="schoolTimeId">时间段ID</param>
        /// <returns>课次表详情</returns>
        public static ClassTimetableResponse GetNoClassTimetable(long classRoomId, long schoolTimeId)
        {
            ClassTimetableResponse res = new ClassTimetableResponse
            {
                IsClassJoin = false
            };

            SchoolTimeService service = SchoolTimeService.CreateSchoolTimeService(schoolTimeId);
            List<TblDatSchoolTime> allSchoolTimes = service.TblDatSchoolTime;
            TblDatTerm term = service.TblDatTerm;

            //获取当前时间段
            var firstTime = allSchoolTimes.FirstOrDefault(x => x.SchoolTimeId == schoolTimeId);

            //第一节课时间段
            res.Time1 = new List<string> { firstTime.BeginTime, firstTime.EndTime };

            //上课老师
            res.Teacher = TeachService.GetIncumbentTeachers(term.SchoolId);

            //当前教室门牌号
            res.RoomNo = new SchoolClassRoomService(term.SchoolId).GetClassRoom(classRoomId)?.RoomNo;

            //课程与课程等级
            res.ClassTimetableCourse = GetClassTimetableCourseResponse(term.SchoolId, classRoomId);

            ClassTimetableSchoolTimeResult classTimetableSchoolTimeResult = GetClassTimetableSchoolTimeResult(allSchoolTimes, classRoomId, schoolTimeId, term.TermId);

            //星期几与上课时间的数据
            res.WeekDaySchoolTimes = classTimetableSchoolTimeResult
                .WeekDaySchoolTimes
                .OrderBy(m => m.WeekDay).ToList();

            res.CanClassJoin = classTimetableSchoolTimeResult.CanClassJoin;

            //第二节课时间段
            res.Time2 = classTimetableSchoolTimeResult.Time2;

            return res;
        }

        /// <summary>
        /// 课程表基础数据
        /// </summary>
        private class ClassTimetableSchoolTimeResult
        {
            public List<string> Time2 { get; set; }

            public List<ClassTimetableSchoolTimeResponse> WeekDaySchoolTimes { get; set; }

            public bool CanClassJoin { get; set; }
        }

        /// <summary>
        /// 校验时间段是否可以连上
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="allClassTime">所有上课时间段</param>
        /// <param name="classRoomId">教室Id</param>
        /// <param name="schoolTimeId">上课时间段</param>
        /// <param name="termId">学期Id</param>
        /// <returns>课程表基础数据</returns>
        private static ClassTimetableSchoolTimeResult GetClassTimetableSchoolTimeResult(
            List<TblDatSchoolTime> allClassTime, long classRoomId, long schoolTimeId, long termId)
        {
            ClassTimetableSchoolTimeResult res = new ClassTimetableSchoolTimeResult()
            {
                Time2 = new List<string>(),
                WeekDaySchoolTimes = new List<ClassTimetableSchoolTimeResponse>(),
                CanClassJoin = false
            };

            TermCourseTimetableAuditService termCourseTimetableAuditService = new TermCourseTimetableAuditService(termId);

            List<long> schoolTimeIds = allClassTime.Select(x => x.SchoolTimeId).Distinct().ToList();

            List<long> alreadySchoolTimeIds = null;//已上课时间段

            if (termCourseTimetableAuditService.NoPass)
            {
                alreadySchoolTimeIds = termCourseTimetableAuditService.GetAlreadyClassTime(classRoomId);
            }
            else
            {
                alreadySchoolTimeIds = GetAlreadyClassTime(termId, classRoomId);
            }

            //1.第一节上课时间
            TblDatSchoolTime firstClassTime = allClassTime.FirstOrDefault(x => x.SchoolTimeId == schoolTimeId);
            //2.第二节上课时间
            TblDatSchoolTime secondClassTime = null;

            //当天所有上课时间段
            var dayClassTimes = allClassTime
                .Where(x => x.WeekDay == firstClassTime.WeekDay && x.Duration == firstClassTime.Duration)
                .OrderBy(x => x.BeginTime)
                .ToList();

            //第一个时间段索引
            int firstTimeIndex = dayClassTimes.IndexOf(firstClassTime);
            //第二个时间段索引
            int secondClassTimeIndex = firstTimeIndex + 1;

            //班级第二个时间段
            if (dayClassTimes.Count > secondClassTimeIndex)
            {
                secondClassTime = dayClassTimes[secondClassTimeIndex];
                if (!alreadySchoolTimeIds.Any(x => x == secondClassTime.SchoolTimeId))
                {
                    res.CanClassJoin = true;
                    res.Time2.Add(secondClassTime.BeginTime);
                    res.Time2.Add(secondClassTime.EndTime);
                }
            }

            //一周的上课时间
            for (int i = 1; i <= 7; i++)
            {
                ClassTimetableSchoolTimeResponse ctst = new ClassTimetableSchoolTimeResponse
                {
                    HasSchoolTime1 = false,
                    SchoolTimeId1 = 0,
                    HasSchoolTime2 = false,
                    SchoolTimeId2 = 0,
                    IsChecked = false,
                    WeekDay = i
                };

                //当天时间段
                var currentDayTime = allClassTime
                    .Where(x => x.WeekDay == i && x.Duration == firstClassTime.Duration)
                    .OrderBy(x => x.BeginTime)
                    .ToList();

                //第一节
                TblDatSchoolTime first = currentDayTime.FirstOrDefault(x =>
                                            x.Duration == firstClassTime.Duration &&
                                            x.BeginTime == firstClassTime.BeginTime &&
                                            x.EndTime == firstClassTime.EndTime);

                //该天没有此时间段
                if (first == null)
                {
                    res.WeekDaySchoolTimes.Add(ctst);
                    continue;
                }

                //第一节课基本数据
                ctst.HasSchoolTime1 = true;
                ctst.SchoolTimeId1 = first.SchoolTimeId;
                if (first.SchoolTimeId == firstClassTime.SchoolTimeId)
                {
                    ctst.IsChecked = true;
                }

                //第一节课时间段段是否占用
                if (alreadySchoolTimeIds.Any(x => x == first.SchoolTimeId))
                {
                    ctst.HasSchoolTime1 = false;
                    ctst.SchoolTimeId1 = 0;
                    res.WeekDaySchoolTimes.Add(ctst);
                    continue;
                }

                //第二节 是否能够与下一个时间段连上
                if (secondClassTime != null)
                {
                    TblDatSchoolTime second = currentDayTime.FirstOrDefault(x =>
                                x.Duration == secondClassTime.Duration &&
                                x.BeginTime == secondClassTime.BeginTime &&
                                x.EndTime == secondClassTime.EndTime);

                    if (second != null)
                    {
                        if (!alreadySchoolTimeIds.Any(x => x == second.SchoolTimeId))
                        {
                            ctst.HasSchoolTime2 = true;
                            ctst.SchoolTimeId2 = second.SchoolTimeId;
                        }
                    }

                }
                res.WeekDaySchoolTimes.Add(ctst);
            }

            return res;
        }

        /// <summary>
        /// 获取已排课时间段
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="termId">学期id</param>
        /// <param name="classRoomId">教室Id</param>
        /// <returns>上课时间段Id列表</returns>
        private static List<long> GetAlreadyClassTime(long termId, long classRoomId)
        {
            TblDatClassRepository classRepository = new TblDatClassRepository();
            TblTimClassTimeRepository classTimeRepository = new TblTimClassTimeRepository();

            List<TblDatClass> classs = classRepository.GetTermIdByClass(termId)
                .Where(x => x.ClassRoomId == classRoomId)
                .ToList();

            List<long> classIds = classs.Select(x => x.ClassId).ToList();

            List<TblTimClassTime> classTimes = classTimeRepository.GetByClassId(classIds)
                .Result
                .Where(x => classIds.Contains(x.ClassId))
                .ToList();

            return classTimes.Select(x => x.SchoolTimeId).Distinct().ToList();
        }
        #endregion

        #region GetClassTimetableCourseResponse 获取课程
        /// <summary>
        /// 获取课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <param name="classRoomId">教室Id</param>
        /// <param name="disableCourse">忽略课程已禁用的课程Id</param>
        /// <returns>课程列表</returns>
        internal static List<ClassTimetableCourseResponse> GetClassTimetableCourseResponse(string schoolId, long classRoomId, long? courseDisableId = null)
        {
            SchoolClassRoomService schoolClassRoomService = new SchoolClassRoomService(schoolId);
            //课程与课程等级
            var roomCourses = schoolClassRoomService.GetRoomCourses(classRoomId);

            List<long> disableCourseIds = schoolClassRoomService
                .RoomCourseList
                .Where(x => x.IsDisabled && x.ClassRoomId == classRoomId)
                .Select(x => x.CourseId)
                .ToList();

            if (courseDisableId.HasValue)
            {
                disableCourseIds = disableCourseIds.Where(x => x != courseDisableId.Value).ToList();
            }

            var res = roomCourses.Where(x => !disableCourseIds.Contains(x.CourseId)).Select(x => new ClassTimetableCourseResponse
            {
                CourseCnName = x.ClassCnName,
                CourseCode = x.CourseCode,
                CourseId = x.CourseId,
                ShortName = x.ShortName,
                Levels = x.CourseLevel.Select(m => new ClassTimetableCourseLevelResponse
                {
                    CourseLevelId = m.CourseLevelId,
                    LevelCnName = m.LevelCnName,
                    LevelCode = m.LevelCode
                }).ToList()
            }).ToList();

            return res;
        }
        #endregion

        #region  添加课表信息
        /// <summary>
        /// 描述：添加班级信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-11</para>
        /// </summary>
        /// <param name="dto">要添加的班级信息</param>
        internal static void AddDatClass(List<TblDatClass> dto)
        {
            new TblDatClassRepository().SaveTask(dto).Wait();
        }

        /// <summary>
        /// 描述：添加上课时间段信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-11</para>
        /// </summary>
        /// <param name="dto"></param>
        internal static void AddTimClassTime(List<TblTimClassTime> dto)
        {
            new TblTimClassTimeRepository().Add(dto);
        }
        #endregion
    }
}
