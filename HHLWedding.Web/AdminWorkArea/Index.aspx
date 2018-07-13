<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <!--bootstrap样式文件-->
    <%--    <link href="http://libs.baidu.com/bootstrap/3.0.3/css/bootstrap.min.css" rel="stylesheet" />
    <script src="http://libs.baidu.com/jquery/2.0.0/jquery.min.js"></script>
    <script src="http://libs.baidu.com/bootstrap/3.0.3/js/bootstrap.min.js"></script>--%>

    <%--<link href="../Content/bootstrap.min.css" rel="stylesheet" />--%>
    <script src="../Scripts/jquery-1.10.2.js"></script>
    <script src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/jquery.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <link href="../Content/css/bootstrap.css" rel="stylesheet" />
    <link href="../Content/font-awesome.css" rel="stylesheet" />
    <link href="../Content/css/MyStyle.css" rel="stylesheet" />







    <script type="text/javascript">
        function submitForm() {


            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "Index.aspx/GetUserName",
                data: "{}",
                dataType: "json",
                success: function (result) {
                    alert("cc");
                }
            });



        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div style="margin: 50px 0px 0px 50px;">
            <input type="button" class="btn btn-success btn-sm" runat="server" id="btnclick" value="点击" title="点击测试" data-toggle="tooltip" />
            <button type="button" class="btn btn-default" data-toggle="tooltip"
                title="默认的 Tooltip">
                默认的 Tooltip
            </button>
        </div>
        <a href="#DetailsModal" title="查看详情" data-toggle="modal" class="btn btn-success btn-mini"><i class="icon-eye-open"></i></a>
        <button type="button" data-toggle="modal" class="btn btn-primary xs" data-target="#DetailsModal">查看</button>


    </form>


    <!--DetailsModal-->
    <div class="modal fade" id="DetailsModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button class="close" type="button" data-dismiss="modal">×</button>
                    <h4 class="modal-title">添加员工职务</h4>
                </div>
                <div class="modal-body" id="UserDetails">
                    <div class="widget-content bg-white" style="border-top: 1px solid #D9D9D9;">
                        <ul class="ul_job">
                            <li class="li_job ">职务名称</li>
                            <li class="li_job">
                                <div class="col-sm-9">
                                    <input type="text" name="txtJobName" class="form-control" id="txtJobName" />
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-success" type="button" onclick="submitForm()">确定</button>
                    <button class="btn btn-default" type="button" data-dismiss="modal">取消</button>
                </div>
            </div>
        </div>
    </div>
</body>
</html>

<script type="text/javascript">
    $(function () { $("[data-toggle='tooltip']").tooltip(); });
</script>
