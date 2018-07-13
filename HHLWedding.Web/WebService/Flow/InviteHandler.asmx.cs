using HHLWedding.BLLAssmbly.Flow;
using HHLWedding.Common;
using HHLWedding.DataAssmblly;
using HHLWedding.DataAssmblly.Dto;
using HHLWedding.EditoerLibrary.Extension;
using HHLWedding.Web.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using HHLWedding.ToolsLibrary;
using System.Web.Services;
using HHLWedding.DataAssmblly.CommonModel;
using HHLWedding.EditoerLibrary;
using HHLWedding.BLLAssmbly;
using HHLWedding.BLLAssmbly.Set;
using System.Transactions;
using Newtonsoft.Json;
using HHLWedding.Control;

namespace HHLWedding.Web.WebService.Flow
{
    /// <summary>
    /// InviteHandler 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]



    public class InviteHandler : System.Web.Services.WebService
    {
        #region 参数

        //默认婚期开始时间和结束时间(如果有选择婚期)
        DateTime start = DateTime.Now.AddYears(-50);
        DateTime end = DateTime.Now.AddYears(50);

        #endregion

        #region 数据服务

        InviteService _inviteService = new InviteService();
        BaseService<FL_InviteDetails> _invteiContentService = new BaseService<FL_InviteDetails>();
        OrderService _orderService = new OrderService();
        ReportService _reportService = new ReportService();
        CustomerService _customerService = new CustomerService();

        #endregion

        #region 获取邀约客户
        /// <summary>
        /// 获取邀约客户 (邀约状态: 1.未邀约 2.邀约中 3.邀约成功)
        /// </summary>
        /// <param name="customerState"> 1.未邀约 2.邀约中 3.邀约成功</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage GetInviteCustomer(int page, int pageSize, string sortName, PropModel[] searchs, string inviteEmployee)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "查询失败,系统异常";
            try
            {
                int count = 0;
                List<Expression<Func<InviteDto, bool>>> parmList = new List<Expression<Func<InviteDto, bool>>>();

                #region 条件
                //默认显示当前登录人以及下级的邀约的信息
                if (!string.IsNullOrEmpty(inviteEmployee))
                {
                    string employeeId = inviteEmployee.ToInt32() > 0 ? inviteEmployee : LoginInfo.UserInfo.EmployeeId.ToString();
                    //选择只显示选择人个人的邀约信息
                    bool isLookLowLevel = inviteEmployee.ToInt32() > 0 ? false : true;
                    ExpressionTools.GetParsByCondition<InviteDto>("InviteEmployee", parmList, employeeId, "Contains", true, isLookLowLevel);
                }
                else
                {
                    //默认获取本人及下级邀约信息
                    MyEmployee sel_emp = new MyEmployee();
                    sel_emp.GetMyEmployee<InviteDto>("InviteEmployee", parmList, true);
                }

                if (searchs.Count() > 0)
                {
                    foreach (PropModel item in searchs)
                    {
                        if (item.value != "-1")          //有明确的某个状态
                        {
                            if (!string.IsNullOrEmpty(item.value))
                            {
                                if (!(item.value.Contains(',')))
                                {
                                    ExpressionTools.GetEqualPars(item.property, parmList, item.value, item.method);
                                }
                                else if (item.value.Contains(','))      //时间 婚期
                                {
                                    TimeHelper.GetPartyDateTime(item.value.ToString(), out start, out end);
                                    parmList.Add(c => c.PartyDate >= start && c.PartyDate <= end);
                                }
                            }
                        }
                        else            //邀约/未邀约/邀约成功/确认到店
                        {
                            parmList.Add(c => ("1,2,3,5").Contains(c.State.ToString()));
                        }
                    }
                }


                #endregion

                var DataList = _inviteService.GetAllByPage(page, pageSize, parmList, sortName, out count);
                ajax.data = DataList;
                ajax.total = count;         //数据总条数
                ajax.PageIndex = page;      //当前页
                ajax.PageSize = pageSize;   //每页显示条数
                ajax.EmpId = LoginInfo.UserInfo.EmployeeId.ToString();      //登录员工

                ajax.IsSuccess = true;
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;

        }
        #endregion

        #region 添加邀约详情信息
        /// <summary>
        /// @author:wp
        /// @datetime:2017-01-12
        /// @desc:添加邀约详情信息
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage CreateInviteContent(FL_InviteDetails inviteDetail)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "数据异常,请稍候再试...";

