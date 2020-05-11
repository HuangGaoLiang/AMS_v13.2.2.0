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
    /// 学生信息
    /// <para>作    者：Huang GaoLiang</para>
    /// <para>创建时间：2019-03-14</para>
    /// </summary>
    public class StudentInfoInDto
    {
        /// <summary>
        /// 学生编号
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public List<string> LinkMobileList { get; set; }
    }

}
