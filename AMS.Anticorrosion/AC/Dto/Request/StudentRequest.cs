using System;

namespace AMS.Anticorrosion.AC
{
    /// <summary>
    /// 描    述: 学生信息请求类
    /// <para>作    者: Huang GaoLiang</para>
    /// <para>创建时间: 2019-02-19</para>
    /// </summary>
    public class StudentRequest
    {
        /// <summary>
        /// 学生序号
        /// </summary>
        public long StuSNo { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name1 { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public byte? Gender { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? DOB { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Guard1Tel
        /// </summary>
        public string Guard1Tel { get; set; }

        /// <summary>
        /// Guard2Tel
        /// </summary>
        public string Guard2Tel { get; set; }

        /// <summary>
        /// Email1
        /// </summary>
        public string Email1 { get; set; }
        /// <summary>
        /// Email2
        /// </summary>
        public string Email2 { get; set; }

        /// <summary>
        /// Email3
        /// </summary>
        public string Email3 { get; set; }
    }
}
