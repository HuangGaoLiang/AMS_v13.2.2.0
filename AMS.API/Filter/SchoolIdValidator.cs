using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace AMS.API.Filter
{
    /// <summary>
    /// 校区Id校验
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SchoolIdValidator : Attribute, IActionFilter
    {
        /// <summary>
        /// 方法执行后
        /// </summary>
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        /// <summary>
        /// 在控制器方法执行之前验证校区Id
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-22</para>
        /// </summary>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //校验是否选择了校区
            bool schoolIdIsNull = context.HttpContext.Request.Headers.TryGetValue("schoolNo", out StringValues schoolNo);
            if (!schoolIdIsNull)
            {
                throw new ApplicationException("请选择校区");
            }
        }
    }
}
