using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web;
 


namespace HHLWedding.ToolsLibrary
{
    public static class JavaScriptTools
    {
        public static void AlterMessage(string Message, Page page)
        {
            
        }

        #region 隐藏权限控件
        
        /// <summary>
        /// 隐藏页面控件
        /// </summary>
        /// <param name="PageType"></param>
        public static void ChecksPageControl(string PageType,System.Web.UI.Page ObjPage)
        {
            //UserJurisdiction ObjUserJurisdictionBLL = new UserJurisdiction();
            ///// <summary>
            ///// 用户控件权限
            ///// </summary>
            //JurisdictionforButton ObjJurisdictionforButtonBLL = new JurisdictionforButton();
            //var ObjControlList = ObjJurisdictionforButtonBLL.GetByChannel(ObjPage.User.Identity.Name.ToInt32(), PageType);
            //int Index = 0;
            //string openScript = "\n\t ";
            //Controls ObjControls = new Controls();
            //foreach (var Objitem in ObjControlList)
            //{
            //    //openScript += "$("#ff").hide();"// "$(#\"" + ObjControls.GetByID(Objitem.ControlID).ConrolKey + "\").hide();\n\t ";
            //    openScript += "$('#" + ObjControls.GetByID(Objitem.ControlID).ConrolKey + "').hide();";
            //    Index++;
            //}
            //System.Web.UI.ClientScriptManager OjbClientScript = ObjPage.ClientScript;
            //OjbClientScript.RegisterStartupScript(ObjPage.GetType(), "hideControl", openScript, true);
            //string openScript = "\n\t ";
            //openScript = openScript + "alert('A');\n\t";
            //System.Web.UI.ClientScriptManager OjbClientScript = ObjPage.ClientScript;
            //OjbClientScript.RegisterStartupScript(ObjPage.GetType(), "AlertAndClosefancybox", openScript, true);
        }

        /// <summary>
        /// 锁定数据编辑
        /// </summary>
        /// <param name="PageType"></param>
        /// <param name="ObjPage"></param>
        public static void LockData(string PageType, System.Web.UI.Page ObjPage)
        {
            //UserJurisdiction ObjUserJurisdictionBLL = new UserJurisdiction();
            //Channel ObjChannelBLL = new Channel();

            //string ScriptString = string.Empty;
            //var ObjModel = ObjUserJurisdictionBLL.GetByChannelType(PageType, ObjPage.User.Identity.Name.ToInt32());
            //if (ObjModel != null)
            //{
            //    switch (ObjModel.DataPower)
            //    {
            //        case 1:
            //            ScriptString = "$('.MyData" + ObjPage.User.Identity.Name.ToInt32() + "').show();";
            //            break;
            //        case 2:
            //            ScriptString = string.Empty;
            //            break;
            //        case 3:
            //            ScriptString = "$('.AllData').hide();\n\t $('.MyData" + ObjPage.User.Identity.Name.ToInt32() + "').hide();";
            //            break;
            //    }
            //}
            //System.Web.UI.ClientScriptManager OjbClientScript = ObjPage.ClientScript;
            //OjbClientScript.RegisterStartupScript(ObjPage.GetType(), "LockData", ScriptString, true);
            //JurisdictionforButton ObjJurisdictionforButtonBLL = new JurisdictionforButton();
            //var ObjControlList = ObjJurisdictionforButtonBLL.GetByChannel(ObjPage.User.Identity.Name.ToInt32(), PageType);

        }
        #endregion
        #region 常规对话框
        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <param name="ProcessResult">显示的提示文字</param>
        /// <param name="page">页面类</param>
        public static void AlertWindow(string ProcessResult, Page page)
        {
            string openScript = "\n\t ";
            openScript = openScript + "alert('" + ProcessResult + "');\n\t";
            System.Web.UI.ClientScriptManager OjbClientScript = page.ClientScript;
            OjbClientScript.RegisterStartupScript(page.GetType(), "AlertAndClosefancybox", openScript, true);
        }

        

        public static void AlertWindowAndGoLocationHref(string ProcessResult, Page page)
        {
            string openScript = "\n\t ";
            openScript = openScript + "alert('" + ProcessResult + "');\n\t";

            openScript = openScript + "  window.location.href = 'OngoingInvite.aspx?NeedPopu=1';\n\t";
            System.Web.UI.ClientScriptManager OjbClientScript = page.ClientScript;
            OjbClientScript.RegisterStartupScript(page.GetType(), "AlertWindowAndGoLocationHref", openScript, true);
        }


