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

$(function () {

    //$("input[name='txtisNum']").onlyNum();

});