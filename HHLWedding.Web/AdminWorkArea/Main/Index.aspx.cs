using HHLWedding.BLLAssmbly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using HHLWedding.ToolsLibrary;
using HHLWedding.Pages;
using HHLWedding.DataAssmblly.CommonModel;
using HHLWedding.BLLAssmbly.FD;
using HHLWedding.Web.Hubs;
using Microsoft.AspNet.SignalR;

namespace HHLWedding.Web.AdminWorkArea.Main
{
    public partial class Index : SystemPage
    {
        /// <summary>
        /// 频道管理
        /// </summary>
        ChannelService ObjChannelBLL = new ChannelService();

        /// <summary>
        /// 权限管理
        /// </summary>
        EmployeePower _powerService = new EmployeePower();

        /// <summary>
        /// 消息
        /// </summary>
        MessageService _msgService = new MessageService();

        public int empId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {


            //退出系统
            string controlName = Request.Params.Get("__EVENTTARGET");
            string eventArgument = Request.Params.Get("__EVENTARGUMENT");

            if (controlName == "btnLook" && eventArgument == "1")
            {
                //注销
                singOut();
            }

            if (!IsPostBack)
            {
                try
                {
                    empId = LoginInfo.UserInfo.EmployeeId;
                }
                catch
                {
                    Page_Init(sender, e);
                }

                BinderChannel();
            }

        }


        #region 绑定父级频道
        /// <summary>
        /// 绑定频道
        /// </summary>
        private void BinderChannel()
        {


            int EmployeeId = User.Identity.Name.ToInt32();

            RepChannel.DataSource = _powerService.GetEmpParentPower(EmployeeId);
            RepChannel.DataBind();
        }
        #endregion

        #region 绑定子级频道
        /// <summary>
        /// 渠道绑定完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RepChannel_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int EmployeeId = User.Identity.Name.ToInt32();
            Repeater Objrep = (Repeater)e.Item.FindControl("repSecond");
            int channelId = ((HiddenField)e.Item.FindControl("hidekey")).Value.ToInt32();

            Objrep.DataSource = _powerService.GetEmpChildPower(EmployeeId, channelId);
            Objrep.DataBind();

        }
        #endregion

        #region 获取渠道地址
        /// <summary>
        /// 获取渠道地址
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public string GetChannelAddress(object Source)
        {
            if (Source != null)
            {
                int ChannelId = Source.ToString().ToInt32();
                var m_channel = ObjChannelBLL.GetByID(ChannelId);
                if (m_channel != null)
                {
                    return m_channel.ChannelAddress;
                }
            }
            return "";
        }
        #endregion


        /// <summary>
        /// 获取未读消息条数
        /// </summary>
        /// <returns></returns>
        public int getMsgCount()
        {
            int EmployeeId = User.Identity.Name.ToInt32();
            int count = _msgService.GetNoReadMsg(EmployeeId);
            return count;
        }
    }
}