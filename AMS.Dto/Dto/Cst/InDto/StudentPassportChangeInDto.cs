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
    /// 学生家长登录账号更改数据
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-13</para>
    /// </summary>
    public class StudentPassportChangeInDto
    {
        /// <summary>
        /// 一组新增的手机号码
        /// </summary>
        public List<string> MobileAddList { get; set; }
        /// <summary>
        /// 一组被删除的手机号码
        /// </summary>
        public List<string> MobileDeleteList { get; set; }
    }

}
