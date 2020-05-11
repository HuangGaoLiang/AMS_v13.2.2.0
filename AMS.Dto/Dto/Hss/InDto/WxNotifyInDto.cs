/****************************************************************************\
所属系统:招生系统
所属模块:家校互联
创建时间：2019-03-13
作    者：蔡亚康
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{

    /// <summary>
    /// 微信通知数据源
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-13</para>
    /// </summary>
    public class WxNotifyInDto
    {
        /// <summary>
        /// 业务ID
        /// </summary>
        public int BusinessId { get; set; }
        /// <summary>
        /// 接收者openid
        /// </summary>
        public List<string> ToUser { get; set; }
        /// <summary>
        /// 模板ID
        /// </summary>
        public long TemplateId { get; set; }
        /// <summary>
        /// 模板跳转链接（海外帐号没有跳转能力）
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 模板内容字体颜色，不填默认为黑色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 模板数据
        /// </summary>
        public List<WxNotifyItemInDto> Data { get; set; }
    }

    /// <summary>
    /// 模板数据
    /// </summary>
    public class WxNotifyItemInDto
    {
        /// <summary>
        /// 数据的Key,即模板中显示的字段
        /// </summary>
        public string DataKey { get; set; }
        /// <summary>
        /// 模板中显示的字段的值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 模板中显示的字段的值颜色
        /// </summary>
        public string Color { get; set; }
    }
}