            try
            {
                if (inviteDetail != null)
                {
                    int selfEmployeeId = LoginInfo.UserInfo.EmployeeId;
                    inviteDetail.CreateDate = DateTime.Now;
                    inviteDetail.EmployeeId = selfEmployeeId;
                    inviteDetail.StateName = inviteDetail.StateValue.GetDisplayName();

                    //客户表  修改相应的状态
                    FL_Customer m_customer = _customerService.GetByGuid(new Guid(inviteDetail.CustomerId.ToString()));
                    m_customer.State = inviteDetail.InviteState;

                    //邀约表
                    FL_Invite m_invite = _inviteService.GetById(inviteDetail.InviteId);
                    m_invite.FollowCount += 1;
                    m_invite.LastFollowDate = inviteDetail.CreateDate;
                    m_invite.NextFollowDate = inviteDetail.NextFollowDate;

                    //统计表(客户)
                    SS_Report report = _reportService.GetByCustomerId(new Guid(inviteDetail.CustomerId.ToString()));


                    if (inviteDetail.InviteState == 4)     //流失
                    {
                        m_invite.IsLose = true;
                        m_invite.LoseDate = DateTime.Now;

                        report.InviteLoseDate = m_invite.LoseDate;
                    }

                    using (TransactionScope scope = new TransactionScope())
                    {
                        //客户表
                        _customerService.Update(m_customer);

                        //邀约详细
                        _invteiContentService.Add(inviteDetail);

                        //邀约表
                        _inviteService.Update(m_invite);

                        //统计
                        _reportService.Update(report);

                        scope.Complete();
                    }
                    ajax.IsSuccess = true;
                    ajax.Message = "记录成功";
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;
        }
        #endregion

        #region 恢复邀约
        /// <summary>
        /// 恢复邀约
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage RevocerInvite(string CustomerId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "信息异常,请稍候再试...";

            try
            {
                if (!string.IsNullOrEmpty(CustomerId))
                {
                    var m_customer = _customerService.GetByGuid(new Guid(CustomerId));
                    m_customer.State = 2;       //邀约中

                    using (TransactionScope scope = new TransactionScope())
                    {
                        //修改客户状态信息
                        _customerService.Update(m_customer);
                        ajax.IsSuccess = true;
                        ajax.Message = "恢复邀约成功";
                        scope.Complete();
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

        #region 根据邀约Id获取邀约沟通详情 (获取流失原因)
        /// <summary>
        /// @author:wp
        /// @datetime:2017-01-22
        /// @method:获取流失原因
        /// </summary>
        /// <param name="inviteId">邀约Id</param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage GetDetailsById(string inviteId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "信息异常,请稍候再试...";

            try
            {
                if (!string.IsNullOrEmpty(inviteId))
                {
                    var m_invite = _inviteService.GetDataListById(inviteId.ToInt32()).FirstOrDefault();
                    if (m_invite != null)
                    {
                        ajax.IsSuccess = true;
                        ajax.Message = m_invite.InviteContent;
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

        #region 确认到店
        /// <summary>
        /// 确认到店
        /// </summary>
        [WebMethod(EnableSession = true)]
        public AjaxMessage ComeToStore(FL_Customer m_customer, string orderEmp, string InviteId, string comeDate)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "异常信息,请稍后再试...";
            try
            {
                if (m_customer != null && !string.IsNullOrEmpty(InviteId))
                {
                    int selfEmployeeId = LoginInfo.UserInfo.EmployeeId;
                    FL_InviteDetails inviteDetail = new FL_InviteDetails();
                    inviteDetail.InviteId = InviteId.ToInt32();
                    inviteDetail.CustomerId = m_customer.CustomerID;
                    inviteDetail.InviteContent = "通过邀约 确认到店";
                    inviteDetail.CreateDate = DateTime.Now;
                    inviteDetail.EmployeeId = selfEmployeeId;
                    inviteDetail.InviteState = 5;
                    inviteDetail.StateName = inviteDetail.StateValue.GetDisplayName();


                    //新人信息
                    var customer = _customerService.GetByGuid(m_customer.CustomerID);
                    customer.Bride = m_customer.Bride;
                    customer.BridePhone = m_customer.BridePhone;
                    customer.Groom = m_customer.Groom;
                    customer.GroomPhone = m_customer.GroomPhone;
                    customer.Budget = m_customer.Budget;
                    customer.DeskCount = m_customer.DeskCount;
                    customer.State = 5;


                    //邀约表  修改跟单人
                    FL_Invite m_invite = _inviteService.GetById(InviteId.ToInt32());
                    m_invite.FollowCount += 1;
                    m_invite.LastFollowDate = inviteDetail.CreateDate;
                    m_invite.NextFollowDate = inviteDetail.NextFollowDate;
                    m_invite.OrderEmployee = orderEmp.ToInt32();
                    m_invite.ComeDate = !string.IsNullOrEmpty(comeDate) ? comeDate.ToDateTime() : DateTime.Now;

                    //统计表(客户) 修改跟单人
                    SS_Report report = _reportService.GetByCustomerId(new Guid(m_customer.CustomerID.ToString()));
                    report.CustomerState = m_customer.State;
                    report.OrderEmployee = orderEmp.ToInt32();
                    report.ComeDate = m_invite.ComeDate;

                    //订单表
                    FL_Order m_order = new FL_Order();

                    m_order.OrderID = Guid.NewGuid();
                    m_order.OrderCoder = Guid.NewGuid().ToString().Replace("-", "");
                    m_order.CustomerId = m_customer.CustomerID;
                    m_order.ComeDate = m_invite.ComeDate;
                    m_order.EmployeeId = orderEmp.ToInt32();
                    m_order.OrderState = (int)CustomerState.ComeOrder;
                    m_order.FollowCount = 0;
                    m_order.CreateEmployee = selfEmployeeId;
                    m_order.CreateDate = DateTime.Now;




                    using (TransactionScope scope = new TransactionScope())
                    {
                        //客户表
                        _customerService.Update(customer);

                        //邀约详细
                        _invteiContentService.Add(inviteDetail);

                        //邀约表
                        _inviteService.Update(m_invite);

                        //订单表
                        _orderService.Add(m_order);

                        //统计
                        _reportService.Update(report);

                        scope.Complete();
                    }
                    ajax.IsSuccess = true;
                    ajax.Message = "确认到店成功";
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;
        }
        #endregion


        #region 改派邀约人
        /// <summary>
        /// @datetime:2017-002-23
        /// @author:wp
        /// @description:客户改派邀约人
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage ReInviteEmployee(string employeeId, string customerId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;

            try
            {
                if (!string.IsNullOrEmpty(employeeId))
                {
                    int empId = employeeId.ToInt32();

                    if (customerId.Contains(","))     //批量改派
                    {
                        string[] customer = customerId.Split(',');
                        List<string> cus_list = new List<string>();
                        for (int i = 0; i < customer.Length; i++)
                        {
                            cus_list.Add(customer[i]);
                        }

                        #region 邀约表

                        //修改实体类(邀约表)
                        FL_Invite m_invite = new FL_Invite();
                        m_invite.EmployeeId = empId;

                        //条件
                        Expression<Func<FL_Invite, bool>> where = c => cus_list.Contains(c.CustomerID.ToString());

                        //修改字段
                        string prop = "EmployeeId";
                        string[] property = new string[] { prop }; ;

                        #endregion

                        #region 统计表
                        //修改实体类(统计表)
                        SS_Report m_report = new SS_Report();
                        m_report.InviteEmployee = m_invite.EmployeeId;

                        //条件
                        Expression<Func<SS_Report, bool>> wheres = c => cus_list.Contains(c.CustomerId.ToString());

                        //修改字段
                        string props = "InviteEmployee";
                        string[] propertys = new string[] { props };
                        #endregion

                        using (TransactionScope scope = new TransactionScope())
                        {
                            _inviteService.ModifyBy(m_invite, where, property);
                            _reportService.ModifyBy(m_report, wheres, propertys);
                            scope.Complete();
                        }

                        ajax.IsSuccess = true;
                        ajax.Message = "改派成功";
                    }
                    else
                    {
                        FL_Invite m_invite = _inviteService.GetByCustomerId(new Guid(customerId));
                        m_invite.EmployeeId = empId;

                        SS_Report m_report = _reportService.GetByCustomerId(new Guid(customerId));
                        m_report.InviteEmployee = empId;

                        using (TransactionScope scope = new TransactionScope())
                        {
                            _inviteService.Update(m_invite);
                            _reportService.Update(m_report);
                            scope.Complete();
                        }

                        ajax.IsSuccess = true;
                        ajax.Message = "改派成功";
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
    }
}
