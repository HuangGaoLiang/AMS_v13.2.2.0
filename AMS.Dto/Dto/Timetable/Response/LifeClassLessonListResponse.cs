using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：写生课学生列表
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-09</para>
    /// </summary>
    public class LifeClassLessonListResponse
    {
        /// <summary>
        /// 主健(课次基础信息表)课次ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long LessonId { get; set; }

        /// <summary>
        /// 所属校区
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 学生ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long StudentId { get; set; }

        /// <summary>
        /// 学生名称
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 出生年月
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 1身份证 2护照 3港澳通行证 4台胞证
        /// </summary>
        public string IDType { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string IDNumber { get; set; }

        /// <summary>
        /// 监护人手机号
        /// </summary>
        public string ContactPersonMobile { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 教室ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ClassRoomId { get; set; }

        /// <summary>
        /// 课次类型
        /// </summary>
        public string LessonType { get; set; }

        /// <summary>
        /// 占用课次
        /// </summary>
        public int LessonCount { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public string BusinessType { get; set; }

        /// <summary>
        /// 业务ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long BusinessId { get; set; }
    }
}
