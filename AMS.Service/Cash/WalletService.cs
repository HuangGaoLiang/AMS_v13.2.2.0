using AMS.Core;
using AMS.Core.Locks;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using AutoMapper;
using Jerrisoft.Platform.Public.PageExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMS.Service
{
    /// <summary>
    /// 表示一个学生在一个校区的钱包实例
    /// </summary>
    public class WalletService
    {
        private Lazy<TblCashWalletRepository> _repository = new Lazy<TblCashWalletRepository>();  //学生钱包
        private Lazy<TblCashWalletTradeRepository> _walletTradeRepository = new Lazy<TblCashWalletTradeRepository>(); //余额交易记录
        private Lazy<TblCashWalletForzenDetailRepository> _forzenRepository = new Lazy<TblCashWalletForzenDetailRepository>(); //余额交易记录
        private readonly string _schoolId;
        private readonly long _studentId;
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// 实例化学生在一个校区的钱包交易对象
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="studentId"></param>
        /// <param name="unitOfWork">创建钱包是否有工作单元事务</param>
        public WalletService(string schoolId, long studentId, UnitOfWork unitOfWork = null)
        {
            if (string.IsNullOrEmpty(schoolId) || schoolId == "undefined")
            {
                throw new ArgumentNullException("SchoolId","校区ID不允许为空");
            }
            this._schoolId = schoolId;
            this._studentId = studentId;
            this._unitOfWork = unitOfWork;

            //初始化仓储
            InitUnitOfWork();
        }

        /// <summary>
        /// 初始化仓储
        /// </summary>
        /// <param name="unitOfWork"></param>
        private void InitUnitOfWork()
        {
            if (this._unitOfWork != null)
            {
                _repository = new Lazy<TblCashWalletRepository>(() => { return this._unitOfWork.GetCustomRepository<TblCashWalletRepository, TblCashWallet>(); });
                _walletTradeRepository = new Lazy<TblCashWalletTradeRepository>(() => { return this._unitOfWork.GetCustomRepository<TblCashWalletTradeRepository, TblCashWalletTrade>(); });
                _forzenRepository = new Lazy<TblCashWalletForzenDetailRepository>(() => { return this._unitOfWork.GetCustomRepository<TblCashWalletForzenDetailRepository, TblCashWalletForzenDetail>(); });
            }
            else
            {
                _repository = new Lazy<TblCashWalletRepository>();  //学生钱包
                _walletTradeRepository = new Lazy<TblCashWalletTradeRepository>(); //余额交易记录
                _forzenRepository = new Lazy<TblCashWalletForzenDetailRepository>(); //余额交易记录
            }
        }


        /// <summary>
        /// 钱包余额是否为负数
        /// </summary>
        public bool IsWalletNegative
        {
            get
            {
                return this.IsWalletNegativeInternal();
            }
        }

        /// <summary>
        /// 学生当前余额
        /// </summary>
        public decimal Balance
        {
            get
            {
                return this.TblCashWallet.Balance;
            }
        }

        private TblCashWallet TblCashWallet
        {
            get
            {
                TblCashWallet result = _repository.Value.GetBySchoolStudentId(this._schoolId, this._studentId);
                if (result == null)
                {
                    result = new TblCashWallet();
                    result.WalletId = IdGenerator.NextId();
                    result.StudentId = this._studentId;
                    result.SchoolId = this._schoolId;
                    result.Balance = 0;
                    result.FrozenAmount = 0;
                    result.UpdateTime = DateTime.Now;
                    _repository.Value.Add(result);
                }
                return result;
            }
        }


        #region IsWalletSufficient 检查学生余额是否足够
        /// <summary>
        /// 检查学生余额是否足够
        /// </summary>
        /// <param name="schoolId">校区ID</param>
        /// <param name="studentId">学生ID</param>
        /// <param name="useBalanceAmount">将要使用的余额金额</param>
        /// <returns>true 足够 false 不足</returns>
        internal static bool IsWalletSufficient(string schoolId, long studentId, decimal useBalanceAmount)
        {
            if (useBalanceAmount <= 0)
            {
                return true;
            }
            WalletService service = new WalletService(schoolId, studentId);
            return service.IsWalletSufficient(useBalanceAmount);
        }


        /// <summary>
        /// 检查学生余额是否足够
        /// </summary>
        /// <param name="useBalanceAmount">将要使用的余额金额</param>
        /// <returns></returns>
        private bool IsWalletSufficient(decimal useBalanceAmount)
        {
            TblCashWallet tblCashWallet = this.TblCashWallet;
            if (tblCashWallet != null)
            {
                return tblCashWallet.Balance >= useBalanceAmount;
            }
            return false;
        }
        #endregion

        #region IsWalletNegativeInternal 检查钱包余额是否为负数
        /// <summary>
        /// 检查钱包余额是否为负数
        /// caiyakang 2018-11-06
        /// </summary>
        /// <returns></returns>
        private bool IsWalletNegativeInternal()
        {
            TblCashWallet tblCashWallet = this.TblCashWallet;
            return tblCashWallet.Balance < 0;
        }
        #endregion

        #region TradeFroze 钱包交易金额冻结
        /// <summary>
        /// 钱包交易金额冻结
        /// </summary>
        /// <param name="transType"></param>
        /// <param name="transAmount"></param>
        /// <param name="unitOfWork">事务工作单元</param>
        internal void TradeFroze(OrderTradeType transType, long orderId, decimal transAmount, string remark)
        {
            if (transAmount == 0)
            {
                return;
            }
            //TODO 以后应用分布式锁
            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_WALLET, this._studentId.ToString()))
            {
                //1、钱包余额冻结金额
                TblCashWallet tblCashWallet = this.TblCashWallet;
                decimal transBefBalance = tblCashWallet.Balance;                //交易前余额
                decimal transAftBalance = tblCashWallet.Balance + transAmount;  //交易后余额
                tblCashWallet.Balance = transAftBalance;
                tblCashWallet.FrozenAmount = transAmount;
                tblCashWallet.UpdateTime = DateTime.Now;

                //2、冻结明细记录
                TblCashWalletForzenDetail detail = new TblCashWalletForzenDetail()
                {
                    BusinessId = orderId,
                    BusinessType = (int)transType,
                    SchoolId = this._schoolId,
                    Status = 1,
                    Amount = transAmount,
                    StudentId = this._studentId,
                    WalletForzenDetailId = IdGenerator.NextId(),
                    CreateTime = DateTime.Now
                };

                //3、余额交易明细
                TblCashWalletTrade walletTrade = new TblCashWalletTrade()
                {
                    WalletTradeId = IdGenerator.NextId(),
                    OrderId = orderId,
                    StudentId = this._studentId,
                    SchoolId = this._schoolId,
                    TradeType = (int)transType,
                    TransBefBalance = transBefBalance,
                    TransAftBalance = transAftBalance,
                    TransAmount = transAmount,
                    TransDate = DateTime.Now,
                    Remark = remark
                };

                //4、写入数据

                _walletTradeRepository.Value.Add(walletTrade);
                _repository.Value.Update(tblCashWallet);  //更新用户钱包余额
                _forzenRepository.Value.Add(detail);      //写入冻结明细
            }
        }
        #endregion

        #region TradeFrozeComplete 冻结交易成功
        /// <summary>
        /// 冻结交易成功
        /// </summary>
        /// <param name="transType"></param>
        /// <param name="orderId"></param>
        internal void TradeFrozeComplete(OrderTradeType transType, long orderId)
        {
            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_WALLET, this._studentId.ToString()))
            {
                //1、处理冻结余额数据
                TblCashWalletForzenDetail detail = ProcessFrozeBalanceComplete(transType, orderId, BalanceFrozeStatus.Finish);
            }
        }
        #endregion

        #region TradeFrozeCancel 冻结交易取消
        /// <summary>
        /// 冻结交易取消
        /// </summary>
        /// <param name="sourceTradeType">原始订单交易类型</param>
        /// <param name="targetTradeType">目标订单交易类型</param>
        /// <param name="orderId"></param>
        internal void TradeFrozeCancel(OrderTradeType sourceTradeType, OrderTradeType targetTradeType, long orderId, string remark)
        {

            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_WALLET, this._studentId.ToString()))
            {
                TblCashWalletForzenDetail detail = this.ProcessFrozeBalanceComplete(sourceTradeType, orderId, BalanceFrozeStatus.Cancel);

                decimal transAmount = detail.Amount * (-1);

                //2、增加交易记录
                TblCashWallet tblCashWallet = this.TblCashWallet;
                decimal transBefBalance = tblCashWallet.Balance + transAmount;//交易前余额,同下
                decimal transAftBalance = tblCashWallet.Balance;  //交易后余额,冻结交易,余额早已扣除，所以该余额即交易后余额
                //余额交易明细
                TblCashWalletTrade walletTrade = new TblCashWalletTrade()
                {
                    WalletTradeId = IdGenerator.NextId(),
                    OrderId = orderId,
                    StudentId = this._studentId,
                    SchoolId = this._schoolId,
                    TradeType = (int)targetTradeType,
                    TransBefBalance = transBefBalance,
                    TransAftBalance = transAftBalance,
                    TransAmount = transAmount,
                    TransDate = DateTime.Now,
                    Remark = remark
                };
                //写入数据
                _walletTradeRepository.Value.Add(walletTrade);
            }
        }
        #endregion

        #region Trade 余额交易
        /// <summary>
        /// 余额交易
        /// 余额交易分两种
        /// </summary>
        /// <param name="transType">交易类型</param>
        /// <param name="transAmount">交易的金额</param>
        public void Trade(OrderTradeType transType, long orderId, decimal transAmount, string remark)
        {
            if (transAmount == 0)
            {
                return;
            }

            //TODO 以后应用分布式锁
            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_WALLET, this._studentId.ToString()))
            {
                //钱包汇总
                TblCashWallet tblCashWallet = this.TblCashWallet;
                decimal transBefBalance = tblCashWallet.Balance;                //交易前余额
                decimal transAftBalance = tblCashWallet.Balance + transAmount;  //交易后余额
                tblCashWallet.Balance = transAftBalance;
                tblCashWallet.UpdateTime = DateTime.Now;


                //余额交易明细
                TblCashWalletTrade walletTrade = new TblCashWalletTrade()
                {
                    WalletTradeId = IdGenerator.NextId(),
                    OrderId = orderId,
                    StudentId = this._studentId,
                    SchoolId = this._schoolId,
                    TradeType = (int)transType,
                    TransBefBalance = transBefBalance,
                    TransAftBalance = transAftBalance,
                    TransAmount = transAmount,
                    TransDate = DateTime.Now,
                    Remark = remark
                };
                //写入数据
                _walletTradeRepository.Value.Add(walletTrade);
                _repository.Value.Update(tblCashWallet);
            }
        }

        /// <summary>
        /// 处理冻结余额为已完成或作废
        /// </summary>
        /// <param name="transType">交易类型</param>
        /// <param name="orderId">订单ID/业务ID</param>
        /// <param name="toFrozeStatus">处理成作废或者完成</param>
        private TblCashWalletForzenDetail ProcessFrozeBalanceComplete(OrderTradeType transType, long orderId, BalanceFrozeStatus toFrozeStatus)
        {
            //1、根据处理后的状态来获取应要查询的状态

            TblCashWalletForzenDetail detail = _forzenRepository.Value.GetByBusinessId(this._schoolId, (int)transType, orderId, (int)BalanceFrozeStatus.Process);

            //2、钱包余额冻结金额完成
            TblCashWallet tblCashWallet = this.TblCashWallet;
            tblCashWallet.FrozenAmount = 0;
            tblCashWallet.UpdateTime = DateTime.Now;
            //防止数据重复操作
            if (detail != null && toFrozeStatus == BalanceFrozeStatus.Cancel)
            {
                //如果取消，应将余额还回去
                tblCashWallet.Balance += detail.Amount * (-1);
            }

            //3、冻结明细记录处理
            //如果存在冻结明细,则记录冻结记录已完成
            if (detail != null)
            {
                detail.Status = (int)toFrozeStatus;
                _forzenRepository.Value.Update(detail);      //写入冻结明细
            }
            //4、钱包余额
            _repository.Value.Update(tblCashWallet);  //更新用户钱包余额
            return detail;
        }
        #endregion

        #region GetWalletTradeList 获取余额明细列表
        /// <summary>
        /// 根据查询条件获取余额明细分页列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="request">余额交易查询条件</param>
        /// <returns>余额明细分页列表</returns>
        public PageResult<WalletTradeListResponse> GetWalletTradeList(WalletTradeListRequest request)
        {
            var result = new PageResult<WalletTradeListResponse>() { Data = new List<WalletTradeListResponse>() };
            var walletTradeList = _walletTradeRepository.Value.GetWalletTradeList(this._schoolId, this._studentId, request);
            if (walletTradeList != null && walletTradeList.Data != null && walletTradeList.Data.Count > 0)
            {
                result.Data = walletTradeList.Data.Select(a => new WalletTradeListResponse()
                {
                    TransDate = a.TransDate,
                    Income = (a.TransAmount > 0 ? a.TransAmount : 0),
                    Expenditure = (a.TransAmount > 0 ? 0 : a.TransAmount),
                    TradeType = EnumName.GetDescription(typeof(OrderTradeType), a.TradeType),
                    TransAftBalance = a.TransAftBalance,
                    Remark = a.Remark
                }).ToList();
                result.CurrentPage = walletTradeList.CurrentPage;
                result.PageSize = walletTradeList.PageSize;
                result.TotalData = walletTradeList.TotalData;
            }
            return result;
        }
        #endregion

        #region GetStudentTransferAmount 获取转校的冻结余额
        /// <summary>
        /// 获取转校的冻结余额
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-14</para>
        /// </summary>
        /// <returns>学生钱包余额冻结总金额</returns>
        public decimal GetStudentTransferAmount()
        {
            var forzenInfo = _forzenRepository.Value.GetByStudentId(this._schoolId, this._studentId).Result;
            if (forzenInfo != null && forzenInfo.Count > 0)
            {
                return forzenInfo.Where(a => a.Status == (int)BalanceFrozeStatus.Process && a.BusinessType == (int)OrderTradeType.ChangeSchool).Sum(b => b.Amount);
            }
            return 0;
        }
        #endregion
    }
}
