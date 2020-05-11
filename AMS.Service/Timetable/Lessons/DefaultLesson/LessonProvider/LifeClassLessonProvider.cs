using System;
using System.Collections.Generic;
using System.Text;
using AMS.Core;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;

namespace AMS.Service
{
    /// <summary>
    /// 写生排课数据提供者
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public abstract class LifeClassLessonProvider : BService, ILessonProvider
    {
        protected TblTimLifeClass _entity;                                                                                    //写生课对象
        protected UnitOfWork _unitOfWork;                                                                                  //工作单元        

        /// <summary>
        /// 写生排课数据提供者实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <param name="entity">写生课对象</param>
        /// <param name="unitOfWork">工作单元</param>
        protected LifeClassLessonProvider(TblTimLifeClass entity, UnitOfWork unitOfWork = null)
        {
            this._entity = entity;
            this._unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 写生课课次类型
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        public virtual int BusinessType => (int)LessonBusinessType.LifeClassMakeLesson;
    }
}
