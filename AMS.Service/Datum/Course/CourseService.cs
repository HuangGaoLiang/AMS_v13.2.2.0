using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AMS.Core;
using AMS.Dto;
using AMS.Storage.Models;
using AMS.Storage.Repository;

namespace AMS.Service
{
    /// <summary>
    /// 课程管理
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-02-20</para>
    /// </summary>
    public class CourseService : BService
    {
        #region 仓储/服务/属性/构造函数
        private readonly Lazy<TblDatCourseRepository> _courseRepository = new Lazy<TblDatCourseRepository>();
        private readonly Lazy<TblDatCourseLevelMiddleRepository> _courseLevelMiddleRepository = new Lazy<TblDatCourseLevelMiddleRepository>();
        private readonly Lazy<TblDatCourseLevelRepository> _courseLevelRepository = new Lazy<TblDatCourseLevelRepository>();

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
        #endregion

        #region 根据课程编号获取课程信息 GetCourseById
        /// <summary>
        /// 根据课程编号获取课程信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="courseId">课程编号</param>
        /// <returns>返回课程信息</returns>
        public TblDatCourse GetCourseById(long courseId)
        {
            return _courseRepository.Value.GetCoursesByCourseId(_companyId, courseId);
        }
        #endregion

        #region GetCourseDetails 获取课程详情

        /// <summary>
        /// 课程详情
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-15</para>
        /// </summary>
        /// <param name="courseId">课程编号</param>
        /// <returns>课程详情数据</returns>
        /// <exception cref="BussinessException">
        /// 异常ID：1，异常描述：未获取到课程数据
        /// </exception>
        public async Task<CourseResponse> GetCourseDetailsAsync(long courseId)
        {
            var course = await _courseRepository.Value.LoadTaskByCourseId(_companyId, courseId);
            if (course == null)
            {
                throw new BussinessException(ModelType.Default, 1);
            }
            CourseResponse courseResponse = AutoMapper.Mapper.Map<CourseResponse>(course);

            List<TblDatCourseLevel> courseLevel = new TblDatCourseLevelRepository()
                .Get(this._companyId)
                .Result
                .OrderBy(x => x.LevelCode)
                .ToList();

            var courseLv = await new ViewCourseLevelMiddleRepository().Get(new List<long> { courseId });

            foreach (var lv in courseLevel)
            {
                CourseLevelMiddleResponse courseLevelMiddle = new CourseLevelMiddleResponse();
                ViewCourseLevelMiddle levelMiddle = courseLv.FirstOrDefault(x => x.CourseLevelId == lv.CourseLevelId);
                if (levelMiddle == null)
                {
                    if (lv.IsDisabled)
                    {
                        continue;
                    }
                    courseLevelMiddle.CourseLevelId = lv.CourseLevelId;
                    courseLevelMiddle.CourseLevelName = lv.LevelCnName;
                    courseLevelMiddle.SAge = courseLevelMiddle.EAge = courseLevelMiddle.Duration = string.Empty;
                    courseLevelMiddle.IsCheck = false;
                }
                else
                {
                    courseLevelMiddle.CourseLevelId = lv.CourseLevelId;
                    courseLevelMiddle.CourseLevelName = lv.LevelCnName;
                    courseLevelMiddle.SAge = levelMiddle.BeginAge.ToString();
                    courseLevelMiddle.EAge = levelMiddle.EndAge.ToString();
                    courseLevelMiddle.Duration = levelMiddle.Duration.ToString();
                    courseLevelMiddle.IsCheck = true;
                }
                courseResponse.CourseLevels.Add(courseLevelMiddle);
            }

            return courseResponse;
        }
        #endregion

