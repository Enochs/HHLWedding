using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHLWedding.ToolsLibrary;
using Wuqi.Webdiyer;
using System.Web;
using HHLWedding.EditoerLibrary;
using HHLWedding.DataAssmblly.CommonModel;
using System.Web.UI.WebControls;
using System.Web.Security;
using HHLWedding.BLLAssmbly.FD;
using System.Web.Mvc;
using HHLWedding.BLLAssmbly.Flow;
using System.Web.UI;

namespace HHLWedding.Pages
{
    public class SystemPage : System.Web.UI.Page
    {


        #region 服务

        /// <summary>
        /// 员工
        /// </summary>
        Employee _empService = new Employee();

        /// <summary>
        /// 员工
        /// </summary>
        CustomerService _customerService = new CustomerService();

        /// <summary>
        /// 员工职务
        /// </summary>
        EmployeeJob _empJobService = new EmployeeJob();


        /// <summary>
        /// 员工类型
        /// </summary>
        EmployeeType _empTypeService = new EmployeeType();


        /// <summary>
        /// 部门
        /// </summary>
        Department _departService = new Department();

        /// <summary>
        /// 酒店
        /// </summary>
        HotelService _hotelService = new HotelService();

        /// <summary>
        /// 渠道
        /// </summary>
        BaseService<FD_SaleSource> _saleSourceService = new BaseService<FD_SaleSource>();

        /// <summary>
        /// 渠道类型
        /// </summary>
        BaseService<FD_SaleType> _saleTypeService = new BaseService<FD_SaleType>();

        #endregion

        #region 必须验证session是否有值  如果为null  跳回登录页面
        /// <summary>
        /// session验证
        /// </summary> 
        public void Page_Init(object sender, EventArgs e)
        {

            if (LoginInfo.UserInfo == null || Session["UserInfo"] == null)
            {
                singOut();              //注销
            }
        }
        #endregion

        #region 获取员工的名称
        /// <summary>
        /// 员工名
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected string GetEmployeeNames(object source)
        {
            if (source != null)
            {
                int employeeId = (source + string.Empty).ToInt32();
                Employee objEmployeeBLL = new Employee();
                Sys_Employee emp = objEmployeeBLL.GetByID(employeeId);

                if (emp != null)
                {
                    return emp.EmployeeName;
                }
            }
            return "暂无";
        }
        #endregion

        #region 获取员工姓名(多个/单个) 
        /// <summary>
        /// 获取员工姓名(多个或单个皆可)
        /// </summary>
        /// <returns></returns>
        public string GetEmployeeName(object source)
        {

            string name = "";
            if (source != null)
            {
                string empId = source.ToString();
                if (!string.IsNullOrEmpty(empId))
                {
                    if (empId.Contains(','))        //多个员工
                    {
                        name = _empService.GetMoreEmpName(empId);
                    }
                    else            //单个员工
                    {
                        name = _empService.GetByID(empId.ToInt32()).EmployeeName;
                    }
                }

            }
            return name;
        }
        #endregion

        #region 通用分页显示
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="CtrPagerIndex"></param>
        public void SavePage(AspNetPager CtrPagerIndex)
        {
            int totalPage = CtrPagerIndex.RecordCount % CtrPagerIndex.PageSize == 0 ? CtrPagerIndex.RecordCount / CtrPagerIndex.PageSize : CtrPagerIndex.RecordCount / CtrPagerIndex.PageSize + 1;
            totalPage = totalPage == 0 ? 1 : totalPage;
            CtrPagerIndex.CustomInfoHTML = "当前第" + CtrPagerIndex.CurrentPageIndex + "/" + totalPage + "页 共" + CtrPagerIndex.RecordCount + "条记录 每页" + CtrPagerIndex.PageSize + "条";

        }
        #endregion

        #region 获取状态文本
        /// <summary>
        /// 获取状态
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public string GetStatusName(object Source)
        {
            if (Source != null)
            {
                int Status = Source.ToString().ToInt32();

                SysStatus value = (SysStatus)Status;
                string name = DisplayNameExtension.GetDisplayNames(SysStatus.Enable, value.ToString());
                return name;
            }
            return "";
        }

        #endregion

        #region 获取客户进度状态
        /// <summary>
        /// 获取客户进度状态
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public string GetCustomerState(object Source)
        {
            if (Source != null)
            {
                int State = Source.ToString().ToInt32();

                CustomerState value = (CustomerState)State;
                string name = DisplayNameExtension.GetDisplayNames(CustomerState.NoInvite, value.ToString());
                return name;
            }
            return "";
        }

        #endregion


        #region 获取员工职务
        /// <summary>
        /// @author;wp
        /// @datetime:2016-08-01
        /// @desc: 获取员工职务
        /// </summary>
        /// <param name="Source">职务ID</param>
        /// <returns></returns>
        public string GetEmployeeJob(object Source)
        {
            if (Source != null)
            {
                int JboId = Source.ToString().ToInt32();

                Sys_EmployeeJob _empJob = _empJobService.GetByID(JboId);
                if (_empJob != null)
                {
                    return _empJob.Jobname;
                }

            }
            return "";
        }

        #endregion

