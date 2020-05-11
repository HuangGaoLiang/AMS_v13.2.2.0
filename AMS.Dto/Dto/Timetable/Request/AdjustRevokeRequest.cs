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

namespace AMS.Dto
{
    /// <summary>
    /// 描述：撤销排课提交数据
    /// <para>作    者：Huang GaoLiang</para>
    /// <para>创建时间：2019-03-11</para>
    /// </summary>
    public class AdjustRevokeRequest : IAdjustLessonRequest
    {
        /// <summary>
        /// 学生编号
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 课次Id
        /// </summary>
        public long LessonId { get; set; }
    }
}
