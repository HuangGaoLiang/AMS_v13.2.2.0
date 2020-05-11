/****************************************************************************\
所属系统：招生系统
所属模块：财务模块
创建时间：2019-03-15
作   者：郭伟佳
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
    /// 描    述：学生课次考勤确认信息实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-15</para>
    /// </summary>
    public class StudentAttendConfirmRequest
    {
        /// <summary>
        /// 老师Id
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// 班级Id
        /// </summary>
        public long ClassId { get; set; }

        /// <summary>
        /// 确认的日期（0表示学期）
        /// </summary>
        public int? Month { get; set; }

        /// <summary>
        /// 签字的图片地址
        /// </summary>
        public string TeacherSignUrl { get; set; }
    }
}