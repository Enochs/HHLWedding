using System;
using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using HHLWedding.ToolsLibrary;

namespace HHLWedding.Control
{
    public class ddlSaleSource : DropDownList
    {
        BaseService<FD_SaleSource> _saleSourceService = new BaseService<FD_SaleSource>();
        public ddlSaleSource()
        {
            //获取启用的渠道
            var DataList = _saleSourceService.FindAll().Where(c => c.Status == 1 && c.SaleTypeId == 1).OrderBy(c => c.letter).ToList();
            this.DataSource = DataList;
            this.DataTextField = "SourceName";
            this.DataValueField = "SourceID";
            this.DataBind();
            this.CssClass = "form-control";
            this.Width = 235;
            this.Items.Insert(0, new ListItem { Text = "请选择", Value = "0" });
        }

        public void BinderData()
        {

        }

    }
}
