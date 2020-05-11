using System;
using System.Collections.Generic;
using System.Linq;
using AMS.Anticorrosion.HRS;
using AMS.Core;
using AMS.Dto;
using AMS.Dto.Dto.AuditFlow;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using AutoMapper;
using FP3.Core.ServiceInput;
using Jerrisoft.Platform.Log;
using YMM.Storage.Models;
using YMM.Test.Storage.Repository;

namespace AMS.Service
{
    /// <summary>
    /// 描述:表示一批赠与奖学金设置的审核
    /// <para>作    者：caiyakang</para>
    /// <para>创建时间：2018-10-24</para>
    /// </summary>
    public class CouponRuleAuditService : BaseAuditService
    {
        private readonly string _schoolId;                                                                             //校区Id
        private readonly TblAutCouponRuleRepository _tblAutCouponRuleRepository = new TblAutCouponRuleRepository();    //赠与奖学金审核表仓储

        /// <summary>
        /// 描述：实例化校区和仓储
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-24</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        public CouponRuleAuditService(string schoolId)
        {
            this._schoolId = schoolId;
            base.TblAutAudit = base._tblAutAuditRepository.GetTblAutAuditByExtField(AuditBusinessType.ScholarshipGive, schoolId, null);
        }

        /// <summary>
        /// 描述：根据审核主表实例化对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="tblAutAudit">审核主表实体</param>
        private CouponRuleAuditService(TblAutAudit tblAutAudit)
        {
            base.TblAutAudit = tblAutAudit;
            this._schoolId = tblAutAudit.ExtField1.Trim();
        }

        /// <summary>
        /// 描述：根据审核主健创建一个实例
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="auditId">审核主表Id</param>
        /// <returns>赠与奖学金表服务实例</returns>
        public static CouponRuleAuditService CreateByAutitId(long auditId)
        {
            TblAutAuditRepository repository = new TblAutAuditRepository();
            var entity = repository.Load(auditId);
            return new CouponRuleAuditService(entity);
        }


        /// <summary>
        /// 描述：获取待提交审核及审核中的数据信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <returns>待提交审核及审核中的流程审批数据信息</returns>
        public CouponRuleAuditResponse GetAuditInfo()
        {
            var entity = new CouponRuleAuditResponse();
            if (base.TblAutAudit != null && base.TblAutAudit.AuditStatus != (int)AuditStatus.Success)
            {
                var auditCouponRule = _tblAutCouponRuleRepository.GetAuditingCouponRule(base.TblAutAudit.AuditId);
                var ruleEntity = Mapper.Map<List<CouponRuleAuditDetailResponse>>(auditCouponRule);

                entity.Details = ruleEntity;
                entity.AuditUserId = base.TblAutAudit.AuditUserId;
                entity.AuditUserName = base.TblAutAudit.AuditUserName;
                entity.AuditDate = base.TblAutAudit.AuditDate;
                entity.AuditStatus = (AuditStatus)base.TblAutAudit.AuditStatus;
                entity.Remark = base.TblAutAudit.DataExt;
                entity.IsFirstSubmitUser = base.IsFlowSubmitUser;
                entity.CreateUserName = base.TblAutAudit.CreateUserName;

            }
            return entity;
        }

        /// <summary>
        /// 描述：获取审核中的数据（平台审核端使用）
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-27</para>
        /// </summary>
        /// <returns>审核中的流程信息</returns>
        public List<CouponRuleAuditDetailResponse> GetAuditingCouponRuleList()
        {
            var auditCouponRule = _tblAutCouponRuleRepository.GetAuditingCouponRule(base.TblAutAudit.AuditId);
            var ruleEntityList = Mapper.Map<List<CouponRuleAuditDetailResponse>>(auditCouponRule);
            return ruleEntityList;
        }

