<%@ Page Title="员工信息列表" Language="C#" MasterPageFile="~/AdminWorkArea/Master/MainMaster.Master" AutoEventWireup="true" CodeBehind="Sys_EmployeeManager.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Sys.Sys_Employee.Sys_EmployeeManager" %>

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
            hhl.dialog.open(["Sys_EmployeeCreate.aspx"], "添加员工", {
                area: ["500px", "600px"],
                btn: ["确定", "取消"],
                title: "添加员工",
            });
        }

        //修改窗口
        function OpenUpdate(employeeId) {
            var isSuccess = false;
            hhl.dialog.openUpdate(["Sys_EmployeeUpdate.aspx?employeeId=" + employeeId], "修改员工信息", {
                area: ["500px", "600px"],
                btn: ["确定", "取消"],
                title: "修改员工信息",
            });
        }

        //编辑权限
        function ShowPower(EmployeeId) {
            hhl.dialog.openUpdate(["Sys_EmployeePowerCreate.aspx?EmployeeId=" + EmployeeId], "编辑员工权限", {
                area: ["300px", "500px"],
                btn: ["确定", "关闭"],
                title: "编辑员工权限",
            });
        }

        //禁用/启用
        function SetSingleEnable(employeeId, index) {
            hhl.ajax("/WebService/EmployeeHandler.asmx/SetEmployeeSingleStatus", '{"EmployeeId":"' + employeeId + '","index":"' + index + '" }', hhl.callbackStatus);

        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="crumb-wrap wrap-Title">
        <i class="Hui-iconfont">&#xe67f;</i> 首页 
                <span class="c-gray en">&gt;</span> 员工管理 
                <span class="c-gray en">&gt;</span> 员工管理列表 
                <a class="btn btn-success radius r mr-20" href="javascript:location.replace(location.href);" data-toggle="tooltip" title="刷新"><i class="icon-refresh"></i></a>
    </div>

    <div class="panel ">
        <div class="widget-box">
            <div class="widget-header">
                <h4 class="h-title pull-left"><i class="icon-reorder"></i><b>员工列表</b></h4>
                <div class="box pull-right">
                    <div class="div_angle">
                        <span class="span-angel">
                            <i id="icon_angle" class="icon-angle-down"></i>
                        </span>
                    </div>
                </div>
            </div>
            <div class="widget-content bg-white">
                <div class="form-group col-sm-3">
                    <asp:TextBox runat="server" ID="txtEmployee" CssClass="chosen-text" placeholder="请输入员工名称" />
                </div>
                <div class="form-group col-sm-3">
                    <asp:DropDownList runat="server" ID="ddlStatus" CssClass="chosen-select form-control" DataTextField="Text" DataValueField="Value" />
                </div>
                <div class="form-group col-sm-3">
                    <asp:DropDownList runat="server" ID="ddlDepartment" CssClass="chosen-select" DataTextField="Text" DataValueField="Value" />
                </div>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="form-group col-sm-3">
                            <asp:Button runat="server" ID="btnConfirm" Text="查询" ClientIDMode="Static" CssClass="btn btn-primary" OnClick="btnConfirm_Click" Style="display: none" />
                            <button type="button" class="btn btn-info" title="搜索" data-toggle="tooltip" id="_btnSearch" runat="server" onserverclick="btnConfirm_Click"><i class="icon-search"></i>&nbsp; 搜索</button>
                            <button type="button" class="btn btn-info" title="刷新" data-toggle="tooltip" id="btnRefresh" runat="server"><i class="icon-refresh" onclick="window.location.reload()"></i>&nbsp; 刷新</button>
                            <a class="btn btn-success" title="添加员工" data-toggle="tooltip" onclick="OpenCreate()"><i class="icon-plus"></i>&nbsp; 添加员工</a>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
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
                                <th>员工名称</th>
                                <th>所属部门</th>
                                <th>职位</th>
                                <th>员工类型</th>
                                <th>登录名</th>
                                <th>性别</th>
                                <th>生日</th>
                                <th>手机号码</th>
                                <th>状态</th>
                                <th>操作</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="repEmployee" ClientIDMode="Static">
                                <ItemTemplate>
                                    <tr id='<%#Eval("EmployeeID") %>'>
                                        <td>
                                            <label class="checkbox-inline">
                                                <input type="checkbox" name="empId" value='<%#Eval("EmployeeID") %>' />
                                            </label>
                                        </td>
                                        <td><%#Eval("EmployeeName") %></td>
                                        <td><%#GetDepartmentName(Eval("DepartmentID")) %></td>
                                        <td><%#GetEmployeeJob(Eval("JobId")) %></td>
                                        <td><%#GetEmployeeType(Eval("EmployeeTypeID")) %></td>
                                        <td><%#Eval("LoginName") %> </td>
                                        <td><%#Eval("Sex").ToString() == "False" ? "男" : "女" %></td>
                                        <td><%#Eval("BornDate","{0:yyyy-MM-dd}") %></td>
                                        <td><%#Eval("TelPhone") %></td>
                                        <td><%#GetStatusName(Eval("Status")) %></td>
                                        <th>
                                            <button type="button" name="btn_status" onclick='SetSingleEnable(<%#Eval("EmployeeID") %>,10)' class="btn btn-<%#Eval("Status").ToString() == "1" ? "danger btn-xs" : "success btn-xs" %>" data-toggle="tooltip" title="<%#Eval("Status").ToString() == "1" ? "点击禁用" : "点击启用" %>">
                                                <%#Eval("Status").ToString() == "1" ? "<i name='icon_status' id='icon_status' class='icon-off'></i>&nbsp; 禁用" : "<i name='icon_status' id='icon_status' class='icon-ok'></i>&nbsp; 启用" %>
                                            </button>
                                            <a class="btn btn-info btn-xs" onclick='OpenUpdate(<%#Eval("EmployeeID") %>)' data-original-title="编辑员工" data-toggle="tooltip"><i class="icon-edit"></i>&nbsp; 编辑</a>
                                            <a class="btn btn-primary btn-xs" onclick='ShowPower(<%#Eval("EmployeeID") %>)'><i class="icon-share"></i>&nbsp; 编辑权限</a>
                                        </th>
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
