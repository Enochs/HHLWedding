<%@ Page Title="" Language="C#" MasterPageFile="~/AdminWorkArea/Master/PopuMaster.Master" AutoEventWireup="true" CodeBehind="Sys_EmployeeJobCreate.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Sys.JobType.Sys_EmployeeJobCreate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        //提交添加
        function submitForm() {
            var name = $("#txtJobName").val();
            if (name == "") {
                hhl.notify.warn("请输入职务名称", "提示", { timeOut: 1000 });
                return false;
            }
            hhl.ajax("/WebService/EmployeeHandler.asmx/CreateEmployeeJob", '{"JobName":"' + name + '"}', hhl.callbackParent);
        }

        //按下回车键
        $(function () {
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
