using System.Collections.Generic;
using System.Linq;
using AMS.Core;
using AMS.Core.Constants;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;

namespace AMS.Service
{
    /// <summary>
    /// 描述：老师代课课次结束者
    /// <para>作者：瞿琦</para>
    /// <para>创建时间：2019-3-8</para>
    /// </summary>
    public class AdjustLessonTeacherFinisher : AdjustLessonTeacherProvider, ILessonFinisher
    {
        private readonly List<TblTimAdjustLesson> _adjustLessonList;   //课次调整信息表信息
        private readonly List<TblTimLesson> _lessonList;     //要销毁的课次信息


        /// <summary>
        /// 描述：老师代课课次结束者
        /// <para>作者：瞿琦</para>
        /// <para>创建时间:2019-3-8</para>
        /// </summary>
        /// <param name="adjustLessonList">课次调整信息表信息</param>
        /// <param name="lessonList">要销毁的课次列表</param>
        public AdjustLessonTeacherFinisher(List<TblTimAdjustLesson> adjustLessonList, List<TblTimLesson> lessonList)
        {
            _adjustLessonList = adjustLessonList;
            _lessonList = lessonList;
        }

        /// <summary>
        /// 业务类型
        /// </summary>
        public override int BusinessType { get { return (int)ProcessBusinessType.F_AdjustLessonTeacher; } set { } } 

        /// <summary>
        /// 暂不实现
        /// </summary>
        public void AfterLessonFinish()
        {
            
        }

        /// <summary>
        /// 描述：获取更新代课老师要销毁的课次
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-8</para>
        /// </summary>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：44，异常描述：找不到课次调整业务Id
        /// </exception>
        /// <returns>要销毁的课次列表</returns>
        public List<LessonFinisherInfo> GetLessonFinisherInfo()
        {
            var result = new List<LessonFinisherInfo>();
            foreach (var item in _lessonList)
            {
                var adjustLessonEntity= _adjustLessonList.FirstOrDefault(k => k.FromLessonId == item.LessonId);
                if (adjustLessonEntity==null)  //如果课次调整信息为空，则提示找不到课次调整信息
                {
                    throw new BussinessException((byte)ModelType.Timetable,44);
                }
                //待添加异常处理
                var lissonEntity = new LessonFinisherInfo
                {
                    BusinessId = adjustLessonEntity.AdjustLessonId,
                    BusinessType = this.BusinessType,
                    LessonId = item.LessonId,
                    Remark = LessonProcessConstants.LessonTeacherRemark
                };
                result.Add(lissonEntity);
            }
            
            return result;
        }
    }
}
