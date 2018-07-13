<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/PopuMaster.Master" AutoEventWireup="true" CodeBehind="DetailsMessage.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Message.DetailsMessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

   

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

            hhl.ajax("/WebService/MessageHandler.asmx/GetMessage", '{"mid":"' + id + '","sendType":"' + type + '"}', function (result) {
                if (result.d.IsSuccess) {
                    var msg = result.d.Data;
                    $("#b_title").html(msg.MessageTitle);
                    $(".span_getMsg").text(msg.ToEmpName + "(" + msg.ToLoginName + ")");
                    $(".span_time").text(jsonDateFormat(msg.SendDateTime, 1));
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
            window.location.href = "MessageCreate.aspx?empId=" + employeeId + "&mid=" + '<%=Request["mid"]%>' + "&type=3&sendType=" + '<%=Request["sendType"] %>';
        }

        //点击转发
        function ForWard() {
            window.location.href = "MessageCreate.aspx?mid=" + '<%=Request["mid"]%>' + "&type=3&sendType=" + '<%=Request["sendType"] %>';
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
                                    url = "DetailsMessage.aspx?mid=" + msg.MessageId + "&sendType=" + type;
                                } else {
                                    if (type == 1) {
                                        url = "InBoxMessage.aspx";
                                    } else {
                                        url = "OutBoxMessage.aspx";
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel">
        <div class="panel-body">
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
                <a class="btn btn-info btn-sm" href='<%=Request["sendType"].ToString() == "1" ? "InBoxMessage.aspx" : "OutBoxMessage.aspx" %>'>《返回</a>
                <a class="btn btn-info btn-sm" onclick="Reply()">回复</a>
                <a class="btn btn-info btn-sm" onclick="ForWard()">转发</a>
                <a class="btn btn-info btn-sm" id="a_delete" onclick="ConfirmDel()">删除</a>
                <a class="btn btn-info btn-sm" id="a_pre" onclick="showPre()" data-toggle="tooltip">上一封</a>
                <a class="btn btn-info btn-sm" id="a_next" onclick="showNext()" data-toggle="tooltip">下一封</a>
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
                        window.location.href = "DetailsMessage.aspx?mid=" + msg.MessageId + "&sendType=" + type;
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
                        window.location.href = "DetailsMessage.aspx?mid=" + msg.MessageId + "&sendType=" + type;
                    }
                } else {
                    hhl.notify.error(result.d.Message, "提示");
                }
            });
        }
    </script>
</asp:Content>
