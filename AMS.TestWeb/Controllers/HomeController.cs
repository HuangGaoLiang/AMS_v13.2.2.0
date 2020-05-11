using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Jerrisoft.Platform.TestWeb.Models;
using AMS.API.Controllers;
using AMS.Core;
using AMS.Service;
using Jerrisoft.Platform.Exception;
using AMS.Anticorrosion.HRS;
using Jerrisoft.Platform.IdentityClient.Filters;
using AMS.Storage.Repository;
using AMS.SDK;
using AMS.Storage.Models;
using System;
using AMS.Storage;

namespace Jerrisoft.Platform.TestWeb.Controllers
{
    public class HomeController : BaseController
    {
        public override byte BussinessId { get; } = BusinessConfig.BussinessID;


        public IActionResult Index()
        {
           
            //var bll = new TestService();
            //var a= ServicesManage.GetCourseService().GetAllCourse();
            return NoContent();
        }


        [HttpPost]
        public IActionResult Post()
        {
            //返回非列表数据

            return Ok();//new ObjectResult(new TeacherService().GetTeachers());
        }

        [HttpGet("{id}-{name}")]
        public IActionResult Get(int id, string name)
        {
            return Ok();//new ObjectResult(new TeacherService().GetTeachers());
            //无数据返回
            //return new EmptyResult();
        }

        [HttpGet("{id}-{name}/{age}")]
        public void Get(int id, string name, int age)
        {
            //查看日志文件，会记录请求参数和详细错误信息。
            throw new PlatformException(7, 20, $"抛出一个异常。{id}不能为空！");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
