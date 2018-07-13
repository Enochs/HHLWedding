using HHLWedding.BLLAssmbly;
using HHLWedding.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HHLWedding.ToolsLibrary;
using HHLWedding.DataAssmblly;
using HHLWedding.BLLAssmbly.PublicTools;

namespace HHLWedding.Web.AdminWorkArea.Sys
{
    public partial class Sys_DepartmentManager : SystemPage
    {
        Department ObjDepartmentBLL = new Department();

        Employee ObjEmployeeBLL = new Employee();

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string code = CommonService.getCode(3);
                //初始化加载部门信息
                //DataBinder();
            }

        }
        /// <summary>
        /// 加载部门信息绑定Repeater控件
        /// </summary>
        private void DataBinder()
        {
            //int startIndex = DepartmentPager.StartRecordIndex;
            //int resourceCount = 0;

            //var query = ObjDepartmentBLL.GetByAll();
            //DepartmentPager.RecordCount = resourceCount;
            ////加载部门信息
            //rptDepartment.DataSource = query;
            //rptDepartment.DataBind();


        }
        /// <summary>
        /// 部门前空格  使有层次性
        /// </summary>
        /// <param name="ItemLevel"></param>
        /// <returns></returns>
        public string GetItemNbsp(object ItemLevel)
        {
            if (ItemLevel != null)
            {

                int Count = ItemLevel.ToString().ToInt32();
                string Nbsp = "";
                if (Count == 1)
                {
                    return string.Empty;
                }
                for (int i = 0; i < Count; i++)
                {
                    Nbsp += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                return Nbsp;
            }
            else
            {
                return string.Empty;
            }
        }


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

        #region 分页的数据绑定
        /// <summary>
        /// 分页
        /// </summary>
        protected void DepartmentPager_PageChanged(object sender, EventArgs e)
        {
            DataBinder();
        }
        #endregion

    }
}