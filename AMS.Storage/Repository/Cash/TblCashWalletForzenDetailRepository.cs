/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblCashWalletForzenDetail仓储
    /// </summary>
    public class TblCashWalletForzenDetailRepository : BaseRepository<TblCashWalletForzenDetail>
    {
        public TblCashWalletForzenDetailRepository(DbContext context) : base(context)
        {

        }

        public TblCashWalletForzenDetailRepository()
        {

        }

        /// <summary>
        /// 根据业务获取冻结明细
        /// </summary>
        /// <param name="schoolId">校区ID</param>
        /// <param name="businessType">业务类型</param>
        /// <param name="businessId">业务ID</param>
        /// <param name="status">查询状态</param>
        /// <returns></returns>
        public TblCashWalletForzenDetail GetByBusinessId(string schoolId, int businessType, long businessId, int status)
        {
            return this.LoadQueryable()
                        .Where(t =>
                                    t.SchoolId == schoolId
                                    && t.BusinessType == businessType
                                    && t.BusinessId == businessId
                                    && t.Status == status)
                        .FirstOrDefault();
        }

        /// <summary>
        /// 根据校区id和学生Id获取学生钱包余额冻结明细
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-14</para>
        /// </summary>
        /// <param name="schoolId">校区id</param>
        /// <param name="studentId">学生id</param>
        /// <returns>学生钱包余额冻结明细</returns>
        public async Task<List<TblCashWalletForzenDetail>> GetByStudentId(string schoolId, long studentId)
        {
            return await LoadLisTask(a => a.SchoolId == schoolId && a.StudentId == studentId);
        }
    }
}