        #region GetList 获取课程列表
        /// <summary>
        /// 获取课程列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-14</para>
        /// </summary>
        /// <param name="courseType">null=所有 1=必修课 2=选修课</param>
        /// <returns>课程列表数据</returns>
        public List<CourseListResponse> GetList(int? courseType)
        {
            var courses = _courseRepository.Value.GetByCourseType(_companyId, courseType, null, null).Result;
            if (!courses.Any())
            {
                return new List<CourseListResponse>();
            }

            var courseIds = courses.Select(m => m.CourseId);

            List<CourseListResponse> res = AutoMapper.Mapper.Map<List<CourseListResponse>>(courses);

            var courseLv = new ViewCourseLevelMiddleRepository().Get(courseIds).Result;

            res.ForEach(x =>
            {
                x.CourseLevels = courseLv
                .Where(m => m.CourseId == x.CourseId)
                .OrderBy(m => m.LevelCode)
                .Select(m => new CourseListLevelResponse
                {
                    CourseLevelId = m.CourseLevelId,
                    CourseLevelName = m.LevelCnName,
                    SAge = m.BeginAge,
                    EAge = m.EndAge,
                    Duration = m.Duration
                })
                .ToList();
            });

            return res.OrderBy(m => m.IsDisabled).ThenBy(m => m.CourseCode, new NaturalStringComparer()).ToList();
        }

        /// <summary>
        /// 获取课程基本信息列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-19</para>
        /// </summary>
        /// <param name="cType">0:必修课程 1:选修课程 null:所有</param>
        /// <returns>课程简要信息集合</returns>
        public List<CourseShortResponse> GetShortList(int? cType)
        {
            Expression<Func<TblDatCourse, bool>> where = m => m.IsDisabled == false && m.CompanyId == _companyId;
            if (cType.HasValue)
            {
                where = where.And(m => m.CourseType == cType.Value);
            }

            var courses = new TblDatCourseRepository()
                .GetCoursesAsync()
                .Result
                .Where(where.Compile())
                .OrderBy(x => x.CourseCode, new NaturalStringComparer())
                .ToList();

            return AutoMapper.Mapper.Map<List<CourseShortResponse>>(courses);
        }
        #endregion

        #region Add 添加一个课程
        /// <summary>
        /// 添加一个课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-14</para>
        /// </summary>
        /// <param name="request">课程添加的数据源</param>
        public void Add(CourseAddRequest request)
        {
            //校验
            Validate(request);

            //准备课程数据
            TblDatCourse course = AutoMapper.Mapper.Map<TblDatCourse>(request);
            course.CourseId = IdGenerator.NextId();
            course.CompanyId = _companyId;
            course.CreateTime = course.UpdateTime = DateTime.Now;
            course.IsDisabled = false;

            //准备课程级别数据
            List<TblDatCourseLevelMiddle> courseLevelMiddles = request.CourseLevels.Select(c => new TblDatCourseLevelMiddle
            {
                CourseLevelMiddleId = IdGenerator.NextId(),
                CourseId = course.CourseId,
                BeginAge = c.SAge,
                EndAge = c.EAge,
                CourseLevelId = c.CourseLevelId,
                Duration = c.Duration,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
            }).ToList();

            new TblDatCourseRepository().Add(course);
            new TblDatCourseLevelMiddleRepository().Add(courseLevelMiddles);
        }

