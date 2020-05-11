using System;
using AMS.Dto;
using AMS.Service;
using Jerrisoft.Platform.Public.PageExtensions;
using Microsoft.AspNetCore.Mvc;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 描    述：学钱钱包余额交易
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-06</para>
    /// </summary>
    [Produces("application/json"), Route("api/AMS/WalletTrade")]
    [ApiController]
    public class LoginApi : BsnoController
    {
        /// <summary>
        /// 根据学生Id获取余额明细列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <param name="request">余额交易查询条件</param>
        /// <returns>余额交易分页列表</returns>
        [HttpGet, Route("GetWalletTradeList")]
        public PageResult<WalletTradeListResponse> GetWalletTradeList(long studentId, [FromQuery] WalletTradeListRequest request)
        {
            return new WalletService(base.SchoolId, studentId).GetWalletTradeList(request);
        }

        /// <summary>
        /// 根据学生Id获取余额总额和奖学金总额
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-14</para>
        /// </summary>
        /// <param name="studentId">学生ID</param>
        /// <returns>学生余额信息</returns>
        [HttpGet, Route("GetBalance")]
        public StudentBalanceResponse GetBalance(long studentId)
        {
            WalletService walletService = new WalletService(base.SchoolId, studentId);
            StudentBalanceResponse result = new StudentBalanceResponse()
            {
                TransferAmount = walletService.GetStudentTransferAmount(),//待转出余额
                CouponAmount = new CouponService(base.SchoolId).GetCouponBalanceAmount(studentId),//奖学金
                BalanceAmount = walletService.Balance//余额
            };
            return result;
        }
    }
}
