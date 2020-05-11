/****************************************************************************\
所属系统：招生系统
所属模块：课表模块
创建时间：2019-03-06
作    者：郭伟佳
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/


using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：学生考勤详细列表实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-06</para>
    /// </summary>
    public class StudentTimeLessonListResponse
    {
        /// <summary>
        /// 学生常规课Id
        /// </summary>
        public string StudentLessonIds { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public string ClassDate { get; set; }

        /// <summary>
        /// 考勤状态
        /// </summary>
        public int AttendStatus { get; set; }

        /// <summary>
        /// 考勤状态名称
        /// </summary>
        public string AttendStatusName { get; set; }

        /// <summary>
        /// 耗用课次
        /// </summary>
        public int LessonCount { get; set; }

        /// <summary>
        /// 课程状态：1已考勤2已经安排补课3已安排调课
        /// </summary>
        public int LessonStatus { get; set; }

        /// <summary>
        /// 课程状态名称：1已考勤2已经安排补课3已安排调课
        /// </summary>
        public string LessonStatusName { get; set; }
    }
}