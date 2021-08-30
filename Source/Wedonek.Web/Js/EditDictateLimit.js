$(document).ready(function () {
    _Init();
    $(".TokenLimit").hide();
    $(".LeakyLimit").hide();
    $("#LimitType").on("change", function () {
        var type = $(this).val();
        _InitLimitType(type);
    });
});

function _InitLimitType(type) {
    if (type == "3") {
        $(".TokenLimit").show();
        $(".TimeLimit").hide();
        $(".LeakyLimit").hide();
    } else if (type == "4") {
        $(".TokenLimit").hide();
        $(".TimeLimit").hide();
        $(".LeakyLimit").show();
    } else {
        $(".TokenLimit").hide();
        $(".TimeLimit").show();
        $(".LeakyLimit").hide();
    }
}
function _Init() {
    var id = urlTools.GetUrlValue("id");
    SubmitData("ServerDictate/Get", { id: id }, null, function (data) {
        if (data != null) {
            $('#DictateLimit').InitForm(data);
            _InitLimitType(data.LimitType);
        } 
    });
}
function _Save() {
    var data = $('#DictateLimit').FormData();
    var id = urlTools.GetUrlValue("id");
    SubmitData("ServerDictate/Set", { id: id }, data, function () {
        alert("设置成功!");
        window.location = "RpcServer.aspx?id=" + urlTools.GetUrlValue("serverId");
    });
}