/****************************************************************************\
所属系统:招生系统
所属模块:课表模块
创建时间:
作   者:
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/
using AMS.Anticorrosion.HRS;
using AMS.Core;
using AMS.Core.Constants;
using AMS.Dto;
using AMS.Service.Hss;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMS.Service
{
    /// <summary>
    /// 描述：课程调整--班级上课时间调整
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-04</para>
    /// </summary>
    public class AdjustLessonClassTimeService : BaseLessonAdjustService
    {


        /// <summary>
        /// 描述：实例化一个班级上课时间调整课次服务
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-8</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        public AdjustLessonClassTimeService(string schoolId) : base(schoolId)
        {

        }

        #region Adjust 班级上课时间调整课次信息保存
        /// <summary>
        /// 描述：班级上课时间调整课次信息保存
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-8</para>
        /// </summary>
        /// <param name="iRequest">要保存的课次调整信息</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常Id:7,异常描述：上课时间段编号不能为空
        /// 异常Id:61,异常描述：调整日期不能小于当前日期
        /// </exception>
        public override void Adjust(IAdjustLessonRequest iRequest)
        {
            var request = iRequest as AdjustClassTimeRequest;
            if (request.NewClassDate <= DateTime.Now)  //如果调整日期小于当前日期，则抛出异常
            {
                throw new BussinessException(ModelType.Timetable, 61);
            }
            using (var unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.BeginTransaction();
                    var lessonRepository = unitOfWork.GetCustomRepository<TblTimLessonRepository, TblTimLesson>();
                    LessonService lessonService = new LessonService(unitOfWork);
                    //获取班级上课时间段
                    var classTimeList = this.GetClassTimesList(request.ClassId);
                    if (!classTimeList.Any())
                    {
                        throw new BussinessException(ModelType.Datum, 7);
                    }
                    //1.获取要更换时间段的课次
                    var changeLessonList = lessonRepository.GetTimLessonByClassIdList(new List<long>() { request.ClassId },new List<DateTime> {request.OldClassDate }, LessonUltimateStatus.Normal);

                    //2.向调整课次业务表添加数据
                    var adjustLessonList = AddAdjustLessonInfo(request, changeLessonList, classTimeList, unitOfWork);

                    //3.提供要销毁的课次对象
                    var lessonClassTimeFinisher = new AdjustLessonClassTimeFinisher(adjustLessonList, changeLessonList);
                    lessonService.Finish(lessonClassTimeFinisher);

                    //4.提供要更新的课次
                    var lessonClassTimeCreator = new AdjustLessonClassTimeCreator(request, adjustLessonList, changeLessonList, classTimeList);
                    if (lessonClassTimeCreator.IsLessonData)
                    {
                        lessonService.Create(lessonClassTimeCreator);                       //正常课次
                    }
                    var replenishLessonService = new ReplenishLessonService(unitOfWork);
                    if (lessonClassTimeCreator.IsReplenishLessonData)
                    {
                        replenishLessonService.Create(lessonClassTimeCreator);               //补课调课
                    }
                   
                    unitOfWork.CommitTransaction();

                }
                catch (Exception)
                {
                    unitOfWork.RollbackTransaction();
                    throw;
                }
            }
        }
        #endregion

        #region GetClassTimesList 获取班级上课的时间段
        /// <summary>
        ///描述： 获取班级上课的时间段
        ///<para>作者：瞿琦</para>
        ///<para>创建时间：2019-3-16</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        /// <returns>上课时间段列表</returns>
        private List<TblDatSchoolTime> GetClassTimesList(long classId)
        {
            //班级上课时间
            var classTimesList = new TimClassTimeService(classId).GetSchoolTimeIds();
            var schoolTimeList = SchoolTimeService.GetBySchoolTimeIds(classTimesList);
            return schoolTimeList;
        }
        #endregion


        #region AddAdjustLessonInfo 向课次调整信息表添加记录
        /// <summary>
        /// 描述：向课次调整信息表添加记录
        /// <para>作   者：瞿琦</para>
        /// <para>创建时间：2019-3-8</para>
        /// </summary>
        /// <param name="request">要保存的课次调整信息</param>
        /// <param name="timLessonList">要调整的课次列表</param>
        /// <param name="classTimeList">上课时间段集合</param>
        /// <param name="unitOfWork">事务</param>
        /// <returns>添加到数据库的课次调整表数据</returns>
        private List<TblTimAdjustLesson> AddAdjustLessonInfo(AdjustClassTimeRequest request, List<TblTimLesson> timLessonList, List<TblDatSchoolTime> classTimeList, UnitOfWork unitOfWork)
        {
            var adjustLessonList = new List<TblTimAdjustLesson>();
            var batchNo = IdGenerator.NextId();
            foreach (var totalItem in timLessonList)
            {
                //开始上课时间
                var beginTime = request.NewClassBeginTime;
                //时间间隔
                var timeInterval = 0;
                var studentLessonList = timLessonList.Where(x => x.ClassId == totalItem.ClassId && x.ClassDate == totalItem.ClassDate && x.StudentId == totalItem.StudentId);
                foreach (var studentLessonItem in studentLessonList)
                {
                    var dateBeginTime = DateTime.Parse($"{request.NewClassDate:yyyy-MM-dd} {beginTime}");
                    var classEndTime = dateBeginTime.AddMinutes(classTimeList.FirstOrDefault().Duration);

                    var adjuseLessonEntity = new TblTimAdjustLesson
                    {
                        AdjustLessonId = IdGenerator.NextId(),
                        BatchNo = batchNo,
                        BusinessType = (int)LessonBusinessType.AdjustLessonClassTime,
                        ClassDate = request.NewClassDate,           //调整后的上课日期               
                        ClassBeginTime = beginTime,                 //调整后的上课时间
                        ClassEndTime = classEndTime.ToShortTimeString().ToString(),   //调整后的下课时间
                        ClassId = studentLessonItem.ClassId,
                        ClassRoomId = studentLessonItem.ClassRoomId,
                        FromLessonId = studentLessonItem.LessonId,
                        FromTeacherId = studentLessonItem.TeacherId,
                        SchoolTimeId = 0,   //此次业务是自定义时间段，为0
                        SchoolId = studentLessonItem.SchoolId,
                        StudentId = studentLessonItem.StudentId,
                        ToTeacherId = studentLessonItem.TeacherId,
                        Status = (int)TimAdjustLessonStatus.Normal,
                        Remark = EnumName.GetDescription(typeof(LessonBusinessType), (int)LessonBusinessType.AdjustLessonClassTime),
                        CreateTime = DateTime.Now
                    };
                    adjustLessonList.Add(adjuseLessonEntity);

                    if (classTimeList.Count > 1)
                    {
                        var lessonTime1 = DateTime.Parse($"{request.NewClassDate:yyyy-MM-dd} {classTimeList[0].EndTime}");
                        var lessonTime2 = DateTime.Parse($"{request.NewClassDate:yyyy-MM-dd} {classTimeList[1].BeginTime}");

                        timeInterval = (int)(lessonTime2 - lessonTime1).TotalMinutes;
                    }
                    beginTime = classEndTime.AddMinutes(timeInterval).ToShortTimeString().ToString();
                }
            }
            unitOfWork.GetCustomRepository<TblTimAdjustLessonRepository, TblTimAdjustLesson>().Add<TblTimAdjustLesson>(adjustLessonList);
            return adjustLessonList;
        }
        #endregion
    }
}
