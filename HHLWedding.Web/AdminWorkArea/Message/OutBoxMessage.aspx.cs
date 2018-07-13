using HHLWedding.BLLAssmbly;
using HHLWedding.BLLAssmbly.FD;
using HHLWedding.Pages;
using HHLWedding.ToolsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HHLWedding.Web.AdminWorkArea.Message
{
    public partial class OutBoxMessage : SystemPage
    {
        #region 服务

        /// <summary>
        /// 通用DBHelper
        /// </summary>
        DBHelper db = new DBHelper();

        /// <summary>
        /// 消息
        /// </summary>
        MessageService _msgService = new MessageService();
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

        #region 数据绑定  发件箱
        /// <summary>
        /// 绑定消息
        /// </summary>
        public void BinderData()
        {

            List<PMSParameters> pars = new List<PMSParameters>();
            //var DataList = db.GetExcuteForList<sm_Message>(pars, 10, 1, orderByName, "MessageId", ref sourceCount);
            var DataList = _msgService.GetAllMessage(User.Identity.Name.ToInt32(), 2);
            repMessage.DataSource = DataList;
            repMessage.DataBind();
        }
        #endregion

        #region 异步刷新
        /// <summary>
        /// @author:wp
        /// @datetime:2016-08-16
        /// @desc: 异步刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            BinderData();
        }
        #endregion
    }
}