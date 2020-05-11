/****************************************************************************\
所属系统：招生系统
所属模块：基础资料模块
创建时间：2019-03-05
作   者：郭伟佳
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/

using AMS.Anticorrosion.HRS;
using AMS.Anticorrosion.JDW;
using AMS.Core;
using AMS.Core.Constants;
using AMS.Dto;
using AMS.Dto.Enum.Custom;
using AMS.Service.Hss;
using AMS.Storage;
using AMS.Storage.Models;
using AutoMapper;
using Jerrisoft.Platform.Public.PageExtensions;
using NS2.SDK;
using NS2.SDK.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AMS.Service
{
    /// <summary>
    /// 描    述：信息投诉与建议业务服务类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-05</para>
    /// </summary>
    public class FeedbackService
    {
        protected readonly Lazy<TblDatFeedbackRepository> _repository;
        private readonly string _companyId; //公司编号

        /// <summary>
        /// 信息投诉与建议实例化
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="companyId">公司编号</param>
        public FeedbackService(string companyId)
        {
            _companyId = companyId;
            _repository = new Lazy<TblDatFeedbackRepository>();
        }

        #region GetFeedBackList 根据查询条件获取投诉与建议信息
        /// <summary>
        /// 根据查询条件获取投诉与建议信息
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="search">投诉与建议查询条件</param>
        /// <returns>返回信息投诉与建议信息列表</returns>
        public PageResult<FeedbackListResponse> GetFeedBackList(FeedbackSearchRequest search)
        {
            search.CompanyId = this._companyId;
            // 获取查询结果后的分页数据
            var feedbackList = _repository.Value.GetFeedBackList(search);

            // 转换数据
            PageResult<FeedbackListResponse> list = Mapper.Map<PageResult<TblDatFeedback>, PageResult<FeedbackListResponse>>(feedbackList);

            return list;
        }
        #endregion

        #region GetFeedbackDetail 根据主键编号获取对应的投诉与建议详情
        /// <summary>
        /// 根据主键编号获取对应的投诉与建议详情
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="detailId">投诉与建议Id</param>
        /// <returns>信息投诉与建议详情</returns>
        public async Task<FeedbackDetailResponse> GetFeedbackDetail(long detailId)
        {
            // 1、根据主键编号获取对应的投诉与建议详情
            TblDatFeedback feedbackDetail = await GetFeedbackInfoById(detailId);
            var feedbackDetailResponse = Mapper.Map<FeedbackDetailResponse>(feedbackDetail);

            // 2、根据主键编号和类型获取附件
            feedbackDetailResponse.Urls = new AttchmentService(feedbackDetail.SchoolId).GetAttchList(feedbackDetail.FeedbackId, AttchmentType.FEEDBACK).Select(m => m.Url).ToList();

            return feedbackDetailResponse;
        }

        /// <summary>
        /// 根据主键编号获取对应的投诉与建议详情
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="detailId">投诉与建议Id</param>
        /// <returns>信息投诉与建议详情</returns>
        /// <exception cref = "BussinessException" >
        /// 异常ID：1，异常描述：未找到数据
        /// </exception> 
        private async Task<TblDatFeedback> GetFeedbackInfoById(long detailId)
        {
            var feedbackDetail = await _repository.Value.LoadTask(detailId);
            if (feedbackDetail == null)
            {
                throw new BussinessException((byte)ModelType.Default, 1);
            }

            return feedbackDetail;
        }
        #endregion

        #region UpdateProcessStatus 更新信息投诉与建议的处理状态
        /// <summary>
        /// 更新信息投诉与建议的处理状态
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="detailId">投诉与建议Id</param>
        /// <param name="feedbackRequest">处理信息</param>
        /// <returns>信息投诉与建议详情</returns>
        /// <exception cref = "BussinessException" >
        /// 异常ID：1，异常描述：未找到数据
        /// </exception> 
        public async Task UpdateProcessStatus(long detailId, FeedbackRequest feedbackRequest)
        {
            // 1、根据主键编号获取信息
            TblDatFeedback feedbackDetail = await GetFeedbackInfoById(detailId);
            feedbackDetail.ProcessStatus = (int)feedbackRequest.ProcessStatus;
            feedbackDetail.ProcessRemark = feedbackRequest.ProcessRemark;
            feedbackDetail.ProcessUserId = feedbackRequest.ProcessUserId;
            feedbackDetail.ProcessUserName = feedbackRequest.ProcessUserName;
            feedbackDetail.ProcessTime = DateTime.Now;
            await _repository.Value.UpdateFeedbackById(feedbackDetail);
        }
        #endregion

        #region AddIFeedback 添加投诉与建议信息
        /// <summary>
        /// 添加投诉与建议信息
        /// <para>作    者： 郭伟佳</para>
        /// <para>创建时间： 2019-03-07</para>
        /// </summary>
        /// <param name="feedbackRequest">回馈添加请求对象</param>
        public async Task AddIFeedback(FeedbackAddRequest feedbackRequest)
        {
            //1、数据校验
            CheckData(feedbackRequest);

            //2、添加投诉与建议信息
            TblDatFeedback feedbackInfo = new TblDatFeedback()
            {
                FeedbackId = IdGenerator.NextId(),
                CompanyId = _companyId,
                SchoolId = feedbackRequest.SchoolId,
                SchoolName = feedbackRequest.SchoolName,
                StudentId = feedbackRequest.StudentId,
                ParentUserCode = feedbackRequest.LinkMobile,
                LinkMobile = feedbackRequest.LinkMobile,
                CreatorName = feedbackRequest.CreatorName,
                Content = feedbackRequest.Content,
                CreateTime = DateTime.Now,
                ProcessStatus = (int)FeedbackProcessStatus.ToBeProcessed
            };
            await _repository.Value.AddTask(feedbackInfo);

            //3、保存上传的附件
            await AddAttachment(feedbackRequest.SchoolId, feedbackRequest.AttachmentUrlList, feedbackInfo.FeedbackId);

            //4、发送投诉与建议的微信通知
            SendWxMessage(feedbackInfo);

            //5、发送投诉与建议的邮件
            SendEmail(feedbackRequest, feedbackInfo);
        }

        /// <summary>
        /// 发送投诉与建议的邮件
        /// <para>作    者： 郭伟佳</para>
        /// <para>创建时间： 2019-03-07</para>
        /// </summary>
        /// <param name="feedbackRequest">回馈添加请求对象</param>
        /// <param name="feedbackInfo">投诉与建议信息</param>
        private void SendEmail(FeedbackAddRequest feedbackRequest, TblDatFeedback feedbackInfo)
        {
            //1、获取校区的公司信息
            var schoolInfo = new JDWService().GetSchoolInfo(feedbackInfo.SchoolId);

            //2、获取公司家校互联相关的配置
            var schoolSettingInfo = new HomeSchoolSettingService().GetSettingList(schoolInfo.CompanyId)
                .Where(a => a.FuntionId == (int)HomeSchoolBusinessType.Feedback).FirstOrDefault();
            if (schoolSettingInfo != null)
            {
                //邮件内容
                StringBuilder contentMsg = new StringBuilder();
                contentMsg.Append("<div>");
                contentMsg.Append($"<span>{DatumConstants.Content}：{feedbackInfo.Content}</span><br /><br />");
                contentMsg.Append($"<span>{DatumConstants.School}： {feedbackInfo.SchoolName}</span><br />");
                contentMsg.Append($"<span>{DatumConstants.ContactPhone}：{feedbackInfo.LinkMobile}</span><br />");
                contentMsg.Append($"<span>{DatumConstants.StudentName}： {feedbackInfo.CreatorName}</span><br />");
                contentMsg.Append($"<span>{DatumConstants.SubmitTime}：{feedbackInfo.CreateTime.ToString("yyyy-MM-dd HH:mm")}</span><br /><br /><br /></div>");
                //3、发送邮件
                new MailSdk(ClientConfigManager.HssConfig.EmailSender.UserId, ClientConfigManager.HssConfig.EmailSender.UserName).SendMail(new MailSendDto()
                {
                    IsSaveDrafts = false,
                    Title = DatumConstants.FeedbackTitle,
                    Contents = contentMsg.ToString(),
                    FileList = feedbackRequest.AttachmentUrlList.Select(a =>
                    {
                        //获取url文件扩展名
                        var fileSuffix = Regex.Match(a.Url, @"(\.\w+)+(?!.*(\w+)(\.\w+)+)").ToString();
                        return new FileInfoDto()
                        {
                            Url = a.Url,
                            Name = a.Name,
                            Suffix = fileSuffix.Remove(0, 1),//去掉扩展名前的点
                            Size = 0
                        };
                    }).ToList(),
                    ReceiverList = new List<MailUserInfoDto> { new MailUserInfoDto { UserId = schoolSettingInfo.DataId, UserName = schoolSettingInfo.DataValue } }
                });
            }
        }

        /// <summary>
        /// 添加投诉与建议信息之前，进行数据校验
        /// <para>作    者： 郭伟佳</para>
        /// <para>创建时间： 2019-03-07</para>
        /// </summary>
        /// <param name="feedbackRequest">回馈添加请求对象</param>
        private void CheckData(FeedbackAddRequest feedbackRequest)
        {
            //检查学生是否存在系统中
            var studentInfo = StudentService.GetStudentInfo(feedbackRequest.StudentId);
            if (studentInfo == null)
            {
                throw new BussinessException((byte)ModelType.Customer, 1);
            }
        }

        /// <summary>
        /// 保存投诉与建议上传的附件
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-08</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="attachmentUrlList">附件地址列表</param>
        /// <param name="feedbackId">投诉与建议Id</param>
        private async Task AddAttachment(string schoolId, List<AttchmentAddRequest> attachmentUrlList, long feedbackId)
        {
            if (attachmentUrlList == null || attachmentUrlList.Count == 0)
            {
                return;
            }
            attachmentUrlList.ForEach(a => { a.AttchmentType = AttchmentType.FEEDBACK; a.BusinessId = feedbackId; });
            await new AttchmentService(schoolId).AddAsync(attachmentUrlList);
        }

        /// <summary>
        /// 发送投诉与建议的微信通知
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-08</para>
        /// </summary>
        /// <param name="feedbackInfo">投诉与建议信息</param>
        public void SendWxMessage(TblDatFeedback feedbackInfo)
        {
            //获取学生信息
            var studentInfo = StudentService.GetStudentInfo(feedbackInfo.StudentId);

            //请假信息
            WxNotifyInDto wxNotify = new WxNotifyInDto
            {
                Data = new List<WxNotifyItemInDto> {
                    new WxNotifyItemInDto{ DataKey="first", Value=ClientConfigManager.HssConfig.WeChatTemplateTitle.FeedbackNotice },
                    new WxNotifyItemInDto{ DataKey="keyword1", Value=WeChatTemplateContentConstants.FeedbackReceiver },
                    new WxNotifyItemInDto{ DataKey="keyword2", Value=feedbackInfo.CreateTime.ToString("yyyy.MM.dd HH:mm") },
                    new WxNotifyItemInDto{ DataKey="remark", Value=feedbackInfo.SchoolName}
                },
                ToUser = StudentService.GetWxOpenId(studentInfo),
                TemplateId = WeChatTemplateConstants.FeedbackTemplateId,
                Url = string.Empty
            };

            WxNotifyProducerService.Instance.Publish(wxNotify);
        }

        #endregion
    }
}
