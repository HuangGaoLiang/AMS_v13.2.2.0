using AMS.Anticorrosion.HRS;
using AMS.Dto;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using AMS.Core;
using System.Threading.Tasks;
using AMS.Core.Constants;

namespace AMS.Service
{
    /// <summary>
    /// 描    述：生成学生学习计划
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public class StudyPlanService
    {
        private readonly string _schoolId;                                                                              //校区Id
        private readonly Lazy<TblOdrStudyPlanRepository> _repository;                            //学习计划仓储
        private readonly Lazy<TblOdrStudyPlanTermRepository> _termRepository;           //学习计划学期仓储

        /// <summary>
        /// 实例化一个学习计划
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <param name="schoolId">所属校区Id</param>
        public StudyPlanService(string schoolId)
        {
            _schoolId = schoolId;
            _repository = new Lazy<TblOdrStudyPlanRepository>();
            _termRepository = new Lazy<TblOdrStudyPlanTermRepository>();
        }

        /// <summary>
        /// 1、知道学期类型和年份、BeginDate、Classes60、Classes90、Classes180；
        /// 2、拿最小BeginDate和学期类型，根据出生日期推算出“学生年龄”；
        /// 3、再根据学生年龄获取课时（比如60分钟），再从“表2”对应学期类型，知道课次；
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <param name="birthday">出生日期</param>
        /// <returns>学生课程计划列表</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：5.请联系系统管理员配置【校区学期类型】
        /// </exception>
        public List<StudyPlanResponse> GetStudyPlanList(DateTime birthday)
        {
            //检查校区学期类型是否配置
            if (ClientConfigManager.AppsettingsConfig.TermType == null)
            {
                throw new BussinessException((byte)ModelType.Default, 5);
            }

            List<StudyPlanResponse> result = new List<StudyPlanResponse>();
            //获取校区学期类型信息
            var studyPlanTermList = GetStudyPlanTermList();
            if (studyPlanTermList.Any())
            {
                result = GetStudyPlanBySeasons(studyPlanTermList, birthday);
            }
            return result;
        }

        /// <summary>
        /// 获取校区学期类型信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-12-24</para>
        /// </summary>
        /// <returns>报名学习计划学期列表</returns>
        private IEnumerable<TblOdrStudyPlanTerm> GetStudyPlanTermList()
        {
            return _termRepository.Value.GetList(_schoolId).Result.Where(a => a.EndDate > DateTime.Now);
        }

        /// <summary>
        /// 根据出生日期，生成每季节对应的学习计划
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-29</para>
        /// </summary>
        /// <param name="studyPlanTermList">获取校区学期类型信息</param>
        /// <param name="birthday">出生日期</param>
        /// <returns>学生课程计划列表</returns>
        private List<StudyPlanResponse> GetStudyPlanBySeasons(IEnumerable<TblOdrStudyPlanTerm> studyPlanTermList, DateTime birthday)
        {
            List<StudyPlanResponse> result = new List<StudyPlanResponse>();                                            //学生课程计划列表
            var studyPlanList = _repository.Value.GetList(_schoolId).Result;                                                    //校区课程信息
            List<SeasonType> seasonList = GetSeasonList();                                                                           //季节
            TblOdrStudyPlanTerm term = null;                                                                                                    //报名学习计划学期信息
            int year = 0;                                                                                                                                       //年份
            int maxValue = 3;                                                                                                                              //年份界限值
            //从当前年份开始，以年份界限进行循环，生成年份的学习计划课程
            for (int i = 0; i < maxValue; i++)
            {
                year = DateTime.Now.Year + i;
                var studyPlanTerms = studyPlanTermList.Where(a => a.Year == year);
                StudyPlanResponse studyPlanResponse = new StudyPlanResponse { Year = year };
                //循环四个季节，生成季节的学习计划课程
                foreach (var season in seasonList)
                {
                    switch (season)
                    {
                        case SeasonType.Spring: //春
                            term = studyPlanTerms.FirstOrDefault(a => a.Year == year && a.TermTypeId == ClientConfigManager.AppsettingsConfig.TermType.Spring);
                            studyPlanResponse.Data.Add(GenerateStudyPlanTerm(term, studyPlanList, birthday, season.ToString()));
                            break;
                        case SeasonType.Summer://夏
                            term = studyPlanTerms.FirstOrDefault(a => a.Year == year && a.TermTypeId == ClientConfigManager.AppsettingsConfig.TermType.Summer);
                            studyPlanResponse.Data.Add(GenerateStudyPlanTerm(term, 4, season.ToString()));
                            break;
                        case SeasonType.Autumn://秋
                            term = studyPlanTerms.FirstOrDefault(a => a.Year == year && a.TermTypeId == ClientConfigManager.AppsettingsConfig.TermType.Autumn);
                            studyPlanResponse.Data.Add(GenerateStudyPlanTerm(term, studyPlanList, birthday, season.ToString()));
                            break;
                        case SeasonType.Winter://冬
                            term = studyPlanTerms.FirstOrDefault(a => a.Year == year && a.TermTypeId == ClientConfigManager.AppsettingsConfig.TermType.Winter);
                            studyPlanResponse.Data.Add(GenerateStudyPlanTerm(term, 3, season.ToString()));
                            break;
                        default:
                            break;
                    }
                }
                result.Add(studyPlanResponse);
            }
            return result;
        }

        /// <summary>
        /// 获取季节
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-02-21</para>
        /// </summary>
        /// <returns></returns>
        private List<SeasonType> GetSeasonList()
        {
            return new List<SeasonType>()
            {
                SeasonType.Spring,
                SeasonType.Autumn,
                SeasonType.Winter,
                SeasonType.Summer
            };
        }

        /// <summary>
        /// 根据用户选择的课程级别，按校区课程计划进行分配
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-05</para>
        /// </summary>
        /// <param name="courseId">出生日期</param>
        /// <param name="courseId">课程Id</param>
        /// <param name="courseLevelId">课程级别Id</param>
        /// <returns>学生课程计划列表</returns>
        public List<StudyPlanResponse> GetStudyPlanList(DateTime birthday, long courseId, long courseLevelId)
        {
            //校区课程信息
            var studyPlan = _repository.Value.GetListAsync(_schoolId, courseId, courseLevelId).Result.OrderBy(a => a.Age).FirstOrDefault();
            var studyPlanTerm = GetStudyPlanTermList().OrderBy(o => o.Year).ThenBy(o => o.BeginDate).FirstOrDefault();
            //如果用户选择的课程存在对应的课程信息，并且存在学期类型信息，则计算用户选择课程年龄大小，进行推荐课程学习计划
            if (studyPlanTerm != null && studyPlan != null)
            {
                int age = Age.GetAgeByDate(birthday, studyPlanTerm.BeginDate);
                birthday = birthday.AddYears(age - studyPlan.Age);
            }
            return GetStudyPlanList(birthday);
        }

        /// <summary>
        /// 生成课程学习计划（包括寒假特训营和暑假特训营）
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-05</para>
        /// </summary>
        /// <param name="term">课程学习计划详情对象</param>
        /// <param name="numbers">个数</param>
        /// <param name="seasonName">季节名称</param>
        /// <returns>学习课程计划详细信息</returns>
        private StudyPlanTermResponse GenerateStudyPlanTerm(TblOdrStudyPlanTerm term, int numbers, string seasonName)
        {
            StudyPlanTermResponse termResponse = new StudyPlanTermResponse { Season = seasonName };
            //校区存在学习计划学期，则生成寒假特训营（包括一个和两个的课程）/暑假特训营（包括一个、两个和三个的课程）的课程
            if (term != null)
            {
                for (int j = 1; j < numbers; j++)
                {
                    StudyPlanTermItemResponse termItem = new StudyPlanTermItemResponse
                    {
                        ClassTimes = term.Classes90 * j,
                        Name = j.GetNumberCnName() + OrdersConstants.Piece,
                        TermTypeId = term.TermTypeId,
                        MaterialFee = term.MaterialFee,
                        TuitionFee = term.TuitionFee,
                        Duration = 90
                    };
                    termResponse.Data.Add(termItem);
                }
            }
            return termResponse;
        }

        /// <summary>
        /// 生成课程学习计划（包括春季班和秋季班）
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-05</para>
        /// </summary>
        /// <param name="term">课程学习计划详情对象</param>
        /// <param name="studyPlanList">课程学习计划对象</param>
        /// <param name="birthday">出生日期</param>
        /// <param name="seasonName">季节名称</param>
        /// <returns>学习课程计划详细信息</returns>
        private StudyPlanTermResponse GenerateStudyPlanTerm(TblOdrStudyPlanTerm term, List<TblOdrStudyPlan> studyPlanList, DateTime birthday, string seasonName)
        {
            StudyPlanTermResponse springTermResponse = new StudyPlanTermResponse { Season = seasonName };
            //校区存在学习计划学期，则生成春季班/秋季班的课程（包括必修课和选修课）
            if (term != null)
            {
                int age = Age.GetAgeByDate(birthday, term.BeginDate);//学生年龄
                //根据学生年龄，获取对应的学习计划课程
                var studyPlan = studyPlanList.FirstOrDefault(a => a.Age == age);
                if (studyPlan != null)
                {
                    //必修课
                    StudyPlanTermItemResponse requiredItem = new StudyPlanTermItemResponse
                    {
                        ClassTimes = GetClassTimesByMinutes(term, studyPlan.Duration),//计算出课次
                        Name = studyPlan.CourseName + (studyPlan.CourseLevelName == OrdersConstants.Nothing ? "" : studyPlan.CourseLevelName.Replace(OrdersConstants.Level, "")),
                        CourseId = studyPlan.CourseId,
                        CourseLevelId = studyPlan.CourseLevelId,
                        CourseType = CourseType.Compulsory,
                        TermTypeId = term.TermTypeId,
                        MaterialFee = term.MaterialFee,
                        TuitionFee = term.TuitionFee,
                        Duration = studyPlan.Duration
                    };
                    springTermResponse.Data.Add(requiredItem);
                }
                //选修课
                StudyPlanTermItemResponse optionalItem = new StudyPlanTermItemResponse
                {
                    Name = OrdersConstants.OptionalLesson,
                    ClassTimes = term.Classes90,
                    CourseType = CourseType.Elective,
                    TermTypeId = term.TermTypeId,
                    MaterialFee = term.MaterialFee,
                    TuitionFee = term.TuitionFee,
                    Duration = 90
                };
                springTermResponse.Data.Add(optionalItem);
            }
            return springTermResponse;
        }

        /// <summary>
        /// 根据分钟获取对应的课次
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-05</para>
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="term"></param>
        /// <returns>课次</returns>
        private int GetClassTimesByMinutes(TblOdrStudyPlanTerm term, int duration)
        {
            switch (duration)
            {
                case 60:
                    return term.Classes60;
                case 90:
                    return term.Classes90;
                case 180:
                    return term.Classes180;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 同步课程名称
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-12-04</para>
        /// </summary>
        /// <param name="newCourseName">课程名称字典</param>
        /// <returns>无</returns>
        public static async Task AsyncCourseName(Dictionary<long, string> newCourseName)
        {
            Lazy<TblOdrStudyPlanRepository> repository = new Lazy<TblOdrStudyPlanRepository>();
            foreach (var courseId in newCourseName.Keys)
            {
                await repository.Value.UpdateCourseNameAsync(courseId, newCourseName[courseId]);
            }
        }

        /// <summary>
        /// 同步课程级别名称
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-12-04</para>
        /// </summary>
        /// <param name="newCourseLevelName">课程级别名称字典</param>
        /// <returns>无</returns>
        public static async Task AsyncCourseLevelName(Dictionary<long, string> newCourseLevelName)
        {
            Lazy<TblOdrStudyPlanRepository> repository = new Lazy<TblOdrStudyPlanRepository>();
            foreach (var courseLevelId in newCourseLevelName.Keys)
            {
                await repository.Value.UpdateCourseLevelNameAsync(courseLevelId, newCourseLevelName[courseLevelId]);
            }
        }


        /// <summary>
        /// 学期时间变化，重新同步一个校区的学期数据
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-12-04</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <returns>无</returns>
        public static async Task AsyncSchoolTermType(string schoolId)
        {
            if (string.IsNullOrEmpty(schoolId)) return;
            await new SchoolTermTypeBuilder(new List<string>() { schoolId }).Build();
        }


        /// <summary>
        /// 授权课程变化，重新同步一个校区的课程数据
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-12-04</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <returns>无</returns>
        public static async Task AsyncSchoolCourse(string schoolId)
        {
            if (string.IsNullOrEmpty(schoolId)) return;
            await new SchoolCourseBuilder(new List<string>() { schoolId }).Build();
        }

        /// <summary>
        /// 获取所有校区
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <returns>校区Id列表</returns>
        private static List<string> GetSchoolIds()
        {
            List<SchoolResponse> schoolList = new OrgService().GetAllSchoolList();
            if (schoolList != null && schoolList.Count > 0)
            {
                return schoolList.Select(a => a.SchoolId).Distinct().ToList();
            }
            return new List<string>();
        }

        /// <summary>
        /// 开启生成计划
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <returns>无</returns>
        public static async Task StartAsync()
        {
            List<string> schoolIds = GetSchoolIds();
            await new SchoolTermTypeBuilder(schoolIds).Build();
            await new SchoolCourseBuilder(schoolIds).Build();
        }
    }
}
