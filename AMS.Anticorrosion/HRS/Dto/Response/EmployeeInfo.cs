using System;

namespace AMS.Anticorrosion.HRS
{
    /// <summary>
    /// 员工信息
    /// </summary>
    internal class EmployeeInfo
    {
        /// <summary>
        /// 公司编号
        /// </summary>
        public string CompanyId { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 城市Id
        /// </summary>
        public string CityId { get; set; }

        /// <summary>
        /// 组织Id
        /// </summary>
        public string OrgId { get; set; }

        /// <summary>
        /// 组织名称
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public string DeptId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 校区ID
        /// </summary>
        public int SchoolSno { get; set; }

        /// <summary>
        /// 职位ID
        /// </summary>
        public string PositionId { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// 用户工号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserSno { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string IdentityCardNo { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string HeadPic { get; set; }

        /// <summary>
        /// 人员信息核对状态
        /// </summary>
        public byte CheckStatus { get; set; }

        /// <summary>
        /// 水印文字
        /// </summary>
        public string Watermark { get; set; }

        /// <summary>
        /// 员工编号
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// 员工名称
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 人员在职状态 0:在职 1:离职
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 离职日期
        /// </summary>
        public DateTime? LeaveDate { get; set; }
    }
}
