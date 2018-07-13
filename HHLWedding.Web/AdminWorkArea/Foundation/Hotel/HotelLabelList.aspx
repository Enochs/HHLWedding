<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/MainMaster.Master" AutoEventWireup="true" CodeBehind="HotelLabelList.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Foundation.Hotel.HotelLabelList" %>

<%@ Register Assembly="HHLWedding.ToolsLibrary" Namespace="HHLWedding.ToolsLibrary" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">


        //复选框全选
        function checkAll(obj) {
            $("input[name='empId']").prop("checked", $(obj).prop("checked"));
        }

        //添加窗口
        function OpenCreate() {
            var isSuccess = false;
            hhl.dialog.open(["HotelLabelCreate.aspx", "no"], "添加标签", {
                area: ["400px", "180px"],
                btn: ["确定", "取消"],
                title: "添加标签",
            });
        }

        //修改窗口
        function OpenUpdate(labelId) {
            hhl.dialog.openUpdate(["HotelLabelUpdate.aspx?labelId=" + labelId, "no"], "修改标签信息", {
                area: ["400px", "180px"],
                btn: ["确定", "取消"],
                title: "修改标签信息",
            });
        }

        //禁用/启用
        function SetSingleEnable(labelId) {
            hhl.ajax("/WebService/HotelHandler.asmx/SetHotelSingleStatus", '{"labelId":"' + labelId + '"}', hhl.callbackStatus);

        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%--    <div class="crumb-wrap wrap-Title">
        <i class="Hui-iconfont">&#xe67f;</i> 首页 
                <span class="c-gray en">&gt;</span> 酒店管理 
                <span class="c-gray en">&gt;</span> 场地标签列表 
                <a class="btn btn-success radius r mr-20" href="javascript:location.replace(location.href);" data-toggle="tooltip" title="刷新"><i class="icon-refresh"></i></a>
    </div>--%>

    <div class="panel">
        <div class="widget-box">
            <div class="widget-header">
                <h4 class="h-title pull-left"><i class="icon-reorder"></i><b>场地标签列表</b></h4>
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
                    <asp:TextBox runat="server" ID="txtLabelName" CssClass="form-control" placeholder="请输入标签名称" />
                </div>

                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="form-group col-sm-3">
                            <asp:Button runat="server" ID="btnConfirm" Text="查询" ClientIDMode="Static" CssClass="btn btn-primary" OnClick="btnConfirm_Click" Style="display: none" />
                            <button type="button" class="btn btn-info" title="搜索" data-toggle="tooltip" id="_btnSearch" runat="server" onserverclick="_btnSearch_ServerClick"><i class="icon-search"></i></button>
                            <button type="button" class="btn btn-info" title="刷新" data-toggle="tooltip" id="_btnRefresh" runat="server" onclick="window.location.reload()"><i class="icon-refresh"></i></button>
                            <a class="btn btn-success" title="添加菜单" data-toggle="tooltip" onclick="OpenCreate()"><i class="icon-plus"></i></a>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="widget-box" style="background-color: #F1F2F9;">
                    <table id="tbl_Channel" class="table table-bordered table-hover table-blue bg-white table-list">
                        <thead>
                            <tr class="th_title">
                                <th>
                                    <label class="checkbox-inline">
                                        <input type="checkbox" name="chkAll" onclick="checkAll(this)" />
                                    </label>
                                </th>
                                <th>编号</th>
                                <th>标签名称</th>
                                <th>创建时间</th>
                                <th>创建人</th>
                                <th>状态</th>
                                <th>操作</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="repLabel" ClientIDMode="Static">
                                <ItemTemplate>
                                    <tr id='<%#Eval("LabelID") %>'>
                                        <td>
                                            <label class="checkbox-inline">
                                                <input type="checkbox" name="cid" value='<%#Eval("LabelID") %>' />
                                            </label>
                                        </td>
                                        <td><%#Eval("LabelID") %></td>
                                        <td><%#Eval("LabelName") %></td>
                                        <td><%#Eval("CreateDate","{0:yyyy-MM-dd}") %></td>
                                        <td><%#GetEmployeeName(Eval("CreateEmployee")) %></td>
                                        <td><%#GetStatusName(Eval("Status")) %></td>
                                        <td>
                                            <button type="button" name="btn_status" onclick='SetSingleEnable(<%#Eval("LabelID") %>,9)' class="btn btn-<%#Eval("Status").ToString() == "1" ? "danger btn-xs" : "success btn-xs" %>" data-toggle="tooltip">
                                                <%#Eval("Status").ToString() == "1" ? "<i name='icon_status' id='icon_status' class='icon-off'></i>" : "<i name='icon_status' id='icon_status' class='icon-ok'></i>" %>
                                            </button>
                                            <a class="btn btn-info btn-xs" onclick='OpenUpdate(<%#Eval("LabelID") %>)' data-original-title="编辑标签" data-toggle="tooltip"><i class="icon-edit"></i></a>
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
</asp:Content>
