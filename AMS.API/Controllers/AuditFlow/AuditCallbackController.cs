using AMS.API.Controllers;
using AMS.Dto;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace JAMS.API.Controllers.External
{
    /// <summary>
    /// 审核回调(用了平台组SDK，此外部接口仅用于测试功能)
    /// </summary>
    [Route("api/AMS/AuditCallback")]
    [ApiController]
    public class AuditCallbackController : BaseController
    {
        /// <summary>
        /// 数据回调处理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void Post(AuditCallbackRequest dto)
        {
            //业务类型
            var auditService = AuditFlowFactory.Instance.CreateAuditService(dto);
            auditService.AuditComplete(dto);
        }
    }

}
