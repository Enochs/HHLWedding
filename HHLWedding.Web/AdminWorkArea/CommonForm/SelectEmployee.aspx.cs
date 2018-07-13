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

namespace HHLWedding.Web.AdminWorkArea.CommonForm
{
    public partial class SelectEmployee : SystemPage
    {
        HHLWedding.BLLAssmbly.Department _departService = new HHLWedding.BLLAssmbly.Department();

        Employee _empService = new Employee();

        #region 页面初始化
        /// <summary>
        /// 绑定数据
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                //加载部门信息
                BindTree(this.treeViewDepartment, null, "0");
            }
        }
        #endregion

        #region 树形递归公司
        /// <summary>
        /// 递归
        /// </summary>
        /// <param name="tr">TreeView</param>
        /// <param name="node">节点</param>
        private void BindTree(TreeView tr, TreeNode node, string Pid = "0")
        {

            var DepartList = _departService.GetByAll();
            if (node == null)
            {
                List<Sys_Department> DataList = DepartList.Where(C => C.Parent == Pid.ToInt32()).ToList();
                if (DataList.Count > 0)
                {
                    for (int i = 0; i < DataList.Count; i++)
                    {
                        node = new TreeNode();
                        var Objitem = DataList[i];
                        node.Text = Objitem.DepartmentName.ToString();
                        node.Value = Objitem.DepartmentID.ToString();
                        tr.Nodes.Add(node);
                        BindTree(null, node, node.Value);
                    }
                }
            }
            else
            {

                var list = _departService.GetByAll().Where(C => C.Parent == node.Value.ToInt32()).ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    var objItem = list[i];
                    TreeNode ChildNodes = new TreeNode();
                    ChildNodes.Text = objItem.DepartmentName.ToString();
                    ChildNodes.Value = objItem.DepartmentID.ToString();
                    node.ChildNodes.Add(ChildNodes);

                    BindTree(null, ChildNodes, ChildNodes.Value);
                }
            }
        }
        #endregion

        #region 部门选择变化事件
        /// <summary>
        /// 部门选择变化事件
        /// </summary>
        protected void treeViewDepartment_SelectedNodeChanged(object sender, EventArgs e)
        {
            int DepartmentId= treeViewDepartment.SelectedValue.ToInt32();

            List<Sys_Employee> EmployeeList = _empService.GetByDepartmetnID(DepartmentId);

            //根据部门查询人员
            rptEmployee.DataSource = EmployeeList;
            rptEmployee.DataBind();
        }
        #endregion
    }
}