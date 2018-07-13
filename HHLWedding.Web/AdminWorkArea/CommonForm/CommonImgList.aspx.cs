using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HHLWedding.ToolsLibrary;
using HHLWedding.DataAssmblly;
using HHLWedding.BLLAssmbly;
using HHLWedding.BLLAssmbly.Sys;
using HHLWedding.Pages;

namespace HHLWedding.Web.AdminWorkArea.CommonForm
{
    public partial class CommonImgList : SystemPage
    {

        CommonImageService _imgService = new CommonImageService();

        string orderByName = "State";

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
            int sourceCount = 0;
            List<PMSParameters> pars = new List<PMSParameters>();
            if (Request["Type"] != null)        //类型
            {
                int type = Request["Type"].ToInt32();
                pars.Add("TypeId", type, NSqlTypes.Equal);
            }

            if (Request["Id"] != null)          //酒店Id
            {
                int Id = Request["Id"].ToInt32();
                pars.Add("CommonId", Id, NSqlTypes.Equal);
            }

            var DataList = _imgService.GetAllByPager(pars, orderByName, CtrPageIndex.CurrentPageIndex, CtrPageIndex.PageSize, out sourceCount);

            CtrPageIndex.RecordCount = sourceCount;


            SavePage(CtrPageIndex);
            dlImgList.DataSource = DataList;
            dlImgList.DataBind();
        }
        #endregion

        #region 分页
        /// <summary>
        /// 分页
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