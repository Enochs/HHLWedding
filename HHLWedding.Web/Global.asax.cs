using HHLWedding.ToolsLibrary;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI;

namespace HHLWedding.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
        }


        void Application_End(object sender, EventArgs e)
        {
            //  在应用程序关闭时运行的代码
            Session.Abandon();
        }



        /// <summary>
        /// 提交时发生
        /// </summary>
        /// <param name="sernder"></param>
        /// <param name="e"></param>
        void Application_BeginRequest(object sernder, EventArgs e)
        {


        }
    }
}
