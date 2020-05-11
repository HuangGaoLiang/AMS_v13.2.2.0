
namespace AMS.Dto
{

    /// <summary>
    /// 打印收据类型
    /// </summary>
    public enum PrintBillType
    {
        /// <summary>
        /// 学生信息采集表
        /// </summary>
        StudentInformation = 1,
        /// <summary>
        /// 定金收据
        /// </summary>
        DepositOrderBill = 2,
        /// <summary>
        /// 报班收据
        /// </summary>
        EnrollOrderBill = 3,
        /// <summary>
        /// 报名
        /// </summary>
        SingUp = 4,
        /// <summary>
        /// 退班
        /// </summary>
        LeaveClass = 5,
        /// <summary>
        /// 余额退费
        /// </summary>
        BalanceRefund = 6
    }
}
