<%@ Page Title="添加客户" Language="C#" MasterPageFile="~/AdminWorkArea/Master/PopuMaster.Master" AutoEventWireup="true" CodeBehind="FL_CustomerCreate.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Flow.Customer.FL_CustomerCreate" %>

<%@ Register Assembly="HHLWedding.Control" Namespace="HHLWedding.Control" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">



        angle.isMore = true;
        $(function () {
            //从列表进入添加页面  可以有返回按钮
            var type = '<%=Request["Type"] %>';

            if (type == 'add') {
                $("#btnBack").removeClass("hide");
            }

            var chars = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];

            //手机号后缀
            function CreateSuffix(n) {
                var res = "";
                for (var i = 0; i < n; i++) {
                    var phone = Math.ceil(Math.random() * 10);
                    if (phone >= 10) phone = 0;
                    if (phone == 0 && i == 0) phone = 9;
                    res += chars[phone];
                }
                return res;
            }
            //手机号前缀
            var fix = ['13', '15', '18']

            function CreatePreFix() {
                var num = Math.ceil(Math.random() * 10);
                if (num >= 3) var phone = num % 3;
                else phone = num;
                res = fix[phone];
                return res;
            }

            $(".tr_Groom").show();
            $(".tr_Bride").hide();
            $(".tr_Operator").hide();

            $(".txtGroomPhone").val(CreatePreFix() + CreateSuffix(9));

            //选择状态(加载)
            var state = $(".sel_customerState").val();
            $(".sel_customerState").change(function () {
                state = $(this).val();
                if (state == 5) {       //确认到店
                    $(".tr_orderEmp").removeClass("hide");
                } else {
                    $(".tr_orderEmp").addClass("hide");
                }
            });

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

            //日期时间
            $("#partyDate").jeDate({
                //dateCell: "#start",//isinitVal:true,
                format: "YYYY-MM-DD",
                isTime: false, //isClear:false,
                isClear: true,                        //是否显示清空
                isToday: true,                        //是否显示今天或本月
                ishmsVal: false,
                festival: true,                       //是否显示节日
            })


            //选择客户类型事件
            $(".chkCus").on("click", function (e) {
                var chk = $(this).is(":checked");
                var length = $(".chkCus:checked").length;
                if (length > 0) {
                    if ($(this).hasClass("chkBride")) {
                        if (chk == true) {
                            $(".tr_Bride").show();
                            $(".txtBridePhone").val(CreatePreFix() + CreateSuffix(9));
                        } else if (chk == false) {
                            $(".tr_Bride").hide();
                            $(".txtBridePhone").val('');
                        }
                    } else if ($(this).hasClass("chkGroom")) {
                        if (chk == true) {
                            $(".tr_Groom").show();
                            $(".txtGroomPhone").val(CreatePreFix() + CreateSuffix(9));
                        } else if (chk == false) {
                            $(".tr_Groom").hide();
                            $(".txtGroomPhone").val('');
                        }
                    }
                    else if ($(this).hasClass("chkOperator")) {
                        if (chk == true) {
                            $(".tr_Operator").show();
                            $(".txtOperatorPhone").val(CreatePreFix() + CreateSuffix(9));
                        } else if (chk == false) {
                            $(".tr_Operator").hide();
                            $(".txtOperatorPhone").val('');
                        }
                    }
                } else {
                    this.checked = true;
                }
            });


            //验证手机号码
            function checkPhone() {
                var res = true;
                var resNum = 0;
                var message = "手机号码验证通过";
                $("#tblSystem tr").each(function () {
                    if ($(this).hasClass("Customer")) {
                        var phone = $(this).find("td").eq(3).find("input[name='txtisNum']").val();
                        if (phone != "" && phone != undefined) {
                            if (isRegPhone().test(phone)) {
                                if (res == false) {
                                    res = false;
                                } else {    //验证通过  判断号码是否已经存在
                                    //同步
                                    $.ajax({
                                        async: false,
                                        cache: false,
                                        datatype: 'text',
                                        type: 'post',
                                        data: '{"phone":' + phone + '}',
                                        contentType: "application/json",
                                        url: '/WebService/Flow/CustomerHandler.asmx/IsExistsPhone',
                                        success: function (result) {
                                            resNum = result.d;
                                            if (resNum == 1) {
                                                message = "手机号码已经存在,请重新输入";
                                                res = false;
                                                return false;
                                            } if (resNum == 2) {
                                                message = "手机号码格式不正确";
                                                res = false;
                                                return false;
                                            } else {
                                                message = "手机号码验证通过";
                                                resNum = 0;
                                                res = true;
                                            }
                                        }
                                    });
                                }
                            } else {
                                resNum = 2;
                                message = "手机号码格式不正确";
                                res = false;
                            }
                        }
                    }
                });
                return res + "|" + resNum + "|" + message;
            }

            //点击保存
            $("#btnSave").on("click", function () {

                //手机号码验证
                var res = checkPhone().split('|');

                //电话验证通过
                if (res[0] == "true") {


                    if ($("#ddlSaleType").val() == "0") {
                        layer.msg("请选择渠道类型");
                        return false;
                    } else if ($("#ddlSaleSource").val() == "0" || $("#ddlSaleSource").val() == null) {
                        layer.msg("请选择渠道名称");
                        return false;
                    }
                    state = $(".sel_customerState").val();
                    //确认到店 需要选择跟单人
                    if (state == 5) {
                        if ($("#txtOrderEmployee").attr("title") == 0) {
                            layer.msg("请选择跟单人");
                            return false;
                        }
                    }

                    var customer = {
                        customer: {
                            Bride: $(".txtBride").val(),
                            Groom: $(".txtGroom").val(),
                            Operator: $(".txtOperator").val(),
                            BridePhone: $(".txtBridePhone").val(),
                            GroomPhone: $(".txtGroomPhone").val(),
                            OperatorPhone: $(".txtOperatorPhone").val(),
                            PartyDate: $("#partyDate").val(),
                            Hotel: $("#ddlHotel option:selected").val(),
                            SaleType: $("#ddlSaleType option:selected").val(),
                            Channel: $("#ddlSaleSource option:selected").val(),
                            Type: $(".sel_Type").val(),
                            State: $(".sel_customerState").val(),
                            IsVip: $(".rdoIsVip:checked").val(),
                            BanqueType: $(".rdoBanqueType:checked").val(),
                            Description: $(".txtDescription").val()
                        },
                        contactType: $(".rdoContactMan:checked").val(),
                        orderEmp: $("#txtOrderEmployee").attr("title")
                    };

                    hhl.ajax("/WebService/Flow/CustomerHandler.asmx/CreateCustomer", customer, hhl.callback, 1);

                } else {
                    layer.msg(res[2]);
                    return false;
                }

            });

        });

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .top-sel {
            width: 200px;
        }

        li .solar {
            color: #27d193;
        }

        li .lunar {
            color: #ff6a00;
        }

        #tblSystem tr td input[type='text'] {
            width: 235px;
        }
    </style>

    <div class="panel">
        <div class="panel-body">
            <div class="widget-box">
                <div class="widget-header">
                    <h4 class="h-title pull-left"><i class="icon-reorder"></i><b>添加客户</b></h4>
                    <div class="box pull-right">
                        <div class="div_angle">
                            <span class="span-angel">
                                <i id="icon_angle" class="icon-angle icon-angle-up"></i>
                            </span>
                        </div>
                    </div>
                </div>

                <div class="widget-content">

                    <table id="tblSystem" class="table table-bordered" style="width: 60%; margin-left: 10px;">
                        <tr>
                            <td>客户状态</td>
                            <td>
                                <select class="form-control top-sel sel_customerState">
                                    <option value="1">未邀约</option>
                                    <option value="2">邀约中</option>
                                    <option value="5">确认到店</option>
                                </select>

                            </td>
                            <td colspan="2">
                                <select class="form-control top-sel sel_Type">
                                    <option value="1">婚宴</option>
                                    <option value="2">生日宴</option>
                                    <option value="3">宝宝宴</option>
                                </select>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <hr style="width: 100%; color: gray;" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_cusType" colspan="4">
                                <input type="checkbox" class="checkbox-inline chkCus chkGroom" checked="checked" /><span class="span_groom">新郎</span>
                                <input type="checkbox" class="checkbox-inline chkCus chkBride" /><span class="span_bride">新娘</span>
                                <input type="checkbox" class="checkbox-inline chkCus chkOperator" /><span class="span_operator">经办人</span>
                            </td>
                        </tr>
                        <tr class="tr_Groom Customer">
                            <td>新郎</td>
                            <td>
                                <input type="text" class="form-control txtGroom" /></td>
                            <td>联系电话</td>
                            <td>
                                <input type="text" name="txtisNum" class="form-control txtphone txtGroomPhone" />
                                <span style="color: red;" class="hide has-error-es"></span>
                            </td>
                        </tr>
                        <tr class="tr_Bride Customer">
                            <td>新娘</td>
                            <td>
                                <input type="text" class="form-control txtBride" /></td>
                            <td>联系电话</td>
                            <td>
                                <input type="text" name="txtisNum" class="form-control txtphone txtBridePhone" />
                                <span style="color: red;" class="hide has-error-es">手机号码格式错误</span>
                            </td>
                        </tr>

                        <tr class="tr_Operator Customer">
                            <td>经办人</td>
                            <td>
                                <input type="text" class="form-control txtOperator" /></td>
                            <td>联系电话</td>
                            <td>
                                <input type="text" name="txtisNum" class="form-control txtphone txtOperatorPhone" />
                                <span style="color: red;" class="hide has-error-es">手机号码格式错误</span>
                            </td>
                        </tr>
                        <tr>
                            <td>婚期</td>
                            <td>
                                <input class="wicon form-control" id="partyDate" name="partyDate" type="text" placeholder="婚期" value="" /></td>
                            <td>酒店</td>
                            <td>
                                <cc1:ddlHotel runat="server" ID="ddlHotel" ClientIDMode="Static" />
                            </td>
                        </tr>
                        <tr>
                            <td>渠道类型</td>
                            <td>
                                <cc1:ddlSaleType runat="server" ID="ddlSaleType" ClientIDMode="Static" />
                            </td>
                            <td>渠道名称</td>
                            <td>
                                <cc1:ddlSaleSource runat="server" ID="ddlSaleSource" ClientIDMode="static" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <img src="/images/vip.png" /></td>
                            <td>
                                <input type="radio" name="rdoVip" class="radio radio-inline rdoIsVip" checked="checked" value="1" /><span class="span_isVip" style="margin-right: 20px;">是</span>
                                <input type="radio" name="rdoVip" class="radio radio-inline rdoIsVip" value="0" /><span class="span_noVip">否</span>
                            </td>
                            <td>宴会时间</td>
                            <td>
                                <input type="radio" name="rdoBanqueType" class="radio radio-inline rdoBanqueType" checked="checked" value="1" /><span class="span_lunch" style="margin-right: 20px;">午宴</span>
                                <input type="radio" name="rdoBanqueType" class="radio radio-inline rdoBanqueType" value="2" /><span class="span_dinner">晚宴</span></td>
                        </tr>
                        <tr>
                            <td>主要联系人</td>
                            <td>
                                <input type="radio" name="rdoContactMan" class="radio radio-inline rdoContactMan" value="0" checked="checked" /><span class="spanBride" style="margin-right: 20px;">新郎</span>
                                <input type="radio" name="rdoContactMan" class="radio radio-inline rdoContactMan" value="1" /><span class="spanGroom" style="margin-right: 20px;">新娘</span>
                                <input type="radio" name="rdoContactMan" class="radio radio-inline rdoContactMan" value="2" /><span class="spanOperator">经办人</span>
                            </td>
                        </tr>

                        <tr>
                            <td>说明</td>
                            <td colspan="3">
                                <textarea rows="3" style="width: 559px; max-width: 559px;" class="form-control txtDescription"></textarea></td>
                        </tr>
                        <tr class="tr_orderEmp hide">
                            <td>跟单人</td>
                            <td colspan="3">
                                <input type="text" name="txtorderEmployee" id="txtOrderEmployee" class="form-control pull-left" onclick="SelectEmployee()" style="margin-right: 5px" title="0" readonly />
                                <button type="button" class="btn btn-info btn-sm" onclick="SelectEmployee()">选择跟单人</button>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="4">
                                <button type="button" id="btnSave" title="添加" data-toggle="tooltip" class="btn btn-success btn-sm">添加</button>
                                <button type="reset" id="btnClear" title="清空" data-toggle="tooltip" class="btn btn-danger btn-sm">清空</button>
                                <a class="btn btn-primary btn-sm hide" id="btnBack" href="FL_CustomerManager.aspx" title="返回" data-toggle="tooltip">返回</a>
                            </td>
                        </tr>
                    </table>


                </div>
            </div>
        </div>
    </div>


    <script type="text/javascript">

        $(function () {

            $(".chosen-drop").css("width", "235px").css("border", "none");
            $(".chosen-search").addClass("form-control").css("width", "235px").css("height", "auto");
            $(".chosen-results").addClass("form-control").css("width", "235px").css("height", "auto");
            $(".chosen-single").attr("class", "form-control").css("width", "235px");
        });

        //选择跟单人
        function SelectEmployee() {
            hhl.dialog.open(["/AdminWorkArea/CommonForm/SelectEmployee.aspx?type=orderEmployee"], "选择责任人", {
                area: ["450px", "420px"],
                btn: ["确定", "取消"],
            });
        }

    </script>
</asp:Content>
