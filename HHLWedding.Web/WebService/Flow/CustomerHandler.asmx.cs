using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly;
using HHLWedding.DataAssmblly.CommonModel;
using HHLWedding.EditoerLibrary.Extension;
using HHLWedding.Web.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Services;
using HHLWedding.ToolsLibrary;
using System.Transactions;
using HHLWedding.BLLAssmbly.Flow;
using Newtonsoft.Json;
using HHLWedding.Pages;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using HHLWedding.EditoerLibrary;

namespace HHLWedding.Web.WebService.Flow
{
    /// <summary>
    /// CustomerHandler 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class CustomerHandler : System.Web.Services.WebService
    {

        #region 服务

        CustomerService _cusService = new CustomerService();
        BaseService<FL_Customer> _customerService = new BaseService<FL_Customer>();
        BaseService<FD_SaleSource> _saleSourceService = new BaseService<FD_SaleSource>();
        BaseService<SS_Report> _reportService = new BaseService<SS_Report>();
        BaseService<FL_Invite> _inviteService = new BaseService<FL_Invite>();
        BaseService<FL_InviteDetails> _inviteDetailsService = new BaseService<FL_InviteDetails>();
        BaseService<FL_Order> _orderService = new BaseService<FL_Order>();

        #endregion

        #region 判断手机号码是否存在
        /// <summary>
        /// 判断手机号码是否重复
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public int IsExistsPhone(string phone)
        {
            if (RegexExtension.IsValidPhone(phone))
            {
                Expression<Func<FL_Customer, bool>> pars = c => c.BridePhone == phone || c.GroomPhone == phone || c.OperatorPhone == phone;
                var model = _customerService.GetModel(pars);

                if (model != null)      //号码已经存在
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 2;
            }
        }
        #endregion

        #region 添加客户信息
        /// <summary>
        /// 添加客户信息
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage CreateCustomer(FL_Customer customer, string contactType, string orderEmp)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "添加成功";

            int EmployeeId = LoginInfo.UserInfo.EmployeeId;

            try
            {
                int cType = 0;
                if (!string.IsNullOrEmpty(contactType))
                {
                    cType = contactType.ToString().ToInt32();
                }
                //客户信息
                customer.CustomerID = Guid.NewGuid();
                customer.Status = (byte)SysStatus.Enable;
                customer.CreateDate = DateTime.Now;
                customer.IsFinish = 0;
                customer.EvalState = 0;
                customer.DeskCount = 0;
                customer.Budget = 0;
                customer.ReCommand = _saleSourceService.Find(customer.Channel).CommoandName;
                customer.CreateEmployee = EmployeeId;
                if (cType == 0)
                {
                    customer.ContactMan = customer.Groom;
                    customer.ContactPhone = customer.GroomPhone;
                }
                else if (cType == 1)
                {
                    customer.ContactMan = customer.Bride;
                    customer.ContactPhone = customer.BridePhone;
                }
                else if (cType == 2)
                {
                    customer.ContactMan = customer.Operator;
                    customer.ContactPhone = customer.OperatorPhone;
                }




                //Report 信息
                SS_Report report = new SS_Report();

                report.CustomerId = customer.CustomerID;
                report.CreateEmployee = EmployeeId;
                report.InviteEmployee = EmployeeId;
                report.CreateDate = DateTime.Now;
                report.CustomerState = customer.State;


                //邀约信息
                FL_Invite m_invite = new FL_Invite();
                m_invite.CustomerID = customer.CustomerID;
                m_invite.EmployeeId = EmployeeId;
                m_invite.CreateEmployee = EmployeeId;
                m_invite.CreateDate = DateTime.Now;
                m_invite.IsLose = false;

                FL_InviteDetails details = new FL_InviteDetails();
                FL_Order order = new FL_Order();
                if (customer.State == 5)        //确认到店
                {
                    //邀约内容
                    m_invite.OrderEmployee = orderEmp.ToString().ToInt32();

                    details.CustomerId = customer.CustomerID;
                    details.EmployeeId = EmployeeId;
                    details.InviteState = (int)CustomerState.ComeOrder;
                    details.StateName = details.StateValue.GetDisplayName();
                    details.InviteContent = "添加录入客户  直接确认到店";
                    details.CreateDate = DateTime.Now;

                    //订单 统筹信息
                    if (!string.IsNullOrEmpty(orderEmp))
                    {
                        order.OrderID = Guid.NewGuid();
                        order.OrderCoder = Guid.NewGuid().ToString().Replace("-", "");
                        order.CustomerId = customer.CustomerID;
                        order.ComeDate = DateTime.Now;
                        order.EmployeeId = EmployeeId;
                        order.OrderState = (int)CustomerState.ComeOrder;
                        order.FollowCount = 0;
                        order.CreateEmployee = EmployeeId;
                        order.CreateDate = DateTime.Now;

                        m_invite.FollowCount = 1;
                        m_invite.OrderEmployee = orderEmp.ToString().ToInt32();
                        m_invite.LastFollowDate = DateTime.Now;

                        report.OrderEmployee = orderEmp.ToString().ToInt32();
                    }

                }
                using (TransactionScope scope = new TransactionScope())
                {
                    //添加客户
                    _customerService.Add(customer);

                    //添加邀约
                    FL_Invite r_invite = _inviteService.Add(m_invite);

                    if (customer.State == 5)        //确认到店
                    {
                        details.InviteId = r_invite.InviteId;
                        //添加邀约内容
                        _inviteDetailsService.Add(details);


                        //添加到店  订单
                        FL_Order r_order = _orderService.Add(order);
                        report.OrderId = r_order.OrderID;
                    }
                    //添加统计
                    _reportService.Add(report);

                    scope.Complete();
                }

                ajax.IsSuccess = true;
                ajax.Message = "添加成功";
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }
            return ajax;
        }
        #endregion

