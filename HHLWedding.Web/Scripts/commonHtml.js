

(function ($) {

    //验证是否有客户登录
    checkLogin();


   
    //获取子页面url
    var url = window.location.href;
    var newUrl = url.substring(url.indexOf("/AdminWorkArea"));
    console.log(newUrl);
    var data = JSON.stringify({ url: newUrl });
    hhl.ajax("/WebService/ChannelHandler.asmx/GetChannelByUrl", data, function (result) {
        if (result.d.data != null) {
            var channel = result.d.data;
            $(".m_spanTheme").html(channel.StyleSheethem);
            $(".m_spanTitle").html(channel.ChannelName);
        }
    });

})(jQuery);


$(function () {
    //拼接html字符
    $("body").prepend(
                   "<div class='widget-box' style='height: 40px;'>" +
                       "<div class='crumb-wrap wrap-Title'>" +
                            "<i class='Hui-iconfont'>&#xe67f;</i> 首页" +
                            "<span class='c-gray en'> &gt;</span><span class='m_spanTheme'></span>" +
                            "<span class='c-gray en'> &gt;</span><span class='m_spanTitle'></span>" +
                            "<a class='btn btn-success radius r mr-20' href='javascript:location.replace(location.href);' data-toggle='tooltip' data-placement='left' title='刷新'><i class='icon-refresh'></i></a>" +
                       "</div>" +
                   "</div>" +
                   "<div style='height: 10px; border-bottom: 1px solid gray;'></div>");
});

//判断是否有用户登录
function checkLogin() {

    var cookieName = "Login";
    var isLogin = false;
    var cookies = document.cookie.split("; ");
    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i].split("=");
        if (cookie[0] == cookieName) {
            isLogin = true;
        }
    }

    //没有登录进入登录页面
    if (isLogin == false) {
        this.location.href = "/AdminWorkArea/Login.aspx";
    }
}


function getUrlParam(name) {
    //构造一个含有目标参数的正则表达式对象  
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    //匹配目标参数  
    var r = window.location.search.substr(1).match(reg);
    //返回参数值  
    if (r != null) return unescape(r[2]);
    return null;
}


//type 1.完整格式 年月日 时分秒  2.只有年月日
//dateType 1.格式 - 显示 (2016-02-04)    2.年月日显示格式 (2016年2月4日)
function jsonDateFormat(type, dateType, jsonDate) {//json日期格式转换为正常格式
    try {//出自http://www.cnblogs.com/ahjesus 尊重作者辛苦劳动成果,转载请注明出处,谢谢!
        var date = new Date();
        if (jsonDate) {
            var date = new Date(parseInt(jsonDate.replace("/Date(", "").replace(")/", ""), 10));
        }

        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hours = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();;
        var minutes = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();;
        var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();;
        var milliseconds = date.getMilliseconds();

        if (type == 1) {
            if (dateType == 1) {
                return date.getFullYear() + "-" + month + "-" + day + " " + hours + ":" + minutes + ":" + seconds;
            } else {
                return date.getFullYear() + "年" + month + "月" + day + "日 " + hours + ":" + minutes + ":" + seconds;
            }
        } else {
            if (dateType == 1) {
                return date.getFullYear() + "-" + month + "-" + day;
            } else {
                return date.getFullYear() + "年" + month + "月" + day + "日";
            }
        }
    } catch (ex) {//出自http://www.cnblogs.com/ahjesus 尊重作者辛苦劳动成果,转载请注明出处,谢谢!
        return "";
    }
}


