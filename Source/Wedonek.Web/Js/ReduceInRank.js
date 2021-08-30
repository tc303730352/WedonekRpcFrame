$(document).ready(function () {
    _Init();
});

function _Init() {
    var id = urlTools.GetUrlValue("merId");
    var serverId = urlTools.GetUrlValue("serverId");
    SubmitData("ReduceInRank/Get", { rpcMerId: id, serverId: serverId }, null, function (data) {
        if (data != null) {
            $('#ReduceInRank').InitForm(data);
        }
    });
}
function _Save() {
    var data = $('#ReduceInRank').FormData();
    data.RpcMerId = urlTools.GetUrlValue("merId");
    data.ServerId = urlTools.GetUrlValue("serverId");
    SubmitData("ReduceInRank/Sync", null, data, function () {
        alert("设置成功!");
        window.location = "RpcMerInfo.aspx?id=" + data.RpcMerId;
    });
}