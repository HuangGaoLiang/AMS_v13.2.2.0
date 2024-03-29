﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：写生排课（班级选择）实体请求类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-01-02</para>
    /// </summary>
    public class LifeClassSelectClassRequest
    {
        /// <summary>
        /// 学生Id
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 学生编号
        /// </summary>
        public string StudentNo { get; set; }

        /// <summary>
        /// 学生名称
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 班级Id
        /// </summary>
        public long ClassId { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 学生报名项Id
        /// </summary>
        public long EnrollOrderItemId { get; set; }
    }
}
