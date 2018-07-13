
using HHLWedding.BLLAssmbly.Flow;
using HHLWedding.BLLAssmbly.Set;
using HHLWedding.Common;
using HHLWedding.Control;
using HHLWedding.DataAssmblly;
using HHLWedding.DataAssmblly.CommonModel;
using HHLWedding.DataAssmblly.Dto;
using HHLWedding.Web.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Services;
using HHLWedding.ToolsLibrary;
using HHLWedding.EditoerLibrary;
using HHLWedding.BLLAssmbly;
using System.Data.SqlClient;
using System.Transactions;

namespace HHLWedding.Web.WebService.Flow
{
    /// <summary>
    /// OrderHandler 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class OrderHandler : System.Web.Services.WebService
    {

        #region 参数

        //默认婚期开始时间和结束时间(如果有选择婚期)
        DateTime start = DateTime.Now.AddYears(-50);
        DateTime end = DateTime.Now.AddYears(50);

        #endregion

        #region 服务

        OrderService _orderService = new OrderService();
        BaseService<FL_OrderDetails> _detailsService = new BaseService<FL_OrderDetails>();

        ReportService _reportService = new ReportService();

        CustomerService _customerService = new CustomerService();
        #endregion


        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        #region 记录客户沟通内容
        /// <summary>
        /// @author:wp
        /// @datetime:2017-02-28
        /// @description:记录客户沟通内容
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage OrderCotentCreate(FL_OrderDetails orderDetail)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "异常错误,请稍后再试...";

            try
            {               //暂不考虑成功预定

                int employeeId = LoginInfo.UserInfo.EmployeeId;
                Guid CustomerId = new Guid(orderDetail.CustomerId.ToString());

                //客户详情
                FL_Customer m_customer = _customerService.GetByGuid(CustomerId);
                m_customer.State = orderDetail.OrderState;

                //订单信息
                FL_Order m_order = _orderService.GetByGuid(new Guid(orderDetail.OrderId.ToString()));
                m_order.OrderState = m_customer.State;
                m_order.FollowCount++;
                if (orderDetail.OrderState != 9)
                {
                    m_order.NextFollowDate = orderDetail.NextFollowDate;
                }
                m_order.LastFollowDate = DateTime.Now;



                //订单详情
                orderDetail.EmployeeId = employeeId;
                orderDetail.CreateDate = DateTime.Now;



                //统计信息
                SS_Report m_report = _reportService.GetByCustomerId(CustomerId);
                m_report.CustomerState = m_customer.State;


                //事务
                using (TransactionScope scope = new TransactionScope())
                {
                    //修改客户表
                    _customerService.Update(m_customer);

                    //订单详情
                    _detailsService.Add(orderDetail);

                    //修改订单表
                    _orderService.Update(m_order);

                    //修改统计表
                    _reportService.Update(m_report);

                    scope.Complete();
                }

                ajax.IsSuccess = true;
                ajax.Message = "记录成功";


            }
            catch (Exception e)
            {
                ajax.Message = e.Message.ToString();
            }

            return ajax;
        }
        #endregion

        #region 根据条件获取客户信息集合
        /// <summary>
        /// @author:wp
        /// @datetime:2017-03-02
        /// @desc:根据条件获取客户信息
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage GetOrderCustomer(int page, int pagesize, string sortName, PropModel[] searchs, string orderEmployee)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            try
            {
                List<Expression<Func<OrderDto, bool>>> parmList = new List<Expression<Func<OrderDto, bool>>>();

                if (!string.IsNullOrEmpty(orderEmployee))
                {
                    parmList.Add(c => c.EmployeeId.ToString() == orderEmployee);

                    string employeeId = orderEmployee.ToInt32() > 0 ? orderEmployee : LoginInfo.UserInfo.EmployeeId.ToString();
                    //选择只显示选择人个人的邀约信息
                    bool isLookLowLevel = orderEmployee.ToInt32() > 0 ? false : true;
                    ExpressionTools.GetParsByCondition<OrderDto>("EmployeeId", parmList, employeeId, "Contains", true, isLookLowLevel);
                }
                else
                {
                    //默认获取本人及下级邀约信息
                    MyEmployee sel_emp = new MyEmployee();
                    sel_emp.GetMyEmployee<OrderDto>("EmployeeId", parmList, true);
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
                        else            //二选一/多选一/确认到店
                        {
                            parmList.Add(c => ("5,6,7,8").Contains(c.OrderState.ToString()));
                        }
                    }
                }
                int count = 0;

