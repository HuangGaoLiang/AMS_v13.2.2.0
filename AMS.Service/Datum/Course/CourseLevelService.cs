using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Core;
using AMS.Dto;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using AutoMapper;

namespace AMS.Service
{
    /// <summary>
    /// 描    述: 表示所有的课程级别管理 
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-19 </para>
    /// </summary>
    public class CourseLevelService : BService
    {
        #region 仓储

        /// <summary>
        /// 课程等级仓储
        /// </summary>
        private readonly Lazy<TblDatCourseLevelRepository> _courseLevelRepository = new Lazy<TblDatCourseLevelRepository>();

        /// <summary>
        /// 课程中间仓储
        /// </summary>
        private readonly Lazy<TblDatCourseLevelMiddleRepository> _courseLevelMiddleRepository = new Lazy<TblDatCourseLevelMiddleRepository>();
        #endregion
        /// <summary>
        /// 公司编号
        /// </summary>
        private readonly string _companyId;

        /// <summary>
        /// 表示一个课程级别
        /// </summary>
        /// <param name="companyId">公司编号</param>
        public CourseLevelService(string companyId)
        {
            this._companyId = companyId;
        }

        #region TblDatCourseLevel
        /// <summary>
        /// 获取一个课程等级信息
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2019-02-18</para>
        /// </summary>
        /// <returns></returns>
        public TblDatCourseLevel GetCourseLevel(long courseLevelId)
        {
            return _courseLevelRepository.Value.GetCourseLevelById(_companyId, courseLevelId);
        }
        #endregion

        #region GetList 获取所有课程级别列表
        /// <summary>
        /// 获取所有课程级别列表 区别公司
        /// <para>作     者:Huang GaoLiang  </para>
        /// <para>创建时间：2018-09-14  </para>
        /// </summary>
        /// <returns>返回课程级别设置列表集合</returns>
        public async Task<List<CourseLevelResponse>> GetList()
        {
            return await GetCourseLiveListList(this._companyId);
        }

        /// <summary>
        /// 获取所有课程级别列表 不区别公司
        /// <para>作     者:Huang GaoLiang  </para>
        /// <para>创建时间：2018-09-14  </para>
        /// </summary>
        /// <returns>返回课程级别设置列表集合</returns>
        internal static async Task<List<CourseLevelResponse>> GetCourseLevelList()
        {
            return await GetCourseLiveListList();
        }

        /// <summary>
        /// 获取所有的课程级别
        /// <para>作     者:Huang GaoLiang  </para>
        /// <para>创建时间：2018-09-14  </para>
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns>返回课程级别设置列表集合</returns>
        private static async Task<List<CourseLevelResponse>> GetCourseLiveListList(string companyId = null)
        {
            List<TblDatCourseLevel> courseLevelList = new List<TblDatCourseLevel>();
            if (!string.IsNullOrWhiteSpace(companyId))
            {
                courseLevelList = await new TblDatCourseLevelRepository().Get(companyId);
            }
            else
            {
                courseLevelList = await new TblDatCourseLevelRepository().LoadLisTask(m => true);
            }

            List<CourseLevelResponse> courseLeaveList = Mapper.Map<List<TblDatCourseLevel>, List<CourseLevelResponse>>(courseLevelList)
                .OrderBy(m => m.IsDisabled)
                .ThenBy(m => m.LevelCode)
                .ToList();

            return courseLeaveList;
        }



        #endregion

        #region SetEnable 启用

        /// <summary>
        /// 启用
        /// <para>作     者:Huang GaoLiang   </para>
        /// <para>创建时间：2018-09-14 </para>
        /// </summary>
        /// <param name="courseLevelId">课次编号级别</param>
        public async Task SetEnable(long courseLevelId)
        {
            await SetDisabled(courseLevelId);
        }

        #endregion

        #region SetDisable 禁用

        /// <summary>
        /// 禁用
        /// <para>作     者:Huang GaoLiang  </para>
        /// <para>创建时间：2018-09-14 </para>
        /// </summary>
        /// <param name="courseLevelId">课程级别id</param>
        public async Task SetDisable(long courseLevelId)
        {
            await SetDisabled(courseLevelId, true);
        }
        #endregion

