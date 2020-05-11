/*此代码由生成工具字段生成，生成时间2019/3/7 16:44:09 */

using System;
using System.Collections.Generic;
using System.Linq;
using AMS.Dto;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblTimReplenishLesson仓储
    /// </summary>
    public class TblTimReplenishLessonRepository : BaseRepository<TblTimReplenishLesson>
    {
        public TblTimReplenishLessonRepository(DbContext context) : base(context)
        {

        }

        public TblTimReplenishLessonRepository()
        {

        }

        /// <summary>
        /// 根据根课程Id获取对应的补课/调课信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="rootLessonId">根课程Id</param>
        /// <returns>补课信息列表</returns>
        public List<TblTimReplenishLesson> GetLessonListByRootId(long rootLessonId)
        {
            return LoadList(w => w.RootLessonId == rootLessonId);
        }

        /// <summary>
        /// 更新学生补课/调课的考勤状态
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="lessonStudentId">学生考勤ID集合</param>
        /// <param name="newAttendStatus">新考勤状态</param>
        /// <param name="oldAttendStatus">旧考勤状态</param>
        /// <param name="oldAdjustType">旧补课/调课状态</param>
        /// <param name="time">考勤时间</param>
        /// <param name="attendUserType">签到人员类型</param>
        public bool UpdateAttendStatus(IEnumerable<long> lessonStudentId,
            AttendStatus newAttendStatus,
            AttendStatus oldAttendStatus,
            AdjustType oldAdjustType,
            DateTime time,
            AttendUserType attendUserType = AttendUserType.DEFAULT)
        {
            return base.Update(x => lessonStudentId.Contains(x.ReplenishLessonId) && x.AttendStatus == (int)oldAttendStatus && x.AdjustType == (int)oldAdjustType,
                 m => new TblTimReplenishLesson()
                 {
                     AttendStatus = (int)newAttendStatus,
                     AttendUserType = (int)attendUserType,
                     AttendDate = time,
                     UpdateTime = time
                 });
        }

        /// <summary>
        /// 更新学生补课/调课的考勤状态
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-16</para>
        /// </summary>
        /// <param name="lessonIdList">学生考勤ID集合</param>
        /// <param name="newAttendStatus">新考勤状态</param>
        /// <param name="oldAttendStatus">旧考勤状态</param>
        /// <param name="oldAdjustType">旧补课/调课状态</param>
        /// <param name="attendUserType">签到人员类型</param>
        public bool UpdateAttendStatus(IEnumerable<long> lessonIdList,
            AttendStatus newAttendStatus,
            AttendStatus oldAttendStatus,
            AdjustType oldAdjustType = AdjustType.DEFAULT,
            AttendUserType attendUserType = AttendUserType.DEFAULT)
        {
            return base.Update(x => lessonIdList.Contains(x.LessonId) && x.AttendStatus == (int)oldAttendStatus && x.AdjustType == (int)oldAdjustType,
                 m => new TblTimReplenishLesson()
                 {
                     AttendStatus = (int)newAttendStatus,
                     AttendUserType = (int)attendUserType,
                     UpdateTime = DateTime.Now
                 });
        }

        /// <summary>
        /// 补签学生考勤
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="lessonStudentId">学生考勤ID集合</param>
        /// <param name="adjustType">调整状态</param>
        /// <param name="oldAttendStatus">旧考勤状态</param>
        /// <param name="time">考勤时间</param>
        /// <param name="replenishCode">补签码</param>
        public void UpdateStuAttend(
            IEnumerable<long> lessonStudentId, AdjustType adjustType,
            AttendStatus oldAttendStatus, DateTime time, string replenishCode)
        {
            base.Update(
                x => lessonStudentId.Contains(x.ReplenishLessonId) && x.AttendStatus == (int)oldAttendStatus,
                m => new TblTimReplenishLesson { AdjustType = (int)adjustType, AttendDate = time, ReplenishCode = replenishCode, UpdateTime = DateTime.Now });
        }

        /// <summary>
        /// 根据根课次编号查询补课信息记录
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-08</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <param name="studentId">学生编号</param>
        /// <param name="rootLessonIds">根课次ID</param>
        /// <param name="attendStatus">状态</param>
        /// <returns>返回补课信息记录</returns>
        public List<TblTimReplenishLesson> GetTimReplenishLessonList(string schoolId, long studentId, List<long> rootLessonIds, int attendStatus)
        {
            return base.LoadList(m => m.SchoolId == schoolId && m.StudentId == studentId && rootLessonIds.Contains(m.RootLessonId) && m.AttendStatus == attendStatus);
        }

        /// <summary>
        /// 根据考勤码获取考勤信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="replenishCode">考勤补签码</param>
        /// <returns>true:存在 false:不存在</returns>
        public TblTimReplenishLesson GetByReplenishCode(string replenishCode)
        {
            return base.Load(x => x.ReplenishCode == replenishCode);
        }

        /// <summary>
        /// 根据考勤码更新家长已确认补签
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="replenishCode">考勤补签码</param>
        /// <param name="oldAdjustType">旧类型</param>
        /// <param name="newAdjustType">新类型</param>
        public void UpdateAdjustTypeByReplenishCode(string replenishCode, AdjustType oldAdjustType, AdjustType newAdjustType)
        {
            base.Update(
               x => x.ReplenishCode == replenishCode && x.AdjustType == (int)oldAdjustType,
               m => new TblTimReplenishLesson { AdjustType = (int)newAdjustType, UpdateTime = DateTime.Now });
        }

        /// <summary>
        /// 根据课次Id获取对应的补课/调课信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="lessonIds">课次Id</param>
        /// <returns>补课信息列表</returns>
        public List<TblTimReplenishLesson> GetLessonListByLessonId(IEnumerable<long> lessonIds)
        {
            return LoadList(x => lessonIds.Contains(x.LessonId));
        }

        /// <summary>
        /// 根据课次ID更新已补课/已调课
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="lessonIdList">课次ID</param>
        /// <param name="oldAdjustType">旧类型</param>
        /// <param name="newAdjustType">新类型</param>
        public void UpdateAdjustTypeByLessonId(IEnumerable<long> lessonIdList, AdjustType oldAdjustType, AdjustType newAdjustType)
        {
            base.Update(
               x => lessonIdList.Contains(x.LessonId) && x.AdjustType == (int)oldAdjustType,
               m => new TblTimReplenishLesson { AdjustType = (int)newAdjustType, UpdateTime = DateTime.Now });
        }

        /// <summary>
        /// 根据课次ID上课调整课次表数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="lessonIdList">课次ID</param>
        public void DeleteByLessonId(IEnumerable<long> lessonIdList)
        {
            base.Delete(x => lessonIdList.Contains(x.LessonId));
        }
    }
}
