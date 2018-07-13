using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly;
using HHLWedding.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HHLWedding.ToolsLibrary;
using HHLWedding.DataAssmblly.CommonModel;
using System.Web.Security;
using HHLWedding.EditoerLibrary;
using System.Web.Mvc;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;

namespace HHLWedding.Web.AdminWorkArea.Sys.EmployeeInfo
{
    public partial class DetailsEmployee : SystemPage
    {

        #region 服务

        /// <summary>
        /// 日志
        /// </summary>
        BaseService<sys_LoginLog> _logService = new BaseService<sys_LoginLog>();

        /// <summary>
        /// 员工
        /// </summary>
        Employee _empService = new Employee();

        #endregion

        #region 页面加载
        /// <summary>
        /// 页面加载
        /// </summary>      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlEmployee.DataBinder();
                BinderData();


            }
        }
        #endregion

        #region 绑定数据
        /// <summary>
        /// 数据绑定
        /// </summary>
        public void BinderData()
        {
            int sourceCount = 0;

            //条件
            List<Expression<Func<sys_LoginLog, bool>>> parmList = new List<Expression<Func<sys_LoginLog, bool>>>();

            //获取本人及下级客户(默认只显示本人自己一个人的信息)
            ddlEmployee.GetMyEmployee<sys_LoginLog>("LoginEmployee", parmList);

            //登录时间
            DateTime start = TimeHelper.GetStartTime(Request["start"]);
            DateTime end = TimeHelper.GetEndTime(Request["end"]);
            parmList.Add(c => c.LoginDate >= start && c.LoginDate <= end);

            var DataList = _logService.GetPagedList<DateTime?>(CtrPageIndex.CurrentPageIndex, CtrPageIndex.PageSize, ref sourceCount, parmList, c => c.LoginDate, false);
            CtrPageIndex.RecordCount = sourceCount;
            //SavePage(CtrPageIndex);
            CtrPageIndex.OnLoad();
            repLoginLog.DataSource = DataList;
            repLoginLog.DataBind();
        }
        #endregion

        #region 为实现异步刷新
        /// <summary>
        /// 异步刷新
        /// </summary>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            BinderData();
        }
        #endregion

        #region 分页
        /// <summary>
        /// 页码分页
        /// </summary>
        protected void CtrPageIndex_PageChanged(object sender, EventArgs e)
        {
            BinderData();
        }
        #endregion

        #region 导出Excel
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {

            int count = 0;
            var DataList = _logService.GetPagedList<DateTime?>(CtrPageIndex.CurrentPageIndex, CtrPageIndex.PageSize, ref count, null, c => c.LoginDate, false);
            DataTable dt = ListToDataTable.ListToTable<sys_LoginLog>(DataList);
            ExcelUtil.RenderToExcel(dt, Context, "日志.xls");
        }


        #endregion


    }
}