using System;
using System.Collections.Generic;
using System.Text; 

namespace Jerrisoft.Platform.Controllers
{
    /// <summary>
    /// 活动类型仓储
    /// </summary>
    public class TActivityTypeRepository : BaseRepository<TActivityType>
    {

        /// <summary>
        ///  获取活动类型
        /// --魏明 2018.06.05
        /// </summary> 
        /// <returns></returns>
        public List<TActivityType> QueryAll()
        {
            return base.LoadList(m => true);
        }

    }
}
