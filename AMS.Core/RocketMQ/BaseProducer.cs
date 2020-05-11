/****************************************************************************\
所属系统:招生系统
所属模块:公共库
创建时间:
作   者:
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/
using Aliyun.MQ;
using Jerrisoft.Platform.Log;
using Newtonsoft.Json;
using ons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AMS.Core
{
    /// <summary>
    /// 描    述：ROCKMQ 生成者基类
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-14</para>
    /// </summary>
    public abstract class BaseProducer
    {

        private Producer _producer;                             //生产者
        private static readonly string Ons_AccessKey = string.Empty;      //AccessKey 阿里云身份验证，在阿里云服务器管理控制台创建
        private static readonly string Ons_SecretKey = string.Empty;  //SecretKey 阿里云身份验证，在阿里云服务器管理控制台创建
        private static string Ons_NameSrv = string.Empty;//接入地址
        private static readonly string Ons_GroupId = string.Empty; // 您在控制台创建的 Consumer ID(Group ID)

        private static object s_SyncLock;
        private readonly string _topicId = string.Empty;  //所属的 Topic

        /// <summary>
        /// 初始化静态配置
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        static BaseProducer()
        {
            s_SyncLock = new object();
            var config = ClientConfigManager.AppsettingsConfig.RocketMQConfig;
            Ons_AccessKey = config.AccessKey;
            Ons_SecretKey = config.SecretKey;
            Ons_GroupId = config.GroupId;
            Ons_NameSrv = config.NameSrv;
        }

        /// <summary>
        /// 根据Topic实例化一个生产者
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        /// <param name="topicId"></param>
        protected BaseProducer(string topicId)
        {
            try
            {
                this._topicId = topicId;
                CreateProducer();
                StartProducer();
            }
            catch (Exception ex)
            {
                LogWriter.Write(this, "RocketMQ初始化失败" + ex.Message, LoggerType.Error);
                throw;
            }
        }


        /// <summary>
        /// 获取日志的路径
        /// </summary>
        /// <returns></returns>
        private string GetLogPath()
        {
            String basePath1 = AppContext.BaseDirectory;
            String basePath2 = Path.GetDirectoryName(this.GetType().Assembly.Location);
            String path = Path.Combine(basePath2, "RocketMQ_Logs");
            return path;
        }


        /// <summary>
        /// 获取配置
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        /// <returns></returns>
        private ONSFactoryProperty GetFactoryProperty()
        {
            ONSFactoryProperty factoryInfo = new ONSFactoryProperty();
            factoryInfo.setFactoryProperty(ONSFactoryProperty.AccessKey, Ons_AccessKey);
            factoryInfo.setFactoryProperty(ONSFactoryProperty.SecretKey, Ons_SecretKey);
            //factoryInfo.setFactoryProperty(ONSFactoryProperty.ConsumerId, Ons_GroupId);
            factoryInfo.setFactoryProperty(ONSFactoryProperty.ProducerId, Ons_GroupId);
            factoryInfo.setFactoryProperty(ONSFactoryProperty.PublishTopics, _topicId);
            factoryInfo.setFactoryProperty(ONSFactoryProperty.NAMESRV_ADDR, Ons_NameSrv);
            factoryInfo.setFactoryProperty(ONSFactoryProperty.LogPath, GetLogPath());
            return factoryInfo;
        }

        /// <summary>
        /// 创建生产者
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        private void CreateProducer()
        {

            _producer = ONSFactory.getInstance().createProducer(GetFactoryProperty());
        }

        /// <summary>
        /// 启动生产者
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        private void StartProducer()
        {
            _producer.start();
        }


        /// <summary>
        /// 关闭生产者
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        private void ShutdownProducer()
        {
            _producer.shutdown();
        }


        /// <summary>
        /// 发送一个消息
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="msgBody">消息数据</param>
        /// <param name="tag">消息标签</param>
        public void SendMessage<T>(T msgBody, String tag = "")
        {
            string message = JsonConvert.SerializeObject(msgBody);
            this.SendMessage(message, tag);
        }

        /// <summary>
        /// 发送一个消息
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        /// <param name="msgBody">消息数据</param>
        /// <param name="tag"></param>
        public void SendMessage(string msgBody, String tag = "")
        {
            msgBody = Base64Helper.Base64Encode(msgBody);
            Message msg = new Message(_topicId, tag, msgBody);
            msg.setKey(Guid.NewGuid().ToString());
            try
            {
                SendResultONS sendResult = _producer.send(msg);
            }
            catch (Exception ex)
            {
                //Console.WriteLine("send failure{0}", ex.ToString());
                LogWriter.Write("BaseProducer_SendMessage", ex.ToString(), LoggerType.Error);
            }
        }

    }
}