        #region SetDisabled 启用/禁用
        /// <summary>
        /// 启用/禁用
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-09-15 </para>
        /// </summary>
        /// <param name="courseLevelId">课程等级id</param>
        /// <param name="isDisabled">启用：false;禁用：true</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：11,课程级别编号不能为空
        /// 异常ID：12,系统中没有此课程级别
        /// </exception>
        private async Task SetDisabled(long courseLevelId, bool isDisabled = false)
        {
            // 如果课程级别编号小于或者等于0
            if (courseLevelId <= 0)
            {
                throw new BussinessException((byte)ModelType.Datum, 11);
            }

            // 根据课程级别编号查询课程级别信息
            TblDatCourseLevel courseLeave = _courseLevelRepository.Value.Load(courseLevelId);
            if (courseLeave == null)
            {
                throw new BussinessException((byte)ModelType.Datum, 12);
            }
            courseLeave.IsDisabled = isDisabled;
            await _courseLevelRepository.Value.UpdateTask(courseLeave);
        }

        #endregion

        #region Add 添加一个课程级别
        /// <summary>
        /// 添加一个课程级别
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-09-14 </para>
        /// </summary>
        /// <param name="dto">添加课程等级请求数据</param>
        public async Task Add(CourseLevelAddRequest dto)
        {
            // 1、数据校验
            await CourseLeaveVerification(dto);

            // 2、准备数据
            TblDatCourseLevel courseLevelInfo = new TblDatCourseLevel
            {
                CourseLevelId = IdGenerator.NextId(),
                CompanyId = this._companyId,
                LevelCode = dto.LevelCode,
                LevelCnName = dto.LevelCnName,
                LevelEnName = dto.LevelEnName,
                IsDisabled = false,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };

            // 3、写入数据库
            await new TblDatCourseLevelRepository().AddTask(courseLevelInfo);
        }

        #endregion

        #region Modify 修改一个课程级别
        /// <summary>
        /// 修改一个课程级别
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-09-14 </para>
        /// </summary>
        /// <param name="dto">课程等级实体</param>
        /// <exception cref="BussinessException">
        /// 异常ID：11,课程级别编号不能为空
        /// </exception>
        public async Task Modify(CourseLevelAddRequest dto)
        {
            if (dto.CourseLevelId <= 0)
            {
                throw new BussinessException((byte)ModelType.Datum, 11);
            }

            // 1、数据校验
            await CourseLeaveVerification(dto);

            // 2、根据课程级别id查询课程级别信息
            TblDatCourseLevel courseLevel = new TblDatCourseLevelRepository().GetCourseLevelById(dto.CompanyId, dto.CourseLevelId);

            bool isAsyncCourseLevelName = !dto.LevelCnName.Equals(courseLevel.LevelCnName);

            if (courseLevel != null)
            {
                courseLevel.LevelCode = dto.LevelCode;
                courseLevel.LevelCnName = dto.LevelCnName;
                courseLevel.LevelEnName = dto.LevelEnName;
                courseLevel.UpdateTime = DateTime.Now;
            }

            // 3、修改数据库数据
            await new TblDatCourseLevelRepository().UpdateTask(courseLevel);

            // 4、级别中文名称发生变化，则同步学习计划级别中文名称
            if (isAsyncCourseLevelName)
            {
                Dictionary<long, string> keyValues = new Dictionary<long, string>();
                keyValues.Add(courseLevel.CourseLevelId, courseLevel.LevelCnName);

                // 学习计划同步级别中文名称
                StudyPlanService.AsyncCourseLevelName(keyValues);
            }
        }
        #endregion

        #region Remove 删除

        /// <summary>
        /// 删除
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-09-14 </para>
        /// </summary>
        ///  <param name="courseLevelId">课程级别编号</param>
        /// <exception cref="BussinessException">
        /// 异常ID：17,删除失败
        /// 异常ID：18,本级别已被使用，不可删除
        /// </exception>
        public async Task Remove(long courseLevelId)
        {
            // 1、校验课程等级有没有被使用
            bool used = await _courseLevelMiddleRepository.Value.CourseLevelIsUse(courseLevelId);
            if (used)
            {
                throw new BussinessException((byte)ModelType.Datum, 18);
            }

            // 2、根据课程等级id查询实体
            TblDatCourseLevel courseLevel = await _courseLevelRepository.Value.LoadTask(courseLevelId);

            // 3、删除
            int result = await _courseLevelRepository.Value.DeleteAsync(courseLevel);
            if (result < 0)
            {
                throw new BussinessException((byte)ModelType.Datum, 17);
            }
        }
        #endregion

