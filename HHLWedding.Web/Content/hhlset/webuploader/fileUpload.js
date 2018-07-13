
document.write('<link href="/Content/hhlset/webuploader/css/webuploader.css" rel="stylesheet" />' +
    '<link href="/Content/hhlset/webuploader/css/fileUp.css" rel="stylesheet" />' +
    '<link href="/Content/css/bootstrap.css" rel="stylesheet" />' +
    '<script src="/Content/hhlset/webuploader/webuploader.js"></script>');

$(function () {
    var $li = $('#thelist'),
        $delete = $('#delete'),
        $btn = $('#ctlBtn');

    var uploader = WebUploader.create({
        pick: {
            label: '选择文件',
            id: '#picker'
        },                  // 选择文件的按钮。可选
        resize: false, // 不压缩image
        swf: '/Content/hhlset/webuploader/Uploader.swf', // swf文件路径
        server: '/Content/hhlset/webuploader/fileupload.ashx', // 文件接收服务端。 
        chunked: true, //是否要分片处理大文件上传
        chunkSize: 2 * 1024 * 1024, //分片上传，每片2M，默认是5M
        chunkRetry: 2, //如果某个分片由于网络问题出错，允许自动重传次数
        fileNumLimit: 3,//文件个数
        fileSizeLimit: 1024 * 1024 * 1024,    // 1G
        fileSingleSizeLimit: 1024 * 1024 * 1024,    // 1G       设定单个文件大小

        //auto: false //选择文件后是否自动上传
        //runtimeOrder: 'html5,flash',
        //accept: {
        //    title: 'Files',
        //    extensions: 'doc,docx,xls,xlsx,ppt,pptx,txt',
        //    mimeTypes: '*'
        //}
    });

    // 当有文件被添加进队列的时候
    uploader.on('fileQueued', function (file) {
        //限制只能选择3个文件(当移除文件之后  再次选择文件 可以选择多个文件 并且也可以超过3个文件)
        var fileCount = $li.find("div[class='item']").length;

        //文件名
        var name = file.name;
        var length = file.name.length;
        if (length >= 50) {
            name = file.name.substring(0, 50) + "…";
        }

        var html = '<div id="' + file.id + '" class="item">' +
            '<span class="info" title="' + file.name + '">' + name + '</span>' +
            '<span class="remove-this btn btn-danger btn-sm" title="移除">×</span>' +
            '<p class="state">等待上传...</p>' +
            '</div>';
        $li.append(html);
        $delete = $li.find('span.remove-this');


        //移除文件
        $li.on('click', '.remove-this', function () {

            uploader.removeFile(file, true);
            $(this).parent().parent().remove();
            //重置uploder   目前只重置了文件队列
            uploader.reset();

        });

    });




    // 文件上传过程中创建进度条实时显示。
    uploader.on('uploadProgress', function (file, percentage) {
        var $li = $('#' + file.id),
            $percent = $li.find('.progress .progress-bar');

        // 避免重复创建
        if (!$percent.length) {
            $percent = $('<div class="progress progress-striped active">' +
              '<div class="progress-bar" role="progressbar" style="width: 0%;background-color:#218b3a;">' +
              '<span class="spanPercent"></span>' +
              '</div>' +
            '</div>').appendTo($li).find('.progress-bar');
        }

        $li.find('p.state').text('上传中');
        //console.log($percent.css('width'));
        var pefFloat = fomatFloat(percentage * 100, 2);
        $percent.css("text-align", "center").css('width', pefFloat + '%').find("span").text(pefFloat + '%');
        //console.log($percent.css('width') + "||" + $percent.find("span").text() + percentage);
    });
    // 文件上传成功
    uploader.on('uploadSuccess', function (file) {
        $('#' + file.id).find('p.state').text('上传成功').css("color", "green");;
        $delete.remove();
    });

    // 文件上传失败，显示上传出错
    uploader.on('uploadError', function (file) {
        $('#' + file.id).find('p.state').text('上传出错');
    });
    // 完成上传完
    uploader.on('uploadComplete', function (file) {
        $('#' + file.id).find('.progress-bar').removeClass("progress-bar");
        $('#' + file.id).find('.progress').css("background-color", "#218b3a").css("text-align", "center").find("span").text("100%").css("color", "white");

    });

    //点击开始上传
    $btn.on('click', function () {
        if ($(this).hasClass('disabled')) {
            return false;
        }
        var fileCount = $li.find("div[class='item']").length;
        if (fileCount > 0) {

            uploader.upload();
            var state = uploader.getStats();
        }

        // if (state === 'ready') {
        //     uploader.upload();
        // } else if (state === 'paused') {
        //     uploader.upload();
        // } else if (state === 'uploading') {
        //     uploader.stop();
        // }
    });

    //保留两位小数
    function fomatFloat(price, num) {
        return parseFloat(price).toFixed(num);
    }

});