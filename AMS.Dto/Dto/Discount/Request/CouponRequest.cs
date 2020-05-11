using Jerrisoft.Platform.Public.PageExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：奖学金请求实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-06</para>
    /// </summary>
    public class CouponRequest : Page
    {
        /// <summary>
        /// 学生Id
        /// </summary>
        public long StudentId { get; set; }
    }


    /// <summary>
    /// 描述：优惠券状态，不用在数据相关
    /// <para>作  者：瞿琦</para>
    ///  <para>创建时间：2018-11-2</para>
    /// </summary>
    public enum CouponStatus
    {
        /// <summary>
        /// 全部
        /// </summary>
        [Description("全部")]
        All = 0,

        /// <summary>
        /// 未使用
        /// </summary>
        [Description("未使用")]
        NoUse = 1,
        /// <summary>
        /// 已使用
        /// </summary>
        [Description("已使用")]
        HasUse = 2,

        /// <summary>
        /// 已过期(不存数据库，用于页面标识)
        /// </summary>
        [Description("已过期")]
        NoEffect = 3
    }
}
