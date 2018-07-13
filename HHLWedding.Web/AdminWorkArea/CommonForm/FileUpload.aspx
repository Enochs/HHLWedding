<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileUpload.aspx.cs" Inherits="HHLWedding.Web.AdminWorkArea.CommonForm.FileUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>文件上传</title>
    <!--jquery文件 -->
    <script src="/Scripts/jquery.js"></script>
    <script src="/Scripts/jquery.min.js"></script>

    <!--弹出层 layer   提示文件-->
    <link href="/Content/hhlset/layer/skin/layer.css" rel="stylesheet" />
    <script src="/Content/hhlset/layer/layer.js"></script>

    <!--文件上传 封装js文件 -->
    <script src="/Content/hhlset/webuploader/fileUpload.js"></script>
    <style type="text/css">
        .row {
            padding: 10px 0px 0px 50px;
        }

        .demo {
            min-width: 360px;
            margin: 30px auto;
            padding: 10px 20px;
        }

            .demo h3 {
                line-height: 40px;
                font-weight: bold;
            }

        .file-item {
            float: left;
            position: relative;
            width: 110px;
            height: 110px;
            margin: 0 20px 20px 0;
            padding: 4px;
        }

            .file-item .info {
                overflow: hidden;
            }

        .uploader-list {
            width: 100%;
            overflow: hidden;
        }
    </style>
</head>
<body>
    <div class="row">
        <div class="col-sm-6">
            <div class="Demo">
                <div id="uploadfile">
                    <div class="form-group form-inline">
                        <div id="picker" style="float: left">选择文件</div>
                        &nbsp;
                    <button id="ctlBtn" class="btn btn-default" style="padding: 8px 15px;">开始上传</button>
                    </div>
                    <!--用来存放文件信息-->
                    <div id="thelist" class="uploader-list"></div>
                </div>
            </div>
        </div>
    </div>



</body>
</html>
