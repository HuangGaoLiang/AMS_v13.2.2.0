using System;
using System.Collections.Generic;
using System.Text;
using Jerrisoft.Platform.Exception;

namespace AMS.Core
{

    /*---------------------------------------------------------------------------------------------------
    * 项目名称 ：$Projectname$
    * 项目描述 ：
    * 类 名 称 ：BussinessException
    * 类 描 述 ：
    * 所在的域 ：YMM-杰人-技术总
    * 命名空间 ：AMS.Storage
    * 机器名称 ：YMM-杰人-技术总 
    * CLR 版本 ：4.0.30319.42000
    * 作    者 ：user
    * 创建时间 ：2018/7/23 9:37:06 
    * Ver         变更日期                       负责人          变更内容
    * ─────────────────────────────────────────────────
    * V1.0.0.0    2018/7/23 9:37:06       user

    *******************************************************************

    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘

    *******************************************************************
    --------------------------------------------------------------------------------------------------------*/

    /// <summary>
    /// 业务异常
    /// </summary>
    public class BussinessException : BaseException
    {
        public override byte BussinessId
        {
            get
            {
                return BusinessConfig.BussinessID;
            }
        }


        public BussinessException(byte modelID, ushort innerExceptionID, string message = null)
            : base(modelID, innerExceptionID, message)
        {

        }

        public BussinessException(ModelType modelID, ushort innerExceptionID, string message = null)
           : base((byte)modelID, innerExceptionID, message)
        {

        }
    }
}
