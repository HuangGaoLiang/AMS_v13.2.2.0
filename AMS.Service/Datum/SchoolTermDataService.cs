using System.Collections.Generic;
using System.Linq;
using AMS.Core;
using AMS.Dto;

namespace AMS.Service
{
    /// <summary>
    /// 校区的学期相关基础数据
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-02-20</para>
    /// </summary>
    public class SchoolTermDataService
    {
        private readonly string _schoolId;//校区Id

        /// <summary>
        /// 根据校区Id构建当前校区学期基础数据服务
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="schoolId">校区ID</param>
        public SchoolTermDataService(string schoolId)
        {
            this._schoolId = schoolId;
        }

        /// <summary>
        /// 获取年份数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-07</para>
        /// </summary>
        /// <returns>年份列表</returns>
        public List<int> GetYears()
        {
            return TermService.GetPredictYears(_schoolId);
        }

        /// <summary>
        /// 根据年份获取学期类型
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-07</para>
        /// </summary>
        /// <returns>学期类型列表</returns>
        public List<TermTypeMiniDataResponse> GetTermTypes()
        {
            return new TermTypeService().GetAll().OrderBy(x => x.Sort).Select(x => new TermTypeMiniDataResponse
            {
                TermTypeId = x.TermTypeId,
                TermTypeName = x.TermTypeName
            }).ToList();
        }

        /// <summary>
        /// 根据学期类型Id加年度获取学期
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-09</para>
        /// </summary>
        /// <returns>学期列表</returns>
        public List<TermMiniDataResponse> GetTermByTermTypeAndYear(long termTypeId, int year)
        {
            TermService service = new TermService(_schoolId, year);

            List<TermDetailResponse> responses = service.GetTermList();

            List<TermMiniDataResponse> res = responses
                .Where(x => x.TermTypeId == termTypeId)
                .Select(x => new TermMiniDataResponse
                {
                    TermId = x.TermId,
                    TermName = x.TermName,
                    Year = x.Year
                })
                .ToList();

            return res;
        }

        /// <summary>
        /// 获取该校区所有授权课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-07</para>
        /// </summary>
        /// <returns>课程列表</returns>
        public List<CourseMiniDataResponse> GetAuthCourse(string companyId)
        {
            return new SchoolCourseAccessService(_schoolId, companyId)
                .AuthCourses
                .Select(x => new CourseMiniDataResponse
                {
                    ClassCnName = x.ClassCnName,
                    ClassEnName = x.ClassEnName,
                    CourseCnName = x.CourseCnName,
                    CourseCode = x.CourseCode,
                    CourseEnName = x.CourseEnName,
                    CourseId = x.CourseId,
                    CourseType = x.CourseType,
                    ShortName = x.ShortName
                }).ToList();
        }

        /// <summary>
        /// 根据课程ID获取课程级别
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-07</para>
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="courseId">课程Id</param>
        /// <returns>课程列表</returns>
        public List<CourseLevelMiniDataResponse> GetCourseLevel(string companyId, long courseId)
        {
            var courseLevelMiniData = new CourseService(companyId).GetCourseDetailsAsync(courseId)
                .Result
                .CourseLevels
                .Select(x => new CourseLevelMiniDataResponse
                {
                    CourseLevelId = x.CourseLevelId,
                    CourseLevelName = x.CourseLevelName,
                    Duration = x.Duration,
                    EAge = x.EAge,
                    IsCheck = x.IsCheck,
                    SAge = x.SAge
                }).ToList();
            return courseLevelMiniData;
        }

        /// <summary>
        /// 获取教室
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <returns>教室列表</returns>
        public List<ClassRoomMiniDataResponse> GetClassRoom() 
        {
            return ClassRoomService.GetClassRoomBySchoolId(_schoolId)
                .Select(x => new ClassRoomMiniDataResponse
                {
                    ClassRoomId = x.ClassRoomId,
                    RoomNo = x.RoomNo,
                    IsDisable = x.IsDisabled
                }).Distinct(new AMS.Core.Compare<ClassRoomMiniDataResponse>((x, y) => (x != null && y != null && x.ClassRoomId == y.ClassRoomId)))
                .ToList();
        }
    }
}
