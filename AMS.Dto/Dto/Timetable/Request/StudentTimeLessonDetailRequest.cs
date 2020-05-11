/****************************************************************************\
所属系统：招生系统
所属模块：课表模块
创建时间：2019-03-20
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
    /// 描    述：学生考勤详情查询条件
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-20</para>
    /// </summary>
    public class StudentTimeLessonDetailRequest
    {
        /// <summary>
        /// 学生Id
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 学生课程Id集合
        /// </summary>
        public string LessonIdList { get; set; }
    }
}