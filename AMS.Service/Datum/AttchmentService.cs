using AMS.Core;
using AMS.Dto;
using AMS.Models;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Service
{
    /// <summary>
    /// 附件服务
    /// </summary>
    public class AttchmentService
    {

        private readonly string _schoolId;
        private Lazy<TblDatAttchmentRepository> _repository;
        private readonly UnitOfWork _unitOfWork;//工作单元

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="unitOfWork"></param>
        public AttchmentService(string schoolId, UnitOfWork unitOfWork = null)
        {
            this._schoolId = schoolId;
            this._unitOfWork = unitOfWork;

            //初始化仓储
            InitUnitOfWork();
        }

        /// <summary>
        /// 初始化仓储
        /// </summary>
        /// <param name="unitOfWork"></param>
        private void InitUnitOfWork()
        {
            _repository = this._unitOfWork != null
                ? new Lazy<TblDatAttchmentRepository>(() =>
                {
                    return this._unitOfWork.GetCustomRepository<TblDatAttchmentRepository, TblDatAttchment>();
                }) : new Lazy<TblDatAttchmentRepository>();
        }

        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="businessId">业务ID</param>
        /// <param name="attchType">附件类型</param>
        /// <returns></returns>
        public List<AttchmentDetailResponse> GetAttchList(long businessId, AttchmentType attchType)
        {
            var list = _repository.Value.GetByBusinessId(businessId, attchType.ToString());
            return Mapper.Map<List<TblDatAttchment>, List<AttchmentDetailResponse>>(list);
        }


        /// <summary>
        /// 添加一个附件
        /// </summary>
        /// <param name="dto"></param>
        public void Add(AttchmentAddRequest dto)
        {
            List<AttchmentAddRequest> dtoList = new List<AttchmentAddRequest>() { dto };
            this.Add(dtoList);
        }


        /// <summary>
        /// 添加一组附件
        /// </summary>
        /// <param name="dtoList"></param>
        public void Add(List<AttchmentAddRequest> dtoList)
        {
            List<TblDatAttchment> list = GetAttachmentList(dtoList);
            _repository.Value.Add(list);
        }

        /// <summary>
        /// 异步添加附件信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="dtoList">附件信息列表</param>
        public async Task AddAsync(List<AttchmentAddRequest> dtoList)
        {
            List<TblDatAttchment> list = GetAttachmentList(dtoList);
            await _repository.Value.AddTask(list);
        }

        /// <summary>
        /// 获取附件信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="dtoList">附件信息列表</param>
        /// <returns></returns>
        private List<TblDatAttchment> GetAttachmentList(List<AttchmentAddRequest> dtoList)
        {
            List<TblDatAttchment> list = new List<TblDatAttchment>();
            dtoList.ForEach(d =>
            {
                list.Add(new TblDatAttchment()
                {
                    AttchmentId = IdGenerator.NextId(),
                    AttchmentType = d.AttchmentType.ToString(),
                    BusinessId = d.BusinessId,
                    CreateTime = DateTime.Now,
                    SchoolId = this._schoolId,
                    Url = d.Url,
                    Name = d.Name
                });
            });
            return list;
        }

        /// <summary>
        /// 删除一个附件
        /// </summary>
        /// <param name="attchmentId">附件ID</param>
        public void Remove(long attchmentId)
        {
            this.Remove(new List<long>() { attchmentId });
        }

        /// <summary>
        /// 删除一组附件
        /// </summary>
        /// <param name="attchmentId">一组附件的ID</param>
        public void Remove(List<long> attchmentId)
        {
            _repository.Value.DeleteById(this._schoolId, attchmentId);
        }




    }
}
