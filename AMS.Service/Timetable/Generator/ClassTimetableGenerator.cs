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
using AMS.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Service
{
    /// <summary>
    /// 根据班级ID创建生成器
    /// </summary>
    public class ClassTimetableGenerator : ITimetableGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="classId"></param>
        public ClassTimetableGenerator(long classId)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ClassTimetableGeneratorResponse> Generator()
        {
            return new List<ClassTimetableGeneratorResponse>();
        }
    }
}
