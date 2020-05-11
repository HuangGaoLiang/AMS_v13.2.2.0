using System;
using System.Collections.Generic;
using System.Linq;
using AMS.Anticorrosion.HRS;
using AMS.Core;
using AMS.Dto;

namespace AMS.Service
{
    /// <summary>
    /// 教师服务
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-02-20</para>
    /// </summary>
    public static class TeachService
    {
        /// <summary>
        /// 获取所有老师信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-20</para>
        /// </summary>
        /// <returns></returns>
        internal static List<ClassTimetableTeacherResponse> GetTeachers()
        {
            var res = EmployeeService.GetAll().Select(x => new ClassTimetableTeacherResponse
            {
                TeacherId = x.EmployeeId,
                TeacherName = x.EmployeeName
            }).OrderBy(x => x.TeacherName, new NaturalStringComparer()).ToList();

            return res;
        }

        /// <summary>
        /// 根据老师Id获取老师信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-16</para>
        /// </summary>
        /// <param name="teacherId">老师Id</param>
        /// <returns>上课老师</returns>
        internal static ClassTimetableTeacherResponse GetTeacher(string teacherId)
        {
            return GetTeachers().FirstOrDefault(x => x.TeacherId == teacherId);
        }

        /// <summary>
        /// 描述:获取老师（包含离职N个月的老师）
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-3-11</para>
        /// </summary>
        /// <param name="schoolId">老师所在校区</param>
        /// <param name="dimissionDay">包含离职多少天内的老师</param>
        /// <returns>老师信息列表</returns>
        public static List<ClassTimetableTeacherResponse> GetTeachers(string schoolId, int dimissionDay)
        {
            var currentDate = DateTime.Now;
            //获取在职老师列表
            var incumbentTeacherList = GetIncumbentTeachers(schoolId);
            //离职三个月的老师
            var leaveTeacherList = EmployeeService.GetAllBySchoolId(schoolId).Where(x => x.Status == (int)PersonnelStatus.Resignation && x.LeaveDate >= currentDate.AddDays(-dimissionDay))
                .Select(x => new ClassTimetableTeacherResponse
                {
                    TeacherId = x.EmployeeId,
                    TeacherName = x.EmployeeName
                });
            incumbentTeacherList.AddRange(leaveTeacherList);

            return incumbentTeacherList;
        }

        /// <summary>
        /// 获取所有在职老师信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-18</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <returns>上课老师列表</returns>
        public static List<ClassTimetableTeacherResponse> GetIncumbentTeachers(string schoolId)
        {
            var res = EmployeeService.GetAllBySchoolId(schoolId)
                        .Where(x => x.Status == (int)PersonnelStatus.Incumbent)
                        .Select(x => new ClassTimetableTeacherResponse
                        {
                            TeacherId = x.EmployeeId,
                            TeacherName = x.EmployeeName
                        })
                        .OrderBy(x => x.TeacherName, new NaturalStringComparer())
                        .ToList();

            return res;
        }

        /// <summary>
        /// 获取所有在职老师信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-18</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="departureTeacherId">离职老师Id</param>
        /// <returns>上课老师列表</returns>
        internal static List<ClassTimetableTeacherResponse> GetIncumbentTeachers(
            string schoolId, string departureTeacherId)
        {
            //在职老师
            var incumbent = GetIncumbentTeachers(schoolId);

            if (!incumbent.Any(x => x.TeacherId == departureTeacherId))
            {
                //离职老师
                var leaveTeacher = EmployeeService.GetByEmployeeId(departureTeacherId);
                if (leaveTeacher != null)
                {
                    incumbent.Add(new ClassTimetableTeacherResponse
                    {
                        TeacherId = leaveTeacher.EmployeeId,
                        TeacherName = leaveTeacher.EmployeeName
                    });
                }
            }
            return incumbent.OrderBy(x => x.TeacherName, new NaturalStringComparer()).ToList();
        }
    }
}
