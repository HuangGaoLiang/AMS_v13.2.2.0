using AMS.Core;
using AMS.Dto;
using AMS.Service;
using AMS.Service.Hss;
using AMS.Storage.Context;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMS.API.Controllers
{
    [Produces("application/json"), Route("api/AMS/Publish")]
    [ApiController]
    public class PublishController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="capPublisher"></param>
        public PublishController()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public int[] Get(int type)
        {
            int[] arr1 = new int[] { 1, 2, 3, 4, 5 };

            int[] arr2 = new int[] { 1, 3, 4, 5, 6, 7,8 };
            if (type == 1)
            {
                return arr1.Except(arr2).ToArray();
            }
            else
            {
                return arr2.Except(arr1).ToArray();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        [HttpPost]
        public void Post()
        {
            /* 14000000000000001 投诉建议受理通知
             * 14000000000000002 请假成功通知
             * 14000000000000003 已到校通知
             * 14000000000000004 补课通知
             * 14000000000000005 补签通知
             * 14000000000000006 老师课程调整通知
             * 14000000000000007 上课时间课程调整通知
             * 
             * 队列名称 微信通知:        hss.wxnotify.queue
                        家长手机号变更:  hss.passport.queue
             */
            TestAttend();
        }

        private void TestFeedBack()
        {
            WxNotifyProducerService publisher = WxNotifyProducerService.Instance;
            PassportService passportService = new PassportService();
            WxNotifyInDto wxNotify = new WxNotifyInDto();
            wxNotify.Data = new List<WxNotifyItemInDto>()
            {
                new WxNotifyItemInDto(){ DataKey="first",Color="#173177", Value="您好！我们会尽快处理您提交的信息。 感谢您的支持！" },
                new WxNotifyItemInDto(){ DataKey="keyword1",Color="#173177", Value="总部客服" },
                new WxNotifyItemInDto(){ DataKey="keyword2",Color="#173177", Value="2018.02.01 15:23" },
                new WxNotifyItemInDto(){ DataKey="remark",Color="#173177", Value="杨梅红国际私立美校深圳校区" }
            };
            wxNotify.ToUser = passportService.GetByUserCodes(new List<string> { "18138878080" }).Where(t => !string.IsNullOrEmpty(t.UnionId)).Select(t => t.UnionId).ToList();
            wxNotify.TemplateId = 14000000000000001;
            wxNotify.Url = "www.ymm.cn";
            publisher.Publish(wxNotify);
        }

        private void TestAttend()
        {
            WxNotifyProducerService publisher = WxNotifyProducerService.Instance;
            PassportService passportService = new PassportService();
            WxNotifyInDto wxNotify = new WxNotifyInDto();
            wxNotify.Data = new List<WxNotifyItemInDto>()
            {
                    new WxNotifyItemInDto{ DataKey="first", Value="杨宏家长，您的孩子已经到校上课。(感谢您对孩子坚持学术艺术的鼓励)" },
                    new WxNotifyItemInDto{ DataKey="keyword1", Value=$"2018.02.28 14:29"},
                    new WxNotifyItemInDto{ DataKey="keyword2", Value="米罗小小班" },
                    new WxNotifyItemInDto{ DataKey="keyword3", Value="302" },
                    new WxNotifyItemInDto{ DataKey="keyword4", Value="2018-02-28 14:30-15:30"},
                    new WxNotifyItemInDto{ DataKey="remark", Value="杨梅红深圳市东海校区 [以上信息如有误，请点击留言]" }
            };
            wxNotify.ToUser = passportService.GetByUserCodes(new List<string> { "18138878080" }).Where(t => !string.IsNullOrEmpty(t.UnionId)).Select(t => t.UnionId).ToList();
            wxNotify.TemplateId = 14000000000000003;
            wxNotify.Url = "";
            publisher.Publish(wxNotify);
        }
    }
}
