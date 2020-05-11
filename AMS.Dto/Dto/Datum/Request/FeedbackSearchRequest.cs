/****************************************************************************\
所属系统:招生系统
所属模块:基础资料模块
创建时间:
作   者:
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/

using Jerrisoft.Platform.Public.PageExtensions;
using Newtonsoft.Json;
using System;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：信息投诉与建议查询请求实体类
    /// <para>作    者：Hunag GaoLiang</para>
    /// <para>创建时间：2019-03-07</para>
    /// </summary>
    public class FeedbackSearchRequest : Page
    {
        /// <summary>
        /// 公司编号
        /// </summary>
        [JsonIgnore]
        public string CompanyId { get; set; }

        /// <summary>
        /// 处理状态
        /// </summary>
        public FeedbackProcessStatus? FeedbackProcessStatus { get; set; }

        /// <summary>
        /// 提交开始日期
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 提交结束日期
        /// </summary>
        public DateTime? EndtTime { get; set; }

        /// <summary>
        /// 校区编号
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 提交人
        /// </summary>
        public string CreatorName { get; set; }
    }
}
