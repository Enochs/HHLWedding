using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly;
using HHLWedding.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HHLWedding.ToolsLibrary;
using HHLWedding.EditoerLibrary;

namespace HHLWedding.Web.AdminWorkArea.SaleSource
{
    public partial class SaleTypeManager : SystemPage
    {
        #region 数据服务

        BaseService<FD_SaleType> _baseService = new BaseService<FD_SaleType>();

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
        public void BinderData(bool First = false)
        {
            Expression<Func<FD_SaleType, DateTime?>> order = c => c.CreateDate;
            //条件
            List<Expression<Func<FD_SaleType, bool>>> parmList = new List<Expression<Func<FD_SaleType, bool>>>();

            //parmList.Add(c => c.SaleTypeID >= 3);

            var DataList = _baseService.GetListBy<DateTime?>(parmList, order, true);
            repSaleType.DataSource = DataList;
            repSaleType.DataBind();
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