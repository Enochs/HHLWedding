using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly;
using HHLWedding.Web.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using HHLWedding.ToolsLibrary;
using HHLWedding.DataAssmblly.CommonModel;

namespace HHLWedding.Web.WebService
{
    /// <summary>
    /// DepartmentHandler 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class DepartmentHandler : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        Department _departmentService = new Department();

        #region 添加部门
        /// <summary>
        /// @author:wupeng
        /// @datetime:2016-07-28
        /// @desc: 添加部门
        /// </summary>
        /// <param nme="_department">部门实体类</param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage CreateDepartment(Sys_Department _department)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            if (_department != null)
            {
                var parentDepartment = _departmentService.GetByID(_department.Parent);
                _department.EmployeeID = User.Identity.Name.ToInt32();
                _department.CreateTime = DateTime.Now;
                if (_department.Parent == 0)
                {
                    _department.ItemLevel = 1;
                }
                else
                {
                    _department.ItemLevel = parentDepartment.ItemLevel + 1;
                }
                _department.SortOrder = _departmentService.GetMaxSort(_department.ItemLevel.ToString().ToInt32(), _department.Parent.ToString().ToInt32());
                _department.Status = (byte)SysStatus.Enable;

                int result = _departmentService.Insert(_department);
                if (result > 0)
                {
                    ajax.IsSuccess = true;
                    ajax.Message = "添加成功";
                }
            }

            return ajax;
        }
        #endregion

        #region 修改部门
        /// <summary>
        /// @author:wupeng
        /// @datetime:2016-07-28
        /// @desc: 修改部门
        /// </summary>
        /// <param nme="department">部门Id</param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage UpdateDepartment(Sys_Department _department)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            if (_department != null)
            {
                Sys_Department department = _departmentService.GetByID(_department.DepartmentID);
                department.DepartmentName = _department.DepartmentName;
                int result = _departmentService.Update(department);

                if (result > 0)
                {
                    ajax.IsSuccess = true;
                    ajax.Message = "修改成功";
                }
            }

            return ajax;
        }
        #endregion

        #region 删除部门
        /// <summary>
        /// @author:wupeng
        /// @datetime:2016-07-28
        /// @desc: 删除部门
        /// </summary>
        /// <param nme="department">部门Id</param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage DeleteDepartment(int DepartmentId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            if (DepartmentId != 0 && !string.IsNullOrEmpty(DepartmentId.ToString()))
            {
                Sys_Department department = _departmentService.GetByID(DepartmentId);
                int result = _departmentService.Delete(department);

                if (result > 0)
                {
                    ajax.IsSuccess = true;
                    ajax.Message = "删除成功";
                }
            }

            return ajax;
        }
        #endregion

        #region 根据部门Id 查找部门
        /// <summary>
        /// @author:wupeng
        /// @datetime:2016-07-28
        /// @desc: 根据部门Id查找部门
        /// </summary>
        /// <param nme="parentId">部门Id</param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage GetDepartmentById(int parentId, int Department)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            if (Department == 0)
            {
                if (parentId != 0 && !string.IsNullOrEmpty(parentId.ToString()))
                {
                    Sys_Department department = _departmentService.GetByID(parentId);
                    if (department != null)
                    {
                        ajax.IsSuccess = true;
                        ajax.Value = department.DepartmentName;
                        ajax.Message = "加载成功";
                    }
                }
            }
            else if (Department != 0)
            {
                Sys_Department department = _departmentService.GetByID(Department);
                string parentName = "顶级部门";
                if (parentId != 0)
                {
                    Sys_Department parentDepartment = _departmentService.GetByID(parentId);
                    parentName = parentDepartment.DepartmentName;
                }

                ajax.IsSuccess = true;
                string[] name = new string[2];
                name[0] = department.DepartmentName;
                name[1] = parentName;
                ajax.Name = name;
                ajax.Message = "加载成功";
            }

            return ajax;
        }
        #endregion

        #region 获取所有部门
        /// <summary>
        /// @author:wupeng
        /// @datetime:2016-07-30
        /// @desc: 获取所有部门(启用)
        /// </summary>
        [WebMethod]
        public AjaxMessage GetAllEmployeeJob()
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                List<Sys_Department> list = _departmentService.GetByAll(); ;


                if (list.Count > 0)
                {
                    ajax.IsSuccess = true;
                    ajax.Data = list;
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;
        }
        #endregion

        #region 设置部门主管
        /// <summary>
        /// @author:wupeng
        /// @datetime:2016-08-02
        /// @desc:修改部门主管
        /// </summary>
        [WebMethod]
        public AjaxMessage SetDepartmentManager(int EmployeeId, int DepartmentId)
        {
            AjaxMessage ajax = new AjaxMessage();

            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                Sys_Department department = _departmentService.GetByID(DepartmentId);
                department.DepartmentManager = EmployeeId;
                int result = _departmentService.Update(department);
                if (result > 0)
                {
                    ajax.IsSuccess = true;
                    ajax.Message = "修改成功";
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }
            return ajax;
        }
        #endregion
    }
}
