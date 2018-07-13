<%@ Page Title="编辑客户" Language="C#" MasterPageFile="~/AdminWorkArea/Master/PopuMaster.Master" AutoEventWireup="true" CodeBehind="FL_CustomerModify.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Flow.Customer.FL_CustomerModify" %>

<%@ Register Assembly="HHLWedding.Control" Namespace="HHLWedding.Control" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .radio-list > tbody > tr > td > label {
            font-weight: normal;
            padding-left: 3px;
        }
    </style>

    <script src="/Content/js/Common.js"></script>
    <script type="text/javascript">

        var height = $(".widget-content").css("height");
        $(function () {

            //选择渠道类型来绑定渠道
            $("#ddlSaleType").change(function () {

                $("#ddlSaleSource").empty();
                var type = $("#ddlSaleType").val();
                if (type > 0) {
                    hhl.ajax("/WebService/BaseHandler.asmx/GetSaleSourceByType", '{"SaleTypeId":"' + type + '"}', function (result) {
                        if (result.d.IsSuccess) {
                            var saleSourceList = result.d.Data;
                            if (saleSourceList.length > 0) {
                                for (var i = 0; i < saleSourceList.length; i++) {
                                    var m_saleSource = saleSourceList[i];
                                    $("#ddlSaleSource").append($("<option></option>").text(m_saleSource.SourceName).val(m_saleSource.SourceId));
                                }
                            }
                        }
                    });
                }

                $("#ddlSaleSource").append($("<option></option>").text("请选择").val("0"));

            });

            angle.isMore = true;

            //日期时间
            $("#txtPartyDate").jeDate({
                //dateCell: "#start",//isinitVal:true,
                format: "YYYY-MM-DD",
                isTime: false, //isClear:false,
                isClear: true,                        //是否显示清空
                isToday: true,                        //是否显示今天或本月
                ishmsVal: false,
                festival: true,                       //是否显示节日
            })

            $("#btnConfirm").click(function () {
                var perFix = "ContentPlaceHolder1_";

                var conType = $("#rdoContactMan input[type='radio']:checked").val();
                if (conType == 0 && $("#" + perFix + "txtGroomPhone").val() == "") {
                    hhl.notify.error("主要联系人(新郎)手机号码不能为空", "提示");
                    return false;
                } else if (conType == 1 && $("#" + perFix + "txtBridePhone").val() == "") {
                    hhl.notify.error("主要联系人手机(新娘)号码不能为空", "提示");
                    return false;
                } else if (conType == 2 && $("#" + perFix + "txtOperatorPhone").val() == "") {
                    hhl.notify.error("主要联系人(经办人)手机号码不能为空", "提示");
                    return false;
                }

                if ($("#ddlSaleType").val() == "0") {
                    layer.msg("请选择渠道类型");
                    return false;
                } else if ($("#ddlSaleSource").val() == "0" || $("#ddlSaleSource").val() == null) {
                    layer.msg("请选择渠道名称");
                    return false;
                }

                var customer = {
                    customer: {
                        Bride: $("#" + perFix + "txtBride").val(),
                        Groom: $("#" + perFix + "txtGroom").val(),
                        Operator: $("#" + perFix + "txtOperator").val(),
                        BridePhone: $("#" + perFix + "txtBridePhone").val(),
                        GroomPhone: $("#" + perFix + "txtGroomPhone").val(),
                        OperatorPhone: $("#" + perFix + "txtOperatorPhone").val(),
                        Hotel: $("#ddlHotel option:selected").val(),
                        PartyDate: $("#" + perFix + "txtPartyDate").val(),
                        SaleType: $("#ddlSaleType option:selected").val(),
                        Channel: $("#ddlSaleSource option:selected").val(),
                        ReCommand: $("#" + perFix + "txtReCommand").val(),
                        DesckCount: $("#" + perFix + "txtDeskCount").val(),
                        Budget: $("#" + perFix + "txtBudget").val(),
                        IsVip: $("#rdoVip input[type='radio']:checked").val(),
                        BanqueType: $("#rdoBanqueType input[type='radio']:checked").val(),
                        Description: $("#txtDescription").val()
                    },
                    contactType: $("#rdoContactMan input[type='radio']:checked").val(),
                    customerId: '<%=Request["CustomerId"] %>'
                };
                console.log(JSON.stringify(customer));
                hhl.ajax("/WebService/Flow/CustomerHandler.asmx/ModifyCustomer", customer, hhl.callback, 1);

            });

        });





    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel">
        <div class="panel-body">
            <div class="widget-box">
                <div class="widget-header">
                    <h4 class="h-title pull-left"><i class="icon-reorder"></i><b>客户详情</b></h4>
                    <div class="box box0 pull-right">
                        <div class="div_angle">
                            <span class="span-angel">
                                <i id="icon_angle" class="icon-angle icon-angle-up"></i>
                            </span>
                        </div>
                    </div>
                </div>


                <div class="widget-content" style="height: auto;">
                    <table id="tblSystem" class="table table-bordered table-no-border">
                        <tr class="form-group">
                            <td>主要联系人</td>
                            <td>
                                <asp:RadioButtonList CssClass="radio-list" runat="server" ID="rdoContactMan" name="rdoContactMan" RepeatDirection="Horizontal" Width="220px" RepeatLayout="Table" BorderStyle="None" CellPadding="5" CellSpacing="10" ClientIDMode="Static">
                                    <asp:ListItem Text="新郎" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="新娘" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="经办人" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr class="form-group">
                            <td class="tr-width-2"><span class="control-label">新娘</span></td>
                            <td class="col-width-2">
                                <input type="text" class="form-control col-width-8" runat="server" id="txtBride" name="txtBride" /></td>
                            <td class="tr-width-2"><span class="control-label">新郎</span></td>
                            <td class="col-width-2">
                                <input type="text" class="form-control col-width-8" runat="server" id="txtGroom" name="txtGroom" /></td>
                            <td class="tr-width-2"><span class="control-label">经办人</span></td>
                            <td class="col-width-2">
                                <input type="text" class="form-control col-width-8" runat="server" id="txtOperator" name="txtOperator" /></td>
                        </tr>
                        <tr class="form-group">
                            <td><span class="control-label">新娘联系电话</span></td>
                            <td>
                                <input type="text" class="form-control col-width-8" runat="server" id="txtBridePhone" name="txtBridePhone" /></td>
                            <td><span class="control-label">新郎联系电话</span></td>
                            <td>
                                <input type="text" class="form-control col-width-8" runat="server" id="txtGroomPhone" name="txtGroomPhone" /></td>
                            <td><span class="control-label">经办人联系电话</span></td>
                            <td>
                                <input type="text" class="form-control col-width-8" runat="server" id="txtOperatorPhone" name="txtOperatorPhone" /></td>
                        </tr>
                        <tr class="form-group">
                            <td><span class="control-label">酒店</span></td>
                            <td>
                                <cc1:ddlHotel runat="server" ID="ddlHotel" ClientIDMode="Static" /></td>
                            <td><span class="control-label">婚期</span></td>
                            <td>
                                <input class="form-control col-width-8 txtDate" runat="server" id="txtPartyDate" name="txtPartyDate" /></td>
                            <td><span class="control-label">录入日期</span></td>
                            <td>
                                <input class="form-control col-width-8 input-disable" runat="server" id="txtCreateDate" name="txtCreateDate" /></td>
                        </tr>
                        <tr class="form-group">
                            <td><span class="control-label">渠道类型</span></td>
                            <td>
                                <cc1:ddlSaleType runat="server" ID="ddlSaleType" ClientIDMode="Static" />
                            </td>
                            <td><span class="control-label">渠道名称</span></td>
                            <td>
                                <cc1:ddlSaleSource runat="server" ID="ddlSaleSource" ClientIDMode="Static" />
                            </td>
                            <td><span class="control-label">推荐人</span></td>
                            <td>
                                <input class="form-control col-width-8" runat="server" id="txtReCommand" name="txtReCommand" /></td>

                        </tr>
                        <tr class="form-group">
                            <td><span class="control-label">桌数</span></td>
                            <td>
                                <input class="form-control col-width-8" runat="server" id="txtDeskCount" name="txtDeskCount" /></td>
                            <td><span class="control-label">婚礼预算</span></td>
                            <td>
                                <input class="form-control col-width-8" runat="server" id="txtBudget" name="txtBudget" /></td>
                            <td><span class="control-label">录入人</span></td>
                            <td>
                                <input class="form-control col-width-8 input-disable" runat="server" id="txtCreateEmployee" name="txtCreateEmployee" /></td>
                        </tr>
                        <tr class="form-group">
                            <td>
                                <img src="/images/vip.png" /></td>
                            <td>
                                <asp:RadioButtonList CssClass="radio-list" runat="server" ID="rdoVip" name="rdoIsVip" RepeatDirection="Horizontal" Width="220px" RepeatLayout="Table" BorderStyle="None" CellPadding="0" CellSpacing="10" ClientIDMode="Static">
                                    <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="否" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td><span class="control-label">宴会时间</span></td>
                            <td>
                                <asp:RadioButtonList CssClass="radio-list" runat="server" ID="rdoBanqueType" name="rdoBanType" RepeatDirection="Horizontal" Width="220px" RepeatLayout="Table" BorderStyle="None" CellPadding="0" CellSpacing="10" ClientIDMode="Static">
                                    <asp:ListItem Text="午宴" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="晚宴" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td><span class="control-label">状态进度</span></td>
                            <td>
                                <input class="form-control col-width-8 input-disable" runat="server" id="txtState" name="txtState" /></td>
                        </tr>
                        <tr class="form-group">
                            <td><span class="control-label">电销</span></td>
                            <td>
                                <input class="form-control col-width-8 input-disable" runat="server" id="txtInviteEmployee" name="txtInviteEmployee" /></td>
                            <td><span class="control-label">统筹师</span></td>
                            <td>
                                <input class="form-control col-width-8 input-disable" runat="server" id="txtOrderEmployee" name="txtOrderEmployee" /></td>
                            <td><span class="control-label">策划师</span></td>
                            <td>
                                <input class="form-control col-width-8 input-disable" runat="server" id="txtQuotedEmployee" name="txtQuotedEmployee" /></td>
                        </tr>
                        <tr class="form-group">
                            <td><span class="control-label">是否已完成</span></td>
                            <td>
                                <input class="form-control col-width-8 input-disable" runat="server" id="txtIsFinish" name="txtIsFinish" /></td>
                            <td><span class="control-label">是否已评价</span></td>
                            <td>
                                <input class="form-control col-width-8 input-disable" runat="server" id="txtEvalState" name="txtEvalState" /></td>

                        </tr>
                        <tr class="form-group">
                            <td><span class="control-label">说明</span></td>
                            <td colspan="5">
                                <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Rows="5" Columns="100" ClientIDMode="Static" /></td>
                        </tr>
                        <tr class="th_content">
                            <td colspan="6">
                                <button type="button" id="btnConfirm" class="btn btn-success btn-large" title="确定">确定</button>
                                <a href="FL_CustomerManager.aspx" class="btn btn-info  btn-large">返回</a>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            $(".form-control").css("width", "237px");

            $(".chosen-drop").css("width", "237px").css("border", "none");
            $(".chosen-search").addClass("form-control").css("width", "237px").css("height", "auto");
            $(".chosen-results").addClass("form-control").css("width", "237px").css("height", "auto");
            $(".chosen-single").attr("class", "form-control").css("width", "237px");

        });
    </script>
</asp:Content>
