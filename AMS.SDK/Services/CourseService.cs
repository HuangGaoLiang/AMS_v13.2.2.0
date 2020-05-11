using System.Collections.Generic;

namespace AMS.SDK
{
    /// <summary>
    /// 课程服务
    /// </summary>
    public class CourseService
    {
        /// <summary>
        /// 公司编号
        /// </summary>
        private readonly string _companyId;

        /// <summary>
        /// 表示一个公司下的课程信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="companyId">公司编号</param>
        public CourseService(string companyId)
        {
            _companyId = companyId;
        }

        /// <summary>
        /// 获取所有课程信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public List<CourseResponse> GetAllCourse()
        {
            List<CourseResponse> result = new List<CourseResponse>();

            //1、从服务层获取所需要的课程数据
            var courses = new AMS.Service.CourseService(_companyId).GetList(null);


            //2、将数据转成SDK数据
            courses.ForEach(t =>
            {
                //2.1 转换课程信息
                CourseResponse courseItem = new CourseResponse()
                {
                    CourseId = t.CourseId,
                    CourseCode = t.CourseCode,
                    ShortName = t.ShortName,
                    CourseCnName = t.CourseCnName,
                    CourseEnName = t.CourseEnName,
                    ClassCnName = t.ClassCnName,
                    ClassEnName = t.ClassEnName,
                    IsDisabled = t.IsDisabled,
                    CourseLevels = new List<CourseLevelResponse>()
                };

                //2、转换课程级别信息
                t.CourseLevels.ForEach(lt =>
                {
                    CourseLevelResponse levelItem = new CourseLevelResponse();
                    levelItem.CourseLevelId = lt.CourseLevelId;
                    levelItem.CourseLevelName = lt.CourseLevelName;
                    levelItem.MinAge = lt.SAge;
                    levelItem.MaxAge = lt.EAge;
                    levelItem.Duration = lt.Duration;
                    courseItem.CourseLevels.Add(levelItem);
                });

                result.Add(courseItem);
            });
            return result;
        }
    }
}
