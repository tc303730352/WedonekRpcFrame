$(document).ready(function () {
    _Init();
});
function _Init() {
    SubmitData("ServerRegion/Gets", null, null, function (list) {
        var html = template('ListTemplate', { list: list });
        $("#RegionList").html(html);
    });
}
var _id = 0;
function _set(id) {
    _id = id;
    var control = $('#Set_Region');
    $(control).modal('toggle');
}
function _add() {
    _id = 0;
    var control = $('#Set_Region');
    $(control).modal('toggle');
}
function _save() {
    var name = $("#RegionName").val();
    if (_id == 0) {
        _AddData(name);
        return;
    }
    _SetData(name);
}
function _SetData(name) {
    SubmitData("ServerRegion/Set", null, { RegionName: name, Id: _id }, function () {
        alert("修改成功!");
        _Init();
        $('#Set_Region').modal('hide');
    });
}
function _AddData(name) {
    SubmitData("ServerRegion/Add", { name: escape(name) }, null, function () {
        alert("添加成功!");
        _Init();
        $('#Set_Region').modal('hide');
    });
}
function _drop(id) {
    if (!window.confirm("确定删除该区域？")) {
        return;
    }
    SubmitData("ServerRegion/Drop", { id: id }, null, function () {
        alert("删除成功!");
        _Init();
    });
}