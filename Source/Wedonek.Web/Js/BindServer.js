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
    SubmitData("RemoteGroup/Query", { rpcmerid: id }, paging, function (data) {
        var html = template('ListTemplate', data);
        $("#ServerList").html(html);
        _InitPaging(data.count);
    });
}
function _unbind(id) {
    if (!window.confirm("确定解绑？")) {
        return;
    }
    SubmitData("RemoteGroup/Drop", { id: id }, null, function () {
        _Init();
    });
}
function _bind(serverId) {
    var id = urlTools.GetUrlValue("id");
    SubmitData("RemoteGroup/Set", null, { RpcMerId: id, ServerId: serverId }, function () {
        _Init();
    });
}