        /// <summary>
        /// 校验数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="request">课程添加的数据源</param>
        /// <param name="courseId">课程Id 添加的时可空，编辑时传入</param>
        /// <exception cref="BussinessException">
        /// 异常ID：4,异常描述：课程等级不能为空
        /// 异常ID：1,异常描述：根据业务校验课程的一些基本信息是否重复
        /// </exception>
        private void Validate(CourseAddRequest request, long? courseId = null)
        {
            if (request.CourseLevels == null || !request.CourseLevels.Any())
            {
                throw new BussinessException(ModelType.Timetable, 4);
            }

            var courses = GetAllAsync().Result.Where(x => !x.IsDisabled && x.CompanyId == this._companyId).ToList();

            if (courseId.HasValue)
            {
                courses = courses.Where(x => x.CourseId != courseId.Value).ToList();
            }

            //校验课程编号是否存在
            bool courseCodeIsExist = courses.Any(x => x.CourseCode == request.CourseCode);
            if (courseCodeIsExist)
            {
                throw new BussinessException(ModelType.Timetable, 1, $"课程代码\"{ request.CourseCode }\"已存在");
            }
            //校验课程简称
            bool shortNameIsExist = courses.Any(x => x.ShortName == request.ShortName);
            if (shortNameIsExist)
            {
                throw new BussinessException(ModelType.Timetable, 1, $"课程简称\"{request.ShortName}\"已存在");
            }
            //校验课程名称(中文)
            bool courseCnNameIsExist = courses.Any(x => x.CourseCnName == request.CourseCnName);
            if (courseCnNameIsExist)
            {
                throw new BussinessException(ModelType.Timetable, 1, $"课程名称(中文)\"{request.CourseCnName}\"已存在");
            }
            //校验课程名称(英文)
            bool courseEnNameIsExist = courses.Any(x => x.CourseEnName == request.CourseEnName);
            if (courseEnNameIsExist)
            {
                throw new BussinessException(ModelType.Timetable, 1, $"课程名称(英文)\"{request.CourseEnName}\"已存在");
            }
            //校验班级名称(中文)
            bool classCnNameIsExist = courses.Any(x => x.ClassCnName == request.ClassCnName);
            if (classCnNameIsExist)
            {
                throw new BussinessException(ModelType.Timetable, 1, $"班级名称(中文)\"{request.ClassCnName}\"已存在");
            }
            //校验班级名称(英文)
            bool classEnNameIsExist = courses.Any(x => x.ClassEnName == request.ClassEnName);
            if (classEnNameIsExist)
            {
                throw new BussinessException(ModelType.Timetable, 1, $"班级名称(英文)\"{request.ClassEnName}\"已存在");
            }

            List<AgeGroup> ageGroups = request.CourseLevels.Select(x => new AgeGroup { MinAge = x.SAge, MaxAge = x.EAge }).ToList();

            VerifyAgeCross(ageGroups, request.CourseType, _companyId, courseId);
        }

        /// <summary>
        /// 校验年龄段交叉
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-14</para>
        /// </summary>
        /// <param name="ageGroups">待校验的年龄段数据</param>
        /// <param name="courseType">课程类别</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="courseId">课程Id 添加时可空,编辑时传入</param>
        private static void VerifyAgeCross(List<AgeGroup> ageGroups, CourseType courseType, string companyId, long? courseId = null)
        {
            if (ageGroups == null || ageGroups.Count == 0)
            {
                return;
            }

            //必修课所有时间段不允许交叉
            //选修课课程级别内时间段不允许
            if (courseType == CourseType.Compulsory)
            {
                var compulsorys = new TblDatCourseRepository().GetByCourseType(companyId, (int)CourseType.Compulsory, courseId).Result;
                if (compulsorys.Any())
                {
                    var courseIds = compulsorys.Select(x => x.CourseId);
                    var ages = new TblDatCourseLevelMiddleRepository()
                        .GetByCourseId(courseIds)
                        .Result
                        .Select(x => new AgeGroup
                        {
                            MinAge = x.BeginAge,
                            MaxAge = x.EndAge
                        });

                    ageGroups.AddRange(ages);
                }
            }
            //校验
            CrossCalc(ageGroups);
        }

        #endregion

        #region CrossCalc 数字段交叉算法
        /// <summary>
        /// 数字段交叉算法
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-14</para>
        /// </summary>
        /// <param name="age">年龄段集合</param>
        /// <exception cref="BussinessException">
        /// 异常ID：2,异常描述：年龄不允许交叉
        /// 异常ID：11,异常描述：年龄段范围值不符合规则
        /// </exception>
        private static void CrossCalc(List<AgeGroup> age)
        {
            //把年龄段集合装入数组按最小年龄排序
            //根据数组下标取值进行比较
            age = age.OrderBy(m => m.MinAge).ToList();

            int[] arr = new int[age.Count * 2];
            int temp = 0;
            foreach (var item in age)
            {
                arr[temp] = item.MinAge;
                temp++;
                arr[temp] = item.MaxAge;
                temp++;
            }

            for (int i = 1; i < arr.Length; i++)
            {
                if (i % 2 == 0) //minAge
                {
                    if (arr[i] <= arr[i - 1])
                    {
                        //年龄不允许交叉
                        throw new BussinessException(ModelType.Timetable, 2);
                    }
                    continue;
                }

                if (arr[i] < arr[i - 1]) //maxAge
                {
                    //年龄段范围值不符合规则
                    throw new BussinessException(ModelType.Timetable, 11);
                }
            }
        }

