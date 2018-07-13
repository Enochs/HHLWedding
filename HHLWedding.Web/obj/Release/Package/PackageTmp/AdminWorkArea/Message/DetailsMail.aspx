<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/PopuMaster.Master" AutoEventWireup="true" CodeBehind="DetailsMail.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Message.DetailsMail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="/Content/mail/bootstrap.min.css?v=3.3.6" rel="stylesheet" />
    <link href="/css/font-awesome.css?v=4.4.0" rel="stylesheet" />
    <link href="/Content/mail/animate.css" rel="stylesheet" />
    <link href="/Content/mail/style.css?v=4.1.0" rel="stylesheet" />
    <link href="/Content/hhlset/toastr/toastr.css" rel="stylesheet" />

    <script src="/Scripts/jquery.signalR-2.1.2.js"></script>
    <script src="/Scripts/jquery.signalR-2.1.2.min.js"></script>
    <script src="/Signalr/Hubs"></script>

    <style type="text/css">
        a {
            cursor: pointer;
        }

        .a_show {
            font-size: 12px;
        }

        a:hover {
            text-decoration: none;
            /*color: blue;*/
        }

        #div_details {
            font-size: 12px;
        }

        .span_title {
            color: #AFBCDA;
            line-height: 23px;
            font-size: 13px;
        }

        h3 {
            color: #000000;
        }

        .panel-heading {
            width: 100%;
            background-color: #F4F6F8;
        }

        .span_getMsg {
            font-weight: bold;
            color: #5FA207;
        }

        .span_sendMsg {
            font-weight: bold;
            color: #a84013;
        }

        #divContent {
            min-height: 400px;
        }
    </style>

    <script type="text/javascript">
        var employeeId = "";
        $(function () {
            $(".a_show").click(function () {
                $("#div_details").slideToggle();
            });



            //加载页面详细信息
            var id = '<%=Request["mid"] %>';
            var type = '<%=Request["sendType"] %>';

            //如果是发件箱  就不能回复   可以转发
            if (type == 2) {        //发件箱
                $("#btnReply").hide();      //隐藏回复
                $("#btnForWard").hide();    //隐藏转发
            }

            hhl.ajax("/WebService/MessageHandler.asmx/GetMessage", '{"mid":"' + id + '","sendType":"' + type + '"}', function (result) {
                if (result.d.IsSuccess) {
                    var msg = result.d.Data;
                    $("#b_title").html(msg.MessageTitle);
                    $(".span_getMsg").text(msg.ToFullName);
                    var date = new Date(parseInt(msg.SendDateTime.replace("/Date(", "").replace(")/", ""), 10));
                    var hours = date.getHours();
                    var time = ["凌晨", "早上", "上午", "下午", "傍晚", "晚上"];
                    var hour = [6, 8, 12, 18, 20, 24];
                    var dayTime = "";
                    var hourRule = "";
                    for (var i = 0; i < time.length; i++) {
                        if (hours < hour[i]) {
                            dayTime = time[i];
                            break;
                        }
                    }
                    if (hours > 12) {
                        hourRule = hours - 12;
                    }

                    var week = date.getDay();
                    var day = new Array("日", "一", "二", "三", "四", "五", "六");


                    $(".span_time").text(jsonDateFormat(2, msg.SendDateTime, 1) + "  (星期" + day[week] + ") " + dayTime + " " + hourRule + ":" + date.getMinutes());
                    $(".spant_theme").text(msg.MessageTitle);
                    $(".span_sendMsg").text(msg.FromEmpName + "(" + msg.FromLoginName + ")");
                    $("#divContent").html(msg.MessageContent);
                    employeeId = msg.FromEmployee;
                }
            });

            //上一封
            hhl.ajax("/WebService/MessageHandler.asmx/PreMsg", '{"mid":"' + id + '","sendType":"' + type + '"}', function (result) {
                if (result.d.IsSuccess) {
                    var msg = result.d.Data;
                    if (msg != null) {
                        $("#a_pre").attr("data-original-title", msg.MessageTitle);
                    } else {
                        $("#a_pre").attr("data-original-title", result.d.Message);
                        $("#a_pre").attr("disabled", "disabled");
                    }
                }
            });


            //下一封
            hhl.ajax("/WebService/MessageHandler.asmx/NextMsg", '{"mid":"' + id + '","sendType":"' + type + '"}', function (result) {
                if (result.d.IsSuccess) {
                    var msg = result.d.Data;
                    if (msg != null) {
                        $("#a_next").attr("data-original-title", msg.MessageTitle);
                    } else {
                        $("#a_next").attr("data-original-title", result.d.Message);
                        $("#a_next").attr("disabled", "disabled");
                    }
                }
            });
        });

        //点击回复
        function Reply() {
            window.location.href = "MailCreate.aspx?empId=" + employeeId + "&mid=" + '<%=Request["mid"]%>' + "&type=3&sendType=" + '<%=Request["sendType"] %>';
        }

        //点击转发
        function ForWard() {
            window.location.href = "MailCreate.aspx?mid=" + '<%=Request["mid"]%>' + "&type=3&sendType=" + '<%=Request["sendType"] %>';
        }

        //点击删除
        function ConfirmDel(obj) {
            var id = '<%=Request["mid"] %>';
            var type = '<%=Request["sendType"] %>';
            var url = window.location.href;


            hhl.message.confirm("你确定删除本条消息吗?", function (result) {

                //提前获取页面(删除之后  就不能获取实体类)

                if (result) {
                    $.ajax({
                        async: true,           //外部能够获取url
                        type: "post",
                        contentType: "application/json",
                        url: "/WebService/MessageHandler.asmx/GetLaterMsg",
                        datatype: "json",
                        data: '{"mid":"' + id + '","sendType":"' + type + '"}',
                        success: function (result) {
                            if (result.d.IsSuccess) {
                                var msg = result.d.Data;
                                if (msg != null) {
                                    url = "DetailsMail.aspx?mid=" + msg.MessageId + "&sendType=" + type;
                                } else {
                                    if (type == 1) {
                                        url = "InBoxMail.aspx";
                                    } else {
                                        url = "OutBoxMail.aspx";
                                    }
                                }
                            } else {
                                hhl.notify.error(result.d.Message, "提示");
                            }
                        },
                        error: function (jqXHP, status, error) {
                            hhl.notify.error(jqXHP.responseText, "错误消息");
                        }
                    });


                    hhl.ajax("/WebService/MessageHandler.asmx/DeleteMsg", "{'mid':'" + id + "'}", function (result) {
                        //删除成功
                        if (result.d.IsSuccess) {
                            hhl.notify.success("删除成功", "提示");
                            setTimeout(function () {
                                window.location.href = url;
                            }, 1000)

                        } else {
                            hhl.notify.error(result.d.Message, "提示");
                        }
                    });
                }
            });
        }

        $(function () {
            InBoxMessage();
        });

        //获取未读信息条数方法
        function InBoxMessage() {
            //同步加载
            $.ajax({
                async: true,
                type: "post",
                contentType: "application/json",
                url: "/WebService/MessageHandler.asmx/GetNoReadMsg",
                datatype: "json",
                success: function (result) {
                    if (result.d.IsSuccess) {
                        $(".inMessage").text(result.d.Value);
                        $("#spanDraftCount").text(result.d.Count);
                    } else {
                        //window.location.reload();
                        hhl.notify.error("系统错误");
                    }
                }
            });
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="wrapper wrapper-content">
        <div class="row">
            <div class="col-sm-3">
                <div class="ibox float-e-margins">
                    <div class="ibox-content mailbox-content">
                        <div class="file-manager">
                            <a class="btn btn-block btn-primary compose-mail" href="MailCreate.aspx">写信</a>
                            <div class="space-25"></div>
                            <h5>文件夹</h5>
                            <ul class="folder-list m-b-md" style="padding: 0">
                                <li>
                                    <a href="InBoxMail.aspx"><i class="fa fa-inbox "></i>收件箱 <span class="label label-warning pull-right inMessage"></span>
                                    </a>
                                </li>
                                <li>
                                    <a href="OutBoxMail.aspx"><i class="fa fa-envelope-o"></i>发信</a>
                                </li>
                                <li>
                                    <a href="OutBoxMail.aspx?type=1"><i class="fa fa-file-text-o"></i>草稿 <span id="spanDraftCount" class="label label-danger pull-right">0</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="mailbox.html"><i class="fa fa-trash-o"></i>重要邮件</a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="mail-outBox" class="col-sm-9 animated fadeInRight">

                <div class="mail-box">
                    <div class="form-horizontal panel-heading">
                        <div class="form-group">
                            <h4><b id="b_title"></b></h4>
                        </div>
                        <div class="form-group">
                            <span class="span_title">收件人:&nbsp;&nbsp;</span><span class="span_getMsg"></span>&nbsp;&nbsp;<a class="a_show">详情</a>
                            <div id="div_details" style="display: none;">
                                <span class="span_title">时&nbsp;&nbsp;  间:&nbsp;&nbsp;</span><span class="span_time"></span><br />
                                <span class="span_title">主&nbsp;&nbsp;  题:&nbsp;&nbsp;</span><span class="spant_theme"></span><br />
                                <span class="span_title">发件人:</span>&nbsp;&nbsp;<span class="span_sendMsg"></span>
                            </div>
                        </div>
                    </div>
                    <div id="divContent" class="form-group">
                        &nbsp;&nbsp; 
                    </div>
                    <div class="panel-footer">
                        <a class="btn btn-info btn-sm" href='<%=Request["sendType"].ToString() == "1" ? "InBoxMail.aspx" : "OutBoxMail.aspx" %>'>《返回</a>
                        <a class="btn btn-info btn-sm" id="btnReply" onclick="Reply()">回复</a>
                        <a class="btn btn-info btn-sm" id="btnForWard" onclick="ForWard()">转发</a>
                        <a class="btn btn-info btn-sm" id="a_delete" onclick="ConfirmDel()">删除</a>
                        <a class="btn btn-info btn-sm" id="a_pre" onclick="showPre()" data-toggle="tooltip">上一封</a>
                        <a class="btn btn-info btn-sm" id="a_next" onclick="showNext()" data-toggle="tooltip">下一封</a>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">

        //点击上一封
        function showPre() {
            var Id = '<%=Request["mid"] %>';
            var type = '<%=Request["sendType"] %>';
            hhl.ajax("/WebService/MessageHandler.asmx/PreMsg", '{"mid":"' + Id + '","sendType":"' + type + '"}', function (result) {
                if (result.d.IsSuccess) {
                    var msg = result.d.Data;
                    if (msg != null) {
                        window.location.href = "DetailsMail.aspx?mid=" + msg.MessageId + "&sendType=" + type;
                    }
                } else {
                    hhl.notify.error(result.d.Message, "提示");
                }
            });
        }


        //点击下一封
        function showNext() {
            var Id = '<%=Request["mid"] %>';
            var type = '<%=Request["sendType"] %>';

            hhl.ajax("/WebService/MessageHandler.asmx/NextMsg", '{"mid":"' + Id + '","sendType":"' + type + '"}', function (result) {
                if (result.d.IsSuccess) {
                    var msg = result.d.Data;
                    if (msg != null) {
                        window.location.href = "DetailsMail.aspx?mid=" + msg.MessageId + "&sendType=" + type;
                    }
                } else {
                    hhl.notify.error(result.d.Message, "提示");
                }
            });
        }
    </script>
</asp:Content>
