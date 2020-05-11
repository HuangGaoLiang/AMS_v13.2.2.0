using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：老师使用K信扫描学生二维码考勤输入
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-03</para>
    /// </summary>
    public class StudentScanCodeAttendRequest
    {
        /// <summary>
        /// 学生ID
        /// </summary>
        [Required(ErrorMessage = "学生ID不可为空")]
        public long StudentId { get; set; }

        /// <summary>
        /// 如果同一时间扫码存在多个课次,请选择所在班级
        /// </summary>
        public List<long> ClassIds { get; set; }
    }
}
