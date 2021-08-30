var paging = { Index: 1, Size: 30 };
var task_paging = { Index: 1, Size: 30 };
$(document).ready(function () {
    template.defaults.imports.GetValueType = function (type) {
        if (type == 0) {
            return "字符串";
        }
        return "JSON";
    };
    template.defaults.imports.GetTaskType = function (type) {
        if (type == 0) {
            return "定时任务";
        }
        else if (type == 1) {
            return "间隔任务";
        }
        return "定时间隔任务";
    };
    template.defaults.imports.GetSendType = function (type) {
        if (type == 0) {
            return "RPC消息";
        }
        else if (type == 1) {
            return "HTTP请求";
        }
        return "广播信息";
    };
    $("#myTab a").bind("click", _InitTab);
    LoadControl($("#ServerGroup"), $("#ServerType"));
    _InitRpcMer();
    $("#QueryBtn").on("click", function () {
        paging.Index = 1;
        _InitConfig();
    });
    $("#TaskQueryBtn").on("click", function () {
        task_paging.Index = 1;
        _InitTask();
    });
});
function _InitTab() {
    var tab = $(this).attr("aria-controls");
    if (tab == "home") {
        _InitRpcMer();
    } else if (tab == "profile") {
        _InitServer();
    } else if (tab == "contact") {
        _InitConfig();
    } else if (tab == "task") {
        _InitTask();
    } else if (tab == "MerConfig") {
        _InitMerConfig();
    }
}
function _InitMerConfig() {
    var id = urlTools.GetUrlValue("id");
    SubmitData("RpcMerConfig/Gets", { rpcMerId: id }, null, function (data) {
        var html = template('MerConfigListTemplate', { list: data });
        $("#MerConfigList").html(html);
    });
}
var _task_count = 0;
function _InitTaskPaging(count) {
    if (count == 0) {
        $("#Task_Pagination").hide();
        return;
    }
    $("#Task_Pagination").show();
    if (_count != count || task_paging.Index == 1) {
        _task_count = count;
        $("#Task_Pagination").pagination({
            totalData: count,
            showData: task_paging.Size,
            current: task_paging.Index,
            homePage: '首页',
            endPage: '末页',
            prevContent: '上页',
            nextContent: '下页',
            coping: true,
            callback: function (e) {
                var index = e.getCurrent();
                if (index != task_paging.Index) {
                    task_paging.Index = index;
                    _InitTask();
                }
            }
        });
    }
}
function _dropMerConfig(id) {
    if (!window.confirm("确定删除？")) {
        return;
    }
    SubmitData("RpcMerConfig/Drop", { id: id }, null, function (data) {
        _InitMerConfig();
    });
}
function _InitTask() {
    var id = urlTools.GetUrlValue("id");
    task_paging.Param = $('#TaskQueryForm').FormData();
    task_paging.Param.RpcMerId = id;
    SubmitData("AutoTask/Query", null, task_paging, function (data) {
        var html = template('TaskTemplate', data);
        $("#TaskList").html(html);
        _InitTaskPaging(data.count);
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
        _count = count;
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
function _InitConfig() {
    var id = urlTools.GetUrlValue("id");
    paging.Param = $('#QueryForm').FormData();
    paging.Param.RpcMerId = id;
    SubmitData("SysConfig/Query", null, paging, function (data) {
        var html = template('ConfigsTemplate', data);
        $("#ConfigList").html(html);
        _InitPaging(data.count);
    });
}
function _InitRpcMer() {
    var id = urlTools.GetUrlValue("id");
    SubmitData("RpcMer/Get", { id: id }, null, function (data) {
        data.AllowServerIp = $(data.AllowServerIp).Join(",");
        $('#RpcMerEdit').InitForm(data);
    });
}
function _Save() {
    var data = $('#RpcMerEdit').FormData();
    if (data.AllowServerIp == "") {
        data.AllowServerIp = new Array();
        data.AllowServerIp.push("*");
    }
    else {
        data.AllowServerIp = data.AllowServerIp.split(",");
    }
    var id = urlTools.GetUrlValue("id");
    SubmitData("RpcMer/Set", { id: id }, data, function () {
        window.location.reload();
    });
}

function _InitServer() {
    var id = urlTools.GetUrlValue("id");
    SubmitData("RemoteGroup/Get", { rpcmerid: id }, null, function (data) {
        var html = template('ListTemplate', { list: data });
        $("#ServerList").html(html);
    });
}
function _dropConfig(id) {
    if (!window.confirm("确定删除该配置？")) {
        return;
    }
    SubmitData("SysConfig/Drop", { configId: id }, null, function (data) {
        _InitConfig();
    });
}
function _dropTask(id) {
    if (!window.confirm("确定删除该任务？")) {
        return;
    }
    SubmitData("AutoTask/Drop", { id: id }, null, function (data) {
        _InitTask();
    });
}
function _drop(id) {
    if (!window.confirm("确定解绑？")) {
        return;
    }
    SubmitData("RemoteGroup/Drop", { id: id }, null, function (data) {
        _InitServer();
    });
}
function _showJson(id) {
    var control = $('#show_Json');
    var json = $('#Json_' + id).html();
    $(control).find(".modal-body").JSONView(json, { expand: true, nl2br: true, recursive_collapser: true });
    $(control).modal('toggle');
}
var _dataId = 0;
function _setWeight(id) {
    _dataId = id;
    var control = $('#Set_Weight');
    $(control).modal('toggle');
}
function _SaveWeight() {
    SubmitData("RemoteGroup/SetWeight", {
        id: _dataId,
        weight: parseInt($("#WeightNum").val())
    }, null, function (data) {
        _InitServer();
        $('#Set_Weight').modal('hide');
    });
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
var _merConfigId = 0;
function _setMerConfig(id) {
    _merConfigId = id;
    SubmitData("RpcMerConfig/Get", {
        id: id
    }, null, function (data) {
            var control = $('#Set_MerConfig');
            if (data.IsRegionIsolate) {
                $("#IsRegionIsolate").attr("checked", "checked");
            } else {
                $("#IsRegionIsolate").removeAttr("checked");
            }
            $("#IsolateLevel").val(data.IsolateLevel.toString().toLowerCase());
        $(control).modal('toggle');
    });
}
function _SaveMerConfig() {
    SubmitData("RpcMerConfig/Set", {
        id: _merConfigId
    }, {
        IsRegionIsolate: $("#IsRegionIsolate").val()=="on",
        IsolateLevel: $("#IsolateLevel").val(),
    }, function (data) {
        _InitMerConfig();
        $('#Set_MerConfig').modal('hide');
    });
}