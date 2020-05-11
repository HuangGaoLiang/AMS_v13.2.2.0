using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述: 学生注册类
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class StudentRegisterRequest
    {
        /// <summary>
        /// 学生姓名
        /// </summary>
        [Required]
        public string StudentName { get; set; }

        /// <summary>
        /// 性别(1男2女)
        /// </summary>
        [Required]
        public SexEnum Sex { get; set; }

        /// <summary>
        /// 出生年月
        /// </summary>
        [Required]
        public DateTime Birthday { get; set; }
        /// <summary>
        /// 方形头像地址
        /// </summary>
        public string HeadFaceUrl { get; set; }
        /// <summary>
        /// 就读学校
        /// </summary>
        public string CurrentSchool { get; set; }

        /// <summary>
        /// 所在区域(存储的是区域ID,可以推算出省市)
        /// </summary>
        public string AreaId { get; set; }

        /// <summary>
        /// 家庭详细地址
        /// </summary>
        public string HomeAddress { get; set; }

        /// <summary>
        /// 家庭详细地址(原始地址)
        /// </summary>
        public HomeAddressFormatRequest HomeAddressFormat { get; set; }

        /// <summary>
        /// 1身份证 2护照 3港澳通行证 4台胞证
        /// </summary>
        public DocumentType IDType { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string IDNumber { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Required]
        public string LinkMobile { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string LinkMail { get; set; }

        /// <summary>
        /// 监护人信息
        /// </summary>
        public List<GuardianRequest> ContactPerson { get; set; }

        /// <summary>
        /// 名单来源(1：自然到访 2：线下市场活动 3：线上活动 4地推 5：异业合作 6：400电话)
        /// </summary>
        [Required]
        public ListSource CustomerFrom { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 推荐人
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 校区编号
        /// </summary>
        [JsonIgnore]
        public string SchoolId { get; set; }

        /// <summary>
        /// 操作人编号
        /// </summary>
        [JsonIgnore]
        public string UserId { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        [JsonIgnore]
        public string UserName { get; set; }

        /// <summary>
        /// 业务编号
        /// </summary>
        //[JsonIgnore]
        //public long BusinessId { get; set; }

        /// <summary>
        /// 学生编号
        /// </summary>
        //[JsonIgnore]
        //public long StudentId { get; set; }
    }
}
