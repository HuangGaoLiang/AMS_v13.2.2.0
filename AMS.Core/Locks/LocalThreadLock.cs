using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMS.Core.Locks
{
    /// <summary>
    /// 本地线程锁
    /// </summary>
    public static class LocalThreadLock
    {
        /// <summary>
        /// 线程锁字典对象
        /// </summary>
        private static List<LockObject> _lockers = new List<LockObject>();

        /// <summary>
        /// 获取锁定名称
        /// </summary>
        public static LockObject GetLockKeyName(string keyPart1, string keyPart2)
        {
            string lockKey = $"{keyPart1}:{keyPart2}";
            if (!_lockers.Any(a => a.Key == lockKey))
            {
                _lockers.Add(new LockObject { Key = lockKey, CreateTime = DateTime.Now });
                _lockers.RemoveAll(a => (DateTime.Now - a.CreateTime).Hours > 1);//移除超过1小时的数据
            }
            return _lockers.FirstOrDefault(a => a.Key == lockKey);
        }

        /// <summary>
        /// 获取锁定名称
        /// </summary>
        public static LockObject GetLockKeyName(string keyPart1, string keyPart2, string keyPart3)
        {
            string lockKey = $"{keyPart1}:{keyPart2}:{keyPart3}";
            if (!_lockers.Any(a => a.Key == lockKey))
            {
                _lockers.Add(new LockObject { Key = lockKey, CreateTime = DateTime.Now });
                _lockers.RemoveAll(a => (DateTime.Now - a.CreateTime).Hours > 1);//移除超过1小时的数据
            }
            return _lockers.FirstOrDefault(a => a.Key == lockKey);
        }
    }

    /// <summary>
    /// 锁对象
    /// </summary>
    public class LockObject
    {
        /// <summary>
        /// Key主键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Key创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
