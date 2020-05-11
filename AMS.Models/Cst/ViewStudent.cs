using System;
using Newtonsoft.Json;

namespace AMS.Models
{
    /// <summary>
    /// 学生实体
    /// </summary>
    public class ViewStudent
    {
        /// <summary>
        /// 主健(TblCstStudent学生信息)
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 校区编号
        /// </summary>
        [JsonIgnore]
        public string SchoolId { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNo { get; set; }

        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 学生头像
        /// </summary>
        public string HeadFaceUrl { get; set; }

        /// <summary>
        /// 性别(1男2女)
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 出生年月
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string LinkMobile { get; set; }

        /// <summary>
        /// 监护人手机号
        /// </summary>
        public string ContactPersonMobile { get; set; }

        /// <summary>
        /// 学生状态编号（1在读2休学3流失）
        /// </summary>
        public int StudyStatus { get; set; }

        /// <summary>
        /// 剩余课次
        /// </summary>
        public int RemindClassTimes { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
