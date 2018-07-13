using HHLWedding.BLLAssmbly;
using HHLWedding.BLLAssmbly.FD;
using HHLWedding.DataAssmblly;
using HHLWedding.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HHLWedding.Web.AdminWorkArea.SaleSource
{
    public partial class SaleSourceCreate : SystemPage
    {
        #region 构建服务

        /// <summary>
        /// 渠道类型
        /// </summary>
        //BaseService<FD_SaleType> _saleTypeService = new BaseService<FD_SaleType>();
        SaleSourceService _salesourceService = new SaleSourceService();
        #endregion


        #region 页面加载
        /// <summary>
        /// 页面初始化
        /// </summary> 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DDLDataBind();
            }
        }
        #endregion


        #region 下拉框绑定
        /// <summary>
        /// 下拉框绑定
        /// </summary>

        public void DDLDataBind()
        {
            List<Expression<Func<FD_SaleType, bool>>> parmList = new List<Expression<Func<FD_SaleType, bool>>>();

            //启用状态
            parmList.Add(c => c.Status == 1);

            List<ListItem> list = _salesourceService.GetSaleTypeDDL();
            ddlSaleType.DataSource = list;
            ddlSaleType.DataBind();
        }
        #endregion
    }
}