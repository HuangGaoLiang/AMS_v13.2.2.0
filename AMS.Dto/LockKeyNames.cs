using System;
using System.Collections.Generic;
using System.Linq;

namespace AMS.Dto
{
    /// <summary>
    /// 锁的名称
    /// </summary>
    public class LockKeyNames
    {
        /// <summary>
        /// 不允许直接实例化实用
        /// </summary>
        protected LockKeyNames()
        {

        }

        /// <summary>
        /// 订金充值
        /// </summary>
        public const string LOCK_DEPOSITORDERADD = "Lock:DEPOSITORDERADD:StudentId";

        /// <summary>
        /// 订金作废
        /// </summary>
        public const string LOCK_DEPOSITORDERCANCEL = "Lock:DEPOSITORDERCANCEL:StudentId";

        /// <summary>
        /// 钱包交易
        /// </summary>
        public const string LOCK_WALLET = "Lock:Wallet:StudentId";

        /// <summary>
        /// 打印服务
        /// </summary>
        public const string LOCK_PRINT = "Lock:Print:StudentId:PrintBillType";

        /// <summary>
        /// 打印服务
        /// </summary>
        public const string LOCK_LIFE_CLASS = "Lock:LifeClass:LifeClassId";

        /// <summary>
        /// 招生业务锁,包含以下业务：报名订单、报名订单作废、常规排课、休学、退费、转班、转校、写生排课
        /// </summary>
        public const string LOCK_AMSSCHOOLSTUDENT = "Lock:AMS:SchoolId:StudentId";
    }
}
