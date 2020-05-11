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
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto.Hss
{
    /// <summary>
    /// 描    述：家校互联学生课表信息
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-08</para>
    /// </summary>
    public class StudentLessonResponse
    {
        /// <summary>
        /// 上课日期
        /// </summary>
        public DateTime ClassDate { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 最新课程标志：（0不是最新；1是最新）
        /// </summary>
        public int LastFlag { get; set; }

        /// <summary>
        /// 时间过去标志（0时间未过去；1时间已过去）
        /// </summary>
        public int TimePastFlag { get; set; }

        /// <summary>
        /// 上课地点
        /// </summary>
        public string RoomNo { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public string ClassTime { get; set; }

        /// <summary>
        /// 老师名称
        /// </summary>
        public string TeacherName { get; set; }
    }
}
