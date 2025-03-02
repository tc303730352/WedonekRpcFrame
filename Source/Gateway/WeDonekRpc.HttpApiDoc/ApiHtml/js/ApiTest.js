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
        if (data.SubmitBody != null) {
            _initJson(data.SubmitBody);
        }
    });
}
var _editor;
function _initJson(data) {
    var schema = {
        title: "POST数据",
        type: "object",
        uniqueItems: true,
        items:[]
    };
    if (data.DataType == 1) {
        schema.type = "array";
        schema.format = "table";
    }
    for (var i = 0; i < data.ProList.length; i++) {
        var pro = data.ProList[i];

    }
    var doc = document.getElementById("JsonBody");
    _editor = new JSONEditor(doc, {
        theme: "bootstrap2",
        disable_properties: true,
        disable_collapse: true,
        disable_array_reorder: true,
        no_additional_properties: false,
        schema: {
            title: "负载配置",
            type: "array",
            format: "table",
            uniqueItems: true,
            items: {
                type: "object",
                title: "配置项",
                properties: {
                    TransmitType: {
                        type: "integer",
                        title: "负载方式",
                        enum: [{
                            title: "close",
                            value: 0
                        }, {
                            title: "ZoneIndex",
                            value: 1
                        },
                        {
                            title: "HashCode",
                            value: 2
                        }, {
                            title: "Number",
                            value: 3
                        }, {
                            title: "FixedType",
                            value: 4
                        }
                        ],
                        default: 0
                    },
                    Value: {
                        type: "string",
                        title: "固定值"
                    },
                    TransmitId: {
                        type: "integer",
                        title: "负载Id"
                    },
                    Range: {
                        type: "array",
                        title: "范围设定",
                        format: "table",
                        uniqueItems: true,
                        items: {
                            type: "object",
                            title: "取值范围",
                            properties: {
                                IsFixed: {
                                    type: "boolean",
                                    title: "固定值"
                                },
                                BeginRange: {
                                    type: "integer",
                                    title: "起始值"
                                },
                                EndRanage: {
                                    type: "integer",
                                    title: "结束值"
                                }
                            }
                        }
                    }
                }
            }
        }
    });
}