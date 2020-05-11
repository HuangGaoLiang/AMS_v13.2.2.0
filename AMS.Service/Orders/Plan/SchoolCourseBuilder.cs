using AMS.Storage.Models;
using AMS.Storage.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMS.Service
{
    /// <summary>
    /// 描    述：校区的课程生成
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public class SchoolCourseBuilder
    {
        private List<string> _schoolIdList;                                                                         //校区Id集合
        private Lazy<TblOdrStudyPlanRepository> _tblOdrStudyPlanRepository;           //学习计划仓储

        /// <summary>
        /// 校区课程生成实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <param name="schoolIdList">校区Id列表</param>
        public SchoolCourseBuilder(List<string> schoolIdList)
        {
            _schoolIdList = schoolIdList;
            _tblOdrStudyPlanRepository = new Lazy<TblOdrStudyPlanRepository>();
        }

        /// <summary>
        /// 开始生成校区课程级别信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <returns>无</returns>
        public async Task Build()
        {
            //获取校区的课程级别相关信息
            List<TblOdrStudyPlan> studyPlans = _tblOdrStudyPlanRepository.Value.GetStudyPlanList(_schoolIdList);
            if (studyPlans != null && studyPlans.Count > 0)
            {
                //删除旧的校区课程信息
                await _tblOdrStudyPlanRepository.Value.DeleteStudyPlanAsync(_schoolIdList);

                //生成新的校区课程信息
                await _tblOdrStudyPlanRepository.Value.AddStudyPlansAsync(studyPlans);
            }
        }
    }
}
