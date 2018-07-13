$(function () {
    $("div .box").mouseenter(function () {
        $(this).css("background", "white").css("border-bottom", "1px solid #D9D9D9").css("height", "39px");
    });

    $("div .box").mouseleave(function () {
        $(this).css("background", "#F1F2F7").css("border-bottom", "1px solid #D9D9D9").css("height", "39px");
    });

    $("div .box").click(function () {
        var icon = $("#icon_angle");
        if (icon.hasClass("icon-angle-down")) {
            $(".widget-content").css("min-height", "0px");
            icon.removeClass("icon-angle-down").addClass("icon-angle-up");
            $(".widget-content").slideToggle();
        } else {
            $(".widget-content").css("height", "70px");
            icon.removeClass("icon-angle-up").addClass("icon-angle-down");
            $(".widget-content").slideToggle();
            setTimeout(function () {
                $(".widget-content").css("min-height", "70px").css("height", "auto");
            }, 500);

        }

    });

    SetPage();

});


function SetPage() {
    //pagenator(title提示)
    //当前页
    var cpage = $(".active").text();
    $(".active").attr("title", cpage)
    $(".active").attr("data-toggle", "tooltip");

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
}