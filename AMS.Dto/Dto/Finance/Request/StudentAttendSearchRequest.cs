/****************************************************************************\
所属系统：招生系统
所属模块：财务模块
创建时间：2019-03-05
作   者：郭伟佳
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：学生课次考勤查询条件实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-04</para>
    /// </summary>
    public class StudentAttendSearchRequest
    {
        /// <summary>
        /// 学期Id集合
        /// </summary>
        [Required]
        public long? TermId { get; set; }

        /// <summary>
        /// 班级Id
        /// </summary>
        [Required]
        public long? ClassId { get; set; }

        /// <summary>
        /// 状态：1正常-1无效
        /// </summary>
        public LessonUltimateStatus LessonStatus { get; set; } = LessonUltimateStatus.Normal;

        /// <summary>
        /// 年份
        /// </summary>
        [Required]
        public int Year { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; set; }
    }
}
