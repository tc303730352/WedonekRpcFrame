$(document).ready(function () {
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
function _Save() {
    var data = $('#DictateLimit').FormData();
    data.ServerId = urlTools.GetUrlValue("id");
    SubmitData("ServerDictate/Add", null, data, function () {
        alert("设置成功!");
        window.location = "RpcServer.aspx?id=" + data.ServerId;
    });
}