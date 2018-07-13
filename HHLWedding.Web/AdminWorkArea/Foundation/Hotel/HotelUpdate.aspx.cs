using HHLWedding.BLLAssmbly.FD;
using HHLWedding.DataAssmblly.CommonModel;
using HHLWedding.EditoerLibrary;
using HHLWedding.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HHLWedding.Web.AdminWorkArea.Foundation.Hotel
{
    public partial class HotelUpdate : SystemPage
    {
        //场地标签
        HotelLabelService _labelService = new HotelLabelService();

        #region 页面加载  初始化
        /// <summary>
        /// 初始化
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BinderLabel();
            }
        }
        #endregion

        #region 绑定场地标签
        /// <summary>
        /// @author:wp
        /// @datetime:2016-08-31
        /// @desc:绑定场地标签
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