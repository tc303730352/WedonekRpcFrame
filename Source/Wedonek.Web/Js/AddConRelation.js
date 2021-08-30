var paging = { Index: 1, Size: 30 };
$(document).ready(function () {
    LoadControl($("#ServerGroup"), $("#ServerType"));
    _Init();
    $("#QueryBtn").on("click", function () {
        paging.Index = 1;
        _Init();
    });
});
var _count = 0;
function _InitPaging(count) {
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
    SubmitData("ServerConRelation/QueryRelation", { serverId: id }, paging, function (data) {
        data.serverId = id;
        var html = template('ListTemplate', data);
        $("#ServerList").html(html);
        _InitPaging(data.count);
    });
}
function _unbind(id) {
    if (!window.confirm("删除关系？")) {
        return;
    }
    var serverId = urlTools.GetUrlValue("id");
    SubmitData("ServerConRelation/Drop", { serviceId: serverId, remoteId: id }, null, function () {
        _Init();
    });
}
function _bind(remoteId) {
    var id = urlTools.GetUrlValue("id");
    SubmitData("ServerConRelation/Add", { serviceId: id, remoteId: remoteId }, null, function () {
        _Init();
    });
}