using AMS.Core;
using AMS.Dto;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Service
{
    /// <summary>
    /// 描    述:  时间段管理 时间段是由学期管理，所以一个不同的学期代表的一个不同的时间段对象
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2018-09-07</para>
    /// </summary>
    public class SchoolTimeService : BService
    {
        /// <summary>
        /// 上课时间段仓储
        /// </summary>
        private readonly Lazy<TblDatSchoolTimeRepository> _schoolTimeRepository = new Lazy<TblDatSchoolTimeRepository>();
        /// <summary>
        /// 学期仓储
        /// </summary>
        private readonly Lazy<TblDatTermRepository> _datTermRepository = new Lazy<TblDatTermRepository>();

        /// <summary>
        /// 学期
        /// </summary>
        private readonly long _termId;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="termId">学期</param>
        public SchoolTimeService(long termId)
        {
            this._termId = termId;
        }

        /// <summary>
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2019-02-19</para>
        /// </summary>
        /// <param name="schoolTimeId">上课时间段编号</param>
        /// <returns>返回上课时间段信息</returns>
        public static SchoolTimeService CreateSchoolTimeService(long schoolTimeId)
        {
            TblDatSchoolTimeRepository repository = new TblDatSchoolTimeRepository();
            TblDatSchoolTime entity = repository.Load(schoolTimeId);
            return new SchoolTimeService(entity.TermId);
        }

        /// <summary>
        /// 学期的所有时间段数据
        /// <para>作    者: 蔡亚康 </para>
        /// <para>创建时间: 2019-02-19</para>
        /// </summary>
        internal List<TblDatSchoolTime> TblDatSchoolTime
        {
            get
            {
                return _schoolTimeRepository.Value.GetSchoolTimeByTermId(this._termId);
            }
        }

        /// <summary>
        /// 学期的信息
        /// <para>作    者: 蔡亚康 </para>
        /// <para>创建时间: 2019-02-19</para>
        /// </summary>
        internal TblDatTerm TblDatTerm
        {
            get
            {
                return _datTermRepository.Value.Load(this._termId);
            }
        }

        /// <summary>
        /// 根据上课时间Id获取上课时间段表数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-08</para>
        /// </summary>
        /// <param name="schoolTimeId">上课时间段Id</param>
        /// <returns>返回上课时间段信息</returns>
        public static TblDatSchoolTime GetBySchoolTimeId(long schoolTimeId)
        {
            return new TblDatSchoolTimeRepository().Load(schoolTimeId);
        }

        /// <summary>
        /// 根据上课时间Id获取上课时间段表数据
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-11-02 </para>
        /// </summary>
        /// <param name="schoolTimeIds">上课时间段Id</param>
        /// <returns>返回上课时间段集合</returns>
        public static List<TblDatSchoolTime> GetBySchoolTimeIds(List<long> schoolTimeIds)
        {
            return new TblDatSchoolTimeRepository().LoadList(m => schoolTimeIds.Contains(m.SchoolTimeId)).OrderBy(m => m.BeginTime).ToList();
        }

        #region GetSchoolTimeList 获取上课时间段
        /// <summary>
        /// 根据学期、和星期几获取上课时间段
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-11-02 </para>
        /// </summary>
        /// <param name="weekDay">星期几</param>
        /// <returns>返回上课时间段集合</returns>
        public List<SchoolTimeDetailResponse> GetSchoolTimeList(int weekDay)
        {
            //如果学期编号为空或者星期星期不存在
            if (_termId == 0 || weekDay <= 0)
            {
                throw new BussinessException((byte)ModelType.Datum, 2);
            }
            List<TblDatSchoolTime> tblDatSchoolTimeList = _schoolTimeRepository.Value.GetSchoolTimeByTermAndWeekDayList(_termId, weekDay).OrderBy(m => m.BeginTime).ToList();

            List<SchoolTimeDetailResponse> schoolTimeDetailResponseList = Mapper.Map<List<TblDatSchoolTime>, List<SchoolTimeDetailResponse>>(tblDatSchoolTimeList);

            return schoolTimeDetailResponseList;

        }
        #endregion

        #region Save 保存上课时间段
        /// <summary>
        /// 保存上课时间段
        /// <para>作     者:Huang GaoLiang  </para>
        /// <para>创建时间：2018-11-02 </para>
        /// </summary>
        /// <param name="schoolTimeSaveRequestList">上课时间段集合</param>
        /// <param name="schoolId">校区编号</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：1,学期编号不能为空
        /// </exception>
        public async Task Save(List<SchoolTimeSaveRequest> schoolTimeSaveRequestList, string schoolId)
        {
            // 如果学期不存在
            if (_termId < 1)
            {
                throw new BussinessException((byte)ModelType.Datum, 3);
            }

            // 如果上课时间段集合为空
            if (schoolTimeSaveRequestList == null && !schoolTimeSaveRequestList.Any())
            {
                return;
            }

            // 1、校验数据的合法性
            List<TblDatSchoolTime> dbSchoolTimeIds = new TblDatSchoolTimeRepository().LoadList(m => m.SchoolId == schoolId && m.TermId == _termId && m.WeekDay == schoolTimeSaveRequestList.FirstOrDefault().WeekDay);

            foreach (SchoolTimeSaveRequest schoolTime in schoolTimeSaveRequestList)
            {
                List<long> schoolTimeIds = dbSchoolTimeIds.Select(m => m.SchoolTimeId).ToList();
                Verification(schoolTimeSaveRequestList, schoolTime, schoolTimeIds);
            }

            // 2、集合重新排序，生成时间段编号
            List<SchoolTimeSaveRequest> resultList = schoolTimeSaveRequestList.OrderBy(m => m.BeginTime).ToList();

            // 3、生成编号
            CodingRules(resultList);
            var info = schoolTimeSaveRequestList.FirstOrDefault();
            int weekDay = 0;
            if (info != null)
            {
                weekDay = info.WeekDay;
            }
            List<TblDatSchoolTime> schoolTimeList = Mapper.Map<List<SchoolTimeSaveRequest>, List<TblDatSchoolTime>>(resultList);

            // 校区编号
            foreach (var item in schoolTimeList)
            {
                item.SchoolId = schoolId;
            }
            // 3、1添加之前先删除对应的数据
            await _schoolTimeRepository.Value.DeleteBytermIdAndWeek(_termId, weekDay);

            // 3、2添加
            await _schoolTimeRepository.Value.BatchInsertsAsync(schoolTimeList);

        }

        /// <summary>
        /// 上课时间段编号生成规则
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-11-02 </para>
        /// </summary>
        /// <param name="resultList">上课时间段集合</param>
        private static void CodingRules(List<SchoolTimeSaveRequest> resultList)
        {
            int sixtyclassTimeNo = 0;
            int ninetyclassTimeNo = 0;
            foreach (SchoolTimeSaveRequest item in resultList)
            {
                // 如果学期编号为空
                if (item.SchoolTimeId < 1)
                {
                    item.SchoolTimeId = IdGenerator.NextId();
                }

                // 如果时间为60分钟
                if (item.Duration == Convert.ToInt32(TimeType.Sixty))
                {
                    sixtyclassTimeNo++;
                    item.ClassTimeNo = "S" + sixtyclassTimeNo;
                }

                // 如果时间为90分钟
                else if (item.Duration == Convert.ToInt32(TimeType.Ninety))
                {
                    ninetyclassTimeNo++;
                    item.ClassTimeNo = "M" + ninetyclassTimeNo;
                }

                // 如果创建时间与当前时间相等
                if (item.CreateTime == DateTime.MinValue)
                {
                    item.CreateTime = DateTime.Now;
                }

                // 如果更新时间与当前时间相等
                if (item.UpdateTime == DateTime.MinValue)
                {
                    item.UpdateTime = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// 校验数据的合法性
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-11-02 </para>
        /// </summary>
        /// <param name="schoolTimeSaveRequestList">上课时间段集合</param>
        /// <param name="schoolTime">上课时间段</param>
        /// <param name="schoolTimeIds">上课时间段编号集合</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：4,您输入的数据有误，请重新输入
        /// 异常ID：5,您输入的时间有误，输入时间段必须为60分钟或者90分钟
        /// 异常ID：6,时间不能交叉
        /// </exception>
        private static void Verification(List<SchoolTimeSaveRequest> schoolTimeSaveRequestList, SchoolTimeSaveRequest schoolTime, List<long> schoolTimeIds)
        {
            // 1数据为空检测
            if (!DataValidation(schoolTime))
            {
                throw new BussinessException((byte)ModelType.Datum, 4);
            }

            // 2、检查时间是不是间隔60分钟和90分钟
            if (!IsRuleTime(schoolTime.BeginTime, schoolTime.EndTime))
            {
                throw new BussinessException((byte)ModelType.Datum, 5);
            }
            // 3、检查集合中有不有交叉的时间段
            if (!CheckDate(schoolTime.BeginTime, schoolTime.Duration, schoolTimeSaveRequestList))
            {
                throw new BussinessException((byte)ModelType.Datum, 6);
            }

            // 4、数据安全性检测
            List<long> requestSchoolTimeIds = schoolTimeSaveRequestList.Where(m => m.SchoolTimeId > 0).Select(m => m.SchoolTimeId).ToList();

            if (!schoolTimeIds.All(m => requestSchoolTimeIds.Any(b => b == m)) || requestSchoolTimeIds.Count != schoolTimeIds.Count)//验证上课时间段主编id与数据库里面的主键id是否匹配
            {
                throw new BussinessException((byte)ModelType.Datum, 4);
            }
        }

        /// <summary>
        /// 数据验证
        /// <para>作     者:Huang GaoLiang  </para>
        /// <para>创建时间：2018-11-02 </para>
        /// </summary>
        /// <param name="schoolTime">上课时间段</param>
        /// <returns>返回true/false</returns>
        private static bool DataValidation(SchoolTimeSaveRequest schoolTime)
        {
            // 判断时间是否在周一至周日
            if (schoolTime.WeekDay < (int)Week.Monday || schoolTime.WeekDay > (int)Week.Sunday)
            {
                return false;
            }

            // 判断时间是否是60分钟还是90分钟
            if (schoolTime.Duration != (int)TimeType.Sixty && schoolTime.Duration != (int)TimeType.Ninety)
            {
                return false;
            }

            // 判断上课时间和下课时间是否为空
            if (string.IsNullOrWhiteSpace(schoolTime.BeginTime) || string.IsNullOrWhiteSpace(schoolTime.EndTime))
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Remove 删除上课时间段
        /// <summary>
        /// 根据主键删除一条上课时间段
        /// <para>作     者:Huang GaoLiang  </para>
        /// <para>创建时间：2018-09-07  </para>
        /// </summary>
        /// <param name="schoolTimeId">上课时间段主键编号</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：7,上课时间段编号不能为空
        /// </exception>
        public static async Task Remove(long schoolTimeId)
        {
            if (schoolTimeId < 1)
            {
                throw new BussinessException((byte)ModelType.Datum, 7);
            }

            // 1、查询需要数据的删除
            TblDatSchoolTimeRepository schoolTimeRepository = new TblDatSchoolTimeRepository();
            TblDatSchoolTime schoolTime = await schoolTimeRepository.LoadTask(schoolTimeId);

            // 2、校验是否可以删除（在课表中被使用的时间段不允许被删除）
            CanDelete(schoolTimeId, schoolTime.TermId);

            List<TblDatSchoolTime> schoolTimeList = new List<TblDatSchoolTime>();
            schoolTimeList = await SchoolTimeCombinated(schoolTimeRepository, schoolTime, schoolTimeList);
            await schoolTimeRepository.DeleteTask(schoolTime.TermId, schoolTime.WeekDay, schoolTime.Duration);
            await schoolTimeRepository.AddTask(schoolTimeList);
        }

        /// <summary>
        /// 重新构造上课时间段编号
        /// <para>作     者:Huang GaoLiang  </para>
        /// <para>创建时间：2018-09-07  </para>
        /// </summary>
        /// <param name="schoolTimeRepository">上课时间段仓储</param>
        /// <param name="schoolTime">上课时间段实体</param>
        /// <param name="schoolTimeList">上课时间段集合</param>
        /// <returns>返回上课时间段编号集合</returns>
        private static async Task<List<TblDatSchoolTime>> SchoolTimeCombinated(TblDatSchoolTimeRepository schoolTimeRepository, TblDatSchoolTime schoolTime, List<TblDatSchoolTime> schoolTimeList)
        {
            // 判断上课时间段数据是否为空
            if (schoolTime != null)
            {
                schoolTimeList = await schoolTimeRepository.Get(schoolTime.TermId, schoolTime.WeekDay, schoolTime.Duration);
                schoolTimeList = schoolTimeList.Where(m => m.SchoolTimeId != schoolTime.SchoolTimeId).OrderBy(m => m.Duration).ThenBy(m => m.BeginTime).ToList();
                int sixtyclassTimeNo = 0;
                int ninetyclassTimeNo = 0;

                foreach (TblDatSchoolTime item in schoolTimeList)
                {
                    // 60分钟的编号加上S
                    if (item.Duration == (int)TimeType.Sixty)
                    {
                        sixtyclassTimeNo++;
                        item.ClassTimeNo = "S" + sixtyclassTimeNo;
                    }
                    // 90分钟的编号加上M
                    if (item.Duration == (int)TimeType.Ninety)
                    {
                        ninetyclassTimeNo++;
                        item.ClassTimeNo = "M" + sixtyclassTimeNo;
                    }
                }
            }

            return schoolTimeList;
        }

        /// <summary>
        /// 数据校验
        /// <para>作     者:Huang GaoLiang  </para>
        /// <para>创建时间：2018-09-07  </para>
        /// </summary>
        /// <param name="schoolTimeId">上课时间段主键编号</param>
        /// <param name="termId">学期编号</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：8,该时间段已排课，不可删除
        /// </exception>
        private static void CanDelete(long schoolTimeId, long termId)
        {
            // 1、查询已生效的数据
            TblTimClassTimeRepository cousClassRepository = new TblTimClassTimeRepository();
            TblTimClassTime cousClass = cousClassRepository.LoadList(m => m.SchoolTimeId == schoolTimeId).FirstOrDefault();

            // 如果查询的班级课表数据不存在
            if (cousClass != null)
            {
                throw new BussinessException((byte)ModelType.Datum, 8);
            }
            // 2、草稿中已被使用的也不能删除
            TermCourseTimetableAuditService auditService = new TermCourseTimetableAuditService(termId);
            if (auditService.IsAuditing || auditService.CanSubmitToAudit)
            {
                List<TblAutClassTime> autClassTimeList = new TermCourseTimetableAuditService(termId).GetAutClassTimeListById(schoolTimeId);

                // 如果查询的班级课表数据存在
                if (autClassTimeList.Any())
                {
                    throw new BussinessException((byte)ModelType.Datum, 8);
                }
            }
        }
        #endregion

        #region GetPredictYears 获取可预见的年份
        /// <summary>
        /// 根据校区编号获取可预见的年份
        /// <para>作     者:Huang GaoLiang   </para>
        /// <para>创建时间：2018-09-07  </para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <returns>返回年度编号集合</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：9,校区编号为空
        /// </exception>
        public static List<int> GetPredictYears(string schoolId)
        {
            // 如果校区编号为空
            if (string.IsNullOrWhiteSpace(schoolId))
            {
                throw new BussinessException((byte)ModelType.Datum, 9);
            }
            //获取当前校区授权学期下的年份
            TblDatTermRepository termRepository = new TblDatTermRepository();
            List<int> yearList = termRepository.GetShoolNoByTblDatTerm(schoolId)
                                                .OrderByDescending(m => m.CreateTime)
                                                .Select(m => m.Year)
                                                .Distinct()
                                                .ToList();
            return PredictYear.Get(yearList);
        }
        #endregion


        /// <summary>
        /// 描述：根据日期获取时间段集合
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-14</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="day">日期</param>
        /// <returns>时间段集合</returns>
        public static List<SchoolDayTimeListResponse> GetDayTimeList(string schoolId, DateTime day)
        {
            //获取今天属于星期几
            int dayInt = WeekDayConvert.DayOfWeekToInt(day);
            //获取日期所属学期
            var termList = new TblDatTermRepository().GetDateByTermList(day);
            var schoolTimeList = new TblDatSchoolTimeRepository().GetSchoolOrTermBySchoolTimeList(schoolId, termList.Select(x => x.TermId));
            var daySchoolTimeList = schoolTimeList.Where(x => x.WeekDay == dayInt).Select(x => new SchoolDayTimeListResponse
            {
                BeginTime = x.BeginTime,
                EndTime = x.EndTime
            }).Distinct(new Compare<SchoolDayTimeListResponse>((x, y) => (x != null && y != null && x.BeginTime == y.BeginTime && x.EndTime == y.EndTime)))
            .ToList();
            return daySchoolTimeList;
        }

        #region IsRuleTime 判断时间是否等于60分钟或者是90分钟
        /// <summary>
        /// 判断时间是否等于60或者是90分钟
        /// <para>作     者:Huang GaoLiang   </para>
        /// <para>创建时间：2018-09-07  </para>
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="result">返回结果</param>
        /// <returns>返回boo类型的值</returns>
        private static bool IsRuleTime(string start, string end, bool result = false)
        {
            DateTime endTime = Convert.ToDateTime(end);
            DateTime sTime = Convert.ToDateTime(start);

            double time = (endTime - sTime).TotalMinutes;
            //如果是60分钟/90分钟
            if (time == ((int)TimeType.Sixty) || (endTime - sTime).TotalMinutes == (int)TimeType.Ninety)
            {
                result = true;
            }
            return result;
        }
        #endregion

        #region CheckDate 判断时间是否有交叉
        /// <summary>
        /// 判断时间是否有交叉
        /// <para>作     者:Huang GaoLiang   </para>
        /// <para>创建时间：2018-09-07  </para>
        /// </summary>
        /// <param name="inputTime">输入时间</param>
        /// <param name="duration">时长</param>
        /// <param name="classTieList">时间段集合</param>
        /// <param name="result">bool值</param>
        /// <returns>返回bool值</returns>
        private static bool CheckDate(string inputTime, int duration, List<SchoolTimeSaveRequest> classTieList, bool result = true)
        {
            // 60分钟的时间集合
            List<SchoolTimeSaveRequest> sixtyClassTieList = new List<SchoolTimeSaveRequest>();

            // 90分钟的时间集合
            List<SchoolTimeSaveRequest> ninetyClassTieList = new List<SchoolTimeSaveRequest>();

            foreach (SchoolTimeSaveRequest item in classTieList)
            {
                DateTime endTime = Convert.ToDateTime(item.EndTime);
                DateTime sTime = Convert.ToDateTime(item.BeginTime);

                if ((endTime - sTime).TotalMinutes == (int)TimeType.Sixty)
                {
                    sixtyClassTieList.Add(item);
                }
                else if ((endTime - sTime).TotalMinutes == (int)TimeType.Ninety)
                {
                    ninetyClassTieList.Add(item);
                }
            }

            // 60分钟的时间集合
            DateTime iTime = Convert.ToDateTime(inputTime);
            foreach (SchoolTimeSaveRequest item in sixtyClassTieList)
            {
                if (iTime > Convert.ToDateTime(item.BeginTime) && iTime < Convert.ToDateTime(item.EndTime) && item.Duration == duration)
                {
                    result = false;
                }
            }

            // 90分钟的时间集合
            foreach (SchoolTimeSaveRequest item in ninetyClassTieList)
            {
                if (iTime > Convert.ToDateTime(item.BeginTime) && iTime < Convert.ToDateTime(item.EndTime) && item.Duration == duration)
                {
                    result = false;
                }
            }
            return result;
        }
        #endregion

        #region  AddTaskDatSchoolTime 批量添加上课时间段

        /// <summary>
        /// 批量添加上课时间段
        /// <para>作     者:Huang GaoLiang  </para>
        /// <para>创建时间：2018-09-26  </para>
        /// </summary>
        /// <param name="list">上课时间段集合</param>
        /// <param name="toTermId">学期编号</param>
        /// <returns>返回受影响的行数</returns>
        internal async Task<int> AddTaskDatSchoolTime(List<TblDatSchoolTime> list, long toTermId)
        {
            TblDatSchoolTimeRepository schoolTimeRepository = new TblDatSchoolTimeRepository();
            await schoolTimeRepository.DeleteTask(toTermId); //先删除，后添加
            return await schoolTimeRepository.AddTask(list);
        }
        #endregion



    }
}
