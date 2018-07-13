<%@ Page Title="发件箱" Language="C#" MasterPageFile="~/AdminWorkArea/Master/MainMaster.Master" AutoEventWireup="true" CodeBehind="OutBoxMail.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Message.OutBoxMail" %>

<%@ Register Assembly="HHLWedding.ToolsLibrary" Namespace="HHLWedding.ToolsLibrary" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%--<link href="/Content/mail/bootstrap.min.css?v=3.3.6" rel="stylesheet" />--%>
    <link href="/css/font-awesome.css?v=4.4.0" rel="stylesheet" />
    <link href="/Content/mail/animate.css" rel="stylesheet" />
    <link href="/Content/mail/style.css?v=4.1.0" rel="stylesheet" />
    <link href="/Content/hhlset/toastr/toastr.css" rel="stylesheet" />

    <script src="/Scripts/jquery.signalR-2.1.2.js"></script>
    <script src="/Scripts/jquery.signalR-2.1.2.min.js"></script>
    <script src="/Signalr/Hubs"></script>


    <style type="text/css">
        #table-mail tbody tr td:first-child {
            width: 110px;
        }
    </style>

    <script type="text/javascript">
        //全选 所有
        function checkAll(obj) {
            $("input[name='mid']").prop("checked", $(obj).prop("checked"));
        }

        function ConfirmDelete() {
            var type = '<%=Request["type"] %>';

            var chk = $("input[name='mid']:checked");
            if (chk.length > 0) {
                hhl.message.confirm("你确定删除选中的信息吗?", function (result) {
                    if (result) {
                        var id = "";
                        chk.each(function () {
                            id = id + this.value + ",";
                        });
                        id = id.substr(0, id.length - 1);
                        hhl.ajax("/WebService/MessageHandler.asmx/DeleteMsg", "{'mid':'" + id + "'}", function (result) {
                            if (result.d.IsSuccess) {
                                if (type == 1) {            //删除草稿   页面重新加载
                                    this.location.reload();
                                } else {                    //删除邮件    页面异步加载
                                    hhl.notify.clear();
                                    hhl.notify.success(result.d.Message, "提示", { icon: 7 });
                                    $("#btnConfirm").click();
                                }
                            } else {
                                hhl.notify.warn(result.d.Message, "提示");
                            }
                        });

                    }
                });
            } else {
                hhl.notify.error("请选择你要删除的信息", "提示", { timeOut: 1000 });
            }

        }

        $(function () {

            //加载未读信息条数
            InBoxMessage();

        });

        //获取未读信息条数方法
        function InBoxMessage() {
            //同步加载
            $.ajax({
                async: true,
                type: "post",
                contentType: "application/json",
                url: "/WebService/MessageHandler.asmx/GetNoReadMsg",
                datatype: "json",
                success: function (result) {
                    if (result.d.IsSuccess) {
                        $(".inMessage").text(result.d.Value);
                        $("#spanDraftCount").text(result.d.Count);
                    } else {
                        //window.location.reload();
                        hhl.notify.error("系统错误");
                    }
                }
            });
        }

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
                    $(".inMessage").text(oldCount + 1);
                } else {
                    $(".inMessage").text("1");
                }
                $("#spanDraftCount").text(msg.draftCount);
            }

        }


        //阅读信息提交
        pushHub.client.readMsg = function (read) {
            //当前登录员工ID
            var employeeId = '<%=empId%>';
            if (read.employeeId == employeeId) {
                $(".inMessage").text(read.count);
                $("#spanDraftCount").text(read.draftCount);
            }
        }

        $.connection.hub.start().done(function () {

        });

        function LookDetails(mid, sendType, draft) {
            if (draft == 1) {       //草稿
                this.location.href = "MailCreate.aspx?mid=" + mid + "&type=" + sendType;
            } else {
                this.location.href = "DetailsMail.aspx?mid=" + mid + "&sendType=" + sendType;
            }
        }


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
                                <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                    <ContentTemplate>
                                        <button type="submit" class="btn btn-sm btn-primary" runat="server" onserverclick="btnConfirm_Click">
                                            搜索
                                        </button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <h2>发件箱
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
                                <button type="button" class="btn btn-danger btn-sm" data-toggle="tooltip" data-placement="top" title="删除邮件" onclick="ConfirmDelete()">
                                    <i class="fa fa-trash-o">&nbsp;&nbsp;删除</i>
                                </button>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                    <ContentTemplate>
                        <div class="mail-box">
                            <table id="table-mail" class="table table-bordered table-hover table-list bg-white ">
                                <thead>
                                    <tr class="th_titles">
                                        <th>
                                            <label class="checkbox-inline">
                                                <input type="checkbox" id="chkAll" class="checkbox" onclick="checkAll(this)" />
                                            </label>
                                            <img class="img-message" src="/images/message.png" data-toggle="tooltip" title="邮件" style="width: 24px; height: 24px" />
                                        </th>
                                        <th>收件人</th>
                                        <th>标题</th>
                                        <th>时间</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater runat="server" ID="repMessage">
                                        <ItemTemplate>
                                            <tr <%#Eval("IsRead").ToString() == "0" ? "class='unread'" : "class='read'" %>>
                                                <td>
                                                    <label class="checkbox-inline">
                                                        <input type="checkbox" name="mid" value='<%#Eval("MessageId") %>' class="checkbox" />
                                                    </label>
                                                    <a href="DetailsMail.aspx?mid=<%#Eval("MessageId") %>&sendType=<%#Eval("SendType") %>">
                                                        <img class="img-message" src='<%#Eval("IsRead").ToString() == "0" ? "/images/message-noread.png" : "/images/message-read.png" %>' data-toggle="tooltip" title='<%#Eval("IsRead").ToString() == "0" ? "收件人未读" : "收件人已读" %>' /></a>
                                                </td>
                                                <td id="td_single"><%#GetEmployeeName(Eval("ToEmployee")) %><%#Eval("IsDraft").ToString() == "1" ? "<span style='color:red;'>(草稿)</span>" : "" %>
                                                </td>
                                                <td><a onclick='LookDetails(<%#Eval("MessageId") %>,<%#Eval("SendType") %>,<%#Eval("IsDraft") %>)'><%#Eval("MessageTitle") %></a>
                                                </td>
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

</asp:Content>
