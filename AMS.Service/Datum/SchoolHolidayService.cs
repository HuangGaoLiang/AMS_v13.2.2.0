using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using AMS.Core;
using AMS.Dto;
using AMS.Storage.Models;
using AMS.Storage.Repository;

namespace AMS.Service
{
    /// <summary>
    /// 停课日设置
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-02-20</para>
    /// </summary>
    public class SchoolHolidayService : BService
    {
        private readonly Lazy<TblDatHolidayRepository> _tblDatHolidayRepository = new Lazy<TblDatHolidayRepository>();

        private readonly string _schoolId;  //校区
        private readonly int _year;         //年份

        /// <summary>
        /// 根据一个校区，下年度创建停课日
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="schoolId">校区</param>
        /// <param name="year">年份</param>
        public SchoolHolidayService(string schoolId, int year)
        {
            this._schoolId = schoolId;
            this._year = year;
        }

        #region  获取一天的停课日数据
        /// <summary>
        /// 获取一天的停课日数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="weekDay">星期</param>
        /// <returns>停课日时间段列表</returns>
        public async Task<List<SchoolHolidayResponse>> GetWeekDayHolidayList()
        {
            var holidays = await _tblDatHolidayRepository.Value.Get(this._schoolId, this._year);

            var res = holidays.Select(m => new SchoolHolidayResponse
            {
                HolidayId = m.HolidayId,
                STime = m.BeginDate,
                ETime = m.EndDate,
                Remark = m.Remark
            })
            .OrderBy(m => m.STime)
            .ToList();

            return res;
        }
        #endregion

        #region Save 添加或修改保存
        /// <summary>
        /// 添加或修改保存
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        public async Task Save(List<SchoolHolidayRequest> dto)
        {
            //1、校验数据合法性
            this.DataVerification(dto);

            //准备数据
            DateTime currentTime = DateTime.Now;
            List<TblDatHoliday> holidays = dto.Select(item => new TblDatHoliday
            {
                HolidayId = IdGenerator.NextId(),
                BeginDate = item.STime,
                EndDate = item.ETime,
                SchoolId = this._schoolId,
                Year = this._year,
                CreateTime = currentTime,
                UpdateTime = currentTime,
                Remark = item.Remark
            }).ToList();

            //2.数据存储
            await _tblDatHolidayRepository.Value.DeleteAsync(_schoolId, _year);
            await _tblDatHolidayRepository.Value.BatchInsertsAsync(holidays);
        }

        /// <summary>
        /// 校验停课日数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-11</para>
        /// </summary>
        private void DataVerification(List<SchoolHolidayRequest> dto)
        {
            foreach (var item in dto)
            {
                if (item.STime > item.ETime || item.STime.Year != _year || item.ETime.Year != _year)
                {
                    throw new BussinessException((byte)ModelType.Default, 2);
                }
            }
        }
        #endregion

        #region  Remove 移除一条数据
        /// <summary>
        /// 删除一条停课日数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="id">停课日ID</param>
        public async Task Remove(long holidayId)
        {
            //1、执行数据删除
            var holiday = await _tblDatHolidayRepository.Value.LoadTask(holidayId);

            if (holiday == null)
            {
                throw new BussinessException((byte)ModelType.Default, 1);
            }

            //2.数据存储
            await _tblDatHolidayRepository.Value.DeleteTask(holiday);
        }
        #endregion

        #region GetPredictYears 获取可预见的年份
        /// <summary>
        /// 获取可预见的年份
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        public static List<int> GetPredictYears(string schoolId)
        {
            var years = new TblDatHolidayRepository()
                .Get(schoolId)
                .Result
                .Select(x => x.Year)
                .ToList();

            return PredictYear.Get(years);
        }
        #endregion

        #region TodayIsSuspendClasses 今天停课吗
        /// <summary>
        /// 今天停课吗?
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-08</para>
        /// </summary>
        /// <param name="schoolHoliday">停课日</param>
        /// <param name="time">today 表示当天</param>
        /// <returns>true:停课 false:不停课</returns>
        internal static bool TodayIsSuspendClasses(List<SchoolHolidayResponse> schoolHoliday, DateTime time)
        {
            return schoolHoliday.Any(item => item.STime <= time && time <= item.ETime);
        }
        #endregion
    }
}
