using AMS.Core;
using AMS.Dto;
using AMS.Service;
using Jerrisoft.Platform.Public.PageExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 学生信息资源
    /// </summary>
    [Produces("application/json"), Route("api/AMS/MyTest")]
    [ApiController]
    public class MyTestController : Controller
    {
        /// <summary>
        /// 测试导出学生信息
        /// </summary>
        /// <param name="request"></param>
        [HttpPost]
        public IActionResult Post(StudentRegisterRequest request)
        {
            List<StudentSearchResponse> list = StudentService.GetListBykeyWord("");
            string[] header = new string[] { "ID", "学生姓名", "年龄", "性别ID", "性别", "出生年月", "手机号", "校区名称" };
            NPOIExcelExport excelExport = new NPOIExcelExport();
            excelExport.Add<StudentSearchResponse>(list, header);
            return excelExport.DownloadToFile(this, "学生资料");
        }
    }
}
