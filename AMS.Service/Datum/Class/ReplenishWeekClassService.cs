/****************************************************************************\
所属系统:招生系统
所属模块:基础资料模块
创建时间:
作   者:
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using AMS.Core;
using AMS.Dto;
using AMS.Storage.Models;
using AMS.Storage.Repository;

namespace AMS.Service
{
    /// <summary>
    /// 补课周的班级服务
    /// <para>作    者：Huang GaoLiang</para>
    /// <para>创建时间：2019-03-12</para>
    /// </summary>
    internal class ReplenishWeekClassService : BaseClassService
    {
        /// <summary>
        /// 班级类型
        /// </summary>
        protected override ClassType ClassType => ClassType.ReplenishWeek;

        private readonly Lazy<TblDatClassRepository> _classRepository = new Lazy<TblDatClassRepository>();

        /// <summary>
        /// 班级编号
        /// </summary>
        private readonly string _classNo;

        /// <summary>
        /// 班级Id
        /// </summary>
        private readonly long _classId;

        /// <summary>
        /// 补课周补课构造函数
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-20</para>
        /// </summary>
        /// <param name="classId">班级Id</param>
        public ReplenishWeekClassService(long classId)
        {
            this._classId = classId;
        }

        private TblDatClass _tblDatClass;

        /// <summary>
        /// 班级信息
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-20</para>
        /// </summary>
        public TblDatClass TblDatClass
        {
            get
            {
                if (_tblDatClass == null)
                {
                    _tblDatClass = this.GetTblDatClass();
                }
                return _tblDatClass;
            }
        }

        /// <summary>
        /// 获取班级信息
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-20</para>
        /// </summary>
        /// <returns>班级信息</returns>
        private TblDatClass GetTblDatClass()
        {
            TblDatClass classInfo = _classRepository.Value.Load(_classId);
            return classInfo;
        }
    }
}
