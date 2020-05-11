/****************************************************************************\
所属系统:招生系统
所属模块:客户模块-队列发送
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
using AMS.Storage.Models;
using AMS.Storage.Repository;
using Jerrisoft.Platform.Log;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Service
{
    /// <summary>
    /// 学生家长手机号更改队列生产者服务
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-13</para>
    /// </summary>
    public class StudentFamilyProducerService : BaseProducer
    {

        private static readonly string TOPIC_ID = ClientConfigManager.AppsettingsConfig.BusinessQueueName.NameOfHssPassport;


        /// <summary>
        /// 单例
        /// </summary>
        public static readonly StudentFamilyProducerService Instance = new StudentFamilyProducerService();

        /// <summary>
        /// 实例化一个队列发送实例
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        public StudentFamilyProducerService() : base(TOPIC_ID)
        {
        }


        /// <summary>
        /// 发送一组消息推送
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        /// <param name="data">一组消息</param>
        public void Publish(StudentPassportChangeInDto data)
        {
            //当前采用线程池的线程处理!
            //后续家校互联独立后，才会考虑使用消息队列!
            Task.Run(() =>
            {
                TblHssPassportRepository repository = new TblHssPassportRepository();
                if (data.MobileAddList != null)
                {
                    foreach (string mobile in data.MobileAddList)
                    {
                        this.AddMobile(mobile, repository);
                    }
                }
                if (data.MobileDeleteList != null)
                {
                    foreach (string mobile in data.MobileDeleteList)
                    {
                        repository.DeleteByUserCode(mobile);
                    }
                }
            });
            //base.SendMessage(data);
        }


        private void AddMobile(string mobile, TblHssPassportRepository repository)
        {
            try
            {
                TblHssPassport oldEntity=repository.GetByUserCode(mobile);
                if (oldEntity == null)
                {
                    TblHssPassport entity = new TblHssPassport()
                    {
                        CreateTime = DateTime.Now,
                        CurrentLoginIp = "",
                        LastLoginIp = "",
                        LoginTimes = 0,
                        OpenId = "",
                        PassporId = IdGenerator.NextId(),
                        UnionId = "",
                        UserCode = mobile
                    };
                    repository.Add(entity);
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write(this,"创建家校互联登陆账号出错:"+ex.Message,LoggerType.Error);
            }
        }
    }
}
