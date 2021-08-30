$(document).ready(function () {
    _Init();
    $(".TokenLimit").hide();
    $("#LimitType").on("change", function () {
        var type = $(this).val();
        _InitLimitType(type);
    });
});

function _InitLimitType(type) {
   if (type == "3") {
        $(".TokenLimit").show();
        $(".TimeLimit").hide();
    } else {
        $(".TokenLimit").hide();
        $(".TimeLimit").show();
    }
}
function _Init() {
    var id = urlTools.GetUrlValue("merId");
    var serverId = urlTools.GetUrlValue("serverId");
    SubmitData("ClientLimit/Get", { rpcMerId: id, serverId: serverId }, null, function (data) {
        if (data != null) {
            var isEnable = data.IsEnable;
            delete data.IsEnable;
            $('#ClientLimitConfig').InitForm(data);
            if (isEnable) {
                $("#IsEnable").attr("checked", "checked");
            }
            _InitLimitType(data.LimitType);
        } else {
            $("#DropConfig").hide();
        }
    });
}
function _Drop() {
    var id = urlTools.GetUrlValue("merId");
    var serverId = urlTools.GetUrlValue("serverId");
    var that = this;
    SubmitData("ClientLimit/Drop", { rpcMerId: id, serverId: serverId }, null, function (data) {
        alert("删除成功!");
        $(that).hide();
    });
}
function _Save() {
    var data = $('#ClientLimitConfig').FormData();
    data.IsEnable = data.IsEnable == "on";
    data.RpcMerId = urlTools.GetUrlValue("merId");
    data.ServerId = urlTools.GetUrlValue("serverId");
    SubmitData("ClientLimit/Sync", null, data, function () {
        alert("设置成功!");
        $("#DropConfig").show();
    });
}