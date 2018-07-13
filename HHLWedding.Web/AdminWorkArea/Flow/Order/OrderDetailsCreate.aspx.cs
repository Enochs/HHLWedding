using HHLWedding.BLLAssmbly;
using HHLWedding.BLLAssmbly.Flow;
using HHLWedding.BLLAssmbly.Set;
using HHLWedding.DataAssmblly;
using HHLWedding.Pages;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HHLWedding.ToolsLibrary;

namespace HHLWedding.Web.AdminWorkArea.Flow.Order
{
    public partial class OrderDetailsCreate : SystemPage
    {
        #region 构建服务
        CustomerService _customerService = new CustomerService();
        ReportService _reportService = new ReportService();
        InviteService _inviteService = new InviteService();
        OrderService _orderService = new OrderService();
        BaseService<FD_SaleType> _saleTypeService = new BaseService<FD_SaleType>();
        BaseService<FD_SaleSource> _saleSourceService = new BaseService<FD_SaleSource>();
        #endregion


        #region 页面加载
        /// <summary>
        /// 页面加载
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BinderData();
            }
        }
        #endregion


        #region 数据绑定
        /// <summary>
        /// 数据绑定
        /// </summary>
        public void BinderData()
        {
            if (Request["CustomerId"] != null)
            {
                Guid customerId = new Guid(Request["CustomerId"].ToString());
                FL_Customer m_customer = _customerService.GetById(customerId);
                if (m_customer != null)
                {
                    //主要联系人
                    if (m_customer.ContactMan == m_customer.Groom)
                    {
                        rdoContactMan.SelectedValue = "0";
                    }
                    else if (m_customer.ContactMan == m_customer.Bride)
                    {
                        rdoContactMan.SelectedValue = "1";
                    }
                    else
                    {
                        rdoContactMan.SelectedValue = "2";
                    }

                    txtBride.Value = m_customer.Bride;
                    txtGroom.Value = m_customer.Groom;
                    txtOperator.Value = m_customer.Operator;
                    txtBridePhone.Value = m_customer.BridePhone;
                    txtGroomPhone.Value = m_customer.GroomPhone;
                    txtOperatorPhone.Value = m_customer.OperatorPhone;
                    ddlHotel.SelectedValue = m_customer.Hotel.ToString();
                    txtPartyDate.Value = m_customer.PartyDate.ToString().ToDateTime().ToString("yyyy-MM-dd");
                    txtDeskCount.Value = m_customer.DeskCount.ToString();
                    txtBudget.Value = m_customer.Budget.ToString();
                    rdoVip.SelectedValue = m_customer.IsVip.ToString();
                    rdoBanqueType.SelectedValue = m_customer.BanqueType.ToString();
                    txtState.Value = GetCustomerState(m_customer.State);
                    switch (m_customer.IsFinish)
                    {
                        case 0:
                            txtIsFinish.Value = "未完成";
                            break;
                        case 1:
                            txtIsFinish.Value = "已完成";
                            break;
                        case 2:
                            txtIsFinish.Value = "已完结";
                            break;
                    }
                    if (m_customer.EvalState == 0)
                    {
                        txtEvalState.Value = "未评价(" + txtIsFinish.Value + ")";
                    }
                    else
                    {
                        txtEvalState.Value = "已评价(" + txtIsFinish.Value + ")";
                    }
                }

                //渠道类型
                FD_SaleType m_saleType = _saleTypeService.GetById(m_customer.SaleType);
                ddlSaleType.SelectedValue = m_customer.SaleType.ToString();


                //渠道名称条件
                List<Expression<Func<FD_SaleSource, bool>>> parmList = new List<Expression<Func<FD_SaleSource, bool>>>();
                parmList.Add(c => c.SaleTypeId == m_saleType.SaleTypeID);
                ddlSaleSource.DataSource = _saleSourceService.GetListBy(parmList);
                ddlSaleSource.DataBind();

                //渠道名称
                FD_SaleSource m_saleSource = _saleSourceService.GetById(m_customer.Channel); ;
                ddlSaleSource.SelectedValue = m_customer.Channel.ToString();
                txtReCommand.Value = m_saleSource.CommoandName;

                txtCreateEmployee.Value = GetEmployeeNames(m_customer.CreateEmployee);
                txtCreateDate.Value = m_customer.CreateDate.ToString();


                SS_Report m_report = _reportService.GetByCustomerId(customerId);
                txtInviteEmployee.Value = GetEmployeeNames(m_report.InviteEmployee);
                txtOrderEmployees.Value = GetEmployeeNames(m_report.OrderEmployee);
                txtQuotedEmployee.Value = GetEmployeeNames(m_report.QuotedEmployee);

                txtDescription.Text = m_customer.Description;


                //邀约沟通记录
                var dataList = _inviteService.GetDataListByCustomerId(customerId);

                if (dataList.Count > 0)         //有沟通记录
                {
                    tr_noInvite.Visible = false;
                    repInviteContent.DataSource = dataList;
                    repInviteContent.DataBind();
                }
                else
                {
                    tr_noInvite.Visible = true;
                }


                //跟单沟通记录
                var orderList = _orderService.GetDataListByCustomerId(customerId);

                if (orderList.Count > 0)         //有沟通记录
                {
                    tr_noOrder.Visible = false;
                    repOrderContent.DataSource = orderList;
                    repOrderContent.DataBind();
                }
                else
                {
                    tr_noOrder.Visible = true;
                }
            }



        }


        #endregion
    }
}