        /// <summary>
        /// 提示信息 并且跳转到指定页面
        /// </summary>
        /// <param name="ProcessResult"></param>
        /// <param name="URI"></param>
        /// <param name="page"></param>
        public static void AlertWindowAndLocation(string ProcessResult,string URI, Page page)
        {
            string openScript = "\n\t ";
            openScript = openScript + "alert('" + ProcessResult + "');\n\t";

            openScript = openScript + "  window.location.href = '"+URI+"';\n\t";
            System.Web.UI.ClientScriptManager OjbClientScript = page.ClientScript;
            OjbClientScript.RegisterStartupScript(page.GetType(), "AlertWindowAndLocation", openScript, true);
        }





        /// <summary>
        /// 打开新窗口
        /// </summary>
        /// <param name="URI"></param>
        /// <param name="page"></param>
        public static void OpenWindown(string URI,Page page)
        {
            string openScript = string.Empty;
            //openScript = openScript + "alert('" + ProcessResult + "');\n\t";
           
            openScript = openScript + "  window.open('"+URI+"');\n\t";
            System.Web.UI.ClientScriptManager OjbClientScript = page.ClientScript;
            OjbClientScript.RegisterStartupScript(page.GetType(), "OpenWindown", openScript, true);
        }

        public static void AlertWindowAndReaload(string ProcessResult, Page page)
        {
            string openScript = "\n\t ";
            openScript = openScript + "alert('" + ProcessResult + "');\n\t parent.location.reload();";
            System.Web.UI.ClientScriptManager OjbClientScript = page.ClientScript;
            OjbClientScript.RegisterStartupScript(page.GetType(), "AlertAndClosefancybox", openScript, true);
        }
        /// <summary>
        /// 运行页面JS方法
        /// </summary>
        /// <param name="functionName">方法名 exp:  test(1)</param>
        /// <param name="page">页面类</param>
        public static void DoPageFunction(string functionName, Page page)
        {
            string openScript = "\n\t ";
            System.Web.UI.ClientScriptManager OjbClientScript = page.ClientScript;
            OjbClientScript.RegisterStartupScript(page.GetType(), "AlertAndClosefancybox", openScript, true);

        }
 

        /// <summary>
        /// 弹出提示并关闭fancybox
        /// </summary>
        /// <param name="ProcessResult">提示</param>
        /// <param name="page"></param>
        public static void AlertAndClosefancybox1(string ProcessResult, Page page)
        {
            string openScript = "\n\t ";
            openScript = openScript + "alert('" + ProcessResult + "');parent.parent.$.fancybox.close(1);\n\t";
            System.Web.UI.ClientScriptManager OjbClientScript = page.ClientScript;
            OjbClientScript.RegisterStartupScript(page.GetType(), "AlertAndClosefancybox", openScript, true);
        }

        /// <summary>
        /// 重载弹出对话框
        /// </summary>
        /// <param name="ProcessResult">显示的提示文字</param>
        /// <param name="cReturnUrl">返回地址</param>
        /// <param name="page">页面类</param>
        public static void AlertWindow(string ProcessResult, string cReturnUrl, Page page)
        {
            string openScript = "";
            openScript = openScript
               + "\r\n "
               + "alert('" + ProcessResult + "');"
               + "\r\n "
               + "location.href('" + cReturnUrl + "');"
               + "\r\n ";
            System.Web.UI.ClientScriptManager OjbClientScript = page.ClientScript;
            OjbClientScript.RegisterStartupScript(page.GetType(), "AlertAndClosefancybox", openScript, true);
        }


        /// <summary>
        /// 重载弹出对话框
        /// </summary>
        /// <param name="ProcessResult">显示的提示文字</param>
        /// <param name="cReturnUrl">返回地址</param>
        /// <param name="page">页面类</param>
        public static void ConfirmWindow(string ProcessResult, string cReturnUrl, Page page)
        {
            string openScript = "";
            openScript = openScript
               + "\r\n "
               + "if(confirm('" + ProcessResult + "')){"
               + "\r\n "
               + "location.href('" + cReturnUrl + "')};"
               + "\r\n ";
            System.Web.UI.ClientScriptManager OjbClientScript = page.ClientScript;
            OjbClientScript.RegisterStartupScript(page.GetType(), "AlertAndClosefancybox", openScript, true);
        }


        /// <summary>
        /// 弹出一个确认对话框,并再点击后重新加载当前页面
        /// </summary>
        /// <param name="ProcessResult">显示的文本</param>
        /// <param name="page">页面类</param>
        public static void AlertWindowAndReload(string ProcessResult, Page page)
        {
            string openScript = "";
            openScript = openScript
               + "\r\n "
               + "alert('" + ProcessResult + "');"
               + "\r\n "
               + "location.reload(True);"
               + "\r\n ";
            System.Web.UI.ClientScriptManager OjbClientScript = page.ClientScript;
            OjbClientScript.RegisterStartupScript(page.GetType(), "AlertAndClosefancybox", openScript, true);
        }

