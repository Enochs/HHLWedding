using HHLWedding.BLLAssmbly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HHLWedding.ToolsLibrary;
using HHLWedding.Pages;

namespace HHLWedding.Web.AdminWorkArea.Sys.Department
{
    public partial class Sys_DepartmentManager : SystemPage
    {
        HHLWedding.BLLAssmbly.Department ObjDepartmentBLL = new HHLWedding.BLLAssmbly.Department();

        Employee ObjEmployeeBLL = new Employee();

        #region 页面初始化
        /// <summary>
        /// 绑定数据
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //初始化加载部门信息
                DataBinder();
            }
        }
        #endregion

        #region 数据绑定
        /// <summary>
        /// 加载部门信息绑定Repeater控件
        /// </summary>
        private void DataBinder()
        {
            var query = ObjDepartmentBLL.GetByAll();
            //加载部门信息
            rptDepartment.DataSource = query;
            rptDepartment.DataBind();
        }
        #endregion

        /// <summary>
        /// 通过ItemCommand事件进行删除操作
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void rptDepartment_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            //if (e.CommandName == "Delete")
            //{
            //    int departmentId = e.CommandArgument.ToString().ToInt32();
            //    var EmployeeList = ObjEmployeeBLL.GetByDepartmetnID(departmentId);
            //    if (EmployeeList.Count == 0)
            //    {

            //        //创建部门实体类
            //        Sys_Department sys_Department = new Sys_Department()
            //        {
            //            DepartmentID = departmentId
            //        };
            //        //删除部门
            //        ObjDepartmentBLL.Delete(sys_Department);
            //        //删除之后重新绑定数据源
            //        DataBinder();
            //    }
            //    else
            //    {
            //        JavaScriptTools.AlertWindow("此部门下还有员工存在，要删除部门请先将员工移除此部门！", Page);
            //    }

            //}


            ////保存部门负责人
            //if (e.CommandName == "SaveChange")
            //{
            //    var ObjUpdateModel = ObjDepartmentBLL.GetByID(e.CommandArgument.ToString().ToInt32());
            //    HiddenField ObjddlEmployee = (HiddenField)e.Item.FindControl("hideEmpLoyeeID");
            //    if (ObjddlEmployee.Value != string.Empty)
            //    {
            //        ObjUpdateModel.DepartmentManager = ObjddlEmployee.Value.ToInt32();
            //        ObjDepartmentBLL.Update(ObjUpdateModel);
            //    }

            //    JavaScriptTools.AlertWindow("设置成功", Page);
            //}
        }

        #region 页面刷新
        /// <summary>
        /// 实现异步刷新
        /// </summary>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            DataBinder();
        }
        #endregion

    }
}