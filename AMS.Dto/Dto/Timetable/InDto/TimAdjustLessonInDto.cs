/*此代码由生成工具字段生成，生成时间2019/3/6 16:11:58 */
using System;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：补课周补课信息查询
    /// <para>作    者：Huang GaoLiang</para>
    /// <para>创建时间：2019-03-22</para>
    /// </summary>
    public class TimAdjustLessonInDto
    {
        /// <summary>
        /// 班级编号
        /// </summary>
        public long ClassId { get; set; }

        /// <summary>
        /// 校区ID
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 新老师
        /// </summary>
        public string ToTeacherId { get; set; }

        /// <summary>
        /// 业务类型 3补课 5补课周补课 6调课 7插班补课 8老师代课 9全校上课日期调整 10班级上课时间调整
        /// </summary>
        public ProcessBusinessType BusinessType { get; set; }

        /// <summary>
        /// 状态 （1正常 -1无效）
        /// </summary>
        public TimAdjustLessonStatus? Status { get; set; }

    }
}
