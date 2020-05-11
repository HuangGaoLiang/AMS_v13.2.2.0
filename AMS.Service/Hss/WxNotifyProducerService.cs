/****************************************************************************\
所属系统:招生系统
所属模块:家校互联-微信通知
创建时间：2019-03-13
作    者：蔡亚康
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/
using AMS.Core;
using AMS.Dto;
using Jerrisoft.Platform.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Service.Hss
{
    /// <summary>
    /// 微信通知消息发送到消息队列
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-13</para>
    /// </summary>
    public class WxNotifyProducerService : BaseProducer
    {
        private static readonly string TOPIC_ID = ClientConfigManager.AppsettingsConfig.BusinessQueueName.NameOfWxNotify;

        /// <summary>
        /// 使用单例
        /// </summary>
        public static readonly WxNotifyProducerService Instance = new WxNotifyProducerService();

        /// <summary>
        /// 实例化一个微信通知队列发送实例
        /// </summary>
        private WxNotifyProducerService() : base(TOPIC_ID)
        {

        }

        /// <summary>
        /// 发送一个消息推送
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        /// <param name="data">一个消息</param>
        public void Publish(WxNotifyInDto data)
        {
            this.Publish(new List<WxNotifyInDto>() { data });
        }

        /// <summary>
        /// 发送一组消息推送
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        /// <param name="datas">一组消息</param>
        public void Publish(List<WxNotifyInDto> datas)
        {
            Task.Run(()=> 
            {
                try
                {
                    datas = datas.Where(t => t.ToUser.Count()>0).ToList();
                    foreach (WxNotifyInDto data in datas)
                    {
                        data.BusinessId = BusinessConfig.BussinessID;
                    }
                    var config = ClientConfigManager.AppsettingsConfig;
                    if (datas.Count > 0)
                    {
                        base.SendMessage(datas);
                    }
                }
                catch (Exception ex)
                {
                    LogWriter.Write(this, "发送一组消息推送发生错误:" + ex.Message, LoggerType.Error);
                }
            });
        }
    }
}
