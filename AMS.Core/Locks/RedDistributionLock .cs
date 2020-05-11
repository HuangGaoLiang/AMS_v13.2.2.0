using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AMS.Core.Locks
{
    /// <summary>
    /// 使用red分布式锁
    /// </summary>
    public class RedDistributionLock : IDistributionLock
    {

        public void Lock()
        {
            this.Lock(30);
        }

        public void Lock(int second)
        {
        }

        public void UnLock()
        {
        }
    }
}
