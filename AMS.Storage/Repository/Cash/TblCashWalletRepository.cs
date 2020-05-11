/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AMS.Storage.Context;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblCashWallet仓储
    /// </summary>
    public class TblCashWalletRepository : BaseRepository<TblCashWallet>
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public TblCashWalletRepository()
        {
        }

        /// <summary>
        /// 带上下文参数的构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public TblCashWalletRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 根据学生ID获取钱包
        /// </summary>
        /// <param name="schoolId">校区ID</param>
        /// <param name="studentId">学生ID</param>
        /// <returns></returns>
        public TblCashWallet GetBySchoolStudentId(string schoolId, long studentId)
        {
            return this.LoadQueryable()
                        .Where(t => t.SchoolId == schoolId && t.StudentId == studentId)
                        .FirstOrDefault();
        }
    }
}
