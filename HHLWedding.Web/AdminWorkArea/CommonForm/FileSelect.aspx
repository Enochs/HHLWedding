<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/MainMaster.Master" AutoEventWireup="true" CodeBehind="FileSelect.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.CommonForm.FileSelect" %>


<%@ Register Assembly="HHLWedding.ToolsLibrary" Namespace="HHLWedding.ToolsLibrary" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        //全选
        function checkAll(obj) {
            $("input[name='fid']").prop("checked", $(obj).prop("checked"));
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel">
        <div class="panel-body">
            <table class="table table-bordered table-hover">
                <thead>
                    <tr>
                        <th>
                            <label class="checkbox-inline">
                                <input type="checkbox" name="chkAll" onclick="checkAll(this)" />
                            </label>
                        </th>
                        <th>图片</th>
                        <th>文件名</th>
                    </tr>
                </thead>

                <tbody id="file">
                    <asp:Repeater runat="server" ID="repFile">
                        <ItemTemplate>

                            <tr>
                                <td>
                                    <input type="checkbox" name="fid" class="checkbox" /></td>
                                <td>
                                    <span id="span_Index" class="hide"><%# Container.ItemIndex %></span>
                                    <img src='/<%#Eval("Directory.Name")+"/"+Eval("Name") %>' style="width: 120px; height: 120px; border: 1px solid gray;" /></td>
                                <td style="vertical-align: middle;">
                                    <span class="spanName"><%#Eval("Name") %></span>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>

            </table>
        </div>
    </div>

</asp:Content>
