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
using AMS.Core;
using AMS.Core.Constants;
using AMS.Core.Locks;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using System;

namespace AMS.Service
{
    /// <summary>
    /// 描述：课程调整--撤销排课
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-04</para>
    /// </summary>
    public class AdjustLessonRevokeService : BaseLessonAdjustService
    {
        /// <summary>
        /// 校区编号
        /// </summary>
        private readonly string _schoolId;

        /// <summary>
        /// 描述：撤销排课构造函数
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        public AdjustLessonRevokeService(string schoolId) : base(schoolId)
        {
            this._schoolId = schoolId;
        }


        /// <summary>
        /// 描述：撤销排课
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="iRequest">撤销排课请求参数</param>
        /// <exception>
        /// 异常ID：1,未找到数据
        /// 异常ID：46,该课程已考勤，不能撤销！
        /// </exception>
        public override void Adjust(IAdjustLessonRequest iRequest)
        {
            AdjustRevokeRequest request = iRequest as AdjustRevokeRequest;

            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_AMSSCHOOLSTUDENT, this._schoolId, request.StudentId.ToString()))
            {
                LessonFinisherInfo info = new LessonFinisherInfo();

                // 根据课次编号获取学生课次信息
                var timLessonStudent = new StudentTimetableService(this._schoolId, request.StudentId).GetStudentTimLessonByLessId(request.LessonId);
                if (timLessonStudent == null)
                {
                    throw new BussinessException(ModelType.Default, 1);
                }

                // 如果考勤状态是已考勤或者是已补签
                if (timLessonStudent.AttendStatus == (int)AttendStatus.Normal || timLessonStudent.AdjustType == (int)AdjustType.SUPPLEMENTCONFIRMED)
                {
                    throw new BussinessException(ModelType.Timetable, 46);
                }

                // 根据课次编号获取学生排课记录信息
                TblTimLesson timLesson = new StudentTimetableService(this._schoolId, request.StudentId).GetTimLessonById(request.LessonId);
                TblTimAdjustLesson timAdjustLesson = new TblTimAdjustLesson
                {
                    AdjustLessonId = IdGenerator.NextId(),
                    SchoolId = this._schoolId,
                    BatchNo = IdGenerator.NextId(),
                    FromLessonId = timLesson.LessonId,
                    FromTeacherId = timLesson.TeacherId,
                    ToTeacherId = "",
                    StudentId = request.StudentId,
                    ClassRoomId = timLesson.ClassRoomId,
                    ClassId = timLesson.ClassId,
                    SchoolTimeId = 0,
                    ClassDate = timLesson.ClassDate,
                    ClassBeginTime = timLesson.ClassBeginTime,
                    ClassEndTime = timLesson.ClassEndTime,
                    BusinessType = (int)LessonBusinessType.CancelMakeLess,
                    Remark = LessonProcessConstants.Remark,
                    Status = (int)TimAdjustLessonStatus.Invalid,
                    CreateTime = DateTime.Now,
                };

                using (var unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.BeginTransaction();

                        // 1、写入调整课次业务表
                        unitOfWork.GetCustomRepository<TblTimAdjustLessonRepository, TblTimAdjustLesson>().Add(timAdjustLesson);

                        // 2、调用课次服务
                        var adjustLessonRevokeFinisher = new AdjustLessonRevokeFinisher(this._schoolId, request.StudentId, request.LessonId, unitOfWork);
                        LessonService lessonService = new LessonService(unitOfWork);
                        lessonService.Finish(adjustLessonRevokeFinisher);

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

    }
}
