using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly;
using HHLWedding.DataAssmblly.CommonModel;
using HHLWedding.DataAssmblly.Model;
using HHLWedding.Pages;
using HHLWedding.ToolsLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;

namespace HHLWedding.Web.Handler
{
    /// <summary>
    /// EmployeeHandler 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]

    public class EmployeeHandler : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";

        }


        #region 服务

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
        /// 所有菜单
        /// </summary>
        ChannelService _channelService = new ChannelService();

        /// <summary>
        /// 所有权限
        /// </summary>
        EmployeePower _empPowerService = new EmployeePower();

        #endregion



        #region EmployeeJob
        /// <summary>
        /// @author:wupeng
        /// @datetime:2016-07-21
        /// @desc: 添加员工职务
        /// </summary>
        /// <param name="JobName">职务名称</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage CreateEmployeeJob(string JobName)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                Sys_EmployeeJob empJob = new Sys_EmployeeJob();
                empJob.Jobname = JobName;
                empJob.createTime = DateTime.Now;
                empJob.EmployeeId = LoginInfo.UserInfo.EmployeeId;
                empJob.Status = (byte)SysStatus.Enable;
                int result = _empJobService.Insert(empJob);
                if (result > 0)
                {
                    ajax.IsSuccess = true;
                    ajax.Message = "添加成功";
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;
        }


        /// <summary>
        /// @author:wupeng
        /// @datetime:2016-07-27
        /// @desc: 根据员工Id获取员工职位
        /// </summary>
        /// <param name="jobId">职务Id</param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage GetEmpJobById(int JobId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            Sys_EmployeeJob empJob = _empJobService.GetByID(JobId);
            if (empJob != null)
            {
                ajax.IsSuccess = true;
                ajax.Value = empJob.Jobname;
                ajax.Id = empJob.JobID;
            }
            return ajax;
        }

        /// <summary>
        /// @author:wupeng
        /// @datetime:2016-07-27
        /// @desc: 修改员工职务
        /// </summary>
        /// <param name="JobName">职务名称</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage UpdateEmployeeJob(Sys_EmployeeJob _empJob)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                Sys_EmployeeJob empJob = _empJobService.GetByID(_empJob.JobID); ;
                empJob.Jobname = _empJob.Jobname;
                empJob.EmployeeId = LoginInfo.UserInfo.EmployeeId;
                int result = _empJobService.Update(empJob);
                if (result > 0)
                {
                    ajax.IsSuccess = true;
                    ajax.Message = "修改成功";
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;
        }

        /// <summary>
        /// 单个禁用/启用
        /// </summary>
        /// <param name="ChannelId"></param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage SetEmpJobSingleStatus(int jobId, int index)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                if (!string.IsNullOrEmpty(jobId.ToString()) && jobId != 0)
                {
                    Sys_EmployeeJob m_empJob = _empJobService.GetByID(jobId);
                    if (m_empJob.Status == (byte)SysStatus.Enable)
                    {
                        m_empJob.Status = (byte)SysStatus.Disable;
                    }
                    else
                    {
                        m_empJob.Status = (byte)SysStatus.Enable;
                    }

                    int result = _empJobService.Update(m_empJob);
                    if (result > 0)
                    {
                        ajax.IsSuccess = true;
                        ajax.Id = jobId;
                        ajax.Index = index;
                        if (m_empJob.Status == (byte)SysStatus.Disable)
                        {
                            ajax.Message = "禁用成功";
                        }
                        else
                        {
                            ajax.Message = "启用成功";
                        }
                    }

                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;

        }

        #endregion


        #region EmployeeType
        /// <summary>
        /// @author:wupeng
        /// @datetime:2016-07-21
        /// @desc: 添加员工类型
        /// </summary>
        /// <param name="TypeName">类型名称</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage CreateEmployeeType(string TypeName)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                Sys_EmployeeType empType = new Sys_EmployeeType();
                empType.TypeName = TypeName;
                empType.CreateTime = DateTime.Now;
                empType.EmployeeId = LoginInfo.UserInfo.EmployeeId;
                empType.Status = (byte)SysStatus.Enable;
                int result = _empTypeService.Insert(empType);
                if (result > 0)
                {
                    ajax.IsSuccess = true;
                    ajax.Message = "添加成功";
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;
        }


        /// <summary>
        /// @author:wupeng
        /// @datetime:2016-07-27
        /// @desc: 根据员工Id获取员工类型
        /// </summary>
        /// <param name="TypeId">职务Id</param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage GetEmpTypeById(int TypeId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            Sys_EmployeeType empType = _empTypeService.GetByID(TypeId);
            if (empType != null)
            {
                ajax.IsSuccess = true;
                ajax.Value = empType.TypeName;
                ajax.Id = empType.EmployeeTypeID;
            }
            return ajax;
        }

        /// <summary>
        /// @author:wupeng
        /// @datetime:2016-07-27
        /// @desc: 修改员工类型
        /// </summary>
        /// <param name="TypeName">类型名称</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage UpdateEmployeeType(Sys_EmployeeType _empType)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                Sys_EmployeeType empType = _empTypeService.GetByID(_empType.EmployeeTypeID);
                empType.TypeName = _empType.TypeName;
                empType.EmployeeId = LoginInfo.UserInfo.EmployeeId;
                int result = _empTypeService.Update(empType);
                if (result > 0)
                {
                    ajax.IsSuccess = true;
                    ajax.Message = "修改成功";
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;
        }

        /// <summary>
        /// @author:wupeng
        /// @datetime:2016-07-27
        /// @desc: 单个禁用/启用
        /// </summary>
        /// <param name="TypeId">类型ID</param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage SetEmpTypeSingleStatus(int TypeId, int index)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                if (!string.IsNullOrEmpty(TypeId.ToString()) && TypeId != 0)
                {
                    Sys_EmployeeType m_empType = _empTypeService.GetByID(TypeId);
                    if (m_empType.Status == (byte)SysStatus.Enable)
                    {
                        m_empType.Status = (byte)SysStatus.Disable;
                    }
                    else
                    {
                        m_empType.Status = (byte)SysStatus.Enable;
                    }

                    int result = _empTypeService.Update(m_empType);
                    if (result > 0)
                    {
                        ajax.IsSuccess = true;
                        ajax.Id = TypeId;
                        ajax.Index = index;
                        if (m_empType.Status == (byte)SysStatus.Disable)
                        {
                            ajax.Message = "禁用成功";
                        }
                        else
                        {
                            ajax.Message = "启用成功";
                        }
                    }

                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;

        }

        #endregion


        #region Employee

        #region 获取Employee职工数量
        /// <summary>
        /// 获取数量
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage GetEmployeeCount()
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            var DataList = _empService.GetByAll();
            if (DataList != null && DataList.Count > 0)
            {
                int count = DataList.Count();
                ajax.IsSuccess = true;
                ajax.Value = count.ToString();
            }
            return ajax;
        }
        #endregion

        #region 建立初始账户
        /// <summary>
        /// 初始账户
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage CreateLoadEmp()
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";

            try
            {
                bool isExists = _empService.CheckLoginName("wupeng");
                if (isExists == false)
                {
                    Sys_Employee emp = new Sys_Employee();
                    emp.JobID = -1;
                    emp.DepartmentID = -1;
                    emp.EmployeeTypeID = -1;
                    emp.EmployeeName = "吴鹏";
                    emp.LoginName = "wupeng";
                    emp.PassWord = "123456";
                    emp.CreateDate = DateTime.Now;
                    emp.ComeInDate = DateTime.Now;
                    emp.Sex = false;
                    emp.BornDate = DateTime.Now.AddYears(-10);
                    emp.TelPhone = "";
                    emp.Status = (byte)SysStatus.Enable;
                    //建立帐号
                    int final = _empService.Insert(emp);
                    if (final > 0)
                    {
                        //给予权限
                        string channel = "1,2,3,4,5,6";
                        //逗号隔开获取菜单ID
                        string[] power = channel.Split(',');

                        int result = 0;

                        for (int i = 0; i < power.Length; i++)
                        {
                            int ChannelId = power[i].ToInt32();

                            Sys_Channel m_channel = _channelService.GetByID(ChannelId);

                            Sys_EmployeePower m_empPower = null;
                            m_empPower = new Sys_EmployeePower();
                            m_empPower.Powername = m_channel.ChannelName;
                            m_empPower.EmployeeID = emp.EmployeeID;
                            m_empPower.DepartmentID = emp.DepartmentID;
                            m_empPower.ChannelID = ChannelId;
                            m_empPower.UrlAddress = m_channel.ChannelAddress;
                            m_empPower.CreateDate = DateTime.Now;
                            m_empPower.Status = (byte)SysStatus.Enable;
                            m_empPower.Parent = m_channel.Parent;
                            m_empPower.Sort = m_channel.SortInt;
                            //区别 1级菜单和2级菜单
                            if (m_channel.Parent == 0)
                            {
                                m_empPower.ItemLevel = 1;
                            }
                            else
                            {
                                m_empPower.ItemLevel = 2;
                            }

                            //添加到数据库
                            result = _empPowerService.Insert(m_empPower);
                            if (result > 0)
                            {
                                ajax.IsSuccess = true;
                                ajax.Message = "建立初始账户成功";
                            }

                        }
                    }
                }

            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;
        }
        #endregion

        #region 判断用户名是否存在
        /// <summary>
        /// @author:wupeng
        /// @datetime:2016-07-30
        /// @desc: 判断用户名是否存在
        /// </summary>
        /// <param name="loginName">用户名</param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage CheckLoginName(string loginName)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                bool IsExists = _empService.CheckLoginName(loginName);
                if (IsExists == true)       //用户名已经存在
                {
                    ajax.IsSuccess = IsExists;
                    ajax.Message = "用户名已经存在";
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;
        }
        #endregion

        #region 根据员工Id获取员工信息
        /// <summary>
        /// @author:wupeng
        /// @datetime:2016-07-28
        /// @desc: 根据员工Id获取员工信息
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <returns></returns>
        [WebMethod]
        public DataAssmblly.Sys_Employee GetEmpById(int employeeId)
        {

            DataAssmblly.Sys_Employee employee = _empService.GetByID(employeeId);
            employee.Sys_Department = null;
            employee.Sys_EmployeeJob = null;
            employee.Sys_EmployeeType = null;

            return new Sys_Employee
            {
                EmployeeID = employee.EmployeeID,
                EmployeeName = employee.EmployeeName,
                JobID = employee.JobID,
                DepartmentID = employee.DepartmentID,
                EmployeeTypeID = employee.EmployeeTypeID,
                Sex = employee.Sex,
                LoginName = employee.LoginName,
                //PassWord = StringControl.MD5Decrypt(employee.PassWord, skey),
                BornDate = employee.BornDate,
                TelPhone = employee.TelPhone,
                IdCard = employee.IdCard,
                ImageURL = employee.ImageURL,
                UploadImageName = employee.UploadImageName,
                ComeInDate = employee.ComeInDate
            };
        }
        #endregion

        #region 根据员工Id获取员工信息
        /// <summary>
        /// @author:wupeng
        /// @datetime:2016-07-28
        /// @desc: 根据员工Id获取员工信息
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage GetLoginName(int employeeId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";

            try
            {
                Sys_Employee employee = _empService.GetByID(employeeId);

                HHL_Employee m_employee = new HHL_Employee
                {
                    EmployeeId = employee.EmployeeID,
                    EmployeeName = employee.EmployeeName,
                    JobId = employee.JobID,
                    TypeId = employee.EmployeeTypeID,
                    LoginName = employee.LoginName,
                };
                Sys_EmployeeJob m_job = _empJobService.GetByID(m_employee.JobId);
                Sys_EmployeeType m_type = _empTypeService.GetByID(m_employee.TypeId);
                m_employee.TypeName = m_type.TypeName;
                m_employee.JobName = m_job.Jobname;

                ajax.IsSuccess = true;
                ajax.Data = m_employee;
            }
            catch
            {
                ajax.IsSuccess = false;
                ajax.Message = "系统异常,请稍候再试...";
            }
            return ajax;
        }
        #endregion

        #region 单个禁用/启用
        /// <summary>
        /// @author:wupeng
        /// @datetime:2016-07-28
        /// @desc: 单个禁用/启用
        /// </summary>
        /// <param name="TypeId">类型ID</param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage SetEmployeeSingleStatus(int EmployeeId, int index)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                if (!string.IsNullOrEmpty(EmployeeId.ToString()) && EmployeeId != 0)
                {
                    Sys_Employee m_employee = _empService.GetByID(EmployeeId);
                    if (m_employee.Status == (byte)SysStatus.Enable)
                    {
                        m_employee.Status = (byte)SysStatus.Disable;
                    }
                    else
                    {
                        m_employee.Status = (byte)SysStatus.Enable;
                    }

                    int result = _empService.Update(m_employee);
                    if (result > 0)
                    {
                        ajax.IsSuccess = true;
                        ajax.Id = EmployeeId;
                        ajax.Index = index;
                        if (m_employee.Status == (byte)SysStatus.Disable)
                        {
                            ajax.Message = "禁用成功";
                        }
                        else
                        {
                            ajax.Message = "启用成功";
                        }
                    }

                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;

        }
        #endregion

        #region 获取最后一次登录时间
        /// <summary>
        /// 获取最后一次登录时间
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage GetLastLoginTime()
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            try
            {
                int EmployeeId = LoginInfo.UserInfo.EmployeeId;
                Sys_Employee m_emp = _empService.GetByID(EmployeeId);
                if (m_emp != null)
                {
                    //条件
                    List<Expression<Func<sys_LoginLog, bool>>> parmList = new List<Expression<Func<sys_LoginLog, bool>>>();
                    parmList.Add(c => c.LoginEmployee == EmployeeId);

                    BaseService<sys_LoginLog> _logService = new BaseService<sys_LoginLog>();
                    ajax.Value = m_emp.LastLoginTime == null ? null : m_emp.LastLoginTime.ToString().ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                    ajax.Index = _logService.GetListBy(parmList).Count;
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

        #region 获取当前登录员工信息
        /// <summary>
        /// 获取当前登录员工信息
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage GetEmployeeInfo()
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            try
            {
                int EmployeeId = LoginInfo.UserInfo.EmployeeId;
                Sys_Employee m_emp = _empService.GetByID(EmployeeId);
                if (m_emp != null)
                {
                    ajax.Data = new EmployeeInfo()
                    {
                        EmployeeID = m_emp.EmployeeID,
                        LoginName = m_emp.LoginName,
                        EmployeeName = m_emp.EmployeeName,
                        Sex = m_emp.Sex == true ? "女" : "男",
                        Phone = m_emp.TelPhone,
                        TypeName = _empTypeService.GetByID(m_emp.EmployeeTypeID).TypeName,
                        JobName = _empJobService.GetByID(m_emp.JobID).Jobname,
                        CreateDate = m_emp.CreateDate,
                        BornDate = m_emp.BornDate
                    };
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


        #region 获取我管理的下级
        /// <summary>
        /// 获取我管理的下级信息
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage GetMyEmpKey()
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            try
            {
                int employeeId = LoginInfo.UserInfo.EmployeeId;
                List<EmployeeInfo> empList = _empService.GetMyMnanagerInfo(employeeId);

                ajax.IsSuccess = true;
                ajax.data = empList;
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }
            return ajax;
        }
        #endregion


        /// <summary>
        /// 核对密码
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage CheckModifyPwd(string pwd, string newPwd, string confirmPwd)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            try
            {
                int Id = LoginInfo.UserInfo.EmployeeId;
                Sys_Employee employee = _empService.GetByID(Id);
                if (employee.PassWord == pwd.MD5Hash() || employee.PassWord == pwd)         //原密码输入正确  
                {
                    if (!string.IsNullOrEmpty(newPwd) && newPwd == confirmPwd)
                    {
                        employee.PassWord = newPwd;
                        int result = _empService.Update(employee);
                        if (result > 0)
                        {
                            ajax.Message = "修改成功";
                        }
                        else
                        {
                            ajax.Message = "修改失败,请稍候再试...";
                        }
                    }
                    //修改密码
                    ajax.IsSuccess = true;
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message.ToString();
            }
            return ajax;
        }

        #endregion

        /// <summary>
        /// 修改员工信息
        /// </summary>
        [WebMethod(EnableSession = true)]
        public AjaxMessage ModifyEmpInfo(string empName, string sex, string phone)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;

            try
            {
                Sys_Employee m_emp = _empService.GetByID(LoginInfo.UserInfo.EmployeeId);
                m_emp.EmployeeName = empName;
                m_emp.Sex = sex == "1" ? true : false;
                m_emp.TelPhone = phone;
                int result = _empService.Update(m_emp);
                if (result > 0)
                {
                    ajax.IsSuccess = true;
                    ajax.Message = "修改成功";
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }
            return ajax;
        }


        #region EmployeePower

        #region 给员工添加权限
        /// <summary>
        /// @author:wupeng
        /// @datetime:2016-07-28
        /// @desc: 给员工添加权限
        /// </summary>
        /// <param name="EmployeeId">员工Id</param>
        /// <param name="channel">拼接的权限Id</param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage EmployeePowerCreate(int employeeId, string channel)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;

            try
            {
                //逗号隔开获取菜单ID
                string[] power = channel.Split(',');
                Sys_Employee employee = _empService.GetByID(employeeId);

                List<Sys_EmployeePower> powerList = _empPowerService.GetAllByEmployeeId(employeeId);
                foreach (var item in powerList)
                {
                    item.Status = (byte)SysStatus.Disable;
                    _empPowerService.Update(item);
                }

                int result = 0;

                for (int i = 0; i < power.Length; i++)
                {
                    int ChannelId = power[i].ToInt32();
                    bool IsExists = _empPowerService.CheckPower(employeeId, ChannelId);
                    Sys_Channel m_channel = _channelService.GetByID(ChannelId);

                    Sys_EmployeePower m_empPower = null;
                    if (IsExists == false)      //不存在  就添加
                    {
                        m_empPower = new Sys_EmployeePower();
                        m_empPower.Powername = m_channel.ChannelName;
                        m_empPower.EmployeeID = employeeId;
                        m_empPower.DepartmentID = employee.DepartmentID;
                        m_empPower.ChannelID = ChannelId;
                        m_empPower.UrlAddress = m_channel.ChannelAddress;
                        m_empPower.CreateDate = DateTime.Now;
                        m_empPower.Status = (byte)SysStatus.Enable;
                        m_empPower.Parent = m_channel.Parent;
                        m_empPower.Sort = m_channel.SortInt;
                        //区别 1级菜单和2级菜单
                        if (m_channel.Parent == 0)
                        {
                            m_empPower.ItemLevel = 1;
                        }
                        else
                        {
                            m_empPower.ItemLevel = 2;
                        }

                        result = _empPowerService.Insert(m_empPower);
                    }
                    else
                    {
                        m_empPower = _empPowerService.GetEmpPower(employeeId, ChannelId);
                        m_empPower.Powername = m_channel.ChannelName;
                        m_empPower.UrlAddress = m_channel.ChannelAddress;
                        m_empPower.Status = (byte)SysStatus.Enable;
                        m_empPower.Parent = m_channel.Parent;
                        m_empPower.Sort = m_channel.SortInt;
                        result = _empPowerService.Update(m_empPower);
                    }
                }

                if (result > 0)
                {
                    ajax.IsSuccess = true;
                    ajax.Message = "保存权限成功";
                }

            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;

        }
        #endregion

        #region 获取该员工启用的权限
        /// <summary>
        /// @author:wp
        /// @datetime:2016-08-03
        /// @desc: 获取启用的权限
        /// </summary>
        /// <param name="EmployeeId">员工id</param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage GetCheckPower(int EmployeeId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;

            try
            {
                //获取该员工启用的权限
                List<Sys_EmployeePower> powerList = _empPowerService.GetAllByStatus(EmployeeId, 1);
                if (powerList.Count > 0)
                {
                    ajax.Data = powerList;
                    ajax.IsSuccess = true;
                    ajax.Message = "页面加载成功";
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;
        }
        #endregion

        #endregion

        #region 获取员工信息 根据传入的拼接的客户id字符串
        /// <summary>
        /// @author:wupeng
        /// @datetime:2016-06-09
        /// @desc:获取员工信息
        /// </summary>
        [WebMethod(EnableSession = true)]
        public AjaxMessage GetEmployeeName(string employeeId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;

            try
            {
                if (!string.IsNullOrEmpty(employeeId))
                {

                    var emplist = _empService.GetEmployees(employeeId);

                    if (emplist != null)
                    {

                        //放入另一个list  否则会陷入循环引用
                        List<Sys_Employee> list = new List<Sys_Employee>();
                        list = emplist.Select(c => new Sys_Employee { EmployeeName = c.EmployeeName, EmployeeID = c.EmployeeID }).ToList();
                        ajax.IsSuccess = true;
                        ajax.Data = list;
                    }
                    else
                    {
                        ajax.IsSuccess = true;
                        ajax.Message = "没有选择客户";
                    }
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message.ToString();
            }
            return ajax;
        }
        #endregion

        #region 测试分页
        //[WebMethod]
        //public AjaxMessage CreatePager(int page)
        //{
        //    AjaxMessage ajax = new AjaxMessage();
        //    var DataList = _channelService.GetByAll();
        //    var ChannelList = new PagedList<Sys_Channel>(DataList, page, 3);
        //    ajax.Value = PagerExtensions.PagerAjax<Sys_Channel>(ChannelList);
        //    ajax.IsSuccess = true;
        //    ajax.Data = ChannelList;
        //    return ajax;

        //}
        #endregion

    }
}
