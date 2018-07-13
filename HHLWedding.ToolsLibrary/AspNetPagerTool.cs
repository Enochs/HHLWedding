using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wuqi.Webdiyer;

namespace HHLWedding.ToolsLibrary
{
    public class AspNetPagerTool : AspNetPager
    {
        public AspNetPagerTool()
        {
            this.PageSize = 10;
            this.AlwaysShow = true;
            this.PrevPageText = "上一页";
            this.NextPageText = "下一页";
            //this.ShowPrevNext = false;
            this.FirstPageText = "首页";
            this.LastPageText = "末页";

            this.CurrentPageButtonClass = "active";
            this.ShowCustomInfoSection = Wuqi.Webdiyer.ShowCustomInfoSection.Left;
            this.LayoutType = Wuqi.Webdiyer.LayoutType.Table;
            this.SubmitButtonClass = "btn btn-danger";
            this.CssClass = "paginator";

            this.NumericButtonCount = 6;               // 显示页码的个数




        }

        public void OnLoad()
        {
            int totalPage = this.RecordCount % this.PageSize == 0 ? this.RecordCount / this.PageSize : this.RecordCount / this.PageSize + 1;
            totalPage = totalPage == 0 ? 1 : totalPage;
            this.CustomInfoHTML = "当前第" + this.CurrentPageIndex + "/" + totalPage + "页 共" + this.RecordCount + "条记录 每页" + this.PageSize + "条";

        }



    }
}
