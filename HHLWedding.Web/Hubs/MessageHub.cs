using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly;
using HHLWedding.DataAssmblly.CommonModel;
using HHLWedding.BLLAssmbly.FD;

namespace HHLWedding.Web.Hubs
{
    public class MessageHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void GetNoReadMsg()
        {
            //BaseService<sm_Message> _msgService = new BaseService<sm_Message>();
            MessageService _msgService = new MessageService();
            int EmployeeId = LoginInfo.UserInfo.EmployeeId;
            int count = _msgService.GetNoReadMsg(EmployeeId);
            Clients.All.NoReadMsg(count);
        }
        public void GetMsg()
        {
        }
    }
}