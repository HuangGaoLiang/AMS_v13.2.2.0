using System.Collections.Generic;

namespace AMS.Service
{
    /// <summary>
    /// 课次结束者接口
    /// </summary>
    public interface ILessonFinisher
    {
        /// <summary>
        /// 获取要创建课次的数据
        /// </summary>
        /// <returns></returns>
        List<LessonFinisherInfo> GetLessonFinisherInfo();

        /// <summary>
        /// 课次废弃之后
        /// </summary>
        void AfterLessonFinish();
    }
}