        /// <summary>
        /// 年龄段
        /// </summary>
        private class AgeGroup
        {
            /// <summary>
            /// 
            /// </summary>
            public int MinAge { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public int MaxAge { get; set; }
        }
        #endregion

        #region ModifyAsync 修改课程
        /// <summary>
        /// 修改课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-14</para>
        /// </summary>
        /// <param name="dto">课程修改的数据</param>
        /// <exception cref="BussinessException">
        /// 异常ID：1,异常描述：课程对象为空
        /// </exception>
        public async Task ModifyAsync(CourseAddRequest dto)
        {
            //校验
            var oldCourse = await _courseRepository.Value.LoadTaskByCourseId(_companyId, dto.CourseId);
            if (oldCourse == null)
            {
                throw new BussinessException(ModelType.Default, 1);
            }
            Validate(dto, dto.CourseId);

            bool isAsyncCourseName = !dto.ShortName.Equals(oldCourse.ShortName);

            //准备课程数据
            TblDatCourse course = AutoMapper.Mapper.Map<TblDatCourse>(dto);
            course.CreateTime = oldCourse.CreateTime;
            course.UpdateTime = DateTime.Now;
            course.IsDisabled = oldCourse.IsDisabled;
            course.CourseId = oldCourse.CourseId;
            //准备课程级别数据
            List<TblDatCourseLevelMiddle> courseLevelMiddles = dto.CourseLevels.Select(c => new TblDatCourseLevelMiddle
            {
                CourseLevelMiddleId = IdGenerator.NextId(),
                CourseId = course.CourseId,
                BeginAge = c.SAge,
                EndAge = c.EAge,
                CourseLevelId = c.CourseLevelId,
                Duration = c.Duration,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
            }).ToList();

            await _courseRepository.Value.UpdateTask(course);
            await _courseLevelMiddleRepository.Value.BatchDeleteByCourseId(course.CourseId);
            await _courseLevelMiddleRepository.Value.AddTask(courseLevelMiddles);

            //课程简称发生变化,则学习计划同步课程简称
            if (isAsyncCourseName)
            {
                Dictionary<long, string> keyValues = new Dictionary<long, string>();
                keyValues.Add(course.CourseId, course.ShortName);

                //学习计划同步课程简称
                StudyPlanService.AsyncCourseName(keyValues);
            }
        }
        #endregion

        #region SetEnableAsync/SetDisableAsync 启用/禁用

        /// <summary>
        /// 启用
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-14</para>
        /// </summary>
        /// <param name="courseId">课程编号</param>
        /// <exception cref="BussinessException">
        /// 异常ID：1,异常描述：课程对象为空
        /// </exception>
        public async Task SetEnableAsync(long courseId)
        {
            var course = await _courseRepository.Value.LoadTaskByCourseId(_companyId, courseId);
            if (course == null)
            {
                throw new BussinessException(ModelType.Default, 1);
            }

            //必修课所有时间段不允许交叉
            if (course.CourseType == (int)CourseType.Compulsory)
            {
                //启用时校验时间段
                List<AgeGroup> ageGroup = _courseLevelMiddleRepository.Value
                     .GetByCourseId(new List<long> { course.CourseId })
                     .Result
                     .Select(x => new AgeGroup { MinAge = x.BeginAge, MaxAge = x.EndAge })
                     .ToList();

                VerifyAgeCross(ageGroup, CourseType.Compulsory, _companyId, course.CourseId);
            }

            course.IsDisabled = false;
            course.UpdateTime = DateTime.Now;

            await _courseRepository.Value.UpdateTask(course);
        }

