<%@ Page Title="频道列表" Language="C#" MasterPageFile="~/AdminWorkArea/Master/MainMaster.Master" AutoEventWireup="true" CodeBehind="Sys_ChannelList.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Channel.Sys_ChannelList" %>

<%@ Register Assembly="HHLWedding.ToolsLibrary" Namespace="HHLWedding.ToolsLibrary" TagPrefix="cc1" %>

<%@ Import Namespace="HHLWedding.DataAssmblly.CommonModel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        ul li {
            list-style: none;
            font-size: 14px;
            line-height: 40px;
            width: 280px;
            /*border: 1px solid red;*/
            text-align: center;
            float: left;
        }

        table:first-child {
            border-bottom: none;
        }
    </style>
    <script type="text/javascript">
        angle.isMore = false;
        //全部选中
        function checkAll(obj) {
            $("input[name='cid']").prop("checked", $(obj).prop("checked"));
        }

        //禁用/启用
        function SetSingleEnable(cid, index) {
            hhl.ajax("/WebService/ChannelHandler.asmx/SetSingleStatus", '{"ChannelId":"' + cid + '","index":"' + index + '" }', hhl.callbackStatus);
        }

        //编辑频道
        var isSuccess = false;
        function OpenEdit(Id) {
            var pages = $("#CtrPageIndex").find("tr:first").find("td").eq(1).find("span:first").html();
            hhl.dialog.openUpdate(["Sys_ChannelEdit.aspx?ChannelId=" + Id, "no"], "编辑频道", {
                area: ['360px', '490px'],           //长 ，  高
                btn: ["确定", "取消"],
            });
        }

        //添加频道
        function OpenCreate() {
            var pages = $("#CtrPageIndex").find("tr:first").find("td").eq(1).find("span:first").html();
            hhl.dialog.openUpdate(["Sys_ChannelCreate.aspx", "no"], "添加频道", {
                area: ['360px', '490px'],           //长 ，  高
                btn: ["确定", "取消"],
            });
        }

        //批量修改状态
        function SetPartStatus(type) {

            var cid = '';
            var chk = $("input[name='cid']:checked");
            if (chk.length > 0) {
                $("input[name='cid']:checked").each(function () {
                    cid = cid + this.value + ",";
                });
                cid = cid.substring(0, cid.lastIndexOf(','));

                hhl.ajax("/WebService/ChannelHandler.asmx/SetPartStatus", '{"ChannelId":"' + cid + '","type":"' + type + '" }', hhl.callbackStatus);
            } else {
                hhl.notify.error("请选择渠道进行操作", "提示");
            }

        }

        //点击切换视图
        function viewChange() {

            var icon = $("#btnChange").find("i");
            if ($("#viewList").hasClass("hide")) {
                icon.addClass("icon-list").removeClass("icon-list-alt");
                $("#viewList").removeClass("hide")
                $("#pageList").addClass("hide");

            } else {
                icon.addClass("icon-list-alt").removeClass("icon-list");
                $("#viewList").addClass("hide")
                $("#pageList").removeClass("hide");

            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel">
        <div class="panel-body">
            <div class="widget-box">
                <div class="widget-header">
                    <h4 class="h-title pull-left"><i class="icon-reorder"></i><b>频道列表</b></h4>
                    <div class="box pull-right">
                        <div class="div_angle">
                            <span class="span-angel">
                                <i id="icon_angle" class="icon-angle icon-angle-up"></i>
                            </span>
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="widget-content">
                            <div class="form-group col-sm-3">
                                <asp:TextBox runat="server" ID="txtChannelName" CssClass="form-control" placeholder="请输入频道名称" />
                            </div>
                            <div class="form-group col-sm-3">
                                <asp:DropDownList runat="server" ID="ddlAllParentSystem" ClientIDMode="Static" CssClass="form-control ddlSelChange" onchange="ddlChange()"></asp:DropDownList>
                            </div>

                            <div class="form-group col-sm-6">
                                <asp:Button runat="server" ID="btnConfirm" Text="查询" ClientIDMode="Static" CssClass="btn btn-primary" OnClick="btnConfirm_Click" Style="display: none" />
                                <button type="button" class="btn btn-info" title="搜索" data-toggle="tooltip" id="_btnSearch" runat="server" onserverclick="btnSearch_Click"><i class="icon-search"></i>&nbsp; 搜索</button>
                                <button type="button" class="btn btn-info" title="刷新" data-toggle="tooltip" id="_btnRefresh" runat="server" onclick="window.location.reload()"><i class="icon-refresh"></i>&nbsp; 刷新</button>
                                <a class="btn btn-success" title="添加菜单" data-toggle="tooltip" onclick="OpenCreate()"><i class="icon-plus"></i>&nbsp; 添加菜单</a>
                                <a class="btn btn-danger" title="禁用" data-toggle="tooltip" onclick="SetPartStatus(2)"><i class="icon-off">&nbsp; 禁用</i></a>
                                <a class="btn btn-success" title="启用" data-toggle="tooltip" onclick="SetPartStatus(1)"><i class="icon-ok">&nbsp; 启用</i></a>

                                <button type="button" class="btn btn-info" id="btnChange" onclick="viewChange()"><i class="icon-list-alt">&nbsp; 切换视图</i></button>
                            </div>

                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div id="pageList" class="widget-box" style="background-color: #F1F2F9;">
                        <table id="tbl_Channel" class="table table-bordered table-hover bg-white table-list">
                            <thead>
                                <tr class="th_title">
                                    <th>
                                        <label class="checkbox-inline">
                                            <input type="checkbox" name="chkAll" onclick="checkAll(this)" />全选
                                        </label>
                                    </th>
                                    <th>频道名称</th>
                                    <th>父级频道</th>
                                    <th>url地址</th>
                                    <th>创建时间</th>
                                    <th>状态</th>
                                    <th>操作</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater runat="server" ID="repChannel" ClientIDMode="Static" OnItemCommand="repChannel_ItemCommand">
                                    <ItemTemplate>
                                        <tr id='<%#Eval("ChannelId") %>'>
                                            <td style="height: 28px; text-align: center;">
                                                <label class="checkbox-inline">
                                                    <input type="checkbox" name="cid" value='<%#Eval("ChannelId") %>' onclick="checkId(this)" />
                                                </label>
                                            </td>
                                            <td><%#Eval("ChannelName") %></td>
                                            <td><%#GetParentName(Eval("Parent")) %></td>
                                            <td><%#Eval("ChannelAddress") %></td>
                                            <td><%#Eval("CreateDate","{0:yyyy-MM-dd}") %></td>
                                            <td class="td_content">
                                                <a class="btn btn-<%#Eval("Status").ToString() == "1" ? "success" : "danger" %> btn-xs" onclick='SetSingleEnable(<%#Eval("ChannelId") %>,9)' title="<%#Eval("Status").ToString() == "1" ? "启用" : "禁用" %> " data-toggle="tooltip">
                                                    <%#Eval("Status").ToString() == "1" ? "<i class='icon-ok'></i>" : "<i class='icon-minus-sign'></i>" %>
                                                </a>
                                            </td>
                                            <td>
                                                <button type="button" name="btn_status" onclick='SetSingleEnable(<%#Eval("ChannelId") %>,9)' class="btn btn-<%#Eval("Status").ToString() == "1" ? "danger btn-xs" : "success btn-xs" %>" <%#Eval("Status").ToString() == "1" ? "title='禁用'" : "title='启用'" %> data-toggle="tooltip">
                                                    <%#Eval("Status").ToString() == "1" ? "<i name='icon_status' id='icon_status' class='icon-off'></i>&nbsp; 禁用" : "<i name='icon_status' id='icon_status' class='icon-ok' title='启用'></i>&nbsp; 启用" %>
                                                </button>
                                                <a class="btn btn-info btn-xs" onclick='OpenEdit(<%#Eval("ChannelId") %>)' data-original-title="编辑频道" data-toggle="tooltip"><i class="icon-edit"></i>&nbsp; 修改</a>
                                                <a href='<%#Eval("ChannelAddress") %>' target="_blank">访问</a>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>

                        <cc1:AspNetPagerTool ID="CtrPageIndex" ClientIDMode="Static" runat="server" OnPageChanged="CtrPageIndex_PageChanged"></cc1:AspNetPagerTool>
                        <div class="form-group">
                            <span>每页显示</span>
                            <asp:DropDownList runat="server" ID="ddlPageSize" OnSelectedIndexChanged="CtrPageIndex_PageChanged" AutoPostBack="true">
                                <asp:ListItem Text="10" Value="1"></asp:ListItem>
                                <asp:ListItem Text="20" Value="2"></asp:ListItem>
                                <asp:ListItem Text="50" Value="3"></asp:ListItem>
                            </asp:DropDownList>
                            <span>条</span>
                        </div>
                    </div>

                    <div id="viewList" class="widget-box hide">
                        <table id="tblChannel" class="table table-bordered bg-white" style="width: 100%; padding: 0px 0px;">
                            <asp:Repeater runat="server" ID="repParent" OnItemDataBound="repParent_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td width="5%" style="vertical-align: middle;"><%#Container.ItemIndex+1 %></td>
                                        <td width="10%" style="vertical-align: middle;"><%#Eval("ChannelName") %></td>
                                        <td>
                                            <asp:HiddenField runat="server" ID="HideParentId" Value='<%#Eval("ChannelID") %>' />
                                            <ul>
                                                <asp:Repeater runat="server" ID="repChannel">
                                                    <ItemTemplate>
                                                        <li><%#Eval("ChannelName") %> </li>
                                                    </ItemTemplate>
                                                </asp:Repeater>

                                            </ul>

                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>

                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
