<%@ Page Title="客户管理" Language="C#" MasterPageFile="~/AdminWorkArea/Master/MainMaster.Master" AutoEventWireup="true" CodeBehind="FL_CustomerManager.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Flow.Customer.FL_CustomerManager" %>

<%@ Register Assembly="HHLWedding.ToolsLibrary" Namespace="HHLWedding.ToolsLibrary" TagPrefix="cc1" %>
<%@ Register Assembly="HHLWedding.Control" Namespace="HHLWedding.Control" TagPrefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        //单个 禁用/启用
        function SetSingleEnable(customerId) {
            hhl.ajax('/WebService/Flow/CustomerHandler.asmx/ModifySingleStatus', { cusId: customerId }, hhl.callbackStatus, 1);
        }

        //批量导入客户
        function ImportCustomer() {
            hhl.dialog.open(["ImportCustomer.aspx", "no"], "导入客户模版", {
                area: ["100%", "100%"],
                btn: ["导入客户", "取消"],
                maxmin: false,
                end: function () {
                    $("#btnConfirm").click();
                    hhl.notify.clear();
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel">
        <div class="panel-body">
            <div class="widget-box">
                <div class="widget-header">
                    <h4 class="h-title pull-left"><i class="icon-reorder"></i><b>客户列表</b></h4>
                    <div class="box pull-right">
                        <div class="div_angle">
                            <span class="span-angel">
                                <i id="icon_angle" class="icon-angle icon-angle-up"></i>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="widget-content">
                    <div class="form-group col-sm-2">
                        <input type="text" class="form-control" name="txtName" placeholder="请输入新人姓名" />
                    </div>
                    <div class="form-group col-sm-2">
                        <input type="text" class="form-control" name="txtPhone" placeholder="请输入联系电话" />
                    </div>
                    <div class="form-group col-sm-2">
                        <cc2:ddlState runat="server" ID="ddlState" ClientIDMode="Static" onchange="ddlChange()" />
                    </div>

                    <div class="form-group col-sm-2">
                        <cc2:ddlHotel runat="server" ID="ddlHotel" ClientIDMode="Static"  onchange="ddlChange()" />
                    </div>
                    <div class="form-group col-sm-4">
                        <asp:UpdatePanel runat="server" ID="updateSearch">
                            <ContentTemplate>
                                <asp:Button runat="server" ID="btnConfirm" Text="查询" ClientIDMode="Static" CssClass="btn btn-primary" OnClick="btnConfirm_Click" Style="display: none" />
                                <button type="button" class="btn btn-info" title="搜索" data-toggle="tooltip" id="_btnSearch" runat="server" onserverclick="btnConfirm_Click"><i class="icon-search"></i>&nbsp; 搜索</button>
                                <button type="button" class="btn btn-info" title="刷新" data-toggle="tooltip" id="_btnRefresh" runat="server" onclick="window.location.reload()"><i class="icon-refresh"></i>&nbsp; 刷新</button>
                                <a class="btn btn-success" title="添加客户" data-toggle="tooltip" href="FL_CustomerCreate.aspx?type=add"><i class="icon-plus"></i>&nbsp; 添加客户</a>
                                <a class="btn btn-info" title="导入客户" data-toggle="tooltip" onclick="ImportCustomer()"><i class="glyphicon glyphicon-import"></i>&nbsp; 导入客户</a>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <div class="widget-box">
                <asp:UpdatePanel runat="server" ID="updateCustomerList">
                    <ContentTemplate>
                        <table class="table table-bordered table-hover bg-white">
                            <thead>
                                <tr class="th_title">
                                    <th>姓名</th>
                                    <th>联系电话</th>
                                    <th>婚期</th>
                                    <th>酒店</th>
                                    <th>沟通进度</th>
                                    <th>电销</th>
                                    <th>渠道类型</th>
                                    <th>渠道</th>
                                    <th>状态</th>
                                    <th>操作</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater runat="server" ID="repCustomer">
                                    <ItemTemplate>
                                        <tr class="th_content">
                                            <td><a href='FL_CustomerDetails.aspx?CustomerId=<%#Eval("CustomerID") %>' style="color: gray;" target="_blank"><%#GetCustomerName(Eval("CustomerID")) %></a></td>
                                            <td><%#Eval("ContactPhone") %></td>
                                            <td><%#Eval("PartyDate","{0:yyyy-MM-dd}") %></td>
                                            <td><%#GetHotelName(Eval("Hotel")) %></td>
                                            <td><%#GetCustomerState(Eval("State")) %></td>
                                            <td><%#GetEmployeeName(Eval("InviteEmployee")) %></td>
                                            <td><%#GetSaleTypeName(Eval("SaleType")) %></td>
                                            <td><%#GetSaleSourceName(Eval("Channel")) %></td>
                                            <td>
                                                <a title='<%#Eval("Status").ToString() =="1" ? "启用" : "禁用" %>' data-toggle="tooltip" class="btn btn-<%#Eval("Status").ToString() == "1" ? "success" : "danger" %> btn-xs" onclick="SetSingleEnable('<%#Eval("CustomerID") %>')">
                                                    <%#Eval("Status").ToString() == "1" ? "<i class='icon-ok'></i>" : "<i class='icon-minus-sign'></i>" %>
                                                </a>

                                            </td>
                                            <td><a href="FL_CustomerModify.aspx?CustomerId=<%#Eval("CustomerID") %>" target="_blank" class="btn btn-info btn-xs" data-original-title="编辑客户" data-toggle="tooltip"><i class="icon-edit"></i>&nbsp; 修改</a></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>

                        <cc1:AspNetPagerTool ID="CtrPageIndex" ClientIDMode="Static" runat="server" OnPageChanged="btnConfirm_Click"></cc1:AspNetPagerTool>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(function () {
            $(".chosen-drop").css("border", "none");
            $(".chosen-search").addClass("form-control").css("height", "auto");
            $(".chosen-results").addClass("form-control").css("height", "auto");
            $(".chosen-single").attr("class", "form-control");
        });
    </script>
</asp:Content>

