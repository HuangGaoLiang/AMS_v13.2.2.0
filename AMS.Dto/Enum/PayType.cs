using System.ComponentModel;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述:  支付方式
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2018-09-07</para>
    /// </summary>
    public enum PayType
    {
        /// <summary>
        /// 刷卡
        /// </summary>
        [Description("刷卡")]
        Pos = 1,
        /// <summary>
        /// 现金
        /// </summary>
        [Description("现金")]
        Cash = 2,
        /// <summary>
        /// 银行转账
        /// </summary>
        [Description("转账")]
        BankTransfer = 3,
        /// <summary>
        /// 微信
        /// </summary>
        [Description("微信")]
        WxPay = 4,
        /// <summary>
        /// 支付宝
        /// </summary>
        [Description("支付宝")]
        AliPay = 5,
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 9,
        /// <summary>
        /// 余额
        /// </summary>
        [Description("余额")]
        UseBalance = 20,

    }
}
