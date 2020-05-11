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
    /// 描述：课程调整--全校上课日期调整
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-04</para>
    /// </summary>
    public class AdjustLessonSchoolClassTimeService : BaseLessonAdjustService
    {
        

        /// <summary>
        /// 描述：实例化全校上课日期调整
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-8</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        public AdjustLessonSchoolClassTimeService(string schoolId) : base(schoolId)
        {

        }

        #region Adjust 全校上课日期调整信息保存
        /// <summary>
        /// 描述：全校上课日期调整信息保存
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-8</para>
        /// </summary>
        /// <param name="iRequest">要保存的课程调整信息</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常Id:61,异常描述：调整日期不能小于当前日期
        /// </exception>
        public override void Adjust(IAdjustLessonRequest iRequest)
        {
            AdjustSchoolClassTimeRequest request = iRequest as AdjustSchoolClassTimeRequest;
            if (request.AdjustDate <= DateTime.Now)  //如果调整时间小于当前时间，则抛出异常
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
                    //1.要更换全校上课日期的课次                    
                    var changeLessonList = lessonRepository.GetTimLessonByClassIdList(request.LessonInfoList.Select(x=>x.ClassId),request.LessonInfoList.Select(x=>x.ClassDate), LessonUltimateStatus.Normal);

                    //2.向调整课次业务表添加数据
                    var adjustLessonList = AddAdjustLessonInfo(request.AdjustDate, changeLessonList, unitOfWork);
                    //3.提供要销毁的课次对象
                    var schoolClassTimeFinisher = new AdjustLessonSchoolClassTimeFinisher(adjustLessonList, changeLessonList);
                    lessonService.Finish(schoolClassTimeFinisher);
                    //4.提供要更新的课次
                    var schoolClassTimeCreator = new AdjustLessonSchoolClassTimeCreator(request.AdjustDate, adjustLessonList, changeLessonList);
                    if (schoolClassTimeCreator.IsLessonData)
                    {
                        lessonService.Create(schoolClassTimeCreator);                        //正常课次
                    }
                    var replenishLessonService = new ReplenishLessonService(unitOfWork);
                    if (schoolClassTimeCreator.IsReplenishLessonData)
                    {
                        replenishLessonService.Create(schoolClassTimeCreator);               //补课调课
                    }
                    
                    unitOfWork.CommitTransaction();
                }
                catch (Exception ex)
                {
                    unitOfWork.RollbackTransaction();
                    throw ex;
                }
            }
        }
        #endregion


        #region AddAdjustLessonInfo 向课次调整信息表添加记录
        /// <summary>
        /// 描述：向课次调整信息表添加记录
        /// <para>作   者：瞿琦</para>
        /// <para>创建时间：2019-3-8</para>
        /// </summary>
        /// <param name="adjustDate">要调整的日期</param>
        /// <param name="timLessonList">要调整的课次列表</param>
        /// <param name="unitOfWork">工作单元事务</param>
        /// <returns>添加到数据库的课次调整表数据</returns>
        private List<TblTimAdjustLesson> AddAdjustLessonInfo(DateTime adjustDate, List<TblTimLesson> timLessonList, UnitOfWork unitOfWork)
        {
            var adjustLessonList = new List<TblTimAdjustLesson>();
            var batchNo = IdGenerator.NextId();
            foreach (var item in timLessonList)
            {
                var adjuseLessonEntity = new TblTimAdjustLesson
                {
                    AdjustLessonId = IdGenerator.NextId(),
                    BatchNo = batchNo,
                    BusinessType = (int)LessonBusinessType.AdjustLessonSchoolClassTime,
                    ClassDate = adjustDate,                     //调整后的日期
                    ClassBeginTime = item.ClassBeginTime,
                    ClassEndTime = item.ClassEndTime,
                    ClassId = item.ClassId,
                    ClassRoomId = item.ClassRoomId,
                    FromLessonId = item.LessonId,
                    FromTeacherId = item.TeacherId,
                    SchoolTimeId = 0,
                    SchoolId = item.SchoolId,
                    StudentId = item.StudentId,
                    ToTeacherId = item.TeacherId,
                    Status = (int)TimAdjustLessonStatus.Normal,
                    Remark = EnumName.GetDescription(typeof(LessonBusinessType), (int)LessonBusinessType.AdjustLessonSchoolClassTime),
                    CreateTime = DateTime.Now
                };
                adjustLessonList.Add(adjuseLessonEntity);
            }
            unitOfWork.GetCustomRepository<TblTimAdjustLessonRepository, TblTimAdjustLesson>().Add<TblTimAdjustLesson>(adjustLessonList);

            return adjustLessonList;
        }
        #endregion
        
    }
}
