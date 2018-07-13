using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly;
using HHLWedding.DataAssmblly.CommonModel;
using HHLWedding.Pages;
using HHLWedding.ToolsLibrary;
using HHLWedding.Web.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HHLWedding.Web.AdminWorkArea.Sys.JobType
{
    public partial class Sys_EmployeeJobManager : SystemPage
    {

        EmployeeJob _empJobService = new EmployeeJob();

        string orderByName = "createTime";
        int page = 1;

        #region 页面初始化
        /// <summary>
        /// 页面初始化
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {

            //string controlName = Request.Params.Get("__EVENTTARGET");
            //string eventArgument = Request.Params.Get("__EVENTARGUMENT");

            //// layer 关闭之后  调用
            //if (controlName == "btnLook" && eventArgument == "1")
            //{
            //    BinderData();
            //}

            if (!IsPostBack)
            {
               
                CtrPageIndex.CurrentPageIndex = page;
                BinderData();
            }
        }
        #endregion

        #region 数据加载
        /// <summary>
        /// 数据加载
        /// </summary>
        public void BinderData()
        {
            int sourceCount = 0;
            List<PMSParameters> pars = new List<PMSParameters>();
            
            var DataList = _empJobService.GetAllByPager(pars, orderByName, CtrPageIndex.CurrentPageIndex, CtrPageIndex.PageSize, out sourceCount);
            CtrPageIndex.RecordCount = sourceCount;

            SavePage(CtrPageIndex);

            repEmpJob.DataSource = DataList;
            repEmpJob.DataBind();
        }
        #endregion

        #region 分页控件分页
        /// <summary>
        /// 分页
        /// </summary>
        protected void CtrPageIndex_PageChanged(object sender, EventArgs e)
        {
            BinderData();
        }
        #endregion

        #region 点击确定  添加员工职务
        /// <summary>
        /// 添加员工职务
        /// </summary> 
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            BinderData();
        }
        #endregion

    }
}