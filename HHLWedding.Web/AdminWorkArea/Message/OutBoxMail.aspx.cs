using HHLWedding.BLLAssmbly;
using HHLWedding.BLLAssmbly.FD;
using HHLWedding.BLLManager;
using HHLWedding.DataAssmblly;
using HHLWedding.DataAssmblly.CommonModel;
using HHLWedding.Pages;
using HHLWedding.ToolsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HHLWedding.Web.AdminWorkArea.Message
{
    public partial class OutBoxMail : SystemPage
    {
        #region 服务

        /// <summary>
        /// 通用DBHelper
        /// </summary>
        DBHelper db = new DBHelper();

        /// <summary>
        /// 消息
        /// </summary>
        MessageService _msgService = new MessageService();

        /// <summary>
        /// 消息管理 
        /// </summary>
        MessageManager _msgManager = new MessageManager();
        /// <summary>
        /// 员工
        /// </summary>
        Employee _empService = new Employee();

        #endregion

        public int empId = 0;
        int type = 0;
        #region 页面加载
        /// <summary>
        /// 页面初始化
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    empId = LoginInfo.UserInfo.EmployeeId;
                    if (Request["type"] != null)
                    {
                        type = Request["type"].ToString().ToInt32();
                    }
                    BinderData();

                }
                catch
                {
                    Page_Init(sender, e);
                }
            }
        }
        #endregion

        #region 数据绑定  发件箱
        /// <summary>
        /// 绑定消息
        /// </summary>
        public void BinderData()
        {
            int sourceCount = 0;
            List<PMSParameters> pars = new List<PMSParameters>();


            string search = string.Empty;
            string toEmployee = string.Empty;

            if (Request["search"] != null)
            {
                search = Request["search"].ToString();
            }

            if (Request["toEmployee"] != null)
            {
                toEmployee = Request["toEmployee"].ToString();
            }

            var DataList = _msgManager.GetAllByPager(CtrPageIndex.CurrentPageIndex, CtrPageIndex.PageSize, search, toEmployee, ref sourceCount, CtrPageIndex, 2, type);

            repMessage.DataSource = DataList;
            repMessage.DataBind();
        }
        #endregion

        #region 分页
        /// <summary>
        /// 分页查询
        /// </summary>
        protected void CtrPageIndex_PageChanged(object sender, EventArgs e)
        {
            BinderData();
        }
        #endregion

        #region 异步刷新
        /// <summary>
        /// @author:wp
        /// @datetime:2016-08-16
        /// @desc: 异步刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            BinderData();
        }
        #endregion
    }
}