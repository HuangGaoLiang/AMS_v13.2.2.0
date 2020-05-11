using AMS.Core;
using AMS.Core.Locks;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AMS.Service
{
    /// <summary>
    /// 描    述：写生课排课
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public class LifeClassMakeLessonService : IMakeLessonService
    {
        private readonly string _schoolId;                                                                                                                                                                                  //校区Id
        private readonly long _lifeClassId;                                                                                                                                                                                 //写生课Id
        private readonly Lazy<TblTimLifeClassRepository> _repository = new Lazy<TblTimLifeClassRepository>();                                                             //写生课仓储
        private readonly Lazy<TblTimLessonRepository> _timLessonRepository = new Lazy<TblTimLessonRepository>();                                                  //课次基础信息仓储
        private readonly Lazy<ViewTimeLessonClassRepository> _vTimeLessonClassRepository = new Lazy<ViewTimeLessonClassRepository>();         //学期班级学生课次信息仓储

        /// <summary>
        /// 写生课排课实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="lifeClassId">写生排课Id</param>
        public LifeClassMakeLessonService(string schoolId, long lifeClassId)
        {
            _schoolId = schoolId;
            _lifeClassId = lifeClassId;
        }

        #region Make写生课
        /// <summary>
        /// 写生排课
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="request">学期班级学生请求列表</param>
        /// <returns>写生排课结果</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：
        /// 13. 系统不存在该写生课信息
        /// 28. 系统不存在该报名订单课程明细信息
        /// 29. 请选择学生进行写生排课
        /// </exception>
        internal LifeClassResultResponse Make(List<TimeLessonClassRequest> request)
        {
            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_LIFE_CLASS, _lifeClassId.ToString()))
            {
                //1.获取写生课信息     
                TblTimLifeClass lifeClassInfo = _repository.Value.GetLifeClassInfo(_lifeClassId);

                //2.获取课次基础信息表
                var lessonClassList = _vTimeLessonClassRepository.Value.GetTimeLessonClassList(_schoolId, request).Result;

                //4.数据校验
                CheckDataBeforeMake(request, lifeClassInfo, lessonClassList);

                //5.写生排课
                LifeClassResultResponse resultResponse = MakeLessonForStudent(request, lifeClassInfo, lessonClassList);

                return resultResponse;
            }
        }

        /// <summary>
        /// 学生写生排课
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-05</para>
        /// </summary>
        /// <param name="request">学期班级学生请求参数</param>
        /// <param name="lifeClassInfo">写生课信息</param>
        /// <param name="lessonClassList">报班明细信息</param>
        /// <returns>写生排课结果</returns>
        private LifeClassResultResponse MakeLessonForStudent(List<TimeLessonClassRequest> request, TblTimLifeClass lifeClassInfo, List<ViewTimeLessonClass> lessonClassList)
        {
            LifeClassResultResponse resultResponse = new LifeClassResultResponse();
            foreach (var studentId in lessonClassList.Select(a => a.StudentId).Distinct())
            {
                lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_AMSSCHOOLSTUDENT, _schoolId, studentId.ToString()))
                {
                    //1.获取学生对应的课次信息
                    var studentLessonList = lessonClassList.Where(a => a.StudentId == studentId);

                    //2.获取报名订单课程明细信息
                    var studentEnrollItemList = EnrollOrderService.GetEnrollOrderItemByItemId(studentLessonList.Select(a => a.EnrollOrderItemId).Distinct());

                    //3.获取需要生成的写生课明细、一个学生多个班级列表和课次不够的学生列表
                    var result = GetLifeClassLesson(request, lifeClassInfo, studentLessonList, studentEnrollItemList);
                    resultResponse.LackTimeList.AddRange(result.Item1);//获取课次不够的学生列表
                    List<LifeClassLessonMakeRequest> lifeClassLessonList = result.Item2;//获取写生课明细
                    resultResponse.StudentClassList.AddRange(result.Item3);//获取一个学生多个班级列表

                    //4.进行写生排课
                    MakeLifeClass(lifeClassInfo, lifeClassLessonList);
                }
            }
            return resultResponse;
        }

        /// <summary>
        /// 写生排课(选择学生班级)
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-03</para>
        /// </summary>
        /// <param name="lifeClassSelectList">学生班级课程对象</param>
        /// <returns>学生课次不够的信息列表</returns>
        internal List<LifeClassLackTimesListResponse> SelectClassToMake(List<LifeClassSelectClassRequest> lifeClassSelectList)
        {
            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_LIFE_CLASS, _lifeClassId.ToString()))
            {
                List<LifeClassLackTimesListResponse> result = new List<LifeClassLackTimesListResponse>();

                //1.获取写生课信息
                TblTimLifeClass lifeClassInfo = _repository.Value.GetLifeClassInfo(_lifeClassId);

                foreach (var studentId in lifeClassSelectList.Select(a => a.StudentId).Distinct())
                {
                    lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_AMSSCHOOLSTUDENT, _schoolId, studentId.ToString()))
                    {
                        //2.获取学生对应的班级课程信息
                        var studentLifeList = lifeClassSelectList.Where(a => a.StudentId == studentId);

                        //3.获取报名订单课程明细信息
                        var enrollOrderItemInfoList = EnrollOrderService.GetEnrollOrderItemByItemId(studentLifeList.Select(a => a.EnrollOrderItemId).Distinct());

                        //4.获取课次不够扣的报名订单课程明细信息
                        var enrollNotEnoughTimeList = enrollOrderItemInfoList.Where(a => a.Status == (int)OrderItemStatus.Enroll && a.ClassTimes - a.ClassTimesUse < lifeClassInfo.UseLessonCount);

                        //5.获取课次不够的学生列表
                        result.AddRange(GetNoEnoughTimeList(lifeClassInfo, studentLifeList, enrollNotEnoughTimeList));

                        //6.过滤课次不够的写生排课
                        var lifeClassList = studentLifeList.Where(a => !enrollNotEnoughTimeList.Any(b => b.EnrollOrderItemId == a.EnrollOrderItemId))
                                    .Select(a => new LifeClassLessonMakeRequest()
                                    {
                                        StudentId = a.StudentId,
                                        ClassId = a.ClassId,
                                        EnrollOrderItemId = a.EnrollOrderItemId
                                    }).ToList();

                        //7.写生排课
                        MakeLifeClass(lifeClassInfo, lifeClassList);
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 获取课次不够扣的学生列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-02</para>
        /// </summary>
        /// <param name="lifeClassInfo">写生课信息</param>
        /// <param name="lifeClassList">所选班级学生信息</param>
        /// <param name="notEnoughTimeList">课次不够的报班明细信息</param>
        /// <returns>学生课次不够的信息列表</returns>
        private List<LifeClassLackTimesListResponse> GetNoEnoughTimeList(TblTimLifeClass lifeClassInfo, IEnumerable<LifeClassSelectClassRequest> lifeClassList, IEnumerable<TblOdrEnrollOrderItem> notEnoughTimeList)
        {
            List<LifeClassLackTimesListResponse> result = new List<LifeClassLackTimesListResponse>();
            foreach (var item in notEnoughTimeList)
            {
                var lifeClass = lifeClassList.FirstOrDefault(a => a.EnrollOrderItemId == item.EnrollOrderItemId);
                if (!result.Any(a => a.ClassNo == lifeClass?.ClassNo && a.StudentName == lifeClass?.StudentName && a.StudentNo == lifeClass?.StudentNo))
                {
                    result.Add(new LifeClassLackTimesListResponse
                    {
                        ClassNo = lifeClass?.ClassNo,
                        StudentName = lifeClass?.StudentName,
                        StudentNo = lifeClass?.StudentNo
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// 写生排课
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-02</para>
        /// </summary>
        /// <param name="lifeClassInfo">写生课信息对象</param>
        /// <param name="lifeClassLessonList">班级学生信息对象</param>
        private void MakeLifeClass(TblTimLifeClass lifeClassInfo, List<LifeClassLessonMakeRequest> lifeClassLessonList)
        {
            if (lifeClassLessonList.Count == 0)
            {
                return;
            }
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    LifeClassLessonCreator lessonCreator = new LifeClassLessonCreator(lifeClassInfo, lifeClassLessonList, unitOfWork);  //实例化课次生产者
                    LessonService lessonService = new LessonService(unitOfWork);
                    lessonService.Create(lessonCreator);//课次生产

                    unitOfWork.CommitTransaction();
                }
                catch (Exception ex)
                {
                    unitOfWork.RollbackTransaction();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 对写生排课进行数据校验
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-20</para>
        /// </summary>
        /// <param name="request">学期班级学生实体请求对象</param>
        /// <param name="lifeClassInfo">写生课对象</param>
        /// <param name="lessonClassList">课次基础信息表</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：
        /// 13. 系统不存在该写生课信息
        /// 28. 系统不存在该报名订单课程明细信息
        /// 29. 请选择学生进行写生排课
        /// </exception>
        private void CheckDataBeforeMake(List<TimeLessonClassRequest> request, TblTimLifeClass lifeClassInfo, List<ViewTimeLessonClass> lessonClassList)
        {
            //获取写生课信息
            if (lifeClassInfo == null)
            {
                throw new BussinessException(ModelType.Timetable, 13);
            }
            if (request == null || request.Count == 0)
            {
                throw new BussinessException(ModelType.Timetable, 29);
            }

            //获取报名订单课程明细信息
            var enrollOrderItemInfoList = EnrollOrderService.GetEnrollOrderItemByItemId(lessonClassList.Select(a => a.EnrollOrderItemId).Distinct());

            //检查课次基础信息是否存在
            var classEnrollOrderItemList = lessonClassList.Where(a => request.Any(b => b.TermId == a.TermId && b.ClassId == a.ClassId)).GroupBy(g => new { g.EnrollOrderItemId, g.StudentId, g.ClassId })
                                            .Select(s => new { s.Key.ClassId, s.Key.EnrollOrderItemId, s.Key.StudentId });
            //如果学生不存在课次基础信息，则直接抛出异常信息
            if (!enrollOrderItemInfoList.Exists(a => classEnrollOrderItemList.Any(b => b.EnrollOrderItemId == a.EnrollOrderItemId) && a.Status == (int)OrderItemStatus.Enroll))
            {
                throw new BussinessException(ModelType.Timetable, 28);
            }
        }

        /// <summary>
        /// 获取需要生成的写生课和课次不够的学生列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-20</para>
        /// </summary>
        /// <param name="request">学期班级学生实体请求对象</param>
        /// <param name="lifeClassInfo">写生课信息</param>
        /// <param name="studentLessonList">学生课次基础信息表</param>
        /// <param name="enrollItemList">学生报名订单课程明细信息</param>
        /// <returns>
        /// 1、学生课次不够的信息列表
        /// 2、写生课排课列表
        /// 3、写生学生班级列表
        /// </returns>
        private (List<LifeClassLackTimesListResponse>, List<LifeClassLessonMakeRequest>, List<LifeClassStudentClassResponse>) GetLifeClassLesson(List<TimeLessonClassRequest> request,
            TblTimLifeClass lifeClassInfo,
            IEnumerable<ViewTimeLessonClass> studentLessonList,
            List<TblOdrEnrollOrderItem> enrollItemList)
        {
            //课次基础信息表
            var lessonInfoList = studentLessonList.Where(a => request.Any(x => x.TermId == a.TermId && x.ClassId == a.ClassId));

            //获取剩余课次不够写生课扣减课次的学生列表
            var enrollOrderItemInfo = from a in enrollItemList
                                      join b in lessonInfoList on a.EnrollOrderItemId equals b.EnrollOrderItemId
                                      where a.Status == (int)OrderItemStatus.Enroll
                                      select new LifeClassStudentListResponse
                                      {
                                          ClassTimes = a.ClassTimes,
                                          ClassTimesUse = a.ClassTimesUse,
                                          EnrollOrderItemId = b.EnrollOrderItemId,
                                          StudentId = b.StudentId,
                                          ClassId = b.ClassId
                                      };

            //获取班级信息
            List<TblDatClass> classInfoList = DefaultClassService.GetClassByClassIdAsync(request.Select(a => a.ClassId).ToList()).Result;

            //根据写生课Id，获取写生课对应的课次基础信息列表
            var lifeInfoList = _timLessonRepository.Value.GetStudentLessonListAsync(_schoolId, new List<long> { _lifeClassId });

            //课次不够扣的学生列表
            List<LifeClassLackTimesListResponse> notEnoughTimeList = GetNotEnoughTimeStudnetList(lifeClassInfo, enrollOrderItemInfo, classInfoList, lifeInfoList);

            //课次够扣的报班学生列表和学生多班级列表
            (List<LifeClassLessonMakeRequest>, List<LifeClassStudentClassResponse>) lifeLessonList = GetEnoughTimeStudentList(lifeClassInfo, enrollOrderItemInfo, classInfoList, lifeInfoList);

            return (notEnoughTimeList, lifeLessonList.Item1, lifeLessonList.Item2);
        }

        /// <summary>
        /// 课次够扣的报班学生列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-21</para>
        /// </summary>
        /// <param name="lifeClassInfo">写生课信息</param>
        /// <param name="enrollOrderItemInfo">报名订单课程明细信息</param>
        /// <param name="classInfoList">班级信息</param>
        /// <param name="lifeInfoList">写生课程明细信息</param>
        /// <returns>
        /// 1、写生课排课列表
        /// 2、写生学生班级列表
        /// </returns>
        private (List<LifeClassLessonMakeRequest>, List<LifeClassStudentClassResponse>) GetEnoughTimeStudentList(TblTimLifeClass lifeClassInfo,
            IEnumerable<LifeClassStudentListResponse> enrollOrderItemInfo,
            List<TblDatClass> classInfoList,
            List<TblTimLesson> lifeInfoList)
        {
            List<LifeClassLessonMakeRequest> lifeOneClassList = new List<LifeClassLessonMakeRequest>();
            List<LifeClassStudentClassResponse> lifeMoreClassList = new List<LifeClassStudentClassResponse>();
            //课次够扣的报班学生列表
            var enoughTimeList = enrollOrderItemInfo.Where(a => (a.ClassTimes - a.ClassTimesUse >= lifeClassInfo.UseLessonCount))
                                        .GroupBy(g => new { g.EnrollOrderItemId, g.StudentId, g.ClassId })
                                        .Select(b => new LifeClassLessonMakeRequest()
                                        {
                                            EnrollOrderItemId = b.Key.EnrollOrderItemId,
                                            ClassId = b.Key.ClassId,
                                            StudentId = b.Key.StudentId
                                        });

            if (enoughTimeList != null && enoughTimeList.Any())
            {
                //一个学生一个班级
                lifeOneClassList = GetLifeClassOfStudentOneClass(enoughTimeList, lifeInfoList);

                //一个学生多个班级
                lifeMoreClassList = GetLifeClassOfStudentMoreClass(enoughTimeList, classInfoList);
            }
            return (lifeOneClassList, lifeMoreClassList);
        }

        /// <summary>
        /// 一个学生只有一个班级
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-02</para>
        /// </summary>
        /// <param name="enoughTimeList">够扣的课次信息对象</param>
        /// <param name="lifeInfoList">写生课程明细信息</param>
        /// <returns>写生课排课列表</returns>
        private List<LifeClassLessonMakeRequest> GetLifeClassOfStudentOneClass(IEnumerable<LifeClassLessonMakeRequest> enoughTimeList, List<TblTimLesson> lifeInfoList)
        {
            List<LifeClassLessonMakeRequest> result = new List<LifeClassLessonMakeRequest>();
            var studentOneClassList = enoughTimeList.Where(a => enoughTimeList.Count(b => b.StudentId == a.StudentId) == 1);
            if (studentOneClassList.Any())
            {
                foreach (var item in studentOneClassList)
                {
                    //如学生已存在写生课的课次基础信息，则不需要再次添加
                    var studentLessonInfo = lifeInfoList.FirstOrDefault(a => a.StudentId == item.StudentId && a.ClassId == item.ClassId);
                    if (studentLessonInfo == null)
                    {
                        result.Add(new LifeClassLessonMakeRequest()
                        {
                            ClassId = item.ClassId,
                            StudentId = item.StudentId,
                            EnrollOrderItemId = item.EnrollOrderItemId
                        });
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 一个学生多个班级的学生班级信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-02</para>
        /// </summary>
        /// <param name="enoughTimeList">写生课足够课次列表</param>
        /// <param name="classInfoList">班级信息列表</param>
        /// <returns></returns>
        private List<LifeClassStudentClassResponse> GetLifeClassOfStudentMoreClass(IEnumerable<LifeClassLessonMakeRequest> enoughTimeList, List<TblDatClass> classInfoList)
        {
            List<LifeClassStudentClassResponse> result = new List<LifeClassStudentClassResponse>();
            var studentMoreClassList = enoughTimeList.Where(a => enoughTimeList.Count(b => b.StudentId == a.StudentId) > 1);
            if (studentMoreClassList.Any())
            {
                //获取该校区的所有学生
                var studentInfoList = StudentService.GetStudentByIds(enoughTimeList.Select(a => a.StudentId).Distinct()).Result;
                foreach (var item in studentMoreClassList)
                {
                    //获取班级信息
                    var classInfo = classInfoList.FirstOrDefault(c => c.ClassId == item.ClassId);
                    //获取lifeRepeatList中学生对应的对象
                    var lifeRepeatInfo = result.FirstOrDefault(l => l.StudentId == item.StudentId);
                    //检查重复学生是否存在于lifeRepeatList中，不存在，则新增同一学生多班级信息的对象
                    if (lifeRepeatInfo == null)
                    {
                        //获取学生信息
                        var studentInfo = studentInfoList.FirstOrDefault(s => s.StudentId == item.StudentId);
                        lifeRepeatInfo = new LifeClassStudentClassResponse()
                        {
                            StudentId = item.StudentId,
                            StudentName = studentInfo?.StudentName,
                            StudentNo = studentInfo?.StudentNo
                        };
                        result.Add(lifeRepeatInfo);
                    }
                    //StudentClassList是否存在该班级，不存在，则添加至StudentClassList
                    if (!lifeRepeatInfo.StudentClassList.Any(a => a.ClassId == item.ClassId && a.EnrollOrderItemId == item.EnrollOrderItemId))
                    {
                        lifeRepeatInfo.StudentClassList.Add(new LifeClassClassInfoResponse()
                        {
                            ClassId = item.ClassId,
                            ClassNo = classInfo?.ClassNo,
                            EnrollOrderItemId = item.EnrollOrderItemId
                        });
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 课次不够扣的学生列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-21</para>
        /// </summary>
        /// <param name="lifeClassInfo">写生课信息</param>
        /// <param name="enrollOrderItemInfo">报名订单课程明细信息</param>
        /// <param name="classInfoList">班级信息列表</param>
        /// <param name="lifeInfoList">课次基础信息列表</param>
        /// <returns></returns>
        private List<LifeClassLackTimesListResponse> GetNotEnoughTimeStudnetList(TblTimLifeClass lifeClassInfo, IEnumerable<LifeClassStudentListResponse> enrollOrderItemInfo,
            List<TblDatClass> classInfoList, List<TblTimLesson> lifeInfoList)
        {
            List<LifeClassLackTimesListResponse> result = new List<LifeClassLackTimesListResponse>();
            //课次不够扣的学生列表
            var notEnoughTimeList = enrollOrderItemInfo.Where(a => (a.ClassTimes - a.ClassTimesUse < lifeClassInfo.UseLessonCount) && !lifeInfoList.Any(l => l.EnrollOrderItemId == a.EnrollOrderItemId))
                                        .GroupBy(g => new { g.EnrollOrderItemId, g.StudentId, g.ClassId })
                                        .Select(b => new
                                        {
                                            b.Key.EnrollOrderItemId,
                                            b.Key.ClassId,
                                            b.Key.StudentId
                                        });
            if (notEnoughTimeList != null && notEnoughTimeList.Any())
            {
                //获取该校区的所有学生
                var studentInfoList = StudentService.GetStudentByIds(notEnoughTimeList.Select(a => a.StudentId).Distinct()).Result;
                foreach (var item in notEnoughTimeList)
                {
                    var studentInfo = studentInfoList.FirstOrDefault(a => a.StudentId == item.StudentId);//学生对应的学生信息
                    var classInfo = classInfoList.FirstOrDefault(a => a.ClassId == item.ClassId);//班级信息
                    if (!result.Any(a => a.ClassNo == classInfo?.ClassNo && a.StudentName == studentInfo?.StudentName && a.StudentNo == studentInfo?.StudentNo))
                    {
                        result.Add(new LifeClassLackTimesListResponse
                        {
                            ClassNo = classInfo?.ClassNo,
                            StudentName = studentInfo?.StudentName,
                            StudentNo = studentInfo?.StudentNo
                        });
                    }
                }
            }

            return result;
        }
        #endregion

        #region Cancel写生课
        /// <summary>
        /// 取消写生排课
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：
        /// 13. 系统不存在该写生课信息
        /// 14. 该学期已结束，不可取消
        /// 34. 写生课已取消
        /// </exception>
        public void Cancel()
        {
            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_LIFE_CLASS, _lifeClassId.ToString()))
            {
                var lifeClassInfo = _repository.Value.GetLifeClassInfo(_lifeClassId);

                //1、根据写生课Id，获取写生课对应的课次基础信息的课程Id列表
                var studentLessonInfoList = _timLessonRepository.Value.GetStudentLessonListAsync(_schoolId, new List<long> { _lifeClassId });
                var lessonIdList = studentLessonInfoList.Where(a => a.Status == (int)LessonUltimateStatus.Normal).Select(a => a.LessonId).ToList();

                //2、校验
                CheckDataBeforeCancel(lifeClassInfo, lessonIdList);

                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.BeginTransaction(IsolationLevel.Snapshot);

                        //3、取消写生课Id对应的所有学生的课次
                        CancelLifeClass(unitOfWork, lifeClassInfo, lessonIdList);

                        //4、删除对应的写生课
                        TblTimLifeClassRepository lifeClassRepository = unitOfWork.GetCustomRepository<TblTimLifeClassRepository, TblTimLifeClass>();
                        lifeClassRepository.DeleteLifeClassById(_lifeClassId);

                        unitOfWork.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        unitOfWork.RollbackTransaction();
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 取消学生的某个课次
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <param name="lessonIdList">课程Id列表</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：
        /// 13. 系统不存在该写生课信息
        /// 14. 该学期已结束，不可取消
        /// 34. 写生课已取消
        /// </exception>
        public void Cancel(List<long> lessonIdList)
        {
            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_LIFE_CLASS, _lifeClassId.ToString()))
            {
                //1、校验
                var lifeClassInfo = _repository.Value.GetLifeClassInfo(_lifeClassId);
                CheckDataBeforeCancel(lifeClassInfo, lessonIdList);

                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.BeginTransaction(IsolationLevel.Snapshot);

                        //2、取消学生的写生排课
                        CancelLifeClass(unitOfWork, lifeClassInfo, lessonIdList);

                        unitOfWork.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        unitOfWork.RollbackTransaction();
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 取消写生排课
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-12-10</para>
        /// </summary>
        /// <param name="unitOfWork">事务单元</param>
        /// <param name="lifeClassInfo">写生课信息</param>
        /// <param name="lessonIdList">课程Id列表</param>
        private void CancelLifeClass(UnitOfWork unitOfWork, TblTimLifeClass lifeClassInfo, List<long> lessonIdList)
        {
            //调用课次操作
            LifeClassLessonFinisher lessonFinisher = new LifeClassLessonFinisher(lifeClassInfo, lessonIdList, unitOfWork);  //实例化课次生产者
            new LessonService(unitOfWork).Finish(lessonFinisher);//课次结束
        }

        /// <summary>
        /// 取消之前，先进行检查数据
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-10</para>
        /// </summary>
        /// <param name="lifeClassInfo">写生课信息</param>
        /// <param name="lessonIdList">课程Id列表</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：
        /// 13. 系统不存在该写生课信息
        /// 14. 该学期已结束，不可取消
        /// 34. 写生课已取消
        /// </exception>
        private void CheckDataBeforeCancel(TblTimLifeClass lifeClassInfo, List<long> lessonIdList)
        {
            //检查写生课排课是否存在
            if (lifeClassInfo == null)
            {
                throw new BussinessException(ModelType.Timetable, 13);
            }
            //检查学生写生课是否已取消，如已取消，不可再取消
            var lifeLessonList = _timLessonRepository.Value.GetByLessonIdTask(lessonIdList).Result;
            if (lifeLessonList.Any(a => a.Status == (int)LessonUltimateStatus.Invalid))
            {
                throw new BussinessException(ModelType.Timetable, 34);
            }
            //学期的结束日期小于当前日期，则不允许取消
            var termInfo = TermService.GetTermByTermId(lifeClassInfo.TermId);
            if (termInfo != null && termInfo.EndDate < DateTime.Now)
            {
                throw new BussinessException(ModelType.Timetable, 14);
            }
        }
        #endregion

        #region Change写生课
        /// <summary>
        /// 写生调课
        /// </summary>
        /// <param name="lessonId">要转的课次</param>
        /// <param name="classId">转入的班级</param>
        public void Change(long lessonId, long classId)
        {
            //1、校验

            //2、调用课次操作
            //先结束原来的,在产生新的课次
        }
        #endregion
    }
}
