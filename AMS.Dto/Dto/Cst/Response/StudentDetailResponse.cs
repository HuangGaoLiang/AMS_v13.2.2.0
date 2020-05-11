using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述: 学生的详情信息类
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class StudentDetailResponse
    {
        /// <summary>
        /// 主健(TblCstStudent学生信息)
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long StudentId { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNo { get; set; }

        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 性别(1男2女)
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 性别名称
        /// </summary>
        public string SexName { get; set; }

        /// <summary>
        /// 出生年月
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 获取学生年龄
        /// </summary>
        public string Age { get; set; }

        /// <summary>
        /// 头像地址
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
        public int IDType { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public string IdTypeName { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string IDNumber { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
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
        /// 监护人手机
        /// </summary>
        public string ContactPersonMobile { get; set; }

        /// <summary>
        /// 名单来源编号
        /// </summary>
        public int CustomerFromNumber { get; set; }
        /// <summary>
        /// 名单来源(1自然到访 2线下市场活动 3线上活动 4地推 5异业合作 6 400电话)
        /// </summary>
        public string CustomerFrom { get; set; }

        /// <summary>
        /// 推荐人
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 学生余额
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
