<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/MainMaster.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Main.Index" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="css/H-ui.min.css" rel="stylesheet" />
    <link href="css/H-ui.admin.css" rel="stylesheet" />
    <link href="/AdminWorkArea/Main/lib/Hui-iconfont/1.0.1/iconfont.css" rel="stylesheet" type="text/css" />
    <link href="/AdminWorkArea/Main/skin/blue/skin.css" rel="stylesheet" id="skin" />

    <!--之所以屏蔽  是会影响tooltip-->
    <%--    <script src="lib/jquery/1.9.1/jquery.min.js"></script>
    <script src="lib/layer/1.9.3/layer.js"></script>--%>
    <script src="js/H-ui.admin.js" type="text/javascript"></script>
    <script src="js/H-ui.js"></script>
    <script src="/Scripts/jquery.signalR-2.1.2.js"></script>
    <script src="/Scripts/jquery.signalR-2.1.2.min.js"></script>
    <script src="/Signalr/Hubs"></script>

    <script type="text/javascript">

        //重写__doPostBack
        function __doPostBack(eventTarget, eventArgument) {
            var theform = document.form1;

            if (theform.__EVENTTARGET != null) {
                theform.__EVENTTARGET.value = eventTarget;
            }

            if (theform.__EVENTARGUMENT != null) {
                theform.__EVENTARGUMENT.value = eventArgument;
            }
            theform.submit();
        }


        //修改密码 弹出窗体
        function openModify() {
            hhl.dialog.openUpdate(["/AdminWorkArea/Sys/EmployeeInfo/UpdatePassWord.aspx", "no"], "修改密码", {
                area: ["440px", "420px"],
            });
        }

        $(function () {

            //注销
            $(".a_singOut").click(function () {
                __doPostBack('btnLook', '1');
            });

            //读取登录人姓名和类型
            var employeeId = '<%=empId%>';
            hhl.ajax("/WebService/EmployeeHandler.asmx/GetLoginName", '{"employeeId":"' + employeeId + '"}', function (result) {
                if (result.d.IsSuccess) {
                    $("#empType").text(result.d.Data.TypeName);
                    $("#userName").text(result.d.Data.EmployeeName);
                }
            });

            //实时时间
            setInterval(function () {
                var time = jsonDateFormat(1);
                $(".spanTime").text(time);
            }, 1000)

            var day = new Array("日", "一", "二", "三", "四", "五", "六");

            var weekDay = new Date().getDay();

            $(".spanWeek").text("星期" + day[weekDay]);

            var pushHub = $.connection.messageHub;

            //有人发信息提交
            pushHub.client.newMsg = function (msg) {
                //当前登录员工ID
                var employeeId = ',' + <%=empId%> + ',';
                //所有收信人ID
                var mEmpId = ',' + msg.employeeId + ',';

                if (mEmpId.indexOf(employeeId) >= 0) {

                    //数字验证
                    var oldCount = parseInt(message.innerText);
                    var regs = /^[0-9]*$/;
                    if (regs.test(oldCount)) {
                        message.innerText = (oldCount + 1);
                    } else {
                        message.innerText = "1";
                    }
                }

            }


            //阅读信息提交
            pushHub.client.readMsg = function (read) {
                //当前登录员工ID
                var employeeId = '<%=empId%>';
                if (read.employeeId == employeeId) {
                    message.innerText = read.count;
                }
            }

            $.connection.hub.start().done(function () {

            });
        });
    </script>




