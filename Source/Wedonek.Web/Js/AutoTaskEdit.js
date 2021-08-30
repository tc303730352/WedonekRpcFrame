var _TaskData = null;
$(document).ready(function () {
    $("#SendType").on("change", function () {
        var type = parseInt($(this).val());
        _InitSendType(type);
    });
    _Init();
});
function _Init() {
    var id = urlTools.GetUrlValue("id");
    SubmitData("AutoTask/Get", { id: id }, null, function (data) {
        if (data != null) {
            $('#AutoTaskFrom').InitForm(data);
            _TaskData = data;
            _InitSendType(data.SendType);
        }
    });
}
function _InitSendType(type) {
    var data = null;
    if (_TaskData != null && _TaskData.SendType == type) {
        if (_TaskData.SendType == 0) {
            data = _format(_TaskData.SendParam.RpcConfig);
        } else if (_TaskData.SendType == 1) {
            data = _TaskData.SendParam.HttpConfig;
        } else {
            data = _format(_TaskData.SendParam.BroadcastConfig);
        }
    }
    if (type == "0") {
        _InitJson({
            title: "消息发送配置",
            type: "object",
            properties: {
                SystemType: {
                    type: "string",
                    title: "节点类型"
                },
                ServerId: {
                    type: "integer",
                    title: "服务节点Id"
                },
                SendConfig: {
                    type: "object",
                    title: "发送配置",
                    properties: {
                        SysDictate: {
                            type: "string",
                            title: "发送指令"
                        },
                        IsRetry: {
                            type: "boolean",
                            title: "失败是否重试",
                            format: "checkbox",
                            default: true
                        },
                        TransmitId: {
                            type: "integer",
                            title: "负载均衡Id"
                        },
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
                        IdentityColumn: {
                            type: "string",
                            title: "负载计算属性名"
                        },
                        ZIndexBit: {
                            type: "object",
                            title: "计算ZoneIndex索引位",
                            properties: {
                                One: {
                                    type: "integer",
                                    title: "首位"
                                },
                                Two: {
                                    type: "integer",
                                    title: "第二位"
                                }
                            }
                        },
                        LockType: {
                            type: "integer",
                            title: "锁类型",
                            enum: [{
                                title: "同步锁",
                                value: 0
                            }, {
                                title: "排斥锁",
                                value: 1
                            },
                            {
                                title: "普通锁",
                                value: 2
                            }],
                            default: 0
                        },
                        LockColumn: {
                            type: "array",
                            title: "用于锁计算的属性",
                            format: "table",
                            uniqueItems: true,
                            items: {
                                type: "string",
                                title: "属性名"
                            }
                        }
                    }
                },
                Data: {
                    type: "array",
                    title: "消息体",
                    format: "table",
                    items: {
                        type: "object",
                        properties: {
                            Name: {
                                type: "string",
                                title: "属性名"
                            }, Value: {
                                type: "string",
                                title: "属性值"
                            }
                        }
                    }
                }
            }
        }, data);
    }
    else if (type == "1") {
        _InitJson({
            title: "发送Http配置",
            type: "object",
            properties: {
                Uri: {
                    type: "string",
                    title: "请求地址",
                    format: "url"
                },
                RequestMethod: {
                    type: "string",
                    title: "请求方式",
                    default: "GET"
                },
                ReqType: {
                    type: "integer",
                    title: "请求类型",
                    enum: [{
                        title: "普通请求",
                        value: 0
                    }, {
                        title: "Json",
                        value: 1
                    },
                    {
                        title: "image",
                        value: 2
                    }, {
                        title: "File",
                        value: 3
                    }, {
                        title: "Html",
                        value: 4
                    }, {
                        title: "XML",
                        value: 5
                    }
                    ],
                    default: 0
                },
                PostParam: {
                    type: "string",
                    title: "POST提交的数据",
                    format: "textarea"
                }
            }
        }, data);
    }
    else {
        _InitJson({
            title: "广播发送配置",
            type: "object",
            properties: {
                TypeVal: {
                    type: "array",
                    title: "节点地址(可以是节点组或节点类型)",
                    format: "table",
                    uniqueItems: true,
                    items: {
                        type: "string",
                        title: "值"
                    }
                },
                ServerId: {
                    type: "array",
                    title: "节点列表",
                    format: "table",
                    uniqueItems: true,
                    items: {
                        type: "integer",
                        title: "服务节点ID"
                    }
                }, IsOnly: {
                    type: "boolean",
                    title: "是否限定唯一",
                    format: "checkbox",
                    default: true
                },
                IsCrossGroup: {
                    type: "boolean",
                    title: "是否跨集群广播",
                    format: "checkbox",
                    default: true
                },
                BroadcastType: {
                    type: "integer",
                    title: "广播方式",
                    enum: [{
                        title: "默认",
                        value: 0
                    }, {
                        title: "消息",
                        value: 1
                    },
                    {
                        title: "订阅",
                        value: 2
                    }
                    ],
                    default: 0
                },
                SendConfig: {
                    type: "object",
                    title: "发送配置",
                    properties: {
                        SysDictate: {
                            type: "string",
                            title: "发送指令"
                        },
                        IsRetry: {
                            type: "boolean",
                            title: "失败是否重试",
                            format: "checkbox",
                            default: true
                        },
                        TransmitId: {
                            type: "integer",
                            title: "负载均衡Id"
                        },
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
                        IdentityColumn: {
                            type: "string",
                            title: "负载计算属性名"
                        },
                        ZIndexBit: {
                            type: "object",
                            title: "计算ZoneIndex索引位",
                            properties: {
                                One: {
                                    type: "integer",
                                    title: "首位"
                                },
                                Two: {
                                    type: "integer",
                                    title: "第二位"
                                }
                            }
                        },
                        LockType: {
                            type: "integer",
                            title: "锁类型",
                            enum: [{
                                title: "同步锁",
                                value: 0
                            }, {
                                title: "排斥锁",
                                value: 1
                            },
                            {
                                title: "普通锁",
                                value: 2
                            }],
                            default: 0
                        },
                        LockColumn: {
                            type: "array",
                            title: "用于锁计算的属性",
                            format: "table",
                            uniqueItems: true,
                            items: {
                                type: "string",
                                title: "属性名"
                            }
                        }
                    }
                },
                Data: {
                    type: "array",
                    title: "消息体",
                    format: "table",
                    uniqueItems: true,
                    items: {
                        type: "object",
                        properties: {
                            Name: {
                                type: "string",
                                title: "属性名"
                            }, Value: {
                                type: "string",
                                title: "属性值"
                            }
                        }
                    }
                }
            }
        }, data);
    }
}
function _format(data) {
    if (data.SendConfig.ZIndexBit == null) {
        data.SendConfig.ZIndexBit = { One: 0, Two:0 };
    }
    if (data.SendConfig.LockColumn == null) {
        data.SendConfig.LockColumn = [];
    }
    if (data.SendConfig.IdentityColumn == null) {
        data.SendConfig.IdentityColumn="";
    }
    if (data.MsgBody == null || data.MsgBody == {}) {
        data.Data = [];
    }
    else {
        data.Data = [];
        for (var i in data.MsgBody) {
            data.Data.push({ Name: i, Value: data[i] });
        }
    }
    delete data.MsgBody;
    return data;
}
var _editor = null;
function _InitJson(schema, data) {
    var doc = document.getElementById("SendParam");
    if (_editor != null) {
        _editor.destroy();
    }
    _editor = new JSONEditor(doc, {
        theme: "bootstrap2",
        disable_properties: true,
        disable_collapse: true,
        disable_array_reorder: true,
        no_additional_properties: false,
        schema: schema
    });
    if (data != null) {
        _editor.setValue(data);
    }
}
function _Save() {
    var data = $('#AutoTaskFrom').FormData();
    data.Id = urlTools.GetUrlValue("id");
    var param = _editor.getValue();
    if (param.ServerId == 0) {
        delete param.ServerId;
    }
    if (data.SendType == 0 || data.SendType == 2) {
        if (param.SendConfig.TransmitType != 1) {
            delete param.SendConfig.ZIndexBit;
        }
        else if (param.SendConfig.ZIndexBit.One == 0 && param.SendConfig.ZIndexBit.Two == 0) {
            delete param.SendConfig.ZIndexBit;
        }
        if (param.SendConfig.IdentityColumn == "") {
            delete param.SendConfig.IdentityColumn;
        }
        if (param.Data.length != 0) {
            var obj = new Object();
            for (var i = 0; i < param.Data.length; i++) {
                var k = param.Data[i];
                if (k.Value != null && k.Value != "") {
                    obj[k.Name] = k.Value;
                }
            }
            delete param.Data;
            param.MsgBody = obj;
        } else {
            delete param.Data;
        }
    }
    if (data.SendType == 0) {
        data.SendParam = { RpcConfig: param };
    } else if (data.SendType == "1") {
        data.SendParam = { HttpConfig: param };
    } else {
        data.SendParam = { BroadcastConfig: param };
    }
    SubmitData("AutoTask/Set", null, data, function () {
        alert("修改成功!");
        window.location = "RpcMerInfo.aspx?id=" + urlTools.GetUrlValue("rid");;
    });
}