using System;
using System.Collections.Generic;
using System.Linq;
using AMS.Core;
using AMS.Core.Locks;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;

namespace AMS.Service
{
    /// <summary>
    /// 课次调整服务(补课/调课)
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-13</para>
    /// </summary>
    public class ReplenishLessonService : BaseLessonService
    {
        private readonly Lazy<TblTimReplenishLessonRepository> _replenishLessonRepository;
        private readonly Lazy<TblTimLessonProcessRepository> _lessonProcessRepository;
        private readonly Lazy<TblTimLessonRepository> _lessonRepository;
        private readonly Lazy<TblTimLessonStudentRepository> _lessonStudentRepository;

        /// <summary>
        /// 构造一个课次调整服务
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="unitOfWork">事物单元</param>
        public ReplenishLessonService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
            _replenishLessonRepository = new Lazy<TblTimReplenishLessonRepository>
                (unitOfWork.GetCustomRepository<TblTimReplenishLessonRepository, TblTimReplenishLesson>);
            _lessonProcessRepository = new Lazy<TblTimLessonProcessRepository>
                (unitOfWork.GetCustomRepository<TblTimLessonProcessRepository, TblTimLessonProcess>);
            _lessonRepository = new Lazy<TblTimLessonRepository>
                (unitOfWork.GetCustomRepository<TblTimLessonRepository, TblTimLesson>);
            _lessonStudentRepository = new Lazy<TblTimLessonStudentRepository>
                (unitOfWork.GetCustomRepository<TblTimLessonStudentRepository, TblTimLessonStudent>);
        }

        /// <summary>
        /// 调整课次创建
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="creator">调整课次的产生者接口</param>
        public void Create(IReplenishLessonCreator creator)
        {
            List<ReplenishLessonCreatorInfo> lessonCreatorInfoList = creator.GetReplenishLessonCreatorInfo();

            if (lessonCreatorInfoList == null || !lessonCreatorInfoList.Any())
            {
                //创建的课次为空
                throw new BussinessException(ModelType.Timetable, 35);
            }

            var lessonCreatorInfo = lessonCreatorInfoList.FirstOrDefault();

            List<long> outLessonIdList = lessonCreatorInfoList.Select(x => x.OutLessonId).ToList();

            var outReplenishLessonList = _replenishLessonRepository.Value.GetLessonListByLessonId(outLessonIdList);

            List<TblTimLesson> lessonList = new List<TblTimLesson>();                            //课次
            List<TblTimLessonProcess> lessonProceseList = new List<TblTimLessonProcess>();       //课次过程
            List<TblTimReplenishLesson> replenishLessonList = new List<TblTimReplenishLesson>(); //课次调整

            foreach (var m in lessonCreatorInfoList)
            {
                //课次
                var lessonModel = this.GetTblTimLesson(m);
                lessonList.Add(lessonModel);

                //课次过程
                var lessonProcessModel = this.GetTblTimLessonProcess(lessonModel);
                lessonProceseList.Add(lessonProcessModel);

                //补课
                var replenishLessonModel = this.GetTblTimReplenishLesson(outReplenishLessonList, m, lessonModel);
                replenishLessonList.Add(replenishLessonModel);
            }

            lock (LocalThreadLock.GetLockKeyName(
                LockKeyNames.LOCK_AMSSCHOOLSTUDENT, lessonCreatorInfo.SchoolId,
                lessonCreatorInfo.StudentId.ToString()))
            {
                //常规课校验排课时间是否冲突
                base.VerifyClassTimeCross(lessonCreatorInfo.SchoolId,
                    lessonCreatorInfo.StudentId, this.GetVerifyClassTimeList(lessonCreatorInfoList),
                    lessonCreatorInfo.LessonType);

                _replenishLessonRepository.Value.Add(replenishLessonList);
                _lessonProcessRepository.Value.Add(lessonProceseList);
                _lessonRepository.Value.Add(lessonList);

                this.UpdateAdjustType(lessonCreatorInfoList, outReplenishLessonList);
            }

            creator.AfterReplenishLessonCreate();
        }

        /// <summary>
        /// 获取补课信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="outReplenishLessonList">转出的补课信息列表</param>
        /// <param name="replenishLessonCreatorInfo">课次调整对象</param>
        /// <param name="lessonModel">新构建的课次信息</param>
        /// <returns>补课信息构建</returns>
        private TblTimReplenishLesson GetTblTimReplenishLesson(
            List<TblTimReplenishLesson> outReplenishLessonList,
            ReplenishLessonCreatorInfo replenishLessonCreatorInfo,
            TblTimLesson lessonModel)
        {
            var outReplenishLesson = outReplenishLessonList
                .FirstOrDefault(x => x.LessonId == replenishLessonCreatorInfo.OutLessonId);

            TblTimReplenishLesson replenishLessonModel = new TblTimReplenishLesson
            {
                AdjustType = (int)AdjustType.DEFAULT,
                AttendDate = null,
                AttendStatus = (int)AttendStatus.NotClockIn,
                AttendUserType = (int)AttendUserType.DEFAULT,
                CreateTime = DateTime.Now,
                LessonId = lessonModel.LessonId,
                ParentLessonId = replenishLessonCreatorInfo.OutLessonId,
                ReplenishCode = string.Empty,
                ReplenishLessonId = IdGenerator.NextId(),
                RootLessonId = outReplenishLesson == null ? replenishLessonCreatorInfo.OutLessonId : outReplenishLesson.RootLessonId,
                SchoolId = replenishLessonCreatorInfo.SchoolId,
                StudentId = replenishLessonCreatorInfo.StudentId,
                UpdateTime = DateTime.Now
            };

            return replenishLessonModel;
        }

