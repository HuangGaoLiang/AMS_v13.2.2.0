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
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AMS.Storage.Context;
using AMS.Storage.Models;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 家校互联家长登陆账户仓储
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-06</para>
    /// </summary>
    public class TblHssPassportRepository : BaseRepository<TblHssPassport>
    {

        /// <summary>
        /// 根据用户账号查询
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <param name="userCode">用户账号，一般为手机号</param>
        /// <returns>账户实体信息</returns>
        public TblHssPassport GetByUserCode(string userCode)
        {
            return this.LoadQueryable()
                        .Where(t => t.UserCode == userCode)
                        .FirstOrDefault();
        }

        /// <summary>
        /// 根据用户账号查询
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="userCodes">一组用户账号，一般为手机号</param>
        /// <returns>账户实体信息集合</returns>
        public List<TblHssPassport> GetByUserCodes(List<string> userCodes)
        {
            return this.LoadQueryable()
                        .Where(t => userCodes.Contains(t.UserCode))
                        .ToList();
        }



        /// <summary>
        /// 根据OPENID获取账户
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-08</para>
        /// </summary>
        /// <param name="openId">微信用户在公众号的唯一标识</param>
        /// <returns>账户实体信息</returns>
        public TblHssPassport GetByOpenId(string openId)
        {
            return this.LoadQueryable()
                        .Where(t => t.OpenId == openId)
                        .FirstOrDefault();
        }


        /// <summary>
        /// 检查账户是否存在微信号绑定
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="userCodes">一组账号,账户就是手机号码</param>
        /// <returns></returns>
        public bool IsMobileBind(List<string> userCodes)
        {
            return this.LoadQueryable()
                        .Any(t => userCodes.Contains(t.UserCode) && !string.IsNullOrEmpty(t.OpenId));
        }

        /// <summary>
        /// 根据用户账号删除
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-19</para>
        /// </summary>
        /// <param name="userCode">用户账号，一般为手机号</param>
        public void DeleteByUserCode(string userCode)
        {
            this.Delete(t => t.UserCode == userCode);
        }
    }
}