        /// <summary>
        /// 弹出对话框,然后关闭当前窗口
        /// </summary>
        /// <param name="ProcessResult">显示的提示文字</param>
        /// <param name="page">页面类</param>
        public static void CloseWindow(string ProcessResult, Page page)
        {
            string openScript = "";
            openScript = openScript
              + "\r\n "
              + "alert('" + ProcessResult + "');\r\n"
              + "window.close();window.opener.location.reload();\r\n";
 
            System.Web.UI.ClientScriptManager OjbClientScript = page.ClientScript;
            OjbClientScript.RegisterStartupScript(page.GetType(), "WindowCloseWindow", openScript, true);
        }
        /// <summary>
        /// 注册JS代码，并运行
        /// </summary>
        /// <param name="jsString"></param>
        /// <param name="page"></param>
        public static void RegisterJsCodeSource(string jsString, Page page)
        {
            System.Web.UI.ClientScriptManager OjbClientScript = page.ClientScript;
            OjbClientScript.RegisterStartupScript(page.GetType(), "RegisterJsCodeSource", jsString, true);
        
        }
        /// <summary>
        /// 弹出窗体
        /// </summary>
        /// <param name="PageUrl">窗体的地址</param>
        /// <param name="Title">显示的标题</param>
        /// <param name="Width">窗体的宽度</param>
        /// <param name="Height">窗体的高度</param>
        /// <param name="page">页面类</param>
        public static void PopUpWindow(string PageUrl, string Title, int Width, int Height, Page page)
        {
            string openScript = "";
            openScript = openScript
               + "\r\n "
               + "window.open('" + PageUrl + "','" + Title + "','width=" + Width.ToString() + ",height=" + Height.ToString() + ",scrollbars=yes')"
                 + "\r\n ";
            System.Web.UI.ClientScriptManager OjbClientScript = page.ClientScript;
            OjbClientScript.RegisterStartupScript(page.GetType(), "AlertAndClosefancybox", openScript, true);
        }
        /// <summary>
        /// 弹出窗体
        /// </summary>
        /// <param name="PageUrl">窗体的地址</param>
        /// <param name="Title">显示的标题</param>
        /// <param name="Width">窗体的宽度</param>
        /// <param name="Height">窗体的高度</param>
        /// <param name="page">页面类</param>
        public static void PopUpWindowModalDialog(string PageUrl, string Title, int Width, int Height, Page page)
        {

            string openScript = "";
            openScript = openScript
               + "\r\n "
               + "window.showModalDialog('" + PageUrl + "','" + Title + "','dialogWidth:" + Width.ToString() + "px;dialogHeight:" + Height.ToString() + "px;Scroll=no;status=no')"
                 + "\r\n ";
            System.Web.UI.ClientScriptManager OjbClientScript = page.ClientScript;
            OjbClientScript.RegisterStartupScript(page.GetType(), "AlertAndClosefancybox", openScript, true);
        }
  

        /// <summary>
        /// 弹出提示并关闭fancybox
        /// </summary>
        /// <param name="ProcessResult">提示</param>
        /// <param name="page"></param>
        public static void AlertAndClosefancybox(string ProcessResult, Page page)
        {
            string openScript = "\n\t ";
            openScript = openScript + "alert('" + ProcessResult + "');parent.window.location.href = parent.window.location.href;parent.$.fancybox.close(1);\n\t";
            System.Web.UI.ClientScriptManager OjbClientScript = page.ClientScript;
            OjbClientScript.RegisterStartupScript(page.GetType(),"AlertAndClosefancybox",openScript, true);
        }

        /// <summary>
        /// 直接关闭fancybox
        /// </summary>
        /// <param name="ProcessResult">提示</param>
        /// <param name="page"></param>
        public static void AlertAndClosefancybox(Page page)
        {
            string openScript = "\n\t ";
            openScript = openScript + "parent.window.location.href = parent.window.location.href;parent.$.fancybox.close(1);\n\t";
            System.Web.UI.ClientScriptManager OjbClientScript = page.ClientScript;
            OjbClientScript.RegisterStartupScript(page.GetType(), "AlertAndClosefancybox", openScript, true);
        }


        /// <summary>
        /// 弹出提示并关闭fancybox
        /// </summary>
        /// <param name="Url">Url</param>
        /// <param name="page"></param>
        public static void AlertAndCloseJumpfancybox(string Url, Page page)
        {
            string openScript = "\n\t ";
            openScript = openScript + "parent.window.location.href ='" + Url + "';parent.$.fancybox.close(1);\n\t";
            System.Web.UI.ClientScriptManager OjbClientScript = page.ClientScript;
            OjbClientScript.RegisterStartupScript(page.GetType(), "AlertAndClosefancybox", openScript, true);
        }
        /// <summary>
        /// 弹出提示并关闭fancybox 但不刷新父级页面
        /// </summary>
        /// <param name="ProcessResult">提示</param>
        /// <param name="page"></param>
        public static void AlertAndClosefancyboxNoRenovate(string ProcessResult, Page page)
        {
            string openScript = "\n\t ";
            openScript = openScript + "alert('" + ProcessResult + "');parent.$.fancybox.close(1);\n\t";
            System.Web.UI.ClientScriptManager OjbClientScript = page.ClientScript;
            OjbClientScript.RegisterStartupScript(page.GetType(), "AlertAndClosefancybox", openScript, true);
        }


