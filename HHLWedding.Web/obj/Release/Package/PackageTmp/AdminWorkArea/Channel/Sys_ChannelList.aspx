<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/MainMaster.Master" AutoEventWireup="true" CodeBehind="Sys_ChannelList.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Channel.Sys_ChannelList" %>

<%@ Register Assembly="HHLWedding.ToolsLibrary" Namespace="HHLWedding.ToolsLibrary" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        table:first-child {
            border-bottom: none;
        }
    </style>
    <script type="text/javascript">
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

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="crumb-wrap wrap-Title">
        <i class="Hui-iconfont">&#xe67f;</i> 首页 
                <span class="c-gray en">&gt;</span> 频道管理 
                <span class="c-gray en">&gt;</span> 频道列表 
                <a class="btn btn-success radius r mr-20" href="javascript:location.replace(location.href);" data-toggle="tooltip" title="刷新"><i class="icon-refresh"></i></a>
    </div>

    <div class="panel">
        <div class="widget-box">
            <div class="widget-header">
                <h4 class="h-title pull-left"><i class="icon-reorder"></i><b>频道列表</b></h4>
                <div class="box pull-right">
                    <div class="div_angle">
                        <span class="span-angel">
                            <i id="icon_angle" class="icon-angle-down"></i>
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
                            <asp:DropDownList runat="server" ID="ddlAllParentSystem" ClientIDMode="Static" CssClass="form-control ddlSelChange" onchange="ddlChange()" ></asp:DropDownList>
                        </div>

                        <div class="form-group col-sm-3">
                            <asp:Button runat="server" ID="btnConfirm" Text="查询" ClientIDMode="Static" CssClass="btn btn-primary" OnClick="btnConfirm_Click" Style="display: none" />
                            <button type="button" class="btn btn-info" title="搜索" data-toggle="tooltip" id="_btnSearch" runat="server" onserverclick="btnSearch_Click"><i class="icon-search"></i>&nbsp; 搜索</button>
                            <button type="button" class="btn btn-info" title="刷新" data-toggle="tooltip" id="_btnRefresh" runat="server" onclick="window.location.reload()"><i class="icon-refresh"></i>&nbsp; 刷新</button>
                            <a class="btn btn-success" title="添加菜单" data-toggle="tooltip" onclick="OpenCreate()"><i class="icon-plus"></i>&nbsp; 添加菜单</a>
                        </div>

                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="widget-box" style="background-color: #F1F2F9;">
                    <table id="tbl_Channel" class="table table-bordered table-hover bg-white table-list">
                        <thead>
                            <tr class="th_title">
                                <th>
                                    <label class="checkbox-inline">
                                        <input type="checkbox" name="chkAll" onclick="checkAll(this)" />
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
                        <tbody style="height: 28px;">
                            <asp:Repeater runat="server" ID="repChannel" ClientIDMode="Static" OnItemCommand="repChannel_ItemCommand">
                                <ItemTemplate>
                                    <tr id='<%#Eval("ChannelId") %>' style="height: 28px;">
                                        <td style="height: 28px;">
                                            <label class="checkbox-inline">
                                                <input type="checkbox" name="cid" value='<%#Eval("ChannelId") %>' />
                                            </label>
                                        </td>
                                        <td><%#Eval("ChannelName") %></td>
                                        <td><%#GetParentName(Eval("Parent")) %></td>
                                        <td><%#Eval("ChannelAddress") %></td>
                                        <td><%#Eval("CreateDate","{0:yyyy-MM-dd}") %></td>
                                        <td><span style="font-weight: bolder; <%#Eval("Status").ToString() == "1" ? "color:green;": "color:red;" %>"><%#GetStatusName(Eval("Status")) %></span></td>
                                        <td>
                                            <button type="button" name="btn_status" onclick='SetSingleEnable(<%#Eval("ChannelId") %>,9)' class="btn btn-<%#Eval("Status").ToString() == "1" ? "danger btn-xs" : "success btn-xs" %>" data-toggle="tooltip">
                                                <%#Eval("Status").ToString() == "1" ? "<i name='icon_status' id='icon_status' class='icon-off'></i>&nbsp; 禁用" : "<i name='icon_status' id='icon_status' class='icon-ok'></i>&nbsp; 启用" %>
                                            </button>
                                            <a class="btn btn-info btn-xs" onclick='OpenEdit(<%#Eval("ChannelId") %>)' data-original-title="编辑频道" data-toggle="tooltip"><i class="icon-edit"></i>&nbsp; 修改</a>
                                        </td>
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
    <style type="text/css">
        .table tbody tr {
            height: 25px;
        }

            .table tbody tr td {
                height: 25px;
            }
    </style>
</asp:Content>
