using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Core;
using AMS.Dto;
using AMS.Storage.Models;
using AMS.Storage.Repository;

namespace AMS.Service
{
    /// <summary>
    /// 老师课程表
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2018-11-06</para>
    /// </summary>
    public class TeacherCourseSchedule
    {
        private readonly Lazy<TblDatClassRepository> _classRepository = new Lazy<TblDatClassRepository>();
        private readonly Lazy<TblTimClassTimeRepository> _classTimeRepository = new Lazy<TblTimClassTimeRepository>();

        private readonly long _termId; //学期Id
        private readonly string _teacherId; //老师Id

        /// <summary>
        /// 老师课程表服务
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="termId">学期Id</param>
        /// <param name="teacherId">老师Id</param>
        public TeacherCourseSchedule(long termId, string teacherId)
        {
            this._termId = termId;
            this._teacherId = teacherId;
        }

        /// <summary>
        /// 获取教室查看课表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <returns>老师个人课表数据</returns>
        public async Task<List<TeacherCourseTimetableResponse>> GetCourseTimetable(string companyId)
        {
            List<TeacherCourseTimetableResponse> res = new List<TeacherCourseTimetableResponse>();

            //1.获取老师上课班级
            List<TblDatClass> classes = _classRepository.Value.GetClassByTermIdAsync(_termId)
                .Result
                .Where(x => x.TeacherId == _teacherId)
                .ToList();

            //2.获取班级上课时间
            List<TblTimClassTime> classTimes = await _classTimeRepository.Value.GetByClassId(classes.Select(x => x.ClassId));

            //3.获取基础数据
            //3.1 课程
            List<TblDatCourse> courses = CourseService.GetAllAsync().Result;

            //3.2 课程等级
            List<CourseLevelResponse> courseLevels = new CourseLevelService(companyId).GetList().Result;

            //3.3 获取教室
            TermCourseTimetableService termCourseService = new TermCourseTimetableService(_termId);
            List<TblDatClassRoom> classRooms = new SchoolClassRoomService(termCourseService.TblDatTerm.SchoolId).ClassRoomList;

            //3.4.获取上课时间段基础数据
            List<TblDatSchoolTime> schoolTimes = new SchoolTimeService(_termId).TblDatSchoolTime;

            int maxLength = 0;
            //4.整合数据
            for (int i = 1; i <= 7; i++)
            {
                TeacherCourseTimetableResponse classRoom = new TeacherCourseTimetableResponse
                {
                    Week = WeekDayConvert.IntToString(i),
                    ClassTimes = new List<TeacherClassTime>()
                };

                var cts = (from a in classTimes                                                 //教室下的班级上课时间段
                           join b in classes on a.ClassId equals b.ClassId                      //教室下的班级
                           join c in schoolTimes on a.SchoolTimeId equals c.SchoolTimeId        //基础数据 学期下所有上课时间段
                           join d in courseLevels on b.CourseLeveId equals d.CourseLevelId      //基础数据 课程等级
                           join e in courses on b.CourseId equals e.CourseId                    //基础数据 课程
                           join f in classRooms on b.ClassRoomId equals f.ClassRoomId           //基础数据 教室
                           where c.WeekDay == i
                           select new TeacherClassTime
                           {
                               BeginTime = c.BeginTime,
                               EndTime = c.EndTime,
                               CourseName = e.ShortName,
                               LevelCnName = d.LevelCnName,
                               RoomNo = f.RoomNo
                           }).OrderBy(x => x.BeginTime);

                classRoom.ClassTimes.AddRange(cts);

                if (cts.Count() > maxLength)
                {
                    maxLength = cts.Count();
                }

                res.Add(classRoom);
            }

            //补齐
            foreach (var item in res)
            {
                if (item.ClassTimes.Count >= maxLength)
                {
                    continue;
                }

                while (item.ClassTimes.Count < maxLength)
                {
                    item.ClassTimes.Add(new TeacherClassTime());
                }
            }

            return res;
        }
    }
}
