<%@ Page Title="添加频道" Language="C#" MasterPageFile="~/AdminWorkArea/Master/PopuMaster.Master" AutoEventWireup="true" CodeBehind="Sys_ChannelCreate.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Channel.Sys_ChannelCreate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function submitForm() {

            if ($("#txtChannelName").val() == "") {
                hhl.notify.warn("频道名称不能为空", "提示");
                return false;
            } else if ($("#txtChannelAddress").val() == "") {
                hhl.notify.warn("路径不能为空", "提示");
                return false;
            } else if ($("#txtChannelGetType").val() == "") {
                hhl.notify.warn("模块类名不能为空", "提示");
                return false;
            }
            var channel = '{"channel": {"ChannelName":"' + $("#txtChannelName").val() + '","ChannelAddress":"' + $("#txtChannelAddress").val() + '","ChannelGetType":"' + $("#txtChannelGetType").val() + '","StyleSheethem":"' + $("#ddlAllParentSystem option:selected").text() + '","SortInt":"' + $("#txtSort").val() + '","Remark":"' + $("#txtRemark").val() + '","Parent":"' + $("#ddlAllParentSystem").val() + '"}}';
            hhl.ajax("/WebService/ChannelHandler.asmx/CreateChannel", channel,hhl.callbackRefresh);

        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel">
        <div class="panel-body col-md-12">
            <table id="tblSystem" class="table table-bordered">
                <tr>
                    <td>频道名称</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtChannelName" ClientIDMode="Static" CssClass="form-control" /></td>
                </tr>
                <tr>
                    <td>所属系统</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlAllParentSystem" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>路径</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtChannelAddress" ClientIDMode="Static" CssClass="form-control" /></td>
                </tr>
                <tr>
                    <td>排序</></td>
                    <td>
                        <asp:TextBox runat="server" ID="txtSort" ClientIDMode="Static" CssClass="form-control" /></td>
                </tr>
                <tr>
                    <td style="vertical-align: top;">模块类名</td>
                    <td style="vertical-align: top;">
                        <asp:TextBox runat="server" ID="txtChannelGetType" ClientIDMode="Static" CssClass="form-control" /></td>
                </tr>
                <tr>
                    <td style="vertical-align: top;">备注</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtRemark" TextMode="MultiLine" ClientIDMode="Static" CssClass="form-control" Rows="4" Columns="28"></asp:TextBox></td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
