<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/PopuMaster.Master" AutoEventWireup="true" CodeBehind="Sys_EmployeeJobUpdate.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Sys.JobType.Sys_EmployeeJobUpdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        //提交修改
        function submitForm() {
            var name = $("#txtJobName").val();
            if (name == "") {
                hhl.notify.warn("请输入职务名称", "提示", { timeOut: 1000 });
                return false;
            }
            var jobId = getUrlParam("jobId");
            hhl.ajax("/WebService/EmployeeHandler.asmx/UpdateEmployeeJob", '{"_empJob":{"JobId":"' + jobId + '","Jobname":"' + name + '"}}', hhl.callbackRefresh);
        }

        //页面加载
        $(function () {
            var jobId = getUrlParam("jobId");
            hhl.ajax("/WebService/EmployeeHandler.asmx/GetEmpJobById", '{"JobId":"' + jobId + '"}', function (result) {
                if (result.d.IsSuccess) {
                    $("#txtJobName").val(result.d.Value);
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
                    <li class="li_title">职务名称</li>
                    <li class="li_title">
                        <div class="col-sm-9">
                            <input type="text" name="txtJobName" class="form-control" id="txtJobName" />
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
