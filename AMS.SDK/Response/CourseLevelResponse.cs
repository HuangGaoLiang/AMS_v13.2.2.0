using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.SDK
{
    public class CourseLevelResponse
    {
        /// <summary>
        /// 课程等级Id
        /// </summary>
        public long CourseLevelId { get; set; }

        /// <summary>
        /// 等级名称
        /// </summary>
        public string CourseLevelName { get; set; }

        /// <summary>
        /// 开始年龄
        /// </summary>
        public int MinAge { get; set; }

        /// <summary>
        /// 结束年龄
        /// </summary>
        public int MaxAge { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        public int Duration { get; set; }
    }
}