        #region 获取部门
        /// <summary>
        /// @author;wp
        /// @datetime:2016-08-01
        /// @desc: 获取部门
        /// </summary>
        /// <param name="Source">部门Id</param>
        /// <returns></returns>
        public string GetDepartmentName(object Source)
        {
            if (Source != null)
            {
                int DepartmentID = Source.ToString().ToInt32();

                Sys_Department _empDepart = _departService.GetByID(DepartmentID);
                if (_empDepart != null)
                {
                    return _empDepart.DepartmentName;
                }

            }
            return "";
        }

        #endregion

        #region 获取员工类型
        /// <summary>
        /// @author;wp
        /// @datetime:2016-08-01
        /// @desc: 获取员工类型
        /// </summary>
        /// <param name="Source">员工类型ID</param>
        /// <returns></returns>
        public string GetEmployeeType(object Source)
        {
            if (Source != null)
            {
                int TypeId = Source.ToString().ToInt32();

                Sys_EmployeeType _empType = _empTypeService.GetByID(TypeId);
                if (_empType != null)
                {
                    return _empType.TypeName;
                }

            }
            return "";
        }

        #endregion

        #region 部门前空格
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
        #endregion

        #region 绑定所有部门
        /// <summary>
        /// 绑定部门
        /// </summary>
        public void GetAllDepartment(DropDownList ddlDepartment)
        {
            var DataList = _departService.GetByAll();

            ddlDepartment.DataSource = DataList;
            ddlDepartment.DataTextField = "DepartmentName";
            ddlDepartment.DataValueField = "DepartmentID";
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, new ListItem { Text = "请选择部门", Value = "0" });
        }
        #endregion

        #region 获取酒店类型名称
        /// <summary>
        /// 获取酒店类型名称
        /// </summary>   
        public string GetHotelTypeName(object source)
        {
            int TypeId = source.ToString().ToInt32();

            HotelTypeName value = (HotelTypeName)TypeId;
            string typename = DisplayNameExtension.GetDisplayNames(HotelTypeName.HType1, value.ToString());

            return typename.ToString();
        }
        #endregion

        #region 获取酒店名称
        /// <summary>
        /// 根据酒店Id获取酒店名称
        /// </summary>
        public string GetHotelName(object source)
        {
            string hotelName = "";
            if (source != null)
            {
                int hotelId = source.ToString().ToInt32();
                var hotel = _hotelService.GetByID(hotelId);
                if (hotel != null)
                {
                    hotelName = hotel.HotelName;
                }
            }
            return hotelName;
        }
        #endregion

        #region 获取渠道类型名称
        /// <summary>
        /// 根据渠道类型Id获取名称
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public string GetSaleTypeName(object Source)
        {
            string saleType = string.Empty;
            if (Source != null)
            {
                int SaleTypeId = Source.ToString().ToInt32();
                FD_SaleType m_saleType = _saleTypeService.GetById(SaleTypeId);
                if (m_saleType != null)
                {
                    saleType = m_saleType.SaleTypeName;
                }
            }
            return saleType;
        }
        #endregion

        #region 获取渠道名称
        /// <summary>
        /// 根据渠道Id获取名称
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public string GetSaleSourceName(object Source)
        {
            string saleSource = string.Empty;
            if (Source != null)
            {
                int sourceId = Source.ToString().ToInt32();
                FD_SaleSource m_saleSource = _saleSourceService.GetById(sourceId);
                if (m_saleSource != null)
                {
                    saleSource = m_saleSource.SourceName;
                }
            }
            return saleSource;
        }
        #endregion

        #region 获取新人姓名
        /// <summary>
        /// 获取新人姓名
        /// </summary>
        public string GetCustomerName(object source)
        {
            string name = string.Empty;
            if (source != null)
            {
                Guid CusId = new Guid(source.ToString());
                var customer = _customerService.GetById(CusId);
                if (customer != null)
                {

                    //新娘姓名
                    if (customer.Bride != "")
                    {
                        name = customer.Bride;
                    }

                    //新郎
                    if (customer.Groom != "")
                    {
                        if (name == string.Empty)
                        {
                            name = customer.Groom;
                        }
                        else
                        {
                            name += "/" + customer.Groom;
                        }
                    }

                    //经办人
                    if (customer.Operator != string.Empty)
                    {
                        if (name == string.Empty)
                        {
                            name = "经办人：" + customer.Operator;
                        }
                    }

                }
                else
                {
                    name = "无/无";
                }
            }
            return name;
        }
        #endregion

        #region 帐号注销
        /// <summary>
        /// 注销当前帐号
        /// </summary>
        public void singOut()
        {
            //清除全部Session
            Session.Abandon();
            StringControl.DeleteCookie("Login");

            //注销
            FormsAuthentication.SignOut();
            Response.Write("<script>this.top.window.location.href ='/AdminWorkArea/Login.aspx'</script>");

        }
        #endregion

        #region 防止UpdatePanel失效
        /// <summary>
        /// 防止js失效
        /// </summary>
        public void JsReload()
        {
            //UpdatePanel js方法失效
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "page", "BinderPage();", true);

        }
        #endregion
    }
}
