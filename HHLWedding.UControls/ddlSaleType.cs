using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using HHLWedding.ToolsLibrary;

namespace HHLWedding.Control
{
    public class ddlSaleType : DropDownList
    {
        BaseService<FD_SaleType> _saleTypeService = new BaseService<FD_SaleType>();

        BaseService<FD_SaleSource> _saleSourceService = new BaseService<FD_SaleSource>();
        public ddlSaleType()
        {
            //获取启用的渠道类型
            var DataList = _saleTypeService.FindAll().Where(c => c.Status == 1).ToList();
            this.DataSource = DataList;
            this.DataTextField = "SaleTypeName";
            this.DataValueField = "SaleTypeID";
            this.DataBind();
            this.CssClass = "form-control chosen-selects";
            this.Items.Insert(0, new ListItem { Text = "请选择", Value = "0" });
        }

        public void BinderData()
        {

        }

    }
}
