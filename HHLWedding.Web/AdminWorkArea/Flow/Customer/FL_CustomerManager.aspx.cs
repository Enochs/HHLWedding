using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HHLWedding.ToolsLibrary;
using HHLWedding.Pages;
using HHLWedding.BLLAssmbly.Flow;
using HHLWedding.BLLAssmbly.Set;

namespace HHLWedding.Web.AdminWorkArea.Flow.Customer
{
    public partial class FL_CustomerManager : SystemPage
    {
        #region 服务

        CustomerService _customerService = new CustomerService();

        NormalService _normalService = new NormalService();

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
        /// 绑定数据
        /// </summary>
        public void BinderData()
        {
            int sourceCount = 0;
            string name = Request["txtName"];
            string phone = Request["txtPhone"];
            int state = ddlState.SelectedValue.ToInt32();
            int hotelId = ddlHotel.SelectedValue.ToInt32();
            var DataList = _customerService.GetCustomerListByPager(CtrPageIndex.CurrentPageIndex, CtrPageIndex.PageSize, ref sourceCount, CtrPageIndex, name, phone, state, hotelId);
            repCustomer.DataSource = DataList;
            repCustomer.DataBind();

            JsReload();
        }
        #endregion

        #region 回传  异步刷新
        /// <summary>
        /// 异步刷新页面
        /// </summary>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            BinderData();
        }

        #endregion
    }
}