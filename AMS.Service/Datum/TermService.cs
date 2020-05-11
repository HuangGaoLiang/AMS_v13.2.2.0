using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Core;
using AMS.Dto;
using AMS.Storage.Models;
using AMS.Storage.Repository;

namespace AMS.Service
{
    /// <summary>
    /// 描述：学期及收费标准设置
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-7</para>
    /// </summary>
    public class TermService
    {
        private readonly TermAuditService _auditService;                    //审核服务
        private readonly Lazy<TblDatTermRepository> _tblDatTermRepository;  //已生效数据的学期仓储
        private readonly Lazy<TermTypeService> _termTypeService;            //学期类型服务
        private readonly string _schoolId;                                  //校区
        private readonly int _year;                                         //年份

        /// <summary>
        /// 描述：学期列表
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        private TermListResponse _termList;

        /// <summary>
        /// 描述：实例化一个学期收费标准设置对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="schoolId">校区</param>
        /// <param name="year">年份</param>
        public TermService(string schoolId, int year)
        {
            this._schoolId = schoolId;
            this._year = year;
            _auditService = new TermAuditService(this._schoolId, this._year);
            _tblDatTermRepository = new Lazy<TblDatTermRepository>();
            _termTypeService = new Lazy<TermTypeService>();
        }

        /// <summary>
        ///描述：审核中学期列表
        ///<para>作    者：瞿琦</para>
        ///<para>创建时间：2018-11-7</para>
        /// </summary>
        public TermListResponse TermList
        {
            get
            {
                if (_termList == null)
                {
                    _termList = GetTermAuditList();
                }
                return _termList;
            }
        }

        /// <summary>
        /// 描述：获取学期类型列表
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <returns>学期类型列表</returns>

        public List<TermTypeResponse> GetTermTypeList()
        {
            var termTypeList = _termTypeService.Value.GetAll();
            return termTypeList;
        }

        #region  GetTermList 获取已生效的收费标准数据
        /// <summary>
        /// 描述：获取已生效的收费标准数据
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <returns>已生效的学期详情列表</returns>
        public List<TermDetailResponse> GetTermList()
        {
            var currentDate = DateTime.Now;
            //获取学期类型列表
            var tremTypeList = GetTermTypeList();
            var result = _tblDatTermRepository.Value.GetTblDatTremList(_schoolId, _year).Select(x => new TermDetailResponse
            {
                Year = x.Year,
                TermId = x.TermId,
                TermName = x.TermName,
                TermTypeId = x.TermTypeId,
                TermTypeName = tremTypeList.Any(k => k.TermTypeId == x.TermTypeId) ? tremTypeList.FirstOrDefault(k => k.TermTypeId == x.TermTypeId)?.TermTypeName : "",
                BeginDate = x.BeginDate,
                EndDate = x.EndDate,
                IsBeginDateChange = x.BeginDate >= DateTime.Now,
                IsEndDateChange = x.EndDate >= DateTime.Now,
                Classes60 = x.Classes60,
                Classes90 = x.Classes90,
                Classes180 = x.Classes180,
                TuitionFee = x.TuitionFee,
                MaterialFee = x.MaterialFee,
                IsCurrentTerm = x.BeginDate <= currentDate && currentDate <= x.EndDate
            }).OrderBy(x => x.TermTypeId).ThenBy(x => x.TermName).ToList();
            return result;
        }
        #endregion

        /// <summary>
        /// 描述：获取审核中的学期收费标准
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="auditId">审核主表Id</param>
        /// <returns>审核中的学期详情列表</returns>

        public List<TermDetailResponse> GetAuditTermList(long auditId)
        {
            TermAuditService auditService = TermAuditService.CreateByAutitId(auditId);
            //获取学期类型列表
            var tremTypeList = GetTermTypeList();
            var queryTerm = auditService.TermList.Select(x => new TermDetailResponse
            {
                TermId = x.TermId,
                TermName = x.TermName,
                TermTypeId = x.TermTypeId,
                TermTypeName = tremTypeList.Any(k => k.TermTypeId == x.TermTypeId) ? tremTypeList.FirstOrDefault(k => k.TermTypeId == x.TermTypeId)?.TermTypeName : "",
                BeginDate = x.BeginDate,
                EndDate = x.EndDate,
                IsBeginDateChange = x.BeginDate >= DateTime.Now,
                IsEndDateChange = x.EndDate >= DateTime.Now,
                Classes60 = x.Classes60,
                Classes90 = x.Classes90,
                Classes180 = x.Classes180,
                TuitionFee = x.TuitionFee,
                MaterialFee = x.MaterialFee,
            }).OrderBy(x => x.TermTypeId).ThenBy(x => x.TermName).ToList();
            return queryTerm;
        }

