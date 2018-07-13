<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Test.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>测试页面</title>

     <script type="text/javascript">
        //重新绑定提示(tooltip)
        function BinderPage() {

            SetPage();
            $("[data-toggle='tooltip']").tooltip();
        }
    </script>

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


    <!--tooltip modal弹出  点击页面防关闭 提示-->
    <script src="/Scripts/bootstrap.min.js"></script>

    <!--时间插件-->
    <script src="/Content/hhlset/My97DatePicker/WdatePicker.js"></script>
    <script src="/Scripts/jedate/jquery.jedate.js"></script>

    <%-- chosen 样式  js文件 --%>
    <link href="/Content/css/select/chose.css" rel="stylesheet" />
    <script src="/Content/js/chosen.jquery.min.js"></script>
</head>
<body>
    <form runat="server">
        <div class="panel">
            <div class="widget-content" style="height: auto;">
                <table id="tblSystem" class="table table-bordered table-no-border">
                    <tr>
                        <td><span class="control-label">新娘</span></td>
                        <td>
                            <input type="text" class="form-control col-width-8" runat="server" id="txtBride" name="txtBride" /></td>
                        <td><span class="control-label">新郎</span></td>
                        <td>
                            <input type="text" class="form-control col-width-8" runat="server" id="txtGroom" name="txtGroom" /></td>
                        <td><span class="control-label">经办人</span></td>
                        <td>
                            <input type="text" class="form-control col-width-8" runat="server" id="txtOperator" name="txtOperator" /></td>
                    </tr>
                    <tr>
                        <td><span class="control-label">说明</span></td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ID="txtDescription" CssClass="form-control" TextMode="MultiLine" Rows="10" /></td>
                    </tr>
                    <tr>
                        <td><span class="control-label">说明</span></td>
                        <td colspan="5">
                            <textarea rows="3"  class="form-control txtDescription"></textarea></td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>

    <%-- 下拉框 模糊搜索 --%>
<script src="/Content/js/chosen.js"></script>

<script type="text/javascript">

    //title 提示
    $(function () { $("[data-toggle='tooltip']").tooltip(); });

    //获取参数
    function getUrlParam(name) {
        //构造一个含有目标参数的正则表达式对象  
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        //匹配目标参数  
        var r = window.location.search.substr(1).match(reg);
        //返回参数值  
        if (r != null) return unescape(r[2]);
        return null;
    }

    //获取图片名称
    function getFileName(o) {
        var pos = o.lastIndexOf("\\");
        return o.substring(pos + 1);
    }

    //type 1.完整格式 年月日 时分秒  2.只有年月日
    function jsonDateFormat(type, jsonDate, fu) {//json日期格式转换为正常格式
        try {//出自http://www.cnblogs.com/ahjesus 尊重作者辛苦劳动成果,转载请注明出处,谢谢!
            var date = new Date();
            if (jsonDate) {
                var date = new Date(parseInt(jsonDate.replace("/Date(", "").replace(")/", ""), 10));
            }
            var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
            var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
            var hours = date.getHours();
            var minutes = date.getMinutes();
            var seconds = date.getSeconds();
            var milliseconds = date.getMilliseconds();

            var fuyear = "-";
            var fumonth = "-";
            var fudate = "-";
            if (fu) {
                fuyear = "年";
                fumonth = "月";
                fudate = "日";
            }

            if (type == "1") {
                return date.getFullYear() + fumonth + month + fudate + day + " " + hours + ":" + minutes;
            } else {
                return date.getFullYear() + fuyear + month + fumonth + day;
            }
        } catch (ex) {//出自http://www.cnblogs.com/ahjesus 尊重作者辛苦劳动成果,转载请注明出处,谢谢!
            return "";
        }
    }

</script>
</html>
