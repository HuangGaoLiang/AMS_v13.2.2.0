/****************************************************************************\
所属系统:招生系统
所属模块:微信授权服务
创建时间:
作   者:
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/
using AMS.Core;
using Jerrisoft.Platform.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Anticorrosion.WX
{
    /// <summary>
    /// 微信小程序通过code调用后端服务获取openid
    /// 微信原话:登录凭证校验。通过 wx.login() 接口获得临时登录凭证 code 后传到开发者服务器调用此接口完成登录流程。
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-07</para>
    /// </summary>
    public class Code2SessionService
    {

        /// <summary>
        /// <summary>
        /// 获取用户的openId
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// </summary>
        /// <param name="code">通过小程序获取到的用户登录凭证（有效期五分钟）。</param>
        /// <exception>
        /// 异常ID：4->OPENID获取失败
        /// </exception>
        /// <returns></returns>
        public OpenIdResponse GetOpenId(string code)
        {
            OpenIdResponse result = null;
            try
            {
                HssConfig config = ClientConfigManager.HssConfig;

                Dictionary<string, string> paras = new Dictionary<string, string>();
                paras.Add("appid", config.AppId);
                paras.Add("secret", config.AppSecret);
                paras.Add("js_code", code);
                paras.Add("grant_type", WxConfig.GRANT_TYPE);
                HttpClientRequest request = new HttpClientRequest();
                result = request.Get<OpenIdResponse>(config.JsCode2SessionUrl, paras);
                if (result.ErrCode != WxConfig.RESULT_SUCCESS)
                {
                    throw new BussinessException(ModelType.Hss, 4,result.ErrMsg);
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write(this, ex.Message, LoggerType.Error);
                throw new BussinessException(ModelType.Hss, 4);
            }
            return result;
        }
    }
}
