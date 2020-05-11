/*此代码由生成工具字段生成，生成时间2019/3/7 16:54:48 */
using System;

namespace AMS.Storage.Models
{
     /// <summary>
     /// 课次基础信息表
     /// </summary>
    public partial class TblTimLessonStudent
     {
          /// <summary>
          /// 主健(课次基础信息表)课次ID
          /// </summary>
         public long LessonStudentId  { get; set; }

          /// <summary>
          /// 校区ID
          /// </summary>
         public string SchoolId  { get; set; }

          /// <summary>
          /// 学生ID
          /// </summary>
         public long StudentId  { get; set; }

          /// <summary>
          /// 课次ID
          /// </summary>
         public long LessonId  { get; set; }

          /// <summary>
          /// 考勤状态(0正常签到 1补签 2请假)
          /// </summary>
         public int AttendStatus  { get; set; }

          /// <summary>
          /// 创建时间
          /// </summary>
         public DateTime CreateTime  { get; set; }

        /// <summary>
        /// 该字段为冗余字段，TblTimReplenishLesson,1已经安排补课2已安排调课,
        /// </summary>
        public int AdjustType { get; set; }

          /// <summary>
          /// 考勤补签码
          /// </summary>
         public string ReplenishCode  { get; set; }

          /// <summary>
          /// 签到人员类型 1老师9财务
          /// </summary>
         public int AttendUserType  { get; set; }

          /// <summary>
          /// 考勤时间
          /// </summary>
         public DateTime? AttendDate  { get; set; }

          /// <summary>
          /// 更新时间
          /// </summary>
         public DateTime? UpdateTime  { get; set; }

     }
}
