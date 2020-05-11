using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Core
{
    /// <summary>
    /// ID序号生成器
    /// </summary>
    public class IdGenerator
    {
        private static IdWorker _idworker;
        private static object _locker = new object();

        static IdGenerator()
        {
            _idworker = new IdWorker(1);
        }
        
        /// <summary>
        /// 生成下一个ID
        /// </summary>
        /// <returns></returns>
        public static long NextId()
        {
            return _idworker.nextId();
        }
    }
}
