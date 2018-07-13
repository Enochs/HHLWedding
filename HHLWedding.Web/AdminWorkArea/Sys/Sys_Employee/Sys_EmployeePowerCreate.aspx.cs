using HHLWedding.BLLAssmbly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HHLWedding.ToolsLibrary;
using HHLWedding.DataAssmblly;

namespace HHLWedding.Web.AdminWorkArea.Sys.Sys_Employee
{
    public partial class Sys_EmployeePowerCreate : System.Web.UI.Page
    {
        /// <summary>
        /// 所有菜单
        /// </summary>
        ChannelService _channelService = new ChannelService();

        /// <summary>
        /// 员工权限
        /// </summary>
        EmployeePower _empPowerService = new EmployeePower();


        #region 页面初始化
        /// <summary>
        /// 页面初始化
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var DataList = _channelService.GetAllByStatus(1, 0);
                rptParentChannel.DataSource = DataList;
                rptParentChannel.DataBind();
            }
        }
        #endregion


        #region Repeater绑定完成事件
        /// <summary>
       /// Repeater绑定完成
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
 
       
        protected void rptParentChannel_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int EmployeeId = Request["EmployeeId"].ToInt32();
            int ChannelId = (e.Item.FindControl("hideChannelId") as HiddenField).Value.ToInt32();
            List<Sys_Channel> channelList = _channelService.GetAllByStatus(1, ChannelId);

            Repeater rptChannel = e.Item.FindControl("rptChannel") as Repeater;
            rptChannel.DataSource = channelList;
            rptChannel.DataBind();

        }
        #endregion
    }
}