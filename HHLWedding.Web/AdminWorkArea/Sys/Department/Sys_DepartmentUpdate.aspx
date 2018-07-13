<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/PopuMaster.Master" AutoEventWireup="true" CodeBehind="Sys_DepartmentUpdate.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Sys.Department.Sys_DepartmentUpdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">


        //提交修改
        function submitForm() {

            var department = getUrlParam("department");

            var name = $("#txtDepartmentName").val();
            if (name == "") {
                hhl.notify.warn("请输入部门名称", "提示", { timeOut: 1000 });
                return false;
            }
            hhl.ajax("/WebService/DepartmentHandler.asmx/UpdateDepartment", '{"_department":{"DepartmentName":"' + name + '","DepartmentID":"' + department + '"}}', hhl.callbackRefresh);
        }


        //页面加载
        $(function () {
            var department = getUrlParam("department");
            var parent = getUrlParam("parentId");
            hhl.ajax("/WebService/DepartmentHandler.asmx/GetDepartmentById", '{"parentId":"' + parent + '","Department":"' + department + '"}', function (result) {
                if (result.d.IsSuccess) {
                    $("input[name='txtDepartmentName']").val(result.d.Name[0]);
                    $("span[name='span_parent']").html(result.d.Name[1]);
                }
            });

            //回车键提交
            $("#txtDepartmentName").keydown(function () {
                if (event.keyCode == 13) {
                    submitForm();
                }
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel">
        <div class="panel-body col-md-12">
            <table id="tblSystem" class="table table-bordered">
                <tr>
                    <td>部门名称</td>
                    <td>
                        <input type="text" name="txtDepartmentName" class="form-control" id="txtDepartmentName" />
                    </td>
                </tr>
                <tr>
                    <td>顶级部门</td>
                    <td>
                        <span id="span_parent" name="span_parent" runat="server">顶级部门</span>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
