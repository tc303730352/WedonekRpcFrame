var paging = { Index: 1, Size: 30 };
$(document).ready(function () {
    template.defaults.imports.GetValueType = function (type) {
        if (type == 0) {
            return "字符串";
        }
        return "JSON";
    };
    _Init();
    $("#QueryBtn").on("click", function () {
        paging.Index = 1;
        _Init();
    });
});
var _count = 0;
function _InitPaging(count) {
    if (count == 0) {
        $("#Pagination").Hide();
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
                    _Init();
                }
            }
        });
    }
}
function _Init() {
    var id = urlTools.GetUrlValue("id");
    paging.Param = $('#QueryForm').FormData();
    paging.Param.SystemTypeId = id;
    paging.Param.RpcMerId = 0;
    SubmitData("SysConfig/Query", null, paging, function (data) {
        var html = template('ConfigsTemplate', data);
        $("#ConfigList").html(html);
        _InitPaging(data.count);
    });
}
function _showJson(id) {
    var control = $('#show_Json');
    var json = $('#Json_' + id).html();
    $(control).find(".modal-body").JSONView(json, { expand: true, nl2br: true, recursive_collapser: true });
    $(control).modal('toggle');
}

function _dropConfig(id) {
    if (!window.confirm("确定删除该配置？")) {
        return;
    }
    SubmitData("SysConfig/Drop", { configId: id }, null, function (data) {
        _Init();
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
        _Init();
        $('#Set_Config').modal('hide');
    });
}