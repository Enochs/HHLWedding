using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly;
using HHLWedding.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HHLWedding.ToolsLibrary;

namespace HHLWedding.Web.AdminWorkArea.Channel
{
    public partial class Sys_ChannelCreate : SystemPage
    {
        ChannelService _channelService = new ChannelService();

        #region 页面加载
        /// <summary>
        /// 页面加载
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int parent = ddlAllParentSystem.SelectedValue.ToInt32();
                int maxSort = _channelService.GetMaxSort(parent);
                txtSort.Text = maxSort.ToString();

                BindAlllChannel();
            }
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
        }

        #endregion
    }
}