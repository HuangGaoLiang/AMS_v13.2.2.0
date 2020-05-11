/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AMS.Storage.Context;
using AMS.Storage.Models;
using Jerrisoft.Platform.Public.PageExtensions;
using AMS.Dto;
using Jerrisoft.Platform.Storage;
using System;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描    述：写生课仓储
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public class TblTimLifeClassRepository : BaseRepository<TblTimLifeClass>
    {

        /// <summary>
        /// 写生课仓储实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <param name="context"></param>
        public TblTimLifeClassRepository(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// 写生课仓储实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        public TblTimLifeClassRepository()
        {
        }


        /// <summary>
        /// 根据校区Id和请求参数分页获取写生课排课分页列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-07</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="request">写生列表查询条件</param>
        /// <returns>写生课排课分页列表</returns>
        public PageResult<TblTimLifeClass> GetLifeClassListAsync(string schoolId, LifeClassListSearchRequest request)
        {
            var query = LoadQueryable().Where(a => a.SchoolId == schoolId && a.TermId == request.TermId
                    && (string.IsNullOrEmpty(request.Title) || a.Title.Contains(request.Title)))
                .OrderBy(a => a.ClassBeginTime);
            return query.ToPagerSource(request.PageIndex, request.PageSize);
        }

        /// <summary>
        /// 根据写生课Id，获取写生课信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-10</para>
        /// </summary>
        /// <param name="lifeClassId">写生课Id</param>
        /// <returns>写生课信息</returns>
        public TblTimLifeClass GetLifeClassInfo(long lifeClassId)
        {
            return Load(x => x.LifeClassId == lifeClassId);
        }

        /// <summary>
        /// 根据写生课Id列表，获取写生课列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-10</para>
        /// </summary>
        /// <param name="lifeClassIdList">写生课Id列表</param>
        /// <returns>写生课信息列表</returns>
        public async Task<List<TblTimLifeClass>> GetLifeClassListAsync(List<long> lifeClassIdList)
        {
            return await LoadLisTask(x => lifeClassIdList.Contains(x.LifeClassId));
        }

        /// <summary>
        /// 根据写生课Id列表，获取写生课列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-10</para>
        /// </summary>
        /// <param name="lifeClassIdList">写生课Id列表</param>
        /// <returns>写生课信息列表</returns>
        public List<TblTimLifeClass> GetLifeClassList(IEnumerable<long> lifeClassIdList)
        {
            return base.LoadList(x => lifeClassIdList.Contains(x.LifeClassId));
        }

        /// <summary>
        /// 根据学期Id列表，获取写生课列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-20</para>
        /// </summary>
        /// <param name="termIdList">学期Id集合</param>
        /// <returns>写生课信息列表</returns>
        public async Task<List<TblTimLifeClass>> GetLifeClassListByTermId(List<long> termIdList)
        {
            return await LoadLisTask(x => termIdList.Contains(x.TermId));
        }

        /// <summary>
        /// 生成写生课代码（上课日期+序号）
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-10</para>
        /// </summary>
        /// <param name="classBeginDate">上课开始日期</param>
        /// <returns>写生课代码</returns>
        public string GenerateLifeClassCode(DateTime classBeginDate)
        {
            var lifeClassCode = classBeginDate.ToString("yyyyMMdd") + "01";
            var seqList = LoadList(x => x.ClassBeginTime.Value.Date == classBeginDate.Date).Select(a => int.Parse(a.LifeClassCode.Substring(8))).ToList();
            if (seqList != null && seqList.Count > 0)
            {
                int seq = seqList.Max() + 1;
                lifeClassCode = classBeginDate.ToString("yyyyMMdd") + seq.ToString().PadLeft(2, '0');
            }
            return lifeClassCode;
        }

        /// <summary>
        /// 删除写生课
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="lifeClassId">写生课Id</param>
        public void DeleteLifeClassById(long lifeClassId)
        {
            Delete(x => x.LifeClassId == lifeClassId);
        }
    }
}