        #region  GetTermAuditList 获取设置中的数据
        /// <summary>
        /// 描述：获取设置中的数据,该数据可能包含审核中数据
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <returns>学期详情数据</returns>
        public TermListResponse GetTermAuditList()
        {
            var respon = new TermListResponse();
            var auditInfo = _auditService.TblAutAudit;
            //获取学期类型列表
            var tremTypeList = GetTermTypeList();
            //学期列表
            var queryTerm = new List<TermDetailResponse>();
            if (auditInfo == null)
            {
                respon.AuditStatus = null;
            }
            else if (auditInfo.AuditStatus == (int)AuditStatus.Auditing || auditInfo.AuditStatus == (int)AuditStatus.Return || auditInfo.AuditStatus == (int)AuditStatus.Forwarding) //审核中的数据
            {
                //获取审核中的学期收费标准
                queryTerm = GetAuditTermList(auditInfo.AuditId);
                respon.AuditStatus = (AuditStatus)auditInfo.AuditStatus;
                respon.AuditUserId = auditInfo.AuditUserId;
                respon.AuditUserName = auditInfo.AuditUserName;
                respon.AuditDate = auditInfo.AuditDate;
                respon.Remark = auditInfo.DataExt;

                respon.IsFirstSubmitUser = _auditService.IsFlowSubmitUser;
                respon.CreateUserName = auditInfo.CreateUserName;
            }
            else //已生效的数据
            {
                queryTerm = GetTermList();
                respon.AuditStatus = (AuditStatus)auditInfo.AuditStatus;
                respon.AuditUserId = auditInfo.AuditUserId;
                respon.AuditUserName = auditInfo.AuditUserName;
                respon.AuditDate = auditInfo.AuditDate;
                respon.Remark = auditInfo.DataExt;
            }

            respon.Data = queryTerm;   //学期列表
            respon.TermTypeList = tremTypeList;
            return respon;
        }
        #endregion

        #region Audit 提交审核
        /// <summary>
        /// 描述：提交审核
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <returns>无</returns>
        public void Audit(TermAuditRequest data)
        {
            //调用顺序过程：
            //1、验证各种数据合法、是否存在审核等等
            this.ValidateAudit(data.TermAuditDetail);

            //2、调用审核类提交审核到审核平台
            _auditService.SubmitAudit(data);
        }

        /// <summary>
        /// 描述：审核校验
        /// <para>作    者：瞿琦</para
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <returns>无</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：1, 异常描述:当前已存在审核数据
        /// 异常ID：3, 异常描述:学期名称不能重复
        /// </exception>
        private void ValidateAudit(List<TermAuditDetailRequest> dto)
        {
            //1、验证当前单据是否在审批中
            var result = _auditService.TblAutAudit;
            if (result != null && result.AuditStatus == (int)AuditStatus.Auditing)
            {
                throw new BussinessException((byte)ModelType.Audit, 1); //, "当前已存在审核数据"
            }
            var repeatCount = from x1 in dto group x1 by x1;
            foreach (var item in repeatCount)
            {
                if (item.Count() > 1) throw new BussinessException(ModelType.Audit, 3);   //学期名称不能重复
            }
        }

        #endregion

        /// <summary>
        /// 描述：删除学期
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="termId">学期Id</param>
        /// <returns>无</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：10, 异常描述:本学期已被占用，不可删除
        /// 异常ID：12, 异常描述:当前数据已在审核中，不能删除
        /// </exception>
        public void DeleteTerm(long termId)
        {
            //3.上课时间段
            var schoolTimeService = new SchoolTimeService(termId);
            var schoolTimeList = schoolTimeService.TblDatSchoolTime;
            if (schoolTimeList.Any())   //classList.Any() ||  || auditClassService.IsAuditing
            {
                throw new BussinessException((byte)ModelType.Datum, 10); //, "本学期已被占用，不可删除。"
            }

            if (_auditService.IsAuditing)
            {
                throw new BussinessException((byte)ModelType.Audit, 12); //, "审核中的数据不可以删除。"
            }
            //_auditService.RemoveTerm(termId); //如果有,则移除审核中的学期信息
        }


        /// <summary>
        /// 描述：移除当前学期数据
        /// <para>作   者：caiyakang</para>
        /// <para>创建时间:2018-09-29</para>
        /// </summary>
        public void Remove()
        {
            _tblDatTermRepository.Value.DeleteBySchoolAndYear(this._schoolId, this._year);
        }

        /// <summary>
        /// 描述：添加学期数据
        /// <para>作   者：caiyakang</para>
        /// <para>创建时间:2018-09-29</para>
        /// </summary>
        public void ResetTermList(List<TblDatTerm> newTermList)
        {
            //始终要对数据进行校区和年度的处理
            newTermList.ForEach(t =>
            {
                t.SchoolId = this._schoolId;
                t.Year = this._year;
            });

            _tblDatTermRepository.Value.Add(newTermList);
        }

