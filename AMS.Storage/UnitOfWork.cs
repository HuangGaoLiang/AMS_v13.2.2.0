using AMS.Storage.Context;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Storage
{
    /// <summary>
    /// 事务单元
    /// </summary>
    public class UnitOfWork : DefaultUnitOfWork<AMSContext>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnitOfWork() : base(new AMSContext())
        {

        }
    }
}
