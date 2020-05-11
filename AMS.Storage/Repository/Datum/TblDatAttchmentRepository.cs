/*此代码由生成工具字段生成，生成时间2018/11/5 16:44:29 */
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AMS.Storage.Repository;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage
{
    /// <summary>
    /// TblDatAttchment仓储
    /// </summary>
    public class TblDatAttchmentRepository : BaseRepository<TblDatAttchment>
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public TblDatAttchmentRepository()
        {
        }

        /// <summary>
        /// 带上下文参数的构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public TblDatAttchmentRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 根据ID删除附件
        /// </summary>
        /// <param name="schollId"></param>
        /// <param name="ids"></param>
        public void DeleteById(string schollId, List<long> ids)
        {
            this.Delete(t => t.SchoolId == schollId && ids.Contains(t.AttchmentId));
        }


        /// <summary>
        /// 根据业务ID和类型查找附件
        /// </summary>
        /// <param name="businessId">业务ID</param>
        /// <param name="attchType">附件类型</param>
        /// <returns></returns>
        public List<TblDatAttchment> GetByBusinessId(long businessId, string attchType)
        {
            return this.LoadQueryable(t => t.BusinessId == businessId && t.AttchmentType == attchType,false)
                       .ToList();
        }


    }
}
