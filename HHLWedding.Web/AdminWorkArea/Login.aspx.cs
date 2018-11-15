using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly;
using HHLWedding.DataAssmblly.CommonModel;
using HHLWedding.EditoerLibrary;
using HHLWedding.ToolsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HHLWedding.Web.AdminWorkArea
{
    public partial class Login : System.Web.UI.Page
    {
        Employee _empService = new Employee();

        public string Ip;
        public string loginName;
        public string Pwd;
        public string login_Msg;
        public bool login_Success = false;

        #region 页面加载
        /// <summary>
        /// 加载
        /// </summary>  
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //清除全部Session
                Session.Abandon();

                Ip = Page.Request.UserHostAddress;
                HttpCookie aCookie = Request.Cookies["Employee"];
                if (aCookie != null)
                {
                    loginName = aCookie.Values["Name"];
                    Pwd = aCookie.Values["PWd"];
                    string state = aCookie.Values["State"];
                    if (state == "True")
                    {
                        ChkLoginState.Checked = true;
                    }
                }
                string url = this.Page.Request.Url.ToString();
                if (url.Contains('?'))
                {
                    Response.Redirect(url.Substring(0, url.IndexOf('?')));
                }
            }
        }
        #endregion

        #region 点击登录功能
        /// <summary>
        /// 登录
        /// </summary>   
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string loginName = Request["loginName"].ToString().Trim();
            string passwords = Request["passWord"].ToString().Trim();
            bool IsCheck = ChkLoginState.Checked;
            Sys_Employee m_emp = _empService.GetByLoginName(loginName);
            if (m_emp != null)
            {
                if (m_emp.PassWord != passwords.MD5Hash())
                {
                    JavaScriptTools.AlertWindow("密码输入错误", Page);
                    return;
                }
            }
            else
            {
                JavaScriptTools.AlertWindow("帐号不存在", Page);
                return;
            }

            #region 利用方法 获取帐号 密码

            var form = System.Web.HttpContext.Current.Request.Form;

            string name = CommonTools.GetKeyValue(form, "loginName");
            string password = CommonTools.GetKeyValue(form, "passWord");

            #endregion

            //保存个人登录信息 
            FormsAuthentication.RedirectFromLoginPage(m_emp.EmployeeID.ToString(), false);

            //写入登录日志
            //LoginLog(m_emp);

            //保存Session
            UserEmployee user = new UserEmployee();
            user.EmployeeId = m_emp.EmployeeID;
            user.LoginName = m_emp.LoginName;
            user.EmployeeName = m_emp.EmployeeName;

            Session["UserInfo"] = user;

            //html页面验证Cookie是否已经登录(/Script/PublicCommon.js)
            StringControl.DeleteCookie("Login");

            HttpCookie cookie = new HttpCookie("Login");
            cookie.Values.Add("EmpId", user.EmployeeId.ToString());
            cookie.Expires = System.DateTime.Now.AddDays(7);
            HttpContext.Current.Response.Cookies.Add(cookie);

            //WebService.Login login = new WebService.Login();
            //login.SaveLoginCookie(loginName, passwords, IsCheck);
        }
        #endregion

        #region 登录日志
        /// <summary>
        /// 写入登录日志
        /// </summary>
        /// <param name="m_emp"></param>
        public void LoginLog(Sys_Employee m_emp)
        {
            BaseService<sys_LoginLog> _logService = new BaseService<sys_LoginLog>();

            //修改最后一次登录时间
            var m_LastLog = _logService.GetListBy<DateTime?>(null, c => c.LoginDate, false).FirstOrDefault();
            if (m_LastLog != null)
            {
                m_emp.LastLoginTime = m_LastLog.LoginDate;
                _empService.Update(m_emp);
            }

            //添加登录日志
            string ip = IpAddress.GetIP();
            sys_LoginLog log = new sys_LoginLog();
            log.LoginId = Guid.NewGuid();
            log.LoginEmployee = m_emp.EmployeeID;
            log.LoginEmployeeKey = m_emp.EmployeeID.ToString();
            log.LoginDate = DateTime.Now;
            log.LoginCity = ComputerInfo.Address;       //登录城市
            log.LoginIpAddress = ComputerInfo.WIp;                //外网Ip
            log.LoginInIp = ComputerInfo.IpAddress;        //内网Ip

            _logService.Add(log);
        }
        #endregion
    }
}