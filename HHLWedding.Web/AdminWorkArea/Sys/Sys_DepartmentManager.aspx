<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/MainMaster.Master" AutoEventWireup="true" CodeBehind="Sys_DepartmentManager.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Sys.Sys_DepartmentManager" %>


<%@ Register Assembly="HHLWedding.ToolsLibrary" Namespace="HHLWedding.ToolsLibrary" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <%-- <!--日期插件-->
    <script src="../Content/daterangerpicker/moment.js"></script>
    <script src="../Content/daterangerpicker/daterangepicker.js"></script>
    <script src="../Content/daterangerpicker/hhl.datepicker.js"></script>
    <link href="../Content/daterangerpicker/daterangepicker.css" rel="stylesheet" />
    <script type="text/javascript">

        layer.config({
            skin: 'layer-ext-moon',
            extend: 'skin/moon/style.css',
        });


        function OpenCreate() {
            hhl.dialog.open(["Sys_DepartmentCreate.aspx", "no"], "添加部门", {
                area: ["550px", "250px"],
                btn: ["确定", "取消"],
                title: "添加部门",
                skin: 'layer-ext-moon',
                extend: '../Content/layer/skin/moon/style.css',
                //closeBtn: 0, //不显示关闭按钮
                close: function () {
                    window.location.reload();
                },
                yes: function (index, layero) {
                    hhl.dialogYes(layero);
                }
            });
        }

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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="crumb-wrap wrap-Title">
        <i class="Hui-iconfont">&#xe67f;</i> 首页 
        <span class="c-gray en">&gt;</span> 资讯管理 
        <span class="c-gray en">&gt;</span> 资讯列表 
        <a class="btn btn-success radius r mr-20" href="javascript:location.replace(location.href);" title="刷新"><i class="icon-refresh"></i></a>
    </div>

    <br />
    <div class="panel">
        <div class="panel-body">
            <div class="form-group pull-left" style="margin-top: -20px;">
                <a class="btn btn-primary" onclick="GetDates()" title="获取时间">获得时间</a>
                <a href="javaascript:void(0)" class="btn btn-primary  btn-mini" onclick="OpenCreate()" data-toggle="tooltip" title="添加顶级部门" data-original-title="添加顶级部门">添加顶级部门</a>
                <a class="btn btn-primary  btn-mini" href="Sys_EmployeeManager.aspx?DepartmentID=-1&NeedPopu=1">已停用的员工</a><br />
            </div>


            <%--<div id="reportrange" class="form-group pull-left" style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc">
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
                            <tr skey='<%#Eval("DepartmentID") %>'>
                                <td><%#GetItemNbsp(Eval("ItemLevel")) %> <%#Eval("DepartmentName") %></td>
                                <td>
                                    <a href="#" class="btn btn-mini btn-primary" onclick='ShowPopu(<%#Eval("DepartmentID") %>);'>添加子部门</a>
                                    <a href="Sys_EmployeeManager.aspx?DepartmentID=<%#Eval("DepartmentID") %>&NeedPopu=1" class="btn btn-mini btn-primary">部门员工管理</a>
                                    <a href="#" class="btn btn-mini btn-primary" onclick='ShowWindows(<%#Eval("DepartmentID") %>,<%#Eval("Parent") %>,this);'>修改</a>
                                    <asp:LinkButton CssClass="btn btn-danger btn-mini" ID="lkbtnDelete" runat="server" CommandName="Delete" CommandArgument='<%#Eval("DepartmentID") %>'>删除</asp:LinkButton>
                                </td>
                                <td id="<%#Guid.NewGuid().ToString() %>">
                                    <asp:HiddenField ID="hiddDepartmentManager" runat="server" Value='<%#Eval("DepartmentManager") %>' />
                                    <asp:HiddenField ID="hiddDepartmentID" runat="server" Value='<%#Eval("DepartmentID") %>' />
                                    <input style="margin: 0" runat="server" id="txtEmpLoyee" class="txtEmpLoyeeName" onclick="ShowEmployeePopu(this);" type="text" value='<%#GetEmployeeName(Eval("DepartmentManager")) %>' />
                                    <a href="#" class="btn btn-mini btn-primary" onclick="ShowEmployeePopu(this);" class="SetState">选择部门负责人</a>
                                    <asp:HiddenField ID="hideEmpLoyeeID" ClientIDMode="Static" Value='' runat="server" />
                                    <asp:Button ID="Button1" CommandArgument='<%#Eval("DepartmentID") %>' CommandName="SaveChange" runat="server" Text="保存设置" CssClass="btn btn-success btn-mini" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <cc1:AspNetPagerTool ID="DepartmentPager" AlwaysShow="true" OnPageChanged="DepartmentPager_PageChanged" runat="server"></cc1:AspNetPagerTool>
        </div>
    </div>
</asp:Content>
