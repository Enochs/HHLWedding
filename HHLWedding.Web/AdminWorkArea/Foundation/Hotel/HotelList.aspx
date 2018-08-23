<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/MainMaster.Master" AutoEventWireup="true" CodeBehind="HotelList.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Foundation.Hotel.HotelList" %>

<%@ Register Assembly="HHLWedding.ToolsLibrary" Namespace="HHLWedding.ToolsLibrary" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(function () {

        });

        //添加酒店
        function OpenCreate() {
            hhl.dialog.openfull(["HotelCreate.aspx"], "添加酒店", null, 2);
        }

        //修改酒店信息
        function OpenUpdate(hotelId) {
            hhl.dialog.openfull(["HotelUpdate.aspx?hotelId=" + hotelId], "修改酒店信息");
        }

        //上传图片
        function UploadImage(type, name, id) {
            hhl.dialog.openUpdate(["/AdminWorkArea/CommonForm/FileUpload.aspx?type=" + type + "&name=" + name + "&id=" + id], "修改酒店信息", {
                btn: ["关闭页面"],
                yes: function (index) {
                    layer.close(index);
                }
            });
        }

        //修改状态
        function SetSingleEnable(hotelId) {
            hhl.ajax("/WebService/HotelHandler.asmx/SetSingleStatus", '{"hotelId":"' + hotelId + '"}', hhl.callbackStatus);
        }

    </script>

    <style type="text/css">
        /* 隐藏还原按钮 */
        .layui-layer-setwin .layui-layer-maxmin {
            display: none;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel">
        <div class="widget-box">
            <div class="widget-header">
                <h4 class="h-title pull-left"><i class="icon-reorder"></i><b>酒店列表</b></h4>
                <div class="box pull-right">
                    <div class="div_angle">
                        <span class="span-angel">
                            <i id="icon_angle" class="icon-angle icon-angle-up"></i>
                        </span>
                    </div>
                </div>
            </div>
            <div class="widget-content">
                <div class="form-group col-sm-3">
                    <asp:TextBox runat="server" ID="txtHotelName" CssClass="form-control" placeholder="请输入酒店名称" />
                </div>

                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="form-group col-sm-3">
                            <asp:Button runat="server" ID="btnConfirm" Text="查询" ClientIDMode="Static" CssClass="btn btn-primary" OnClick="btnConfirm_Click" Style="display: none" />
                            <button type="button" class="btn btn-info" title="搜索" data-toggle="tooltip" id="_btnSearch" runat="server" onserverclick="_btnSearch_ServerClick"><i class="icon-search"></i></button>
                            <button type="button" class="btn btn-info" title="刷新" data-toggle="tooltip" id="_btnRefresh" runat="server" onclick="window.location.reload()"><i class="icon-refresh"></i></button>
                            <a class="btn btn-success" title="添加酒店" data-toggle="tooltip" onclick="OpenCreate()"><i class="icon-plus"></i></a>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="widget-box" style="background-color: #F1F2F9;">
                    <table id="tbl_Hotel" class="table table-bordered table-hover bg-white table-list">
                        <thead>
                            <tr class="th_title">
                                <th>
                                    <label class="checkbox-inline">
                                        <input type="checkbox" name="chkAll" onclick="checkAll(this)" />
                                    </label>
                                </th>
                                <th>编号</th>
                                <th>酒店名称</th>
                                <th>酒店类型</th>
                                <th>酒店地址</th>
                                <th>餐标</th>
                                <th>创建时间</th>
                                <th>状态</th>
                                <th>操作</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="repHotel" ClientIDMode="Static">
                                <ItemTemplate>
                                    <tr id='<%#Eval("HotelID") %>'>
                                        <td>
                                            <label class="checkbox-inline">
                                                <input type="checkbox" name="cid" value='<%#Eval("HotelID") %>' />
                                            </label>
                                        </td>
                                        <td><%#Eval("HotelID") %></td>
                                        <td><%#Eval("HotelName") %></td>
                                        <td><%#GetHotelTypeName(Eval("HotelType")) %></td>
                                        <td><%#Eval("Address") %></td>
                                        <td>￥<%#Eval("Start") %>-<%#Eval("End") %>/桌</td>
                                        <td><%#Eval("CreateDate","{0:yyyy-MM-dd}") %></td>
                                        <td><%#GetStatusName(Eval("Status")) %></td>
                                        <td>
                                            <button type="button" name="btn_status" onclick='SetSingleEnable(<%#Eval("HotelID") %>)' class="btn btn-<%#Eval("Status").ToString() == "1" ? "danger btn-xs" : "success btn-xs" %>" data-toggle="tooltip">
                                                <%#Eval("Status").ToString() == "1" ? "<i name='icon_status' id='icon_status' class='icon-off'></i>&nbsp; 禁用" : "<i name='icon_status' id='icon_status' class='icon-ok'></i>&nbsp; 启用" %>
                                            </button>
                                            <a class="btn btn-info btn-xs" onclick='OpenUpdate(<%#Eval("HotelID") %>)' data-original-title="编辑酒店" data-toggle="tooltip"><i class="icon-edit"></i>&nbsp; 编辑</a>
                                            <a class="btn btn-success btn-xs" onclick='UploadImage("hotelUrl","<%#Eval("HotelName") %>",<%#Eval("HotelID") %>)'>上传图片</a>
                                            <a class="btn btn-primary btn-xs" onclick='ShowImageList(<%#Eval("HotelID") %>)'>图片管理</a>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                    <script>
                        function ShowImageList(hotelId) {
                            var pages = $("#CtrPageIndex").find("tr:first").find("td").eq(1).find("span:first").html();
                            window.location.href = "/AdminWorkArea/CommonForm/CommonImgList.aspx?Type=1&Id=" + hotelId + "&page=" + pages;
                        }
                    </script>
                    <cc1:AspNetPagerTool ID="CtrPageIndex" ClientIDMode="Static" runat="server" OnPageChanged="CtrPageIndex_PageChanged"></cc1:AspNetPagerTool>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
