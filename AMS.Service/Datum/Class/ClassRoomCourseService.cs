using System;
using System.Threading.Tasks;
using AMS.Core;
using AMS.Dto;
using AMS.Storage.Models;
using AMS.Storage.Repository;

namespace AMS.Service
{
    /// <summary>
    /// 表示某个教室安排的一个课程的服务
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-02-20</para>
    /// </summary>
    public class ClassRoomCourseService : BService
    {
        #region 仓储/服务/属性/构造函数
        /// <summary>
        /// 教室仓储
        /// </summary>
        private readonly Lazy<TblDatClassRoomRepository> _tblDatClassRoomRepository = new Lazy<TblDatClassRoomRepository>();
        /// <summary>
        /// 教室下的课程仓储
        /// </summary>
        private readonly Lazy<TblDatRoomCourseRepository> _tblDatRoomCourseRepository = new Lazy<TblDatRoomCourseRepository>();
        /// <summary>
        /// 教室课程Id
        /// </summary>
        private readonly long _roomCourseId;
        /// <summary>
        /// 教室课程服务
        /// </summary>
        /// <param name="roomCourseId">教室课程Id</param>
        public ClassRoomCourseService(long roomCourseId)
        {
            _roomCourseId = roomCourseId;
        }
        #endregion

        #region AddAsync 添加教室分配及学位设置
        /// <summary>
        /// 添加班级课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-10</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <param name="request">教室分配及学位设置数据源</param>
        /// <returns></returns>
        public static async Task AddAsync(string schoolId, ClassRoomRequest request)
        {
            //1、添加教室
            long classRoomId = await ClassRoomService.AddClassRoomAsync(schoolId, request.RoomNo);

            //2、准备数据
            TblDatRoomCourse roomCourse = new TblDatRoomCourse
            {
                RoomCourseId = IdGenerator.NextId(),
                ClassRoomId = classRoomId,
                CourseId = request.CourseId,
                CreateTime = DateTime.Now,
                IsDisabled = false,
                MaxStageStudents = request.MaxStageStudents,
                MaxWeekStage = request.MaxWeekStage,
                UpdateTime = DateTime.Now
            };

            //3.添加课程
            await AddClassCourseAsync(roomCourse);
        }

        /// <summary>
        /// 添加课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-10</para>
        /// </summary>
        /// <param name="course">课程对象</param>
        /// <returns></returns>
        private static async Task AddClassCourseAsync(TblDatRoomCourse course)
        {
            TblDatRoomCourseRepository roomCourseRepository = new TblDatRoomCourseRepository();
            //1、门牌号+班级名称 不允许重复。
            var exist = await roomCourseRepository.VerificationCourseAsync(course.ClassRoomId, course.CourseId);
            if (exist)
            {
                throw new BussinessException((byte)ModelType.Datum, 1);
            }

            //2、写入数据库
            await roomCourseRepository.AddTask(course);
        }
        #endregion

        #region Modify 修改教室分配及学位设置

        /// <summary>
        /// 修改班级课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-10</para>
        /// </summary>
        /// <param name="request">修改的教室分配及学位设置数据</param>
        /// <exception cref="BussinessException">
        /// 异常ID：1,异常描述：课程信息为空；教室信息为空
        /// </exception>
        public async Task Modify(ClassRoomRequest request)
        {
            //校验是否可以修改
            var course = _tblDatRoomCourseRepository.Value.Load(_roomCourseId);
            if (course == null)
            {
                throw new BussinessException((byte)ModelType.Default, 1);
            }

            var classRoom = _tblDatClassRoomRepository.Value.Load(course.ClassRoomId);

            if (classRoom == null)
            {
                throw new BussinessException((byte)ModelType.Default, 1);
            }

            //3、准备数据
            course.MaxStageStudents = request.MaxStageStudents;
            course.MaxWeekStage = request.MaxWeekStage;
            course.UpdateTime = DateTime.Now;

            //5、执行数据库更新
            await _tblDatRoomCourseRepository.Value.UpdateTask(course);
        }
        #endregion

        #region SetCourseEnable/SetCourseDisable 教室课程启用/停用
        /// <summary>
        /// 设置启用
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-11</para>
        /// </summary>
        /// <exception cref="BussinessException">
        /// 异常ID：1,异常描述：课程信息为空
        /// </exception>
        public async Task SetCourseEnable()
        {
            var roomCourse = await _tblDatRoomCourseRepository.Value.LoadTask(_roomCourseId);

            if (roomCourse == null)
            {
                throw new BussinessException((byte)ModelType.Default, 1);
            }

            roomCourse.IsDisabled = false;
            roomCourse.UpdateTime = DateTime.Now;

            await _tblDatRoomCourseRepository.Value.UpdateTask(roomCourse);
        }

        /// <summary>
        /// 设置停用
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-11</para>
        /// </summary>
        /// <exception cref="BussinessException">
        /// 异常ID：1,异常描述：课程信息为空
        /// </exception>
        public async Task SetCourseDisable()
        {
            var roomCourse = await _tblDatRoomCourseRepository.Value.LoadTask(_roomCourseId);

            if (roomCourse == null)
            {
                throw new BussinessException((byte)ModelType.Default, 1);
            }

            roomCourse.IsDisabled = true;
            roomCourse.UpdateTime = DateTime.Now;

            await _tblDatRoomCourseRepository.Value.UpdateTask(roomCourse);
        }
        #endregion

        #region Remove 删除
        /// <summary>
        /// 删除
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-11</para>
        /// </summary>
        /// <exception cref="BussinessException">
        /// 异常ID：20,异常描述：课程已使用不可删除
        /// </exception>
        public async Task Remove()
        {
            //调用顺序过程：
            //1、校验是否可以删除 (校区排课总表已使用不可删除)
            var courseIsUseDat = await new TblDatClassRepository().CourseIsUseByRoomCourseId(_roomCourseId);
            if (courseIsUseDat)
            {
                throw new BussinessException((byte)ModelType.Datum, 20);
            }
            var courseIsUseAut = TermCourseTimetableAuditService.CourseIsUseByRoomCourseId(_roomCourseId);
            if (courseIsUseAut)
            {
                throw new BussinessException((byte)ModelType.Datum, 20);
            }
            //2、调用删除操作
            TblDatRoomCourse roomCourse = await _tblDatRoomCourseRepository.Value.LoadTask(_roomCourseId);

            if (roomCourse == null)
            {
                throw new BussinessException((byte)ModelType.Default, 1);
            }

            await _tblDatRoomCourseRepository.Value.DeleteTask(roomCourse);
        }
        #endregion

        #region CourseIsUse 教室是否引用课程
        /// <summary>
        /// 教室是否引用课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-11</para>
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <returns>true:已使用 false:未使用</returns>
        public static bool CourseIsUse(long courseId)
        {
            return new TblDatRoomCourseRepository().CourseIsUse(courseId).Result;
        }
        #endregion
    }
}
