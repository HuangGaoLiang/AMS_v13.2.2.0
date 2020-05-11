/*此代码由生成工具字段生成，生成时间2018/10/27 16:52:22 */

using AMS.Storage.Models;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描    述: TblCstStudent仓储
    /// <para>作    者: Huang GaoLiang</para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class TblCstStudentRepository : BaseRepository<TblCstStudent>
    {

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public TblCstStudentRepository()
        {

        }

        /// <summary>
        /// 带上下文的构造函数
        /// </summary>
        /// <param name="context">数据上下文</param>
        public TblCstStudentRepository(DbContext context) : base(context)
        {

        }

        /// <summary>
        /// 根据关键词搜索学生
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-27</para>
        /// </summary>
        /// <param name="key">关键词</param>
        /// <returns>学生列表信息</returns>
        public List<TblCstStudent> GetStudentsByKey(string key)
        {
            var queryable = from a in base.CurrentContext.TblCstStudent
                            let mobile = (a.ContactPersonMobile.IndexOf(",") > 0 ? a.ContactPersonMobile.Remove(a.ContactPersonMobile.IndexOf(",")) : a.ContactPersonMobile)
                            where a.StudentName.Contains(key) || mobile.Contains(key)
                            select a;

            return queryable.ToList();
        }

        /// <summary>
        /// 根据电话号码和学生名称查询学生信息
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-10-29</para>
        /// </summary>
        /// <param name ="studentId"> 学生编号 </param >
        /// <param name="studentName">学生姓名</param>
        /// <param name="linkMobile">手机号</param>
        /// <returns>返回学生集合</returns>
        public TblCstStudent GetCstStudentId(long studentId, string studentName, string linkMobile)
        {
            return base.Load(m => m.StudentId != studentId && m.StudentName == studentName && m.LinkMobile == linkMobile);
        }

        /// <summary>
        /// 根据学生编号，获取学生详细信息
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-11-02</para>
        /// </summary>
        /// <param name="studentId">学生编号</param>
        /// <returns>返回学生详细信息</returns>
        public TblCstStudent GetCstStudentId(long studentId)
        {
            return base.Load(m => m.StudentId == studentId);
        }

        /// <summary>
        /// 根据学生编号、学生名称、联系电话查询学生信息
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-11-02</para>
        /// </summary>
        /// <param name="stuName">学生姓名</param>
        /// <param name="linkMobile">联系电话</param>
        /// <param name="studentId">学生编号</param>
        /// <returns>返回学生详细信息</returns>
        public List<TblCstStudent> GetCstStudentList(string stuName, string linkMobile, long studentId)
        {
            return base.LoadList(m => m.StudentName == stuName && m.LinkMobile == linkMobile).WhereIf(studentId > 0, m => m.StudentId != studentId).ToList();
        }

        /// <summary>
        /// 根据学生编号获取学生信息
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-11-21</para>
        /// </summary>
        /// <param name="ids">学生编号集合</param>
        /// <returns>返回学生集合</returns>
        public async Task<List<TblCstStudent>> GetStudentsByIdTask(IEnumerable<long> ids)
        {
            return await this.LoadLisTask(m => ids.Contains(m.StudentId));
        }

        /// <summary>
        /// 根据学生编号获取学生信息
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-11-21</para>
        /// </summary>
        /// <param name="ids">学生编号集合</param>
        /// <returns>返回学生集合</returns>
        public List<TblCstStudent> GetStudentsById(IEnumerable<long> ids)
        {
            return this.LoadList(m => ids.Contains(m.StudentId));
        }

        /// <summary>
        /// 根据学生电话号码获取学生信息
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2019-03-08 </para>
        /// </summary>
        /// <param name="mobile">电话号码</param>
        /// <returns>返回学生集合</returns>
        public List<TblCstStudent> GetStudentList(string mobile)
        {
            return this.LoadList(m => m.LinkMobile == mobile);
        }

        /// <summary>
        /// 根据一组学生电话号码，获取学生数据
        /// <para>作    者: 蔡亚康 </para>
        /// <para>创建时间: 2019-03-21 </para>
        /// </summary>
        /// <param name="mobiles">电话号码</param>
        /// <returns>返回学生集合</returns>
        public List<TblCstStudent> SearchByMobiles(List<string> mobiles)
        {
            return this.LoadList(m => mobiles.Contains(m.LinkMobile));
        }

        /// <summary>
        /// 根据学生电话号码获取学生信息
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2019-03-18 </para>
        /// </summary>
        /// <param name="mobiles">电话号码</param>
        /// <returns>返回学生集合</returns>
        public List<TblCstStudent> GetStudentByMobileList(List<string> mobiles)
        {
            return this.LoadList(m => mobiles.Contains(m.LinkMobile) || mobiles.Contains(m.ContactPersonMobile));
        }
    }
}
