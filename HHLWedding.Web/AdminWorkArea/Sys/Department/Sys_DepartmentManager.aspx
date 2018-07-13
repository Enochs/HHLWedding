<%@ Page Title="部门管理" Language="C#" MasterPageFile="~/AdminWorkArea/Master/MainMaster.Master" AutoEventWireup="true" CodeBehind="Sys_DepartmentManager.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Sys.Department.Sys_DepartmentManager" %>


<%@ Register Assembly="HHLWedding.ToolsLibrary" Namespace="HHLWedding.ToolsLibrary" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <!--日期插件-->
    <%--    <script src="/Content/hhlset/daterangerpicker/moment.js"></script>
    <script src="/Content/hhlset/daterangerpicker/daterangepicker.js"></script>
    <script src="/Content/hhlset/daterangerpicker/hhl.datepicker.js"></script>
    <link href="/Content/hhlset/daterangerpicker/daterangepicker.css" rel="stylesheet" />

    <script src="/Content/hhlset/daterangerpicker/hhl.datepicker.js"></script>
    <script type="text/javascript">

        layer.config({
            skin: 'layer-ext-moon',
            extend: '/Content/hhlset/layer/skin/moon/style.css',
        });

        //获取时间
        function GetDates() {
            var start = $("#hideBegin").val();
            var end = $("#hideEnds").val();
            alert(start + "-" + end);
        }

        $(function () {
            $("#reportrange").hhldatePicker();
        })



    </script>--%>

    <script type="text/javascript">

        //添加窗口
        function OpenCreate(pid) {
            var isSuccess = false;
            hhl.dialog.openUpdate(["Sys_DepartmentCreate.aspx?parentId=" + pid, "no"], "添加部门", {
                area: ["500px", "220px"],
                btn: ["确定", "取消"],
            },2);
        }

        //修改窗口
        function OpenUpdate(departmentId, pid) {
            hhl.dialog.openUpdate(["Sys_DepartmentUpdate.aspx?department=" + departmentId + "&parentId=" + pid, "no"], "修改部门", {
                area: ["500px", "220px"],
                btn: ["确定", "取消"],
            });
        }

        //选择部门主管
        function SelectEmployee(departmentId) {
            hhl.dialog.openUpdate(["/AdminWorkArea/CommonForm/SelectEmployee.aspx?type=Department&Id=" + departmentId], "选择责任人", {
                area: ["450px", "420px"],
                btn: ["确定", "取消"],
            });
        }

        //删除功能
        function DepartmentDelete(id, name) {
            hhl.message.confirm("你确定删除" + name + "吗？(" + name + "下的所有部门都会删除)", function (result) {
                if (result) {
                    hhl.ajax("/WebService/DepartmentHandler.asmx/DeleteDepartment", '{"DepartmentId":"' + id + '"}',hhl.callbackStatus);
                }
            });
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--      <div class="crumb-wrap wrap-Title">
                <i class="Hui-iconfont">&#xe67f;</i> 首页 
                <span class="c-gray en">&gt;</span> 部门管理 
                <span class="c-gray en">&gt;</span> 部门员工管理列表 
                <a class="btn btn-success radius r mr-20" href="javascript:location.replace(location.href);" data-toggle="tooltip" title="刷新"><i class="icon-refresh"></i></a>
            </div>--%>
            <div class="wrapper wrapper-content">
                <div class="panel">
                    <div class="panel-body col-md-12">
                        <div class="form-group pull-left">
                            <a href="javaascript:void(0)" class="btn btn-primary  btn-sm" onclick="OpenCreate()" data-toggle="tooltip" title="添加顶级部门" data-original-title="添加顶级部门">添加顶级部门</a>
                        </div>


                        <%-- <div id="reportrange" class="form-group pull-left" style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc">
                        <asp:HiddenField runat="server" ID="hideBegin" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hideEnds" ClientIDMode="Static" />
                        <i class="icon-calendar"></i>
                        <span></span>
                        <b class="caret"></b>
                    </div>--%>

                        <table class="table table-bordered table-striped table-select">
                            <thead>
                                <tr>
                                    <th>部门名称</th>
                                    <th>操作</th>
                                    <th>设置部门主管</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptDepartment" runat="server" OnItemCommand="rptDepartment_ItemCommand">
                                    <ItemTemplate>
                                        <tr id='<%#Eval("DepartmentID") %>'>
                                            <td><%#GetItemNbsp(Eval("ItemLevel")) %> <%#Eval("DepartmentName") %></td>
                                            <td>
                                                <a class="btn btn-info btn-xs" onclick='OpenCreate(<%#Eval("DepartmentID") %>);' title="添加子部门" data-toggle="tooltip"><i class="icon-plus"></i></a>
                                                <a href="../Sys_Employee/Sys_EmployeeManager.aspx?DepartmentId=<%#Eval("DepartmentID") %>" class="btn btn-primary btn-xs" title="部门员工管理" data-toggle="tooltip"><i class="icon-th"></i></a>
                                                <a class="btn btn-primary btn-xs" onclick='OpenUpdate(<%#Eval("DepartmentID") %>,<%#Eval("Parent") %>,this);' title="修改" data-toggle="tooltip"><i class="icon-edit"></i></a>
                                                <button type="button" id="btnDelete" class="btn btn-primary btn-danger btn-xs" title="删除" data-toggle="tooltip" onclick='DepartmentDelete(<%#Eval("DepartmentID") %>, "<%#Eval("DepartmentName") %>")'><i class="icon-trash"></i></button>
                                            </td>
                                            <td id="<%#Guid.NewGuid().ToString() %>">
                                                <input id="txtEmpLoyeeName" class="txtEmpLoyeeName" type="text" onclick='SelectEmployee(<%#Eval("DepartmentID") %>)' value="<%#GetEmployeeName(Eval("DepartmentManager")) %>    " />
                                                <a onclick='SelectEmployee(<%#Eval("DepartmentID") %>)' class="btn btn-primary btn-xs">选择部门负责人</a>

                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
                <asp:Button runat="server" ID="btnConfirm" Text="查询" ClientIDMode="Static" CssClass="btn btn-primary" OnClick="btnConfirm_Click" Style="display: none;" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