</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <header class="Hui-header cl">
        <a class="Hui-logo l" title="重庆好婚礼后台管理系统" href="Index.aspx">
            <img src="images/logo.png" />
        </a>


        <ul class="ulTime radius">
            <li><span class="spanTime">2016年10月10日 17:07:56</span><span class="spanWeek"></span></li>
        </ul>


        <ul class="Header-bar">
            <li><a class="a_header" title="个人信息" _href="/AdminWorkArea/Sys/EmployeeInfo/DetailsEmployee.aspx" href="javascript:void(0)">个人信息</a></li>

            <li><a title="修改密码" onclick="openModify()">修改密码</a></li>

            <li><a class="a_singOut">退出</a></li>
        </ul>

        <ul class="Hui-userbar">
            <li><span id="empType" class="span_empType">超级管理员</span></li>
            <li class="dropDown dropDown_hover"><a id="userName" href="#" class="dropDown_A">admin</a>
                <ul class="dropDown-menu radius box-shadow">
                    <li><a class="a_header" title="个人信息" _href="/AdminWorkArea/Sys/EmployeeInfo/DetailsEmployee.aspx" href="javascript:void(0)">个人信息</a></li>
                    <li><a class="a_singOut">切换账户</a></li>
                    <li><a id="home" onclick="SetHome(window.location)">设为主页</a></li>
                    <li><a class="a_singOut">退出</a></li>
                </ul>
            </li>
            <li id="Hui-msg">
                <a class="a_header" _href="/AdminWorkArea/Message/InBoxMail.aspx" href="javascript:void(0)" title="收件箱">
                    <span id="message" class="badge badge-danger">0</span>
                    <i class="Hui-iconfont" style="font-size: 18px">&#xe68a;</i>
                </a>
            </li>
            <li id="Hui-skin" class="dropDown right dropDown_hover"><a href="javascript:;" title="换肤"><i class="Hui-iconfont" style="font-size:18px">&#xe62a;</i></a>
			<ul class="dropDown-menu radius box-shadow">
				<li><a href="javascript:;" data-val="blue" title="默认（蓝色）">默认（蓝色）</a></li>
				<li><a href="javascript:;" data-val="default" title="黑色">黑色</a></li>
				<li><a href="javascript:;" data-val="green" title="绿色">绿色</a></li>
				<li><a href="javascript:;" data-val="red" title="红色">红色</a></li>
				<li><a href="javascript:;" data-val="yellow" title="黄色">黄色</a></li>
				<li><a href="javascript:;" data-val="orange" title="绿色">橙色</a></li>
			</ul>
		</li>
        </ul>

        <a aria-hidden="false" class="Hui-nav-toggle" href="#"></a>
    </header>


    <aside class="Hui-aside">
        <input runat="server" id="divScrollValue" type="hidden" value="" />
        <div class="menu_dropdown bk_2">
            <asp:Repeater ID="RepChannel" runat="server" OnItemDataBound="RepChannel_ItemDataBound">
                <ItemTemplate>
                    <dl id="menu-<%#Eval("PowerID") %>">
                        <dt><i class="Hui-iconfont">&#xe616;</i> <%#Eval("PowerName") %><i class="Hui-iconfont menu_dropdown-arrow">&#xe6d5;</i>
                            <asp:HiddenField runat="server" ID="hidekey" Value='<%#Eval("ChannelID") %>' />
                        </dt>
                        <dd>
                            <ul>
                                <asp:Repeater ID="repSecond" runat="server">
                                    <ItemTemplate>
                                        <li><a _href="<%#Eval("UrlAddress") %>" href="javascript:void(0)"><%#Eval("PowerName") %></a></li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </dd>
                    </dl>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </aside>
   <div class="dislpayArrow"><a class="pngfix" href="javascript:void(0);" onclick="displaynavbar(this)"></a></div>

    <section class="Hui-article-box">
        <div id="Hui-tabNav" class="Hui-tabNav">
            <div class="Hui-tabNav-wp">
                <ul id="min_title_list" class="acrossTab cl">
                    <li><span title="我的桌面" data-href="welcome.html">我的桌面</span><em></em></li>
                </ul>
            </div>
            <div class="Hui-tabNav-more btn-group"><a id="js-tabNav-prev" class="btn radius btn-default size-S" href="javascript:;"><i class="Hui-iconfont">&#xe6d4;</i></a><a id="js-tabNav-next" class="btn radius btn-default size-S" href="javascript:;"><i class="Hui-iconfont">&#xe6d7;</i></a></div>
        </div>
        <div id="iframe_box" class="Hui-article">
            <div class="show_iframe">
                <div style="display: none" class="loading"></div>
                <iframe src="welcome.html"></iframe>
                <%--<iframe src="../Sys/EmployeeInfo/DetailsEmployee.aspx"></iframe>--%>
            </div>
        </div>
    </section>
</asp:Content>