                var dataList = _orderService.GetAllByPages(page, pagesize, parmList, sortName, out count);
                if (dataList.Count > 0)
                {
                    ajax.IsSuccess = true;

                    ajax.data = dataList;
                    ajax.total = count;         //数据总条数
                    ajax.PageIndex = page;      //当前页
                    ajax.PageSize = pagesize;   //每页显示条数
                    ajax.EmpId = LoginInfo.UserInfo.EmployeeId.ToString();      //登录员工
                }



            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;
        }
        #endregion

        #region 改派跟单人
        /// <summary>
        /// @author:wp
        /// @datetime:2017-03-03
        /// @desc:改派跟单人
        /// </summary>
        /// <param name="employeeId">改派的跟单人Id</param>
        /// <param name="customerId">改派的客户Id</param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage ReOrderEmployee(string employeeId, string customerId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;

            try
            {
                if (!string.IsNullOrEmpty(customerId) && !string.IsNullOrEmpty(employeeId))
                {
                    //修改 FL_Invite  SS_Report  FL_Order
                    int rowAffected = 0;
                    string cus_id = "";
                    string[] customer = customerId.Split(',');
                    for (int i = 0; i < customer.Length; i++)
                    {
                        cus_id += "'" + customer[i] + "',";
                    }
                    cus_id = cus_id.Substring(0, cus_id.Length - 1);

                    #region 执行存储过程

                    DBHelper db = new DBHelper();
                    SqlParameter[] pars = new SqlParameter[] { new SqlParameter("@employeeId", employeeId), new SqlParameter("@customerId", cus_id) };
                    int result = db.RunProcedure("proc_ReOrder", pars, out rowAffected);

                    if (result > 0)
                    {
                        ajax.IsSuccess = true;
                        ajax.Message = "改派成功";
                    }
                    #endregion
                }
                else
                {
                    if (!string.IsNullOrEmpty(customerId))
                    {
                        ajax.Message = "请选择客户";
                    }
                    else
                    {
                        ajax.Message = "请选择跟单人";
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


        #region 恢复邀约
        /// <summary>
        /// @author:wp
        /// @datetime:2017-03-06
        /// @desc:恢复邀约
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage ReConfirmOrder(string CustomerId, string OrderId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "系统异常,请稍后再试...";

            try
            {
                if (!string.IsNullOrEmpty(CustomerId))
                {
                    Guid cusId = new Guid(CustomerId);
                    Guid orderId = new Guid(OrderId);
                    SS_Report report = _reportService.GetByCustomerId(cusId);
                    report.CustomerState = 7;

                    FL_Customer m_customer = _customerService.GetByGuid(cusId);
                    m_customer.State = 7;

                    FL_Order m_order = _orderService.GetByGuid(orderId);
                    m_order.OrderState = 7;

                    using (TransactionScope scope = new TransactionScope())
                    {
                        _reportService.Update(report);

                        _customerService.Update(m_customer);

                        _orderService.Update(m_order);

                        scope.Complete();
                    }

                    ajax.IsSuccess = true;
                    ajax.Message = "恢复跟单成功,请在客户跟单中查看";
                }

            }
            catch (Exception e)
            {
                ajax.Message = e.Message.ToString();
            }

            return ajax;
        }
        #endregion


        #region 获取流失原因 
        /// <summary>
        /// @author:wp
        /// @datetime:2017-03-07
        /// @desc:获取流失(跟单)内容
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage GetLoseContent(string CustomerId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "系统异常,请稍候再试...";
            try
            {
                if (!string.IsNullOrEmpty(CustomerId))
                {
                    FL_OrderDetails m_details = _orderService.GetLoseOrder(new Guid(CustomerId));

                    ajax.IsSuccess = true;
                    ajax.Message = m_details.OrderContent;
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message.ToString();
            }

            return ajax;
        }
        #endregion
    }
}

