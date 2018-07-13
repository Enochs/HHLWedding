using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace HHLWedding.Control
{
    public class btnReload : Button
    {
        public btnReload()
        {
            this.Text = "条件重置";
            this.CssClass = "btn btn-primary btn-sm";
            this.Click += BtnReload_Click;
        }

        private void BtnReload_Click(object sender, EventArgs e)
        {
            this.Page.Response.Redirect(this.Page.Request.Url.ToString());
        }
    }
}
