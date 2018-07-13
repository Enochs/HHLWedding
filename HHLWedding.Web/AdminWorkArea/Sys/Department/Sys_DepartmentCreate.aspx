<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/PopuMaster.Master" AutoEventWireup="true" CodeBehind="Sys_DepartmentCreate.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Sys.Department.Sys_DepartmentCreate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">


        //提交添加
        function submitForm() {
            var parent = getUrlParam("parentId");

            if (parent == 'undefined') {
                parent = 0;
            }

            var name = $("#txtDepartmentName").val();
            if (name == "") {
                hhl.notify.warn("请输入部门名称", "提示", { timeOut: 1000 });
                return false;
            }
            hhl.ajax("/WebService/DepartmentHandler.asmx/CreateDepartment", '{"_department":{"DepartmentName":"' + name + '","Parent":"' + parent + '"}}', hhl.callbackRefresh);
        }

        //页面加载
        $(function () {

            var parent = getUrlParam("parentId");
            if (parent != 'undefined') {
                hhl.ajax("/WebService/DepartmentHandler.asmx/GetDepartmentById", '{"parentId":"' + parent + '","Department":"0"}', function (result) {
                    if (result.d.IsSuccess) {
                        $("span[name='span_parent']").html(result.d.Value);
                    }
                });
            }

            //回车键提交
            $("#txtDepartmentName").keydown(function (e) {
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
