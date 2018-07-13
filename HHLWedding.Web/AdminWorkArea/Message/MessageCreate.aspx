<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/PopuMaster.Master" AutoEventWireup="true" CodeBehind="MessageCreate.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Message.MessageCreate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .dl_getEmp dd {
            cursor: pointer;
        }

        .dl_sendEmp dd {
            cursor: pointer;
        }

        #span_empName:hover {
            text-decoration: underline;
        }

        .ddEmp:hover {
            background-color: #F1F2F7;
        }
    </style>

    <%-- 编辑器 必须引用 --%>
    <script type="text/javascript" charset="utf-8" src="/Content/ueditor/ueditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="/Content/ueditor/ueditor.all.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="/Content/ueditor/lang/zh-cn/zh-cn.js"></script>

    <script type="text/javascript">

        //实例化编辑器
        //建议使用工厂方法getEditor创建和引用编辑器实例，如果在某个闭包下引用该编辑器，直接调用UE.getEditor('editor')就能拿到相关的实例
        var ue = UE.getEditor('editor');

        //页面加载
        $(function () {

            //回复  页面的加载
            var empId = '<%=Request["empId"] %>';
            if (empId != "") {
                var name = '<%=GetEmployeeName(Request["empId"]) %>';
                $("#txtgetMsg").val(name);
                $("#txtgetMsg").attr("title", empId);
            }

            //转发 消息 
            if (empId == "") {      //转发消息 empId就是空   回复  不加载消息信息
                var id = '<%=Request["mid"] %>';
                if (id != "") {
                    hhl.ajax("/WebService/MessageHandler.asmx/GetMessage", '{"mid":"' + id + '","sendType":"0"}', function (result) {
                        if (result.d.IsSuccess) {
                            $("#txtTitle").val(result.d.Data.MessageTitle);
                            setTimeout(function () {
                                ue.setContent(result.d.Data.MessageContent);
                            }, 500);

                        }
                    });
                }
            }   
        });

        //点击发送功能
        function messageSend() {
            if ($("#txtgetMsg").val() == "") {
                hhl.notify.warn('请选择收件人', "提示", { positionClass: 'toast-top-right' });
                return false;
            } else if ($("#txtTitle").val() == "") {
                hhl.notify.warn('标题不能为空', "提示", { positionClass: 'toast-top-right' });
                return false;
            }
            else if (ue.getContent() == "") {
                hhl.notify.warn('信件内容不能为空', "提示", { positionClass: 'toast-top-right' });
                return false;
            }

            var message = "{'msg':{'ToEmployee':'" + $("#txtgetMsg").attr("title") + "','MessageTitle':'" + $("#txtTitle").val() + "' ,'MessageContent':'" + ue.getContent() + "'}}";

            hhl.ajax("/WebService/MessageHandler.asmx/CreateMessage", message, function (result) {
                hhl.notify.success(result.d.Message);
            });
            window.location.href = "OutBoxMessage.aspx";
        }


        //选择收件人(弹出层)
        function SelectedEmployee() {

            var ids = $("#txtgetMsg").attr("title");
            if (ids != undefined) {
                var empId = ids.split(',');
                for (var i = 0; i < empId.length; i++) {
                    var item = empId[i];
                    $("#chkEmp" + item).prop("checked", "checked");
                }
            }

            $("#empSelectModal").modal("show");
        }

        //返回
        function BackUrl() {

            var type = '<%=Request["type"] %>';
            if (type == 1) {                //收件
                window.location.href = "InBoxMessage.aspx";
            } else if (type == 2) {         //发件
                window.location.href = "OutBoxMessage.aspx";
            } else if (type == 3) {         //回复&转发
                var sendtype = '<%=Request["sendType"] %>';
                var id = '<%=Request["mid"] %>';
                window.location.href = "DetailsMessage.aspx?mid=" + id + "&sendType=" + sendtype;
            }
}

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <%-- <div class="panel bg-gray">
        <div class="panel-Content">--%>
            <table id="tblSystem" class="table table-bordered">
                <tr>
                    <td>收信人:</td>
                    <td>
                        <input type="text" name="txtgetMsg" id="txtgetMsg" class="form-control" onclick="SelectedEmployee()" /></td>
                </tr>
                <tr>
                    <td>主题:</td>
                    <td>
                        <input type="text" name="txtTitle" id="txtTitle" class="form-control" /></td>
                </tr>
                <tr>
                    <td>内容:</td>
                    <td>
                        <script id="editor" type="text/plain" style="line-height: 25px; height: 300px; width: 100%; text-decoration: underline"></script>
                    </td>
                </tr>
            </table>
            <div class="form-group">
                <a class="btn btn-success btn-sm" id="a_send" onclick="messageSend()">发送</a>
                <a class="btn btn-info btn-sm" onclick="BackUrl()">返回</a>
            </div>
        <%--</div>
    </div>--%>

    <!--选择收件人 弹出层-->
    <div class="modal fade" id="empSelectModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">选择收件人</h4>
                </div>
                <div class="modal-body" id="action-body">
                    <dl class="dl_getEmp">
                        <asp:Repeater runat="server" ID="repEmployee">
                            <ItemTemplate>
                                <dd class="ddEmp" title='<%#Eval("EmployeeId") %>' onclick="checkEmp(this,1)">
                                    <input type="checkbox" name="chkEmp" id='chkEmp<%#Eval("EmployeeId") %>' value='<%#Eval("EmployeeId") %>' title='<%#Eval("EmployeeId") %>' onclick="checkEmp(this, 1)" />
                                    <span id="span_empName" title='<%#Eval("EmployeeId") %>' onclick="checkEmp(this,2)"><%#Eval("EmployeeName") %></span>
                                </dd>
                            </ItemTemplate>
                        </asp:Repeater>
                    </dl>
                </div>
                <div class="modal-bottom">
                    <button class="btn btn-primary" type="button" data-toggle="tooltip" title="提示" onclick="SaveEmployee()">提交</button>
                    <button class="btn btn-danger" type="button" data-dismiss="modal" data-toggle="tooltip" title="提示" onclick="Close()">关闭</button>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        //选择收件人
        function SaveEmployee() {
            var chk = $(".dl_getEmp dd").find("input[name='chkEmp']:checked");
            if (chk.length > 0) {
                var id = "";
                chk.each(function () {
                    id = id + this.value + ",";
                });

                id = id.substr(0, id.lastIndexOf(','));

                hhl.ajax("/WebService/EmployeeHandler.asmx/GetEmployeeName", '{"employeeId":"' + id + '"}', function (result) {
                    if (result.d.IsSuccess) {
                        if (result.d.Data != null) {
                            var name = "";
                            for (var i = 0; i < result.d.Data.length; i++) {
                                var item = result.d.Data[i];
                                name += item.EmployeeName + ",";
                            }
                            name = name.substr(0, name.length - 1)
                            $("#txtgetMsg").val(name);
                            $("#txtgetMsg").attr("title", id);
                            $("#empSelectModal").modal("hide");
                        } else {
                            hhl.notify.error(result.d.Message, "提示");     //没有找到数据
                        }
                    } else {
                        hhl.notify.error(result.d.Message, "提示");
                    }
                });
            } else {
                hhl.notify.error("请选择收件人", "提示");
            }
        }


        //点击名称选中
        function checkEmp(obj, type) {

            var id = $(obj).attr("title");
            var state = $(".dl_getEmp dd").find("input[id='chkEmp" + id + "']").is(":checked");
            if (type == 1) {        //点击复选框或整行
                $(".dl_getEmp dd").find("input[id='chkEmp" + id + "']").prop("checked", !state);
            } else if (type == 2) {     //点击名称
                $(".dl_getEmp dd").find("input[id='chkEmp" + id + "']").prop("checked", state);
            }
        }

        function Close() {
            $("input[name='chkEmp']").prop("checked", false);
        }

    </script>
</asp:Content>
