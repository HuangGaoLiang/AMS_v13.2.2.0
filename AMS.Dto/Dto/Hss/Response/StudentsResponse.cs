/****************************************************************************\
所属系统:招生系统
所属模块:家校互联
创建时间:
作   者:
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

namespace AMS.Dto.Hss
{
    /// <summary>
    /// 描述：家长绑定的学生信息
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-02-26</para>
    /// </summary>
    public class StudentsResponse
    {
        /// <summary>
        /// 学生名称
        /// </summary>
        public string StudentName { get; set; }
        /// <summary>
        /// 学生ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long StudentId { get; set; }
        /// <summary>
        /// 学生头像
        /// </summary>
        public string HeadFaceUrl { get; set; }
        /// <summary>
        /// 学生在读的校区列表
        /// </summary>
        public List<StudentSchoolListResponse> SchoolList { get; set; }

    }


    /// <summary>
    /// 学生在多个校区数据
    /// </summary>
    public class StudentSchoolListResponse
    {
        /// <summary>
        /// 所在公司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 所在公司ID
        /// </summary>
        public string CompanyId { get; set; }
        /// <summary>
        /// 校区名称
        /// </summary>
        public string SchoolName { get; set; }
        /// <summary>
        /// 校区ID
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 当前的服务器时间
        /// </summary>
        public string ServerDateTime { get; set; }
        /// <summary>
        /// 学生最近一次上课时间
        /// </summary>
        public string ClassLatelyTime { get; set; }
    }

}
