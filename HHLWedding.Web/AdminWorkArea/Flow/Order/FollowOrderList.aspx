<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/MainMaster.Master" AutoEventWireup="true" CodeBehind="FollowOrderList.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Flow.Order.FollowOrderList" %>

<%@ Register Assembly="HHLWedding.ToolsLibrary" Namespace="HHLWedding.ToolsLibrary" TagPrefix="cc1" %>
<%@ Register Assembly="HHLWedding.Control" Namespace="HHLWedding.Control" TagPrefix="cc2" %>
<%@ Import Namespace="HHLWedding.DataAssmblly.CommonModel" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(function () {
            $(".chosen-drop").css("border", "none");
            $(".chosen-search").addClass("form-control").css("height", "auto");
            $(".chosen-results").addClass("form-control").css("height", "auto");
            $(".chosen-single").attr("class", "form-control");

        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel">
        <div class="panel-body">
            <div class="widget-box">
                <div class="widget-header">
                    <h4 class="h-title pull-left"><i class="icon-reorder"></i><b>客户跟单</b></h4>
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
                        <input type="text" id="txtName" name="txtName" class="form-control" placeholder="请输入新人姓名" />
                    </div>
                    <div class="form-group col-sm-2">
                        <input type="text" id="txtPhone" name="txtPhone" class="form-control" placeholder="请输入联系电话" />
                    </div>
                    <div class="form-group col-sm-2">
                        <cc2:MyEmployee runat="server" ID="ddlEmployee" placeholder="请选择跟单人" ClientIDMode="Static" onchange="ddlChange()" />
                    </div>
                    <div class="form-group colo-sm-2">
                        <asp:UpdatePanel runat="server" ID="updateSearch">
                            <ContentTemplate>
                                <asp:Button runat="server" ID="btnConfirm" Text="查询" ClientIDMode="Static" CssClass="btn btn-primary" OnClick="btnConfirm_Click" Style="display: none" />
                                <button type="button" class="btn btn-info" title="搜索" data-toggle="tooltip" id="_btnSearch" runat="server" onserverclick="btnConfirm_Click"><i class="icon-search"></i>&nbsp; 搜索</button>
                                <button type="button" class="btn btn-info" title="刷新" data-toggle="tooltip" id="_btnRefresh" runat="server" onclick="window.location.reload()"><i class="icon-refresh"></i>&nbsp; 刷新</button>
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
                                    <th>跟单次数</th>
                                    <th>渠道类型</th>
                                    <th>渠道</th>
                                    <th>跟单人</th>
                                    <th>操作</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater runat="server" ID="repOrder">
                                    <ItemTemplate>
                                        <tr class="th_content">
                                            <td><a href='../Customer/FL_CustomerDetails.aspx?CustomerId=<%#Eval("CustomerId") %>' style="color: gray;" target="_blank"><%#GetCustomerName(Eval("CustomerId")) %></a></td>
                                            <td><%#Eval("ContactPhone") %></td>
                                            <td><%#Eval("PartyDate","{0:yyyy-MM-dd}") %></td>
                                            <td><%#GetHotelName(Eval("HotelId")) %></td>
                                            <td><%#((CustomerState)Eval("OrderState")).GetDisplayName() %></td>
                                            <td><%#Eval("FollowCount") %></td>
                                            <td><%#GetSaleTypeName(Eval("SaleTypeId")) %></td>
                                            <td><%#GetSaleSourceName(Eval("SaleSourceId")) %></td>
                                            <td><%#GetEmployeeName(Eval("EmployeeId")) %></td>
                                            <td>
                                                <a href="OrderDetailsCreate.aspx?CustomerId=<%#Eval("CustomerId") %>&OrderId=<%#Eval("OrderId") %>" <%#Eval("EmployeeId").ToString() ==LoginInfo.UserInfo.EmployeeId.ToString() ? "" : "style='display:none;'"  %> class="btn btn-success btn-xs" data-original-title="填写跟单记录" data-toggle="tooltip"><i class="icon-edit"></i>&nbsp; 填写跟单记录</a>
                                                <a href="OrderDetailsCreate.aspx?CustomerId=<%#Eval("CustomerId") %>&type=Details" <%#Eval("EmployeeId").ToString() !=LoginInfo.UserInfo.EmployeeId.ToString() ? "" : "style='display:none;'"  %> target="_blank" class="btn btn-danger btn-xs" data-original-title="查看沟通记录" data-toggle="tooltip"><i class="icon-eye-open"></i>&nbsp; 查看沟通记录</a>
                                            </td>
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
</asp:Content>
