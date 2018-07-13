<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/MainMaster.Master" AutoEventWireup="true" CodeBehind="OutBoxMessage.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Message.OutBoxMessage" %>

<%@ Register Assembly="HHLWedding.ToolsLibrary" Namespace="HHLWedding.ToolsLibrary" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        a {
            cursor: pointer;
        }

            a:hover {
                text-decoration: none;
            }

        ul li {
            list-style-type: none;
            line-height: 35px;
            border-bottom: 1px solid #cbc7c7;
            text-align: left;
        }

        .spanCount {
            margin-top: 10px;
            line-height: 10px;
        }
        /*.img-message {
           width:20px;
           height:12px;
        }*/
    </style>

    <script type="text/javascript">
        //全选 所有
        function checkAll(obj) {
            $("input[name='mid']").prop("checked", $(obj).prop("checked"));
        }

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
    </script>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="UpdatePanel">
        <ContentTemplate>
            <div class="panel">
                <div class="panel-body pull-left" style="width: 25%; text-align: center;">
                    <button type="button" class="btn btn-primary btn-large" style="width: 80%; text-align: center;">写信</button>
                    <div class="panel-heading bg-white" style="margin-top: 15px;">
                        <ul>
                            <li><a>收件箱<span class="label label-warning pull-right spanCount">6</span></a></li>
                            <li><a>发件箱<span class="label label-danger pull-right spanCount">5</span></a></li>
                            <li><a>写信</a></li>
                        </ul>
                    </div>
                </div>
                <div class="panel-body pull-left" style="width: 75%;">
                    <div class="panel-heading">
                        <asp:Button runat="server" ID="btnConfirm" Text="查询" ClientIDMode="Static" CssClass="btn btn-primary" OnClick="btnConfirm_Click" Style="display: none" />
                        <a class="btn btn-info btn-sm" onclick="ConfirmDelete()">删除</a>
                        <a class="btn btn-info btn-sm" href="MessageCreate.aspx?type=2">写消息</a>
                    </div>
                    <table class="table table-bordered table-hover table-list bg-white">
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
                                    <tr>
                                        <td>
                                            <label class="checkbox-inline">
                                                <input type="checkbox" name="mid" value='<%#Eval("MessageId") %>' class="checkbox" />
                                            </label>
                                            <a href="DetailsMessage.aspx?mid=<%#Eval("MessageId") %>&sendType=<%#Eval("SendType") %>">
                                                <img class="img-message" src='<%#Eval("IsRead").ToString() == "0" ? "/images/message-noread.png" : "/images/message-read.png" %>' data-toggle="tooltip" title='<%#Eval("IsRead").ToString() == "0" ? "收件人未读" : "收件人已读" %>' /></a>
                                        </td>
                                        <td><%#GetEmployeeName(Eval("ToEmployee")) %></td>
                                        <td><a href='DetailsMessage.aspx?mid=<%#Eval("MessageId") %>&sendType=<%#Eval("SendType") %>'><%#Eval("MessageTitle") %></a></td>
                                        <td><%#Eval("SendDateTime","{0:yyyy-MM-dd HH:mm:ss}") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
