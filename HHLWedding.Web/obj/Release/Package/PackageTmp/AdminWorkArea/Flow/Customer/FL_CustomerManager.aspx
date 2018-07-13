<%@ Page Title="客户管理" Language="C#" MasterPageFile="~/AdminWorkArea/Master/MainMaster.Master" AutoEventWireup="true" CodeBehind="FL_CustomerManager.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Flow.Customer.FL_CustomerManager" %>

<%@ Register Assembly="HHLWedding.ToolsLibrary" Namespace="HHLWedding.ToolsLibrary" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel">
        <div class="panel-body">

            <div class="widget-box">
                <div class="widget-header">
                    <h4 class="h-title pull-left"><i class="icon-reorder"></i><b>客户列表列表</b></h4>
                    <div class="box pull-right">
                        <div class="div_angle">
                            <span class="span-angel">
                                <i id="icon_angle" class="icon-angle-down"></i>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="widget-content">
                    <div class="form-group col-sm-3">
                        <asp:Button runat="server" ID="btnConfirm" Text="查询" ClientIDMode="Static" CssClass="btn btn-primary" OnClick="btnConfirm_Click" Style="display: none" />
                        <button type="button" class="btn btn-info" title="搜索" data-toggle="tooltip" id="_btnSearch" runat="server" onserverclick="btnConfirm_Click"><i class="icon-search"></i>&nbsp; 搜索</button>
                        <button type="button" class="btn btn-info" title="刷新" data-toggle="tooltip" id="_btnRefresh" runat="server" onclick="window.location.reload()"><i class="icon-refresh"></i>&nbsp; 刷新</button>
                        <a class="btn btn-success" title="添加客户" data-toggle="tooltip" onclick="OpenCreate()"><i class="icon-plus"></i>&nbsp; 添加客户</a>
                    </div>
                </div>
            </div>
            <div class="widget-box">
                <table class="table table-bordered table-hover">
                    <thead>
                        <tr>
                            <th>姓名</th>
                            <th>联系电话</th>
                            <th>婚期</th>
                            <th>酒店</th>
                            <th>电销</th>
                            <th>渠道</th>
                            <th>状态</th>
                            <th>操作</th>
                        </tr>
                    </thead>
                </table>

                <cc1:AspNetPagerTool ID="CtrPageIndex" ClientIDMode="Static" runat="server" OnPageChanged="btnConfirm_Click"></cc1:AspNetPagerTool>
            </div>
        </div>
    </div>
</asp:Content>