        #region  CourseLeaveVerification 课程级别数据验证
        /// <summary>
        /// 课程级别数据验证
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-09-14 </para>
        /// </summary>
        /// <param name="dto">课程级别实体</param>
        /// <exception cref="BussinessException">
        /// 异常ID：13,课程级别代码不能为空
        /// 异常ID：14,课程级别名称（中文）不能为空
        /// 异常ID：15,课程级别名称（英文）不能为空
        /// </exception>
        private async Task CourseLeaveVerification(CourseLevelAddRequest dto)
        {
            // 判断等级编号是否为空
            if (string.IsNullOrWhiteSpace(dto.LevelCode))
            {
                throw new BussinessException((byte)ModelType.Datum, 13);
            }

            // 判断等级中文名称是否为空
            if (string.IsNullOrWhiteSpace(dto.LevelCnName))
            {
                throw new BussinessException((byte)ModelType.Datum, 14);
            }

            // 判断等级英文名称是否为空
            if (string.IsNullOrWhiteSpace(dto.LevelEnName))
            {
                throw new BussinessException((byte)ModelType.Datum, 15);
            }

            TblDatCourseLevel courseLevel = Mapper.Map<CourseLevelAddRequest, TblDatCourseLevel>(dto);

            List<TblDatCourseLevel> courseLevelList = await new TblDatCourseLevelRepository().Get(this._companyId);

            // 重复校验（数据不能重复）
            CheckAddOrUpdate(courseLevel, courseLevelList);
        }
        /// <summary>
        /// 数据重复校验
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-09-15 </para>
        /// </summary>
        /// <param name="courseLevel">课程级别id</param>
        /// <param name="courseLevelList">课程级别集合</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：16,系统中已经存在该课程级别，不能重复添加
        /// </exception>
        private static void CheckAddOrUpdate(TblDatCourseLevel courseLevel, List<TblDatCourseLevel> courseLevelList)
        {
            // 如果课程级别大于0
            if (courseLevel.CourseLevelId > 0)
            {
                courseLevelList = courseLevelList.Where(m => m.CourseLevelId != courseLevel.CourseLevelId).ToList();
            }

            // 根据课程级别编号查询课程级别信息
            bool levelCodeIsExist = courseLevelList.Any(m => m.LevelCode == courseLevel.LevelCode);
            if (levelCodeIsExist)
            {
                throw new BussinessException((byte)ModelType.Datum, 16, $"课程级别代码\"{courseLevel.LevelCode }\"已存在");
            }

            bool levelCnNameIsExist = courseLevelList.Any(m => m.LevelCnName == courseLevel.LevelCnName);
            if (levelCnNameIsExist)
            {
                throw new BussinessException((byte)ModelType.Datum, 16, $"课程级别中文名\"{courseLevel.LevelCnName }\"已存在");
            }

            bool levelEnNameIsExist = courseLevelList.Any(m => m.LevelEnName == courseLevel.LevelEnName);
            if (levelEnNameIsExist)
            {
                throw new BussinessException((byte)ModelType.Datum, 16, $"课程级别英文名\"{courseLevel.LevelEnName }\"已存在");
            }
        }

        #endregion

        #region 根据CourseLevelId获取级别信息
        /// <summary>
        /// 根据课程等级Id获取课程级别
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-20</para>
        /// </summary>
        /// <param name="courseLevelId">课程等级Id</param>
        /// <returns>课程级别信息</returns>
        internal static TblDatCourseLevel GetById(long courseLevelId)
        {
            return new TblDatCourseLevelRepository().Load(courseLevelId);
        }

        /// <summary>
        /// 根据课程等级Id获取课程级别
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-20</para>
        /// </summary>
        /// <param name="courseLevelId">课程等级Id</param>
        /// <returns>课程级别信息</returns>
        internal static List<TblDatCourseLevel> GetById(IEnumerable<long> courseLevelId)
        {
            return new TblDatCourseLevelRepository().GetById(courseLevelId);
        }
        #endregion

        #region 获取所有课程级别数据

        /// <summary>
        /// 获取所有课程等级信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-20</para>
        /// </summary>
        /// <returns>获取所有课程等级信息</returns>
        internal static List<TblDatCourseLevel> GetAll()
        {
            return new TblDatCourseLevelRepository().LoadList(x => true);
        }
        #endregion
    }
}
