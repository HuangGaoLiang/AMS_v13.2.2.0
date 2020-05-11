/****************************************************************************\
所属系统:招生系统
所属模块:课表模块
创建时间:
作   者:
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/
using AMS.Core;
using Jerrisoft.Platform.Public.PageExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：转班明细列表查询结果
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2019-3-6</para>
    /// </summary>
    public class ChangeClassListResponse  
    {
        /// <summary>
        /// 校区Id
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 学生Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long StudentId { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNo { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }
        /// <summary>
        /// 报名时间
        /// </summary>
        public DateTime EnrollTime { get; set; }
        /// <summary>
        /// 课程实收价
        /// </summary>
        public decimal TotalTradeAmount { get; set; }
        /// <summary>
        /// 报名课次
        /// </summary>
        public int EnrollClassTimes { get; set;}
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 学期类型Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long TermTypeId { get; set; }
        /// <summary>
        /// 学期类型名称
        /// </summary>
        public string TermTypeName { get; set; }
        /// <summary>
        /// 转班日期（转班提交时间）
        /// </summary>
        public DateTime ChangeClassSubmitTime { get; set; }
        /// <summary>
        /// 转出班级Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long OutClassId { get; set; }
        /// <summary>
        /// 转出班级名称
        /// </summary>
        public string OutClassName { get; set; }
        /// <summary>
        /// 转出班级代码
        /// </summary>
        public string OutClassNo { get; set; }
        /// <summary>
        /// 转出班级老师
        /// </summary>
        public string OutClassTeacher { get; set; }
        /// <summary>
        /// 停课日期
        /// </summary>
        public DateTime OutDate { get; set; }
        /// <summary>
        /// 转班课次
        /// </summary>
        public int ChangeClassTimes { get; set; }

        /// <summary>
        /// 转入班级Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long InClassId { get; set; }
        /// <summary>
        /// 转入班级名称
        /// </summary>
        public string InClassName { get; set; }
        /// <summary>
        /// 转入班级代码
        /// </summary>
        public string InClassNo { get; set; }
        /// <summary>
        /// 转出班级老师
        /// </summary>
        public string InClassTeacher { get; set; }
        /// <summary>
        /// 开始上课日期
        /// </summary>
        public DateTime InDate { get; set; }
    }
}
