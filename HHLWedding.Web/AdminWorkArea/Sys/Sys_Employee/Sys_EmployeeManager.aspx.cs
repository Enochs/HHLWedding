using HHLWedding.BLLAssmbly;
using HHLWedding.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HHLWedding.DataAssmblly;
using HHLWedding.ToolsLibrary;
using HHLWedding.DataAssmblly.CommonModel;
using HHLWedding.EditoerLibrary;

namespace HHLWedding.Web.AdminWorkArea.Sys.Sys_Employee
{
    public partial class Sys_EmployeeManager : SystemPage
    {

        Employee _empService = new Employee();
        string orderName = "CreateDate";

        #region 页面初始化
        /// <summary>
        /// 页面初始化
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DDLDataBind();
                BinderData();
            }
        }
        #endregion


        #region 下拉框状态绑定
        /// <summary>
        /// 下拉框
        /// </summary>
        public void DDLDataBind()
        {
            //绑定状态
            Type type = typeof(SysStatus);

            List<ListItem> list = DisplayNameExtension.GetSelectList(SysStatus.Enable, type);
            ddlStatus.DataSource = list;
            ddlStatus.DataBind();
            //绑定部门
            GetAllDepartment(ddlDepartment);
        }
        #endregion

        #region 数据绑定
        /// <summary>
        /// 数据绑定
        /// </summary>
        public void BinderData()
        {
            int sourceCount = 0;
            
            List<PMSParameters> pars = new List<PMSParameters>();
            //员工姓名
            pars.Add(!string.IsNullOrEmpty(txtEmployee.Text.Trim().ToString()), "EmployeeName", txtEmployee.Text, NSqlTypes.LIKE);
            //状态
            pars.Add(ddlStatus.SelectedValue.ToInt32() >= 0, "Status", ddlStatus.SelectedValue.ToInt32(), NSqlTypes.Equal);
            //部门
            pars.Add(ddlDepartment.SelectedValue.ToInt32() > 0, "DepartmentID", ddlDepartment.SelectedValue.ToInt32(), NSqlTypes.Equal);

            var DataList = _empService.GetAllByPager(pars, orderName, CtrPageIndex.CurrentPageIndex, CtrPageIndex.PageSize, out sourceCount);
            CtrPageIndex.RecordCount = sourceCount;
            SavePage(CtrPageIndex);

            repEmployee.DataSource = DataList;
            repEmployee.DataBind();
        }
        #endregion


        #region 为实现异步刷新
        /// <summary>
        /// 异步刷新
        /// </summary>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            BinderData();
        }
        #endregion

        #region 分页
        /// <summary>
        /// 分页 页码
        /// </summary>
        protected void CtrPageIndex_PageChanged(object sender, EventArgs e)
        {
            BinderData();
        }
        #endregion
    }
}