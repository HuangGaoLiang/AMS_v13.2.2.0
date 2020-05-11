/****************************************************************************\
所属系统：招生系统
所属模块：课表模块
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
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：学生课程考勤详情信息
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-07</para>
    /// </summary>
    public class StudentTimeLessonInfoResponse
    {
        /// <summary>
        /// 常规课考勤信息
        /// </summary>
        public StudentTimeLessonDetailResponse NormalLesson { get; set; }

        /// <summary>
        /// 补课/调课考勤列表信息
        /// </summary>
        public List<StudentTimeLessonDetailResponse> ReplenishLessons { get; set; }
    }
}