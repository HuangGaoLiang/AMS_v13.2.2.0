using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.SDK
{
    /// <summary>
    /// 所有服务管理
    /// </summary>
    public class ServicesManage
    {


        private ServicesManage()
        {

        }

        /// <summary>
        /// 获取课程服务
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public static CourseService GetCourseService(string companyId)
        {
            return new CourseService(companyId);
        }


    }
}
