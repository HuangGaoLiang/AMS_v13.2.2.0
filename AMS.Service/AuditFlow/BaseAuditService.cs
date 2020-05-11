using AMS.Core;
using AMS.Dto;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using FP3.Core.ServiceInput;
using System;


namespace AMS.Service
{
    /// <summary>
    /// 描述：审核流程基类
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018.09.21</para>
    /// </summary>
    public abstract class BaseAuditService : BService
    {
        protected readonly TblAutAuditRepository _tblAutAuditRepository = new TblAutAuditRepository();  //审核仓储

        /// <summary>
        /// 描述：实例化一个审核流程基类对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.09.21</para>
        /// </summary>
        protected BaseAuditService()
        {
        }


        /// <summary>
        /// 描述：当前学期最新审核信息
        /// <para>作  者：瞿琦</para>
        /// <para>创建时间：2018.09.21</para>
        /// </summary>
        internal TblAutAudit TblAutAudit { get; set; }

        /// <summary>
        /// 是否审核中
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-21</para>
        /// </summary>
        public bool IsAuditing
        {
            get
            {
                if (this.TblAutAudit == null)
                {
                    return false;
                }
                return this.TblAutAudit.AuditStatus == (int)AuditStatus.Auditing;
            }
        }

        /// <summary>
        /// 描述：是否可提交审核
        ///       当单据处理待提交或退回时，可提交审核
        /// <para>作    者：瞿琦</para>      
        /// <para>创建时间：2018.09.21</para>
        /// </summary>
        public bool CanSubmitToAudit
        {
            get
            {
                if (this.TblAutAudit == null)
                {
                    return false;
                }
                return this.TblAutAudit.AuditStatus == (int)AuditStatus.WaitAudit || this.TblAutAudit.AuditStatus == (int)AuditStatus.Return;
            }
        }

        /// <summary>
        /// 描述：获取当前单据的最新审核状态
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.09.21</para>
        /// </summary>
        public int CurrentAuditStatus => TblAutAudit?.AuditStatus ?? (int)AuditStatus.Success;

        /// <summary>
        /// 描述：校验回归不允许重复
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.09.21</para>
        /// </summary>
        /// <returns>无</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：11,异常描述:单据已处理，请勿重复提交
        /// </exception>
        private void ValidateAuditComplete()
        {
            if (this.TblAutAudit.AuditStatus != (int)AuditStatus.Auditing && this.TblAutAudit.AuditStatus != (int)AuditStatus.Forwarding)
            {
                throw new BussinessException((byte)ModelType.Audit, 11);
            }
        }


        /// <summary>
        /// 描述：保存审批信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.09.21</para>
        /// </summary>
        /// <param name="dto">审核信息</param>
        /// <returns>无</returns>
        private void SaveAuditComplete(AuditCallbackRequest dto)
        {
            this.TblAutAudit.AuditStatus = (int)dto.Status;
            this.TblAutAudit.AuditUserId = dto.AuditUserId;
            this.TblAutAudit.AuditUserName = dto.AuditUserName;
            this.TblAutAudit.AuditDate = dto.AuditTime;
            this.TblAutAudit.UpdateTime = DateTime.Now;
            this.TblAutAudit.DataExt = dto.Remark;
            this._tblAutAuditRepository.Update(this.TblAutAudit);
        }


        /// <summary>
        /// 描述：审核终审通过
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.09.21</para>
        /// </summary>
        public abstract void ProcessAuditSuccess();
        /// <summary>
        /// 描述：退回提交人
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.09.21</para>
        /// </summary>
        public abstract void ProcessAuditReturn(AuditCallbackRequest dto);

        /// <summary>
        ///描述： 审核完成调取方法，写入数据到正式表
        ///<para>作    者：瞿琦</para>
        ///<para>创建时间：2018.09.21</para>
        /// </summary>
        /// <param name="dto">审核信息</param>
        /// <returns>无</returns>
        public virtual void AuditComplete(AuditCallbackRequest dto)
        {
            //1.验证是否已经提交
            this.ValidateAuditComplete();
            //2.保存审核信息
            this.SaveAuditComplete(dto);
            if (dto.Status == AuditStatus.Success)
            {
                ProcessAuditSuccess();
            }
            else if (dto.Status == AuditStatus.Return)
            {
                ProcessAuditReturn(dto);
            }
        }

        /// <summary>
        /// 描述：向流程平台提交审核流程
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.19</para>
        /// </summary>
        /// <param name="dto">提交的流程信息</param>
        /// <param name="auditStatus">审核状态</param>
        /// <returns></returns>
        public string SubmitAuditFlow(FlowInputDto dto, AuditStatus? auditStatus)
        {
            //向流程平台写入数据
            string flowId;
            FP3.SDK.FlowServices flowServices = new FP3.SDK.FlowServices();
            if (auditStatus != null && auditStatus == AuditStatus.Return) //重新提交
            {
                flowId = flowServices.OldAddTFlowInfor(dto);
            }
            else
            {
                flowId = flowServices.AddTFlowInfor(dto);  //第一次提交
            }
            return flowId;
        }

        /// <summary>
        /// 描述：当前登录人是否是流程提交人
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.19</para>
        /// </summary>
        /// <returns>是否是当前流程提交人</returns>
        public bool IsFlowSubmitUser
        {
            get
            {
                if (this.TblAutAudit == null || string.IsNullOrWhiteSpace(this.TblAutAudit.CreateUserId))
                {
                    return true;
                }
                return this.TblAutAudit.CreateUserId.Trim() == base.CurrentUserId.Trim();
            }

        }
    }
}
