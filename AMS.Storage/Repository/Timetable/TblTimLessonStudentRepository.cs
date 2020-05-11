/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AMS.Storage.Context;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;
using AMS.Dto;
using System;
using Jerrisoft.Platform.Storage;
using System.Linq.Expressions;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblTimLessonStudent仓储
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-07</para>
    /// </summary>
    public class TblTimLessonStudentRepository : BaseRepository<TblTimLessonStudent>
    {
        /// <summary>
        /// 课次基础信息仓储实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <param name="context">数据库上下文</param>
        public TblTimLessonStudentRepository(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// 课次基础信息仓储实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        public TblTimLessonStudentRepository()
        {
        }

        /// <summary>
        /// 根据校区Id和学生Id获取学生课次时间列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-07</para>
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public async Task<List<TblTimLessonStudent>> GetListAsync(string schoolId, long studentId)
        {
            return await LoadLisTask(a => a.SchoolId == schoolId && a.StudentId == studentId);
        }

        /// <summary>
        /// 根据学生课次Id集合获取学生课次信息列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="lessonStudentIdList">学生课次Id集合</param>
        /// <returns>学生课次信息列表</returns>
        public async Task<List<TblTimLessonStudent>> GetListByLessonStudentIdAsnyc(IEnumerable<long> lessonStudentIdList)
        {
            return await LoadLisTask(a => lessonStudentIdList.Contains(a.LessonStudentId));
        }

        /// <summary>
        /// 获取学生课次时间列表
        /// <para>作    者：Huang GaoLiang</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <param name="studentId">学生编号</param>
        /// <param name="attendStatus">考勤状态</param>
        /// <returns>返回学生课次时间集合</returns>
        public List<TblTimLessonStudent> GetList(string schoolId, long studentId, List<int> attendStatus)
        {
            return LoadList(m => m.SchoolId == schoolId
                                && m.StudentId == studentId)
                                .WhereIf(attendStatus != null && attendStatus.Any(), m => attendStatus.Contains(m.AttendStatus))
                                .ToList();
        }

        /// <summary>
        /// 查询学生课次时间信息
        /// <para>作    者：Huang GaoLiang</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <param name="studentId">学生编号</param>
        /// <param name="lessonId">课次编号</param>
        /// <returns>返回学生课次时间信息</returns>
        public TblTimLessonStudent GetTblTimLessonStudent(string schoolId, long studentId, long lessonId)
        {
            return Load(m => m.SchoolId == schoolId
                                && m.StudentId == studentId
                                && m.LessonId == lessonId);
        }

        /// <summary>
        /// 根据课次Id删除学生排课
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-24</para>
        /// </summary>
        /// <param name="lessonIds">一组课次Id</param>
        /// <returns>true:成功 false:失败</returns>
        public void DeleteByLessonId(IEnumerable<long> lessonIds)
        {
            base.Delete(x => lessonIds.Contains(x.LessonId));
        }

        /// <summary>
        /// 更新学生课程的考勤状态
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="lessonStudentId">学生考勤ID集合</param>
        /// <param name="newAttendStatus">新考勤状态</param>
        /// <param name="oldAttendStatus">旧考勤状态</param>
        /// <param name="oldAdjustType">旧补课/调课状态</param>
        /// <param name="attendTime">考勤时间</param>
        /// <param name="attendUserType">签到人员类型</param>
        public bool UpdateAttendStatus(IEnumerable<long> lessonStudentId,
            AttendStatus newAttendStatus, AttendStatus oldAttendStatus,
            AdjustType oldAdjustType, DateTime attendTime, AttendUserType attendUserType)
        {
            Expression<Func<TblTimLessonStudent, bool>> sourceLambda =
                x => lessonStudentId.Contains(x.LessonStudentId) &&
                     x.AttendStatus == (int)oldAttendStatus &&
                     x.AdjustType == (int)oldAdjustType;

            Expression<Func<TblTimLessonStudent, TblTimLessonStudent>> whereLambda =
                x => new TblTimLessonStudent()
                {
                    AttendStatus = (int)newAttendStatus,
                    AttendUserType = (int)attendUserType,
                    UpdateTime = DateTime.Now,
                    AttendDate = attendTime
                };

            return base.Update(sourceLambda, whereLambda);
        }

        /// <summary>
        /// 更新学生课程的考勤状态
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
            return Update(x => lessonIdList.Contains(x.LessonId) && x.AttendStatus == (int)oldAttendStatus && x.AdjustType == (int)oldAdjustType,
                  x => new TblTimLessonStudent()
                  {
                      AttendStatus = (int)newAttendStatus,
                      AttendUserType = (int)attendUserType,
                      UpdateTime = DateTime.Now
                  });
        }

        /// <summary>
        /// 更新学生课程的考勤状态（异步）
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="lessonStudentId">学生考勤ID集合</param>
        /// <param name="newAttendStatus">新考勤状态</param>
        /// <param name="oldAttendStatus">旧考勤状态</param>
        /// <param name="attendTime">考勤时间</param>
        /// <param name="oldAdjustType">旧补课/调课状态</param>
        /// <param name="attendUserType">签到人员类型</param>
        public async Task<bool> UpdateAttendStatusAsync(IEnumerable<long> lessonStudentId,
            AttendStatus newAttendStatus,
            AttendStatus oldAttendStatus,
            DateTime? attendTime,
            AdjustType oldAdjustType = AdjustType.DEFAULT,
            AttendUserType attendUserType = AttendUserType.DEFAULT)
        {
            return await base.UpdateTask(x => lessonStudentId.Contains(x.LessonStudentId) && x.AttendStatus == (int)oldAttendStatus && x.AdjustType == (int)oldAdjustType,
                  x => new TblTimLessonStudent()
                  {
                      AttendStatus = (int)newAttendStatus,
                      AttendUserType = (int)attendUserType,
                      AttendDate = attendTime,
                      UpdateTime = attendTime
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
                x => lessonStudentId.Contains(x.LessonStudentId) && x.AttendStatus == (int)oldAttendStatus,
                m => new TblTimLessonStudent { AdjustType = (int)adjustType, AttendDate = time, ReplenishCode = replenishCode });
        }

        /// <summary>
        /// 根据考勤码获取考勤信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="replenishCode">考勤补签码</param>
        /// <returns>true:存在 false:不存在</returns>
        public TblTimLessonStudent GetByReplenishCode(string replenishCode)
        {
            return LoadQueryable(x => x.ReplenishCode == replenishCode, false).FirstOrDefault();
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
               m => new TblTimLessonStudent { AdjustType = (int)newAdjustType, UpdateTime = DateTime.Now });
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
               m => new TblTimLessonStudent { AdjustType = (int)newAdjustType, UpdateTime = DateTime.Now });
        }
    }
}
