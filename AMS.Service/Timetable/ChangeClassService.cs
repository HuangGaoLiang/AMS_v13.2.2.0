using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Anticorrosion.HRS;
using AMS.Core;
using AMS.Core.Locks;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using Jerrisoft.Platform.Public.PageExtensions;

namespace AMS.Service
{
    /// <summary>
    /// 描述：转班办理
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间:2018-11-7</para>
    /// </summary>
    public class ChangeClassService
    {

        private readonly string _schoolId;                                                 //校区Id
        private readonly long _studentId;                                                  //学生Id
        private readonly Lazy<TblTimChangeClassRepository> _tblTimChangeClassRepository;   //转班表仓储

        /// <summary>
        /// 描述：实例化指定校区的转班对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-3-6</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        public ChangeClassService(string schoolId)
        {
            _schoolId = schoolId;
        }

        /// <summary>
        /// 描述：实例化一个校区学生的转班
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        public ChangeClassService(string schoolId, long studentId)
        {
            this._schoolId = schoolId;
            this._studentId = studentId;
            _tblTimChangeClassRepository = new Lazy<TblTimChangeClassRepository>();
        }


        #region GetChangeClassList 获取转班明细列表
        /// <summary>
        /// 描述：获取转班明细列表
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-3-6</para>
        /// </summary>
        /// <param name="searcher">列表筛选条件</param>
        /// <returns>转班明细分页列表</returns>
        public PageResult<ChangeClassListResponse> GetChangeClassList(ChangeClassListSearchRequest searcher)
        {
            var viewChangeClassRepository = new ViewTimChangeClassRepository();
            //获取转班明细列表信息
            var timChangeClassList = viewChangeClassRepository.GetTimeChangeClassList(this._schoolId, searcher);
            var timChangeClassQuery = AutoMapper.Mapper.Map<List<ChangeClassListResponse>>(timChangeClassList.Data);

            var classIdList = new List<long>();
            classIdList.AddRange(timChangeClassQuery.Select(x=>x.OutClassId)); //将转出班级Id集合添加到集合
            classIdList.AddRange(timChangeClassQuery.Select(x=>x.InClassId));  //将转入班级Id集合添加到集合

            //获取所有的学期类型
            var termTypeService = new TermTypeService();
            var allTermTypeList = termTypeService.GetAll();
            //获取班级信息集合
            var classList=  DefaultClassService.GetClassByClassIdAsync(classIdList).Result;
            //获取所有的课程信息
            var courseList =  CourseService.GetAllAsync().Result;
            //获取所有的人员
            var userList= EmployeeService.GetAllBySchoolId(_schoolId);

            foreach (var item in timChangeClassQuery)
            {
                //学期类型信息
                var termModel = allTermTypeList.FirstOrDefault(x => x.TermTypeId == item.TermTypeId);
                //转出班级信息
                var outClassModel = classList.FirstOrDefault(x => x.ClassId == item.OutClassId);
                //获取转出班级的课程信息
                var outCourseModel = courseList.FirstOrDefault(x => x.CourseId == outClassModel.CourseId);
                //获取转出班级教师
                var outTeacherModel = userList.FirstOrDefault(x => x.EmployeeId == outClassModel.TeacherId);

                //转入班级信息
                var inClassModel = classList.FirstOrDefault(x=>x.ClassId==item.InClassId);
                //获取转入班级的课程信息
                var inCourseModel= courseList.FirstOrDefault(x => x.CourseId == inClassModel.CourseId);
                //获取转入班级老师
                var inTeacherModel = userList.FirstOrDefault(x=>x.EmployeeId==inClassModel.TeacherId);

                item.TermTypeName = termModel == null ? "" : termModel.TermTypeName;        //学期类型
                item.OutClassName = outCourseModel==null?"": outCourseModel.ClassCnName;
                item.OutClassNo= outClassModel == null ? "" : outClassModel.ClassNo;
                item.OutClassTeacher = outTeacherModel==null?"": outTeacherModel.EmployeeName;
                item.InClassName = inCourseModel==null?"": inCourseModel.ClassCnName;
                item.InClassNo = inClassModel==null?"": inClassModel.ClassNo;
                item.InClassTeacher = inTeacherModel==null?"": inTeacherModel.EmployeeName;
            }

            var result = new PageResult<ChangeClassListResponse>
            {
                Data = timChangeClassQuery,
                CurrentPage = timChangeClassList.CurrentPage,
                PageSize = timChangeClassList.PageSize,
                TotalData = timChangeClassList.TotalData,
            };
            return result;
        }
        #endregion

        #region GetChangeClassTermClass 获取可转班的学期和班级数据
        /// <summary>
        /// 描述： 获取可转班的学期和班级数据
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <returns>可转班的学期和班级列表</returns>
        public List<StudentTermResponse> GetChangeClassTermClass()
        {
            var service = new StudentTimetableService(this._schoolId, this._studentId);
            var result = service.GetStudentClass();
            return result;
        }
        #endregion

        #region GetChangeOutClassCount 获取转出的课次
        /// <summary>
        /// 描述：获取转出的课次
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="outClassId">转出班级</param>
        /// <param name="outDate">停课日期</param>
        /// <returns>可以转出的课次</returns>

