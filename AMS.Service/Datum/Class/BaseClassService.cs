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
using AMS.Core;
using AMS.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AMS.Service
{
    /// <summary>
    /// 班级服务基类,班级分成常规班级和排课周班级
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-02-28</para>
    /// </summary>
    public abstract class BaseClassService : BService
    {
        /// <summary>
        /// 实例化一个班级服务
        /// <para>作    者：</para>
        /// <para>创建时间：</para>
        /// </summary>
        protected BaseClassService()
        {
        }

        /// <summary>
        /// 班级类型
        /// </summary>
        protected abstract ClassType ClassType { get; }








    }
}
