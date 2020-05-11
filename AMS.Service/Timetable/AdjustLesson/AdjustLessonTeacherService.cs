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
    /// 描述：课程调整业务--老师代课
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-04</para>
    /// </summary>
    public class AdjustLessonTeacherService : BaseLessonAdjustService
    {


        /// <summary>
        /// 描述：实例化一个课程调整老师代课对象
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-8</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>       
        public AdjustLessonTeacherService(string schoolId) : base(schoolId)
        {

        }

        #region Adjust 老师代课信息保存
        /// <summary>
        /// 描述：老师代课信息保存
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-8</para>
        /// </summary>
        /// <param name="iRequest">要保存的代课老师信息</param>
        public override void Adjust(IAdjustLessonRequest iRequest)
        {
            AdjustTeacherRequest request = iRequest as AdjustTeacherRequest;

            using (var unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.BeginTransaction();
                    var lessonRepository = unitOfWork.GetCustomRepository<TblTimLessonRepository, TblTimLesson>();

                    LessonService lessonService = new LessonService(unitOfWork);                    
                    //1.要更换老师的课次
                    var changeLessonList = lessonRepository.GetTimLessonByClassIdList(request.LessonInfoList.Select(x => x.ClassId),request.LessonInfoList.Select(x=>x.ClassDate), LessonUltimateStatus.Normal);

                    //2.向调整课次业务表添加数据
                    var adjustLessonList = AddAdjustLessonInfo(request.TeacherId, changeLessonList, unitOfWork);

                    //3.提供要销毁的课次对象                    
                    var lessonTeacherFinisher = new AdjustLessonTeacherFinisher(adjustLessonList, changeLessonList);
                    lessonService.Finish(lessonTeacherFinisher);
                    //4.提供要更新的课次
                    var lessonCreator = new AdjustLessonTeacherCreator(request.TeacherId, adjustLessonList, changeLessonList);
                    if (lessonCreator.IsLessonData)  //正常课次有数据则创建
                    {
                        lessonService.Create(lessonCreator);          //正常课次
                    }
                    var replenishLessonService = new ReplenishLessonService(unitOfWork);
                    if (lessonCreator.IsReplenishLessonData) //补课调课有数据则创建
                    {
                        replenishLessonService.Create(lessonCreator); //补课调课
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
        /// <param name="teachaerId">老师Id</param>
        /// <param name="timLessonList">要调整的课次列表</param>
        /// <param name="unitOfWork">事务</param>
        /// <returns>添加到数据库的课次调整表数据</returns>
        private List<TblTimAdjustLesson> AddAdjustLessonInfo(string teachaerId, List<TblTimLesson> timLessonList, UnitOfWork unitOfWork)
        {
            var adjustLessonList = new List<TblTimAdjustLesson>();
            var batchNo = IdGenerator.NextId();
            foreach (var item in timLessonList)
            {
                var adjuseLessonEntity = new TblTimAdjustLesson
                {
                    AdjustLessonId = IdGenerator.NextId(),
                    BatchNo = batchNo,
                    BusinessType = (int)LessonBusinessType.AdjustLessonTeacher,
                    ClassDate = item.ClassDate,
                    ClassBeginTime = item.ClassBeginTime,
                    ClassEndTime = item.ClassEndTime,
                    ClassId = item.ClassId,
                    ClassRoomId = item.ClassRoomId,
                    FromLessonId = item.LessonId,
                    FromTeacherId = item.TeacherId,
                    SchoolTimeId = 0,
                    SchoolId = item.SchoolId,
                    StudentId = item.StudentId,
                    ToTeacherId = teachaerId,    //更换之后的老师Id
                    Status = (int)TimAdjustLessonStatus.Normal,
                    Remark = EnumName.GetDescription(typeof(LessonBusinessType), (int)LessonBusinessType.AdjustLessonTeacher),
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
