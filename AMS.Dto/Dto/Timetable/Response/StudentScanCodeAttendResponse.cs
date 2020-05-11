using System;
using System.Collections.Generic;
using System.Text;
using AMS.Dto.Enum;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：老师使用K信扫描学生二维码考勤
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-03</para>
    /// </summary>
    public class StudentScanCodeAttendResponse
    {
        /// <summary>
        /// 考勤状态 0=成功 1=失败  2=多个课程选项 3=系统警告 4=温馨警告
        /// </summary>
        public ScanCodeAttendStatusResponse AttendStatus { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 班级信息
        /// </summary>
        public List<ScanCodeClassInfo> ClassItem { get; set; }
    }

    /// <summary>
    /// 扫码考勤班级信息
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-03</para>
    /// </summary>
    /// <remarks>
    /// 扫码考勤可能在同一交叉时间段出现两个班级考勤
    /// 这个类定义返回班级让老师选择
    /// </remarks>
    public class ScanCodeClassInfo
    {
        /// <summary>
        /// 班级Id
        /// </summary>
        public long ClassId { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public string ClassTime { get; set; }
    }
}
