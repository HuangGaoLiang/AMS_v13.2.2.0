/****************************************************************************\
所属系统:招生系统
所属模块:家校互联
创建时间:
作   者:
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/
using AMS.Core;
using Jerrisoft.Platform.Cache;
using Jerrisoft.Platform.Log;
using Jerrisoft.Platform.Public.Utils;
using Microsoft.Extensions.Caching.Memory;
using NS1.Core.Dto;
using NS1.Core.Enum;
using NS1.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Service.Hss
{
    /// <summary>
    /// 描述：家校互联短信发送服务
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-06</para>
    /// </summary>
    public class UserLoginSmsService : AMS.Core.BService
    {

        private readonly string _mobile;  //要发送的手机号码


        /// <summary>
        /// 实例化一个登陆短信发送服务
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        public UserLoginSmsService()
        {
        }


        /// <summary>
        /// 实例化一个登陆短信发送服务
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        public UserLoginSmsService(string mobile)
        {
            _mobile = mobile;
        }


        /// <summary>
        /// 发送短信验证码
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        public void Send()
        {
            HssLoginSms config = ClientConfigManager.HssConfig.HssLoginSms;  //"HSS-LOGIN"
            string smsCode = this.GetSmsCode();
            string smsMessage = string.Format(config.SmsContentFormat, smsCode, config.EffectiveTime);

            //启用新的线程发送短信
            Task.Run(() =>
            {
                try
                {
                    SmsServices smsService = new SmsServices();
                    Receiver receiver = new Receiver()
                    {
                        Id = this._mobile,
                        Type = ReceiverType.TelCode
                    };
                    smsService.SendVerifyCode(receiver, smsCode, smsMessage, config.SenderId, BussinessId, config.DepartId, TimeSpan.FromMinutes(config.EffectiveTime));
                }
                catch (Exception ex)
                {
                    LogWriter.Write(this,ex.Message, LoggerType.Error);
                }
            });
        }

        /// <summary>
        /// 校验短信验证码
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <param name="smsCode"></param>
        /// <returns></returns>
        public bool Validate(string smsCode)
        {
            Receiver receiver = new Receiver()
            {
                Id = this._mobile,
                Type = ReceiverType.TelCode
            };

            SmsServices smsService = new SmsServices();
            return smsService.IsVerifyCode(receiver, smsCode,BussinessId);
        }


        /// <summary>
        /// 获取要发送的短信验证码
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <returns>短信验证码</returns>
        private string GetSmsCode()
        {
            //短信验证码每次发送会在缓存中保存30分钟，30分钟内发送依然是此验证码。

            HssLoginSms config = ClientConfigManager.HssConfig.HssLoginSms;  //"HSS-LOGIN"
            string smsCode = string.Empty;
            string cacheKey = GetCacheKey();
            TimeSpan duration = new TimeSpan(); //缓存时间

            //获取缓存
            LoginSmsCodeData smsCodeData = CacheModular.Get<LoginSmsCodeData>(base.BussinessId, cacheKey);

            //找到缓存数据后转换成LoginSmsCodeData
            if (smsCodeData != null)
            {
                //验证码时间间隔，一般为60秒
                if (smsCodeData.LastSendDate.AddSeconds(config.IntervalPer) > DateTime.Now)
                {
                    throw new BussinessException(ModelType.Hss, 3);
                }
                smsCodeData.LastSendDate = DateTime.Now;
                //重新计算缓存时间
                duration = smsCodeData.CreateDate.AddMinutes(config.EffectiveTime) - DateTime.Now;
                smsCode = smsCodeData.Code;
            }
            else
            {
                smsCode= Utils.RandomNum(4);
                smsCodeData = new LoginSmsCodeData()
                {
                    CreateDate = DateTime.Now,
                    LastSendDate = DateTime.Now,
                    Code = smsCode,
                    Mobile = this._mobile,
                };
                //缓存验证码数据
                duration = new TimeSpan(0, config.EffectiveTime, 0);
            }
            //说明验证码缓存未失效
            if (duration.TotalSeconds > 0)
            {
                CacheModular.Set(base.BussinessId, cacheKey, smsCodeData, duration);
            }
            return smsCode;
        }


        /// <summary>
        /// 获取验证码缓存的KEY
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <returns></returns>
        private string GetCacheKey()
        {
            return $"{BusinessCacheKey.HSS_SMS_KEY}:{this._mobile}";
        }
    }


    /// <summary>
    /// 报名短信验证码数据
    /// </summary>
    class LoginSmsCodeData
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 最后发送时间
        /// </summary>
        public DateTime LastSendDate { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
    }
}
