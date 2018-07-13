using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HHLWedding.DataAssmblly;
using HHLWedding.BLLAssmbly;
using HHLWedding.ToolsLibrary;

namespace HHLWedding.Web.AdminWorkArea.Channel
{
    public partial class Sys_ChannelEdit : System.Web.UI.Page
    {

        ChannelService _channelService = new ChannelService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlDataBinder();
                BinderData();
            }
        }

        #region 数据绑定
        /// <summary>
        /// 数据绑定
        /// </summary>
        public void BinderData()
        {
            int ChannelId = Request["ChannelId"].ToString().ToInt32();
            var m_Channel = _channelService.GetByID(ChannelId);
            txtChannelName.Text = m_Channel.ChannelName;
            txtChannelAddress.Text = m_Channel.ChannelAddress;
            txtChannelGetType.Text = m_Channel.ChannelGetType;
            txtSort.Text = m_Channel.SortInt.ToString();
            txtRemark.Text = m_Channel.Remark.ToString();
            ddlAllParentSystem.SelectedValue = m_Channel.Parent.ToString();

        }
        #endregion

        #region 下拉框数据绑定
        /// <summary>
        /// 下拉框数据绑定
        /// </summary>
        public void ddlDataBinder()
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