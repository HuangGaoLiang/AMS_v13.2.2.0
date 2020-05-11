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
using AMS.Anticorrosion.WX;
using AMS.Core;
using AMS.Dto;
using AMS.Dto.Hss;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using Jerrisoft.Platform.Log;
using Jerrisoft.Platform.Public.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    /// 描述：家校互联家长认证服务
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-06</para>
    /// </summary>
    public class AuthenicationService : AMS.Core.BService
    {

        private Lazy<TblHssPassportRepository> _repository = new Lazy<TblHssPassportRepository>();


        #region SendSignInCode 发送短信
        /// <summary>
        /// 发送短信
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        public void SendSignInCode(string mobile)
        {
            TblHssPassport passport = _repository.Value.GetByUserCode(mobile);
            //1、验证账户是否存在
            this.ValidateUserExist(passport);

            //2、发送短信
            UserLoginSmsService smsService = new UserLoginSmsService(mobile);
            smsService.Send();
        }
        #endregion

        #region SignIn 家校登陆
        /// <summary>
        /// 描述：家校登陆
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <param name="request">用户登陆的手机号和验证码等信息</param>
        /// <exception>
        /// 异常ID：2->手机号码在系统不存在
        /// 异常ID：1->验证码有误
        /// 异常ID：6->微信号已被其他手机号绑定
        /// 异常ID：7->用户信息数据解密失败
        /// </exception>
        /// <returns>登陆结果</returns>
        public StudentLoginResponse SignIn(StudentLoginRequest request)
        {

            TblHssPassport passport = _repository.Value.GetByUserCode(request.Mobile);
            StudentLoginResponse result = new StudentLoginResponse();

            //1、验证账户是否存在
            this.ValidateUserExist(passport);

            //2、短信验证码校验
            this.ValidateSmsCode(request.Mobile, request.SmsCode);

            //3、获取openid
            Code2SessionService wxService = new Code2SessionService();
            OpenIdResponse openid=wxService.GetOpenId(request.WxCode);


            //4、检查openid是否已经被其他手机号绑定
            TblHssPassport passport2 = _repository.Value.GetByOpenId(openid.OpenId);
            if (passport2!=null && passport2.OpenId == openid.OpenId && passport2.UserCode != request.Mobile)
            {
                throw new BussinessException(ModelType.Hss, 6);
            }

            //5、用户数据解密
            try
            {
                string data = AESHelper.AESDecrypt(request.EncryptedData, openid.Session_Key, request.Iv);
                JObject wxUserInfo = (JObject)JsonConvert.DeserializeObject(data);

                //检查返回值是否包含unionID,防止出现异常。
                JToken jtoke = null;
                if (wxUserInfo.TryGetValue("unionId", out jtoke))
                {
                    passport.UnionId = jtoke.ToString();
                }
                else
                {
                    LogWriter.Write(this, "解密数据没有unionID,原数据如下:" + GetDecryptData(request, openid), LoggerType.Warn);
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write(this, "小程序解密失败,原数据如下:"+ GetDecryptData(request,openid), LoggerType.Error);
                throw new BussinessException(ModelType.Hss,7);
            }
            //获取到的unionID为空,有可能是用户未允许访问授权
            if (string.IsNullOrEmpty(passport.UnionId))
            {
                throw new BussinessException(ModelType.Hss,8);
            }

            //6、绑定openid 并更新最新登陆信息,包括最新的openid
            passport.OpenId = openid.OpenId;//openid.OpenId;   //如果在另外一个微信上登陆将会被新的替换
            passport.LastLoginIp = passport.CurrentLoginIp;
            passport.LastLoginDate = passport.CurrentLoginDate;
            passport.CurrentLoginIp = request.IpAddress;
            passport.CurrentLoginDate = DateTime.Now;
            passport.LoginTimes = passport.LoginTimes + 1;
            _repository.Value.Update(passport);

            //6、记录登陆日记
            AddOperationLog(passport);

            //7、返回登陆结果
            JwtTokenService tokenService = new JwtTokenService();
            result.Token = tokenService.CreateToken(passport);

            return result;
        }
        #endregion

        /// <summary>
        /// 把解密的数据转成字符串，方便出现异常可能通过此数据进行调试
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-18</para>
        /// </summary>
        /// <param name="request">登陆请求数据</param>
        /// <param name="openid">用户的OPENID信息</param>
        /// <returns></returns>
        private string GetDecryptData(StudentLoginRequest request, OpenIdResponse openid)
        {
            StringBuilder sbMsg = new StringBuilder();
            sbMsg.Append(JsonConvert.SerializeObject(request));
            sbMsg.Append(JsonConvert.SerializeObject(openid));
            return sbMsg.ToString();
        }

        #region  RefreshToken token 检查TOKEN合法性并刷新
        /// <summary>
        /// 检查TOKEN合法性并刷新
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="user">当前要刷新的用户</param>
        /// <param name="ipAddress">当前登陆的IP地址</param>
        /// <exception>
        /// 异常ID：2->手机号码在系统不存在
        /// 异常ID：5->用户登陆已失效
        /// </exception>
        /// <returns></returns>
        public StudentLoginResponse RefreshToken(HssUserPrincipal user,string ipAddress)
        {
            long userId = long.Parse(user.UserId);
            TblHssPassport passport = _repository.Value.Load(userId);
            StudentLoginResponse result = new StudentLoginResponse();

            //1、验证账户是否存在
            ValidateUserExist(passport);
            if (passport.OpenId!=user.OpenId)
            {
                throw new BussinessException(ModelType.Hss, 5);
            }

            //2、更新最新登陆信息
            UpdateLastLoginInfo(passport,ipAddress);

            //3、记录登陆日记
            AddOperationLog(passport);

            //4、重新刷新token
            JwtTokenService tokenService = new JwtTokenService();
            result.Token = tokenService.CreateToken(passport);
            return result;
        }
        #endregion

        #region UpdateLastLoginInfo 更新最新登陆信息
        /// <summary>
        /// 描述：更新最新登陆信息
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="passport">用户账号信息</param>
        /// <param name="ipAddress">IP地址</param>
        private void UpdateLastLoginInfo(TblHssPassport passport,string ipAddress)
        {
            passport.LastLoginIp = passport.CurrentLoginIp;
            passport.LastLoginDate = passport.CurrentLoginDate;
            passport.CurrentLoginIp = ipAddress;
            passport.CurrentLoginDate = DateTime.Now;
            passport.LoginTimes = passport.LoginTimes + 1;
            _repository.Value.Update(passport);
        }
        #endregion

        #region AddOperationLog 添加操作日记
        /// <summary>
        /// 描述：添加操作日记
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="passport">用户账号信息</param>
        private void AddOperationLog(TblHssPassport passport)
        {
            OperationLogService operationLogService = new OperationLogService();
            TblDatOperationLog log = new TblDatOperationLog()
            {
                BusinessId = passport.PassporId,
                BusinessType = (int)LogBusinessType.HssLogin,
                FlowStatus = (int)OperationFlowStatus.Finish,
                OperationLogId = IdGenerator.NextId(),
                OperatorId = "",
                OperatorName = "",
                Remark = $"用户{passport.UserCode} 于 {DateTime.Now} 登陆了家校互联",
                CreateTime = DateTime.Now,
                SchoolId = ""
            };
            operationLogService.Add(log);
        }
        #endregion

        #region  ValidateUserCode 验证家长账号是否存在
        /// <summary>
        /// 验证家长账号是否存在
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <param name="passport">登陆账号的信息</param>
        /// <exception>
        /// 异常ID：2->手机号码在系统不存在
        /// </exception>
        private void ValidateUserExist(TblHssPassport passport)
        {
            if (passport == null)
            {
                LogWriter.Write("AuthenicationService.ValidateUserExist", "找不到用户",LoggerType.Error);
                throw new BussinessException(ModelType.Hss,2);
            }

        }
        #endregion

        #region ValidateSmsCode 短信验证码检查
        /// <summary>
        /// 短信验证码检查
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <param name="smsCode">收到的验证码</param>
        /// <exception>
        /// 异常ID：1->验证码有误
        /// </exception>
        public void ValidateSmsCode(string mobile,string smsCode)
        {
            UserLoginSmsService smsService = new UserLoginSmsService(mobile);
            if (!smsService.Validate(smsCode))
            {
                LogWriter.Write("AuthenicationService.ValidateSmsCode", "ValidateSmsCode");
                throw new BussinessException(ModelType.Hss, 1);
            }
        }
        #endregion
    }
}
 
