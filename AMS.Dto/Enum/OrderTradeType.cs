using System.ComponentModel;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述:  交易类型/订单类型
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2018-09-07</para>
    /// </summary>
    public enum OrderTradeType
    {
        /// <summary>
        /// 报班
        /// </summary>
        [Description("报班")]
        EnrollOrder = 1,
        /// <summary>
        /// 报班退费
        /// </summary>
        [Description("报班退费")]
        LeaveClassOrder = 2,
        /// <summary>
        /// 游学营退费
        /// </summary>
        [Description("游学营退费")]
        CampOrder = 3,
        /// <summary>
        /// 余额退费
        /// </summary>
        [Description("余额退费")]
        BalanceOrder = 4,
        /// <summary>
        /// 定金
        /// </summary>
        [Description("定金")]
        DepositOrder = 5,
        /// <summary>
        /// 休学
        /// </summary>
        [Description("休学")]
        LeaveSchoolOrder = 6,
        /// <summary>
        /// 转校
        /// </summary>
        [Description("转校转出")]
        ChangeSchool = 7,
        /// <summary>
        /// 转校
        /// </summary>
        [Description("转校转入")]
        ChangeSchoolIn = 8,

        /// <summary>
        /// 报班作废
        /// </summary>
        [Description("报班作废")]
        CancelEnrollOrder = -1,
        /// <summary>
        /// 报班退费作废
        /// </summary>
        [Description("报班退费作废")]
        CancelLeaveClassOrder = -2,
        /// <summary>
        /// 游学营退费作废
        /// </summary>
        [Description("游学营退费作废")]
        CancelCampOrder = -3,
        /// <summary>
        /// 余额退费作废
        /// </summary>
        [Description("余额退费作废")]
        CancelBalanceOrder = -4,
        /// <summary>
        /// 定金作废
        /// </summary>
        [Description("定金作废")]
        CancelDepositOrder = -5,
        /// <summary>
        /// 休学作废
        /// </summary>
        [Description("休学作废")]
        CancelLeaveSchoolOrder = -6,
        /// <summary>
        /// 转校撤销
        /// </summary>
        [Description("转校撤销")]
        CancelChangeSchool = -7,
    }
}
