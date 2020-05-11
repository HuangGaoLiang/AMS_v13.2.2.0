using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AMS.Anticorrosion.AC
{
    /// <summary>
    /// 描    述: 档案库
    /// <para>作    者: Huang GaoLiang</para>
    /// <para>创建时间: 2019-02-19</para>
    /// </summary>
    public class ACService : AntService
    {
        /// <summary>
        /// 更新学生信息到档案库
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>2018-12-12 </para>
        /// </summary>
        /// <param name="student">学生信息</param>
        public void StudentInfoToArtLibrary(StudentRequest student)
        {
            try
            {
                Task.Run(() =>
                  {
                      base.AntWriteLog("", $"读取配置文件地址测试{ApiUrlConfig.ACAddURL}", "", "", null);

                      string postData = JsonConvert.SerializeObject(student, new JsonSerializerSettings { DateFormatString = "yyyy/MM/dd HH:mm:ss" });
                      string result = HttpRun.Post(ApiUrlConfig.ACAddURL, postData);

                      ResDate res = JsonConvert.DeserializeObject<ResDate>(result);

                      if (res != null && res.Data == "1")
                      {
                          base.AntWriteLog("", $"修改学生信息到档案库:1个,保存:{res.Data}个", "", "", null);
                      }
                      else
                      {
                          base.AntWriteLog($"{ApiUrlConfig.ACAddURL}", "档案库推送学生失败", "", "", null);
                      }
                  });
            }
            catch (Exception e)
            {
                base.AntWriteLog($"{ApiUrlConfig.ACAddURL}", "档案库推送学生失败", "", e.Message, e);
            }

        }
    }
}
