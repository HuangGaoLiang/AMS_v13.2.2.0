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


using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：家校互联配置信息实体类
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2019-3-11</para>
    /// </summary>
    public class HomeSchoolSettingListResponse
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        public string CompanyId { get; set; }

        /// <summary>
        /// 功能Id
        /// </summary>
        public int FuntionId { get; set; }
        /// <summary>
        /// 功能名称
        /// </summary>
        public string FuntionName { get; set; }

        /// <summary>
        /// 数据Id
        /// </summary>
        public string DataId { get; set; }
        /// <summary>
        /// 数据名称
        /// </summary>
        public string DataValue { get; set; }
    }
}