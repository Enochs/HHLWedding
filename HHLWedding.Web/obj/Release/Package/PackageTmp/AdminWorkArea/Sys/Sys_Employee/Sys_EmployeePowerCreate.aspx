<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/PopuMaster.Master" AutoEventWireup="true" CodeBehind="Sys_EmployeePowerCreate.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Sys.Sys_Employee.Sys_EmployeePowerCreate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        li {
            list-style: none;
        }

        .panel {
            margin-left: 10px;
        }
    </style>

    <script type="text/javascript">

        //所有全选
        function checkAll(obj) {
            $("input[name='chkPAll']").prop("checked", $(obj).prop("checked"));
            $("input[name='cid']").prop("checked", $(obj).prop("checked"));
        }


        //子级全选
        function chkpAll(obj) {
            var id = $(obj).attr("value");
            $("#" + id).find("li input[name='cid']").prop("checked", $(obj).prop("checked"));
        }

        //选中子标签 选中父标签
        function CheckParent(obj) {
            var parent = $(obj).attr("title");
            var length = $("#" + parent).find("li input[name='cid']:checked").length;
            if (length > 0) {
                $("." + parent).find("td").find("input[name='chkPAll']").prop("checked", "checked");
            } else {
                $("." + parent).find("td").find("input[name='chkPAll']").prop("checked", "");
            }

        }


        $(function () {
            //页面加载
            var EmployeeId = '<%=Request["EmployeeId"] %>';
            hhl.ajax("/WebService/EmployeeHandler.asmx/GetCheckPower", '{"EmployeeId":"' + EmployeeId + '"}', function (result) {
                if (result.d.IsSuccess) {
                    if (result.d.Data.length > 0) {
                        for (var i = 0; i < result.d.Data.length; i++) {
                            var power = result.d.Data[i];
                            if (power.ItemLevel == 1) {
                                $("." + power.ChannelID).find("td").find("input[name='chkPAll']").prop("checked", "checked");
                            } else {
                                var chk = $("#" + power.ChannelID).find("input[name='cid']");
                                if (chk.val() == power.ChannelID) {
                                    chk.prop("checked", "checked");
                                }
                            }
                        }
                    }
                }
            });
        });

            //点击保存权限
            function submitForm() {
                var chkPVal = $("input[name='chkPAll']:checked");
                var chkVal = $("input[name='cid']:checked");
                var ids = "";
                chkVal.each(function () {
                    ids = ids + this.value + ",";
                });
                chkPVal.each(function () {
                    ids = ids + this.value + ",";
                });

                ids = ids.substring(0, ids.length - 1);
                var EmployeeId = '<%=Request["EmployeeId"] %>';

                hhl.ajax("/WebService/EmployeeHandler.asmx/EmployeePowerCreate", '{"employeeId":"' + EmployeeId + '","channel":"' + ids + '"}', function (result) {
                    if (result.d.IsSuccess) {
                        hhl.notify.success(result.d.Message, "提示");
                        var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                        parent.layer.close(index);
                    }
                });
            }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel">
        <table>
            <tr>
                <td>
                    <input type="checkbox" id="chkAll" name="chkAll" onclick="checkAll(this)" />全选/取消  频道名称</td>
            </tr>
            <asp:Repeater runat="server" ID="rptParentChannel" OnItemDataBound="rptParentChannel_ItemDataBound">
                <ItemTemplate>
                    <tr class='<%#Eval("ChannelID") %>'>
                        <td>
                            <asp:HiddenField runat="server" ID="hideChannelId" Value='<%#Eval("ChannelID") %>' />
                            <input type="checkbox" id="chkPAll" name="chkPAll" value='<%#Eval("ChannelID") %>' onclick="chkpAll(this)" />
                            <%#Eval("ChannelName") %>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ul id='<%#Eval("ChannelID") %>'>
                                <asp:Repeater runat="server" ID="rptChannel">
                                    <ItemTemplate>
                                        <li id='<%#Eval("ChannelID") %>'>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  <input type="checkbox" id="cid" name="cid" value='<%#Eval("ChannelID") %>' title='<%#Eval("Parent") %>' onclick="CheckParent(this)" />
                                            <%#Eval("ChannelName") %>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <%--<button type="button" class="btn btn-primary btn-mini" onclick="SaveChange()"><i class="icon-save"></i></button>--%>
    </div>
</asp:Content>
