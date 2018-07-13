<%@ Page Title="收件箱" Language="C#" MasterPageFile="~/AdminWorkArea/Master/MainMaster.Master" AutoEventWireup="true" CodeBehind="InBoxMail.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Message.InBoxMail" %>

<%@ Register Assembly="HHLWedding.ToolsLibrary" Namespace="HHLWedding.ToolsLibrary" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%--<link href="/Content/mail/bootstrap.min.css?v=3.3.6" rel="stylesheet" />--%>
    <link href="/css/font-awesome.css?v=4.4.0" rel="stylesheet" />
    <link href="/Content/mail/style.css?v=4.1.0" rel="stylesheet" />
    <link href="/Content/hhlset/toastr/toastr.css" rel="stylesheet" />
    <script src="/Scripts/jquery.signalR-2.1.2.js"></script>
    <script src="/Scripts/jquery.signalR-2.1.2.min.js"></script>
    <script src="/Signalr/Hubs"></script>
    <style type="text/css">
        a {
            cursor: pointer;
        }

            a:hover {
                text-decoration: none;
            }
    </style>

    <script type="text/javascript">
        //全选 所有
        function checkAll(obj) {
            $("input[name='mid']").prop("checked", $(obj).prop("checked"));
        }

        //删除消息
        function ConfirmDelete() {
            var chk = $("input[name='mid']:checked");
            if (chk.length > 0) {
                hhl.message.confirm("你确定删除选中的信息吗?", function (result) {
                    if (result) {
                        var id = "";
                        chk.each(function () {
                            id = id + this.value + ",";
                        });
                        id = id.substr(0, id.length - 1);
                        console.log(id);
                        hhl.ajax("/WebService/MessageHandler.asmx/DeleteMsg", "{'mid':'" + id + "'}", hhl.callbackStatus);
                    }
                });
            } else {
                hhl.notify.error("请选择你要删除的信息", "提示", { timeOut: 1000 });
            }

        }

        $(function () {

            //垃圾箱页面
            var type = '<%=Request["type"] %>';
            if (type != "") {
                $("h2").text("垃圾箱");
                $("#btnGarbage").hide();
                $("#btnRead").hide();
            }
        });

            //标为垃圾邮件/已读
            function setMsgInfo(type) {
                var mid = "";
                var chk = $("#tbody tr").find("input[name='mid']:checked");
                if (chk.length > 0) {
                    chk.each(function () {
                        mid = mid + this.value + ",";
                    });
                    mid = mid.substring(0, mid.length - 1);

                    hhl.ajax("/WebService/MessageHandler.asmx/SetMsgInfo", '{"mid":"' + mid + '","type":"' + type + '"}', hhl.callbackStatus);
                } else {
                    hhl.notify.error("请选择你要标记的邮件", "提示");
                }
            }
    </script>

    <!--实时更新消息-->
    <script type="text/javascript">

        //页面加载
        $(function () {
            InBoxMessage();
        });

        //获取未读信息条数方法
        function InBoxMessage() {
            //加载
            $.ajax({
                async: true,
                type: "post",
                contentType: "application/json",
                url: "/WebService/MessageHandler.asmx/GetNoReadMsg",
                datatype: "json",
                success: function (result) {
                    if (result.d.IsSuccess) {
                        $(".inMessage").text(result.d.Value);
                        var allCount = result.d.Index;
                        $(".allCounts").text("共" + allCount + "封,其中未读消息");
                        $("#spanDraftCount").text(result.d.Count);
                    } else {
                        hhl.notify.error("系统错误");
                    }
                }
            });
        }


        var pushHub = $.connection.messageHub;

        //有人发送信息
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
                    $(".inMessage").text(oldCount + 1);
                } else {
                    $(".inMessage").text("1");
                }

                var allCount = msg.allCount;
                $(".allCounts").text("共" + allCount + "封,其中未读消息");
                //草稿箱
                $("#spanDraftCount").text(msg.draftCount);
            }

        }


        //阅读信息
        pushHub.client.readMsg = function (read) {
            //当前登录员工ID
            var employeeId = '<%=empId%>';
            if (read.employeeId == employeeId) {
                $(".inMessage").text(read.count);
                $("#spanDraftCount").text(read.draftCount);
            }
            var allCount = read.allCount;
            $(".allCounts").text("共" + allCount + "封,其中未读消息");
        }

        $.connection.hub.start().done(function () { });


    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="wrapper wrapper-content">
        <div class="row">
            <div class="col-sm-3">
                <div class="ibox float-e-margins">
                    <div class="ibox-content mailbox-content">
                        <div class="file-manager">
                            <a class="btn btn-block btn-primary compose-mail" href="MailCreate.aspx">写信</a>
                            <div class="space-25"></div>
                            <h5>文件夹</h5>
                            <ul class="folder-list m-b-md" style="padding: 0">
                                <li>
                                    <a href="InBoxMail.aspx"><i class="fa fa-inbox "></i>收件箱 <span id="message" class="label label-warning pull-right inMessage">0</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="OutBoxMail.aspx"><i class="fa fa-envelope-o"></i>发信</a>
                                </li>
                                <li>
                                    <a href="OutBoxMail.aspx?type=1"><i class="fa fa-file-text-o"></i>草稿 <span id="spanDraftCount" class="label label-danger pull-right">0</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="InBoxMail.aspx?type=1"><i class="fa fa-trash-o"></i>重要邮件</a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="mail-outBox" class="col-sm-9 animated fadeInRight">

                <div class="mail-box-header">

                    <div class="pull-right mail-search">
                        <div class="input-group">
                            <input type="text" class="form-control input-sm" id="search" name="search" placeholder="搜索邮件标题，正文等" />
                            <div class="input-group-btn">
                                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                    <ContentTemplate>
                                        <button type="submit" class="btn btn-sm btn-primary" runat="server" onserverclick="btnConfirm_Click">
                                            搜索
                                        </button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <h2>收件箱 (<span class="allCounts"></span><span class="inMessage">0</span>条)
                    </h2>
                    <div class="mail-tools tooltip-demo m-t-md">
                        <div class="btn-group pull-right">
                            <button class="btn btn-white btn-sm">
                                <i class="fa fa-arrow-left"></i>
                            </button>
                            <button class="btn btn-white btn-sm">
                                <i class="fa fa-arrow-right"></i>
                            </button>

                        </div>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                            <ContentTemplate>
                                <asp:Button runat="server" ID="btnConfirm" Text="查询" ClientIDMode="Static" CssClass="btn btn-primary" OnClick="btnConfirm_Click" Style="display: none" />
                                <button class="btn btn-primary btn-sm" data-toggle="tooltip" data-placement="left" title="刷新邮件列表"><i class="icon icon-refresh"></i>&nbsp; 刷新</button>
                                <button id="btnRead" type="button" class="btn btn-success btn-sm" data-toggle="tooltip" data-placement="top" title="标为已读" onclick="setMsgInfo(2)">
                                    <i class="fa fa-eye">&nbsp;&nbsp;标为已读</i>
                                </button>
                                <button id="btnGarbage" type="button" class="btn btn-info btn-sm" data-toggle="tooltip" data-placement="top" title="标为重要邮件" onclick="setMsgInfo(1)">
                                    <i class="fa fa-exclamation">&nbsp;&nbsp;重要邮件</i>
                                </button>
                                <button type="button" class="btn btn-danger btn-sm" data-toggle="tooltip" data-placement="top" title="删除邮件" onclick="ConfirmDelete()">
                                    <i class="fa fa-trash-o">&nbsp;&nbsp;删除</i>
                                </button>

                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <asp:UpdatePanel runat="server" ID="panel1">
                        <ContentTemplate>
                            <div class="mail-box">
                                <table id="tbl-mail" class="table table-bordered table-hover table-list">
                                    <thead>
                                        <tr class="th_titles">
                                            <th>
                                                <label class="checkbox-inline">
                                                    <input type="checkbox" id="chkAll" class="checkbox" onclick="checkAll(this)" />
                                                </label>
                                                <img class="img-message" src="/images/message.png" data-toggle="tooltip" title="邮件" style="width: 24px; height: 24px" />
                                            </th>
                                            <th width="20px" style="text-align: center;"><span><i class="icon icon-info-sign btn-large" data-toggle="tooltip" title="是否是重要邮件"></i></span></th>
                                            <th>发件人</th>
                                            <th>标题</th>
                                            <th>时间</th>
                                        </tr>
                                    </thead>
                                    <tbody id="tbody">
                                        <asp:Repeater runat="server" ID="repMessage">
                                            <ItemTemplate>
                                                <tr id="tr_<%#Eval("MessageId") %>" <%#Eval("IsRead").ToString() == "0" ? "class='unread'" : "class='read'" %>>
                                                    <td>
                                                        <label class="checkbox-inline">
                                                            <input type="checkbox" name="mid" value='<%#Eval("MessageId") %>' class="checkbox" />
                                                        </label>
                                                        <a href="DetailsMail.aspx?mid=<%#Eval("MessageId") %>&sendType=<%#Eval("SendType") %>">
                                                            <img class="img-message" src='<%#Eval("IsRead").ToString() == "0" ? "/images/message-noread.png" : "/images/message-read.png" %>' data-toggle="tooltip" title='<%#Eval("IsRead").ToString() == "0" ? "消息未读" : "消息已读" %>' />
                                                        </a>

                                                    </td>
                                                    <td><i class='<%#Eval("IsGarbage").ToString() == "0" ? "" : "icon icon-warning-sign" %> pull-right' data-toggle="tooltip" title="重要邮件"></i></td>
                                                    <td><%#GetEmployeeName(Eval("FromEmployee")) %></td>
                                                    <td><a href='DetailsMail.aspx?mid=<%#Eval("MessageId") %>&sendType=<%#Eval("SendType") %>'><%#Eval("MessageTitle") %></a></td>
                                                    <td><%#Eval("SendDateTime","{0:yyyy-MM-dd HH:mm:ss}") %></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                                 <cc1:AspNetPagerTool ID="CtrPageIndex" ClientIDMode="Static" runat="server" OnPageChanged="CtrPageIndex_PageChanged"></cc1:AspNetPagerTool>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
