using System;

namespace AMS.Anticorrosion.HRS
{
    /// <summary>
    /// 描    述: 校区人员信息
    /// <para>作    者: Huang GaoLiang</para>
    /// <para>创建时间: 2019-02-19</para>
    public class EmployeeResponse
    {
        /// <summary>
        /// 员工编号
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// 员工名称
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 人员在职状态 0:在职 1:离职
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 离职日期
        /// </summary>
        public DateTime? LeaveDate { get; set; }
    }
}