        /// <summary>
        /// 描述：添加一个审核中的优惠设置
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="dto">要添加的赠与奖学金信息</param>
        /// <returns>无</returns>
        public void Add(CouponRuleDetailRequest dto)
        {
            //1、确定当前校区是否存在正在审核的赠与奖学金设置
            if (base.TblAutAudit != null && base.TblAutAudit.AuditStatus == (int)AuditStatus.Auditing)   //当前单据在审核中，不允许添加
            {
                throw new BussinessException(ModelType.Discount, 2);
            }

            //2、确定当前校区是否存在被退回或未提交的赠与奖学金设置
            //2.1向审核主表添加数据
            long auditId = 0;
            if (base.TblAutAudit == null || base.TblAutAudit.AuditStatus == (int)AuditStatus.Return || base.TblAutAudit.AuditStatus == (int)AuditStatus.Success)
            {
                if (dto.CouponType == CouponType.Recommend)
                {
                    ValidationCouponRule(dto.BeginDate, dto.EndDate);   //验证正式表是否已存在转介绍优惠
                }

                var auditModel = new TblAutAudit()
                {
                    AuditId = IdGenerator.NextId(),
                    SchoolId = this._schoolId,
                    BizType = (int)AuditBusinessType.ScholarshipGive,
                    ExtField1 = _schoolId,
                    ExtField2 = string.Empty,
                    FlowNo = string.Empty,
                    AuditStatus = (int)AuditStatus.WaitAudit,
                    AuditUserId = string.Empty,
                    AuditUserName = string.Empty,
                    AuditDate = DateTime.Now,
                    CreateUserId = "",
                    CreateUserName= "",
                    DataExt = string.Empty,
                    DataExtVersion = string.Empty,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                auditId = auditModel.AuditId;
                _tblAutAuditRepository.Add(auditModel);
            }
            else if (base.TblAutAudit.AuditStatus == (int)AuditStatus.WaitAudit)
            {
                if (dto.CouponType == CouponType.Recommend)
                {
                    //验证正式表是否已存在转介绍优惠
                    ValidationCouponRule(dto.BeginDate, dto.EndDate);
                    //验证临时表是否已存在转介绍优惠
                    ValidationAuditCouponRule(dto.BeginDate, dto.EndDate);
                }

                auditId = base.TblAutAudit.AuditId;
            }

            //2.2向赠与奖学金审核子表添加数据
            var couponRuleModel = Mapper.Map<TblAutCouponRule>(dto);
            couponRuleModel.AutCouponRuleId = IdGenerator.NextId();  //审核子表主键
            couponRuleModel.AuditId = auditId;                       //审核主表Id
            couponRuleModel.CouponRuleId = IdGenerator.NextId();     //奖学金正式表Id
            couponRuleModel.CreateTime = DateTime.Now;
            couponRuleModel.SchoolId = this._schoolId;

            _tblAutCouponRuleRepository.Add(couponRuleModel);
        }

        /// <summary>
        /// 描述：正式数据表转介绍时间段验证
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>无</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：11, 异常描述:此时间段内已存在转介绍的优惠设置
        /// </exception>
        private void ValidationCouponRule(DateTime beginDate, DateTime endDate)
        {
            var service = new CouponRuleService(this._schoolId);
            var result = service.GetTypeByCouponRule(CouponType.Recommend);
            foreach (var item in result)
            {
                if ((item.BeginDate <= beginDate && beginDate <= item.EndDate) || (item.BeginDate <= endDate && endDate <= item.EndDate))
                {
                    throw new BussinessException(ModelType.Discount, 11);
                }
            }
        }
        /// <summary>
        /// 描述：待提交审核数据表转介绍时间段验证
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-29</para>
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>无</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：12, 异常描述:此时间段内已存在待提交审核的转介绍的优惠设置
        /// </exception>
        private void ValidationAuditCouponRule(DateTime beginDate, DateTime endDate)
        {
            var result = _tblAutCouponRuleRepository.GetAuditingCouponRule(base.TblAutAudit.AuditId).Where(x => x.CouponType == (int)CouponType.Recommend);

            if (result.Any(item => (item.BeginDate <= beginDate && beginDate <= item.EndDate) || (item.BeginDate <= endDate && endDate <= item.EndDate)))
            {
                throw new BussinessException(ModelType.Discount, 12);
            }
        }
        /// <summary>
        /// 描述：修改一个审核中的优惠
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="autCouponRuleId">赠与奖学金审核表Id</param>
        /// <param name="dto">要修改的赠与奖学金信息</param
        /// <returns>无</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：1, 异常描述:该奖学金信息不存在
        /// </exception>
        public void Modify(long autCouponRuleId, CouponRuleDetailRequest dto)
        {
            var auditModel = _tblAutCouponRuleRepository.Load(x => x.AutCouponRuleId == autCouponRuleId);
            if (auditModel == null)
            {
                throw new BussinessException(ModelType.Discount, 1);
            }
            var model = Mapper.Map(dto, auditModel);
            _tblAutCouponRuleRepository.Update(model);

        }
        /// <summary>
        /// 描述：删除一条优惠数据
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="autCouponRuleId">赠与奖学金审核表Id</param>
        /// <returns>无</returns>
        public static void Remove(long autCouponRuleId)
        {
            new TblAutCouponRuleRepository().Delete(x => x.AutCouponRuleId == autCouponRuleId);
        }

        /// <summary>
        /// 描述：审核终审通过
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <returns>无</returns>
        public override void ProcessAuditSuccess()
        {
            var couponRuleService = _tblAutCouponRuleRepository.GetAuditingCouponRule(base.TblAutAudit.AuditId);

            var tblAutCouponRule = couponRuleService.Select(x => new TblDctCouponRule
            {
                CouponRuleId = x.CouponRuleId,
                SchoolId = x.SchoolId,
                CouponRuleName = x.CouponRuleName,
                CouponType = x.CouponType,
                FullAmount = x.FullAmount,
                CouponAmount = x.CouponAmount,
                MaxQuota = x.MaxQuota,
                UseQuota = 0,
                BeginDate = x.BeginDate,
                EndDate = x.EndDate,
                Remark = x.Remark,
                CreateTime = DateTime.Now,
                IsDisabled = false
            }).ToList();
            //终审通过之后。
            CouponRuleService.OnAuditSuccess(tblAutCouponRule);
        }

        /// <summary>
        /// 描述：退回提交人（没有特殊业务要求，暂不实现该方法）
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        public override void ProcessAuditReturn(AuditCallbackRequest dto)
        {
        }

        /// <summary>
        /// 描述：提交审核
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="dto">提交的赠与奖学金审核信息</param>
        /// <returns>无</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：3, 异常描述:校区不能为空
        /// </exception>
        public void Audit(CouponRuleSubmitAuditRequest dto)
        {
            OrgService orgService = new OrgService();
            var schoolList = orgService.GetAllSchoolList().FirstOrDefault(x => x.SchoolId.Trim() == _schoolId.Trim());
            if (schoolList == null)
            {
                throw new BussinessException(ModelType.Discount, 3);
            }
            dto.ApplyTitle = schoolList.SchoolName;

            //1.向流程平台提交审核流程,并得到流程记录Id 
            var flowModel = new FlowInputDto
            {
                SystemCode = BusinessConfig.BussinessCode,
                BusinessCode = ((int)AuditBusinessType.ScholarshipGive).ToString(),
                ApplyId = CurrentUserId,
                ApplyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                ApplyCode = this.TblAutAudit.AuditId.ToString(),    //申请单号，没有置空
                ApplyTitle = dto.ApplyTitle,
                AuditUserId = dto.AuditorId,
                FlowConent = string.Empty,
                Describe = string.Empty,
                Number = 0,
                FlowID = this.TblAutAudit != null && this.TblAutAudit.AuditStatus == (int)AuditStatus.Return ? this.TblAutAudit.FlowNo : string.Empty
            };

            //审核状态
            var auditStatus = TblAutAudit?.AuditStatus ?? 0;  //等于this.TblAutAudit != null ? this.TblAutAudit.AuditStatus : 0
            var flowId = base.SubmitAuditFlow(flowModel, (AuditStatus)auditStatus);

            base.TblAutAudit.AuditUserId = dto.AuditorId;
            base.TblAutAudit.AuditUserName = dto.AuditorName;
            base.TblAutAudit.UpdateTime = DateTime.Now;
            base.TblAutAudit.FlowNo = flowId;
            base.TblAutAudit.AuditStatus = (int)AuditStatus.Auditing;
            base.TblAutAudit.CreateUserId = dto.CreateUserId;
            base.TblAutAudit.CreateUserName = dto.CreateUserName;

            base._tblAutAuditRepository.Update(base.TblAutAudit);
        }
    }
}
