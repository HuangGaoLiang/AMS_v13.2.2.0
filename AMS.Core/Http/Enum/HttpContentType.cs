using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Core
{
    /// <summary>
    /// 描    述：Http Post数据类型
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-01-10</para>
    /// </summary>
    public enum HttpContentType
    {
        /// <summary>
        /// JSON:application/json
        /// </summary>
        Json,
        /// <summary>
        ///From: application/x-www-form-urlencoded
        /// </summary>
        Form,
        /// <summary>
        ///From: application/octet-stream
        /// </summary>
        Stream
    }
}
