using HHLWedding.Web.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.UI;
using System.Web.Services;
using System.Web;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly.CommonModel;
using HHLWedding.EditoerLibrary;
using HHLWedding.DataAssmblly;
using HHLWedding.ToolsLibrary;
using System.Linq.Expressions;

namespace HHLWedding.Web.WebService
{
    /// <summary>
    /// BaseHandler 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class BaseHandler : System.Web.Services.WebService
    {

        #region service 基本

        BaseService<FD_SaleSource> _saleSourceService = new BaseService<FD_SaleSource>();
        BaseService<FD_SaleType> _saleTypeService = new BaseService<FD_SaleType>();

        #endregion

        #region 获取基本信息
        /// <summary>
        /// 获取基本信息
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage GetBaseInfo()
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            try
            {
                UserHost host = new UserHost();
                //string hostName = Environment.MachineName;
                //string userDominName = Environment.UserDomainName;
                //string userName = Environment.UserName;

                host.hostName = ComputerInfo.ComputerName;
                host.InIp = ComputerInfo.IpAddress;
                host.loginName = LoginInfo.UserInfo.LoginName;
                host.userName = ComputerInfo.LoginUserName;       //登录用户名称
                host.loginAddress = ComputerInfo.Address;
                host.wIp = ComputerInfo.WIp;
                host.systemType = ComputerInfo.SystemType;

                ajax.IsSuccess = true;
                ajax.Data = host;

            }
            catch (Exception e)
            {
                ajax.Message = e.Message.ToString();
            }

            return ajax;
        }
        #endregion

        #region 根据Ip获取城市
        /// <summary>
        /// 根据IP获取城市
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetCity(string ip)
        {
            string city = IpAddress.GetAddressByIp(ip, 2);
            return city;
        }
        #endregion

        #region 添加登录日志
        /// <summary>
        /// 添加登录日志
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage CreateLoginLog(string ip)
        {
            BaseService<sys_LoginLog> _logService = new BaseService<sys_LoginLog>();
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            try
            {

                var m_LastLog = _logService.GetListBy<DateTime?>(null, c => c.LoginDate, false).FirstOrDefault();
                if (m_LastLog != null)
                {
                    ajax.Value = m_LastLog.LoginDate.ToString().ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    ajax.Value = null;
                }

                sys_LoginLog log = new sys_LoginLog();
                log.LoginId = Guid.NewGuid();
                log.LoginEmployee = LoginInfo.UserInfo.EmployeeId;
                log.LoginDate = DateTime.Now;
                log.LoginCity = IpAddress.GetAddressByIp(ip);
                log.LoginIpAddress = ip;                //外网Ip
                log.LoginInIp = Context.Request.UserHostAddress;        //内网Ip

                sys_LoginLog res_log = _logService.Add(log);
                if (res_log != null)
                {
                    ajax.IsSuccess = true;
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;
        }
        #endregion

        #region 备份数据库
        /// <summary>
        /// 数据库备份
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage BackUpDataBase()
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            try
            {
                string path = "E:\\BackUp";
                string fileName = "HHL_Wedding" + DateTime.Now.ToString("_yyyyMMdd") + ".bak";

                string fileFullPath = path + "/" + fileName;

                //创建文件夹
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //验证文件是否存在
                if (File.Exists(fileFullPath))
                {
                    File.Delete(fileFullPath);
                }

                //将所有文件存在泛型
                string[] paths = Directory.GetFiles(path); //获取文件夹下全部文件路径
                List<FileInfo> files = new List<FileInfo>();
                foreach (string filepath in paths)
                {//确认是本数据库的备份文件
                    bool isExists = filepath.Contains("HHL_Wedding");
                    if (filepath.Contains("HHL_Wedding"))
                    {
                        FileInfo file = new FileInfo(filepath); //获取单个文件
                        files.Add(file);
                    }
                }

                DBHelper db = new DBHelper();
                string strBacl = "backup database HHL_Wedding to disk='" + fileFullPath + "'";

                if (db.ExcuteNonQuery(strBacl) != 0)
                {
                    ajax.Message = "数据备份成功";
                    ajax.IsSuccess = true;
                    //最多保存3个备份文件
                    if (files.Count >= 3)
                    {
                        //获取路径
                        fileFullPath = files[0].ToString();
                        //删除文件
                        if (File.Exists(fileFullPath))
                        {
                            File.Delete(fileFullPath);
                        }
                    }

                }
                else
                {
                    ajax.Message = "数据备份失败";

                }
            }
            catch (Exception ee)
            {
                ajax.Message = ee.Message.ToString();
            }
            return ajax;
        }
        #endregion

        #region 获取渠道信息
        /// <summary>
        /// 根据渠道类型来获取渠道
        /// </summary>
        /// <param name="SaleTypeId"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage GetSaleSourceByType(string SaleTypeId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                if (!string.IsNullOrEmpty(SaleTypeId))
                {
                    int SaleType = SaleTypeId.ToInt32();
                    List<Expression<Func<FD_SaleSource, bool>>> parmList = new List<Expression<Func<FD_SaleSource, bool>>>();
                    parmList.Add(c => c.SaleTypeId == SaleType);
                    var saleSourceList = _saleSourceService.GetListBy(parmList);
                    ajax.IsSuccess = true;
                    ajax.Data = saleSourceList;

                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;
        }
        #endregion


        #region 获取客户状态
        /// <summary>
        /// 获取客户状态
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage GetCustomerState()
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            try
            {
                CustomerState cusState = CustomerState.NoInvite;
                var stateList = cusState.GetSelectList("所有状态");
                if (stateList.Count > 0)
                {
                    ajax.Data = stateList.Where(c => ("1,2,3,5").Contains(c.Value));
                    ajax.IsSuccess = true;
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }
            return ajax;

        }

        #endregion

    }






    public class UserHost
    {
        public string hostName { get; set; }
        public string loginName { get; set; }
        public string userName { get; set; }
        public string InIp { get; set; }
        public string loginAddress { get; set; }
        public string wIp { get; set; }
        public string systemType { get; set; }
    }
}
