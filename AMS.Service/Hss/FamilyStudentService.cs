/****************************************************************************\
所属系统:招生系统
所属模块:家校互联
创建时间:
作   者:
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/
using AMS.Anticorrosion.HRS;
using AMS.Core;
using AMS.Dto.Hss;
using AMS.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMS.Service
{
    /// <summary>
    /// 家长的学生服务
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-08</para>
    /// </summary>
    public class FamilyStudentService
    {
        private readonly string _userCode;


        /// <summary>
        /// 根据家长账号实例化一个家长学生服务
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-08</para>
        /// </summary>
        /// <param name="userCode">家长账号</param>
        public FamilyStudentService(string userCode)
        {
            _userCode = userCode;
        }

        /// <summary>
        /// 获取家长的学生
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-08</para>
        /// </summary>
        /// <returns>学生信息集合</returns>
        public List<StudentsResponse> GetStudents()
        {
            List<StudentsResponse> result = new List<StudentsResponse>();

            //1、获取家长的学生信息
            List<TblCstStudent> students = StudentService.GetStudentList(this._userCode)
                                                        .Distinct()
                                                        .ToList();
            List<long> studentIds = students.Select(t => t.StudentId).ToList();


            //2、取学生对应的校区
            List<TblCstSchoolStudent> schoolStudents = StudentService.GetStudentSchoolList(studentIds);

            //3、获取校区信息
            OrgService orgService = new OrgService();
            List<SchoolResponse> allSchools = orgService.GetAllSchoolList();

            //4、按学生最近要上的课程进行排序
            List<ViewCompleteStudentAttendance> studentAttendance = StudentTimetableService.GetStudentAttendLately(studentIds);

            //5、整合学生的数据到输出结果
            students.ForEach(item =>
            {
                StudentsResponse resultItem = new StudentsResponse()
                {
                    HeadFaceUrl = item.HeadFaceUrl,
                    StudentId = item.StudentId,
                    StudentName = item.StudentName,
                };
                resultItem.SchoolList = new List<StudentSchoolListResponse>();
                var query = from ss in schoolStudents
                            join school in allSchools on ss.SchoolId equals school.SchoolId
                            where ss.StudentId == item.StudentId
                            select new StudentSchoolListResponse()
                            {
                                CompanyId = school.CompanyId,
                                CompanyName = school.Company,
                                SchoolId = school.SchoolId,
                                SchoolName = school.SchoolName,
                                ServerDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                ClassLatelyTime=this.GetClassLatelyTime(school.SchoolId, item.StudentId, studentAttendance)
                            };
                resultItem.SchoolList = query.ToList();
                result.Add(resultItem);
            });

            //6、按最近上课时间排序
            result = result.Where (t=>t.SchoolList.Count>0).OrderBy(t => t.SchoolList.OrderBy(x=>x.ClassLatelyTime, new EmptyStringsAreLast())
                                                     .FirstOrDefault()
                                                     .ClassLatelyTime,new EmptyStringsAreLast())
                            .ToList();
            return result;
        }


        /// <summary>
        /// 根据校区和学生ID
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-18</para>
        /// </summary>
        /// <param name="schoolId">学生所在校区ID</param>
        /// <param name="studentId">学和ID</param>
        /// <param name="studentAttendance">学生最近的所有考勤信息集合</param>
        /// <returns></returns>
        private string GetClassLatelyTime(string schoolId, long studentId, List<ViewCompleteStudentAttendance> studentAttendance)
        {
            ViewCompleteStudentAttendance attendance = studentAttendance.Where(t => t.SchoolId == schoolId && t.StudentId == studentId).FirstOrDefault();
            if (attendance!=null)
            {
                return $"{attendance.ClassDate.ToString("yyyy-MM-dd")} {attendance.ClassBeginTime}";
            }
            return "";
        }
    }
}
