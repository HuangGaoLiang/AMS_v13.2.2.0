using AMS.Core;
using Newtonsoft.Json;
using System;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述: 学生管理列表数据
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class StudentListResponse
    {
        /// <summary>
        /// 主健(TblCstStudent学生信息)
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long StudentId { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNo { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 性别(1男2女)
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 性别(1男2女)
        /// </summary>
        public string SexName { get; set; }

        /// <summary>
        /// 出生年月
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

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
        /// 学生状态名称
        /// </summary>
        public string StudyStatuName { get; set; }

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
