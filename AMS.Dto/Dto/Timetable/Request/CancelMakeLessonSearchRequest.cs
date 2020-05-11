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

using System.ComponentModel.DataAnnotations;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：撤销排课查询条件
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-02-26</para>
    /// </summary>
    public class CancelMakeLessonSearchRequest
    {
        /// <summary>
        /// 学生编号
        /// </summary>
        [Required]
        public long StudentId { get; set; }

        /// <summary>
        /// 学期类型
        /// </summary>
        [Required]
        public long TermId { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        [Required]
        public long CourseId { get; set; }
    }
}
