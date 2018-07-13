var hhl = hhl || {};
(function ($) {

    //$('[data-toggle="tooltip"]').tooltip();


    hhl.log = hhl.log || {};
    hhl.log.levels = {
        DEBUG: 1,
        INFO: 2,
        WARN: 3,
        ERROR: 4
    };

    hhl.log.level = hhl.log.levels.DEBUG;
    hhl.log.log = function (logObject, logLevel) {
        if (!window.console || !window.console.log) { return; }
        if (logLevel != undefined && logLevel < hhl.log.levels) { return; }
        console.log(logObject);
    }

    hhl.log.debug = function (logObject) {
        hhl.log.log("DEBUG", hhl.log.levels.DEBUG);
        hhl.log.log(logObject, hhl.log.levels.DEBUG);
    }

    hhl.log.info = function (logObject) {
        hhl.log.log("INFO", hhl.log.levels.INFO);
        hhl.log.log(logObject, hhl.log.levels.INFO);
    }

    hhl.log.warn = function (logObject) {
        hhl.log.log("WARN", hhl.log.levels.WARN);
        hhl.log.log(logObject, hhl.log.levels.WARN);
    }

    hhl.log.error = function (logObject) {
        hhl.log.log("ERROR", hhl.log.levels.ERROR);
        hhl.log.log(logObject, hhl.log.levels.ERROR);
    }


    hhl.notify = hhl.notify || {};

    hhl.notify.success = function (message, title, options) {
        hhl.log.warn("未实现");
    }

    hhl.notify.info = function (message, title, options) {
        hhl.log.warn("未实现");
    }

    hhl.notify.warn = function (message, title, options) {
        hhl.log.warn("未实现");
    }

    hhl.notify.error = function (message, title, options) {
        hhl.log.warn("未实现");
    }


    if (toastr) {
        toastr.options = {
            positionClass: 'toast-top-center',
            closeButton: true,
            progressBar: true
        };

        var ShowNotification = function (type, message, title, options) {
            toastr[type](message, title, options);
        };

        hhl.notify.success = function (messsage, title, options) {
            ShowNotification('success', messsage, title, options)
        }

        hhl.notify.info = function (messsage, title, options) {
            ShowNotification('info', messsage, title, options)
        }

        hhl.notify.warn = function (messsage, title, options) {
            ShowNotification('warning', messsage, title, options)
        }

        hhl.notify.error = function (messsage, title, options) {
            ShowNotification('error', messsage, title, options)
        }

        hhl.notify.clear = function () {
            toastr.clear();
        }

    }


    if (layer) {
        var dialog = layer;

        hhl.message = hhl.message || {};
        hhl.dialog = hhl.dialog || {};

        var showMessage = function (type, message, title) {
            dialog.alert(message, { icon: type, title: title });
        }

        hhl.message.info = function (message, title) {
            return showMessage(0, message, title);
        }

        hhl.message.warn = function (message, title) {
            return showMessage(1, message, title);
        }

        hhl.message.success = function (message, title) {
            return showMessage(2, message, title);
        }

        hhl.message.error = function (message, title) {
            return showMessage(3, message, title);
        }


        hhl.message.confirm = function (message, callback) {
            dialog.confirm(message, {
                btn: ["确定", "取消"],
                shade: false
            }), function (index) {
                if (callback) {
                    callback(true);
                }
                dialog.close(index);
            }, function (index) {
                if (callback) {
                    callback(false);
                }
                dialog.close(index);
            }
        };

        hhl.dialog.open = function (url, title, options) {
            options = options || {};
            var opts =
                {
                    type: 2,
                    title: title,
                    shade: 0.4,
                    maxmin: true,
                    area: ["80%", "80%"],
                    content: url
                };
            var args = $.extend({}, opts, options);
            dialog.open(args);
        };

        hhl.dialog.closeAll = function () {
            var index = dialog.getFrameIndex(window.name);
            dialog.closeAll();
        };
    }

    //form表单 提交验证
    hhl.ajax = function (url, data, callback, type, datatype) {
        if (!type) {
            type = "post";
        }
        if (!datatype) {
            datatype = "json";
        }

        $.ajax({
            type: type,
            data: data,
            datatype: datatype,
            url: url,
            async: true,
            timeout: 10000,
            error: function (request, status, error) {
                hhl.notify.error("失败");
            },
            success: function (result) {
                if (callback) {
                    callback(result)
                } else {
                    hhl.callback(result);

                }
            }
        });
    }

    hhl.util = hhl.util || {};
    //获取form表单中的数据
    hhl.util.getFormData = function (form) {
        var serialiazeObj = {};
        var $this = $(form);
        var array = $this.serializeArray();
        var str = $this.serialize();
        $(array).each(function () {
            if (serialiazeObj[this.name]) {
                if ($.isArray(serialiazeObj[this.name])) {
                    serialiazeObj[this.name].push(this.value);
                } else {
                    serialiazeObj[this.name] = [serialiazeObj[tshi.name], this.value];
                }
            } else {
                serialiazeObj[this.name] = this.value;
            }
        });
        return serialiazeObj;
    };

    //弹出窗体
    hhl.dialogYes = function (layero) {
        var framewindow = layero.find("iframe")[0].contentWindow;
        framewindow.submitForm(hhl.callback);
    }

    //回调
    hhl.callback = function (result) {
        if (result.IsSuccess) {
            hhl.notify.success(result.Message, "提示");
            hhl.dialog.closeAll();
            setTimeout(function () {
                this.window.location.reload(true);
            }, 1500)
        } else {
            hhl.notify.warn(result.Message, "提示");
        }
    }

    hhl.callbackGoHistory = function (result) {
        if (result.IsSuccess) {
            hhl.notify.success(result.Message, "提示");
            hhl.dialog.closeAll();
            setTimeout(function () {
                self.location = document.referrer;
            }, 1500)
        } else {
            hhl.notify.warn(result.Message, "提示");
        }
    }

})(jQuery);