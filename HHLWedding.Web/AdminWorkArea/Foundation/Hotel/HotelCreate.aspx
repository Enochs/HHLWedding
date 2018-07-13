<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/PopuMaster.Master" AutoEventWireup="true" CodeBehind="HotelCreate.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Foundation.Hotel.HotelCreate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%-- 编辑器 必须引用 --%>
    <script type="text/javascript" charset="utf-8" src="/Content/ueditor/ueditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="/Content/ueditor/ueditor.all.min.js"></script>

    <script type="text/javascript" charset="utf-8" src="/Content/ueditor/lang/zh-cn/zh-cn.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel">
        <div class="panel-body col-md-12">
            <table id="tbl-border" class="table table-bordered">
                <tr>
                    <td>酒店名称</td>
                    <td>
                        <input type="text" id="txtHotelName" name="txtHotelName" class="form-control" data-val-required="酒店名称是必须的" /></td>
                    <td>酒店类型</td>
                    <td>
                        <asp:DropDownList runat="server" ID="selHotelType" CssClass="form-control" ClientIDMode="Static" DataValueField="Value" DataTextField="Text" />
                    </td>
                </tr>
                <tr>
                    <td>区域</td>
                    <td>
                        <input type="text" id="txtArea" name="txtArea" class="form-control" /></td>
                    <td>详细地址</td>
                    <td>
                        <input type="text" id="txtAddress" name="txtAddress" class="form-control" /></td>
                </tr>
                <tr>
                    <td>联系电话</td>
                    <td>
                        <input type="text" id="txtPhone" name="txtPhone" class="form-control" /></td>
                    <td>桌数</td>
                    <td>
                        <input type="text" id="txtDeskCount" name="txtDeskCount" class="form-control" /></td>
                </tr>
                <tr>
                    <td>餐标</td>
                    <td>
                        <div class="form-group form-inline">
                            <input type="text" id="txtStartMoney" name="txtStartMoney" style="width: 135px;" class="col-sm2 form-control" />
                            -
                        <input type="text" id="txtEndMoney" name="txtEndMoney" style="width: 135px;" class="col-sm2 form-control" />
                        </div>
                    </td>

                    <td>排序</td>
                    <td>
                        <input type="text" id="txtSort" name="txtSort" class="form-control" />
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: middle">场地标签</td>
                    <td colspan="3">
                        <dl id="dlLabel">
                            <asp:Repeater runat="server" ID="repLabel">
                                <ItemTemplate>
                                    <dd id='<%#Eval("LabelID") %>'>
                                        <input type="checkbox" name="chkLabel" value='<%#Eval("LabelID") %>' />
                                        <span id='span<%#Eval("LabelID") %>' title='<%#Eval("LabelID") %>' onmouseover="over(this)" onmouseout="out(this)" onclick="spanCheck(this)"><%#Eval("LabelName") %></span>
                                    </dd>
                                </ItemTemplate>
                            </asp:Repeater>
                        </dl>
                    </td>
                </tr>
                <tr>
                    <td>说明</td>
                    <td colspan="3">
                        <script id="editor" type="text/plain" style="height: 300px; width: 100%; text-decoration: underline"></script>
                    </td>
                </tr>
                <tr>
<%--                    <td colspan="4" style="text-align: center">
                        <input type="button" value="提交" class="btn btn-info" onclick="submitForm()" />
                    </td>
                </tr>--%>
            </table>
        </div>


    </div>


    <script type="text/javascript">

        //实例化编辑器
        //建议使用工厂方法getEditor创建和引用编辑器实例，如果在某个闭包下引用该编辑器，直接调用UE.getEditor('editor')就能拿到相关的实例
        var ue = UE.getEditor('editor');

        function over(obj) {
            $(obj).css("text-decoration", "underline");
        }

        function out(obj) {
            $(obj).css("text-decoration", "none");
        }


        //点击文字 选中复选框
        function spanCheck(obj) {
            var id = $(obj).attr("title");
            var chk = $("#" + id).find("input[name='chkLabel']:checked");

            if (chk.length > 0) {
                $("#" + id).find("input[name='chkLabel']").prop("checked", "")
            } else {
                $("#" + id).find("input[name='chkLabel']").prop("checked", "checked")
            }
        }


        //表单提交
        function submitForm() {
            hhl.notify.clear();
            if (validate()) {
                var chk = $("#dlLabel").find("input[name='chkLabel']:checked");
                var labelId = "";
                var lableContent = "";
                chk.each(function () {
                    labelId = labelId + this.value + ",";
                    lableContent = lableContent + $("#span" + this.value).text() + ",";
                })

                labelId = labelId.substr(0, labelId.lastIndexOf(','));
                lableContent = lableContent.substr(0, lableContent.lastIndexOf(','));

                var type = $("#selHotelType option:selected").val();

                var hotel = '{"hotel":{"HotelName":"' + $("#txtHotelName").val() + '","HotelType":"' + $("#selHotelType option:selected").val() + '","Area":"' + $("#txtArea").val() + '","Address":"' + $("#txtAddress").val() + '","Phone":"' + $("#txtPhone").val() + '","Start":"' + $("#txtStartMoney").val() + '","End":"' + $("#txtEndMoney").val() + '","DeskCount":"' + $("#txtDeskCount").val() + '","Description":"' + ue.getContent() +
                  '","Sort":"' + $("#txtSort").val() + '","Label":"' + labelId + '","LabelContent":"' + lableContent + '"}}';

                hhl.ajax("/WebService/HotelHandler.asmx/InsertHotel", hotel, hhl.callbackRefresh);
            }
        }


        //验证
        function validate() {

            if ($("#txtHotelName").val() == "") {
                hhl.notify.warn("请输入酒店名称", "提示");
                return false;
            } else if ($("#selHotelType option:selected").val() == "-1") {
                hhl.notify.warn("请选择酒店类型", "提示");
                return false;
            } else if ($("#txtArea").val() == "") {
                hhl.notify.warn("请输入区域", "提示");
                return false;
            } else if ($("#txtAddress").val() == "") {
                hhl.notify.warn("请输入酒店地址", "提示");
                return false;
            } else if ($("#txtPhone").val() == "") {
                hhl.notify.warn("请输入联系电话", "提示");
                return false;
            } else if ($("#txtDeskCount").val() == "") {
                hhl.notify.warn("请输入桌数", "提示");
                return false;
            } else if ($("#txtStartMoney").val() == "" || $("#txtEndMoney").val() == "") {
                hhl.notify.warn("请输入餐标", "提示");
                return false;
            } else {
                return true;
            }
        }



    </script>
</asp:Content>