        /// <summary>
        /// 对父窗体内的控件赋值 无需回调控件CLICK
        /// </summary>
        /// <param name="ControlKey"></param>
        /// <param name="page"></param>
        public static void  SetValueByParentControl(string ControlKey,string values, Page page)
        {
            string openScript = "\n\t ";
            openScript = openScript + "parent.$(\"#" + ControlKey + "\").attr(\"value\",\"" + values + "\");\n\t parent.$.fancybox.close(1);";
            System.Web.UI.ClientScriptManager OjbClientScript = page.ClientScript;
            OjbClientScript.RegisterStartupScript(page.GetType(), "SetValueByParentControl", openScript, true);
        }        /// <summary>
 


        /// <summary>
        /// 向页面输出指定的JS代码
        /// </summary>
        /// <param name="Script"></param>
        public static void ResponseScript(string Script, Page page)
        {
            System.Web.UI.ClientScriptManager OjbClientScript = page.ClientScript;
            OjbClientScript.RegisterStartupScript(page.GetType(), "ResponseScript", Script, true);
        }

        /// <summary>
        /// 对父窗体内的控件赋值  需回调控件CLICK
        /// </summary>
        /// <param name="ControlKey"></param>
        /// <param name="page"></param>
        public static void SetValueByParentControl(string ControlKey, string values,string CallBackid, Page page)
        {
            string openScript = "\n\t ";
            openScript = openScript + "parent.$(\"#" + ControlKey + "\").attr(\"value\",\"" + values + "\");\n\t parent.$(\"#" + CallBackid + "\").click();\n\t parent.$.fancybox.close(1);";
            System.Web.UI.ClientScriptManager OjbClientScript = page.ClientScript;
            OjbClientScript.RegisterStartupScript(page.GetType(), "SetValueByParentControl", openScript, true);
        }

        public static void ClickParentControl(string ProcessResult, string CallBackid, Page page)
        {
            string openScript = "\n\t ";
            openScript = openScript + "alert('" + ProcessResult + "');\n\t parent.$(\"#" + CallBackid + "\").click();\n\t parent.$.fancybox.close(1);";
            System.Web.UI.ClientScriptManager OjbClientScript = page.ClientScript;
            OjbClientScript.RegisterStartupScript(page.GetType(), "SetValueByParentControl", openScript, true);
        }


        /// <summary>
        /// 弹出提示并关闭fancybox
        /// </summary>
        /// <param name="ProcessResult">提示</param>
        /// <param name="page"></param>
        /// <param name="urlPar">url参数</param>
        public static void AlertAndClosefancybox(string ProcessResult, Page page,string urlPar)
        {
            string openScript = "\n\t "; 
            openScript = openScript + "alert('" + ProcessResult + "');parent.$.fancybox.close(1);parent.window.location.href = parent.window.location.href" + urlPar + ";\n\t";
            System.Web.UI.ClientScriptManager OjbClientScript = page.ClientScript;
            OjbClientScript.RegisterStartupScript(page.GetType(), "AlertAndClosefancybox", openScript, true);
        }

        /// <summary>
        /// 弹出提示并关闭fancybox，刷新父页面，并跳转到父页面指定页数。
        /// </summary>
        /// <param name="message">弹出提示信息。</param>
        /// <param name="pageIndex">父窗体可用 pageindex 来获取通过 URL 传递过来的值。</param>
        /// <param name="page">页面类对象。</param>
        public static void AlertAndClosefancyboxGoPageIndex(string message, string pageIndex, Page page)
        {
            string script = @"function setUrlParam(url, param, v) {
            var re = new RegExp('(\\\?|&)' + param + '=([^&]+)(&|$)', 'i');
            var m = url.match(re);
            if (m) {
                return (url.replace(re, function ($0, $1, $2) { return ($0.replace($2, v)); }));
            }
            else {
                if (url.indexOf('?') == -1)
                    return (url + '?' + param + '=' + v);
                else
                    return (url + '&' + param + '=' + v);
            }
            }
            alert('" + message + "'); parent.window.location.href=setUrlParam(parent.window.location.href, 'pageindex', " + pageIndex + ");parent.$.fancybox.close(1);";
            page.ClientScript.RegisterStartupScript(page.GetType(), "AlertAndClosefancyboxWithPageIndex", script, true);
        }
        #endregion
    }
}
