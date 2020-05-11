using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Service
{
    /// <summary>
    /// 课次的产生者接口
    /// </summary>
    public interface ILessonCreator
    {
        /// <summary>
        /// 获取要创建课次的数据
        /// </summary>
        /// <returns></returns>
        List<LessonCreatorInfo> GetLessonCreatorInfo();

        /// <summary>
        /// 排课之后
        /// </summary>
        void AfterLessonCreate();
    }
}
