<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sys_DepartmentCreate.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Sys.Sys_DepartmentCreate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../Content/bootstrap.css" rel="stylesheet" />
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/Site.css" rel="stylesheet" />

    <script src="../Scripts/jquery-1.10.2.js"></script>
    <script src="../Scripts/jquery-1.10.2.min.js"></script>

    <style type="text/css">
        .table{
            margin-top:-15px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <section class="panel ">
            <div class="form-horizontal">
                <table class="table">
                    <tr>
                        <td>部门名称:</td>
                        <td>
                            <asp:TextBox ID="txtDepartmentName" ClientIDMode="Static" check="1" tip="限20个字符！" CssClass="form-control" runat="server" MaxLength="20" /></td>
                    </tr>
                    <tr>
                        <td>上级部门:</td>
                        <td>
                            <asp:Label runat="server" ID="lblParent" /></td>
                    </tr>
                </table>
            </div>
        </section>
    </form>
</body>
</html>

<script src="../Content/toastr/toastr.js"></script>
<link href="../Content/toastr/toastr.min.css" rel="stylesheet" />
<link href="../Content/toastr/toastr.css" rel="stylesheet" />
<script src="../Content/layer/layer.js"></script>
<link href="../Content/layer/skin/layer.css" rel="stylesheet" />

<script src="../Scripts/hhl.js"></script>

<script type="text/javascript">
    function submitForm() {
        var name = $("#txtDepartmentName").val();

        if (name == "") {
            hhl.notify.warn("部门名称不能为空", "提示");
            setTimeout(function () {
                hhl.notify.clear();
            }, 500);
            return false;
        }
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/WebService/DepartmentService.asmx/CreateDepartment",
            //url: "Sys_DepartmentCreate.aspx/CreateDepartment",
            data: "{name:'" + name + "'}",
            dataType: "json",
            success: function (result) {
                $(result.d).each(function () {
                    alert(this['department']);

                });
            }
        });

    }

</script>
