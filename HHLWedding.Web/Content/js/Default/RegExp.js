
// ----------------------------------------------------------------------
// <summary>
// 手机号码验证
// </summary>
// ----------------------------------------------------------------------
function isRegPhone() {
    var isPhone = /^1[34578][0-9]{9}$/;
    return isPhone;
}

(function ($) {
    $.fn.regPhone = function () {
        $(this).on("keyup", function () {
            var isPhone = /^1[34578][0-9]{9}$/;
            var phone = $(this).val();
            if (isPhone.test(phone)) {
                $(this).parent().find("span").addClass("hide");
            } else {
                $(this).parent().find("span").removeClass("hide");
            }
        });
    }


})(jQuery);

// ----------------------------------------------------------------------
// <summary>
// 限制只能输入数字
// </summary>
// ----------------------------------------------------------------------
$.fn.onlyNum = function () {
    $(this).keyup(function (event) {
        $(this).val($(this).val().replace(/[^\d]/g, ''));
    }).focus(function () {
        //禁用输入法
        this.style.imeMode = 'disabled';
    }).bind("paste", function () {
        //CTRL+事件处理
        $(this).val($(this).val().replace(/[^\d]/g, ''));
        return false;
    }).bind("blur", function () {
        //CTRL+V事件处理
        $(this).val($(this).val().replace(/[^\d]/g, ''));
    });
};

$.fn.onlyPhone = function () {
    $(this).on("keyup", function () {
        var isPhone = /^1[34578][0-9]{9}$/;
        var phone = $(this).val();
        if (isPhone.test(phone)) {
            //$(this).parent().find("span").addClass("hide");
            console.log("验证成功");
        } else {
            //$(this).parent().find("span").removeClass("hide");
        }
    });
}



$(function () {

    $("input[name='txtisNum']").onlyNum();




});
