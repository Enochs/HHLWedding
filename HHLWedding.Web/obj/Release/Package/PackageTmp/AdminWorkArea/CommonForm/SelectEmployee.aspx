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
            if (type == "Department") {
                var DepartmentId = "<%=Request["department"] %>";
                if (EmployeeId != "") {
                    hhl.ajax("/WebService/DepartmentHandler.asmx/SetDepartmentManager", '{"EmployeeId":"' + EmployeeId + '","DepartmentId":"' + DepartmentId + '"}', function (result) {
                        if (result.d.IsSuccess) {
                            var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                            parent.layer.close(index);
                        }
                    });
                } else {
                    hhl.notify.warn("请选择部门主管", "提示", { timeOut: 1000 });
                }
            } else if (type == "message") {
                hhl.notify.success("测试成功");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <section class="panel panel-body">
                <%--<div class="panel-body col-md-12">--%>
                <table class="pull-left">
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