        /// <summary>
        /// 禁用
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-14</para>
        /// <param name="courseId">课程编号</param>
        /// </summary>
        /// <exception cref="BussinessException">
        /// 异常ID：1,异常描述：课程对象为空
        /// </exception>
        public async Task SetDisableAsync(long courseId)
        {
            var course = await _courseRepository.Value.LoadTaskByCourseId(_companyId, courseId);
            if (course == null)
            {
                throw new BussinessException(ModelType.Default, 1);
            }

            course.IsDisabled = true;
            course.UpdateTime = DateTime.Now;

            await _courseRepository.Value.UpdateTask(course);
        }
        #endregion

        #region RemoveAsync 删除
        /// <summary>
        /// 删除
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-14</para>
        /// </summary>
        /// <param name="courseId">课程编号</param>
        /// <remarks>1.已授权给校区 2.教室已使用该课程 3.班级已使用该课程 满足以上不可删除</remarks>
        /// <exception cref="BussinessException">
        /// 异常ID：1,异常描述：课程对象为空
        /// </exception>
        public async Task RemoveAsync(long courseId)
        {
            //获取课程信息
            var course = await _courseRepository.Value.LoadTaskByCourseId(_companyId, courseId);
            if (course == null)
            {
                throw new BussinessException(ModelType.Default, 1);
            }

            //删除校验
            RemoveVerify(courseId);

            await _courseRepository.Value.DeleteTask(course);
            await _courseLevelMiddleRepository.Value.BatchDeleteByCourseId(course.CourseId);
        }

        /// <summary>
        /// 删除校验
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-15</para>
        /// </summary>
        /// <param name="courseId">课程编号</param>
        /// <exception cref="BussinessException">
        /// 异常ID：3,异常描述：已授权给校区
        /// 异常ID：3,异常描述：教室已使用该课程
        /// 异常ID：3,异常描述：班级已使用该课程
        /// </exception>
        private void RemoveVerify(long courseId)
        {
            //1.已授权给校区
            bool schoolIsUse = SchoolCourseService.CourseIsUse(courseId);
            if (schoolIsUse)
            {
                throw new BussinessException(ModelType.Timetable, 3);
            }

            //2.教室已使用该课程
            bool classRoomIsUse = ClassRoomCourseService.CourseIsUse(courseId);
            if (classRoomIsUse)
            {
                throw new BussinessException(ModelType.Timetable, 3);
            }

            //3.班级已使用该课程
            bool classIsUse = new TblDatClassRepository().CourseIsUse(courseId).Result;
            if (classIsUse)
            {
                throw new BussinessException(ModelType.Timetable, 3);
            }
        }
        #endregion

        #region GetAllAsync 获取所有课程信息
        /// <summary>
        /// 获取所有课程信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-18</para>
        /// </summary>
        /// <returns></returns>
        internal static async Task<List<TblDatCourse>> GetAllAsync()
        {
            return await new TblDatCourseRepository().GetCoursesAsync();
        }

        /// <summary>
        /// 获取所有课程信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-18</para>
        /// </summary>
        /// <returns></returns>
        internal static List<TblDatCourse> GetAll()
        {
            return new TblDatCourseRepository().GetAll();
        }
        #endregion

        #region GetByCourseId 获取课程信息
        /// <summary>
        /// 获取课程信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <returns>课程信息</returns>
        internal static TblDatCourse GetByCourseId(long courseId)
        {
            return new TblDatCourseRepository().GetByCourseId(courseId);
        }

        /// <summary>
        /// 获取课程信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <returns>课程信息</returns>
        internal static List<TblDatCourse> GetByCourseId(IEnumerable<long> courseId)
        {
            return new TblDatCourseRepository().GetCourseListBycourseIds(courseId);
        }
        #endregion
    }
}
