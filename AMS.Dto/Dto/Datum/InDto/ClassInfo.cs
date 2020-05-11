/****************************************************************************\
所属系统:运营中心
所属模块:权益金模块-内部类交互对象
创建时间:
作   者:
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/
namespace AMS.Dto
{
    /// <summary>
    /// 描    述:班级查询类
    /// <para>作    者: Huang GaoLiang</para>
    /// <para>创建时间: 2019-03-05</para>
    /// </summary>
    public class ClassInfo
    {
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 学期类型编号
        /// </summary>
        public long TermTypeId { get; set; }

        /// <summary>
        /// 班级编号
        /// </summary>
        public string classNo { get; set; }

        /// <summary>
        /// 校区编号
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 公司编号
        /// </summary>
        public string CompanyId { get; set; }
    }
}
