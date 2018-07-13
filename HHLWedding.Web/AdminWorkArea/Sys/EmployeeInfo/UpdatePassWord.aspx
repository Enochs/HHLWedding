<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdatePassWord.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Sys.EmployeeInfo.UpdatePassWord" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>修改密码</title>

    <!--jquery文件-->
    <script src="/Scripts/jquery-1.10.2.js"></script>
    <script src="/Scripts/jquery-1.10.2.min.js"></script>
    <script src="/Scripts/jquery.min.js"></script>

    <!--弹出层   提示 样式文件-->
    <link href="/Content/hhlset/layer/skin/layer.css" rel="stylesheet" />
    <link href="/Content/hhlset/toastr/toastr.css" rel="stylesheet" />
    <link href="/Content/hhlset/toastr/toastr.min.css" rel="stylesheet" />

    <!--弹出层   提示 js文件-->
    <script src="/Content/hhlset/toastr/toastr.js"></script>
    <script src="/Content/hhlset/layer/layer.js"></script>
    <script src="/Content/hhlset/toastr/toastr.min.js"></script>

    <!--提示  扩展的js文件-->
    <script src="/Scripts/hhl.js"></script>

    <!--验证提示-->
    <link href="/Content/form/style.css" rel="stylesheet" />
    <script src="/Content/form/easyform.js"></script>
    <style type="text/css">
        #tblSystem tr td{
            padding-top:20px;
        }
    </style>

</head>
<body>

    <div class="form-div">
        <form id="reg-form" method="post">
            <table id="tblSystem" class="table">
                <tr>
                    <td>原密码:</td>
                    <td>
                        <input type="text" id="txtOldPwd" class="form-control" easyform="length:0-16;ajax:Formsubmit();" message="请输入原密码" easytip="disappear:lost-focus;theme:blue;position:top;" ajax-message="请输入正确的原密码!" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>新密码:</td>
                    <td>
                        <input type="text" id="txtNewPwd" class="form-control" easyform="length:6-16;" message="密码必须为6—16位" easytip="disappear:lost-focus;theme:blue;position:top" />
                    </td>
                </tr>
                <tr>
                    <td>确认密码:</td>
                    <td>
                        <input type="text" id="txtConfirmPwd" class="form-control" easyform="length:6-16;equal:#txtNewPwd;" message="两次密码输入要一致" easytip="disappear:lost-focus;theme:blue;position:top" />
                    </td>
                </tr>
            </table>

            <div align="center" style="display: none;">
                <input value="确 认" id="confirm" onclick="Formsubmit()" type="submit" style="margin-right: 20px; margin-top: 20px;" />
                <input class="layui-layer-btn1" value="取 消" onclick="closeFrame()" type="button" style="margin-right: 45px; margin-top: 20px;" />
            </div>

            <br class="clear" />
        </form>
    </div>


    <script type="text/javascript">
        $(document).ready(function () {
            $('#reg-form').easyform();
        });


        //提交 修改密码
        function Formsubmit() {
            var pwd = $("#txtOldPwd").val();
            var newPwd = $("#txtNewPwd").val();
            var confirmPwd = $("#txtConfirmPwd").val();

            if (newPwd == confirmPwd && newPwd.length >= 6) {
                hhl.ajax("/WebService/EmployeeHandler.asmx/CheckModifyPwd", '{"pwd":"' + pwd + '","newPwd":"' + newPwd + '","confirmPwd":"' + confirmPwd + '"}', function (result) {
                    if (result.d.IsSuccess) {
                        $("#txtOldPwd").trigger("easyform-ajax", true);

                        var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                        parent.layer.close(index);

                    } else {
                        $("#txtOldPwd").trigger("easyinput-ajax", false);
                    }
                });
            }
        }

        //点击确定
        function submitForm() {
            $("#confirm").click();
        }

        //关闭窗口
        function closeFrame() {
            var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
            parent.layer.close(index);
        }
    </script>
</body>
</html>
