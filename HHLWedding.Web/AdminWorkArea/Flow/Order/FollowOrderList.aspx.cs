using HHLWedding.BLLAssmbly.Flow;
using HHLWedding.BLLAssmbly.Set;
using HHLWedding.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HHLWedding.Web.AdminWorkArea.Flow.Order
{
    public partial class FollowOrderList : SystemPage
    {
        #region 服务

        OrderService _orderService = new OrderService();

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
                ddlEmployee.DataBinder("请选择跟单人");
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
            string orderEmployee = ddlEmployee.SelectedValue;
            var DataList = _orderService.GetAllByPage(CtrPageIndex.CurrentPageIndex, CtrPageIndex.PageSize, ref sourceCount, name, phone, orderEmployee);
            repOrder.DataSource = DataList;
            repOrder.DataBind();

            SavePage(CtrPageIndex);
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