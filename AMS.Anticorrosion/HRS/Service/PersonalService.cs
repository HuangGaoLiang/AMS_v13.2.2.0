using Jerrisoft.Platform.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YMM.HRS.SDK;
using YMM.HRS.SDK.Dto;

namespace AMS.Anticorrosion.HRS
{
    /// <summary>
    /// 人事
    /// </summary>
    public class PersonalService : HRSService
    {
        private readonly string _schoolId;

        public PersonalService(string schoolId)
        {
            this._schoolId = schoolId;
        }

        private static void RequestLog(string action, string actionDesc, string msg, Exception e)
        {
            StringBuilder error = new StringBuilder();
            error.Append($"方法:{action}");
            error.Append($"描述:{actionDesc}");
            error.Append($"异常:{msg}");
            error.Append($"{e}");

            LogWriter.Write("PersonalService", error.ToString(), LoggerType.Debug);
        }

        /// <summary>
        /// 获取人员信息
        /// </summary>
        /// <returns></returns>
        public static List<SchoolEmployeeResponse> GetAll()
        {
            List<SchoolEmployeeResponse> res = new List<SchoolEmployeeResponse>();
            try
            {
                res = new HrSystem()
                          .GetPersonnelDataByCompanyId(new List<string>())
                          .Select(m => new SchoolEmployeeResponse
                          {
                              EmployeeId = m.UserId,
                              EmployeeName = m.UserName,
                              Status = m.Status,
                          })
                          .ToList();
            }
            catch (Exception e)
            {
                RequestLog("PersonalService->GetAll", "YMM.HRS.SDK 人员信息获取失败", e.Message, e);
                throw new ApplicationException("人员信息获取失败");
            }

            return res;
        }

        public static SchoolEmployeeResponse GetByEmployeeId(string uId)
        {
            SchoolEmployeeResponse res = null;
            try
            {
                res = new HrSystem()
                       .GetPersonnelDataByPersonnelId(new List<string> { uId })
                       .Select(m => new SchoolEmployeeResponse
                       {
                           EmployeeId = m.UserId,
                           EmployeeName = m.UserName,
                           Status = m.Status
                       })
                       .FirstOrDefault();
            }
            catch (Exception e)
            {
                RequestLog("PersonalService->GetByEmployeeId", "YMM.HRS.SDK 人员信息获取失败", e.Message, e);
                throw new ApplicationException("人员信息获取失败");
            }

            return res;
        }

        public List<SchoolEmployeeResponse> GetBySchoolId()
        {
            List<SchoolEmployeeResponse> res = new List<SchoolEmployeeResponse>();

            try
            {
                res = new HrSystem()
                          .GetPersonnelDataByCompanyId(new List<string>())
                          .Where(x => x.DepartmentInfo.OrgId == _schoolId)
                          .Select(m => new SchoolEmployeeResponse
                          {
                              EmployeeId = m.UserId,
                              EmployeeName = m.UserName,
                              Status = m.Status,
                          })
                          .ToList();
            }
            catch (Exception e)
            {
                base.AntWriteLog("PersonalService->GetBySchoolId", "根据校区下的人员信息", "YMM.HRS.SDK", e.Message, e);
                throw new ApplicationException("人员信息获取失败");
            }

            return res;
        }

        ///  <summary>
        /// 获取业绩归属人
        ///  by:Huang GaoLiang 2018年11月1日
        ///  </summary>
        /// <returns></returns>
        public List<SchoolEmployeeResponse> GetEmployees()
        {
            return GetSchoolEmployee();
        }

        /// <summary>
        ///获取业绩归属人
        /// by:Huang GaoLiang 2018年11月1日
        /// </summary>
        /// <param name="userId">当前操作人编号</param>
        /// <returns></returns>
        public List<SchoolEmployeeResponse> GetEmployees(string userId = null)
        {
            List<SchoolEmployeeResponse> res = GetSchoolEmployee(userId).Where(m => m.Status == 0).ToList();
            return res;
        }

        private List<SchoolEmployeeResponse> GetSchoolEmployee(string userId = null)
        {
            List<EmployeeInfo> list = this.GetSchoolEmployeeByCompanyId(userId);
            List<SchoolEmployeeResponse> resultList = list.Select(m => new SchoolEmployeeResponse
            {
                EmployeeId = m.EmployeeId,
                EmployeeName = m.EmployeeName,
                Status = m.Status
            }).ToList();

            return resultList;
        }

        #region GetPersonnelDataByCompanyId
        /// <summary>
        /// 根据组织架构获取用户信息
        /// by:Huang GaoLiang 2018年11月1日
        /// </summary>
        /// <param name="userId">当前操作人编号</param>
        /// <returns></returns>
        public List<EmployeeInfo> GetSchoolEmployeeByCompanyId(string userId = null)
        {
            List<EmployeeInfo> employeeList = new List<EmployeeInfo>();
            if (string.IsNullOrWhiteSpace(_schoolId))
            {
                return employeeList;
            }
            try
            {
                List<string> schoolIds = new List<string>
                {
                    _schoolId
                };
                HrSystem rr = new HrSystem();
                List<string> s = new List<string>();
                List<PersonnelOfDeptData> res = rr.GetPersonnelDataByCompanyId(s);

                foreach (PersonnelOfDeptData item in res)
                {
                    if (!schoolIds.Contains(item.DepartmentInfo.OrgId)) continue;
                    EmployeeInfo info = new EmployeeInfo();
                 
                    info.CompanyId = item.DepartmentInfo.CompanyId;
                    info.Company = item.DepartmentInfo.Company;
                    info.EmployeeId = item.UserId;
                    info.EmployeeName = item.UserName;
                    info.Status = item.Status;
                    employeeList.Add(info);
                }
            }
            catch (Exception e)
            {
                base.AntWriteLog("GetPersonnelDataByCompanyId", "根据组织架构获取用户信息", "", e.Message, e);
            }

            employeeList = employeeList.Where((x, i) => employeeList.FindIndex(z => z.EmployeeId == x.EmployeeId) == i).ToList();
            return employeeList;
        }
        #endregion
    }
}
