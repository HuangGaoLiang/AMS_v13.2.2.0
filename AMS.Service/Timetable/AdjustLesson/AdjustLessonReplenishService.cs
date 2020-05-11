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
using System;
using AMS.Core.Locks;
using AMS.Dto;
using AMS.Storage;

namespace AMS.Service
{
    /// <summary>
    /// 描述：课程调整--补课
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-04</para>
    /// </summary>
    internal class AdjustLessonReplenishService : BaseLessonAdjustService
    {
        private readonly string _schoolId; //校区Id

        /// <summary>
        /// 构建一个补课调整服务
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        public AdjustLessonReplenishService(string schoolId) : base(schoolId)
        {
            this._schoolId = schoolId;
        }

        /// <summary>
        /// 补课调整
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        /// <param name="iRequest">补课调整参数</param>
        public override void Adjust(IAdjustLessonRequest iRequest)
        {
            //1、涉及到学生课次开启线程锁
            //2、构建需要补课的课次信息
            //3、调用课次调整服务进行学生补课
            AdjustReplenishRequest request = iRequest as AdjustReplenishRequest;
            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_AMSSCHOOLSTUDENT, _schoolId, request.StudentId.ToString()))
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.BeginTransaction();
                        //构建需要补课的课次信息
                        AdjustLessonReplenishCreator adjustLessonReplenishCreator =
                            new AdjustLessonReplenishCreator(_schoolId,request, unitOfWork);

                        //调用课次调整服务进行学生补课
                        ReplenishLessonService service = new ReplenishLessonService(unitOfWork);
                        service.Create(adjustLessonReplenishCreator);

                        unitOfWork.CommitTransaction();
                    }
                    catch(Exception e)
                    {
                        unitOfWork.RollbackTransaction();
                        throw e;
                    }
                }
            }
        }
    }
}
