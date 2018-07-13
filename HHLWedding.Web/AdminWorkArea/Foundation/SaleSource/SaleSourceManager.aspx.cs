using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly;
using HHLWedding.EditoerLibrary;
using HHLWedding.Pages;
using HHLWedding.ToolsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HHLWedding.Web.AdminWorkArea.SaleSource
{
    public partial class SaleSourceManager : SystemPage
    {
        #region 构建服务

        BaseService<FD_SaleSource> _saleSourceService = new BaseService<FD_SaleSource>();
        BaseService<FD_SaleType> _saleTypeService = new BaseService<FD_SaleType>();

        #endregion


        #region 页面加载
        /// <summary>
        /// 页面初始化
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
            int sourceCount = 0;
            List<Expression<Func<FD_SaleSource, bool>>> parmList = new List<Expression<Func<FD_SaleSource, bool>>>();
            Expression<Func<FD_SaleSource, DateTime?>> Order = c => c.CreateDate;

            if (!string.IsNullOrEmpty(Request["txtSourceName"]))
            {
                string name = Request["txtSourceName"].ToString();
                parmList.Add(c => c.SourceName.Contains(name));
            }

            if (ddlSaleType.SelectedValue.ToInt32() > 0)
            {
                parmList.Add(c => c.SaleTypeId.ToString() == ddlSaleType.SelectedValue);
            }

            var DataList = _saleSourceService.GetPagedList<DateTime?>(CtrPageIndex.CurrentPageIndex, CtrPageIndex.PageSize, ref sourceCount, parmList, Order, false);
            CtrPageIndex.RecordCount = sourceCount;
            foreach (var item in DataList)
            {
                item.letter = PinYin.GetFirstLetter(item.SourceName);
                _saleSourceService.Update(item);
            }
            SavePage(CtrPageIndex);
            repSaleSource.DataSource = DataList;
            repSaleSource.DataBind();
        }
        #endregion

        #region 点击搜索
        /// <summary>
        /// 搜索
        /// </summary>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CtrPageIndex.CurrentPageIndex = 1;
            BinderData();
        }
        #endregion

        #region 分页
        /// <summary>
        /// 分页查询
        /// </summary>
        protected void CtrPageIndex_PageChanged(object sender, EventArgs e)
        {
            BinderData();
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