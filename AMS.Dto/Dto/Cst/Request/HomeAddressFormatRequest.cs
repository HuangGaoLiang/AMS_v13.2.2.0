namespace AMS.Dto
{
    /// <summary>
    /// 描    述: 家庭地址类
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class HomeAddressFormatRequest
    {
        /// <summary>
        /// 国家
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 县区
        /// </summary>
        public string CountyArea { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }
    }
}
