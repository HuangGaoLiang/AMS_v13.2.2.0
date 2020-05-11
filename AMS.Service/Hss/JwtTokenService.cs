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
using AMS.Storage.Models;
using Jerrisoft.Platform.Log;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AMS.Service.Hss
{
    /// <summary>
    /// 描述：家长认证生成token服务
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-06</para>
    /// </summary>
    public class JwtTokenService
    {
        private const string ISSUER = "jerrisoft.hss";
        private const string AUDIENCE = "jerrisoft.hss";
        private const string JwtUserId = "userId";       //Jwt用户ID key
        private const string JwtUserName = "userName";  //Jwt 用户名称 key
        private const string JwtOpenId = "oid";


        #region CreateToken 创建token
        /// <summary>
        /// 创建token
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <param name="user">登陆的用户实体信息</param>
        /// <returns>token值</returns>
        internal String CreateToken(TblHssPassport user)
        {
            string privateKey = ClientConfigManager.HssConfig.TokenKey.PrivateKey;  //使用私钥加密


            int tokenTimestamp = ClientConfigManager.HssConfig.TokenTimestamp;

            RSA rsa = RSAKeyHelper.CreateRsaProviderFromPrivateKey(privateKey);
            //Claims(Payload)
            //       Claims 部分包含了一些跟这个 token 有关的重要信息。 JWT 标准规定了一些字段，下面节选一些字段:

            //iss: The issuer of the token，token 是给谁的
            //       sub: The subject of the token，token 主题
            //       exp: Expiration Time。 token 过期时间，Unix 时间戳格式
            //       iat: Issued At。 token 创建时间， Unix 时间戳格式
            //       jti: JWT ID。针对当前 token 的唯一标识
            //       除了规定的字段外，可以包含其他任何 JSON 兼容的字段。
            var key = new RsaSecurityKey(rsa);
            var creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(JwtUserId, user.PassporId.ToString()));
            claims.Add(new Claim(JwtUserName, user.UserCode));
            claims.Add(new Claim(JwtOpenId, user.OpenId));

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                   issuer: ISSUER,
                   audience: AUDIENCE,
                   claims: claims,
                   expires: DateTime.Now.AddHours(tokenTimestamp),
                   signingCredentials: creds);

            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return token;
        }
        #endregion

        #region GetUser 获取TOKEN对应的用户信息
        /// <summary>
        /// 获取TOKEN对应的用户信息
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public HssUserPrincipal GetUser(String token)
        {
            string publicKey = ClientConfigManager.HssConfig.TokenKey.PublicKey;  //使用私钥加密
            HssUserPrincipal user = null;

            //从token中解析出claims 信息
            List<Claim> claims = this.Decode(publicKey, token);

            if (claims != null && claims.Count > 0)
            {
                user = new HssUserPrincipal();
                user.UserId = this.GetClaimValue(claims, JwtUserId);
                user.UserCode = this.GetClaimValue(claims, JwtUserName);
                user.OpenId= this.GetClaimValue(claims, JwtOpenId);
            }
            return user;
        }

        #endregion

        #region Decode 解析token
        /// <summary>
        /// 解析token
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <param name="publicKey">公钥</param>
        /// <param name="token">要解析的token的值</param>
        /// <returns></returns>
        private List<Claim> Decode(string publicKey, string token)
        {
            List<Claim> claims = new List<Claim>();
            using (RSA rsa = RSAKeyHelper.CreateRsaProviderFromPublicKey(publicKey))
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                TokenValidationParameters validParam = new TokenValidationParameters()
                {
                    IssuerSigningKey = new RsaSecurityKey(rsa),
                    ValidateIssuer = false,     //不验证颁布者
                    ValidateAudience = false,   //不验证使用者
                    ValidateLifetime = true,
                };
                try
                {
                    SecurityToken jwtSecurityToken = null;
                    handler.ValidateToken(token, validParam, out jwtSecurityToken);
                    JwtSecurityToken jwtSecurityToken2 = jwtSecurityToken as JwtSecurityToken;
                    if (jwtSecurityToken2 != null)
                    {
                        claims.AddRange(jwtSecurityToken2.Claims);
                    }
                    else
                    {
                        LogWriter.Write("JwtTokenService.Decode","TOKEN解析发生错误,可能是密钥不正确", LoggerType.Error);
                    }
                }
                catch (SecurityTokenDecryptionFailedException ex)
                {
                    LogWriter.Write("JwtTokenService.Decode", "TOKEN解析发生错误", LoggerType.Error);
                }
                catch (SecurityTokenInvalidSignatureException ex)
                {
                    //签名校验失败
                    LogWriter.Write("JwtTokenService.Decode", "签名校验失败", LoggerType.Error);
                }
                catch (SecurityTokenExpiredException ex)
                {
                    //Token过期,status=-9
                    LogWriter.Write("JwtTokenService.Decode", "Token过期", LoggerType.Error);
                }
                catch (Exception ex)
                {
                    //其他异常
                    LogWriter.Write("JwtTokenService.Decode", ex.Message, LoggerType.Error);
                }
                return claims;
            }
        }
        #endregion

        #region GetClaimValue 从Claim中提取值
        /// <summary>
        /// 从Claim中提取值
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <param name="list">一组从JWT解析出来的chaim</param>
        /// <param name="key">要提取值对应的key</param>
        /// <returns></returns>
        private string GetClaimValue(List<Claim> list,string key)
        {
            Claim claim = list.FirstOrDefault(t => string.Compare(t.Type, key, true) == 0);
            if (claim != null)
            {
                return claim.Value;
            }
            return "";
        }
        #endregion

    }
}
