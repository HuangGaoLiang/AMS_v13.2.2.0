/****************************************************************************\
所属系统：招生系统
所属模块：基础资料模块
创建时间：2019-03-05
作    者：郭伟佳
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/

using AMS.Core;
using Newtonsoft.Json;
using System;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：信息投诉与建议信息列表实体类
    /// <para>作    者: Huang GaoLiang</para>
    /// <para>创建时间: 2018-11-23</para>
    /// </summary>
    public class FeedbackListResponse
    {
        /// <summary>
        /// 主键
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long FeedbackId { get; set; }

        /// <summary>
        /// 提交日期
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 所属校区名称
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 提交人名称
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        /// 提交内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 处理状态
        /// </summary>
        public string ProcessStatusName { get; set; }
    }
}
