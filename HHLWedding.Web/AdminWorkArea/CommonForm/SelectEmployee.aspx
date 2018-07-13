<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/PopuMaster.Master" AutoEventWireup="true" CodeBehind="SelectEmployee.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.CommonForm.SelectEmployee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        var EmployeeId = 0;

        //选中radio
        function ChkEmployee(obj) {
            EmployeeId = $(obj).val();
        }

        //点击确定
        function submitForm() {
            var type = '<%=Request["type"]%>';
            var reType = '<%=Request["reType"] %>';
            if (type == "Department") {
                var DepartmentId = "<%=Request["Id"] %>";
                if (EmployeeId != 0) {
                    hhl.ajax("/WebService/DepartmentHandler.asmx/SetDepartmentManager", '{"EmployeeId":"' + EmployeeId + '","DepartmentId":"' + DepartmentId + '"}', function (result) {
                        if (result.d.IsSuccess) {
                            var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                            parent.layer.close(index);
                        }
                    });
                } else {
                    hhl.notify.warn("请选择部门主管", "提示", { timeOut: 1000 });
                }
            } else if (type == "orderEmployee" || type == "inviteEmployee") {
                if (EmployeeId != 0) {
                    hhl.ajax("/WebService/EmployeeHandler.asmx/GetEmpById", '{"employeeId":"' + EmployeeId + '"}', function (result) {
                        if (result.d) {
                            var m_emp = result.d;
                            if (type == "inviteEmployee") {
                                if (reType == "confirm") {
                                    var CustomerId = '<%=Request["CustomerId"] %>'
                                    var data = { employeeId: EmployeeId, customerId: CustomerId };
                                    hhl.ajax("/WebService/Flow/InviteHandler.asmx/ReInviteEmployee", data, function (res) {
                                        if (res.d.IsSuccess) {
                                            hhl.notify.success(res.d.Message, "提示");
                                            setTimeout(function () {
                                                var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                                                parent.layer.close(index);
                                            }, 1200)
                                        } else {
                                            hhl.notify.error(res.d.Message, "提示");
                                        }
                                    }, 1);

                                } else {
                                    parent.document.getElementById("txtInviteEmployees").value = m_emp.EmployeeName;
                                    parent.document.getElementById("txtInviteEmployees").title = EmployeeId;
                                    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                                    parent.layer.close(index);

                                }

                            } else if (type == "orderEmployee") {
                                if (reType == "confirm") {
                                    var CustomerId = '<%=Request["CustomerId"] %>'
                                    var data = { employeeId: EmployeeId, customerId: CustomerId };
                                        hhl.ajax("/WebService/Flow/OrderHandler.asmx/ReOrderEmployee", data, function (res) {
                                            if (res.d.IsSuccess) {
                                                hhl.notify.success(res.d.Message, "提示");
                                                setTimeout(function () {
                                                    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                                                    parent.layer.close(index);
                                                }, 1200)

                                            } else {
                                                hhl.notify.error(res.d.Message, "提示");
                                            }
                                        }, 1);

                                    } else {
                                        parent.document.getElementById("txtOrderEmployee").value = m_emp.EmployeeName;
                                        parent.document.getElementById("txtOrderEmployee").alt = EmployeeId;
                                        var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                                        parent.layer.close(index);
                                    }
                                }
                        }
                    });
                } else {
                    if (type == "inviteEmployee") {
                        hhl.notify.error("请选择邀约人", "提示", { timeOut: 1000 });
                    } else if (type == "orderEmployee") {
                        hhl.notify.error("请选择跟单人", "提示", { timeOut: 1000 });
                    }
                }
            }

    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <section class="panel panel-body">
                <%--<div class="panel-body col-md-12">--%>
                <table class="tbl_noborder pull-left">
                    <tbody>
                        <tr>
                            <td>
                                <asp:TreeView ID="treeViewDepartment" runat="server" OnSelectedNodeChanged="treeViewDepartment_SelectedNodeChanged">
                                </asp:TreeView>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div style="width: 120px; float: left;">
                    <span style="width: 80px">&nbsp;&nbsp;&nbsp;</span>
                </div>
                <div class="pull-left">
                    <table id="tblEmployee" class="table table-bordered table-hover">
                        <tbody>
                            <asp:Repeater ID="rptEmployee" runat="server">
                                <ItemTemplate>
                                    <tr id='<%#Eval("EmployeeID") %>'>
                                        <td style="width: 28px">
                                            <input type="radio" name="rdoEmployee" id="rdoEmployee" value='<%#Eval("EmployeeID") %>' onchange="ChkEmployee(this)" />
                                        </td>
                                        <td>
                                            <span style="color: black;"><%#Eval("EmployeeName") %></span>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
                <%--</div>--%>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
