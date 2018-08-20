<%@ Page Title="用户登录" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/AdminWorkArea/Main/css/H-ui.min.css" rel="stylesheet" type="text/css" />
    <link href="/AdminWorkArea/Main/css/H-ui.login.css" rel="stylesheet" type="text/css" />
    <link href="/AdminWorkArea/Main/css/style.css" rel="stylesheet" type="text/css" />
    <link href="/AdminWorkArea/Main/lib/Hui-iconfont/1.0.1/iconfont.css" rel="stylesheet" type="text/css" />

    <!--jquery文件-->
    <script type="text/javascript" src="/Scripts/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="/Scripts/jquery-1.10.2.min.js"></script>

    <!--弹出层   提示 样式文件-->
    <link href="/Content/hhlset/layer/skin/layer.css" rel="stylesheet" />
    <link href="/Content/hhlset/toastr/toastr.css" rel="stylesheet" />
    <link href="/Content/hhlset/toastr/toastr.min.css" rel="stylesheet" />

    <!--弹出层   提示 js文件-->
    <script src="/Content/hhlset/toastr/toastr.js"></script>
    <script src="/Content/hhlset/layer/layer.js"></script>
    <%--    <script src="/Content/hhlset/toastr/toastr.min.js"></script>--%>

    <!--提示  扩展的js文件-->
    <script type="text/javascript" src="/Scripts/hhl.js"></script>

    <style type="text/css">
        #divLoading {
            position: absolute;
            text-align: center;
            margin: -5px 0px -10px 0px;
            height: 30px;
            z-index: 2;
        }
    </style>

    <script type="text/javascript">
        //点击登录
        function login() {
            $(".imgLoading").show();
            if (check()) {
                var IpAddress = '<%=Ip %>';
                hhl.ajax("/WebService/Login.asmx/EmpLogin", '{"loginName":"' + $("#loginName").val() + '","passWord":"' + $("#passWord").val() + '","Ip":"' + IpAddress + '"}', function (result) {

                    if (result.d.IsSuccess) {
                        SaveState();
                        $("#btnButton").click();
                    } else {
                        hhl.notify.warn(result.d.Message, "提示");
                        $(".imgLoading").hide();
                    }
                });
            }
        }

        //用户名和密码验证
        function check() {
            $(".imgLoading").show();
            var loginName = $("#loginName").val();
            var passWord = $("#passWord").val();

            if (loginName == "") {
                hhl.notify.warn("请输入用户名", "提示");
                $(".imgLoading").hide();
                return false;
            } else if (passWord == "") {
                hhl.notify.warn("请输入密码", "提示");
                $(".imgLoading").hide();
                return false;
            } else {
                return true;
            }
        }


        $(function () {
            //获取cookie值
            $(".imgLoading").hide();

            var loginName = '<%=loginName %>';
            var pwd = '<%=Pwd %>';
            if (loginName != "" && pwd != "") {
                $("#loginName").val(loginName);
                $("#passWord").val(pwd);
                $("#online").prop("checked", "checked")
            }


            //页面按下回车键提交
            $("body").keypress(function (e) {

                //if (e.keyCode) {
                //    if (e.keyCode == 13)
                //        login();
                //}
            });

            //点击文字 选中复选框 (记住密码)
            $("#lblOnline").click(function () {
                var chk = $("#ChkLoginState").is(":checked");
                if (chk) {
                    $("#ChkLoginState").removeProp("checked");
                } else {
                    $("#ChkLoginState").prop("checked", "checked");
                }
            });


        });



        //记住帐号
        function SaveState() {
            var loginName = $("#loginName").val();
            var passWord = $("#passWord").val();
            hhl.notify.clear();
            var checkState = $("#online").prop("checked");      //获取选中状态

            hhl.ajax("/WebService/Login.asmx/SaveLoginCookie", '{"loginName":"' + loginName + '","passWord":"' + passWord + '","state":"' + checkState + '"}', function (result) {
                if (result.d.IsSuccess) {

                } else {
                    hhl.notify.warn(result.d.Message, "提示");
                    $(".imgLoading").hide();
                    return false;
                }
            });
        }



    </script>


</head>
<body>
    <form runat="server" class="form form-horizontal" method="post" onsubmit="return check()">
        <input type="hidden" id="hideSuccess" />
        <input type="hidden" id="TenantId" name="TenantId" value="" />
        <div class="header"></div>
        <div class="loginWraper">
            <div id="loginform" class="loginBox">

                <div class="row cl" style="z-index: 0;">
                    <label class="form-label col-3"><i class="Hui-iconfont">&#xe60d;</i></label>
                    <div class="formControls col-8">
                        <input id="loginName" name="loginName" type="text" placeholder="账户" class="input-text size-L" />
                    </div>
                </div>
                <div id="divLoading" class="row cl">
                    <img class="imgLoading" src="../images/loading.gif" alt="加载中" />
                </div>
                <div class="row cl" style="z-index: 0;">
                    <label class="form-label col-3"><i class="Hui-iconfont">&#xe60e;</i></label>
                    <div class="formControls col-8">
                        <input id="passWord" name="passWord" type="password" placeholder="密码" class="input-text size-L" />
                    </div>
                </div>
                <div class="row">
                    <div class="formControls col-8 col-offset-3">
                        <%-- <label for="online">
                            <input type="checkbox" name="online" id="online" value="" />
                            使我保持登录状态
                        </label>--%>
                        <label for="online" id="lblOnline">
                            <asp:CheckBox ClientIDMode="Static" runat="server" ID="ChkLoginState" />使我保持登录状态</label>
                    </div>
                </div>
                <div class="row">
                    <div class="formControls col-6 col-offset-3" style="text-indent: 10%;">
                        <asp:Button OnClick="btnLogin_Click" runat="server" CssClass="btn btn-success radius size-L" Text="&nbsp;登&nbsp;&nbsp;&nbsp;&nbsp;录&nbsp;" />
                        <%--<input name="" type="button" onclick="login()" runat="server" class="btn btn-success radius size-L" value="&nbsp;登&nbsp;&nbsp;&nbsp;&nbsp;录&nbsp;" />--%>
                        <input name="" type="reset" class="btn btn-default radius size-L" value="&nbsp;取&nbsp;&nbsp;&nbsp;&nbsp;消&nbsp;" />
                    </div>
                </div>



            </div>

        </div>
        <div class="footer">Copyright 你的公司名称 by H-ui.admin.v2.3</div>

    </form>
</body>
</html>
