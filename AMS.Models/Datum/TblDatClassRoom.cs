using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 教室
    /// </summary>
    public partial class TblDatClassRoom
    {
        /// <summary>
        /// 主键 教室
        /// </summary>
        public long ClassRoomId { get; set; }
        /// <summary>
        /// 所属校区编号
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 门牌号
        /// </summary>
        public string RoomNo { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
