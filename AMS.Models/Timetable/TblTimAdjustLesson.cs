/*此代码由生成工具字段生成，生成时间2019/3/6 16:11:58 */
using System;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 调整课次业务表
    /// </summary>
    public partial class TblTimAdjustLesson
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long AdjustLessonId { get; set; }

        /// <summary>
        /// 校区ID
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public long BatchNo { get; set; }

        /// <summary>
        /// 源课次ID
        /// </summary>
        public long FromLessonId { get; set; }

        /// <summary>
        /// 源老师
        /// </summary>
        public string FromTeacherId { get; set; }

        /// <summary>
        /// 新老师
        /// </summary>
        public string ToTeacherId { get; set; }

        /// <summary>
        /// 上课学生
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 上课教室
        /// </summary>
        public long ClassRoomId { get; set; }

        /// <summary>
        /// 上课班级
        /// </summary>
        public long ClassId { get; set; }

        /// <summary>
        /// 上课时间段ID
        /// </summary>
        public long SchoolTimeId { get; set; }

        /// <summary>
        /// 上课日期
        /// </summary>
        public DateTime ClassDate { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public string ClassBeginTime { get; set; }

        /// <summary>
        /// 下课时间
        /// </summary>
        public string ClassEndTime { get; set; }

        /// <summary>
        /// 业务类型 3补课 5补课周补课 6调课 7插班补课 8老师代课 9全校上课日期调整 10班级上课时间调整
        /// </summary>
        public int BusinessType { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态 （1正常 -1无效）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}
