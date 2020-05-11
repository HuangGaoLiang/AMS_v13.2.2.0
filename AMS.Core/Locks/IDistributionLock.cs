using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Core.Locks
{
    public interface IDistributionLock
    {
        void Lock(int second);
        void Lock();
        void UnLock();
    }
}