        /// <summary>
        /// 获取课次变更数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="lessonModel">新构建的课次信息</param>
        /// <returns>课次变更数据</returns>
        private TblTimLessonProcess GetTblTimLessonProcess(TblTimLesson lessonModel)
        {
            TblTimLessonProcess lessonProcessModel = new TblTimLessonProcess
            {
                SchoolId = lessonModel.SchoolId,
                BusinessId = lessonModel.BusinessId,
                BusinessType = lessonModel.BusinessType,
                LessonId = lessonModel.LessonId,
                LessonProcessId = IdGenerator.NextId(),
                ProcessStatus = (int)ProcessStatus.Create,
                Remark = string.Empty,
                ProcessTime = DateTime.Now,
                CreateTime = DateTime.Now
            };

            return lessonProcessModel;
        }

        /// <summary>
        /// 获取课次信息数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="model">课次调整数据源</param>
        /// <returns>课次信息数据</returns>
        private TblTimLesson GetTblTimLesson(ReplenishLessonCreatorInfo model)
        {
            DateTime classBeginDate; //上课开始时间
            DateTime classEndDate;   //上课结束时间
            if (model.LessonType == LessonType.RegularCourse)
            {
                classBeginDate = DateTime.Parse($"{model.ClassDate:yyyy-MM-dd} {model.ClassBeginTime}");
                classEndDate = DateTime.Parse($"{model.ClassDate:yyyy-MM-dd} {model.ClassEndTime}");
            }
            else
            {
                classBeginDate = DateTime.Parse(model.ClassBeginTime);
                classEndDate = DateTime.Parse(model.ClassEndTime);
            }

            TblTimLesson lessonModel = new TblTimLesson
            {
                LessonId = IdGenerator.NextId(),
                BusinessId = model.BusinessId,
                BusinessType = (int)model.BusinessType,
                ClassBeginTime = model.ClassBeginTime,
                ClassDate = model.ClassDate,
                ClassEndTime = model.ClassEndTime,
                ClassId = model.ClassId,
                ClassRoomId = model.ClassRoomId,
                CourseId = model.CourseId,
                CourseLevelId = model.CourseLevelId,
                EnrollOrderItemId = model.EnrollOrderItemId,
                SchoolId = model.SchoolId,
                StudentId = model.StudentId,
                TeacherId = model.TeacherId,
                TermId = model.TermId,
                LessonType = (int)model.LessonType,
                LessonCount = model.LessonCount,
                Status = (int)LessonUltimateStatus.Normal,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                ClassBeginDate = classBeginDate,
                ClassEndDate = classEndDate
            };
            return lessonModel;
        }

        /// <summary>
        /// 上课时间转换
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="lessonCreatorInfoList">课次调整列表</param>
        /// <returns>上课时间段列表</returns>
        private List<VerifyClassTime> GetVerifyClassTimeList(IEnumerable<ReplenishLessonCreatorInfo> lessonCreatorInfoList)
        {
            return lessonCreatorInfoList.Select(x => new VerifyClassTime
            {
                ClassDate = x.ClassDate,
                ClassBeginTime = x.ClassBeginTime,
                ClassEndTime = x.ClassEndTime
            }).ToList();
        }

        /// <summary>
        /// 更新原课次的调整状态
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="lessonCreatorInfoList">课次调整数据源列表</param>
        /// <param name="outReplenishLessonList">原课次基础信息列表</param>
        private void UpdateAdjustType(
            List<ReplenishLessonCreatorInfo> lessonCreatorInfoList,
            List<TblTimReplenishLesson> outReplenishLessonList)
        {
            var info = lessonCreatorInfoList.FirstOrDefault();

            //补课、调课、补课周补课要把原课次修改为已补课、已调课
            if (
                info.BusinessType == LessonBusinessType.RepairLesson ||
                info.BusinessType == LessonBusinessType.AdjustLessonReplenishWeek ||
                info.BusinessType == LessonBusinessType.AdjustLessonChange)
            {
                var outLessonIdList = lessonCreatorInfoList.Select(x => x.OutLessonId).ToList();
                var replenishLessonList = outReplenishLessonList.Select(x => x.LessonId).ToList();

                //差集 学生正常考勤表的课次Id
                var expectedList = outLessonIdList.Except(replenishLessonList).ToList();
                if (expectedList.Any())
                {
                    //更新正常课次考勤表
                    _lessonStudentRepository
                        .Value.UpdateAdjustTypeByLessonId(expectedList,
                        AdjustType.DEFAULT, this.GetAdjustType(info.BusinessType));
                }


                if (replenishLessonList.Any())
                {
                    //更新调整课次考勤表
                    _replenishLessonRepository.Value
                        .UpdateAdjustTypeByLessonId(replenishLessonList, AdjustType.DEFAULT, this.GetAdjustType(info.BusinessType));
                }
            }
        }

        /// <summary>
        /// 获取课次调整状态
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="lessonBusinessType">业务类型</param>
        /// <returns>是否安排补课/调课状态</returns>
        private AdjustType GetAdjustType(LessonBusinessType lessonBusinessType)
        {
            switch (lessonBusinessType)
            {
                case LessonBusinessType.RepairLesson:
                    return AdjustType.MAKEUP;
                case LessonBusinessType.AdjustLessonReplenishWeek:
                    return AdjustType.SUPPLEMENTARYWEEK;
                case LessonBusinessType.AdjustLessonChange:
                    return AdjustType.ADJUST;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lessonBusinessType), lessonBusinessType, null);
            }
        }
    }
}
