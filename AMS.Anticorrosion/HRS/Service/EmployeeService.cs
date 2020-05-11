using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jerrisoft.Platform.Log;
using YMM.HRS.SDK;

namespace AMS.Anticorrosion.HRS
{
    /// <summary>
    /// 员工信息
    /// </summary>
    public static class EmployeeService
    {
        /// <summary>
        /// 日志记录
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-01</para>
        /// </summary>
        /// <param name="action">方法</param>
        /// <param name="actionDesc">描述</param>
        /// <param name="msg">异常简述</param>
        /// <param name="e">异常信息</param>
        private static void RequestLog(string action, string actionDesc, string msg, Exception e)
        {
            StringBuilder error = new StringBuilder();
            error.Append($"方法:{action}");
            error.Append($"描述:{actionDesc}");
            error.Append($"异常:{msg}");
            error.Append($"{e}");

            LogWriter.Write("EmployeeService", error.ToString(), LoggerType.Debug);
        }

        /// <summary>
        /// 获取人事系统所有人员信息进行转换
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-01-14</para>
        /// </summary>
        /// <returns>人员信息列表</returns>
        private static List<EmployeeInfo> GetAllConvertEmployee()
        {
            HrSystem hrSystem = new HrSystem();
            try
            {
                //未知查询条件,查询所有转换再处理
                var personList = hrSystem.GetPersonnelDataByCompanyId(new List<string>());

                //当人员记录数为0时记录日记，以便跟踪
                if (personList == null || personList.Count <= 0)
                {
                    RequestLog("EmployeeService->GetAllConvertEmployee", $"YMM.HRS.SDK.HrSystem 人员信息获为0条", "", null);
                }

                var result = personList.Select(x => new EmployeeInfo
                {
                    BirthDate = x.BirthDate,
                    CheckStatus = x.CheckStatus,
                    CityId = x.DepartmentInfo.CityId,
                    Company = x.DepartmentInfo.Company,
                    CompanyId = x.DepartmentInfo.CompanyId,
                    DeptId = x.DepartmentInfo.DeptId,
                    DeptName = x.DepartmentInfo.DeptName,
                    Email = x.Email,
                    EmployeeId = x.UserId,
                    EmployeeName = x.UserName,
                    HeadPic = x.HeadPic,
                    IdentityCardNo = x.IdentityCardNo,
                    Mobile = x.Mobile,
                    Number = x.Number,
                    OrgId = x.DepartmentInfo.OrgId,
                    OrgName = x.DepartmentInfo.OrgName,
                    PositionId = x.DepartmentInfo.PositionId,
                    PositionName = x.DepartmentInfo.PositionName,
                    SchoolSno = x.DepartmentInfo.SchoolSno,
                    Sex = x.Sex,
                    Status = x.Status,
                    UserSno = x.UserSno,
                    Watermark = x.Watermark,
                    LeaveDate = x.LeaveDate
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                RequestLog("EmployeeService->GetAllConvertEmployee", "YMM.HRS.SDK.HrSystem 人员信息获取失败", ex.Message, ex);
                throw new ApplicationException("人员信息获取失败");
            }
        }

        /// <summary>
        /// 获取所有人员信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-01-14</para>
        /// </summary>
        /// <returns>所有人员信息列表</returns>
        public static List<EmployeeResponse> GetAll()
        {
            return GetAllConvertEmployee().ToEmployeeResponseList();
        }

        /// <summary>
        /// 获取该校区的业绩归属人(业绩归属人=在职人员)
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-01-14</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <returns>业绩归属人列表</returns>
        public static List<EmployeeResponse> GetPerformanceOwnerBySchoolId(string schoolId)
        {
            return GetAllConvertEmployee()
                .Where(x => x.OrgId == schoolId && x.Status == 0)//0:在职 1:离职
                .ToEmployeeResponseList();
        }

        /// <summary>
        /// 获取校区所有人员信息(包括离职)
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-01-14</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <returns>人员信息列表</returns>
        public static List<EmployeeResponse> GetAllBySchoolId(string schoolId)
        {
            return GetAllConvertEmployee()
                .Where(x => x.OrgId == schoolId)
                .ToEmployeeResponseList();
        }

        /// <summary>
        /// 获取员工信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-01-14</para>
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <returns>员工信息</returns>
        public static EmployeeResponse GetByEmployeeId(string employeeId)
        {
            return GetAllConvertEmployee()
                     .Where(x => x.EmployeeId == employeeId)
                     .ToEmployeeResponseList()
                     .FirstOrDefault();
        }

        /// <summary>
        /// EmployeeInfo列表转EmployeeResponse列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-01-14</para>
        /// </summary>
        /// <param name="source">EmployeeInfo列表</param>
        /// <returns>EmployeeResponse 列表</returns>
        private static List<EmployeeResponse> ToEmployeeResponseList(this IEnumerable<EmployeeInfo> source)
        {
            return source.Select(m => new EmployeeResponse
            {
                EmployeeId = m.EmployeeId,
                EmployeeName = m.EmployeeName,
                Status = m.Status,
                LeaveDate = m.LeaveDate
            }).ToList();
        }
    }
}
