/****************************************************************************\
所属系统：招生系统
所属模块：基础资料模块
创建时间：2019-03-07
作    者：郭伟佳
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：投诉与建议提交请求实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-07</para>
    /// </summary>
    public class FeedbackAddRequest
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        [Required]
        public string CompanyId { get; set; }

        /// <summary>
        /// 校区Id
        /// </summary>
        [Required]
        public string SchoolId { get; set; }

        /// <summary>
        /// 校区名称
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 学生Id
        /// </summary>
        [Required]
        public long StudentId { get; set; }

        /// <summary>
        /// 登录人手机号码
        /// </summary>
        [Required]
        public string LinkMobile { get; set; }

        /// <summary>
        /// 提交人名称
        /// </summary>
        [Required]
        public string CreatorName { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }

        /// <summary>
        /// 上传图片
        /// </summary>
        public List<AttchmentAddRequest> AttachmentUrlList { get; set; }
    }
}