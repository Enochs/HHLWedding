<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Flow.Customer.Default" %>

<%@ Register Assembly="HHLWedding.Control" Namespace="HHLWedding.Control" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>测试员</title>

     
    <!--头部导航栏-->
   <%-- <link href="/Content/css/main.css" rel="stylesheet" />
    <link href="/AdminWorkArea/Main/css/H-ui.admin.css" rel="stylesheet" />--%>

    <!--日期样式-->
    <link href="/Scripts/jedate/skin/jedate.css" rel="stylesheet" />

    <!--bootstrap 样式文件-->
    <link href="/Content/bootstrap.css" rel="stylesheet" />
<%--    <link href="/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/bootstrap.css" rel="stylesheet" />--%>
    <link href="/Content/Site.css" rel="stylesheet" />


    <!--主页配置需要文件-->
 <%--   <link href="/AdminWorkArea/Main/css/style.css" rel="stylesheet" />
    <link href="/AdminWorkArea/Main/lib/Hui-iconfont/1.0.1/iconfont.css" rel="stylesheet" />--%>

    <!--jquery 文件-->
    <script src="/Scripts/jquery-1.10.2.js"></script>
    <script src="/Scripts/jquery-1.10.2.min.js"></script>
    <script src="/Scripts/jquery.min.js"></script>
    <script src="/Scripts/jquery.validate.js"></script>
    <script src="/Scripts/jquery.validate.min.js"></script>

    <!--弹出层   提示 样式文件-->
    <link href="/Content/hhlset/layer/skin/layer.css" rel="stylesheet" />
    <link href="/Content/hhlset/toastr/toastr.css" rel="stylesheet" />
    <link href="/Content/hhlset/toastr/toastr.min.css" rel="stylesheet" />
    <!--弹出层   提示 js文件-->
    <script src="/Content/hhlset/toastr/toastr.js"></script>
    <script src="/Content/hhlset/layer/layer.js"></script>
    <%--<script src="/Content/hhlset/toastr/toastr.min.js"></script>--%>

    <!--图标 样式文件-->
    <link href="/Content/font-awesome.css" rel="stylesheet" />

    <!--提示  扩展的js文件-->
    <script src="/Scripts/hhl.js"></script>
    <link href="/Content/css/MyStyle.css" rel="stylesheet" />
    <script src="/Content/js/Number.js"></script>
    <script src="/Content/js/Common.js"></script>

    <!--tooltip modal弹出  点击页面防关闭 提示-->
    <script src="/Scripts/bootstrap.min.js"></script>

    <!--时间插件-->
    <script src="/Content/hhlset/My97DatePicker/WdatePicker.js"></script>
    <script src="/Scripts/jedate/jquery.jedate.js"></script>

    <%-- chosen 样式  js文件 --%>
    <link href="/Content/css/select/chose.css" rel="stylesheet" />
    <script src="/Content/js/chosen.jquery.min.js"></script>

    <script type="text/javascript">
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
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td>
                        <cc1:ddlsaletype runat="server" id="ddlSaleType" clientidmode="Static" />
                    </td>
                    <td>渠道名称</td>
                    <td>
                        <cc1:ddlsalesource runat="server" id="ddlSaleSource" clientidmode="static" />
                    </td>
                </tr>
            </table>

        </div>
    </form>
</body>
</html>
