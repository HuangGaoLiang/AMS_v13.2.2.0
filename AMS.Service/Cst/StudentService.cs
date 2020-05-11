using AMS.Anticorrosion.AC;
using AMS.Anticorrosion.HRS;
using AMS.Core;
using AMS.Dto;
using AMS.Dto.Enum.Custom;
using AMS.Models;
using AMS.Service.Hss;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using AMS.Storage.Repository.Cst;
using AutoMapper;
using JDW.SDK;
using Jerrisoft.Platform.Log;
using Jerrisoft.Platform.Public.PageExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AMS.Service
{
    /// <summary>
    /// 描    述: 学生信息服务
    /// <para>作    者: Huan GaoLiang</para>
    /// <para>创建时间: 2019-02-14</para>
    /// </summary>
    public class StudentService
    {
        #region 仓储/服务/属性/构造函数

        /// <summary>
        /// 学生信息仓储
        /// </summary>
        private readonly Lazy<TblCstStudentRepository> _studentRepository = new Lazy<TblCstStudentRepository>();

        /// <summary>
        /// 学生校区信息仓储
        /// </summary>
        private readonly Lazy<TblCstSchoolStudentRepository> _schoolStudentRepository = new Lazy<TblCstSchoolStudentRepository>();

        /// <summary>
        /// 学生联系人信息仓储
        /// </summary>
        private readonly Lazy<TblCstStudentContactRepository> _studentContactRepository = new Lazy<TblCstStudentContactRepository>();

        /// <summary>
        /// 校区编号
        /// </summary>
        private readonly string _schoolId;

        /// <summary>
        /// 学生编号前缀
        /// </summary>
        private const string _studentNoTag = "S";

        /// <summary>
        /// 学生信息服务构造函数
        /// <para>作    者: Huan GaoLiang </para>
        /// <para>创建时间: 2019-02-14 </para>
        /// </summary>
        /// <param name="schoolId">学生编号</param>
        public StudentService(string schoolId)
        {
            _schoolId = schoolId;
        }
        #endregion

        #region CreateStudentNo 产生学生号
        /// <summary>
        /// 产生学生号
        /// <para>作    者: Huan GaoLiang </para>
        /// <para>创建时间: 2019-02-14 </para>
        /// </summary>
        /// <param name="studentId">学生编号</param>
        /// <returns>返回学生编号</returns>
        private string CreateStudentNo(long studentId)
        {
            return $"{_studentNoTag}{studentId}";
        }
        #endregion

        #region GetSchoolStudentList 根据学生姓名或手机号查询学生信息
        /// <summary>
        /// 根据学生姓名或手机号查询学生信息
        /// <para>作     者:Huang GaoLiang  </para>
        /// <para>创建时间: 2019-10-29 </para>
        /// </summary>
        /// <param name="keyWord">学生名称或手机号</param>
        /// <returns>返回学生信息</returns>
        public List<StudentSearchResponse> GetSchoolStudentList(string keyWord)
        {
            // 1、根据学生名称模糊或者电话号码查询学生名称信息
            List<ViewStudent> resultList = new ViewStudentRepository().GetViewStudentList(keyWord, this._schoolId);

            // 2、所有的校区信息
            List<SchoolResponse> schoolList = new OrgService().GetAllSchoolList();

            // 3、组合数据
            List<StudentSearchResponse> list = (from stu in resultList

                                                join s in schoolList on stu.SchoolId equals s.SchoolId

                                                select new StudentSearchResponse
                                                {
                                                    StudentId = stu.StudentId,
                                                    StudentName = stu.StudentName,
                                                    HeadFaceUrl = stu.HeadFaceUrl,
                                                    Sex = stu.Sex,
                                                    SexName = EnumName.GetDescription(typeof(SexEnum), stu.Sex),
                                                    Age = Age.GetAge(stu.Birthday),
                                                    Birthday = stu.Birthday,
                                                    LinkMobile = stu.LinkMobile,
                                                    ContactPersonMobile = string.IsNullOrEmpty(stu.ContactPersonMobile) ? "" : Regex.Replace(stu.ContactPersonMobile.Split(',')[0], "(\\d{3})\\d{4}(\\d{4})", "$1****$2"), //只截取第一个监护人手机号
                                                    SchoolId = s.SchoolId,
                                                    SchoolName = s.SchoolName,
                                                    StudentNo = stu.StudentNo
                                                }).ToList();
            return list;
        }
        #endregion

        #region GetStudent 根据学生编号获取学生详情信息
        /// <summary>
        /// 根据学生编号获取学生详情信息
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2018-11-06 </para>
        /// </summary>
        /// <param name="studentId">学生编号</param>
        /// <returns>返回学生详细信息</returns>
        public StudentDetailResponse GetStudent(long studentId)
        {
            // 根据学生编号获取学信息
            TblCstStudent student = _studentRepository.Value.GetCstStudentId(studentId);

            if (student == null)
            {
                return null;
            }
            StudentDetailResponse res = new StudentDetailResponse
            {
                StudentId = student.StudentId,
                StudentNo = student.StudentNo,
                StudentName = student.StudentName,
                Birthday = student.Birthday,
                Age = Age.GetAge(student.Birthday),
                Sex = student.Sex,
                SexName = EnumName.GetDescription(typeof(SexEnum), student.Sex),
                Balance = new WalletService(_schoolId, studentId).Balance,
                AreaId = student.AreaId,
                ContactPerson = JsonConvert.DeserializeObject<List<GuardianRequest>>(student.ContactPerson),
                ContactPersonMobile = student.ContactPersonMobile,
                CurrentSchool = student.CurrentSchool,
                CustomerFromNumber = student.CustomerFrom,
                CustomerFrom = EnumName.GetDescription(typeof(ListSource), student.CustomerFrom),
                HeadFaceUrl = student.HeadFaceUrl,
                HomeAddress = student.HomeAddress,
                HomeAddressFormat = JsonConvert.DeserializeObject<HomeAddressFormatRequest>(student.HomeAddressFormat),
                IDNumber = student.IDNumber,
                IDType = student.IDType,
                IdTypeName = EnumName.GetDescription(typeof(IDType), student.IDType),
                LinkMail = student.LinkMail,
                LinkMobile = student.LinkMobile,
                ParentId = student.ParentId,
                Remark = student.Remark
            };
            return res;
        }

        #endregion

        #region  GetListBykeyWord 根据学生姓名查询学生信息

        /// <summary>
        /// 根据学生姓名查询学生信息
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2018-10-29 </para>
        /// </summary>
        /// <param name="keyWord">学生名称或手机号</param>
        /// <param name="companyId">当前登录的用户</param>
        /// <returns>返回学生集合</returns>
        public static List<StudentSearchResponse> GetListBykeyWord(string keyWord, string companyId = null)
        {
            // 1、根据学生名称模糊或者电话号码查询学生名称信息
            List<ViewStudent> sList = new ViewStudentRepository().GetViewStudentListByKeyWord(keyWord, companyId);

            List<long> ids = sList.Select(m => m.StudentId).ToList();
            List<TblCstSchoolStudent> cList = new TblCstSchoolStudentRepository().GetSchoolIdByStudentIds(ids);

            // 2、所有的校区信息
            List<SchoolResponse> schoolList = new OrgService().GetAllSchoolList();

            // 3、组合数据
            List<StudentSearchResponse> list = (from stu in sList
                                                select new StudentSearchResponse
                                                {
                                                    StudentId = stu.StudentId,
                                                    StudentName = stu.StudentName,
                                                    Sex = stu.Sex,
                                                    SexName = EnumName.GetDescription(typeof(SexEnum), stu.Sex),
                                                    Age = Age.GetAge(stu.Birthday),
                                                    Birthday = stu.Birthday,
                                                    LinkMobile = stu.LinkMobile,
                                                    ContactPersonMobile = string.IsNullOrEmpty(stu.ContactPersonMobile) ? "" : Regex.Replace(stu.ContactPersonMobile, "(\\d{3})\\d{4}(\\d{4})", "$1****$2"), //只截取第一个监护人手机号
                                                    Company = GetCompanyName(stu.StudentId, schoolList, cList),
                                                    SchoolName = GetSchoolNameByStuId(stu.StudentId, schoolList, cList)
                                                }).ToList();
            return list.ToList();
        }

        /// <summary>
        /// 获取校区所在的公司名称
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2019-01-02 </para>
        /// </summary>
        /// <param name="studentId">学生编号</param>
        /// <param name="schoolList">校区集合</param>
        /// <param name="schoolStudentList">校区学生集合</param>
        /// <returns>返回公司名称</returns>
        private static string GetCompanyName(long studentId, List<SchoolResponse> schoolList, List<TblCstSchoolStudent> schoolStudentList)
        {
            var list = (from c in schoolStudentList
                        join s in schoolList on c.SchoolId equals s.SchoolId into sc
                        from temp in sc.DefaultIfEmpty()
                        where c.StudentId == studentId
                        select temp?.Company
                ).ToList();
            return string.Join("、", list);
        }

        /// <summary>
        /// 根据学生编号，查询学生校区信息
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2019-10-29 </para>
        /// </summary>
        /// <param name="studentId">学生编号</param>
        /// <param name="schoolList">校区集合</param>
        /// <param name="schoolStudentList">校区学生集合</param>
        /// <returns>返回学生校区</returns>
        private static string GetSchoolNameByStuId(long studentId, List<SchoolResponse> schoolList, List<TblCstSchoolStudent> schoolStudentList)
        {
            var list = (from c in schoolStudentList
                        join s in schoolList on c.SchoolId equals s.SchoolId into sc
                        from temp in sc.DefaultIfEmpty()
                        where c.StudentId == studentId
                        select temp == null ? "" : temp.SchoolName
                ).ToList();
            return string.Join("、", list);
        }
        #endregion

        #region GetStudentsByKey  根据关键词(名字、手机号=LinkMobile)获取 
        /// <summary>
        /// 根据关键词(名字、手机号=LinkMobile)获取
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-27</para>
        /// </summary>
        /// <param name="key">关键词</param>
        /// <returns>学生信息列表</returns>
        internal static List<TblCstStudent> GetStudentsByKey(string key)
        {
            return new TblCstStudentRepository().GetStudentsByKey(key);
        }
        #endregion


        #region GetStudentPageList 获取学生管理列表

        /// <summary>
        /// 根据查询条件获取学生管理列表
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2019-11-06 </para>
        /// </summary>
        /// <param name="req">查询学生查询条件</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>返回学生分页数据</returns>
        public PageResult<StudentListResponse> GetStudentPageList(StudentListSearchRequest req, int pageIndex, int pageSize)
        {
            PageResult<ViewStudent> studentList = new ViewStudentRepository().GetViewStudentList(req, _schoolId, pageIndex, pageSize);
            PageResult<StudentListResponse> list = Mapper.Map<PageResult<ViewStudent>, PageResult<StudentListResponse>>(studentList);

            foreach (var item in list.Data)
            {
                item.ContactPersonMobile = string.IsNullOrEmpty(item.ContactPersonMobile) ? "" : Regex.Replace(item.ContactPersonMobile, "(\\d{3})\\d{4}(\\d{4})", "$1****$2");
                item.Age = Age.GetAgeByBirthday(item.Birthday);
            }
            return list;
        }

        #endregion

        #region GetStudentInfo 根据学生编号获取学生信息
        /// <summary>
        /// 根据学生编号获取学生信息
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2019-01-10 </para>
        /// </summary>
        /// <param name="studentId">学生编号</param>
        /// <returns>返回学生信息</returns>
        internal static TblCstStudent GetStudentInfo(long studentId)
        {
            return new TblCstStudentRepository().GetCstStudentId(studentId);
        }
        #endregion

        #region GetStudentByIds 根据学生编号获取学生信息
        /// <summary>
        /// 根据学生编号获取学生信息
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-11-21 </para>
        /// </summary>
        /// <param name="ids">学生编号集合</param>
        /// <returns>返回学生信息集合</returns>
        internal static async Task<List<TblCstStudent>> GetStudentByIds(IEnumerable<long> ids)
        {
            return await new TblCstStudentRepository().GetStudentsByIdTask(ids);
        }
        #endregion

        #region GetStudentCard 获取学生卡打印信息
        /// <summary>
        /// 获取学生卡打印信息
        /// <para>作    者: Huan GaoLiang </para>
        /// <para>创建时间: 2019-03-06 </para>
        /// </summary>
        /// <param name="studentId">学生编号</param>
        /// <param name="schoolId">校区编号</param>
        /// <returns>返回学生证信息</returns>
        /// <returns>修改结果数</returns>
        /// <exception>
        /// 异常ID：1,系统不存在该学生
        /// </exception>
        public StudentCardResponse GetStudentCard(long studentId, string schoolId)
        {
            StudentCardResponse studentCar = new StudentCardResponse();

            // 1、根据学生编号查询学生信息
            TblCstStudent student = _studentRepository.Value.GetCstStudentId(studentId);

            if (student == null)
            {
                throw new BussinessException((byte)ModelType.Customer, 1);
            }

            studentCar.StudentId = student.StudentId;
            studentCar.HeadFaceUrl = student.HeadFaceUrl;
            studentCar.StudentNo = student.StudentNo;
            studentCar.StudentName = student.StudentName;

            // 2、获取基础配置中的公司log和学生证反面信息 
            var companyId = SdkClient.CreateDimCommonService().GetAllSchool().FirstOrDefault(m => m.SchoolId == schoolId)?.CompanyId;
            var schoolSetting = new HomeSchoolSettingService().GetSettingList(companyId);

            studentCar.CompanyImage = schoolSetting.FirstOrDefault(m => m.FuntionId == (int)HomeSchoolBusinessType.StudentCradUp)?.DataValue;
            studentCar.NegativeImage = schoolSetting.FirstOrDefault(m => m.FuntionId == (int)HomeSchoolBusinessType.StudentCradDown)?.DataValue;

            return studentCar;


        }

        #endregion

        #region ExistStudent 检查学生是否存在
        /// <summary>
        /// 检查学生是否存在
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-10-29 </para>
        /// </summary>
        /// <param name="studentId">学生编号</param>
        /// <param name="studentName">学生姓名</param>
        /// <param name="linkMobile">手机号</param>
        /// <returns>返回学生编号</returns>
        public static long ExistStudent(long studentId, string studentName, string linkMobile)
        {
            return new TblCstStudentRepository().GetCstStudentId(studentId, studentName, linkMobile.Trim()) == null ? 0 : new TblCstStudentRepository().GetCstStudentId(studentId, studentName, linkMobile.Trim()).StudentId;
        }
        #endregion

        #region CountSchoolStudent 统计校区学生数据
        /// <summary>
        /// 统计校区学生数据
        /// <para>作    者: Huan GaoLiang </para>
        /// <para>创建时间: 2019-03-06 </para>
        /// </summary>
        /// <returns>返回本校的学生总数量</returns>
        public long CountSchoolStudent()
        {
            return _schoolStudentRepository.Value.GetSchoolStudent(_schoolId);
        }
        #endregion

        #region Modify 修改学生信息
        /// <summary>
        /// 修改学生信息
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2019-03-14 </para>
        /// </summary>
        /// <param name="studentId">学生编号</param>
        /// <param name="request">修改输入参数</param>
        /// <returns>返回修改结果受影响的行数</returns>
        /// <exception>
        /// 异常ID：1,系统不存在该学生
        /// </exception>
        public static void Modify(long studentId, StudentRegisterRequest request)
        {
            TblCstStudentContactRepository contactRepository = new TblCstStudentContactRepository();
            TblCstStudentRepository studentRepository = new TblCstStudentRepository();
            // 1、根据编号查询学生信息
            TblCstStudent rawStudent = studentRepository.GetCstStudentId(studentId);     //原学生信息
            if (rawStudent == null)
            {
                throw new BussinessException((byte)ModelType.Customer, 1);
            }
            List<TblCstStudentContact> rawContacts = contactRepository.GetByStudentId(studentId);              //原学生联系人
            TblCstStudent newStudent = TransExpV2<TblCstStudent, TblCstStudent>.Trans(rawStudent);              //新学生


            // 2、验证
            Verification(studentId, request.StudentName, request.LinkMobile);

            // 3、学生信息和学生联系人信息
            List<TblCstStudentContact> newContacts = GetStudentContact(newStudent.StudentId, JsonConvert.SerializeObject(request.ContactPerson));
            SetStudentInfo(request, newStudent);


            //4、数据存储操作
            studentRepository.Update(newStudent);                                          //更新学生信息
            contactRepository.DeleteByStudentId(new List<long> { newStudent.StudentId });  //删除原来的学生联系人
            contactRepository.Add(newContacts);                                            //添加学生联系人


            // 5、推送学生至档案库
            StudentRequest s = GetStudentToAC(rawStudent);
            new ACService().StudentInfoToArtLibrary(s);


            // 6、记录操作日志
            AddOperationLog(rawStudent, newStudent, request);

            // 7、将手机号信息推送至家校互联账户
            StudentPublish(rawStudent, newStudent, rawContacts, newContacts);

        }

        /// <summary>
        /// 组合学生联系人电话
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2019-03-</para>
        /// </summary>
        /// <param name="linkMobile">电话号码</param>
        /// <param name="contactPersonMobile">监护人电话号码</param>
        /// <returns>返回电话号码集合</returns>
        private static List<string> GetMobileList(string linkMobile, string contactPersonMobile)
        {
            var contactMobile = contactPersonMobile.Split(',').Where(m => !string.IsNullOrWhiteSpace(m)).Distinct().ToList();
            List<string> mobileList = contactMobile;
            mobileList.Add(linkMobile);
            return mobileList.Distinct().ToList();
        }

        #region SetStudentInfo 设置需要修改的学生信息
        /// <summary>
        /// 设置需要修改的学生信息
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2019-03-15 </para>
        /// </summary>
        /// <param name="request">学生信息修改请求参数</param>
        /// <param name="student">原学生信息</param>
        private static void SetStudentInfo(StudentRegisterRequest request, TblCstStudent student)
        {
            student.StudentName = request.StudentName.Trim();
            student.Sex = (byte)request.Sex;
            student.Birthday = request.Birthday;
            student.CurrentSchool = request.CurrentSchool;
            student.AreaId = request.AreaId;
            student.HomeAddress = request.HomeAddress;
            student.HomeAddressFormat = JsonConvert.SerializeObject(request.HomeAddressFormat);
            student.IDType = (int)request.IDType;
            student.IDNumber = request.IDNumber;
            student.LinkMobile = request.LinkMobile;
            student.LinkMail = request.LinkMail;
            student.ContactPerson = JsonConvert.SerializeObject(request.ContactPerson);
            student.ContactPersonMobile = ContactPersonMobile(request.ContactPerson) ?? "";
            student.Remark = request.Remark;
        }
        #endregion

        #region AddOperationLog 添加操作日记
        /// <summary>
        /// 添加操作日记 
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2019-03-15 </para>
        /// </summary>
        /// <param name="rawStudent">原学生</param>
        /// <param name="newStudent">新学生</param>
        /// <param name="request">原始提交学生信息</param>
        private static void AddOperationLog(TblCstStudent rawStudent, TblCstStudent newStudent, StudentRegisterRequest request)
        {
            string remark = $"将ID是{ rawStudent.StudentId}学生的姓名{ rawStudent.StudentName}改为 { request.StudentName}; 手机号{ rawStudent.LinkMobile}改为{ newStudent.LinkMobile}";
            TblDatOperationLog log = new TblDatOperationLog()
            {
                OperationLogId = IdGenerator.NextId(),
                SchoolId = request.SchoolId,
                BusinessType = (int)LogBusinessType.Student,
                BusinessId = rawStudent.StudentId,
                FlowStatus = (int)OperationFlowStatus.Submit,
                OperatorId = request.UserId,
                OperatorName = request.UserName,
                Remark = remark,
                CreateTime = DateTime.Now,
            };
            new OperationLogService().Add(log);
        }
        #endregion


        /// <summary>
        /// 修改学生手机号或联系人手机号,将手机号信息推送至家校互联账户
        /// <para>作     者:蔡亚康 </para>
        /// <para>创建时间: 2019-03-21 </para>
        /// </summary>
        /// <param name="rawStudent">原学生信息</param>
        /// <param name="newStudent">新学生信息</param>
        /// <param name="rawContacts">原学生联系人信息</param>
        /// <param name="newContacts">新学生联系人信息</param>
        private static void StudentPublish(TblCstStudent rawStudent, TblCstStudent newStudent, List<TblCstStudentContact> rawContacts, List<TblCstStudentContact> newContacts)
        {
            StudentPassportChangeInDto result = new StudentPassportChangeInDto();

            List<string> rawMobiles = new List<string>();   //原手机号
            List<string> newMobiles = new List<string>();   //新手机号


            //原手机号集合
            rawMobiles.Add(rawStudent.LinkMobile);
            rawMobiles.AddRange(rawContacts.Select(t => t.Mobile));
            rawMobiles = rawMobiles.Where(t => !string.IsNullOrEmpty(t))
                                    .Distinct()
                                    .ToList();


            //新手机号集合
            newMobiles.Add(newStudent.LinkMobile);
            newMobiles.AddRange(newContacts.Select(t => t.Mobile));
            newMobiles = newMobiles.Where(t => !string.IsNullOrEmpty(t))
                                    .Distinct()
                                    .ToList();


            result.MobileAddList = newMobiles.Except(rawMobiles).ToList();
            result.MobileDeleteList = rawMobiles.Except(newMobiles).ToList();


            //获取手机号在在其他学生的账户列表
            TblCstStudentRepository studentRepository = new TblCstStudentRepository();
            TblCstStudentContactRepository contactRepository = new TblCstStudentContactRepository();

            var studentList = studentRepository.SearchByMobiles(result.MobileDeleteList);
            var contactList = contactRepository.SearchByMobiles(result.MobileDeleteList);


            //被删除的号码，在studentList,contactList 还存在，说明还不允许删除
            List<string> existMobiles = new List<string>();
            existMobiles.AddRange(studentList.Select(t => t.LinkMobile));
            existMobiles.AddRange(contactList.Select(t => t.Mobile));
            existMobiles = existMobiles.Distinct().ToList();


            existMobiles.ForEach(item =>
            {
                result.MobileDeleteList.Remove(item);
            });

            // 推送至消息队列
            new StudentFamilyProducerService().Publish(result);
        }
        #endregion

        #region Register 学生注册
        /// <summary>
        /// 学生注册
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2018-10-29 </para>
        /// </summary>
        /// <param name="request">学生注册提交数据</param>
        public string Register(StudentRegisterRequest request)
        {
            long studentId = IdGenerator.NextId();

            // 1、数据合法性验证
            Verification(0, request.StudentName, request.LinkMobile);

            // 2、处理电话号码和监护人信息
            string contactPersonMobile = ContactPersonMobile(request.ContactPerson);

            // 3、获取学生数据
            TblCstStudent student = this.GetStudent(request, studentId, contactPersonMobile);
            _studentRepository.Value.Add(student);

            // 4、构造学生联系人
            List<TblCstStudentContact> studentContactList = GetStudentContact(student.StudentId, student.ContactPerson);
            _studentContactRepository.Value.Add(studentContactList);

            // 5、推送学生至档案库
            StudentRequest s = GetStudentToAC(student);
            new ACService().StudentInfoToArtLibrary(s);

            // 6、将学生的家长账号保存推送至家校互联账户
            // 学生注册,学长的账号只会增加不会存在删除用。
            StudentPassportChangeInDto passportChangeInDto = new StudentPassportChangeInDto();
            passportChangeInDto.MobileAddList = new List<string>();
            passportChangeInDto.MobileAddList.Add(request.LinkMobile);
            passportChangeInDto.MobileAddList.AddRange(studentContactList.Select(t => t.Mobile));
            passportChangeInDto.MobileAddList = passportChangeInDto.MobileAddList.Distinct().ToList();
            new StudentFamilyProducerService().Publish(passportChangeInDto);

            return studentId.ToString();
        }

        /// <summary>
        /// 获取学生联系人
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2019-03-06 </para>
        /// </summary>
        /// <param name="studentId">学生信息</param>
        /// <param name="contactPerson">监护人信息</param>
        /// <returns>返回学生联系人信息</returns>
        private static List<TblCstStudentContact> GetStudentContact(long studentId, string contactPerson)
        {
            List<TblCstStudentContact> list = new List<TblCstStudentContact>();

            // 反序列化学生联系人信息
            List<GuardianRequest> contactPersonList = JsonConvert.DeserializeObject<List<GuardianRequest>>(contactPerson);
            foreach (var item in contactPersonList)
            {
                // 判断联系人信息都不为空才加入到集合里面去
                if (!string.IsNullOrWhiteSpace(item.GuardianName)
                    || !string.IsNullOrWhiteSpace(item.Relationship)
                    || !string.IsNullOrWhiteSpace(item.Mobile)
                    || !string.IsNullOrWhiteSpace(item.Email))
                {
                    TblCstStudentContact studentContact = new TblCstStudentContact
                    {
                        StudentContactId = IdGenerator.NextId(),
                        StudentId = studentId,
                        GuardianName = item.GuardianName,
                        Email = item.Email,
                        Mobile = item.Mobile,
                        Relationship = item.Relationship,
                        CreateTime = DateTime.Now
                    };
                    list.Add(studentContact);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取学生信息推送至档案库
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2019-03-06 </para>
        /// </summary>
        /// <param name="student">学生信息</param>
        /// <returns>返回档案库需要的学生信息</returns>
        private static StudentRequest GetStudentToAC(TblCstStudent student)
        {
            GuardianRequest[] contactPerson = JsonConvert.DeserializeObject<List<GuardianRequest>>(student.ContactPerson).ToArray();

            return new StudentRequest
            {
                StuSNo = student.StudentId,
                Name1 = student.StudentName,
                DOB = student.Birthday,
                Mobile = student.LinkMobile,
                Gender = (byte)student.Sex,
                Email1 = student.LinkMail,
                Email2 = contactPerson[0]?.Email,
                Email3 = contactPerson[1]?.Email,
                Guard1Tel = contactPerson[0]?.Mobile,
                Guard2Tel = contactPerson[1]?.Mobile
            };
        }

        /// <summary>
        /// 获取学生信息
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2019-03-06 </para>
        /// </summary>
        /// <param name="dto">添加学生信息</param>
        /// <param name="studentId">学生编号</param>
        /// <param name="contactPersonMobile">联系人电话</param>
        /// <returns>返回学生信息</returns>
        private TblCstStudent GetStudent(StudentRegisterRequest dto, long studentId, string contactPersonMobile)
        {
            return new TblCstStudent
            {
                StudentId = studentId,
                StudentNo = CreateStudentNo(studentId),
                StudentName = dto.StudentName.Trim(),
                Sex = (byte)dto.Sex,
                Birthday = dto.Birthday,
                HeadFaceUrl = dto.HeadFaceUrl,
                CurrentSchool = dto.CurrentSchool,
                AreaId = dto.AreaId,
                HomeAddress = dto.HomeAddress,
                HomeAddressFormat = JsonConvert.SerializeObject(dto.HomeAddressFormat),
                IDType = (int)dto.IDType,
                IDNumber = dto.IDNumber,
                LinkMobile = dto.LinkMobile,
                LinkMail = dto.LinkMail,
                ContactPerson = JsonConvert.SerializeObject(dto.ContactPerson),
                ContactPersonMobile = contactPersonMobile ?? "",
                CustomerFrom = (int)dto.CustomerFrom,
                ParentId = dto.ParentId,
                Remark = dto.Remark ?? "",
                CreateTime = DateTime.Now
            };
        }

        /// <summary>
        /// 处理监护人信息
        /// <para>作     者:Huang GaoLiang  </para>
        /// <para>创建时间: 2019-11-02 </para>
        /// </summary>
        /// <param name="guardianList">监护人信息</param>
        /// <returns>返回监护人联系电话</returns>
        /// <exception>
        /// 异常ID：1,监护人信息不能全部为空
        /// </exception>
        private static string ContactPersonMobile(List<GuardianRequest> guardianList)
        {
            // 如果监护人信息不存在
            if (!guardianList.Any())
            {
                throw new BussinessException((byte)ModelType.SignUp, 2);
            }
            List<string> conMobile = guardianList.Select(m => m.Mobile).Where(m => !string.IsNullOrWhiteSpace(m)).ToList();

            return string.Join(",", conMobile);
        }


        /// <summary>
        /// 判断电话号码和学生姓名是否存在
        /// <para>作     者：Huang GaoLiang </para>
        /// <para>创建时间: 2019-11-10 </para>
        /// </summary>
        /// <param name="studentId">学生编号</param>
        /// <param name="studentName">学生姓名</param>
        /// <param name="linkMobile">联系电话</param>
        /// <exception>
        /// 异常ID：1,系统中已经存在此学生
        /// </exception>
        private static void Verification(long studentId, string studentName, string linkMobile)
        {
            // 根据学生姓名、学生编号、联系电话查询学生信息
            var list = new TblCstStudentRepository().GetCstStudentList(studentName, linkMobile, studentId);
            if (list.Any())
            {
                throw new BussinessException((byte)ModelType.SignUp, 1);
            }
        }

        #endregion

        #region ModifyHeadFace 更新最新头像
        /// <summary>
        /// 更新最新头像
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2019-12-24 </para>
        /// </summary>
        /// <param name="studentId">学生ID</param>
        /// <param name="newHeadFace">新头像</param>
        /// <exception>
        /// 异常ID：1,系统不存在该学生
        /// </exception>
        public static void ModifyHeadFace(long studentId, string newHeadFace)
        {
            // 根据学生编号查询学生
            TblCstStudent student = new TblCstStudentRepository().GetCstStudentId(studentId);
            if (student == null)
            {
                throw new BussinessException((byte)ModelType.Customer, 1);
            }
            student.HeadFaceUrl = newHeadFace;
            new TblCstStudentRepository().Update(student);
        }
        #endregion

        #region RunStudentStatusJob 学生在读状态更新任务
        /// <summary>
        /// 学生在读状态更新任务
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-11-12 </para>
        /// </summary>
        public static async Task RunStudentStatusJob()
        {
            // 1、获取所有的校区
            List<TblCstSchoolStudent> schools = new TblCstSchoolStudentRepository().LoadList(m => true).ToList();

            // 2、以校区为单位，更新校区学生的剩余课次和学生的在读态度
            foreach (TblCstSchoolStudent s in schools)
            {
                //2.1、根据校区编号和学生编号获取学生的剩余课次
                s.RemindClassTimes = new StudentTimetableService(s.SchoolId, s.StudentId).GetStudentRemindClassTimes();

                await new TblCstSchoolStudentRepository().UpdateStudyRemindClassTimes(s);// 更新学生的剩余课次

                await UpdateStuStudyStatus(s, schools);//更新学生的学生状态
            }
        }
        /// <summary>
        /// 更新学生的学生状态
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-11-12 </para>
        /// </summary>
        /// <param name="student">校区学生信息</param>
        /// <param name="schools">校区集合</param>
        private static async Task UpdateStuStudyStatus(TblCstSchoolStudent student, List<TblCstSchoolStudent> schools)
        {
            // 1、根据学生编号获取学生最近一条的休学信息
            TblOdrLeaveSchoolOrder leave = new LeaveSchoolOrderService(student.SchoolId, student.StudentId).GetLeaveSchoolList();

            // 2、根据条件设置学生在校状态
            GetStudentStatus(leave, student);

            // 3、根据校区编号获取该校区的学生信息
            List<TblCstSchoolStudent> schoolStuStudyStatus = schools.Where(m => m.SchoolId == student.SchoolId).ToList();

            // 4、按照学生编号和在读状态分组
            var list = schoolStuStudyStatus.GroupBy(m => m.StudyStatus).ToList();

            // 5、根据学生编号和校区编号更新学生学生状态
            foreach (var status in list)
            {
                List<long> stuIds = schoolStuStudyStatus.Where(x => x.StudyStatus == status.Key).Select(m => m.StudentId).ToList();//查找该状态下的学生编号

                await new TblCstSchoolStudentRepository().UpdateStudyStatus(student.SchoolId, stuIds, status.Key);//更新该状态下的学生状态
            }
        }

        /// <summary>
        /// 查找学生状态
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-11-14 </para>
        /// </summary>
        /// <param name="leave">休学信息</param>
        /// <param name="school">校区学生信息</param>
        private static void GetStudentStatus(TblOdrLeaveSchoolOrder leave, TblCstSchoolStudent school)
        {
            // 如果退费订单不存在
            if (leave == null)
            {
                return;
            }

            // 在读：学生总的剩余课次大于0
            if (school.RemindClassTimes > 0)
            {
                school.StudyStatus = (byte)StudyStatus.Reading;
            }

            // 休学：剩余课次等于0，并且当前时间处于 休学开始日期至复课日期之间）  休学开始日期<=当前时间<=复课日期之间 或者剩余课次等于0 && 当前时间< 休学开始日期
            if (school.RemindClassTimes <= 0 && (DateTime.Now >= leave.LeaveTime && DateTime.Now < leave.ResumeTime) ||
                DateTime.Now < leave.LeaveTime)
            {
                school.StudyStatus = (byte)StudyStatus.Suspension;
            }

            // 流失：剩余课次等于0，并且当前时间不处于 休学的开始日期至复课日期之间 当前时间>复课日期 或者当前时间<休学的开始日期
            if (school.RemindClassTimes <= 0 && DateTime.Now > leave.ResumeTime)
            {
                school.StudyStatus = (byte)StudyStatus.Loss;
            }
        }

        #endregion

        #region UpdateStudentStatusById 根据学生编号更新学生状态 
        /// <summary>
        /// 根据学生编号更新学生的
        /// <para>作     者:Huang GaoLiang  </para>
        /// <para>创建时间：2018-11-26  </para>
        /// </summary>
        /// <param name="studentId">学生编号</param>
        /// <param name="status">学生状态</param>
        /// <exception>
        /// 异常ID：1,系统中已经存在此学生
        /// </exception>
        internal void UpdateStudentStatusById(long studentId, StudyStatus? status = null)
        {
            // 1、根据学生编号查询学生信息
            TblCstSchoolStudent stu = _schoolStudentRepository.Value.GetStudentById(_schoolId, studentId).Result;

            if (stu == null)
            {
                throw new BussinessException((byte)ModelType.Customer, 1);
            }
            stu.RemindClassTimes = new StudentTimetableService(_schoolId, studentId).GetStudentRemindClassTimes();//获取此学生的报名中的剩余课次

            // 2、根据校区编号和学生编号获取此学生的休学开始日期和休学结束日期
            TblOdrLeaveSchoolOrder leave = new LeaveSchoolOrderService(_schoolId, stu.StudentId).GetLeaveSchoolList();

            SetStudyStatus(status, stu, leave);

            _schoolStudentRepository.Value.Update(stu);
        }

        /// <summary>
        /// 根据学生编号更新学生状态 UpdateStudentStatusById 带事务
        /// <para>作     者:Huang GaoLiang </para> 
        /// <para>创建时间：2018-11-26 </para>
        /// </summary>
        /// <param name="studentId">学生编号</param>
        /// <param name="unitOfWork">事务</param>
        /// <param name="status">学生状态</param>
        /// <exception>
        /// 异常ID：1,系统中已经存在此学生
        /// </exception>
        internal void UpdateStudentStatusById(long studentId, UnitOfWork unitOfWork, StudyStatus? status = null)
        {
            // 1、根据学生编号查询学生信息
            TblCstSchoolStudent stu = unitOfWork.GetCustomRepository<TblCstSchoolStudentRepository, TblCstSchoolStudent>().Load(m => m.SchoolId == _schoolId && m.StudentId == studentId);
            if (stu == null)
            {
                throw new BussinessException((byte)ModelType.Customer, 1);
            }

            stu.RemindClassTimes = new StudentTimetableService(_schoolId, studentId).GetStudentRemindClassTimes(unitOfWork); ;//获取此学生的报名中的剩余课次
                                                                                                                              //2、根据校区编号和学生编号获取此学生的休学开始日期和休学结束日期
            TblOdrLeaveSchoolOrder leave = new LeaveSchoolOrderService(_schoolId, stu.StudentId).GetLeaveSchoolList(unitOfWork);

            SetStudyStatus(status, stu, leave);

            unitOfWork.GetCustomRepository<TblCstSchoolStudentRepository, TblCstSchoolStudent>().Update(stu);
        }

        /// <summary>
        /// 设置学生状态
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-11-26 </para>
        /// </summary>
        /// <param name="status">学生状态</param>
        /// <param name="stu">学生信息</param>
        /// <param name="leave">退费订单信息</param>
        private static void SetStudyStatus(StudyStatus? status, TblCstSchoolStudent stu, TblOdrLeaveSchoolOrder leave)
        {
            // 如果剩余课次大于0
            if (stu.RemindClassTimes > 0)
            {
                stu.StudyStatus = (int)StudyStatus.Reading; //总剩余课次大于0
            }
            else
            {
                // 如果学生状态存在并且当前时间不大于休学时间
                if (status.HasValue && leave != null && DateTime.Now <= leave.LeaveTime)
                {
                    stu.StudyStatus = (int)status;
                }
                // 如果学生状态存在并且当前时间不小于休学时间
                else if (status.HasValue && leave != null && DateTime.Now >= leave.ResumeTime)
                {
                    stu.StudyStatus = (int)status;
                }
                // 如果学生状态存在并且当前时间不小于休学时间并且当前时间不大于休学时间
                else if (status.HasValue && leave != null && DateTime.Now >= leave.LeaveTime && DateTime.Now <= leave.ResumeTime)
                {
                    stu.StudyStatus = (int)status;
                }
                else
                {
                    stu.StudyStatus = (int)StudyStatus.Loss; //如果课次全部转校转出，则该学生的状态为流失 2018年12月3日
                }
            }
        }

        #endregion

        #region RunStudentInfoJob 更新学生联系人至学生联系人表

        /// <summary>
        /// 更新学生联系人至学生联系人表
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2019-03-06 </para>
        /// </summary>
        /// <param name="passWord">密码</param>
        /// <exception>
        /// 异常ID：3,密码不正确，请重新输入
        /// </exception>
        public static void RunStudentInfoJob(string passWord)
        {
            if (passWord != "86337000")
            {
                throw new BussinessException((byte)ModelType.Customer, 3);
            }

            List<TblCstStudentContact> studentContactList = new List<TblCstStudentContact>();

            // 1、获取所有的学生信息
            var studentList = new TblCstStudentRepository().LoadList(m => true).Select(m => new GuardianOutDto
            {
                StudentId = m.StudentId,
                Mobile = m.LinkMobile,
                ContactPerson = m.ContactPerson
            }).ToList();

            foreach (var s in studentList)
            {
                studentContactList.AddRange(GetStudentContact(s.StudentId, s.ContactPerson));
            }

            // 2、批量插入学生至学生联系人表中
            var studentIds = studentList.Select(m => m.StudentId).ToList();
            new TblCstStudentContactRepository().DeleteByStudentId(studentIds);
            new TblCstStudentContactRepository().Add(studentContactList);
        }

        #endregion

        #region GetStudentList 根据学生号码获取学生信息
        /// <summary>
        /// 根据学生电话号码获取学生信息
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2019-03-08 </para>
        /// </summary>
        /// <param name="mobile">电话号码</param>
        /// <returns>返回学生集合</returns>
        internal static List<TblCstStudent> GetStudentList(string mobile)
        {
            List<TblCstStudent> studentList = new List<TblCstStudent>();

            // 1、从学生表中查询符合条件的学生信息
            studentList = new TblCstStudentRepository().GetStudentList(mobile);

            // 2、从学生联系人表中查询数据
            List<long> studentIds = new TblCstStudentContactRepository().GetStudentList(mobile).Select(m => m.StudentId).ToList();
            var list = new TblCstStudentRepository().GetStudentsById(studentIds);
            studentList.AddRange(list);

            studentList = studentList.Where((x, i) => studentList.FindIndex(z => z.StudentId == x.StudentId) == i).ToList();

            return studentList;
        }
        #endregion

        #region GetStudentSchoolList 根据学生编号查询学生所在的校区编号
        /// <summary>
        /// 根据学生编号查询学生所在的校区编号
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2019-03-08 </para>
        /// </summary>
        /// <param name="studentIds">学生编号</param>
        /// <returns>返回校区编号集合</returns>
        internal static List<TblCstSchoolStudent> GetStudentSchoolList(List<long> studentIds)
        {
            List<TblCstSchoolStudent> schoolIds = new TblCstSchoolStudentRepository().GetSchoolIdByStudentIds(studentIds).ToList();
            return schoolIds;
        }
        #endregion

        #region 获取学生WxOpenId GetWxOpenId
        /// <summary>
        /// 获取学生 GetWxOpenId
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="student">学生信息</param>
        /// <returns>微信OPENID</returns>
        internal static List<string> GetWxOpenId(TblCstStudent student)
        {
            PassportService passportService = new PassportService();

            List<string> phones = GetMobileList(student.LinkMobile, student.ContactPersonMobile);

            return passportService.GetByUserCodes(phones).Select(x => x.UnionId).ToList();
        }

        /// <summary>
        /// 获取学生 GetWxOpenId
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <returns>微信OPENID</returns>
        internal static List<string> GetWxOpenId(long studentId)
        {
            List<string> phones = GetMobiles(studentId);
            PassportService passportService = new PassportService();
            return passportService.GetByUserCodes(phones).Select(x => x.UnionId).ToList();
        }
        #endregion

        #region 获取学生联系人手机号码 GetMobiles
        /// <summary>
        /// 获取学生联系人手机号码
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <returns>学生联系人手机号码</returns>
        internal static List<string> GetMobiles(long studentId)
        {
            var student = GetStudentInfo(studentId);

            if (student == null)
            {
                return new List<string>();
            }

            return GetMobileList(student.LinkMobile, student.ContactPersonMobile);
        }
        #endregion
    }
}
