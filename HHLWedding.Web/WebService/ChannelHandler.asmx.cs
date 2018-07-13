using HHLWedding.BLLAssmbly;
using HHLWedding.BLLAssmbly.PublicTools;
using HHLWedding.DataAssmblly;
using HHLWedding.DataAssmblly.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace HHLWedding.Web.Handler
{
    /// <summary>
    /// ChannelHandler 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class ChannelHandler : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }


        ChannelService _channelService = new ChannelService();


        EmployeePower _powerService = new EmployeePower();

        /// <summary>
        /// 添加频道
        /// </summary>
        [WebMethod]
        public AjaxMessage CreateChannel(Sys_Channel channel)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                Sys_Channel m_channel = _channelService.GetByName(channel.ChannelName);

                if (m_channel != null)
                {
                    ajax.Message = "该频道名称已经存在";
                }
                else
                {
                    channel.SortInt = channel.SortInt ?? 10;
                    channel.CreateDate = DateTime.Now;
                    channel.Status = (byte)SysStatus.Enable;
                    channel.ItemLevel = channel.Parent == 0 ? 1 : 2;
                    channel.IndexCode = CommonService.getCode(channel.Parent.ToString() == "0" ? "" : _channelService.GetByID(channel.Parent).IndexCode.ToString(), 3, channel.Parent == 0 ? 0 : 1);

                    int result = _channelService.Insert(channel);
                    if (result >= 1)
                    {
                        ajax.IsSuccess = true;
                        ajax.Message = "添加成功";
                    }
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;
        }


        /// <summary>
        /// 修改频道信息
        /// </summary>
        [WebMethod]
        public AjaxMessage ModifyChannel(Sys_Channel channel, int ChannelId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                Sys_Channel m_channel = _channelService.GetByName(channel.ChannelName); ;
                Sys_Channel n_channel = _channelService.GetByID(ChannelId);

                if (m_channel != null)
                {
                    if (channel.ChannelName != m_channel.ChannelName)
                    {
                        ajax.Message = "该频道名称已经存在";
                    }
                }

                if (n_channel != null && ajax.Message == "")
                {
                    n_channel.ChannelName = channel.ChannelName;
                    n_channel.ChannelAddress = channel.ChannelAddress;
                    if (n_channel.Parent != channel.Parent)     //父级发生改变
                    {
                        n_channel.IndexCode = CommonService.getCode(channel.Parent.ToString() == "0" ? "" : _channelService.GetByID(channel.Parent).IndexCode.ToString(), 3, channel.Parent == 0 ? 0 : 1);
                    }
                    n_channel.Parent = channel.Parent;
                    n_channel.StyleSheethem = channel.StyleSheethem;
                    n_channel.ChannelGetType = channel.ChannelGetType;
                    n_channel.Remark = channel.Remark;
                    n_channel.SortInt = channel.SortInt ?? 10;
                    n_channel.ItemLevel = channel.Parent == 0 ? 1 : 2;
                    int result = _channelService.Update(n_channel);
                    if (result >= 1)
                    {
                        ajax.IsSuccess = true;
                        ajax.Message = "修改成功";
                    }
                }

                //修改权限表中的url
                _powerService.UpdateChannel(ChannelId, channel);

            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;
        }

        /// <summary>
        /// 单个禁用/启用
        /// </summary>
        /// <param name="ChannelId"></param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage SetSingleStatus(int ChannelId, int index)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                if (!string.IsNullOrEmpty(ChannelId.ToString()) && ChannelId != 0)
                {
                    Sys_Channel m_channel = _channelService.GetByID(ChannelId);
                    if (m_channel.Status == (byte)SysStatus.Enable)
                    {
                        m_channel.Status = (byte)SysStatus.Disable;
                    }
                    else
                    {
                        m_channel.Status = (byte)SysStatus.Enable;
                    }

                    int result = _channelService.Update(m_channel);
                    if (result > 0)
                    {
                        ajax.IsSuccess = true;
                        ajax.Id = ChannelId;
                        ajax.Index = index;
                        if (m_channel.Status == (byte)SysStatus.Disable)
                        {
                            ajax.Message = "禁用成功";
                        }
                        else
                        {
                            ajax.Message = "启用成功";
                        }
                    }

                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;

        }



        /// <summary>
        /// 批量禁用/启用
        /// </summary>
        /// <param name="type">类型 1.禁用→启用  2.启用→禁用</param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage SetPartStatus(string ChannelId, int type)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                if (!string.IsNullOrEmpty(ChannelId.ToString()))
                {
                    int result = _channelService.UpdatePartStatus(ChannelId, type);
                    if (result > 0)
                    {
                        ajax.IsSuccess = true;
                        if (type == 1)
                        {
                            ajax.Message = "启用成功";
                        }
                        else
                        {
                            ajax.Message = "禁用成功";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;

        }

        /// <summary>
        /// @datatime:2017-01-10
        /// @auth:wp
        /// @desc:根据url获取渠道信息
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage GetChannelByUrl(string url)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            try
            {
                if (!string.IsNullOrEmpty(url))
                {
                    var channel = _channelService.GetChannelByUrl(url);

                    ajax.data = channel;
                    ajax.IsSuccess = true;
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;
        }

        /// <summary>
        /// @author:wp
        /// @datetime:2017-02-27
        /// @desc:获取最大排序值
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage GetMaxSort(int parent)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            try
            {
                if (!string.IsNullOrEmpty(parent.ToString()))
                {
                    int maxSort = _channelService.GetMaxSort(parent);
                    string theme = _channelService.GetTheme(parent);


                    ajax.Value = maxSort.ToString();
                    ajax.type = theme;

                    ajax.IsSuccess = true;
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;
        }
    }
}
