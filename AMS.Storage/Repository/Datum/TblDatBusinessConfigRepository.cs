/*此代码由生成工具字段生成，生成时间2019/3/13 10:52:09 */
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AMS.Storage.Models;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描述：TblDatBusinessConfig仓储
    /// <para>作者：瞿琦</para>
    /// <para>创建时间：2019-3-13</para>
    /// </summary>
    public class TblDatBusinessConfigRepository : BaseRepository<TblDatBusinessConfig>
    {
        /// <summary>
        /// 描述：根据主键获取配置信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-3-13</para>
        /// </summary>
        /// <returns>配置信息详情</returns>
        public TblDatBusinessConfig GetKeyByDatBusinessConfig(string businessConfigKey)
        {
            var result = base.Load(businessConfigKey);
            return result;
        }
    }
}
