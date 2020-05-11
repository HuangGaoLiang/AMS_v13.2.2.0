namespace AMS.Dto
{
    /// <summary>
    /// 描    述:  员工信息类
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
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
        /// 业绩比例
        /// </summary>
        public decimal Proportion { get; set; }
    }
}
