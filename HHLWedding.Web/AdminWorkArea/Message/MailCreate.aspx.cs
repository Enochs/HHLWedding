using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly;
using HHLWedding.Pages;
using System;
using HHLWedding.ToolsLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HHLWedding.DataAssmblly.CommonModel;

namespace HHLWedding.Web.AdminWorkArea.Message
{
    public partial class MailCreate : SystemPage
    {
        #region 数据服务

        /// <summary>
        /// 员工信息
        /// </summary>
        Employee _empService = new Employee();

        #endregion

        public int empId;

        #region 页面加载
        /// <summary>
        /// 页面初始化
        /// </summary> 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                empId = LoginInfo.UserInfo.EmployeeId;
                DDlDataBinder();
            }
        }
        #endregion

        #region 绑定员工
        /// <summary>
        /// @author;wp
        /// @datetime:2016-09-08
        /// @desc:绑定员工(checkbox)
        /// </summary>
        public void DDlDataBinder()
        {
            //获取启用的员工
            var DataList = _empService.GetByIsDelete(1);


            //不能选择自己(发件人和收件人不能是同一个人)

            Sys_Employee self = _empService.GetByID(User.Identity.Name.ToInt32());
            if (self != null) DataList.Remove(self);

            //收件人
            repEmployee.DataSource = DataList;
            repEmployee.DataBind();
        }
        #endregion
    }
}