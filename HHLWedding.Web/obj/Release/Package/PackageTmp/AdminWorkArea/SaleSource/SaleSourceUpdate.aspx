<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/PopuMaster.Master" AutoEventWireup="true" CodeBehind="SaleSourceUpdate.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.SaleSource.SaleSourceUpdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <script type="text/javascript">
        $(function () {
            var SaleSourceId = '<%=Request["SourceId"] %>'
            hhl.ajax("/WebService/SaleSourceHandler.asmx/GetSaleSource", '{"SaleSourceId":"' + SaleSourceId + '"}', function (result) {
                if (result.d.IsSuccess) {
                    var source = result.d.Data;
                    if (source != null) {

                        $("#rdoReabte" + source.IsRebate).prop("checked", "checked");
                        $("#txtSaleSource").val(source.SourceName);
                        $("#ddlSaleType").val(source.SaleTypeId);
                        $("#txtSourceAddress").val(source.SourceAddress);
                        $("#txtCommandName").val(source.CommoandName);
                        $("#txtPhone").val(source.CommondPhone);
                        $("#txtBankName").val(source.CommondBankName);
                        $("#txtBankCard").val(source.CommondBankCard);
                        $("#txtDescription").val(source.Description);
                    }
                } else {
                    hhl.notify.error(result.d.Message, "提示");
                }
            });
        });

        //提交
        function submitForm() {
            if (validate()) {
                var SaleSourceId = '<%=Request["SourceId"] %>'
                var saleSource = '{"saleSource":{"SourceName":"' + $("#txtSaleSource").val() + '","SaleTypeId":"' + $("#ddlSaleType").val() + '","IsRebate":"' + $("input[name='rdoReabate']:checked").val() + '","SourceAddress":"' + $("#txtSourceAddress").val() + '","CommoandName":"' + $("#txtCommandName").val() + '","CommondPhone":"' + $("#txtPhone").val() + '","CommondBankName":"' + $("#txtBankName").val() + '","CommondBankCard":"' + $("#txtBankCard").val() + '","Description":"' + $("#txtDescription").val() + '"},"SourceId":"' + SaleSourceId + '"}';
                hhl.ajax("/WebService/SaleSourceHandler.asmx/UpdateSaleSource", saleSource, hhl.callbackRefresh)
            }
        }

        //文本框验证
        function validate() {
            if ($("#txtSaleSource").val() == "") {
                hhl.notify.error("请输入渠道名称", "提示");
                return false;
            } else if ($("#ddlSaleType option:selected").val() == "" - 1) {
                hhl.notify.error("请选择渠道类型", "提示");
                return false;
            } else if ($("#txtSourceAddress").val() == "") {
                hhl.notify.error("请输入地址", "提示");
                return false;
            } else if ($("#txtCommandName").val() == "") {
                hhl.notify.error("请输入推荐人姓名", "提示");
                return false;
            } else if ($("#txtPhone").val() == "") {
                hhl.notify.error("请输入联系电话", "提示");
                return false;
            } else if ($("#txtBankName").val() == "") {
                hhl.notify.error("请输入开户银行名称", "提示");
                return false;
            } else if ($("#txtBankCard").val() == "") {
                hhl.notify.error("请输入银行卡号", "提示");
                return false;
            } else {
                return true;
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel">
        <div class="panel-body col-md-12">
            <table id="tblSystem" class="table table-bordered">
                <tr>
                    <td>渠道名称</td>
                    <td>
                        <input type="text" class="form-control" id="txtSaleSource" name="txtSaleSource" /></td>
                </tr>
                <tr>
                    <td>渠道类型</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlSaleType" CssClass="form-control" ClientIDMode="Static" DataTextField="Text" DataValueField="Value" /></td>
                </tr>
                <tr>
                    <td>是否返利</td>
                    <td>
                        <input type="radio" class="radio radio-inline" id="rdoReabte1" name="rdoReabate" value="1" />是
                        <input type="radio" class="radio radio-inline" id="rdoReabte0" name="rdoReabate" value="0" />否
                    </td>
                </tr>
                <tr>
                    <td>地址</td>
                    <td>
                        <input type="text" class="form-control" id="txtSourceAddress" name="txtSourceAddress" /></td>
                </tr>
                <tr>
                    <td>推荐人姓名</td>
                    <td>
                        <input type="text" class="form-control" id="txtCommandName" name="txtCommandName" /></td>
                </tr>
                <tr>
                    <td>联系电话</td>
                    <td>
                        <input type="text" class="form-control" id="txtPhone" name="txtisNum" /></td>
                </tr>
                <tr>
                    <td>开户银行</td>
                    <td>
                        <input type="text" class="form-control" id="txtBankName" /></td>
                </tr>
                <tr>
                    <td>银行卡号</td>
                    <td>
                        <input type="text" class="form-control" id="txtBankCard" name="txtisNum" /></td>
                </tr>
                <tr>
                    <td>说明</td>
                    <td>
                        <textarea rows="5" class="form-control" id="txtDescription" name="txtDescription"></textarea></td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
