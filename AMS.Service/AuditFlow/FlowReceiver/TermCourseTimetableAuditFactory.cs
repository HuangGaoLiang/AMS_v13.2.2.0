using System;
using System.Collections.Generic;
using System.Text;
using AMS.Core;
using AMS.Dto;
using FP3.Logic;
using Jerrisoft.Platform.Log;

namespace AMS.Service.AuditFlow
{
    /// <summary>
    /// 描述：排课表审核对象
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-9-19</para>
    /// </summary>
    public class TermCourseTimetableAuditFactory : FP3.Logic.FlowReceiver
    {        
        public override string SystemCode => BusinessConfig.BussinessCode;                                 //项目代号
        public override string BussinessCode => ((int)AuditBusinessType.TermCourseTimetable).ToString();   //业务类型

        /// <summary>
        /// 描述：审批完成，向排课表写入回调数据
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-9-19</para>
        /// </summary>
        /// <param name="e">审流程平台审批的回调事件</param>
        /// <returns>无</returns>
        public override void Deal(FlowCallbackEventArgs e)
        {
            LogWriter.Write(this, "课表审核审核通过调试接口");
            long auditId = long.Parse(e.ApplyNumber);
            LogWriter.Write(this,"审核Id"+ auditId);
            TermCourseTimetableAuditService service = TermCourseTimetableAuditService.CreateByAutitId(auditId);
            service.AuditComplete(new Dto.AuditCallbackRequest
            {
                ApplyNumber = e.ApplyNumber,
                AuditTime = e.AuditTime,
                AuditUserId = e.AuditUserId,
                AuditUserName = e.AuditUserName,
                BussinessCode = e.BussinessCode,
                Descption = e.Descption,
                Status = (AuditStatus)e.Status,
                WFInstanceId = e.WFInstanceId,
                Remark=e.Remark
            });
        }
    }
}
