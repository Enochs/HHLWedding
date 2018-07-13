<%@ Page Title="员工类型管理" Language="C#" MasterPageFile="~/AdminWorkArea/Master/MainMaster.Master" AutoEventWireup="true" CodeBehind="Sys_EmployeeTypeManager.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Sys.JobType.Sys_EmployeeTypeManager" %>


<%@ Register Assembly="HHLWedding.ToolsLibrary" Namespace="HHLWedding.ToolsLibrary" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">


        //添加类型
        function OpenCreate() {
            hhl.dialog.openUpdate(["Sys_EmployeeTypeCreate.aspx", "no"], "添加员工类型", {
                area: ['380px', '180px'],           //长 ，  高
                btn: ["确定", "取消"],
            }, 2);
        }

        //修改类型
        function OpenUpdate(TypeId) {
            var isSuccess = false;
            hhl.dialog.openUpdate(["Sys_EmployeeTypeUpdate.aspx?TypeId=" + TypeId, "no"], "修改员工类型", {
                area: ['380px', '180px'],           //长 ，  高
                btn: ["确定", "取消"],
            });
        }

        // 禁用/启用
        function SetSingleEnable(TypeId, index) {
            hhl.ajax("/WebService/EmployeeHandler.asmx/SetEmpTypeSingleStatus", '{"TypeId":"' + TypeId + '","index":"' + index + '" }', hhl.callbackStatus);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%--<div class="crumb-wrap wrap-Title">
        <i class="Hui-iconfont">&#xe67f;</i> 首页 
                <span class="c-gray en">&gt;</span> 员工类型管理 
                <span class="c-gray en">&gt;</span> 员工类型列表 
                <a class="btn btn-success radius r mr-20" href="javascript:location.replace(location.href);" data-toggle="tooltip" title="刷新"><i class="icon-refresh"></i></a>
    </div>--%>
    <div class="panel">
        <%--<div class="panel-body">--%>
            <div class="widget-box">
                <div class="widget-header">
                    <h4 class="h-title pull-left"><i class="icon-reorder"></i><b>员工类型管理</b></h4>
                    <div class="box pull-right">
                        <div class="div_angle">
                            <span class="span-angel">
                                <i id="icon_angle" class="icon-angle icon-angle-up"></i>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="widget-content bg-white">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="form-group col-sm-3">
                                <asp:Button runat="server" ID="btnConfirm" Text="查询" ClientIDMode="Static" CssClass="btn btn-primary" OnClick="btnConfirm_Click" Style="display: none;" />
                                <button type="submit" class="btn btn-info" title="搜索" data-toggle="tooltip" id="_btnSearch" runat="server" onserverclick="btnConfirm_Click"><i class="icon-search"></i></button>
                                <button type="button" class="btn btn-info" title="刷新" data-toggle="tooltip" id="_btnRefresh" runat="server" onclick="window.location.reload()"><i class="icon-refresh"></i></button>
                                <a class="btn btn-success" title="添加职务" data-toggle="tooltip" onclick="OpenCreate()"><i class="icon-plus"></i></a>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="widget-box" style="background-color: #F1F2F9;">
                        <table id="tbl_EmployeeType" class="table table-bordered table-hover bg-white table-list">
                            <thead>
                                <tr class="th_title">
                                    <th>
                                        <label class="checkbox-inline">
                                            <input type="checkbox" name="chkAll" onclick="checkAll(this)" />
                                        </label>
                                    </th>
                                    <th>类型名称</th>
                                    <th>创建时间</th>
                                    <th>创建人</th>
                                    <th>状态</th>
                                    <th>操作</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ClientIDMode="Static" runat="server" ID="repEmpType">
                                    <ItemTemplate>
                                        <tr id="<%#Eval("EmployeeTypeId") %>">
                                            <td>
                                                <input type="hidden" name="hideJobId" value='<%#Eval("EmployeeTypeId") %>' />
                                                <label class="checkbox-inline">
                                                    <input type="checkbox" name="cid" value='<%#Eval("EmployeeTypeId") %>' />
                                                </label>
                                            </td>
                                            <td><%#Eval("TypeName") %></td>
                                            <td><%#Eval("CreateTime","{0:yyy-MM-dd HH:mm:ss}") %></td>
                                            <td><%#GetEmployeeName(Eval("EmployeeId")) %></td>
                                            <td><%#GetStatusName(Eval("Status")) %></td>
                                            <td>
                                                <button type="button" name="btn_status" onclick='SetSingleEnable(<%#Eval("EmployeeTypeId") %>,5)' class="btn btn-<%#Eval("Status").ToString() == "1" ? "danger btn-xs" : "success btn-xs" %>" title="<%#Eval("Status").ToString() == "1" ? "点击禁用" : "点击启用" %>" data-toggle="tooltip">
                                                    <%#Eval("Status").ToString() == "1" ? "<i name='icon_status' id='icon_status' class='icon-off'></i>" : "<i name='icon_status' id='icon_status' class='icon-ok'></i>" %>
                                                </button>
                                                <a onclick='OpenUpdate(<%#Eval("EmployeeTypeId") %>)' title="编辑员工类型" data-toggle="tooltip" class="btn btn-info btn-xs"><i class="icon-edit"></i></a>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>

                        <cc1:AspNetPagerTool ID="CtrPageIndex" runat="server" ClientIDMode="Static" OnPageChanged="CtrPageIndex_PageChanged"></cc1:AspNetPagerTool>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    <%--</div>--%>
</asp:Content>
