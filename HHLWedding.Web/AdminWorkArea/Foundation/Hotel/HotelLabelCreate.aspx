<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/PopuMaster.Master" AutoEventWireup="true" CodeBehind="HotelLabelCreate.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Foundation.Hotel.HotelLabelCreate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function submitForm() {
            var name = $("#txtName").val();
            if (name == "") {
                hhl.notify.warn("请输入标签名称", "提示", { timeOut: 1000 });
                return false;
            }

            hhl.ajax("/WebService/HotelHandler.asmx/InsertLabel", '{"name":"' + name + '","empId":"' + <%=User.Identity.Name %> + '"}', function (result) {
                if (result.d.IsSuccess) {
                    hhl.notify.success(result.d.Message, "提示")
                    parent.window.location.reload();
                } else {
                    hhl.notify.warn(result.d.Message, "提示")
                }
            });
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel">
        <div class="panel-body">
            <table id="tblSystem" class="table table-bordered">
                <tr>
                    <td>标签名称</td>
                    <td>
                        <input type="text" id="txtName" name="txtName" class="form-control" /></td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
