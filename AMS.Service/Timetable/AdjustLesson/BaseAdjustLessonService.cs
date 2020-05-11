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
using AMS.Dto;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using System.Collections.Generic;
using System.Linq;

namespace AMS.Service
{
    /// <summary>
    /// 描述：课程调整基类
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-04</para>
    /// </summary>
    public abstract class BaseLessonAdjustService
    {
        private string _schoolId;        //校区Id
        

        /// <summary>
        /// 课程调整基类构造函数
        /// </summary>
        /// <param name="schoolId"></param>
        protected BaseLessonAdjustService(string schoolId)
        {
            _schoolId = schoolId;
           
        }

        /// <summary>
        /// 描述：调整
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-3-8</para>
        /// </summary>
        /// <param name="iRequest">要保存的课程调整信息</param>
        public abstract void Adjust(IAdjustLessonRequest iRequest);


        internal static void GetSsssgg()
        {

        }
    }
}