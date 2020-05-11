using System;
using Jerrisoft.Platform.Public.PageExtensions;

namespace AMS.Dto
{
    /// <summary>
    /// 转校列表查询条件
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class ChangeSchoolOrderListSearchRequest : Page,IOrderListSearchRequest
    {
        /// <summary>
        /// 本校转出状态:{null:全部,-2:已拒绝,-1:已撤销,1:待确认,2:待接收,9:已接收}
        /// 他校转入状态:{null:全部,-2:已拒绝,2:待接收,9:已接收}
        /// </summary>
        public OrderStatus? OrderStatus { get; set; }

        /// <summary>
        /// 提交开始时间
        /// </summary>
        public DateTime? STime { get; set; }

        /// <summary>
        /// 提交结束时间
        /// </summary>
        public DateTime? ETime { get; set; }

        /// <summary>
        /// 关键词搜索 学生姓名或手机号码
        /// </summary>
        public string Key { get; set; }
    }
}
