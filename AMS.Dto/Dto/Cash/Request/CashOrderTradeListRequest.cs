using Jerrisoft.Platform.Public.PageExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：交易记录请求实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-07</para>
    /// </summary>
    public class CashOrderTradeListRequest : Page
    {
        /// <summary>
        /// 学生Id
        /// </summary>
        public long StudentId { get; set; }
    }
}
