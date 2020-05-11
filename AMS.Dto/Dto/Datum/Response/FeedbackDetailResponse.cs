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
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：信息投诉与建议详情实体类
    /// <para>作    者：</para>
    /// <para>创建时间：</para>
    /// </summary>
    public class FeedbackDetailResponse
    {
        /// <summary>
        /// 主键 
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long FeedbackId { get; set; }

        /// <summary>
        /// 校区Id
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 所属校区名称
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 提交人名称
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        /// 联系手机号 
        /// </summary>
        public string LinkMobile { get; set; }

        /// <summary>
        /// 家长账号
        /// </summary>
        public string ParentUserCode { get; set; }

        /// <summary>
        /// 反馈内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 处理状态
        /// </summary>
        public int ProcessStatus { get; set; }

        /// <summary>
        /// 处理状态名称
        /// </summary>
        public string ProcessStatusName { get; set; }

        /// <summary>
        /// 提交图片
        /// </summary>
        public List<string> Urls { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 处理人编号
        /// </summary>
        public string ProcessUserId { get; set; }

        /// <summary>
        /// 处理人名称
        /// </summary>
        public string ProcessUserName { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime ProcessTime { get; set; }

        /// <summary>
        /// 处理意见
        /// </summary>
        public string ProcessRemark { get; set; }
    }
}
