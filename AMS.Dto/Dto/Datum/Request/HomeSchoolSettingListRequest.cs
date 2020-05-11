using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：家校互联配置请求类
    /// <para>作者：瞿琦</para>
    /// <para>创建时间:2019-3-13</para>
    /// </summary>
    public class HomeSchoolSettingListRequest
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        [Required(ErrorMessage = "请选择公司")]
        public string CompanyId { get; set; }

        /// <summary>
        /// 功能Id
        /// </summary>
        [Required(ErrorMessage = "请选择要设置的功能")]
        public int FuntionId { get; set; }

        /// <summary>
        /// 数据Id
        /// </summary>
        public string DataId { get; set; }

        /// <summary>
        /// 数据名称
        /// </summary>
        public string DataValue { get; set; }
    }
}
