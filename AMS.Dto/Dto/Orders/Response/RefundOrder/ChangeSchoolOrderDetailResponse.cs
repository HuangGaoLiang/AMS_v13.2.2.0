using System;
using System.Collections.Generic;

namespace AMS.Dto
{
    /// <summary>
    /// 转校订单详情(提交订单后)
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class ChangeSchoolOrderDetailResponse : IOrderDetailResponse
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderStatusName { get; set; }

        /// <summary>
        /// 订单状态 {-2:拒绝,-1:撤销,0:提交 ,1:确认 10:接收}
        /// </summary>
        public int OrderStatus { get; set; }

        /// <summary>
        /// 操作日期
        /// </summary>
        public DateTime OperationTime { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 从余额转出多少钱
        /// </summary>
        public decimal TransFromBalance { get; set; }

        /// <summary>
        /// 转入校区名称
        /// </summary>
        public string InSchoolName { get; set; }

        /// <summary>
        /// 转出校区名称
        /// </summary>
        public string ToSchoolName { get; set; }

        /// <summary>
        /// 转出时间
        /// </summary>
        public DateTime OutTime { get; set; }

        /// <summary>
        /// 转出原因
        /// </summary>
        public string OutWhy { get; set; }

        /// <summary>
        /// 申请表地址集合
        /// </summary>
        public List<string> FileUrls { get; set; }

        /// <summary>
        /// 已上课次合计
        /// </summary>
        public int TotalHaveClassLesson { get; set; }

        /// <summary>
        /// 应扣除上课费用合计
        /// </summary>
        public decimal TotalLessonFee { get; set; }

        /// <summary>
        /// 票据情况 1:未开发票 2:发票齐全 3:发票遗失
        /// </summary>
        public int ReceiptStatus { get; set; }

        /// <summary>
        /// 扣款合计
        /// </summary>
        public decimal CostTotal { get; set; }

        /// <summary>
        /// 实际金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 账号信息
        /// </summary>
        public TransferAccountsResponse TransferAccounts { get; set; }

        /// <summary>
        /// 转校课程费用明细
        /// </summary>
        public List<TransferCourseFeeDetailResponse> TransferCourseFeeDetail { get; set; }

        /// <summary>
        /// 扣费
        /// </summary>
        public List<ChangeSchoolCostResponse> Cost { get; set; }

        /// <summary>
        /// 操作日志
        /// </summary>
        public List<OperationLogResponse> OperationLog { get; set; }
    }

    /// <summary>
    /// 账户信息
    /// </summary>
    public class TransferAccountsResponse
    {
        /// <summary>
        /// 发卡银行
        /// </summary>
        public string IssuingBank { get; set; }

        /// <summary>
        /// 账户姓名
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        public string BankCardNumber { get; set; }
    }

    /// <summary>
    /// 扣费明细
    /// </summary>
    public class ChangeSchoolCostResponse
    {
        /// <summary>
        /// 费用Id
        /// </summary>
        public long CostId { get; set; }

        /// <summary>
        /// 扣款名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 扣费金额
        /// </summary>
        public decimal CostAmount { get; set; }
    }

    /// <summary>
    /// 操作日志
    /// </summary>
    public class OperationLogResponse
    {
        /// <summary>
        /// 流程状态名称
        /// </summary>
        public string ProcessStatusName { get; set; }

        /// <summary>
        /// 操作日期
        /// </summary>
        public DateTime OperationTime { get; set; }

        /// <summary>
        /// 操作日志状态 {-2:拒绝,-1:撤销,0:提交 ,1:确认 10:接收}
        /// </summary>
        public OperationFlowStatus OperationFlowStatus { get; set; }
        /// <summary>
        /// 操作人名称
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 校区名称
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 退回原因
        /// </summary>
        public string Remark { get; set; }
    }
}
