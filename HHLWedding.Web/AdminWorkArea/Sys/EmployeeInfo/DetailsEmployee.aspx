<%@ Page Language="C#" Title="个人信息" AutoEventWireup="true" MasterPageFile="~/AdminWorkArea/Master/PopuMaster.Master" CodeBehind="DetailsEmployee.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Sys.EmployeeInfo.DetailsEmployee" %>

<%@ Register Assembly="HHLWedding.ToolsLibrary" Namespace="HHLWedding.ToolsLibrary" TagPrefix="cc1" %>
<%@ Register Assembly="HHLWedding.Control" Namespace="HHLWedding.Control" TagPrefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <!--分页样式-->
    <link href="/Content/common/AspNetPager.css" rel="stylesheet" />
    <!--页码tip-->
    <script src="/Content/js/angle.js"></script>

    <link href="/Scripts/jedate/skin/jedate.css" rel="stylesheet" />
    <script type="text/javascript">

        //修改密码 弹出窗体
        function openModify() {
            var IsSuccess = false;
            hhl.dialog.openUpdate(["UpdatePassWord.aspx", "no"], "修改密码", {
                area: ["440px", "420px"],
            });
        }

        //修改个人信息 1.修改  2.返回
        function ModifyInfo(type) {
            if (type == 1) {
                $(".spanUserName").addClass("hide");
                $(".spanSex").addClass("hide");
                $(".spanPhone").addClass("hide");
                $("#btnModify").addClass("hide");


                $(".txtuserName").removeClass("hide");
                $(".rdoSex").removeClass("hide");
                $(".spanMan").removeClass("hide");
                $(".spanWoMan").removeClass("hide");
                $(".txtPhone").removeClass("hide");
                $("#btnBack").removeClass("hide");
                $("#btnSave").removeClass("hide");
            } else {
                $(".spanUserName").removeClass("hide");
                $(".spanSex").removeClass("hide");
                $(".spanPhone").removeClass("hide");
                $("#btnModify").removeClass("hide");


                $(".txtuserName").addClass("hide");
                $(".rdoSex").addClass("hide");
                $(".spanMan").addClass("hide");
                $(".spanWoMan").addClass("hide");
                $(".txtPhone").addClass("hide");
                $("#btnBack").addClass("hide");
                $("#btnSave").addClass("hide");
            }

        }

        //保存个人信息
        function SaveInfo() {
            //手机号码验证
            var reg = /^1[3|4|5|8][0-9]\d{8}$/;

            var name = $(".txtuserName").val();
            var sex = $(".rdoSex:checked").val();
            var phone = $(".txtPhone").val();

            //进行验证
            if (name == "") {
                hhl.notify.error("请输入姓名", "提示");
                $(".txtuserName").focus();
                return false;
            } else if (!reg.test(phone)) {
                hhl.notify.error("请输入正确的手机号码", "提示", { PositionClass: "toast-top-center" });
                $(".txtPhone").focus();
                return false;
            }

            //ajax提交
            hhl.ajax("/WebService/EmployeeHandler.asmx/ModifyEmpInfo", '{"empName":"' + name + '","sex":"' + sex + '","phone":"' + phone + '"}', function (result) {
                if (result.d.IsSuccess) {
                    hhl.notify.success(result.d.Message, "提示");
                    Back();
                } else {
                    hhl.notify.error(result.d.Message, "提示");
                }

            });
        }

        //返回
        function Back() {
            load();
            ModifyInfo(2);
        }


        //页面初始化
        $(function () {
            $("#tbl_details").find("tr").each(function () {
                var obj = $(this).find("td").eq(1);
                obj.find("span:first-child").css("height", "25px").css("width", "110px");
                obj.find("input[type='text']").css("height", "25px").css("width", "110px").css("margin-left", "-2px").css("margin-top", "-4px").css("color", "#797979");
            });
            load();

            //日期时间
            $("#start").jeDate({
                //dateCell: "#start",//isinitVal:true,
                format: "YYYY-MM-DD",
                isTime: false, //isClear:false,
                isClear: true,                        //是否显示清空
                isToday: true,                        //是否显示今天或本月
                ishmsVal: false,
                festival: true,                       //是否显示节日
            })
            //结束时间
            $("#end").jeDate({
                //dateCell: "#end",//isinitVal:true,
                format: "YYYY-MM-DD",
                isTime: false, //isClear:false,
                isClear: true,                        //是否显示清空
                isToday: true,                        //是否显示今天或本月
                festival: true,                       //是否显示节日
            })

            //下拉框选择员工
            $("#ddlEmployee_chosen span").click(function () {
                $(".chosen-results li").eq(0).removeClass("highlighted").remove("result-selected");

                var name = $(this).text();
                $(".chosen-results li").each(function (e) {
                    if ($(".chosen-results li").eq(e).text() == name) {
                        $(".chosen-results li").eq(e).addClass("highlighted");
                    }
                });
            });
        });

        //页面加载方法
        function load() {
            //页面加载获取
            hhl.ajax("/WebService/EmployeeHandler.asmx/GetEmployeeInfo", null, function (result) {
                if (result.d.IsSuccess) {
                    var model = result.d.Data;
                    $(".spanLoginName").text(model.LoginName);
                    $(".spanUserName").text(model.EmployeeName);
                    $(".spanJob").text(model.JobName);
                    var born = new Date(parseInt(model.BornDate.replace("/Date(", "").replace(")/", ""), 10));
                    var now = new Date();
                    var age = now.getFullYear() - born.getFullYear();
                    if (now.getMonth() < born.getMonth()) {
                        age = age - 1;
                    } else if (now.getMonth() == born.getMonth()) {
                        if (now.getDate() < born.getDate()) {
                            age = age - 1;
                        }
                    }

                    $(".spanAge").text(age);
                    $(".spanSex").text(model.Sex);
                    $(".spanPhone").text(model.Phone);
                    $(".spanType").text(model.TypeName);
                    $(".spanCreateDate").text(jsonDateFormat(2, model.CreateDate));


                    $(".txtuserName").val(model.EmployeeName);

                    $("input[name='rdoSex']").each(function () {
                        if (model.Sex == "男") {
                            if (this.value == 0) {
                                $(this).prop("checked", "checked");
                            }
                        } else {
                            if (this.value == 1) {
                                $(this).prop("checked", "checked");
                            }
                        }
                    });

                    $(".txtPhone").val(model.Phone);
                }
            });
        }

    </script>

    <style type="text/css">
        #tbl_Channel {
            margin-left: 1px;
            border-left: none;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel bg-white">
        <div class="panel-body">
            <div class="body-content widget-box pull-left" style="width: 30%; height: 100%;">
                <div class="widget-header" style="background-image: url(/images/header.png);">
                    <h4 class="h-title pull-left" style="color: #000; text-indent: 5px; line-height: 20px;">个人信息</h4>
                </div>
                <div class="widget-content">

                    <!---详细信息-->
                    <table id="tbl_details" class="table table-no-border bg-white">
                        <tr>
                            <td class="td-label">用户名:</td>
                            <td><span class="spanLoginName pull-left"></span></td>
                            <td>
                                <button type="button" id="btnModifyPwd" class="btn btn-warning btn-sm" onclick="openModify()" />
                                修改密码</td>
                        </tr>
                        <tr>
                            <td class="td-label">姓名:</td>
                            <td><span class="spanUserName pull-left"></span>
                                <input type="text" class="txtuserName pull-left hide" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td-label">职位:</td>
                            <td><span class="spanJob pull-left"></span></td>
                        </tr>
                        <tr>
                            <td class="td-label">年龄:</td>
                            <td><span class="spanAge pull-left"></span></td>
                        </tr>
                        <tr>
                            <td class="td-label">性别:</td>
                            <td><span class="spanSex pull-left"></span>
                                <input type="radio" class="rdoSex pull-left hide" value="0" name="rdoSex" /><span class="spanMan pull-left hide">男</span>
                                <input type="radio" class="rdoSex pull-left hide" value="1" name="rdoSex" style="margin-left: 10px;" /><span class="spanWoMan pull-left hide">女</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="td-label">联系电话:</td>
                            <td><span class="spanPhone pull-left"></span>
                                <input type="text" class="txtPhone pull-left hide" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td-label">权限:</td>
                            <td><span class="spanType pull-left"></span></td>
                        </tr>
                        <tr>
                            <td class="td-label">注册时间:</td>
                            <td><span class="spanCreateDate pull-left"></span></td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <button type="button" id="btnModify" class="btn btn-info btn-sm" onclick="ModifyInfo(1)">修改个人信息</button>
                                <button type="button" id="btnBack" class="btn btn-warning btn-sm hide" onclick="Back()">返回</button>
                                <button type="button" id="btnSave" class="btn btn-success btn-sm hide" onclick="SaveInfo()">保存信息</button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>

            <div class="body-content widget-box pull-left" style="width: 69%; height: 100%; padding-bottom: 10px;">
                <div class="widget-header" style="background-image: url(/images/header.png);">
                    <h4 class="h-title pull-left" style="color: #000; text-indent: 5px; line-height: 20px;">个人登录记录</h4>
                </div>
                <div class="widget-content">
                    <div class="form-inline" style="height: 50px;">
                        <div class="form-group form-inline pull-left" style="margin-left: 2px;">
                            <div class="pull-left">
                                <input class="chosen-text wicon" id="start" name="start" type="text" placeholder="开始时间" value="" readonly />
                            </div>
                            <div class="pull-left">
                                <span style="line-height: 30px;">&nbsp;—&nbsp;</span>
                            </div>
                            <div class="pull-left">
                                <input class="chosen-text wicon" id="end" name="end" type="text" placeholder="结束时间" value="" readonly />
                            </div>
                            <div class="pull-left">
                                &nbsp;&nbsp;<cc2:MyEmployee runat="server" ID="ddlEmployee" onchange="ddlChange()" ClientIDMode="Static" />
                            </div>
                        </div>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                            <ContentTemplate>
                                <div class="form-group col-sm-2 pull-left">
                                    <asp:Button runat="server" ID="btnConfirm" Text="查询" ClientIDMode="Static" CssClass="btn btn-primary" OnClick="btnConfirm_Click" Style="display: none" />
                                    <button type="button" class="btn btn-info" title="搜索" data-toggle="tooltip" id="_btnSearch" runat="server" onserverclick="btnConfirm_Click"><i class="icon-search"></i></button>
                                    <button type="button" class="btn btn-info" title="刷新" data-toggle="tooltip" id="btnRefresh" runat="server"><i class="icon-refresh" onclick="window.location.reload()"></i></button>

                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:Button runat="server" ID="btnExportExcel" CssClass="btn btn-primary" ToolTip="导出Excel" Text="导出Excel" OnClick="btnExportExcel_Click" />
                    </div>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                        <ContentTemplate>
                            <table id="tbl_Channel" class="table table-bordered table-hover bg-white table-list" style="margin-top: 10px;">
                                <thead>
                                    <tr class="th_title">
                                        <th>姓名</th>
                                        <th>登陆地点</th>
                                        <th>公网IP</th>
                                        <th>内网IP</th>
                                        <th>登录时间</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater runat="server" ID="repLoginLog">
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#GetEmployeeName(Eval("LoginEmployee")) %></td>
                                                <td><%#Eval("LoginCity") %></td>
                                                <td><%#Eval("LoginIpAddress") %></td>
                                                <td><%#Eval("LoginInIp") %></td>
                                                <td><%#Eval("LoginDate","{0:yyyy-MM-dd HH:mm:ss}") %></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                            <cc1:AspNetPagerTool ID="CtrPageIndex" ClientIDMode="Static" runat="server" OnPageChanged="CtrPageIndex_PageChanged" Style="margin-top: 10px; margin-bottom: 15px;"></cc1:AspNetPagerTool>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
