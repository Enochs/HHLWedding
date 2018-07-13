
var iMaxFilesize = 1048576; // 1MB


function fileSelected() {
    debugger
    // 选中的文件
    var oFile = document.getElementById('ContentPlaceHolder1_image_file').files[0];

    // 有效文件
    var rfilter = /^(image\/bmp|image\/gif|image\/jpeg|image\/png|image\/tiff)$/i;
    if (!rfilter.test(oFile.type)) {
        hhl.notify.warn("文件格式错误", "提示");
        return;
    }


    //文件尺寸
    if (oFile.size > iMaxFilesize) {
        hhl.notify.warn("文件尺寸过大", "提示");
        return;
    }

    //图片框(图片展示位置)
    var oImage = document.getElementById('preview');

    var oReader = new FileReader();
    oReader.onload = function (e) {
        oImage.src = e.target.result;
    };
    oReader.readAsDataURL(oFile);
}
