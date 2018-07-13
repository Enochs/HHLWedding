<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/MainMaster.Master" AutoEventWireup="true" CodeBehind="SaleSourceManager.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.SaleSource.SaleSourceManager" %>

<%@ Register Assembly="HHLWedding.ToolsLibrary" Namespace="HHLWedding.ToolsLibrary" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">



        //添加渠道
        function OpenCreate() {
            hhl.dialog.openUpdate(["SaleSourceCreate.aspx"], "添加渠道", {
                area: ['500px', '620px'],           //长 ，  高
            });
        }

        //编辑渠道
        function OpenEdit(Id) {
            var pages = $("#CtrPageIndex").find("tr:first").find("td").eq(1).find("span:first").html();
            hhl.dialog.openUpdate(["SaleSourceUpdate.aspx?SourceId=" + Id], "编辑渠道", {
                area: ['500px', '620px'],           //长 ，  高
            });
        }


        //禁用/启用
        function SingleStatus(sourceId) {
            hhl.ajax("/WebService/SaleSourceHandler.asmx/SetSingleStatus", '{"SaleSourceId":"' + sourceId + '" }', hhl.callbackStatus);
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="panel">
        <div class="panel-body">

            <div class="widget-box">
                <div class="widget-header">
                    <h4 class="h-title pull-left"><i class="icon-reorder"></i><b>渠道列表</b></h4>
                    <div class="box pull-right">
                        <div class="div_angle">
                            <span class="span-angel">
                                <i id="icon_angle" class="icon-angle-down"></i>
                            </span>
                        </div>
                    </div>
                </div>

                <div class="widget-content">
                    <div class="form-group col-sm-3">
                        <input type="text" class="form-control" id="txtSourceName" name="txtSourceName" placeholder="请输入渠道名称" />
                    </div>
                    <asp:UpdatePanel ID="UpdatePanelSearch" runat="server">
                        <ContentTemplate>
                            <div class="form-group col-sm-3">
                                <asp:Button runat="server" ID="btnConfirm" Text="查询" ClientIDMode="Static" CssClass="btn btn-primary" OnClick="btnConfirm_Click" Style="display: none" />
                                <button type="button" class="btn btn-info" title="搜索" data-toggle="tooltip" id="_btnSearch" runat="server" onserverclick="btnSearch_Click"><i class="icon-search"></i>&nbsp; 搜索</button>
                                <button type="button" class="btn btn-info" title="刷新" data-toggle="tooltip" id="_btnRefresh" runat="server" onclick="window.location.reload()"><i class="icon-refresh"></i>&nbsp; 刷新</button>
                                <a class="btn btn-success" title="添加渠道" data-toggle="tooltip" onclick="OpenCreate()"><i class="icon-plus"></i>&nbsp; 添加渠道</a>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
        </div>
                
            <asp:UpdatePanel ID="UpdatePanelList" runat="server">
                <ContentTemplate>
                    <div class="widget-box">
                        <table class="table table-bordered table-hover bg-white table-list">
                            <thead>
                                <tr class="th_title">
                                    <th>渠道类型</th>
                                    <th>渠道名称</th>
                                    <th>创建人</th>
                                    <th>推荐人</th>
                                    <th>创建时间</th>
                                    <th>状态</th>
                                    <th>操作</th>
                                </tr>
                            </thead>

                            <tbody>
                                <asp:Repeater runat="server" ID="repSaleSource">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%#Eval("SourceName") %></td>
                                            <td><%#GetSaleTypeName(Eval("SaleTypeId")) %></td>
                                            <td><%#GetEmployeeName(Eval("CreateEmployee")) %></td>
                                            <td><%#Eval("CommoandName") %></td>
                                            <td><%#Eval("CreateDate","{0:yyyy-MM-dd HH:mm:ss}")%></td>
                                            <td><%#GetStatusName(Eval("Status")) %></td>
                                            <td>
                                                <button type="button" class="btn btn-info btn-xs" value='<%#Eval("SourceId") %>' data-toggle="tooltip" title="编辑" onclick="OpenEdit(<%#Eval("SourceId") %>)"><i class="icon-edit"></i>&nbsp; 编辑</button>
                                                <button type="button" class="btn btn-<%#Eval("Status").ToString() == "1" ? "danger" : "success" %> btn-xs" data-toggle="tooltip" title='<%#Eval("Status").ToString() == "1" ? "点击禁用" : "点击启用" %>' value='<%#Eval("SourceId") %>' onclick="SingleStatus(<%#Eval("SourceId") %>)"><i class='<%#Eval("Status").ToString() == "1" ? "icon-off" : "icon-ok" %>'></i><%#Eval("Status").ToString() == "1" ? "&nbsp; 禁用" : "&nbsp; 启用" %></button>
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
    </section>
</asp:Content>
