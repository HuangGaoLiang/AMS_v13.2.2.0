/****************************************************************************\
所属系统:招生系统
所属模块:家校互联
创建时间：2019-03-08
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
    /// 描    述：家校互联学期与班级信息
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-08</para>
    /// </summary>
    public class StudentTermClassInfoResponse
    {
        /// <summary>
        /// 学期名称
        /// </summary>
        public string TermName { get; set; }

        /// <summary>
        /// 学生班级信息
        /// </summary>
        public List<StudentClassInfoResponse> ClassList { get; set; }
    }

    /// <summary>
    /// 描    述：家校互联班级信息
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-08</para>
    /// </summary>
    public class StudentClassInfoResponse
    {
        /// <summary>
        /// 班级Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseId { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }
    }
}