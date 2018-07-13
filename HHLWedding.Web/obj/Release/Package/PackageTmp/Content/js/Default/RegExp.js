
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
