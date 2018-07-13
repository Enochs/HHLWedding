using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace HHLWedding.Control
{
    public class ddlHotel : DropDownList
    {
        BaseService<FD_Hotel> _hotelService = new BaseService<FD_Hotel>();
        public ddlHotel()
        {
            //获取启用的酒店
            var DataList = _hotelService.FindAll().Where(c => c.Status == 1).OrderBy(c => c.Letter).ToList();
            this.DataSource = DataList;
            this.DataTextField = "HotelName";
            this.DataValueField = "HotelId";
            this.DataBind();
            this.CssClass = "form-control chosen-selects";
            //this.Width = 235;
            this.Items.Insert(0, new ListItem { Text = "请选择酒店", Value = "0" });
        }

        public void BinderData()
        {
           
        }
    }
}
