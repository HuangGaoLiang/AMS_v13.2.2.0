using System;
using System.Collections.Generic;
using System.Text;
using AMS.Core;
using AMS.Dto;

namespace AMS.Service
{
    /// <summary>
    /// 报名排课课次数据提供者
    /// </summary>
    public abstract class MakeLessonProvider : BService, ILessonProvider
    {
        protected readonly long _makeLessonId;

        protected MakeLessonProvider(long makeLessonId)
        {
            this._makeLessonId = makeLessonId;
        }

        public int BusinessType => (int)LessonBusinessType.EnrollMakeLesson;
    }
}
