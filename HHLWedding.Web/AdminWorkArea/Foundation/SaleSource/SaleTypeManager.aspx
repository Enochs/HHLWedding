<%@ Page Title="渠道类型管理" Language="C#" MasterPageFile="~/AdminWorkArea/Master/MainMaster.Master" AutoEventWireup="true" CodeBehind="SaleTypeManager.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.SaleSource.SaleTypeManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        //添加渠道类型
        function AddSaleType() {
            var name = $("#txtTypeName").val();
            if (name == "") {
                hhl.notify.error("请输入渠道类型名称", "提示", { timeOut: 1000 });
                return false;
            }

            hhl.ajax("/WebService/SaleSourceHandler.asmx/CreateSaleType", '{"typeName":"' + name + '"}', hhl.callbackStatus)
        }


        //修改渠道类型
        function Modify(obj) {
            var Id = $(obj).val();
            var name = $(".li_Saletitle").find("input[name='SaleName" + Id + "']").val();

            if (name == "") {
                hhl.notify.error("请输入渠道类型名称", "提示", { timeOut: 1000 });
                return false;
            }
            hhl.ajax("/WebService/SaleSourceHandler.asmx/UpdateSaleType", '{"SaleTypeId":"' + Id + '","SaleTypeName":"' + name + '"}', hhl.callbackStatus);
        }

        //删除渠道类型
        function Delete(obj) {
            debugger
            var Id = $(obj).val();
            var name = $(".li_Saletitle").find("input[name='SaleName" + Id + "']").val();
            var typeName = $(obj).html();

            hhl.message.confirm("你确定" + typeName + name + "吗?", function (result) {
                if (result) {
                    hhl.ajax("/WebService/SaleSourceHandler.asmx/DeleteSaleType", '{"SaleTypeId":"' + Id + '"}', hhl.callbackStatus);
                }
                //else {
                //    debugger
                //    hhl.notify.error(result.d.message, "提示");
                //}
            });

            //hhl.ajax("/WebService/SaleSourceHandler.asmx/DeleteSaleType", '{"SaleTypeId":"' + Id + '"}', hhl.callbackStatus);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="UpdatePanel">
        <ContentTemplate>
            <div class="panel">
                <div class="panel-body">
                    <div class="form-group">
                        <table class="table" style="width: 50%;">
                            <asp:Repeater runat="server" ID="repSaleType">
                                <ItemTemplate>
                                    <tr>
                                        <td class="li_Saletitle">
                                            <input type="text" name='SaleName<%#Eval("SaleTypeId") %>' class="form-control" value='<%#Eval("SaleTypeName") %>' /></td>
                                        <td>
                                            <button type="button" id="btnModify" class="btn btn-primary btn-sm" data-toggle="tooltip" value='<%#Eval("SaleTypeId") %>' title='修改' onclick="Modify(this)">修改</button>
                                            <button type="button" id="btnDelete" class="btn btn-<%#Eval("Status").ToString() == "1" ? "danger" : "success" %> danger btn-sm" data-toggle="tooltip" value='<%#Eval("SaleTypeId") %>' title='<%#Eval("Status").ToString() == "1" ? "禁用" : "启用" %>' onclick="Delete(this)"><%#Eval("Status").ToString() == "1" ? "禁用" : "启用" %></button>

                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>

                            <tr>
                                <td class="li_Saletitle">
                                    <input type="text" class="form-control" id="txtTypeName" /></td>
                                <td>
                                    <button type="button" class="btn btn-success btn-sm" onclick="AddSaleType()">添加</button>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <asp:Button runat="server" ID="btnConfirm" Text="查询" ClientIDMode="Static" CssClass="btn btn-primary" OnClick="btnConfirm_Click" Style="display: none" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
