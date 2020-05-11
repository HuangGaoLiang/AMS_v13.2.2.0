using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：学生课次不够返回实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-20</para>
    /// </summary>
    public class LifeClassLackTimesListResponse
    {
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNo { get; set; }
    }
}
