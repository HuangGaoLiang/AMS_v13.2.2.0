namespace AMS.Core
{
    public class AppsettingsConfig
    {
        public ConnectionStrings ConnectionStrings { get; set; }

        /// <summary>
        /// 学期审核列表跳转后的详情地址
        /// </summary>
        public string TermAuditShowUrl { get; set; }
        /// <summary>
        /// 校区课表审核列表跳转后的详情地址
        /// </summary>
        public string TermCourseTimetableAuditShowUrl { get; set; }
        /// <summary>
        /// 校区学期类型
        /// </summary>
        public SchoolTermType TermType { get; set; }
        /// <summary>
        /// 任务调度的密钥
        /// </summary>
        public string JobAccessSecret { get; set; }

        /// <summary>
        /// 转校备注配置项
        /// </summary>
        public TransferSchool TransferSchool => new TransferSchool();

        /// <summary>
        /// K信老师扫码考勤配置
        /// </summary>
        public ScanCodeAttend ScanCodeAttend { get; set; }

        /// <summary>
        /// RocketMQ配置
        /// </summary>
        public RocketMQConfig RocketMQConfig { get; set; }

        /// <summary>
        /// 队例名称
        /// </summary>
        public BusinessQueueName BusinessQueueName { get; set; }

    }

    public class ConnectionStrings
    {
        public string DefaultContext { get; set; }
    }


    /// <summary>
    /// 队例名称
    /// </summary>
    public class BusinessQueueName
    {
        /// <summary>
        /// 家长账户
        /// </summary>
        public string NameOfHssPassport { get; set; }
        /// <summary>
        /// 微信通知
        /// </summary>
        public string NameOfWxNotify { get; set; }
    }


    /// <summary>
    /// RocketMQ配置
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-12</para>
    /// </summary>
    public class RocketMQConfig
    {
        /// <summary>
        /// AccessKey
        /// </summary>
        public string AccessKey { get; set; }
        /// <summary>
        /// SecretKey
        /// </summary>
        public string SecretKey { get; set; }
        /// <summary>
        /// 消息组ID
        /// </summary>
        public string GroupId { get; set; }
        /// <summary>
        /// 服务地址
        /// </summary>
        public string NameSrv { get; set; }
    }


    /// <summary>
    /// 描    述：校区学期类型
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-02-21</para>
    /// </summary>
    public class SchoolTermType
    {
        /// <summary>
        /// 春季
        /// </summary>
        public long Spring { get; set; }

        /// <summary>
        /// 夏季
        /// </summary>
        public long Summer { get; set; }

        /// <summary>
        /// 秋季
        /// </summary>
        public long Autumn { get; set; }

        /// <summary>
        /// 冬季
        /// </summary>
        public long Winter { get; set; }
    }

    /// <summary>
    /// 转校备注配置项
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-06</para>
    /// </summary>
    public class TransferSchool
    {
        /// <summary>
        /// 转校转出
        /// </summary>
        public string Out => "转校转出";

        /// <summary>
        /// 转校接收
        /// </summary>
        public string Receive => "转校接收";

        /// <summary>
        /// 转校撤销
        /// </summary>
        public string Cancel => "转校撤销";

        /// <summary>
        /// 转校拒绝
        /// </summary>
        public string Refuse => "转校拒绝";
    }

    /// <summary>
    /// K信老师扫码考勤配置
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-06</para>
    /// </summary>
    public class ScanCodeAttend
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 缓冲时间
        /// </summary>
        public int BufferTime { get; set; }
    }
}
