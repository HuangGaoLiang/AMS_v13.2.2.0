using AMS.Anticorrosion.HRS;
using AMS.Core;
using AMS.Dto;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using AutoMapper;
using FP3.Core.ServiceInput;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AMS.Service
{
    /// <summary>
    /// 描述：代表一个校区某个年度收费标准审核
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018.9.19</para>
    /// </summary>
    public class TermAuditService : BaseAuditService
    {
        private readonly TblAutTermRepository _tblAutTermRepository = new TblAutTermRepository();  //学期表审核中数据仓储
        private readonly string _schoolId;                                                         //校区Id
        private readonly int _year;                                                                //年度

        /// <summary>
        /// 描述：实例化最近一个校区的年度学期审核
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.19</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="year">年度</param>
        public TermAuditService(string schoolId, int year)
        {
            this._schoolId = schoolId;
            this._year = year;
            base.TblAutAudit = base._tblAutAuditRepository.GetTblAutAuditByExtField(AuditBusinessType.Term,this._schoolId, this._year.ToString());
        }

        /// <summary>
        /// 描述：根据审核主表实例化学期审核数据
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.19</para>
        /// </summary>
        /// <param name="tblAutAudit">审核主表实例</param>
        private TermAuditService(TblAutAudit tblAutAudit)
        {
            base.TblAutAudit = tblAutAudit;
            this._schoolId = base.TblAutAudit.ExtField1;
            this._year = int.Parse(base.TblAutAudit.ExtField2);
        }

        /// <summary>
        /// 描述：实例化指定学期审核记录的数据
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.19</para>
        /// </summary>
        /// <param name="auditId">审核主表Id</param>
        /// <returns>学期审核表的实例</returns>
        public static TermAuditService CreateByAutitId(long auditId)
        {
            TblAutAuditRepository repository = new TblAutAuditRepository();
            var entity = repository.Load(auditId);
            return new TermAuditService(entity);
        }

        /// <summary>
        /// 描述：删除指定审核中的学期
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.19</para>
        /// </summary>
        /// <param name="termId">学期审核表Id</param>
        /// <returns>无</returns>
        /// <exception cref="AMS.Core.BussinessException">无</exception>
        public void RemoveTerm(long termId)
        {
            if (!base.CanSubmitToAudit) return;
            //已生成审核单据才可删除
            if (this.TblAutAudit != null)
            {
                _tblAutTermRepository.DeleteByAutTermId(this.TblAutAudit.AuditId, termId);
            }
        }
        
        /// <summary>
        /// 描述：审核单据中的学期列表
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.19</para>
        /// </summary>
        /// <returns>学期审核表集合</returns>
        /// <exception cref="AMS.Core.BussinessException">无</exception>
        public List<TblAutTerm> TermList
        {
            get
            {
                return _tblAutTermRepository.GetTblAutTermList(this.TblAutAudit.AuditId);
            }
        }

        /// <summary>
        /// 描述：提交审核
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.19</para>
        /// </summary>
        /// <returns>无</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：9, 异常描述:校区编号为空
        /// 异常ID：2, 异常描述:添加审核表数据失败
        /// </exception>
        public void SubmitAudit(TermAuditRequest data)
        {
            //1.向流程平台提交审核流程,并得到流程记录Id
            var flowAuditId = IdGenerator.NextId();

            var orgService = new OrgService();
            var schoolList = orgService.GetAllSchoolList().FirstOrDefault(x => x.SchoolId.Trim() == _schoolId.Trim());
            if (schoolList == null)
            {
                throw new BussinessException(ModelType.Datum, 9);
            }

            var applyTitle = $"{schoolList.SchoolName}{_year}";

            var flowModel = new FlowInputDto
            {
                SystemCode = BusinessConfig.BussinessCode,
                BusinessCode = ((int)AuditBusinessType.Term).ToString(),
                ApplyId = CurrentUserId,
                ApplyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                ApplyCode = flowAuditId.ToString(),    //申请单号，没有置空
                ApplyTitle = applyTitle,
                AuditUserId = data.AuditUserId,
                FlowConent = "",
                Describe = "",
                Number = 0,
                FlowID = this.TblAutAudit != null && this.TblAutAudit.AuditStatus == (int)AuditStatus.Return ? this.TblAutAudit.FlowNo : string.Empty
            };

            //审核状态
            var auditStatus = TblAutAudit?.AuditStatus ?? 0;  //等于this.TblAutAudit != null ? this.TblAutAudit.AuditStatus : 0
            var flowId = base.SubmitAuditFlow(flowModel, (AuditStatus)auditStatus);

            //2.添加数据到审核表
            var auditModel = new TblAutAudit()
            {
                AuditId = flowAuditId,
                SchoolId=this._schoolId,
                BizType = (int)AuditBusinessType.Term,
                ExtField1 = this._schoolId,
                ExtField2 = this._year.ToString(),
                FlowNo = flowId,
                AuditStatus = (int)AuditStatus.Auditing,
                AuditUserId = data.AuditUserId,
                AuditUserName = data.AuditUserName,
                AuditDate = DateTime.Now,
                CreateUserId=base.CurrentUserId,
                CreateUserName=data.CreateUserName,
                DataExt = string.Empty,
                DataExtVersion = string.Empty,
                CreateTime=DateTime.Now,
                UpdateTime=DateTime.Now
            };
            var flag = _tblAutAuditRepository.Add(auditModel);
            if (!flag) throw new BussinessException((byte)ModelType.Audit, 2);

            //3.添加数据到学期表
            if (this.TblAutAudit != null && this.TblAutAudit.AuditStatus == (int)AuditStatus.Return)
            {
                //退回时，先删除审核表中的记录，再插入
                _tblAutTermRepository.DeleteByAutTermId(_schoolId, _year);
            }
            var entity = Mapper.Map<List<TblAutTerm>>(data.TermAuditDetail);
            foreach (var item in entity)
            {
                item.AutTermId = IdGenerator.NextId();
                item.AuditId = flowAuditId;
                item.SchoolId = this._schoolId;
                item.Year = this._year;
                item.CreateTime = DateTime.Now;

                if (item.TermId == 0)//学期Id为空，则产生新的学期Id
                {
                    item.TermId = IdGenerator.NextId();
                }
            }
            _tblAutTermRepository.Add(entity);
        }

        /// <summary>
        /// 描述：审核终审通过
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.19</para>
        /// </summary>
        /// <returns>无</returns>
        public override void ProcessAuditSuccess()
        {
            TermService termService = new TermService(this._schoolId, this._year);
            var query = _tblAutTermRepository.LoadList(x => x.AuditId == this.TblAutAudit.AuditId);

            //1先删除原来的数据
            termService.Remove();

            //2增加审核后的数据
            var datTermModel = query.Select(x => new TblDatTerm
            {
                TermId = x.TermId,
                TermName = x.TermName,
                TermTypeId = x.TermTypeId,
                SchoolId = x.SchoolId,
                Year = x.Year,
                BeginDate = x.BeginDate,
                EndDate = x.EndDate,
                Classes60 = x.Classes60,
                Classes90 = x.Classes90,
                Classes180 = x.Classes180,
                TuitionFee = x.TuitionFee,
                MaterialFee = x.MaterialFee,
                CreateTime = x.CreateTime,
                UpdateTime = DateTime.Now
            }).ToList();

            termService.ResetTermList(datTermModel);

            //3同步报名时的学习计划数据
            StudyPlanService.AsyncSchoolTermType(this._schoolId).Wait();
        }

        /// <summary>
        /// 描述：退回提交人 （没有特殊业务要求，暂不实现该方法）
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.19</para>
        /// </summary>
        public override void ProcessAuditReturn(AuditCallbackRequest dto)
        {
        }

    }
}
