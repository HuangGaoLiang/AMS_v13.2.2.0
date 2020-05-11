using System;
using System.Collections.Generic;
using System.Linq;
using AMS.Core;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;

namespace AMS.Service
{
    /// <summary>
    /// 课次服务基类(所有操作课次的服务类必须继承此类)
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-13</para>
    /// </summary>
    public abstract class BaseLessonService
    {
        /// <summary>
        /// 学生考勤仓储(正常/补课/调课)
        /// </summary>
        protected readonly ViewCompleteStudentAttendanceRepository ViewCompleteStudentAttendanceRepository;

        /// <summary>
        /// 课次服务基类
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="unitOfWork">事物单元</param>
        protected BaseLessonService(UnitOfWork unitOfWork)
        {
            this.ViewCompleteStudentAttendanceRepository =
                unitOfWork.GetCustomRepository<ViewCompleteStudentAttendanceRepository, ViewCompleteStudentAttendance>();
        }

        /// <summary>
        /// 上课时间校验实体
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        protected class VerifyClassTime
        {
            /// <summary>
            /// 上课日期
            /// </summary>
            public DateTime ClassDate { get; set; }

            /// <summary>
            /// 上课开始时间
            /// </summary>
            public string ClassBeginTime { get; set; }

            /// <summary>
            /// 上课结束时间
            /// </summary>
            public string ClassEndTime { get; set; }
        }

        /// <summary>
        /// 校验学生上课时间是否冲突
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="schoolId">校区ID</param>
        /// <param name="studentId">学生ID</param>
        /// <param name="verifyClassTimes">待校验的上课时间段</param>
        /// <param name="lessonType">课次类型</param>
        /// <exception cref="BussinessException">
        /// 异常ID:32 异常描述：与XX班级时间段冲突
        /// </exception>
        protected virtual void VerifyClassTimeCross(
            string schoolId, long studentId, List<VerifyClassTime> verifyClassTimes, LessonType lessonType)
        {
            if (lessonType != LessonType.RegularCourse)
            {
                return;
            }

            List<int> adjustTypes = new List<int> {
                (int)AdjustType.DEFAULT,                //默认
                (int)AdjustType.SUPPLEMENTNOTCONFIRMED, //补签未确认
                (int)AdjustType.SUPPLEMENTCONFIRMED     //补签已确认
            };

            //学生上课时间
            var stuClassTimeList = this.ViewCompleteStudentAttendanceRepository
                .GetStudentAttendList(schoolId, studentId, adjustTypes, new List<int> { (int)LessonType.RegularCourse })
                .Select(m => new StudentTimetableDto
                {
                    ClassId = m.ClassId,
                    ClassBeginTime = DateTime.Parse($"{m.ClassDate:yyyy-MM-dd} {m.ClassBeginTime}"),
                    ClassEndTime = DateTime.Parse($"{m.ClassDate:yyyy-MM-dd} {m.ClassEndTime}")
                }).ToList();


            foreach (var classTime in verifyClassTimes)
            {
                //今天学生上课时间
                var todayStuClassTimeList = stuClassTimeList
                    .Where(x => x.ClassBeginTime.Date == classTime.ClassDate.Date)
                    .ToList();

                if (todayStuClassTimeList.Count == 0)
                {
                    continue;
                }

                DateTime sTime = DateTime.Parse($"{classTime.ClassDate:yyyy-MM-dd} {classTime.ClassBeginTime}");
                DateTime eTime = DateTime.Parse($"{classTime.ClassDate:yyyy-MM-dd} {classTime.ClassEndTime}");
                foreach (var item in todayStuClassTimeList)
                {
                    if ((item.ClassBeginTime <= sTime && sTime <= item.ClassEndTime) || (sTime <= item.ClassBeginTime && item.ClassBeginTime <= eTime))
                    {
                        var datClass = new DefaultClassService(item.ClassId).TblDatClass;
                        if (datClass != null)
                        {
                            throw new BussinessException(ModelType.Timetable, 32, $"与{datClass.ClassNo}班级上课时间冲突");
                        }
                        else
                        {
                            //班级上课时间冲突
                            throw new BussinessException(ModelType.Timetable, 32);
                        }
                    }
                }
            }
        }
    }
}
