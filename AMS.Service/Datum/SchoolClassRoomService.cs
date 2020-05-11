using System;
using System.Collections.Generic;
using System.Linq;
using AMS.Core;
using AMS.Dto;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using AutoMapper;

namespace AMS.Service
{
    /// <summary>
    /// 校区的教室
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-02-20</para>
    /// </summary>
    public class SchoolClassRoomService : BService
    {
        private readonly Lazy<TblDatClassRoomRepository> _tblDatClassRoomRepository = new Lazy<TblDatClassRoomRepository>();
        private readonly Lazy<ViewRoomCourseRepository> _viewRoomCourseRepository = new Lazy<ViewRoomCourseRepository>();
        private readonly string _schoolId;  //校区

        /// <summary>
        /// 根据校区Id创建一个教室服务
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="schoolId">校区</param>
        public SchoolClassRoomService(string schoolId)
        {
            this._schoolId = schoolId;
        }

        #region ClassRoomList 教室
        private List<TblDatClassRoom> _classRoomList;
        /// <summary>
        /// 教室信息列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        public List<TblDatClassRoom> ClassRoomList => _classRoomList ?? (_classRoomList = this.GetClassRoomList());
        /// <summary>
        /// 教室信息列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <returns>教室信息列表</returns>
        private List<TblDatClassRoom> GetClassRoomList()
        {
            return _tblDatClassRoomRepository.Value.GetClassRoomByWhere(_schoolId).Result;
        }
        #endregion

        #region RoomCourseList 学期的所有教室与课程
        private List<RoomCourseResponse> _rooms;
        /// <summary>
        /// 学期的所有教室与课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        public List<RoomCourseResponse> RoomCourseList => _rooms ?? (_rooms = this.GetRoomCourseList());

        /// <summary>
        /// 学期的所有教室与课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-11</para>
        /// </summary>
        /// <returns>教室与课程列表</returns>
        private List<RoomCourseResponse> GetRoomCourseList()
        {
            var res = Mapper.Map<List<RoomCourseResponse>>(_viewRoomCourseRepository.Value.Get(_schoolId))
                        .OrderBy(x => x.IsDisabled)
                        .ThenBy(x => x.RoomNo, new NaturalStringComparer())
                        .ThenBy(x => x.CourseId)
                        .ToList();

            return res;
        }
        #endregion

        /// <summary>
        /// 获取班级课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-20</para>
        /// </summary>
        /// <param name="classRoomId">教室Id</param>
        /// <returns>校区课程列表</returns>
        public List<SchoolCourseDto> GetRoomCourses(long classRoomId)
        {
            List<SchoolCourseDto> res = new List<SchoolCourseDto>();

            //班级课程
            var roomCourse = _viewRoomCourseRepository.Value
                .Get(_schoolId)
                .Where(x => x.ClassRoomId == classRoomId)
                .ToList();

            if (!roomCourse.Any())
            {
                return res;
            }

            var courseIds = roomCourse.Select(x => x.CourseId).Distinct().ToList();

            //课程基本信息
            var courses = new TblDatCourseRepository().GetCoursesAsync(courseIds).Result;

            //课程等级
            var courseLv = new ViewCourseLevelMiddleRepository().Get(courseIds).Result;

            foreach (var room in roomCourse)
            {
                var course = courses.FirstOrDefault(x => x.CourseId == room.CourseId);
                var sCourse = new SchoolCourseDto
                {
                    ClassCnName = course.ClassCnName,
                    CourseId = course.CourseId,
                    ClassEnName = course.ClassEnName,
                    CourseCnName = course.CourseCnName,
                    CourseCode = course.CourseCode,
                    CourseEnName = course.CourseEnName,
                    CourseType = course.CourseType,
                    IsDisabled = course.IsDisabled,
                    ShortName = course.ShortName,
                    CourseLevel = courseLv.Where(c => course.CourseId == c.CourseId).Select(c => new CourseLevelDto
                    {
                        CourseLevelId = c.CourseLevelId,
                        IsDisabled = c.IsDisabled,
                        LevelCnName = c.LevelCnName,
                        LevelCode = c.LevelCode,
                        LevelEnName = c.LevelEnName
                    }).ToList()
                };
                res.Add(sCourse);
            }

            return res;
        }

        /// <summary>
        /// 获取教室
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-23</para>
        /// </summary>
        /// <param name="classRoomId">教室Id</param>
        /// <returns>教室信息</returns>
        public TblDatClassRoom GetClassRoom(long classRoomId)
        {
            return _tblDatClassRoomRepository.Value.Load(classRoomId);
        }

        /// <summary>
        /// 获取所有启用教室
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-30</para>
        /// </summary>
        /// <returns>启用的教室列表</returns>
        public List<ClassRoomResponse> GetAllEnableClassRoom()
        {
            List<ClassRoomResponse> res = this.RoomCourseList.Where(x => !x.IsDisabled).Select(x => new ClassRoomResponse
            {
                ClassRoomId = x.ClassRoomId,
                RoomNo = x.RoomNo
            })
            .DistinctBys(d => d.ClassRoomId)
            .OrderBy(x => x.RoomNo, new NaturalStringComparer())
            .ToList();

            return res;
        }
    }
}
