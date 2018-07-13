<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/PopuMaster.Master" AutoEventWireup="true" CodeBehind="HotelLabelUpdate.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Foundation.Hotel.HotelLabelUpdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        //提交页面
        function submitForm() {
            var labelId = '<%=Request["labelId"] %>';
            var name = $("#txtName").val();
            if (name == "") {
                hhl.notify.warn("请输入标签名称", "提示");
                return false;
            }

            hhl.ajax("/WebService/HotelHandler.asmx/UpdateLabel", '{"name":"' + name + '","labelId":"' + labelId + '"}', function (result) {
                if (result.d.IsSuccess) {
                    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                    parent.layer.close(index);
                } else {
                    hhl.notify.warn(result.d.Message, "提示")
                }
            });
        }

        //页面加载
        $(function () {
            hhl.ajax("/WebService/HotelHandler.asmx/GetByLabelId", '{"labelId":"' + <%=Request["labelId"] %> + '"}', function (result) {
                if (result.d.IsSuccess) {
                    $("#txtName").val(result.d.Value);
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
                    <td>标签名称</td>
                    <td>
                        <input type="text" id="txtName" name="txtName" class="form-control" /></td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