        public int GetChangeOutClassCount(long outClassId, DateTime outDate)
        {
            var service = new StudentTimetableService(this._schoolId, this._studentId);
            List<int> lessonTypeList = new List<int> { (int)LessonType.RegularCourse };
            var outLessonNum = service.GetStudentTransferOutClassLessonCount(outClassId, outDate, lessonTypeList);

            return outLessonNum;
        }
        #endregion

        #region GetChangeInClassCount 获取最多可转入的课次
        /// <summary>
        /// 描述：获取最多可转入的课次
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="inClassId">转入班级</param>
        /// <param name="inDate">上课日期</param>
        /// <returns>最多可以转入的课次</returns>

        public int GetChangeInClassCount(long inClassId, DateTime inDate)
        {
            var service = new DefaultClassService(inClassId);
            var inLesson = service.GetMaximumLessonByFirstTime(inDate);
            return inLesson;
        }
        #endregion

        #region ChangeIn 保存转班办理
        /// <summary>
        /// 描述：保存转班办理
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="dto">要保存的转班信息</param>
        /// <returns>无</returns>
        public void ChangeIn(ChangeClassAddRequest dto)
        {
            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_AMSSCHOOLSTUDENT, this._schoolId, this._studentId.ToString()))
            {
                //验证
                var outClassTimes = this.ValidateSubmit(dto);  //实际转出课次
                using (var unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.BeginTransaction();
                        //1、插入转班表
                        var timChangeClassEntity = AddTimChangeClass(dto, outClassTimes, unitOfWork);
                        //2、课表数据操作---作废
                        ChangeClassLessonFinisher lessonFinisher = new ChangeClassLessonFinisher(timChangeClassEntity, unitOfWork);
                        LessonService lessonService = new LessonService(unitOfWork);
                        lessonService.Finish(lessonFinisher);//课次销毁

                        //3、课表数据操作---转班重新排课
                        ChangeClassLessonCreator creator = new ChangeClassLessonCreator(timChangeClassEntity, lessonFinisher.EnrollOrderItemId, unitOfWork);
                        lessonService.Create(creator);

                        //4.修改学生状态及课次
                        var studentService = new StudentService(this._schoolId);
                        studentService.UpdateStudentStatusById(this._studentId, unitOfWork);

                        unitOfWork.CommitTransaction();
                    }
                    catch (Exception e)
                    {
                        unitOfWork.RollbackTransaction();
                        throw;
                    }
                }

            }

        }
        #endregion

        #region ValidateSubmit 提交验证，并返回实际转出课次
        /// <summary>
        /// 描述：提交验证，并返回实际转出课次
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常Id：30,异常描述：实际转出课次不能为0
        /// 异常Id：31,异常描述：转入班级课次不足，请调整上课日期或选择其他班级
        /// </exception>
        /// <returns>转出课次</returns>
        private int ValidateSubmit(ChangeClassAddRequest dto)
        {
            var outCount = this.GetChangeOutClassCount(dto.OutClassId, dto.OutDate);  //实际转出课次
            var inCount = this.GetChangeInClassCount(dto.InClassId, dto.InDate);     //最多可转入课次

            if (outCount <= 0) //实际转出课次为0时，抛出异常
            {
                throw new BussinessException(ModelType.Timetable, 30);
            }
            if (outCount > inCount) //当实际转出课次大于最多可转入课次则抛出异常
            {
                throw new BussinessException((byte)ModelType.Timetable, 31);
            }

            return outCount;
        }
        #endregion

        #region AddTimChangeClass 添加转班记录
        /// <summary>
        /// 描述：添加转班记录
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="dto">要添加的状态信息</param>
        /// <param name="outClassTimes">转出课次</param>
        /// <param name="unitOfWork">工作单元事务</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常Id：40,异常描述：找不到该报名订单课程
        /// </exception>
        /// <returns>转班信息</returns>
        private TblTimChangeClass AddTimChangeClass(ChangeClassAddRequest dto, int outClassTimes, UnitOfWork unitOfWork)
        {
            //所属报名订单课程Id
            long enrollOrderItemId = 0;
            var service = new StudentTimetableService(this._schoolId, this._studentId);
            List<int> lessonTypeList = new List<int> { (int)LessonType.RegularCourse };
            //获取转出班级课次信息
            var outClassLessonList = service.GetStudentTransferOutClassLessonList(dto.OutClassId, dto.OutDate, lessonTypeList, unitOfWork);
            if (outClassLessonList.FirstOrDefault() != null)  //如果转出班级课次信息不为空，则找到报名订单Id，否则抛出异常
            {
                enrollOrderItemId = outClassLessonList.FirstOrDefault().EnrollOrderItemId;
            }
            else
            {
                throw new BussinessException((byte)ModelType.Timetable, 40);
            }

            var entity = new TblTimChangeClass
            {
                ChangeClassId = IdGenerator.NextId(),
                SchoolId = _schoolId,
                StudentId = _studentId,
                EnrollOrderItemId = enrollOrderItemId,
                OutClassId = dto.OutClassId,
                InClassId = dto.InClassId,
                InDate = dto.InDate,
                OutDate = dto.OutDate,
                ClassTimes = outClassTimes,
                Remark = dto.Remark,
                CreateTime = DateTime.Now
            };
            var tblTimChangeClassRepository = unitOfWork.GetCustomRepository<TblTimChangeClassRepository, TblTimChangeClass>();
            tblTimChangeClassRepository.Add(entity);
            return entity;
        }
        #endregion

    }
}
