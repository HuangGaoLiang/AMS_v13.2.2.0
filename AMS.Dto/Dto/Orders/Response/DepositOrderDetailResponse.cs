using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述:  定金详情信息
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
   
    public class DepositOrderDetailResponse : IOrderDetailResponse
    {
        /// <summary>
        /// 订单详情无参构造函数
        /// </summary>
        public DepositOrderDetailResponse()
        {

        }
        /// <summary>
        /// 主健（DepositOrderId）
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long DepositOrderId { get; set; }

        /// <summary>
        /// 订金编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 学生编号（TblCstStudent）
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long StudentId { get; set; }

        /// <summary>
        /// 学生编号
        /// </summary>
        public string StudentNo { get; set; }

        /// <summary>
        /// 归属校区
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 付款日期
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// 1:预报名 2:游学营定金 3:写生定金
        /// </summary>
        public int UsesType { get; set; }

        /// <summary>
        /// 1:预报名 2:游学营定金 3:写生定金
        /// </summary>
        public string UsesTypeName { get; set; }

        /// <summary>
        /// 定金
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 收银人编号
        /// </summary>
        public string PayeeId { get; set; }

        /// <summary>
        /// 收银人
        /// </summary>
        public string Payee { get; set; }

        /// <summary>
        /// 1:刷卡 2:现金 3:转账 4:微信 5:支付宝 9:其他
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 付款方式名称
        /// </summary>
        public string PayTypeName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary> 
        /// -1订单取消, 0待付款,1已付款/正常 10 已完成
        /// </summary>
        public int OrderStatus { get; set; }

        /// <summary>
        /// 订单状态名称
        /// </summary>
        public string OrderStatuName { get; set; }

        /// <summary>
        /// 新生/老生
        /// </summary>
        public string OrderNewTypeName { get; set; }

        /// <summary>
        /// 作废操作人
        /// </summary>
        public string CancelUserId { get; set; }

        /// <summary>
        /// 作废操作人名称
        /// </summary>
        public string CancelUserName { get; set; }

        /// <summary>
        /// 作废时间
        /// </summary>
        public DateTime? CancelDate { get; set; }

        /// <summary>
        /// 作废原因
        /// </summary>
        public string CancelRemark { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 校区名称
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public string Age { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string LinkMobile { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        [JsonIgnore]
        public int IDType { get; set; }

        /// <summary>
        /// 证件类型名称
        /// </summary>
        public string IDTypeName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string SexName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string HeadFaceUrl { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string IDNumber { get; set; }

        /// <summary>
        /// 监护人信息
        /// </summary>
        public List<GuardianRequest> ContactPerson { get; set; }

        /// <summary>
        /// 打印序号
        /// </summary>
        public string PrintNumber { get; set; }

        /// <summary>
        /// 新生/老生
        /// </summary>
        public int OrderNewType { get; set; }

        /// <summary>
        /// 收款交接
        /// </summary>
        public FinOrderHandoverResponse FinOrderHandoverList { get; set; }
    }
}
