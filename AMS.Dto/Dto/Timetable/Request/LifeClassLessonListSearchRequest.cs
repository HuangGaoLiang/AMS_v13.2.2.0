using Jerrisoft.Platform.Public.PageExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：学生排课查询条件请求实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public class LifeClassLessonListSearchRequest : Page
    {
        /// <summary>
        /// 写生课Id
        /// </summary>
        public long LifeTimeId { get; set; }

        /// <summary>
        /// 班级Id
        /// </summary>
        public long? ClassId { get; set; }

        /// <summary>
        /// 学生名称或手机号码
        /// </summary>
        public string Keyword { get; set; }
    }
}
