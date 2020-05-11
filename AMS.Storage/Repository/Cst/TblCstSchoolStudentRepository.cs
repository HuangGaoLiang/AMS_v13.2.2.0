/*此代码由生成工具字段生成，生成时间2018/10/27 16:52:22 */

using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描    述: TblCstSchoolStudent仓储
    /// <para>作    者: Huang GaoLiang</para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class TblCstSchoolStudentRepository : BaseRepository<TblCstSchoolStudent>
    {
        /// <summary>
        /// TblCstSchoolStudent无参构造函数
        /// </summary>
        public TblCstSchoolStudentRepository()
        {
        }

        /// <summary>
        /// TblCstSchoolStudent 带上下文的构造函数 
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public TblCstSchoolStudentRepository(DbContext dbContext) : base(dbContext)
        {
        }
        /// <summary>
        /// 更新学生信息
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-11-21</para>
        /// </summary>
        /// <param name="schoolStudent">校区学生实体</param>
        public async Task UpdateStudyRemindClassTimes(TblCstSchoolStudent schoolStudent)
        {
            await this.UpdateTask(schoolStudent);
        }


        /// <summary>
        /// 修改学生状态
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-11-12</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <param name="studentIds">学生编号集合</param>
        /// <param name="newStatus">学生状态（1在读 2休学 3流失）</param>
        public async Task UpdateStudyStatus(string schoolId, List<long> studentIds, int newStatus)
        {
            Expression<Func<TblCstSchoolStudent, TblCstSchoolStudent>> whereLambda = t => new TblCstSchoolStudent
            {
                StudyStatus = newStatus
            };

            await this.UpdateTask(t => t.SchoolId == schoolId && studentIds.Contains(t.StudentId), whereLambda);
        }

        /// <summary>
        /// 根据学生编号获取校区编号
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-11-02</para>
        /// </summary>
        /// <param name="studentIds">学生编号集合</param>
        /// <returns>返回校区学生集合数据</returns>
        public List<TblCstSchoolStudent> GetSchoolIdByStudentIds(List<long> studentIds)
        {
            return LoadList(m => studentIds.Contains(m.StudentId)).ToList();
        }

        /// <summary>
        /// 根据学生编号获取学生信息
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-11-26</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <param name="studentId">学生编号</param>
        /// <returns>返回学生信息</returns>
        public async Task<TblCstSchoolStudent> GetStudentById(string schoolId, long studentId)
        {
            return await LoadTask(m => m.SchoolId == schoolId && m.StudentId == studentId);
        }

        /// <summary>
        /// 统计校区学生数据
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2019-03-06</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <returns>返回本校的学生总数量</returns>
        public long GetSchoolStudent(string schoolId)
        {
            return LoadList(m => m.SchoolId == schoolId).Count;
        }
    }
}
