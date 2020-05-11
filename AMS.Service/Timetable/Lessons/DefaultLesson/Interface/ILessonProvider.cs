using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Service
{
    /// <summary>
    /// 课次操作提供者
    /// </summary>
    public interface ILessonProvider
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        int BusinessType { get; }
    }
}
