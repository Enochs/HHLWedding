﻿using HHLWedding.BLLAssmbly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HHLWedding.ToolsLibrary;
using HHLWedding.DataAssmblly;
using HHLWedding.DataAssmblly.CommonModel;
using System.IO;
using HHLWedding.Pages;

namespace HHLWedding.Web.AdminWorkArea.Sys.Sys_Employee
{
    public partial class Sys_EmployeeUpdate : SystemPage
    {
        #region 服务声明

        /// <summary>
        /// 员工职务
        /// </summary>
        EmployeeJob _empJobService = new EmployeeJob();

        /// <summary>
        /// 员工类型
        /// </summary>
        EmployeeType _empTypeService = new EmployeeType();


        /// <summary>
        /// 员工
        /// </summary>
        Employee _empService = new Employee();

        /// <summary>
        /// 部门
        /// </summary>
        BLLAssmbly.Department _departService = new BLLAssmbly.Department();

        #endregion

        #region 页面初始化
        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlDataBind();
            }
        }
        #endregion

        #region 下拉框绑定
        /// <summary>
        /// 下拉框绑定
        /// </summary>
        public void ddlDataBind()
        {
            //员工职务
            var JobList = _empJobService.GetAllByStatus(1);
            ddlEmpJob.DataSource = JobList;
            ddlEmpJob.DataTextField = "JobName";
            ddlEmpJob.DataValueField = "JobID";
            ddlEmpJob.DataBind();

            //员工类型
            var TypeList = _empTypeService.GetAllByStatus(1);
            ddlEmpType.DataSource = TypeList;
            ddlEmpType.DataTextField = "TypeName";
            ddlEmpType.DataValueField = "EmployeeTypeID";
            ddlEmpType.DataBind();

            //员工部门
            var DepartList = _departService.GetByAll();
            ddlDepart.DataSource = DepartList;
            ddlDepart.DataTextField = "DepartmentName";
            ddlDepart.DataValueField = "DepartmentID";
            ddlDepart.DataBind();
        }
        #endregion

        #region 点击提交 提交功能
        /// <summary>
        /// 提交
        /// </summary>
        protected void confirm_Click(object sender, EventArgs e)
        {
            int EmployeeId = Request["employeeId"].ToInt32();
            //声明实体类
            DataAssmblly.Sys_Employee employee = _empService.GetByID(EmployeeId);
            //赋值
            employee.EmployeeName = Request["txtEmployeeName"].ToString();
            employee.JobID = ddlEmpJob.SelectedValue.ToInt32();
            employee.EmployeeTypeID = ddlEmpType.SelectedValue.ToInt32();
            employee.DepartmentID = ddlDepart.SelectedValue.ToInt32();
            employee.Sex = Request["ddlSex"].ToInt32() == 1 ? true : false;
            employee.LoginName = Request["txtLoginName"].ToString();
            if (!string.IsNullOrEmpty(Request["txtPwd"].ToString()))
            {
                string passWord = Request["txtPwd"].ToString();
                employee.PassWord = passWord.MD5Hash();
            }
            
            employee.BornDate = Request["txtBornDate"].ToString().ToDateTime();
            employee.TelPhone = Request["txtTelPhone"].ToString();
            employee.IdCard = Request["txtIdCard"].ToString();
            employee.ComeInDate = Request["txtComeInDate"].ToString().ToDateTime();

            #region 上传头像

            HttpFileCollection Files = HttpContext.Current.Request.Files;
            HttpPostedFile PostedFile = Files[0];
            if (PostedFile.ContentLength > 0)
            {
                //文件名
                string FileName = PostedFile.FileName;
                //后缀名
                string strExPrentFile = FileName.Substring(FileName.LastIndexOf(".") + 1);
                //路径
                string sFilePath = "/images/Personal";
                //就对路径
                string path = Server.MapPath(sFilePath);
                //存入服务器头像名称
                string NewFileName = employee.EmployeeName + "." + strExPrentFile;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string urlAddress = sFilePath + "/" + NewFileName;
                PostedFile.SaveAs(Server.MapPath(urlAddress));

                employee.ImageURL = urlAddress;
                employee.UploadImageName = NewFileName;
            }
            #endregion

            int result = _empService.Update(employee);
            Response.Write(@"<script>var index = window.parent.layer.getFrameIndex(window.name);window.parent.layer.close(index)</script>");

        }
        #endregion
    }
}