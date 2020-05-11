using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using AMS.Dto;
using AMS.Models;
using Jerrisoft.Platform.Public.PageExtensions;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository.Cst
{
    /// <summary>
    /// 描    述: ViewStudent仓储
    /// <para>作    者: Huang GaoLiang</para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class ViewStudentRepository : BaseRepository<ViewStudent>
    {
        /// <summary>
        /// 带上下文的构造函数
        /// </summary>
        /// <param name="context">数据上下文</param>
        public ViewStudentRepository(DbContext context) : base(context)
        {

        }
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ViewStudentRepository()
        {

        }

        /// <summary>
        /// 获取前50条学生信息
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-10-29</para>
        /// </summary>
        /// <param name="keyWord">关键字</param>
        /// <param name="schoolId">校区编号</param>
        /// <returns>返回前50条学生信息</returns>
        public List<ViewStudent> GetViewStudentList(string keyWord, string schoolId)
        {
            var querySql = @"SELECT s.StudentId,
                                   s.StudentNo,
                                   s.StudentName,
                                   s.HeadFaceUrl,
                                   s.Sex,
                                   s.Birthday,
                                   s.LinkMobile,
                                   s.ContactPersonMobile,
                                   c.SchoolId,
                                   c.StudyStatus,
                                   c.RemindClassTimes,
                                   s.CreateTime
                            FROM dbo.TblCstStudent s
                                INNER JOIN dbo.TblCstSchoolStudent c
                                    ON c.StudentId = s.StudentId
                            WHERE c.SchoolId =@SchoolId ";

            var student = CurrentContext.ViewStudent.FromSql(querySql, new object[]
            {
                new SqlParameter("@SchoolId", schoolId)
            });
            var query = student.WhereIf(!string.IsNullOrWhiteSpace(keyWord), x => x.StudentName.Contains(keyWord) || x.ContactPersonMobile.StartsWith(keyWord)).Take(50).ToList();
            return query;
        }

        /// <summary>
        /// 学生管理列表
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-10-29</para>
        /// </summary>
        /// <param name="req">查询条件</param>
        /// <param name="schoolId">校区编号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>返回学生分页数据</returns>
        public PageResult<ViewStudent> GetViewStudentList(StudentListSearchRequest req, string schoolId, int pageIndex, int pageSize)
        {
            var querySql = $@"SELECT s.StudentId,
                                   s.StudentNo,
                                   s.StudentName,
                                   s.Sex,
                                   s.Birthday,
                                   s.LinkMobile,
                                   s.HeadFaceUrl,
                                   s.ContactPersonMobile,
                                   c.SchoolId,
                                   c.StudyStatus,
                                   c.RemindClassTimes,
                                   s.CreateTime
                            FROM dbo.TblCstStudent s
                                INNER JOIN dbo.TblCstSchoolStudent c
                                    ON c.StudentId = s.StudentId
                            WHERE c.SchoolId =@SchoolId";
            var student = base.CurrentContext.ViewStudent.FromSql(querySql, new object[]
             {
                new SqlParameter("@SchoolId",schoolId)

             });
            var query = student.WhereIf(req.StudySatus > 0, x => x.StudyStatus == req.StudySatus)
                                .WhereIf(req.LessonCount > 0, x => x.RemindClassTimes < req.LessonCount)
                                .WhereIf(!string.IsNullOrWhiteSpace(req.KeyWord), x => x.StudentName.Contains(req.KeyWord) || x.ContactPersonMobile.StartsWith(req.KeyWord))//关键字（学生姓名/手机号）
                                .OrderBy(m => m.RemindClassTimes)
                                .ThenBy(m => m.StudentNo)
                                .ToPagerSource(pageIndex, pageSize);
            return query;
        }

        /// <summary>
        /// 获取本公司前50条学生信息
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-10-29</para>
        /// </summary>
        /// <param name="keyWord">关键字</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>返回本公司前50条数据</returns>
        public List<ViewStudent> GetViewStudentListByKeyWord(string keyWord, string companyId)
        {
            var querySql = @"SELECT s.StudentId,
                                   s.StudentNo,
                                   s.StudentName,
                                   s.Sex,
                                   s.Birthday,
                                   s.LinkMobile,
                                   s.ContactPersonMobile,
                                   s.CreateTime,
                                   s.HeadFaceUrl,
                                   0 AS RemindClassTimes,
                                   '' AS SchoolId,
                                   0 AS StudyStatus
                            FROM dbo.TblCstStudent s
                            WHERE EXISTS
                            (
                                SELECT * FROM dbo.TblCstSchoolStudent c WHERE c.CompanyId = @companyId
                            )
                                  OR NOT EXISTS
                            (
                                SELECT * FROM dbo.TblCstSchoolStudent c WHERE c.StudentId = s.StudentId
                            )";

            var student = CurrentContext.ViewStudent.FromSql(querySql, new object[]
            {
                new SqlParameter("@companyId", companyId)
            });
            var query = student.WhereIf(!string.IsNullOrWhiteSpace(keyWord), x => x.StudentName.Contains(keyWord) || x.ContactPersonMobile.StartsWith(keyWord)).Take(50).ToList();
            return query;
        }

    }
}
