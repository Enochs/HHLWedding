using HHLWedding.BLLAssmbly.FD;
using HHLWedding.Pages;
using HHLWedding.ToolsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HHLWedding.Web.AdminWorkArea.Foundation.Hotel
{
    public partial class HotelLabelList : SystemPage
    {
        /// <summary>
        /// 酒店标签
        /// </summary>
        HotelLabelService _labelService = new HotelLabelService();

        ///排序字段
        string orderByName = "CreateDate";

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
        /// @author:wp
        /// @datetime:2016-08-16
        /// @desc:数据绑定
        /// </summary>
        public void BinderData()
        {
            int sourceCount = 0;
            List<PMSParameters> pars = new List<PMSParameters>();
            pars.Add(!string.IsNullOrEmpty(txtLabelName.Text.Trim().ToString()), "LabelName", txtLabelName.Text.Trim().ToString(), NSqlTypes.LIKE);

            var DataList = _labelService.GetAllByPager(pars, orderByName, CtrPageIndex.CurrentPageIndex, CtrPageIndex.PageSize, out sourceCount);
            CtrPageIndex.RecordCount = sourceCount;

            SavePage(CtrPageIndex);

            repLabel.DataSource = DataList;
            repLabel.DataBind();
        }
        #endregion

        #region 搜索按钮
        /// <summary>
        /// @author:wp
        /// @datetime:2016-08-16
        /// @desc:搜索按钮
        /// </summary>
        protected void _btnSearch_ServerClick(object sender, EventArgs e)
        {
            CtrPageIndex.CurrentPageIndex = 1;
            BinderData();
        }
        #endregion

        #region 分页
        /// <summary>
        /// @author:wp
        /// @datetime:2016-08-16
        /// @desc :分页查询(点击分页页码)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CtrPageIndex_PageChanged(object sender, EventArgs e)
        {
            BinderData();
        }
        #endregion

        #region 异步刷新
        /// <summary>
        /// @author:wp
        /// @datetime:2016-08-16
        /// @desc: 异步刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            BinderData();
        }
        #endregion
    }
}