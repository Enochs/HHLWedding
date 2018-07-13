<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/PopuMaster.Master" AutoEventWireup="true" CodeBehind="Sys_EmployeeTypeUpdate.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Sys.JobType.Sys_EmployeeTypeUpdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

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

        //提交修改
        function submitForm() {
            var name = $("#txtTypeName").val();
            if (name == "") {
                hhl.notify.warn("请输入类型名称", "提示", { timeOut: 1000 });
                return false;
            }
            var TypeId = getUrlParam("TypeId");
            hhl.ajax("/WebService/EmployeeHandler.asmx/UpdateEmployeeType", '{"_empType":{"EmployeeTypeID":"' + TypeId + '","TypeName":"' + name + '"}}', hhl.callbackRefresh);
        }

        //页面加载
        $(function () {
            var TypeId = getUrlParam("TypeId");
            hhl.ajax("/WebService/EmployeeHandler.asmx/GetEmpTypeById", '{"TypeId":"' + TypeId + '"}', function (result) {
                if (result.d.IsSuccess) {
                    $("#txtTypeName").val(result.d.Value);
                } else {
                    hhl.notify.warn("警告错误", "提示");
                }
            });

            $("#txtJobName").keydown(function () {
                if (event.keyCode == 13) {
                    submitForm();
                }
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel">
        <div class="widget-box">
            <div class="widget-content bg-white" style="border-top: 1px solid #D9D9D9;">
                <ul>
                    <li class="li_title">类型名称</li>
                    <li class="li_title">
                        <div class="col-sm-9">
                            <input type="text" name="txtTypeName" class="form-control" id="txtTypeName" />
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