        #region GetPredictYears 获取可预见的年份
        /// <summary>
        /// 描述：获取可预见的年份
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <returns>年度列表</returns>

        public static List<int> GetPredictYears(string schoolId)
        {
            TblDatTermRepository tblDatTermRepository = new TblDatTermRepository();
            //根据校区获取学期列表
            var yearList = tblDatTermRepository.GetShoolNoByTblDatTerm(schoolId).Select(x => x.Year).OrderByDescending(x => x).ToList();
            var yearArray = new List<int>();
            int stratYear = yearList.LastOrDefault();
            if (!yearList.Any())  //如果当前校区没有学期
            {
                stratYear = DateTime.Now.Year;
            }
            else
            {
                if (stratYear > DateTime.Now.Year)
                {
                    stratYear = DateTime.Now.Year;
                }
            }

            for (int i = stratYear; i <= DateTime.Now.Year + 5; i++)
            {
                yearArray.Add(i);
            }

            return yearArray;
        }
        #endregion


        /// <summary>
        /// 描述：根据学期类型获取所有学期数据
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="termTypeId">学期类型ID</param>
        /// <param name="schoolId">校区编号</param>
        /// <returns>学期集合</returns>

        public static List<TblDatTerm> GetTermByTermTypeId(int year, long termTypeId, string schoolId)
        {
            return new TblDatTermRepository().GetTblDatTermByTermTypeId(year, termTypeId, schoolId);
        }


        /// <summary>
        /// 根据学期编号获取学期信息
        /// 作     者:zhiwei.Tang 2018.10.08
        /// </summary>
        /// <param name="termId">学期Id</param>
        /// <returns></returns>
        public static TblDatTerm GetTermByTermId(long termId)
        {
            return new TblDatTermRepository().Load(termId);
        }

        /// <summary>
        /// 根据学期编号获取学期信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-08</para>
        /// </summary>
        /// <param name="termId">学期Id</param>
        /// <returns>学期列表信息</returns>
        public static List<TblDatTerm> GetTermByTermId(IEnumerable<long> termId)
        {
            return new TblDatTermRepository().GetTblDatTermByTermId(termId).Result;
        }

        /// <summary>
        /// 描述：根据校区获取所有的学期
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <returns>学期集合</returns>


        internal static List<TblDatTerm> GetSchoolIdTermList(string schoolId)
        {
            return new TblDatTermRepository().GetSchoolIdTermList(schoolId);
        }

        /// <summary>
        /// 根据学期获取停课日
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-08</para>
        /// </summary>
        /// <param name="term">学期信息</param>
        /// <returns>停课时间段数据列表</returns>
        internal static List<SchoolHolidayResponse> GetSchoolHoliday(TblDatTerm term)
        {
            SchoolHolidayService beginSchoolHoliday = new SchoolHolidayService(term.SchoolId, term.BeginDate.Year);
            SchoolHolidayService endSchoolHoliday = new SchoolHolidayService(term.SchoolId, term.EndDate.Year);

            List<SchoolHolidayResponse> beginSchoolHolidayResponses = beginSchoolHoliday.GetWeekDayHolidayList().Result;
            List<SchoolHolidayResponse> endSchoolHolidayResponses = endSchoolHoliday.GetWeekDayHolidayList().Result;

            List<SchoolHolidayResponse> schoolHoliday = beginSchoolHolidayResponses.Union(endSchoolHolidayResponses).ToList();

            return schoolHoliday;
        }



        /// <summary>
        /// 描述：获取当前时间之后的学期数据
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-1-3</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="currentDate">当前时间</param>
        /// <returns>学期列表</returns>
        internal static List<TblDatTerm> GetFutureTerm(string schoolId, DateTime currentDate)
        {
            return new TblDatTermRepository().GetFutureTerm(schoolId, currentDate);
        }

        /// <summary>
        /// 根据学期，获取学期开始日期至结束日期之间的所有月份
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="termId">学期Id</param>
        /// <returns>学期所有月份</returns>
        public static async Task<List<string>> GetMonthListByTerm(long termId)
        {
            var result = new List<string>();
            var termInfo = await new TblDatTermRepository().GetTblDatTermByTermId(termId);
            if (termInfo != null)
            {
                int year = termInfo.BeginDate.Year;
                int month = termInfo.BeginDate.Month;
                while (year <= termInfo.EndDate.Year && month <= termInfo.EndDate.Month)
                {
                    result.Add($"{year}-{month.ToString().PadLeft(2, '0')}");
                    //如月份已为12月，则接下来就是下一年的1月开始
                    if (month == 12)
                    {
                        month = 1;
                        year++;
                    }
                    else
                    {
                        month++;
                    }
                }
            }
            return result;
        }
    }
}
