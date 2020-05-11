/*此代码由生成工具字段生成，生成时间2018/9/7 12:42:58 */
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Dto;
using AMS.Storage.Models;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblDatClass仓储
    /// </summary>
    public class TblDatClassRepository : BaseRepository<TblDatClass>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public TblDatClassRepository(DbContext context) : base(context)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public TblDatClassRepository()
        {

        }
        /// <summary>
        /// 根据学期Id查找班级信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-25</para>
        /// </summary>
        /// <param name="termId">学期Id</param>
        /// <returns>班级列表</returns>
        public List<TblDatClass> GetTermIdByClass(long termId)
        {
            return base.LoadList(x => x.TermId == termId);
        }

        /// <summary>
        /// 根据班级编号查找班级信息
        /// <para>作    者：Huang GaoLiang</para>
        /// <para>创建时间：2019-03-21</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <param name="classNo">班级编号</param>
        /// <param name="classType">班级类型</param>
        /// <returns>班级列表</returns>
        public TblDatClass GetClassByClassNo(string schoolId, string classNo, ClassType classType)
        {
            return base.Load(x => x.SchoolId == schoolId && x.ClassNo == classNo && x.ClassType == (int)classType);
        }


        /// <summary>
        /// 根据学期Id查找班级信息
        /// 作     者:Caiyakang 2018.09.25
        /// </summary>
        /// <param name="termId">一组学期ID</param>
        /// <param name="classNo">班级代号</param>
        /// <param name="schoolId">校区编号</param>
        /// <returns></returns>
        public TblDatClass GetByClassNo(List<long> termId, string classNo, string schoolId)
        {
            return base.Load(t => termId.Contains(t.TermId) && t.ClassNo == classNo && t.SchoolId == schoolId);
        }

        /// <summary>
        /// 学期下是否存在班级
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-11</para>
        /// </summary>
        /// <param name="termId">学期Id</param>
        /// <returns>true:存在 false:不存在</returns>
        public bool ExistClass(long termId)
        {
            return base.IsExist(x => x.TermId == termId);
        }

        /// <summary>
        /// 根据学期Id获取班级课表信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-25</para>
        /// </summary>
        /// <param name="termId">学期Id</param>
        /// <returns>班级列表</returns>
        public async Task<List<TblDatClass>> GetClassByTermIdAsync(long termId)
        {
            return await base.LoadLisTask(x => x.TermId == termId);
        }

        /// <summary>
        /// 课程是否被使用
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-15</para>
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <returns>true:已使用 false:未使用</returns>
        public async Task<bool> CourseIsUse(long courseId)
        {
            return await base.IsExistAsync(x => x.CourseId == courseId);
        }

        /// <summary>
        /// 课程教室是否被使用
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-15</para>
        /// </summary>
        /// <param name="roomCourseId">教室课程Id</param>
        /// <returns>true:已使用 false:未使用</returns>
        public async Task<bool> CourseIsUseByRoomCourseId(long roomCourseId)
        {
            return await base.IsExistAsync(x => x.RoomCourseId == roomCourseId);
        }

        /// <summary>
        /// 根据学期删除班级信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="termId">学期</param>
        public void DeleteByTermId(long termId)
        {
            base.Delete(t => t.TermId == termId);
        }


        /// <summary>
        /// 根据一组班级Id获取数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="classId">一组班级Id</param>
        /// <returns>班级列表</returns>
        public async Task<List<TblDatClass>> GetByClassIdAsync(IEnumerable<long> classId)
        {
            return await base.LoadLisTask(x => classId.Contains(x.ClassId));
        }

        /// <summary>
        /// 根据一组班级Id获取数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-02</para>
        /// </summary>
        /// <param name="classId">一组班级Id</param>
        /// <returns>班级列表</returns>
        public List<TblDatClass> GetByClassId(IEnumerable<long> classId)
        {
            return base.LoadList(x => classId.Contains(x.ClassId));
        }


        /// <summary>
        /// 根据条件查找班级信息
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>2018年10月11日 </para>
        /// </summary>
        /// <param name="termId">学期Id</param>
        /// <param name="courseId">课程编号</param>
        /// <param name="courseLeaveId">课程等级编号</param>
        /// <param name="roomId">教室编号</param>
        /// <param name="classType">班级类型</param>
        /// <returns>返回班级信息</returns>
        public List<TblDatClass> GetTermIdByClass(long termId, long courseId, long courseLeaveId, long roomId, ClassType classType)
        {
            return base.LoadList(x => x.TermId == termId
               && x.ClassType == (int)classType)
                .WhereIf(courseId > 0, m => m.CourseId == courseId)
                .WhereIf(courseLeaveId > 0, m => m.CourseLeveId == courseLeaveId)
                .WhereIf(roomId > 0, m => m.ClassRoomId == roomId).ToList();
        }

        /// <summary>
        /// 根据校区编号和老师编号获取老师的班级信息
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>2019年03月15日 </para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <param name="teacherId">老师编号</param>
        /// <returns>返回班级信息</returns>
        public List<TblDatClass> GetClassListByTeacherId(string schoolId, string teacherId)
        {
            return base.LoadList(m => m.SchoolId == schoolId && m.TeacherId == teacherId);
        }
    }
}
