/// <reference path="E:\HHL_Wedding\Wedding\HHLWedding\HHLWedding.Web\Handler/EmployeeHandler.asmx" />
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
    hhl.notify.clear = function () {
        toastr.clear();
    }


    if (toastr) {
        toastr.options = {
            positionClass: 'toast-top-right',
            closeButton: true,
            progressBar: true,
            timeOut: 3000
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

    hhl.message = hhl.message || {};

    hhl.message.confirm = function (message, callback, cancelBack) {
        hhl.log.warn("hhl.message.confirm未实现");
    };

    hhl.dialog = hhl.dialog || {};

    hhl.dialog.open = function (url, title, options) {
        hhl.log.warn("hhl.dialog.open未实现");
    };

    hhl.dialog.openfull = function (url, title, options) {
        hhl.log.warn("hhl.dialog.openfull未实现");
    };

    hhl.dialog.openUpdate = function (url, title, options) {
        hhl.log.warn("hhl.dialog.openUpdate未实现");
    };

    hhl.dialog.load = function () {
        hhl.log.warn("hhl.dialog.load未实现");
    };

    hhl.dialog.closeAll = function () {
        hhl.log.warn("hhl.dialog.closeAll未实现");
    };

    hhl.dialog.close = function () {
        hhl.log.warn("hhl.dialog.close未实现");
    };

    hhl.requestDir = function (data) {
        //处理请求Data,主要用于排序
        if (data.sort) {
            var rtStr = "";
            for (var i = 0, len = data.sort.length; i < len; i++) {
                var nowS = data.sort[i];
                rtStr += nowS["field"] + "-" + nowS["dir"];
            }
            data.sort = rtStr;
        }
        return data;
    }


    hhl.layer = hhl.layer || {};
    if (layer) {
        var dialog = layer;
        hhl.layer = layer;


        //判断选择
        hhl.message.confirm = function (message, callback, icons) {
            if (!icons) {
                icons = 7;
            }
            dialog.confirm(message, {
                type: 0,
                area: ["360px", "155px"],
                btn: ["确定", "取消"],
                shade: false,
                icon: icons,
                title: "提示"
            }, function (index) {
                if (callback) {
                    callback(true);
                }
                dialog.close(index);
            }, function (index) {
                if (callback) {
                    callback(false);
                }
                dialog.close(index);
            })
        };

        //弹出窗口
        hhl.dialog.open = function (url, title, options) {
            options = options || {};
            var opts =
                {
                    type: 2,
                    title: title,
                    shade: 0.4,
                    maxmin: true,
                    area: ["80%", "80%"],
                    content: url,
                    skin: 'layui-layer-moon',
                    yes: function (index, layero) {
                        hhl.dialogYes(layero);
                    }
                };
            var args = $.extend({}, opts, options);
            dialog.open(args);
        };

        //弹出全屏
        hhl.dialog.openfull = function (url, title, options, getType) {
            var isSuccess = false;
            if (!getType) {
                getType = 1;
            }

            options = options || {};
            var opts =
                {
                    type: 2,
                    content: url,
                    title: title,
                    area: ["100%", "100%"],
                    btn: ["确定", "取消"],
                    maxmin: true,
                    yes: function (index, layero) {
                        hhl.dialogYes(layero);
                        isSuccess = true;
                    }, btn2: function (index, layero) {
                        isSuccess = false;
                    }, cancel: function () {
                        isSuccess = false;
                    },
                    end: function () {
                        if (isSuccess) {
                            $("#btnConfirm").click();
                            hhl.notify.clear();
                            if (getType == 1) {
                                hhl.notify.success("修改成功", "提示");
                            } else {
                                hhl.notify.success("添加成功", "提示");
                            }
                        }
                    }
                };
            var args = $.extend({}, opts, options);
            var index = dialog.open(args);

            layer.full(index);
        }

        //修改页面  修改之后 异步刷新列表  getType 1.修改成功  2.添加成功
        hhl.dialog.openUpdate = function (url, title, options, getType) {
            var isSuccess = false;
            if (!getType) {
                getType = 1;
            }
            options = options || {};
            var opts =
                {
                    type: 2,
                    btn: ["确定", "取消"],
                    title: title,
                    shade: 0.4,
                    maxmin: true,
                    area: ["80%", "80%"],
                    content: url,
                    yes: function (index, layero) {
                        hhl.dialogYes(layero);
                        isSuccess = true;
                    }, btn2: function (index, layero) {
                        isSuccess = false;
                    }, cancel: function () {
                        isSuccess = false;
                    },
                    end: function () {
                        if (isSuccess) {
                            $("#btnConfirm").click();
                            hhl.notify.clear();
                            if (getType == 1) {
                                hhl.notify.success("修改成功", "提示");
                            } else {
                                hhl.notify.success("添加成功", "提示");
                            }
                        }
                    }
                };
            var args = $.extend({}, opts, options);
            dialog.open(args);
        };

        //页面加载
        hhl.dialog.load = function (icon) {
            if (icon) {
                layer.load(icon);
            }
            else
                layer.load();
        }

        //关闭页面
        hhl.dialog.closeAll = function () {
            var index = dialog.getFrameIndex(window.name);
            dialog.closeAll();
        };

        //关闭当前页面
        hhl.dialog.close = function (index) {
            dialog.close(index);
        };
    }

    hhl.config = hhl.config || {};
    hhl.config.isPost = true;
    hhl.config.contentTypeJson = "application/json;charset=uft-8";
    hhl.config.contentTypeForm = "application/x-www-form-urlencoded; charset=utf-8";


    //form表单 提交验证
    hhl.ajax = function (url, data, callback, stringType, async, type, datatype, contentType) {
        if (!type) {
            type = "post";
        }
        if (!datatype) {
            datatype = "json";
        }
        if (!contentType) {
            contentType = hhl.config.contentTypeJson; //WebService 会返回Json类型";
        }

        if (stringType == 1) {
            data = JSON.stringify(data);
        }
        if (!async) {
            async = true;
        }

        $.ajax({
            async: async,
            cahce: false,
            type: type,
            contentType: contentType,
            url: url,
            datatype: datatype,
            data: data,
            timeout: 10000,
            beforeSend: function () {
                hhl.config.isPost = false;
                hhl.dialog.load();
            },
            complete: function () {
                hhl.dialog.closeAll();
                hhl.config.isPost = true;
            },
            error: function (jqXHP, status, error) {
                hhl.notify.error(jqXHP.responseText, "错误消息");
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

    //提交方法  弹出窗体
    hhl.dialogYes = function (layero) {
        var framewindow = layero.find("iframe")[0].contentWindow;
        framewindow.submitForm(hhl.callback);
    }
    //回调 调整页面高度
    hhl.callBackHeight = function () { };

    //回调  页面重新加载(默认方法)
    hhl.callback = function (result) {
        if (result.d.IsSuccess) {
            hhl.notify.success(result.d.Message, "提示");
            hhl.dialog.closeAll();
            setTimeout(function () {
                this.window.location.reload(true);
            }, 1500)
        } else {
            hhl.notify.warn(result.d.Message, "提示");
        }
    }

    //提示 并返回上一页(页面跳转使用)
    hhl.callbackGoHistory = function (result) {
        if (result.d.IsSuccess) {
            hhl.notify.success(result.d.Message, "提示");
            hhl.dialog.closeAll();
            setTimeout(function () {
                self.location = document.referrer;
            }, 1500)
        } else {
            hhl.notify.warn(result.d.Message, "提示");
        }
    }

    //提示 关闭当前 刷新父级页面(添加可用)
    hhl.callbackParent = function (result) {
        if (result.d.IsSuccess) {
            hhl.notify.success(result.d.Message, "提示")
            setTimeout(function () {
                parent.window.location.reload();
            }, 1200);
        } else {
            hhl.notify.warn(result.d.Message, "提示");
        }
    }

    //异步刷新  调用(修改页面)
    hhl.callbackRefresh = function (result) {
        if (result.d.IsSuccess) {
            var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
            parent.layer.close(index);
        } else {
            hhl.notify.warn(result.d.Message, "提示");
        }
    }

    //修改状态(本页改变)
    hhl.callbackStatus = function (result) {
        if (result.d.IsSuccess) {
            hhl.notify.clear();
            hhl.notify.success(result.d.Message, "提示", { icon: 7 });
            $("#btnConfirm").click();
        } else {
            hhl.notify.warn(result.d.Message, "提示");
        }
    }
    hhl.HtmlStatus = function (result) {
        if (result.d.IsSuccess) {
            hhl.notify.clear();
            hhl.notify.success(result.d.Message, "提示", { icon: 7 });
        } else {
            hhl.notify.warn(result.d.Message, "提示");
        }
    }


    //异步分页(mvc)
    hhl.ajaxPartial = function (page, formId, contentId) {
        if (hhl.config.isPost) {
            if (!contentId) {
                contentId = 'div_pager';
            }
            //var data = {};
            ////如果分页
            //if (page != null && page != "") {
            //    data = '{"page":"' + page + '"}';
            //}

            var url = $("#" + formId + " input[name='url']").val();

            var data = $("#" + formId).serialize();
            if (data.length > 0 && page != "" && page) {
                data = "page=" + page + "&" + data;
            }
            $.ajax({
                timeout: 10000,
                async: true,
                cache: false,
                beforeSend: function () {
                    hhl.config.isPost = false;
                    hhl.dialog.load();
                },
                complete: function () {
                    hhl.dialog.closeAll();
                    hhl.config.isPost = true;
                },
                data: '{"page":"' + page + '"}',
                contentType: "application/json", //WebService 会返回Json类型";
                type: "Post",
                url: url,
                success: function (result) {
                    document.getElementById(contentId).innerHTML = result;
                    //console.log(result.d.Value);
                    //$("#" + contentId).html(result.d.Value);
                    $('[data-toggle="tooltip"]').tooltip();
                },
                error: function (jqXHP, textStatus, errorThrown) {
                    hhl.notify.error(jqXHP.respnoseText, "错误消息");
                }
            });
        }
    }

})(jQuery);