using System.Collections.Generic;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;

namespace AMS.Service
{
    /// <summary>
    /// 操作日记
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-02-20</para>
    /// </summary>
    internal class OperationLogService
    {
        private readonly TblDatOperationLogRepository _operationLogRepository;

        /// <summary>
        /// 业务操作日志记录
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        internal OperationLogService()
        {
            this._operationLogRepository = new TblDatOperationLogRepository();
        }

        /// <summary>
        /// 添加操作日记
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="model">内部操作直接传实体即可</param>
        internal void Add(TblDatOperationLog model, UnitOfWork unitOfWork=null)
        {
            TblDatOperationLogRepository repository = null;
            if (unitOfWork == null)
            {
                repository = new TblDatOperationLogRepository();
            }
            else
            {
                repository = unitOfWork.GetCustomRepository<TblDatOperationLogRepository, TblDatOperationLog>();
            }
            repository.Add(model);
        }

        /// <summary>
        /// 获取操作日志
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-14</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="businessId">业务Id</param>
        /// <returns>业务日志记录列表</returns>
        internal List<TblDatOperationLog> GetList(long businessId)
        {
            return _operationLogRepository.GetList(businessId);
        }
    }
}
