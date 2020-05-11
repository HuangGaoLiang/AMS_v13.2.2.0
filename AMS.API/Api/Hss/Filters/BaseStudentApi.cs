/****************************************************************************\
所属系统:招生系统
所属模块:家校互联--学生认证
创建时间:
作   者:
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/
using AMS.Core;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace AMS.API.Api.Hss.Filters
{
    /// <summary>
    /// 描述：学生控制器基本,学生相关的家校互联控制器都要继承此类。
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-02-26</para>
    /// </summary>
    [DefaultAuthorize]
    public class BaseStudentApi : ControllerBase
    {


        private HssUserPrincipal _userPrincipal;  //用户信息

        /// <summary>
        /// 业务ID
        /// </summary>
        public byte BussinessId => BusinessConfig.BussinessID;

        /// <summary>
        /// 表示一个学生信息
        /// </summary>
        protected HssUserPrincipal UserPrincipal
        {
            get
            {
                if (_userPrincipal == null)
                {
                    AuthenticationService service = new AuthenticationService(base.HttpContext);
                    _userPrincipal = service.GetDefaultUser();
                }
                return _userPrincipal;
            }
        }



        /// <summary>
        /// 保护构造函数,外部不可以直接创建此对象
        /// </summary>
        protected BaseStudentApi()
        {
        }
    }
}
