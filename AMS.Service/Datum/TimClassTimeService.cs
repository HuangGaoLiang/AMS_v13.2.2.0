using AMS.Core;
using AMS.Storage.Repository;
using System.Collections.Generic;
using System.Linq;

namespace AMS.Service
{
    /// <summary>
    /// 班级上课时间服务
    /// </summary>
    public class TimClassTimeService : BService
    {
        private readonly long _classId;  //班级编号

        public TimClassTimeService(long classId)
        {
            this._classId = classId;
        }

        /// <summary>
        /// 获取上课时间段
        ///<para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2019-2-20 </para>
        /// </summary>
        /// <returns>返回上课时间段编号集合</returns>
        public List<long> GetSchoolTimeIds()
        {
            return new TblTimClassTimeRepository().GetByClassId(_classId).Select(m => m.SchoolTimeId).Distinct().ToList();
        }
    }
}
