<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Import.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Flow.Customer.Import" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <!--日期样式-->
    <link href="/Scripts/jedate/skin/jedate.css" rel="stylesheet" />

    <!--bootstrap 样式文件-->
    <link href="/Content/css/bootstrap.css" rel="stylesheet" />
    <link href="/Content/Site.css" rel="stylesheet" />


    <!--jquery 文件-->
    <script src="/Scripts/jquery.js"></script>
    <script src="/Scripts/jquery.min.js"></script>
    <script src="/Scripts/jquery.validate.js"></script>
    <script src="/Scripts/jquery.validate.min.js"></script>

    <!--弹出层 layer   提示文件-->
    <link href="/Content/hhlset/layer/skin/layer.css" rel="stylesheet" />
    <script src="/Content/hhlset/layer/layer.js"></script>

    <!--弹出层 toatr   提示文件-->
    <link href="/Content/hhlset/toastr/toastr.css" rel="stylesheet" />
    <link href="/Content/hhlset/toastr/toastr.min.css" rel="stylesheet" />
    <script src="/Content/hhlset/toastr/toastr.js"></script>


    <!--图标 样式文件-->
    <link href="/Content/font-awesome.css" rel="stylesheet" />

    <!--提示  扩展的js文件-->
    <script src="/Scripts/hhl.js"></script>
    <script src="/Content/js/Default/RegExp.js"></script>
    <script src="/Content/js/Common.js"></script>
    <script src="/Content/js/angle.js"></script>
    <script src="/Scripts/jquery.form.js"></script>

    <!--tooltip modal弹出  点击页面防关闭 提示-->
    <script src="/Scripts/bootstrap.min.js"></script>

    <!--时间插件-->
    <script src="/Content/hhlset/My97DatePicker/WdatePicker.js"></script>
    <script src="/Scripts/jedate/jquery.jedate.js"></script>

    <%-- chosen 样式  js文件 --%>
    <link href="/Content/css/select/chose.css" rel="stylesheet" />
    <script src="/Content/js/chosen.jquery.min.js"></script>


    <script type="text/javascript">
        function submitForm() {
            $("#btnPost").click();
        }


        $(function () {
            $("#btnPost").click(function () {
                $("#myFrorm").ajaxSubmit({
                    url: "/AdminWorkArea/Handler/ImportCustomer.ashx",
                    type: "post",
                    dataType: 'json',
                    success: function (result) {
                        console.log(result);
                    },
                    error: function (data) {
                        hhl.notify.error("异常错误", "警告");
                    }
                });
            });

            //function submitForm() {

            //    $("#myFrorm").ajaxForm(function (result) {
            //        console.log(result);
            //        alert("可以看看 是否成功");
            //    });
            //}
        });
    </script>
</head>
<body>
    <form id="myFrorm" name="importForm" runat="server" method="post" enctype="multipart/form-data">
        <div class="panel">
            <div class="panel-body">
                <div class="form-group">
                    <div class="progress progress-striped">
                        <div class="progress-bar progress-bar-danger" style="width: 33.3%">
                            <label>第一步：下载模版</label>
                        </div>
                        <div class="progress-bar progress-bar-warning" style="width: 33.3%">
                            <label>第二步：上传Excel文件</label>
                        </div>
                        <div class="progress-bar progress-bar-success" style="width: 33.3%">
                            <label>第三步：导入客户</label>
                        </div>
                    </div>
                </div>
                <div class="form-group text-center">
                    <label class="control-label col-sm-7" style="font-weight: normal; font-size: 14px;">模版格式：导入客户模版如下图,请保持一致</label>
                    <div class="col-sm-1"></div>
                    <div class="col-sm-2"><a class="btn btn-info" style="font-weight: bold;" href="/Template/导入客户模版.xlsx">下载模版</a></div>
                </div>
                <div class="form-group">
                    <div class="text-center">
                        <img src="/Template/导入客户模版.png" />
                    </div>
                </div>
                <div class="form-group"></div>
                <div class="col-sm-3"></div>

                <div class="form-group">
                    <div class="text-center">
                        <label class="col-sm-2">请选择导入文件:</label>
                        <div class="col-sm-1">
                            <input type="file" id="upFile" name="upFile" class="btn btn-success" />
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <div class="text-center">
            <input type="submit" value="Form提交" class="btn btn-primary btn-sm" />
            <input type="button" id="btnPost" value="button提交" class="btn btn-info" />
        </div>
    </form>
</body>
</html>
