

var angle = angle || {};
(function ($) {
    angle.isMore = false;
})(jQuery);


///页面展开和收缩
$(function () {
    $("div .box").mouseenter(function () {
        $(this).css("background", "white").css("border-bottom", "1px solid #D9D9D9").css("height", "39px");
    });

    $("div .box").mouseleave(function () {
        $(this).css("background", "#F1F2F7").css("border-bottom", "1px solid #D9D9D9").css("height", "39px");
    });

    //边框重合
    $(".widget-box .widget-header").each(function (e) {
        if (e > 0) {
            $(this).parent().find("div[class='widget-header']").eq(e).css("border-top", "none");
        }
    });

    //展开/收缩
    $(".widget-header .box").click(function () {
        var self = $(this).parent();

        $(".widget-box .widget-header").each(function (e) {
            var header = $(this).parent().find("div[class='widget-header']").eq(e);
            if (header.html() == self.html()) {
                var content = $(this).parent().find("div[class='widget-content']").eq(e);
                var icon = header.find("i").eq(1);

                if (icon.hasClass("icon-angle-up")) {
                    content.css("min-height", "0px");
                    icon.removeClass("icon-angle-up").addClass("icon-angle-down");
                    content.slideUp();

                } else {
                    if (angle.isMore) {
                        icon.removeClass("icon-angle-down").addClass("icon-angle-up");
                        content.slideDown();

                    } else {

                        if (content.attr("title") != "" && content.attr("title") != undefined) {
                            content.css("height", "60" * content.attr("title") + "px");
                        } else {
                            content.css("height", "70px");
                        }
                        icon.removeClass("icon-angle-down").addClass("icon-angle-up");
                        content.slideDown();
                    }


                }
            }
        });
    });


    //页码显示
    SetPage();

});


function SetPage() {
    //pagenator(title提示)
    //当前页
    var cpage = $(".active").text();
    $(".paginator .active").attr("title", cpage)
    $(".paginator .active").attr("data-toggle", "tooltip");

    //每一页
    var page = $(".paginator").find("a")
    page.each(function (i) {
        var obj = page.eq(i);
        obj.attr("data-toggle", "tooltip");
        if (i == 0) {
            obj.attr("title", "首页");
        } else if (i == page.length - 1) {
            obj.attr("title", "末页");
        } else {
            obj.attr("title", obj.text());
        }
    });













    //$(".pagination-sm li").each(function (index) {

    //    var title = $(this).find("a[class='ng-binding']").html();
    //    console.log(title);
    //    $(this).find("a").attr("title", title);
    //    $(this).find("a").attr("data-toggle", "tooltip");
    //    $(this).find("a").attr("data-placement", "top");
    //});

    //setTimeout(function () {
    //    alert($(".pagination-sm li").length);
    //}, 3000)
}


