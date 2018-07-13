//下拉框选择变化
function ddlChange() {
    $("#btnConfirm").click();       //执行查询
}


$(function () {




    //图片展示
    var clegth = $("#file").find("img").length;
    var img = new Array(clegth);

    $('#file tr td').find("img").each(function (index) {
        img[index] = $(this).attr("src");

        var option = {
            images: img
        };

        $(this).click(function () {
            var event = $(this).parent().find("span").html();
            $('#file').imagesGrid(option, event);
        });

    });


    //禁用文本框
    $(".input-disable").attr("disabled", "disabled");


    //婚期
    //日期时间
    $("#StartPdate").jeDate();
    //结束时间
    $("#EndPdate").jeDate();
    $(".txtDate").jeDate();
});


