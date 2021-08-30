$(document).ready(function () {
    template.defaults.imports.GetApiTypeName = function (type) {
        if (type == 1) {
            return "文件上传";
        }
        else if (type == 2) {
            return "数据流";
        } else if (type == 3) {
            return "WebSocket接口";
        }
        else {
            return "接口";
        }
    };
    template.defaults.imports.FormatUpMaxSize = function (num) {
        if (num == 0) {
            return "无限制";
        }
        else {
            var str = "";
            var val = parseInt(num / (1024 * 1024 * 1024));//GB
            if (val != 0) {
                str += num + "GB";
                num = num % (1024 * 1024 * 1024);
            }
            val = parseInt(num / (1024 * 1024));//MB
            if (val != 0) {
                str += val + "MB";
                num = num % (1024 * 1024);
            }
            if (num != 0) {
                str += num + "KB";
            }
            return str;
        }
    };
    template.defaults.imports.GetReturnTypeName = function (type) {
        if (type == 1) {
            return "JSON";
        } else if (type == 2) {
            return "XML";
        } else if (type == 3) {
            return "数据流";
        } else if (type == 2) {
            return "字符串";
        } else if (type == 2) {
            return "HTTP状态值";
        } else if (type == 2) {
            return "页面地址跳转";
        }
        else {
            return "未知";
        }
    };
    var id = urlTools.GetUrlValue("id");
    _LoadApiInfo(id);
});
function _LoadApiInfo(id) {
    SubmitGetData("/GetApi?id=" + id, function (data) {
        var html = template('ApiInfo', data);
        $("#ApiData").html(html);
    });
}