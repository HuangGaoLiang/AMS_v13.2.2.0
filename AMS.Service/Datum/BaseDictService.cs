using AMS.Core;

namespace AMS.Service
{
    /// <summary>
    /// 数据字典基类
    /// </summary>
    public class BaseDictService : BService
    {
        private readonly string _dictCode;  //数据字典编号

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictCode"></param>
        protected BaseDictService(string dictCode)
        {
            _dictCode = dictCode;
        }
    }
}
