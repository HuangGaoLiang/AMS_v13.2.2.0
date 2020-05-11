/*此代码由生成工具字段生成，生成时间2018/10/27 16:52:22 */

using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描    述: TblCstStudentContact仓储
    /// <para>作    者: Huang GaoLiang</para>
    /// <para>创建时间: 2019-03-06</para>
    /// </summary>
    public class TblCstStudentContactRepository : BaseRepository<TblCstStudentContact>
    {

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public TblCstStudentContactRepository()
        {

        }

        /// <summary>
        /// 带上下文的构造函数
        /// </summary>
        /// <param name="context">数据上下文</param>
        public TblCstStudentContactRepository(DbContext context) : base(context)
        {

        }

        /// <summary>
        /// 根据学生编号删除学生联系人信息
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2019-03-06</para>
        /// </summary>
        /// <param name="studentIds">学生编号</param>
        /// <returns></returns>
        public bool DeleteByStudentId(List<long> studentIds)
        {
            return base.Delete(m => studentIds.Contains(m.StudentId));
        }

        /// <summary>
        /// 根据学生ID获取学生信息
        /// <para>作    者: 蔡亚康 </para>
        /// <para>创建时间: 2019-03-21 </para>
        /// </summary>
        /// <param name="studentId">学生ID</param>
        /// <returns>返回学生集合</returns>
        public List<TblCstStudentContact> GetByStudentId(long studentId)
        {
            return this.LoadList(m => m.StudentId == studentId);
        }

        /// <summary>
        /// 根据学生电话号码获取学生信息
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2019-03-08 </para>
        /// </summary>
        /// <param name="mobile">电话号码</param>
        /// <returns>返回学生联系人集合</returns>
        public List<TblCstStudentContact> GetStudentList(string mobile)
        {
            return this.LoadList(m => m.Mobile == mobile);
        }



        /// <summary>
        /// 根据一组手机号码获取学生联系人集合
        /// <para>作    者: 蔡亚康</para>
        /// <para>创建时间: 2019-03-21 </para>
        /// </summary>
        /// <param name="mobiles">一组手机号码</param>
        /// <returns>返回学生联系人集合</returns>
        public List<TblCstStudentContact> SearchByMobiles(List<string> mobiles)
        {
            return this.LoadList(m => mobiles.Contains(m.Mobile));
        }
    }
}
