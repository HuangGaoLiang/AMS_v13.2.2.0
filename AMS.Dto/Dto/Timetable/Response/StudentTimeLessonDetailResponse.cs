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
    /// 描    述：学生课程考勤详情实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-07</para>
    /// </summary>
    public class StudentTimeLessonDetailResponse
    {
        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public string ClassDate { get; set; }

        /// <summary>
        /// 上课地点
        /// </summary>
        public string RoomNo { get; set; }

        /// <summary>
        /// 老师名称
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 考勤时间
        /// </summary>
        public string AttendDate { get; set; }
    }
}