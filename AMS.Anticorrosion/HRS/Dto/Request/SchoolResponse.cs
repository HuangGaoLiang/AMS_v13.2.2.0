namespace AMS.Anticorrosion.HRS
{
    /// <summary>
    /// 描    述: 校区类
    /// <para>作    者: Huang GaoLiang</para>
    /// <para>创建时间: 2019-02-19</para>
    /// </summary>
    public class SchoolResponse
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
        /// 校区编号
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 校区名称
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 城市id
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public bool Sort { get; set; }
    }
}
