var paging = { Index: 1, Size: 30 };
var con_paging = { Index: 1, Size: 30 };
$(document).ready(function () {
    template.defaults.imports.FormDate = function (date) {
        return new Date(date).Format("yyyy-MM-dd");
    };
    template.defaults.imports.GetValueType = function (type) {
        if (type == 0) {
            return "字符串";
        }
        return "JSON";
    };
    template.defaults.imports.FormatTransmitType = function (type) {
        if (type == 0) {
            return "无";
        }
        else if (type == 1) {
            return "ZoneIndex(取首字母和末尾字母的ASCII值取或)";
        } else if (type == 2) {
            return "HashCode";
        } else if (type == 3) {
            return "数字范围";
        } else {
            return "固定值";
        }
    };
    template.defaults.imports.FormatRange = function (range) {
        if (range == null) {
            return "";
        }
        if (range.IsFixed) {
            return "值=" + range.BeginRange + "\r\n"
        } else {
            return "值 >=" + range.BeginRange + " && 值 < " + range.EndRanage + "\r\n";
        }
    };
    LoadServerRegion($("#RegionId"), false);
    _InitServer();
    _InitConfig();
});
var _editor = null;
function _InitServer() {
    var id = urlTools.GetUrlValue("id");
    SubmitData("Server/Get", { id: id }, null, function (data) {
        var html = template('DatumTemplate', data);
        $("#home").html(html);
        $('#RpcServerEdit').InitForm(data);
        LoadControl($("#ServerGroup"), $("#ServerType"), {
            IsAll: false,
            IsGroup: false,
            GroupId: data.GroupId,
            SystemType: data.SystemType
        });
        var doc = document.getElementById("TransmitDiv");
        if (_editor == null) {
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
        _editor.setValue(data.TransmitConfig);
    });
}

function _Save() {
    var data = $('#RpcServerEdit').FormData();
    var id = urlTools.GetUrlValue("id");
    data.TransmitConfig = _editor.getValue();
    SubmitData("Server/Set", { id: id }, data, function (id) {
        alert("修改成功!");
        _InitServer();
    });
}

function _InitConfig() {
    var id = urlTools.GetUrlValue("id");
    paging.Param = $('#QueryForm').FormData();
    paging.Param.ServerId = id;
    SubmitData("SysConfig/Query", null, paging, function (data) {
        var html = template('ConfigsTemplate', data);
        $("#ConfigList").html(html);
        _InitPaging(data.count);
    });
}

var _count = 0;
function _InitPaging(count) {
    if (count == 0) {
        $("#Pagination").hide();
        return;
    }
    $("#Pagination").show();
    if (_count != count || paging.Index == 1) {
        $("#Pagination").pagination({
            totalData: count,
            showData: paging.Size,
            current: paging.Index,
            homePage: '首页',
            endPage: '末页',
            prevContent: '上页',
            nextContent: '下页',
            coping: true,
            callback: function (e) {
                var index = e.getCurrent();
                if (index != paging.Index) {
                    paging.Index = index;
                    _InitConfig();
                }
            }
        });
    }
}
function _showJson(id) {
    var control = $('#show_Json');
    var json = $('#Json_' + id).html();
    $(control).find(".modal-body").JSONView(json, { expand: true, nl2br: true, recursive_collapser: true });
    $(control).modal('toggle')
}
function _dropConfig(id) {
    if (!window.confirm("确定删除该配置？")) {
        return;
    }
    SubmitData("SysConfig/Drop", { configId: id }, null, function (data) {
        _InitConfig();
    });
}

var _conCount = 0;
function _InitConPaging(count) {
    if (_conCount == 0) {
        $("#Con_Pagination").hide();
        return;
    }
    $("#Con_Pagination").show();
    if (_conCount != count || con_paging.Index == 1) {
        $("#Con_Pagination").pagination({
            totalData: count,
            showData: con_paging.Size,
            current: con_paging.Index,
            homePage: '首页',
            endPage: '末页',
            prevContent: '上页',
            nextContent: '下页',
            coping: true,
            callback: function (e) {
                var index = e.getCurrent();
                if (index != con_paging.Index) {
                    con_paging.Index = index;
                    _InitCon();
                }
            }
        });
    }
}

var _configId = null;
function _setConfig(id) {
    _configId = id;
    SubmitData("SysConfig/Get", { configId: id }, null, function (data) {
        $("#ConfigValue").val(data.Value);
        $("#ValueType").val(data.ValueType);
        $('#Set_Config').modal('toggle');
    });
}
function _SaveConfig() {
    var val = $("#ConfigValue").val();
    SubmitData("SysConfig/Set", null, {
        Id: _configId,
        Value: val,
        ValueType: parseInt($("#ValueType").val())
    }, function (data) {
        _InitConfig();
        $('#Set_Config').modal('hide');
    });
}