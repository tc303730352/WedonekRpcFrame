var _editor = null;
$(document).ready(function () {
    LoadServerRegion($("#RegionId"), false);
    _InitServer();
});
function _InitServer() {
    LoadControl($("#ServerGroup"), $("#ServerType"), {
        IsAll: false,
        IsGroup: false
    });
    var doc = document.getElementById("TransmitDiv");
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

function _Save() {
    var data = $('#RpcServerEdit').FormData();
    data.TransmitConfig = _editor.getValue();
    SubmitData("Server/Add", null, data, function (id) {
        alert("添加成功!");
        window.location = "RpcServer.aspx?id=" + id;
    });
}