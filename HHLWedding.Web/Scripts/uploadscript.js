
var iMaxFilesize = 1048576; // 1MB


function fileSelected() {
    debugger
    // ѡ�е��ļ�
    var oFile = document.getElementById('ContentPlaceHolder1_image_file').files[0];

    // ��Ч�ļ�
    var rfilter = /^(image\/bmp|image\/gif|image\/jpeg|image\/png|image\/tiff)$/i;
    if (!rfilter.test(oFile.type)) {
        hhl.notify.warn("�ļ���ʽ����", "��ʾ");
        return;
    }


    //�ļ��ߴ�
    if (oFile.size > iMaxFilesize) {
        hhl.notify.warn("�ļ��ߴ����", "��ʾ");
        return;
    }

    //ͼƬ��(ͼƬչʾλ��)
    var oImage = document.getElementById('preview');

    var oReader = new FileReader();
    oReader.onload = function (e) {
        oImage.src = e.target.result;
    };
    oReader.readAsDataURL(oFile);
}
