using System.Collections.Generic;
using System.Linq;
using AMS.Dto;
using AMS.Storage.Models;

namespace AMS.Service
{
    /// <summary>
    /// 校区学期课程表
    /// </summary>
    public class TermCourseSchedule
    {
        /// <summary>
        /// 学期Id
        /// </summary>
        private readonly long _termId;

        /// <summary>
        /// 一个学期课程表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="termId">学期Id</param>
        public TermCourseSchedule(long termId)
        {
            this._termId = termId;
        }

        #region GetCourseTimetable 获取课程表

        /// <summary>
        /// 获取学期课程表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <returns>校区课程总表数据</returns>
        public TermTimetableResponse GetCourseTimetable()
        {
            TermCourseTimetableService termCourseTimetableService = new TermCourseTimetableService(_termId);

            var res = termCourseTimetableService.TermTimetable;

            res.CourseTimetableSixty = TermCourseTimetableResponseFilter(res.CourseTimetableSixty);
            res.CourseTimetableNinety = TermCourseTimetableResponseFilter(res.CourseTimetableNinety);

            return res;
        }

        /// <summary>
        /// 过滤学期课程表未排课的上课时间段
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="termCourse">90分钟或60分钟课程表</param>
        /// <returns>课程表明细数据</returns>
        private TermCourseTimetableResponse TermCourseTimetableResponseFilter(TermCourseTimetableResponse termCourse)
        {
            List<TblDatSchoolTime> scheduledTimes = GetScheduledTimes(termCourse.ClassRooms, _termId); //获取已排课的时间段

            termCourse.SchoolTimes = GetSchoolTimeResponse(termCourse.SchoolTimes, scheduledTimes);    //渲染上课时间段

            termCourse.ClassRooms = GetClassRoomResponse(termCourse, scheduledTimes);                  //渲染上课时间段对应的班级

            return termCourse;
        }

        /// <summary>
        /// 获取已排课的时间段
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="classRooms">教室列表数据</param>
        /// <param name="termId">学期Id</param>
        /// <returns>上课时间段列表</returns>
        private static List<TblDatSchoolTime> GetScheduledTimes(List<TimetableClassRoomResponse> classRooms, long termId)
        {
            List<TblDatSchoolTime> res = new List<TblDatSchoolTime>();

            List<TblDatSchoolTime> schoolTimes = new SchoolTimeService(termId).TblDatSchoolTime;

            foreach (var classRoom in classRooms)                   //教室
            {
                foreach (var @class in classRoom.Classes)           //班级
                {
                    if (@class.ClassId != 0)
                    {
                        if (!res.Any(x => x.SchoolTimeId == @class.SchoolTimeId))
                        {
                            res.Add(schoolTimes.FirstOrDefault(x => x.SchoolTimeId == @class.SchoolTimeId));
                        }
                    }
                }
            }

            return res.OrderBy(x => x.WeekDay).ThenBy(x => x.BeginTime).ToList();
        }

        /// <summary>
        /// 渲染上课时间段
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="scheduledTimes">已排课的上课时间</param>
        /// <param name="schoolTimes">上课时间段</param>
        /// <returns>上课时间段列表</returns>
        private static List<TimetableSchoolTimeResponse> GetSchoolTimeResponse(List<TimetableSchoolTimeResponse> schoolTimes, List<TblDatSchoolTime> scheduledTimes)
        {
            //渲染上课时间段
            List<TimetableSchoolTimeResponse> res = new List<TimetableSchoolTimeResponse>();

            foreach (var schoolTime in schoolTimes)
            {
                TimetableSchoolTimeResponse schoolTimeResponse = new TimetableSchoolTimeResponse()
                {
                    WeekDay = schoolTime.WeekDay,
                    Times = new List<SchoolTimePeriodResponse>()
                };

                foreach (var time in schoolTime.Times)
                {
                    if (scheduledTimes.Any(x => x.SchoolTimeId == time.SchoolTimeId))
                    {
                        schoolTimeResponse.Times.Add(time);
                    }
                }

                if (schoolTimeResponse.Times.Any())
                {
                    res.Add(schoolTimeResponse);
                }
            }

            return res;
        }

        /// <summary>
        /// 渲染上课时间段对应的班级
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="termCourse">学期课程信息</param>
        /// <param name="scheduledTimes">已排课列表</param>
        /// <returns>课程表教室列表</returns>
        private static List<TimetableClassRoomResponse> GetClassRoomResponse(TermCourseTimetableResponse termCourse, List<TblDatSchoolTime> scheduledTimes)
        {
            List<TimetableClassRoomResponse> classRooms = new List<TimetableClassRoomResponse>();
            foreach (var classRoom in termCourse.ClassRooms)
            {
                TimetableClassRoomResponse newClassRoom = new TimetableClassRoomResponse
                {
                    ClassRoomId = classRoom.ClassRoomId,
                    RoomNo = classRoom.RoomNo,
                    Classes = new List<TimetableClassResponse>()
                };

                foreach (var scheduledTime in scheduledTimes)
                {
                    newClassRoom.Classes.Add(classRoom.Classes.FirstOrDefault(x => x.SchoolTimeId == scheduledTime.SchoolTimeId));
                }

                classRooms.Add(newClassRoom);
            }

            return classRooms;
        }
        #endregion
    }
}
