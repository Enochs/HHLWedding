<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/MainMaster.Master" AutoEventWireup="true" CodeBehind="Sys_ChannelListView.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Channel.Sys_ChannelListView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        ul li {
            list-style: none;
            font-size: 14px;
            line-height: 40px;
            width: 280px;
            /*border: 1px solid red;*/
            text-align:center;
            float: left;
        }

    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel">
        <div class="panel-body">
            <div id="viewList" class="form-horizontal">
                <table id="tblChannel" class="table table-bordered" style="width: 100%; padding: 0px 0px;">
                    <asp:Repeater runat="server" ID="repParent" OnItemDataBound="repParent_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td width="5%" style="vertical-align: middle;"><%#Container.ItemIndex+1 %></td>
                                <td width="10%" style="vertical-align: middle;"><%#Eval("ChannelName") %></td>
                                <td>
                                    <asp:HiddenField runat="server" ID="HideParentId" Value='<%#Eval("ChannelID") %>' />
                                    <ul>
                                        <asp:Repeater runat="server" ID="repChannel">
                                            <ItemTemplate>
                                                <li><%#Eval("ChannelName") %> </li>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </ul>

                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>

            </div>
        </div>
    </div>
</asp:Content>
