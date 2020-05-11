using AMS.Core;
using Jerrisoft.Platform.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Anticorrosion
{
    public class AntService : BService
    {
        /// <summary>
        /// 拼接访问参数
        /// </summary>
        /// <returns></returns>
        protected string JoinParameters(Dictionary<string, string> param)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in param)
            {
                sb.AppendFormat("{0}={1}&", pair.Key, pair.Value);
            }
            return sb.ToString().Trim('&');
        }

        /// <summary>
        /// 防腐层日志记录
        /// </summary>
        /// <param name="action">方法名称</param>
        /// <param name="actionDesc">方法描述</param>
        /// <param name="requestUrl">请求接口地址</param>
        /// <param name="msg">异常信息</param>
        /// <param name="e">Exception</param>
        protected virtual void AntWriteLog(string action, string actionDesc, string requestUrl, string msg, Exception e)
        {
            StringBuilder error = new StringBuilder();
            error.Append($"请求user:{base.CurrentUserId}");
            error.Append($"方法:{action}");
            error.Append($"描述:{actionDesc}");
            error.Append($"请求地址:{requestUrl}");
            error.Append($"异常:{msg}");
            error.Append($"{e}");

            LogWriter.Write(this, error.ToString(), LoggerType.Debug);
        }
    }
}
