using HHLWedding.BLLAssmbly;
using HHLWedding.BLLAssmbly.FD;
using HHLWedding.DataAssmblly;
using HHLWedding.DataAssmblly.CommonModel;
using HHLWedding.ToolsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HHLWedding.Web.AdminWorkArea.Test
{
    public partial class Test : System.Web.UI.Page
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

        public int empId = 0;

        #region 页面加载
        /// <summary>
        /// 页面初始化
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    empId = LoginInfo.UserInfo.EmployeeId;
                }
                catch
                {

                }

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
            var DataList = _msgService.GetAllMessage(User.Identity.Name.ToInt32(), 1);
            //repMessage.DataSource = DataList;
            //repMessage.DataBind();
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


        #region 获取员工的名称
        /// <summary>
        /// 员工名
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected string GetEmployeeName(object source)
        {
            int employeeId = (source + string.Empty).ToInt32();
            Employee objEmployeeBLL = new Employee();
            Sys_Employee emp = objEmployeeBLL.GetByID(employeeId);

            if (emp != null)
            {
                return emp.EmployeeName;
            }
            return "";
        }
        #endregion
    }
}