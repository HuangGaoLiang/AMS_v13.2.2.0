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


using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：信息投诉与建议请求实体类
    /// <para>作    者：Huang GaoLiang </para>
    /// <para>创建时间：2019-03-07</para>
    /// </summary>
    public class FeedbackRequest
    {
        /// <summary>
        /// 处理状态
        /// </summary>
        public FeedbackProcessStatus ProcessStatus { get; set; }

        /// <summary>
        /// 处理意见
        /// </summary>
        public string ProcessRemark { get; set; }

        /// <summary>
        /// 处理人ID
        /// </summary>
        [JsonIgnore]
        public string ProcessUserId { get; set; }

        /// <summary>
        /// 处理人名称
        /// </summary>
        [JsonIgnore]
        public string ProcessUserName { get; set; }

    }
}
