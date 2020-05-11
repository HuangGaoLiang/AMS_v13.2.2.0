using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Core.Locks
{
    /// <summary>
    /// 创建锁的工厂服务
    /// caiyakang 2018-11-19
    /// </summary>
    public class LockFactoryService
    {
        private static List<RedisServerItem> _redisServerList = new List<RedisServerItem>();

        /// <summary>
        /// 不允许直接实例化
        /// </summary>
        protected LockFactoryService()
        {

        }


        /// <summary>
        /// 初始化redis配置
        /// </summary>
        /// <param name="redisServerList"></param>
        public static void InitRedis(List<RedisServerItem> redisServerList)
        {
            _redisServerList = redisServerList;
        }

        /// <summary>
        /// 创建锁服务
        /// </summary>
        /// <param name="lockName"></param>
        public static IDistributionLock GetLock(string lockName)
        { 
            return new RedDistributionLock();
        }
    }
}
