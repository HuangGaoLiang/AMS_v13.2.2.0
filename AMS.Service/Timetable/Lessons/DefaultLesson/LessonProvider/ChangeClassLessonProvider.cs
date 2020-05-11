using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Service
{
    /// <summary>
    /// 描述：转班课次数据提供者
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-7</para>
    /// </summary>
    public abstract class ChangeClassLessonProvider: ILessonProvider
    {

        protected readonly TblTimChangeClass _entity;       //转班信息实体
        protected readonly UnitOfWork _unitOfWork;          //工作单元事务

        /// <summary>
        /// 描述：实例化转班课次数据提供者
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="entity">转班信息d</param>
        /// <param name="unitOfWork">工作单元事务</param>
        protected ChangeClassLessonProvider(TblTimChangeClass entity, UnitOfWork unitOfWork)
        {
            this._entity = entity;
            this._unitOfWork = unitOfWork;
        }
        
        /// <summary>
        /// 业务类型
        /// </summary>
        public int BusinessType => (int)LessonBusinessType.ChangeClass;
    }
}
