<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/PopuMaster.Master" AutoEventWireup="true" CodeBehind="SaleSourceCreate.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.SaleSource.SaleSourceCreate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Content/js/Number.js" type="text/javascript"></script>
    <script src="/Scripts/RightMenu/jquery.contextmenu.r2.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            $("#txtSaleSource").val("喜来登大酒店");
            $("#ddlSaleType").val("1");
            $("#txtSourceAddress").val("重庆市南岸区");
            $("#txtCommandName").val("喜喜");
            $("#txtPhone").val("13883655842");
            $("#txtBankName").val("工商银行");
            $("#txtBankCard").val("62225223655425412");
        });

        function submitForm() {

            if (validate()) {
                var saleSource = '{"saleSource":{"SourceName":"' + $("#txtSaleSource").val() + '","SaleTypeId":"' + $("#ddlSaleType").val() + '","IsRebate":"' + $("input[name='rdoReabate']:checked").val() + '","SourceAddress":"' + $("#txtSourceAddress").val() + '","CommoandName":"' + $("#txtCommandName").val() + '","CommondPhone":"' + $("#txtPhone").val() + '","CommondBankName":"' + $("#txtBankName").val() + '","CommondBankCard":"' + $("#txtBankCard").val() + '","Description":"' + $("#txtDescription").val() + '"}}';

                hhl.ajax("/WebService/SaleSourceHandler.asmx/CreateSaleSource", saleSource, hhl.callbackRefresh)
            }
        }

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
            <table id="tblSystem" class="table table-bordered" >
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
                        <input type="radio" class="radio radio-inline" name="rdoReabate" value="1" checked="checked" />是
                        <input type="radio" class="radio radio-inline" name="rdoReabate" value="0" />否
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
