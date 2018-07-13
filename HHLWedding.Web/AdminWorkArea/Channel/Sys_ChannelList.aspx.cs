using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HHLWedding.ToolsLibrary;
using HHLWedding.EditoerLibrary;
using HHLWedding.Pages;
using System.Web.Services;
using HHLWedding.Web.Handler;
using HHLWedding.DataAssmblly.CommonModel;

namespace HHLWedding.Web.AdminWorkArea.Channel
{
    public partial class Sys_ChannelList : SystemPage
    {
        ChannelService _channelService = new ChannelService();
        string orderByName = "CreateDate";

        #region 页面初始化
        /// <summary>
        /// 页面初始化
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                BindAlllChannel();
                string url = Page.Request.Url.ToString();
                BinderData(true);
            }
        }
        #endregion

        #region 数据绑定
        /// <summary>
        /// 数据绑定
        /// </summary>
        public void BinderData(bool First = false)
        {
            int sourceCount = 0;
            List<PMSParameters> pars = new List<PMSParameters>();

            if (!string.IsNullOrEmpty(txtChannelName.Text))
            {
                pars.Add("ChannelName", txtChannelName.Text.Trim().ToString(), NSqlTypes.LIKE);
            }

            if (ddlAllParentSystem.SelectedValue.ToInt32() > 0)
            {
                int ChannelId = ddlAllParentSystem.SelectedValue.ToInt32();
                pars.Add("Parent,ChannelId", ChannelId, NSqlTypes.OrInt);
            }
            else if (ddlAllParentSystem.SelectedValue.ToInt32() == 0)
            {
                pars.Add("Parent", 0, NSqlTypes.Equal);
            }

            var DataList = _channelService.GetAllByPager(pars, orderByName, CtrPageIndex.CurrentPageIndex, CtrPageIndex.PageSize, out sourceCount);
            CtrPageIndex.RecordCount = sourceCount;

            if (First == true)
            {
                if (Request["page"] != null)
                {
                    int page = Request.QueryString["page"].ToString().ToInt32();
                    CtrPageIndex.CurrentPageIndex = page;
                }
            }

            SavePage(CtrPageIndex);
            repChannel.DataSource = DataList;
            repChannel.DataBind();

            //获取视图顶级
            var ParentData = _channelService.GetAllByParent(0);
            repParent.DataSource = ParentData;
            repParent.DataBind();

            JsReload();
        }
        #endregion

        #region 绑定所有菜单系统
        /// <summary>
        /// 下拉框绑定菜单
        /// </summary>
        public void BindAlllChannel()
        {
            List<Sys_Channel> channelList = _channelService.GetAllParent();

            ddlAllParentSystem.DataSource = channelList;
            ddlAllParentSystem.DataTextField = "ChannelName";
            ddlAllParentSystem.DataValueField = "ChannelId";
            ddlAllParentSystem.DataBind();
            ddlAllParentSystem.Items.Insert(0, new ListItem("顶级系统", "0"));
            ddlAllParentSystem.Items.Insert(0, new ListItem("请选择", "-1"));
        }
        #endregion

        #region 获取上级名称
        /// <summary>
        /// 获取上级名称
        /// </summary>  
        public string GetParentName(object Source)
        {
            int ParentId = Source.ToString().ToInt32();
            var m_Channel = _channelService.GetByID(ParentId);
            if (m_Channel != null)
            {
                return m_Channel.ChannelName;
            }
            return "顶级频道";
        }
        #endregion

        #region 分页
        /// <summary>
        /// 分页查询
        /// </summary>
        protected void CtrPageIndex_PageChanged(object sender, EventArgs e)
        {
            CtrPageIndex.PageSize = ddlPageSize.SelectedItem.Text.ToInt32();
            BinderData();
        }
        #endregion

        #region 点击搜索
        /// <summary>
        /// 搜索
        /// </summary>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CtrPageIndex.CurrentPageIndex = 1;
            BinderData();
        }
        #endregion

        #region Repeater绑定事件
        /// <summary>
        /// 绑定事件
        /// </summary>
        protected void repChannel_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int ChannelId = e.CommandArgument.ToString().ToInt32();
            if (e.CommandName == "Status")
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

                    _channelService.Update(m_channel);
                }
                BinderData();
            }
        }
        #endregion

        #region 视图二级数据绑定
        /// <summary>
        /// 二级数据绑定
        /// </summary>
        protected void repParent_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField hideParent = (HiddenField)e.Item.FindControl("HideParentId");
            Repeater repChanel = e.Item.FindControl("repChannel") as Repeater;

            int parent = hideParent.Value.ToInt32();

            var DataList = _channelService.GetAllByParent(parent);

            repChanel.DataSource = DataList;
            repChanel.DataBind();
        }
        #endregion

        #region 回传  异步刷新
        /// <summary>
        /// 异步刷新页面
        /// </summary>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            BinderData();
        }
        #endregion

    }
}