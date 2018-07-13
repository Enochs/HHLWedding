using HHLWedding.DataAssmblly.CommonModel;
using HHLWedding.ToolsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HHLWedding.Web.AdminWorkArea.Flow.Customer
{
    public partial class ImportCustomer : System.Web.UI.Page
    {
        public int employeeId;
        #region 页面加载
        /// <summary>
        /// 页面初始化
        /// </summary> 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                employeeId = LoginInfo.UserInfo.EmployeeId;
            }
        }
        #endregion


        #region 点击确定上传
        /// <summary>
        /// 确定上传
        /// </summary>
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            JavaScriptTools.AlertWindow("进入调试", Page);
        }
        #endregion
    }
}