namespace AMS.Dto
{
    /// <summary>
    /// 描    述:  订金
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class DepositOrderAddRequest
    {
        /// <summary>
        /// 学生编号（TblCstStudent）
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 订金用途（1预报名 2游学营定金 3写生定金）
        /// </summary>
        public UsesType UsesType { get; set; }

        /// <summary>
        /// 定金
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 支付方式(1刷卡 2现金 3转账 4微信 5支付宝 9其他)
        /// </summary>
        public PayType PayType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
