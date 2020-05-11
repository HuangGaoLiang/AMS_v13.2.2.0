using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Core;
using AMS.Storage.Models;
using AMS.Storage.Repository;

namespace AMS.Service
{
    /// <summary>
    /// 表示一个教室服务
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-02-20</para>
    /// </summary>
    internal class ClassRoomService : BService
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
        /// 教室Id
        /// </summary>
        private readonly long _classRoomId;
        /// <summary>
        /// 根据教室ID实例化一个教室服务
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="classRoomId">教室Id</param>
        public ClassRoomService(long classRoomId)
        {
            this._classRoomId = classRoomId;
        }
        #endregion

        #region ClassRoomInfo 获取教室信息
        private TblDatClassRoom _tblDatClassRoom;
        /// <summary>
        /// 教室信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        public TblDatClassRoom ClassRoomInfo => _tblDatClassRoom ?? (_tblDatClassRoom = GetClassRoom());

        /// <summary>
        /// 获取教室信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <returns>教室信息</returns>
        private TblDatClassRoom GetClassRoom()
        {
            return _tblDatClassRoomRepository.Value.Load(this._classRoomId);
        }
        #endregion

        #region Courses 教室下的所有课程 
        private List<TblDatRoomCourse> _courses;
        /// <summary>
        /// 教室下的所有课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        public List<TblDatRoomCourse> Courses
        {
            get
            {
                if (_courses == null)
                {
                    _courses = this.GetCourses();
                }
                return _courses;
            }
        }

        /// <summary>
        /// 获取课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <returns>教室下的课程集合</returns>
        private List<TblDatRoomCourse> GetCourses()
        {
            return _tblDatRoomCourseRepository.Value.GetByClassRoomId(_classRoomId);
        }
        #endregion

        #region GetByCourseId 获取教室下的课程
        /// <summary>
        /// 根据课程Id获取教室下的课程信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <returns>教室下的课程信息</returns>
        public async Task<TblDatRoomCourse> GetByCourseId(long courseId)
        {
            return await _tblDatRoomCourseRepository.Value.GetDatRoomCourseByWhere(_classRoomId, courseId);
        }
        #endregion

        #region AddClassRoomAsync 添加教室
        /// <summary>
        /// 添加教室
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <param name="roomNo">门牌号</param>
        /// <returns>返回教室ID</returns>
        internal static async Task<long> AddClassRoomAsync(string schoolId, string roomNo)
        {
            TblDatClassRoomRepository classRoomRepository = new TblDatClassRoomRepository();

            //判断门牌号码是否存在
            var classRoom = await classRoomRepository.GetClassRoomByWhere(schoolId, roomNo);

            //添加教室
            if (classRoom == null)
            {
                classRoom = new TblDatClassRoom
                {
                    ClassRoomId = IdGenerator.NextId(),
                    CreateTime = DateTime.Now,
                    RoomNo = roomNo,
                    SchoolId = schoolId,
                    UpdateTime = DateTime.Now
                };
                await classRoomRepository.AddTask(classRoom);
            }

            return classRoom.ClassRoomId;
        }
        #endregion

        #region  GetClassRoomBySchoolId 获取校区下的所有教室
        /// <summary>
        /// 获取校区下的所有教室
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <returns>校区下所有教室信息</returns>
        internal static List<ViewRoomCourse> GetClassRoomBySchoolId(string schoolId)
        {
            return new ViewRoomCourseRepository().Get(schoolId);
        }
        #endregion

        #region  GetClassRoomBySchoolId 获取校区下的所有教室
        /// <summary>
        /// 获取校区下的所有教室
        /// <para>作    者：Huang GaoLiang</para>
        /// <para>创建时间：2019-03-18</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <returns>校区下所有教室信息</returns>
        internal static List<TblDatClassRoom> GetClassRoomListBySchoolId(string schoolId)
        {
            return new TblDatClassRoomRepository().GetClassRoomBySchoolId(schoolId);
        }
        #endregion

        #region  GetClassRoomListByIds 根据教室编号集合获取教室信息
        /// <summary>
        /// 根据教室编号集合获取教室信息
        /// <para>作    者：Huang GaoLiang</para>
        /// <para>创建时间：2019-03-21 </para>
        /// </summary>
        /// <param name="roomIds">教室编号</param>
        /// <returns>返回教室信息</returns>
        internal static List<TblDatClassRoom> GetClassRoomListByIds(List<long> roomIds)
        {
            return new TblDatClassRoomRepository().GetClassRoomByRoomId(roomIds);
        }
        #endregion
    }
}
