using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HHLWedding.ToolsLibrary;
using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly;
using HHLWedding.BLLAssmbly.FD;
using HHLWedding.EditoerLibrary;
using HHLWedding.DataAssmblly.CommonModel;
using HHLWedding.Pages;

namespace HHLWedding.Web.AdminWorkArea.Foundation.Hotel
{
    public partial class HotelCreate : SystemPage
    {
        /// <summary>
        /// 场地标签
        /// </summary>
        HotelLabelService _labelService = new HotelLabelService();

        /// <summary>
        /// 酒店标签
        /// </summary>
        HotelService _hotelService = new HotelService();

        #region 页面初始化
        /// <summary>
        /// 页面加载
        /// </summary>  
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BinderLabel();
            }
        }
        #endregion

        #region 绑定场地标签   绑定酒店类型
        /// <summary>
        /// 绑定场地标签
        /// </summary>
        public void BinderLabel()
        {
            Type type = typeof(HotelTypeName);
            selHotelType.DataSource = DisplayNameExtension.GetSelectList(HotelTypeName.HType1, type);
            selHotelType.DataBind();

            //获取启用的场地标签
            var DataList = _labelService.GetAllByEnable(1);

            repLabel.DataSource = DataList;
            repLabel.DataBind();
        }

        #endregion

    }
}