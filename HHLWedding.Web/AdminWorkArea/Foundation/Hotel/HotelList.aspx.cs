using HHLWedding.BLLAssmbly.FD;
using HHLWedding.DataAssmblly;
using HHLWedding.Pages;
using HHLWedding.ToolsLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HHLWedding.Web.AdminWorkArea.Foundation.Hotel
{
    public partial class HotelList : SystemPage
    {

        HotelService _hotelService = new HotelService();

        string orderByName = "CreateDate";

        #region 页面初始化
        /// <summary>
        /// 页面加载
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CtrPageIndex.CurrentPageIndex = 2;
                BinderData(true);
            }
        }
        #endregion

        #region 数据绑定
        /// <summary>
        /// @author:wp
        /// @datetime:2016-08-16
        /// @desc:数据绑定
        /// </summary>
        public void BinderData(bool First = false)
        {

            int sourceCount = 0;
            List<PMSParameters> pars = new List<PMSParameters>();
            pars.Add(!string.IsNullOrEmpty(txtHotelName.Text.Trim().ToString()), "HotelName", txtHotelName.Text.Trim().ToString(), NSqlTypes.LIKE);
            
            var DataList = _hotelService.GetAllByPager(pars, orderByName, CtrPageIndex.CurrentPageIndex, CtrPageIndex.PageSize, out sourceCount);

            CtrPageIndex.RecordCount = sourceCount;

            if (First == true)
            {
                if (Request["page"] != null)
                {
                    int page = Request.QueryString["page"].ToString().ToInt32();
                    CtrPageIndex.CurrentPageIndex = page;
                    DataList = _hotelService.GetAllByPager(pars, orderByName, CtrPageIndex.CurrentPageIndex, CtrPageIndex.PageSize, out sourceCount);
                }
            }

            SavePage(CtrPageIndex);

            repHotel.DataSource = DataList;
            repHotel.DataBind();
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