/****************************************************************************\
所属系统:招生系统
所属模块:客户模块
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

namespace AMS.Dto
{
    /// <summary>
    /// 描述：学生卡打印信息
    /// <para>作    者：Huang GaoLiang</para>
    /// <para>创建时间：2019-03-06</para>
    /// </summary>
    public class StudentCardResponse
    {
        /// <summary>
        /// 学生编号
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long StudentId { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        public string HeadFaceUrl { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNo { get; set; }

        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 公司图片
        /// </summary>
        public string CompanyImage { get; set; }

        /// <summary>
        /// 学生证反面图片
        /// </summary>
        public string NegativeImage { get; set; }

    }
}
