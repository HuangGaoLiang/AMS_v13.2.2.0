/****************************************************************************\
所属系统:招生系统
所属模块:家校互联
创建时间:
作   者:
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/
using AMS.Storage.Models;
using AMS.Storage.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Service.Hss
{
    /// <summary>
    /// 描述：家长账户服务
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-13</para>
    /// </summary>
    public class PassportService : AMS.Core.BService
    {
        private readonly Lazy<TblHssPassportRepository> _repository = new Lazy<TblHssPassportRepository>();


        #region IsMobileBind 检查账户是否存在微信号绑定
        /// <summary>
        /// 检查账户是否存在微信号绑定
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="mobiles">一组账号,账户就是手机号码</param>
        /// <returns></returns>
        internal bool IsMobileBind(List<string> mobiles)
        {
            return _repository.Value.IsMobileBind(mobiles);
        }
        #endregion

        #region GetByUserCodes 根据用户账号查询
        /// <summary>
        /// 根据用户账号查询
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="mobiles">一组用户账号，一般为手机号</param>
        /// <returns>账户实体信息集合</returns>
        public List<TblHssPassport> GetByUserCodes(List<string> mobiles)
        {
            return _repository.Value.GetByUserCodes(mobiles);
        }
        #endregion
    }
}
