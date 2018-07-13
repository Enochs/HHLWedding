<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/MainMaster.Master" AutoEventWireup="true" CodeBehind="CommonImgList.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.CommonForm.CommonImgList" %>

<%@ Register Assembly="HHLWedding.ToolsLibrary" Namespace="HHLWedding.ToolsLibrary" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <link href="/Content/common/AspNetPager.css" rel="stylesheet" />

    <script type="text/javascript">

        //删除图片
        function ConfirmDelete(id) {
            hhl.message.confirm("你确定要删除这张图片吗?", function (result) {
                if (result) {
                    hhl.ajax("/WebService/CommonImgHandler.asmx/DeleteImgById", '{"imgId":"' + id + '"}', hhl.callbackStatus)
                }
            });
        }

        //设置封面
        function SetTitle(Id, hotelId) {
            hhl.ajax("/WebService/CommonImgHandler.asmx/SetThemeTitle", '{"imgId":"' + Id + '","hotelId":"' + hotelId + '"}', hhl.callbackStatus)
        }

        //返回上一页
        function BackUrl() {
            var page = '<%=Request["page"] %>';
            window.location.href = '/AdminWorkArea/Foundation/Hotel/HotelList.aspx?page=' + page;
        }




    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel">
        <div class="panel-body">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="widget-box">
                        <asp:Button runat="server" ID="btnConfirm" Text="查询" ClientIDMode="Static" CssClass="btn btn-primary" OnClick="btnConfirm_Click" Style="display: none" />
                        <a class="btn btn-info" onclick="BackUrl()">返回上一页</a>
                    </div>
                    <table class="table table-hover bg-white" style="width: 25%">
                        <tr>
                            <th>序号</th>
                            <th>图片</th>
                            <th>操作</th>
                        </tr>
                        <tbody id="file">
                            <tr>
                                <asp:DataList runat="server" ID="dlImgList" RepeatDirection="Horizontal" RepeatColumns="3">
                                    <ItemTemplate>

                                        <td style="vertical-align: middle;"><%# Container.ItemIndex+1 %></td>
                                        <td>
                                            <span id="span_Index" class="hide"><%# Container.ItemIndex %></span>
                                            <img title='<%#Eval("ImgName") %>' src='<%#Eval("ImgUrl") %>' style="width: 120px; height: 120px; border: 1px solid gray;" />&nbsp;&nbsp;

                                        </td>
                                        <td style="vertical-align: middle;">
                                            <button type="button" <%#Eval("State").ToString() == "1" ? "disabled" : "" %> onclick='SetTitle(<%#Eval("ImgId") %>,<%#Eval("CommonId") %>)' class="btn btn-<%#Eval("State").ToString() == "1" ? "success" : "primary" %> btn-xs"><%#Eval("State").ToString() == "1" ? "主题封面" : "设为封面" %></button>

                                            <a class="btn btn-danger btn-xs" onclick='ConfirmDelete(<%#Eval("ImgId") %>)'>删除</a>

                                        </td>
                                    </ItemTemplate>
                                </asp:DataList>
                            </tr>
                        </tbody>
                    </table>

                    <cc1:AspNetPagerTool ID="CtrPageIndex" ClientIDMode="Static" runat="server" OnPageChanged="CtrPageIndex_PageChanged"></cc1:AspNetPagerTool>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>
