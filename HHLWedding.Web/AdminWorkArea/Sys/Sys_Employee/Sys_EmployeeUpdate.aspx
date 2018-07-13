<%@ Page Title="修改员工信息" Language="C#" MasterPageFile="~/AdminWorkArea/Master/PopuMaster.Master" AutoEventWireup="true" CodeBehind="Sys_EmployeeUpdate.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.Sys.Sys_Employee.Sys_EmployeeUpdate" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        //页面加载
        $(function () {
            var employeeId = getUrlParam("employeeId");
            hhl.ajax("/WebService/EmployeeHandler.asmx/GetEmpById", '{"employeeId":"' + employeeId + '"}', function (result) {
                if (result.d != null) {
                    var employee = result.d;
                    $("#txtEmpName").val(employee.EmployeeName);
                    $("#txtLoginName").val(employee.LoginName);
                    //$("#txtPwd").val(employee.PassWord);
                    $("#txtBornDate").val(jsonDateFormat(2,employee.BornDate));
                    $("#txtTelPhone").val(employee.TelPhone);
                    $("#txtIdCard").val(employee.IdCard);
                    $("#txtComeInDate").val(jsonDateFormat(2,employee.ComeInDate));
                    $("#preview").attr("src", employee.ImageURL);
                    $("#ddlSex option").eq(employee.Sex).attr("selected", true);
                    $("#ddlEmpJob").val(employee.JobID);
                    $("#ddlEmpType").val(employee.EmployeeTypeID);
                    $("#ddlDepart").val(employee.DepartmentID);
                }
            });

        });

        //提交修改
        function submitForm() {
            $("#confirm").click();
        }

        //数据验证
        function Validate() {

            if ($("#txtEmpName").val() == "") {
                hhl.notify.warn("请输入员工姓名", "提示");
                return false;
            } else if ($("#txtLoginName").val() == "") {
                hhl.notify.warn("请输入用户名", "提示");
                return false;
            } else if ($("#txtBornDate").val() == "") {
                hhl.notify.warn("请输入生日", "提示");
                return false;
            } else if ($("#txtTelPhone").val() == "") {
                hhl.notify.warn("请输入联系电话", "提示");
                return false;
            } else if ($("#txtIdCard").val() == "") {
                hhl.notify.warn("请输入身份证号码", "提示");
                return false;
            } else if ($("#ContentPlaceHolder1_image_file").val() == "") {
                if ($("#preview").attr("src") == "") {
                    hhl.notify.warn("请选择上传头像", "提示");
                    return false;
                }
            }
        }

    </script>

    <%-- 图片预览 --%>
    <script src="/Scripts/uploadscript.js"></script>
    <link href="/Content/upload/main.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel">
        <div class="panel-body col-md-12" style="margin-top: 20px;">
            <table id="tblSystem" class="table table-bordered">
                <tr>
                    <td>姓名</td>
                    <td>
                        <input type="text" class="form-control" id="txtEmpName" name="txtEmployeeName" /></td>
                </tr>
                <tr>
                    <td>职位</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlEmpJob" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>类型</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlEmpType" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>部门</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlDepart" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>性别</td>
                    <td>
                        <select class="form-control" id="ddlSex" name="ddlSex">
                            <option value="0">男</option>
                            <option value="1">女</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>用户名</td>
                    <td>
                        <div class="form-group">
                            <div class="input-group" style="width: 280px;">
                                <input type="text" class="form-control" id="txtLoginName" name="txtLoginName" readonly="true" style="height:37px;" />
                                <span class="input-group-addon" id="checkSpan" name="checkSpan">
                                    <i class="icon-ok" name="icon_check" id="icon_check" style="color: green"></i>
                                </span>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>密码</td>
                    <td>
                        <input type="password" class="form-control" id="txtPwd" name="txtPwd" /><span style="font-size: 11px; color: red;">(默认 123456)</span></td>
                </tr>
                <tr>
                    <td>生日</td>
                    <td>
                        <input type="text" class="form-control" id="txtBornDate" name="txtBornDate" onclick="WdatePicker()" /></td>
                </tr>
                <tr>
                    <td>联系电话</td>
                    <td>
                        <input type="text" class="form-control" id="txtTelPhone" name="txtTelPhone" /></td>
                </tr>
                <tr>
                    <td>身份证号码</td>
                    <td>
                        <input type="text" class="form-control" id="txtIdCard" name="txtIdCard" maxlength="18" /></td>
                </tr>
                <tr>
                    <td>个人头像</td>
                    <td>
                        <button type="button" class="btn btn-primary btn-xs" id="btnupload" onclick="document.getElementById('ContentPlaceHolder1_image_file').click()">上传头像</button>
                        <input type="file" runat="server" class="form-control" name="image_file" id="image_file" onchange="fileSelected();" title="请选择上传图片" data-toggle="tooltip" style="display: none;" /></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <img id="preview" />
                    </td>
                </tr>
                <tr>
                    <td>入职日期</td>
                    <td>
                        <input type="text" class="form-control" id="txtComeInDate" name="txtComeInDate" onclick="WdatePicker()" /></td>
                </tr>
                <tr style="display: none;">
                    <td>

                        <asp:Button runat="server" ID="confirm" Text="提交" ClientIDMode="Static" CssClass="btn btn-success" OnClick="confirm_Click" OnClientClick="return Validate()" />
                        <button type="button" id="btnBack" class="btn btn-danger">返回</button>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
