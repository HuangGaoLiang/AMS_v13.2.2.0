using AMS.Core;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Service
{
    /// <summary>
    /// 描    述：生成校区的学期数据
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public class SchoolTermTypeBuilder
    {
        private List<string> _schoolIds;                                                                                                             //校区Id列表
        private readonly Lazy<TblDatTermRepository> _tblDatTermRepository;                                              //学期仓储
        private readonly Lazy<TblOdrStudyPlanTermRepository> _tblOdrStudyPlanTermRepository;            //学期课次仓储

        /// <summary>
        /// 校区的学期实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <param name="schoolIds">校区Id集合</param>
        public SchoolTermTypeBuilder(List<string> schoolIds)
        {
            _schoolIds = schoolIds;
            _tblDatTermRepository = new Lazy<TblDatTermRepository>();
            _tblOdrStudyPlanTermRepository = new Lazy<TblOdrStudyPlanTermRepository>();
        }

        /// <summary>
        /// 开始生成学期相关课次信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <returns>无</returns>
        public async Task Build()
        {
            //当前日期的未来三年
            List<int> yearList = GetYearList(2);
            //获取学期相关信息
            List<TblDatTerm> datTermList = _tblDatTermRepository.Value.GetTermListBySchoolIds(this._schoolIds, yearList);
            DateTime currTime = DateTime.Now;

            //获取校区年份学期的相关课次信息（包括学期的开始日期、结束日期，以及学期60分钟、90分钟和180分种的学费和杂费等信息）
            List<TblOdrStudyPlanTerm> studyPlanTermList = new List<TblOdrStudyPlanTerm>();
            foreach (var schoolId in this._schoolIds)
            {
                var terms = datTermList.Where(a => a.SchoolId == schoolId)
                    .GroupBy(g => new { g.Year, g.TermTypeId })
                    .Select(x => new TblOdrStudyPlanTerm()
                    {
                        StudyPlanTermId = IdGenerator.NextId(),
                        Year = x.Key.Year,
                        SchoolId = schoolId,
                        TermTypeName = x.Min(p => p.TermName),
                        TermTypeId = x.Key.TermTypeId,
                        BeginDate = x.Min(p => p.BeginDate),
                        EndDate = x.Max(p => p.EndDate),
                        Classes60 = x.Min(p => p.Classes60),
                        Classes90 = x.Min(p => p.Classes90),
                        Classes180 = x.Min(p => p.Classes180),
                        MaterialFee = x.Min(p => p.MaterialFee),
                        TuitionFee = x.Min(p => p.TuitionFee),
                        CreateTime = currTime
                    }).ToList();
                if (terms != null && terms.Count > 0)
                {
                    studyPlanTermList.AddRange(terms);
                }
            }
            if (studyPlanTermList.Count > 0)
            {
                //先删除旧的学期相关课次信息
                await _tblOdrStudyPlanTermRepository.Value.DeleteStudyPlanTermsAsync(this._schoolIds);

                //生成新的学期相关课次信息
                await _tblOdrStudyPlanTermRepository.Value.AddStudyPlanTermsAsync(studyPlanTermList);
            }
        }

        /// <summary>
        /// 设置年增量，获取当前年份与未来几年年份列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-02-21</para>
        /// </summary>
        /// <param name="value">年份增量值</param>
        /// <returns>当前年份与未来几年年份列表</returns>
        private List<int> GetYearList(int value)
        {
            List<int> yearList = new List<int>();
            for (int i = 0; i <= value; i++)
            {
                yearList.Add(DateTime.Now.AddYears(i).Year);
            }
            return yearList;
        }
    }
}
