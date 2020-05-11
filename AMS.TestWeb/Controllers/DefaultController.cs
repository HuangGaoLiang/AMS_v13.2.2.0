using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.API.Controllers;
using AMS.SDK;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AMS.TestWeb.Controllers
{
    [Route("api/Default")]
    [ApiController]
    public class DefaultController : BaseController
    {

        private TblDatCost CreateTblDatCost()
        {
            TblDatCost entity1 = new TblDatCost()
            {
                CostId = 1,
                CostName = "test1",
                CreateTime = DateTime.Now,
                IsDisabled = true,
                UpdateTime = DateTime.Now
            };
            return entity1;
        }
        private TblDatCostType CreateTblDatCostType()
        {
            TblDatCostType entity2 = new TblDatCostType()
            {
                TblDatCostTypeId = 1,
                TypeName = "test",
                TypeCode = "test",
                CreateTime = DateTime.Now
            };
            return entity2;
        }


        [HttpGet]
        public async Task Get()
        {
            //TblDatCost entity1 = CreateTblDatCost();
            //TblDatCostType entity2 = CreateTblDatCostType();

            //using (UnitOfWork unitOfWork = new UnitOfWork())
            //{
            //    try
            //    {
            //        unitOfWork.BeginTransaction();
            //        TblDatCostTypeRepository store2 = unitOfWork.GetCustomRepository<TblDatCostTypeRepository, TblDatCostType>();
            //        TblDatCostRepository store1 = unitOfWork.GetCustomRepository<TblDatCostRepository, TblDatCost>();

            //        await store1.AddTask(entity1);
            //        store2.Add(entity2);

            //        var rr=store1.LoadList(t => true);
            //        //string a = "abc";
            //        //int c = int.Parse(a);

            //        unitOfWork.CommitTransaction();
            //    }
            //    catch (System.Exception ex)
            //    {
            //        unitOfWork.RollbackTransaction();
            //        throw;
            //    }
            //}

            var r = ServicesManage.GetCourseService(base.CurrentUser.CompanyId).GetAllCourse();
        }
    }
}