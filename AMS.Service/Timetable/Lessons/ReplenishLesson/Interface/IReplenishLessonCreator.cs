using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Service
{
    /// <summary>
    /// 调整课次的产生者接口
    /// </summary>
    public interface IReplenishLessonCreator
    {
        /// <summary>
        /// 获取要创建课次的数据
        /// </summary>
        /// <returns></returns>
        List<ReplenishLessonCreatorInfo> GetReplenishLessonCreatorInfo();

        /// <summary>
        /// 执行之后
        /// </summary>
        void AfterReplenishLessonCreate();
    }
}
