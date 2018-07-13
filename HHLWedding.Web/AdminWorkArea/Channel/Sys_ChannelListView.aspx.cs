using HHLWedding.BLLAssmbly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HHLWedding.ToolsLibrary;

namespace HHLWedding.Web.AdminWorkArea.Channel
{
    public partial class Sys_ChannelListView : System.Web.UI.Page
    {

        #region 服务

        ChannelService _channelService = new ChannelService();

        #endregion


        #region 页面加载
        /// <summary>
        /// 页面初始化
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BinderData();
            }
        }
        #endregion


        #region 数据绑定
        public void BinderData()
        {
            //获取视图顶级
            var ParentData = _channelService.GetAllByParent(0);
            repParent.DataSource = ParentData;
            repParent.DataBind();

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
    }
}