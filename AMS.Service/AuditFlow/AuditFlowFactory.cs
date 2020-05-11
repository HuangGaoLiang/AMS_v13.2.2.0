using AMS.Core;
using AMS.Dto;
using FP3.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Service
{
    public class AuditFlowFactory
    {

        public static AuditFlowFactory Instance = new AuditFlowFactory();

        /// <summary>
        /// 根据类型类型，调取对应的服务
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public BaseAuditService CreateAuditService(AuditCallbackRequest dto)
        {
            var bussinessType = int.Parse(dto.BussinessCode);
            BaseAuditService service = null;
            long auditId = long.Parse(dto.ApplyNumber);
            switch (bussinessType)
            {
                case (int)AuditBusinessType.Term:   //学期审核服务
                    service = TermAuditService.CreateByAutitId(auditId);
                    break;
                case (int)AuditBusinessType.TermCourseTimetable:   //排课审核服务
                    service = TermCourseTimetableAuditService.CreateByAutitId(auditId);
                    break;
                case (int)AuditBusinessType.ScholarshipGive:
                    service = CouponRuleAuditService.CreateByAutitId(auditId);
                    break;
                default:
                    break;
            }
            if (service == null)  //审核单据类型不存在
            {
                throw new BussinessException((byte)ModelType.Audit, 13);
            }

            if (service.TblAutAudit.BizType != bussinessType)  //审核单据类型不一致
            {
                throw new BussinessException((byte)ModelType.Audit, 14);
            }

            return service;
        }
    }
}
