using HHLWedding.DataAssmblly.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using HHLWedding.DataAssmblly;

namespace HHLWedding.Control
{
    public class ddlState : DropDownList
    {
        public ddlState()
        {
            //获取所有状态
            CustomerState cusState = CustomerState.NoInvite;
            var stateList = cusState.GetSelectList("所有状态");
            this.DataSource = stateList;
            this.DataTextField = "Text";
            this.DataValueField = "Value";
            this.DataBind();
            this.CssClass = "form-control chosen-selects";
        }
    }
}
