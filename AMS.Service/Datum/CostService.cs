using AMS.Dto;
using AMS.Storage.Repository;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace AMS.Service
{
    /// <summary>
    /// 描述：费用和费用类别数据提供服务
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2018-11-7</para>
    /// </summary>
    public class CostService
    {
        /// <summary>
        /// 不允许直接实例化实用
        /// </summary>
        protected CostService()
        {

        }

        /// <summary>
        /// 获取指定费用类别的费用
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public static List<CostResponse> GetCosts(Dto.TypeCode typeCode)
        {
            //1、获取类别下的费用
            TblDatCostMiddleRepository repository = new TblDatCostMiddleRepository();
            var costMiddleList = repository.GetByCostType(typeCode.ToString());

            //2、获取所有费用
            TblDatCostRepository costRepository = new TblDatCostRepository();
            var allCostList = costRepository.LoadList(t => true);


            return costMiddleList.Join(allCostList, outer => outer.CostId, inner => inner.CostId, (outer, inner) => new CostResponse
            {
                CostId = inner.CostId,
                CostName = inner.CostName
            }).ToList();
        }
    }
}
