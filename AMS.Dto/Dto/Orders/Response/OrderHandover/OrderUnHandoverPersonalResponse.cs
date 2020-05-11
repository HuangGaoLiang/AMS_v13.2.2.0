using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 招生专员对应的未交接列表
    /// </summary>
    public class OrderUnHandoverPersonalResponse
    {
        /// <summary>
        /// 收银员名称
        /// </summary>
        public string PersonalName { get; set; }

        /// <summary>
        /// 校区名称
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 订单未交接明细
        /// </summary>
        public List<OrderUnHandoverListResponse> Data { get; set; } = new List<OrderUnHandoverListResponse>();
    }
}