        #region 获取所有客户信息
        /// <summary>
        /// 获取所有客户信息
        /// </summary>
        /// <param name="sortType">1.正序排序  2.倒序排序   (kendoUi) test文件夹 kendDoIndex.html页面</param>
        /// <returns></returns>
        [WebMethod]
        public void GetAllCustomer(int page, int pageSize, string name, string phone, string sort)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;

            int count = 0;
            var DataList = _cusService.GetAllPageCustomer(page, pageSize, name, phone, sort, out count);

            ajax.data = JsonConvert.SerializeObject(DataList);
            ajax.total = count;         //数据总条数
            ajax.PageIndex = page;      //当前页
            ajax.PageSize = pageSize;   //每页显示条数

            ajax.IsSuccess = true;
            Context.Response.Write(JsonConvert.SerializeObject(ajax));
        }

        #endregion

        #region 获取所有客户信息
        /// <summary>
        /// 获取所有客户信息
        /// </summary>
        /// <param name="sortType">1.正序排序  2.倒序排序</param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage GetAllCustomers(string sortName, int page, int pageSize, string name, string phone)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;

            int count = 0;
            var DataList = _cusService.GetAllPageCustomer(page, pageSize, name, phone, sortName, out count);

            ajax.data = DataList;
            ajax.total = count;         //数据总条数
            ajax.PageIndex = page;      //当前页
            ajax.PageSize = pageSize;   //每页显示条数

            ajax.IsSuccess = true;
            return ajax;
        }

        #endregion

        #region 修改客户信息
        /// <summary>
        /// 修改客户信息
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage ModifyCustomer(FL_Customer customer, string contactType, string customerId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "修改失败,请稍候再试...";

            int EmployeeId = LoginInfo.UserInfo.EmployeeId;
            Guid cusId = new Guid(customerId);

            try
            {
                var m_customer = _customerService.GetByGuid(cusId);
                int cType = 0;
                if (!string.IsNullOrEmpty(contactType))
                {
                    cType = contactType.ToString().ToInt32();
                }
                //客户信息
                m_customer.Bride = customer.Bride;
                m_customer.Groom = customer.Groom;
                m_customer.Operator = customer.Operator;
                m_customer.BridePhone = customer.BridePhone;
                m_customer.GroomPhone = customer.GroomPhone;
                m_customer.OperatorPhone = customer.OperatorPhone;
                m_customer.PartyDate = customer.PartyDate;
                m_customer.Hotel = customer.Hotel;
                m_customer.SaleType = customer.SaleType;
                m_customer.Channel = customer.Channel;
                m_customer.ReCommand = customer.ReCommand;
                m_customer.IsVip = customer.IsVip;
                m_customer.BanqueType = customer.BanqueType;
                m_customer.DeskCount = customer.DeskCount;
                m_customer.Budget = customer.Budget;
                m_customer.Description = customer.Description;
                if (cType == 0)
                {
                    m_customer.ContactMan = customer.Groom;
                    m_customer.ContactPhone = customer.GroomPhone;
                }
                else if (cType == 1)
                {
                    m_customer.ContactMan = customer.Bride;
                    m_customer.ContactPhone = customer.BridePhone;
                }
                else if (cType == 2)
                {
                    m_customer.ContactMan = customer.Operator;
                    m_customer.ContactPhone = customer.OperatorPhone;
                }

                using (TransactionScope scope = new TransactionScope())
                {
                    //修改客户信息
                    _customerService.Update(m_customer);
                    scope.Complete();
                }

                ajax.IsSuccess = true;
                ajax.Message = "修改成功";
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }
            return ajax;
        }
        #endregion

        #region 客户状态 禁用/启用
        /// <summary>
        /// 禁用/启用
        /// </summary>
        [WebMethod]
        public AjaxMessage ModifySingleStatus(string cusId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.PageSize = 10;
            if (!string.IsNullOrEmpty(cusId))
            {
                Guid Id = new Guid(cusId.ToString());

                var cusModel = _customerService.GetByGuid(Id);
                if (cusModel.Status == (byte)SysStatus.Enable)
                {
                    cusModel.Status = (byte)SysStatus.Disable;
                    ajax.Message = "禁用成功";
                }
                else
                {
                    cusModel.Status = (byte)SysStatus.Enable;
                    ajax.Message = "启用成功";
                }
                int result = _customerService.Update(cusModel);
                if (result > 0)
                {
                    ajax.IsSuccess = true;
                }
                else
                {
                    ajax.Message = "信息异常,请稍后再试...";
                }
            }
            return ajax;
        }
        #endregion


        #region 获取客户信息
        /// <summary>
        /// @author:wp
        /// @datetime:2017-03-01
        /// @desc:根据客户id获取客户信息
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage GetByCustomerID(string customerId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            try
            {
                if (!string.IsNullOrEmpty(customerId))
                {
                    Guid id = new Guid(customerId);
                    var m_customer = _cusService.GetByGuid(id);

                    if (m_customer != null)
                    {
                        ajax.Data = m_customer;
                        ajax.IsSuccess = true;
                    }
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;

            }
            return ajax;
        }

        #endregion


        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage ImportFileUpload()
        {
            var files = HttpContext.Current.Request.Files;

            HttpFileCollection file = HttpContext.Current.Request.Files;
            HttpPostedFile imgFile = Context.Request.Files["upFile"];


            int EmployeeId = LoginInfo.UserInfo.EmployeeId;

            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;


            return ajax;
        }
    }
}
