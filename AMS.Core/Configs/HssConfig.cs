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
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Core
{
    /// <summary>
    /// 描    述：学生登陆认证相关资源
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-07</para>
    /// </summary>
    public class HssConfig
    {
        /// <summary>
        /// 家长确认补签签到地址
        /// </summary>
        public string ParentsConfirmSignUrl { get; set; }

        /// <summary>
        /// 家长认证生成TOKEN的密钥对
        /// </summary>
        public TokenKey TokenKey { get; set; }
        /// <summary>
        /// 家校互联token过期时效 ，以小时为单位
        /// </summary>
        public int TokenTimestamp { get; set; }

        public HssLoginSms HssLoginSms { get; set; }

        /// <summary>
        /// 小程序唯一标识
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 小程序的 app secret
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// 获取openid的请求地址
        /// </summary>
        public string JsCode2SessionUrl { get; set; }

        /// <summary>
        /// 微信模板标题
        /// </summary>
        public WeChatTemplateTitle WeChatTemplateTitle { get; set; }

        /// <summary>
        /// 邮件发送人
        /// </summary>
        public EmailSender EmailSender { get; set; }
    }

    /// <summary>
    /// token 的生成密钥
    /// </summary>
    public class TokenKey
    {
        /// <summary>
        /// 私钥
        /// </summary>
        public string PrivateKey { get; set; }
        /// <summary>
        /// 公钥
        /// </summary>
        public string PublicKey { get; set; }
    }

    /// <summary>
    /// 家校互联登陆短信验证
    /// </summary>
    public class HssLoginSms
    {
        /// <summary>
        /// 短信发送人
        /// </summary>
        public string SenderId { get; set; }
        /// <summary>
        /// 发送短信的部门ID,该部份付费
        /// </summary>
        public string DepartId { get; set; }
        /// <summary>
        /// 短信模板格式
        /// </summary>
        public string SmsContentFormat { get; set; }
        /// <summary>
        /// 有效时间,以分钟为单位
        /// </summary>
        public int EffectiveTime { get; set; }
        /// <summary>
        /// 发送时间间隔,以秒为单位
        /// </summary>
        public int IntervalPer { get; set; }
    }

    /// <summary>
    /// 微信模板标题
    /// </summary>
    public class WeChatTemplateTitle
    {
        /// <summary>
        /// 到校通知
        /// </summary>
        public string ArrivalNotice { get; set; }

        /// <summary>
        /// 补签通知
        /// </summary>
        public string SignReplenishNotice { get; set; }


        /// <summary>
        /// 补课通知
        /// </summary>
        public string MakeupNotice { get; set; }

        /// <summary>
        /// 老师代课调整通知
        /// </summary>
        public string LessonTeacherNotice { get; set; }

        /// <summary>
        /// 全校上课日期调整通知
        /// </summary>
        public string LessonSchoolClassTimeNotice { get; set; }

        /// <summary>
        /// 班级上课时间调整通知
        /// </summary>
        public string LessonClassTimeNotice { get; set; }

        /// <summary>
        /// 学生请假通知
        /// </summary>
        public string LessonStudentLeaveNotice { get; set; }

        /// <summary>
        /// 投诉与建议通知
        /// </summary>
        public string FeedbackNotice { get; set; }
    }

    /// <summary>
    /// 邮件发送人
    /// </summary>
    public class EmailSender
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
    }
}
