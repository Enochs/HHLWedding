<%@ Page Title="确认到店" Language="C#" MasterPageFile="~/AdminWorkArea/Master/PopuMaster.Master" AutoEventWireup="true" CodeBehind="ConfirmCome.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Flow.Invite.ConfirmCome" %>

<%@ Register Assembly="HHLWedding.Control" Namespace="HHLWedding.Control" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/Content/js/vue/vue.js"></script>

    <script type="text/javascript">
        //选择跟单人
        function SelectEmployee() {
            hhl.dialog.open(["/AdminWorkArea/CommonForm/SelectEmployee.aspx?type=orderEmployee"], "选择责任人", {
                area: ["450px", "420px"],
                btn: ["确定", "取消"],
                skin: "layui-layer-lan",
            });
        }

        //文本框的验证
        function Check() {
            if ($("#txtBudget").val() == "" || $("#txtBudget").val() == "0") {
                layer.msg("请输入婚礼预算且不能为0");
                return false;
            }

            if ($("#txtDeskCount").val() == "" || $("#txtDeskCount").val() == "0") {
                layer.msg("请输入桌数且不能为0");
                return false;
            }

            if ($("#txtOrderEmployee").val() == "") {
                layer.msg("请选择跟单人");
                return false;
            }

            if ($("#txtOrderEmployee").val() == "") {
                layer.msg("请选择到店时间");
                return false;
            }
            return true;
        }


        //页面加载
        var m_customer = null;

        var customerId = '<%=Request["CustomerId"] %>';
        var inviteId = '<%=Request["InviteId"] %>';

        $(function () {


            $.ajax({
                async: false,
                cahce: false,
                url: "/WebService/FLow/CustomerHandler.asmx/GetByCustomerID",
                type: "post",
                contentType: hhl.config.contentTypeJson,
                data: JSON.stringify({ customerId: customerId }),
                dataType: "json",
                success: function (result) {
                    if (result.d.IsSuccess) {
                        m_customer = result.d.Data;
                    } else {
                        hhl.notify.error(result.d.Message);
                    }
                },
            })
            var vue = new Vue({
                el: "#tblSystem",
                data: { customer: m_customer },
                mounted: function () {
                    this.plus();
                },

                methods: {
                    plus: function () {
                        $(".txtDate").jeDate();
                    }
                }
            });

            //$("#txtComeDate").val(jsonDateFormat(2));

        });

        //点击提交
        function submitForm() {

            if (Check()) {

                var customer = {
                    m_customer: {
                        CustomerID: customerId,
                        Bride: m_customer.Bride,
                        BridePhone: m_customer.BridePhone,
                        Groom: m_customer.Groom,
                        GroomPhone: m_customer.GroomPhone,
                        Budget: m_customer.Budget,
                        DeskCount: m_customer.DeskCount,
                    },
                    orderEmp: $("#txtOrderEmployee").attr("title"),
                    InviteId: inviteId,
                    comeDate: $("#txtComeDate").val()
                };

                //webServic 确认到店
                hhl.ajax("/WebService/Flow/InviteHandler.asmx/ComeToStore", customer, function (result) {
                    if (result.d.IsSuccess) {
                        hhl.notify.success(result.d.Message, "提示");
                        setTimeout(function () {
                            var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                            parent.layer.close(index);

                            parent.window.location.href = "/AdminWorkArea/Flow/Order/FollowOrderList.aspx";
                        }, 1000);
                    } else {
                        hhl.notify.error(result.d.Message, "提示");
                    }
                }, 1)

            }
        }

    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel">
        <div class="panel-body">
            <table id="tblSystem" class="table table-bordered">
                <tr>
                    <td>状态</td>
                    <td><span>确认到店</span></td>
                </tr>
                <tr>
                    <td>新娘姓名</td>
                    <td>
                        <input type="text" v-model="customer.Bride" id="txtBrideName" class="form-control" placeholder="请输入新娘姓名" /></td>
                </tr>
                <tr>
                    <td>新娘联系电话</td>
                    <td>
                        <input type="text" v-model="customer.BridePhone" id="txtBridePhone" class="form-control" placeholder="请输入新娘联系电话" /></td>
                </tr>
                <tr>
                    <td>新郎姓名</td>
                    <td>
                        <input type="text" v-model="customer.Groom" id="txtGroomName" class="form-control" placeholder="请输入新郎姓名" /></td>
                </tr>
                <tr>
                    <td>新郎联系电话</td>
                    <td>
                        <input type="text" v-model="customer.GroomPhone" id="txtGroomPhone" class="form-control" placeholder="请输入新郎联系电话" /></td>
                </tr>
                <tr>
                    <td><span style="color: red; font-size: 14px;" title="必填">※</span>婚礼预算(必填)</td>
                    <td>
                        <input type="text" v-model="customer.Budget" id="txtBudget" class="form-control" placeholder="请输入婚礼预算" />
                    </td>
                </tr>
                <tr>
                    <td><span style="color: red; font-size: 14px;" title="必填">※</span>桌数(必填)</td>
                    <td>
                        <input type="text" v-model="customer.DeskCount" id="txtDeskCount" class="form-control" placeholder="请输入桌数" />
                    </td>
                </tr>
                <tr>
                    <td><span style="color: red; font-size: 14px;" title="必填">※</span>跟单人(必填)</td>
                    <td>
                        <input type="text" class="form-control bg-fff" id="txtOrderEmployee" onclick="SelectEmployee()" placeholder="请选择跟单人" readonly value="" title="0" /></td>
                </tr>
                <tr>
                    <td><span style="color: red; font-size: 14px;" title="必填">※</span>到店时间(必填)</td>
                    <td>
                        <input type="text"   id="txtComeDate" class="form-control txtDate" placeholder="请选择到店时间"  value="<%=DateTime.Now.ToString("yyyy-MM-dd") %>" />
            

                    </td>
                </tr>
                
            </table>
        </div>
    </div>
</asp:Content>
