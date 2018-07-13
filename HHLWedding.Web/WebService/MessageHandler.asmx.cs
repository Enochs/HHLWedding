
using HHLWedding.Web.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using HHLWedding.ToolsLibrary;
using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly;
using HHLWedding.BLLAssmbly.FD;
using System.Web.Services.Description;
using HHLWedding.EditoerLibrary;
using HHLWedding.DataAssmblly.CommonModel;
using System.Web.UI.WebControls;
using System.Linq.Expressions;

namespace HHLWedding.Web.WebService
{
    /// <summary>
    /// MessageHandler 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class MessageHandler : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        DBHelper db = new DBHelper();
        MessageService _msgService = new MessageService();
        BaseService<sm_Message> _messageService = new BaseService<sm_Message>();

        #region 写消息
        /// <summary>
        /// @author:wp
        /// @datetime:2016-09-09
        /// @desc: 写消息
        /// </summary>
        /// <param name="msg">消息实体类</param>
        /// <param name="type">type 1.写信  2.草稿</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage CreateMessage(sm_Message msg, int msgType, string msgId)
        {

            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            try
            {
                if (msg != null)
                {
                    sm_Message message = new sm_Message();
                    sm_Message draftMessage = new sm_Message();         //草稿
                    List<sm_Message> msgList = new List<sm_Message>();

                    string[] employee = msg.ToEmployee.ToString().Split(',');
                    Type type = typeof(SendTypeName);
                    var Typelist = DisplayNameExtension.GetSelectList(SendTypeName.GetMsg, type, false);
                    foreach (ListItem item in Typelist)
                    {
                        for (int i = 0; i < employee.Length; i++)
                        {
                            message = new sm_Message();
                            message.FromEmployee = LoginInfo.UserInfo.EmployeeId;
                            message.MessageContent = msg.MessageContent;
                            message.MessageTitle = msg.MessageTitle;
                            message.ToEmployee = employee[i];
                            message.SendDateTime = DateTime.Now;
                            message.IsRead = 0;     //0 未读   1 已读
                            message.SendType = item.Value.ToInt32();
                            message.IsDraft = 0;
                            message.IsGarbage = 0;
                            msgList.Add(message);
                        }
                    }

                    if (msgType == 1)       //发件
                    {
                        //删除草稿
                        if (!string.IsNullOrEmpty(msgId))
                        {
                            _msgService.DeleteMsg(msgId);
                        }
                        ////发送信息
                        int result = _msgService.InsertFroList(msgList);

                        if (result > 0)
                        {
                            ajax.Message = "发送成功";
                            ajax.IsSuccess = true;
                        }
                    }
                    else if (msgType == 2)
                    {
                        //草稿

                        if (!string.IsNullOrEmpty(msgId))
                        {
                            draftMessage = _msgService.GetByID(msgId.ToInt32());
                            draftMessage.MessageContent = msg.MessageContent;
                            draftMessage.MessageTitle = msg.MessageTitle;
                        }
                        else
                        {
                            draftMessage = message;
                        }

                        draftMessage.ToEmployee = msg.ToEmployee;
                        draftMessage.IsDraft = 1;
                        draftMessage.SendType = (byte)SendTypeName.SendMsg;

                        int result = 0;
                        if (!string.IsNullOrEmpty(msgId))       //修改
                        {
                            result = _msgService.Update(draftMessage);
                        }
                        else                //新增
                        {
                            result = _msgService.Insert(draftMessage);
                        }

                        if (result > 0)
                        {
                            ajax.Message = "保存草稿成功";
                            ajax.IsSuccess = true;
                        }
                    }


                    var d = new { employeeId = msg.ToEmployee };
                    Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<Hubs.MessageHub>().Clients.All.newMsg(d);
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message.ToString();
                ajax.IsSuccess = false;
            }
            return ajax;
        }
        #endregion

        #region 根据Id获取消息的详细信息   
        /// <summary>
        /// @author:wp
        /// @datetime:2016-09-09
        /// @desc:根据Id查询消息
        /// </summary>
        /// <param name="mid">消息id</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage GetMessage(string mid, int sendType)
        {
            Employee _empService = new Employee();
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            try
            {
                if (!string.IsNullOrEmpty(mid))
                {
                    //sm_Message m_msg = db.GetForList<sm_Message>("sm_Message", "MessageId", mid).FirstOrDefault();
                    sm_Message m_msg = _msgService.GetByID(mid.ToInt32());
                    if (m_msg != null)
                    {
                        if (sendType == 1)          //收件箱 查看 才修改状态
                        {
                            m_msg.IsRead = 1;       //收件消息  说明已读
                            _msgService.Update(m_msg);

                            sm_Message send_msg = _msgService.GetSendMsgByDate(m_msg.SendDateTime, m_msg.ToEmployee.ToInt32(), m_msg.FromEmployee.ToString().ToInt32());
                            if (send_msg != null)
                            {
                                send_msg.IsRead = 1;       //发件消息  说明已读
                                _msgService.Update(send_msg);
                            }
                        }

                        //将数据放入另外一个实体类
                        var fromEmp = _empService.GetByID(m_msg.FromEmployee);
                       
                        string toEmpName = _empService.GetMoreEmpName(m_msg.ToEmployee);
                        string fullname = _empService.GetMoreFullEmpName(m_msg.ToEmployee);

                        FD_Message msg = new FD_Message()
                        {
                            MessageId = m_msg.MessageId,
                            MessageTitle = m_msg.MessageTitle,
                            FromEmployee = m_msg.FromEmployee,
                            ToEmployee = m_msg.ToEmployee,
                            FromEmpName = fromEmp.EmployeeName,
                            FromLoginName = fromEmp.LoginName,
                            ToEmpName = toEmpName,
                            ToFullName = fullname,
                            MessageContent = m_msg.MessageContent,
                            SendDateTime = m_msg.SendDateTime,
                            IsRead = m_msg.IsRead,
                        };

                        if (msg != null)
                        {
                            ajax.Data = msg;
                            if (sendType == 2)
                            {
                                ajax.Value = _empService.GetMoreEmpName(msg.ToEmployee);
                            }
                            ajax.EmpId = msg.ToEmployee;
                            ajax.Message = "提交成功";
                            ajax.IsSuccess = true;

                        }
                        //获取收件人信息条数
                        int count = _msgService.GetNoReadMsg(LoginInfo.UserInfo.EmployeeId);        //未读数量
                        int allCount = _msgService.GetAllMessage(LoginInfo.UserInfo.EmployeeId, 1).Count;       //所有收件数量
                        int draftCount = _msgService.GetDraftCount(LoginInfo.UserInfo.EmployeeId);      //草稿数量
                        var d = new { employeeId = LoginInfo.UserInfo.EmployeeId, count = count, allCount = allCount, draftCount = draftCount };
                        Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<Hubs.MessageHub>().Clients.All.readMsg(d);
                    }
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message.ToString();
                ajax.IsSuccess = false;
            }
            return ajax;
        }
        #endregion

        #region 删除消息
        /// <summary>
        /// @author:wp
        /// @datetime:2016-09-14
        /// @desc: 删除消息
        /// </summary>
        /// <param name="mid">拼接的消息Id</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage DeleteMsg(string mid)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            try
            {
                if (!string.IsNullOrEmpty(mid))
                {
                    int result = _msgService.DeleteMsg(mid);

                    if (result > 0)
                    {
                        ajax.Message = "删除成功";
                        ajax.IsSuccess = true;
                    }

                    //获取收件人信息条数
                    int count = _msgService.GetNoReadMsg(LoginInfo.UserInfo.EmployeeId);
                    int allCount = _msgService.GetAllMessage(LoginInfo.UserInfo.EmployeeId, 1).Count;
                    var d = new { employeeId = LoginInfo.UserInfo.EmployeeId, count = count, allCount = allCount };
                    Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<Hubs.MessageHub>().Clients.All.readMsg(d);
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message.ToString();
                ajax.IsSuccess = false;
            }
            return ajax;
        }
        #endregion

        #region 上一封
        /// <summary>
        /// @author:wp
        /// @datetime:2016-09-14
        /// @desc: 上一封
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage PreMsg(string mid, int sendType)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;

            try
            {
                if (!string.IsNullOrEmpty(mid))
                {
                    var m_msg = _msgService.GetByID(mid.ToInt32());
                    sm_Message msg = null;
                    if (sendType == 1)
                    {
                        msg = _msgService.GetPreMsg(mid.ToInt32(), m_msg.ToEmployee.ToInt32(), sendType);
                    }
                    else
                    {
                        msg = _msgService.GetPreMsg(mid.ToInt32(), m_msg.FromEmployee.ToString().ToInt32(), sendType);
                    }
                    if (msg != null)
                    {
                        ajax.Data = msg;
                        ajax.IsSuccess = true;
                    }
                    else
                    {
                        ajax.Data = null;
                        ajax.Message = "没有数据";
                        ajax.IsSuccess = true;
                    }
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message.ToString();
                ajax.IsSuccess = false;
            }

            return ajax;
        }
        #endregion

        #region 下一封
        /// <summary>
        /// 下一封
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage NextMsg(string mid, int sendType)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;

            try
            {
                if (!string.IsNullOrEmpty(mid))
                {
                    var m_msg = _msgService.GetByID(mid.ToInt32());
                    sm_Message msg = null;
                    if (sendType == 1)
                    {
                        msg = _msgService.GetNextMsg(mid.ToInt32(), m_msg.ToEmployee.ToInt32(), sendType);
                    }
                    else
                    {
                        msg = _msgService.GetNextMsg(mid.ToInt32(), m_msg.FromEmployee.ToString().ToInt32(), sendType);
                    }
                    if (msg != null)
                    {
                        ajax.Data = msg;
                        ajax.IsSuccess = true;
                    }
                    else
                    {
                        ajax.Data = null;
                        ajax.Message = "没有数据";
                        ajax.IsSuccess = true;
                    }
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message.ToString();
                ajax.IsSuccess = false;
            }

            return ajax;
        }
        #endregion

        #region 删除之后   页面显示
        /// <summary>
        /// @author:wp
        /// @datetime:2016-09-18
        /// @desc: 删除之后  页面显示
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage GetLaterMsg(string mid, int sendType)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;

            try
            {
                if (!string.IsNullOrEmpty(mid))
                {
                    var m_msg = _msgService.GetByID(mid.ToInt32());
                    sm_Message msg = null;
                    //获取下一条消息
                    if (sendType == 1)
                    {
                        msg = _msgService.GetNextMsg(mid.ToInt32(), m_msg.ToEmployee.ToInt32(), sendType);
                    }
                    else
                    {
                        msg = _msgService.GetNextMsg(mid.ToInt32(), m_msg.FromEmployee.ToString().ToInt32(), sendType);
                    }
                    //如果有消息  直接返回
                    if (msg != null)
                    {
                        ajax.Data = msg;
                        ajax.IsSuccess = true;
                    }
                    else
                    {
                        //没有下一条消息   读取上一条消息
                        if (sendType == 1)
                        {
                            msg = _msgService.GetPreMsg(mid.ToInt32(), m_msg.ToEmployee.ToInt32(), sendType);
                        }
                        else
                        {
                            msg = _msgService.GetPreMsg(mid.ToInt32(), m_msg.FromEmployee.ToString().ToInt32(), sendType);
                        }
                        //有上一条消息
                        if (msg != null)
                        {
                            ajax.Data = msg;
                            ajax.IsSuccess = true;
                        }
                        else
                        {
                            //上一条消息也没有  直接返回null
                            ajax.Data = null;
                            ajax.Message = "没有数据";
                            ajax.IsSuccess = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message.ToString();
                ajax.IsSuccess = false;
            }
            if (ajax.IsSuccess)
            {
                //获取收件人信息条数
                int count = _msgService.GetNoReadMsg(LoginInfo.UserInfo.EmployeeId);        //未读数量
                int allCount = _msgService.GetAllMessage(LoginInfo.UserInfo.EmployeeId, 1).Count;       //所有收件数量
                int draftCount = _msgService.GetDraftCount(LoginInfo.UserInfo.EmployeeId);      //草稿数量
                var d = new { employeeId = LoginInfo.UserInfo.EmployeeId, count = count, allCount = allCount, draftCount = draftCount };
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<Hubs.MessageHub>().Clients.All.readMsg(d);
            }
            return ajax;
        }
        #endregion

        #region 获取个人的未读信息
        /// <summary>
        /// 获取某个人的未读信息
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage GetNoReadMsg()
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = string.Empty;
            ajax.Value = "0";
            try
            {
                int EmployeeId = LoginInfo.UserInfo.EmployeeId;
                int count = _msgService.GetNoReadMsg(EmployeeId);
                //收件人所有信息条数
                int allCount = _msgService.GetAllMessage(LoginInfo.UserInfo.EmployeeId, 1).Count;
                var d = new { employeeId = LoginInfo.UserInfo.EmployeeId, count = count, allCount = allCount };
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<Hubs.MessageHub>().Clients.All.readMsg(d);
                int draftCount = _msgService.GetDraftCount(LoginInfo.UserInfo.EmployeeId);

                ajax.Value = count.ToString();
                ajax.Index = allCount;
                ajax.Count = draftCount;
                ajax.IsSuccess = true;
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;

        }

        #endregion

        #region 标记重要邮件
        /// <summary>
        /// 标记垃圾邮件
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="mid">type 1.垃圾邮件   2.标记已读</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage SetMsgInfo(string mid, int type)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                if (!string.IsNullOrEmpty(mid))
                {
                    List<Expression<Func<sm_Message, bool>>> pars = new List<Expression<Func<sm_Message, bool>>>();
                    pars.Add(c => mid.Contains(c.MessageId.ToString()));
                    var DataList = _messageService.GetListBy(pars);

                    sm_Message msg = new sm_Message();
                    string prop = "IsGarbage";
                    bool isSuccess = false;
                    if (type == 1)
                    {
                        if (DataList.Where(c => c.IsGarbage == 0).Count() > 0)
                        {
                            msg.IsGarbage = 1;
                            isSuccess = true;
                        }
                    }
                    else
                    {
                        if (DataList.Where(c => c.IsRead == 0).Count() > 0)
                        {
                            msg.IsRead = 1;
                            prop = "IsRead";
                            isSuccess = true;
                        }
                    }
                    //正常标记
                    if (isSuccess)
                    {
                        Expression<Func<sm_Message, bool>> where = c => mid.Contains(c.MessageId.ToString());
                        string[] property = new string[] { prop }; ;
                        int result = _messageService.ModifyBy(msg, where, property);
                        if (result > 0)
                        {
                            ajax.IsSuccess = true;
                            ajax.Message = "标记成功";
                        }
                        else
                        {
                            ajax.Message = "标记失败,请稍候再试...";
                        }
                    }
                    else
                    {
                        ajax.IsSuccess = true;
                        ajax.Message = "标记成功";
                    }

                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            if (ajax.IsSuccess)
            {
                //获取收件人信息条数
                int count = _msgService.GetNoReadMsg(LoginInfo.UserInfo.EmployeeId);
                int allCount = _msgService.GetAllMessage(LoginInfo.UserInfo.EmployeeId, 1).Count;
                var d = new { employeeId = LoginInfo.UserInfo.EmployeeId, count = count, allCount = allCount };
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<Hubs.MessageHub>().Clients.All.readMsg(d);
            }
            return ajax;
        }
        #endregion


    }
}
