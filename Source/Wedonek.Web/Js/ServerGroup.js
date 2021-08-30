var paging = { Index: 1, Size: 20 };
$(document).ready(function () {
    LoadSysGroup($("#ServerGroup"));
    LoadSysGroup($("#ChoiseGroup"),false);
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
    paging.Param = $('#QueryForm').FormData();
    SubmitData("ServerType/Query", null, paging, function (data) {
        var html = template('ListTemplate', data);
        $("#SysTypeList").html(html);
        _InitPaging(data.count);
    });
}
function _drop(id) {
    if (!window.confirm("确定删除？")) {
        return;
    }
    SubmitData("ServerType/Drop", { id: id }, null, function (data) {
        _Init();
    });
}
function _SaveGroup() {
    var param = $('#AddGroup').FormData();
    SubmitData("ServerGroup/Add", null, param, function (data) {
        alert("添加成功!");
        $("#Add_Group").modal('hide');
    });
}
function _SaveSystemtType() {
    var param = $('#AddGroup').FormData();
    SubmitData("ServerGroup/Add", null, param, function (data) {
        alert("添加成功!");
        LoadSysGroup($("#ChoiseGroup"), false);
        $("#Add_Group").modal('hide');
    });
}
function _SaveSysType() {
    var param = $('#AddSysType').FormData();
    SubmitData("ServerType/Add", null, param, function (data) {
        alert("添加成功!");
        _Init();
        $("#Add_SysType").modal('hide');
    });
}