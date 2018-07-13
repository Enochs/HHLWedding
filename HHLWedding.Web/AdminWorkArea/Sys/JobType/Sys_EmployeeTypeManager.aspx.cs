using HHLWedding.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HHLWedding.DataAssmblly;
using HHLWedding.BLLAssmbly;
using HHLWedding.ToolsLibrary;

namespace HHLWedding.Web.AdminWorkArea.Sys.JobType
{
    public partial class Sys_EmployeeTypeManager : SystemPage
    {
        /// <summary>
        /// 员工类型
        /// </summary>
        EmployeeType _empTypeService = new EmployeeType();

        string OrderName = "CreateTime";

        #region 页面初始化
        /// <summary>
        /// 页面初始化
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                BinderData();
            }
        }
        #endregion

        #region  页面加载
        public void BinderData()
        {
            int sourceCount = 0;

            List<PMSParameters> pars = new List<PMSParameters>();

            
            var DataList = _empTypeService.GetAllByPager(pars, OrderName, CtrPageIndex.CurrentPageIndex, CtrPageIndex.PageSize, out sourceCount);
            CtrPageIndex.RecordCount = sourceCount;
            SavePage(CtrPageIndex);
            repEmpType.DataSource = DataList;
            repEmpType.DataBind();
        }

        #endregion

        #region 搜索/查询
        /// <summary>
        /// 搜索
        /// </summary>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            BinderData();
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
    }
}