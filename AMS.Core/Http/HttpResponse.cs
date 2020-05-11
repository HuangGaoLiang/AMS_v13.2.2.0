using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Core
{
    /// <summary>
    /// 描    述：Http请求返回实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-01-09</para>
    /// </summary>
    public class HttpDataResponse
    {
        /// <summary>
        /// 返回的数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public int Code { get; set; }
    }
}
