using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Core.Locks
{
    public class RedisServerItem
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
    }

}
