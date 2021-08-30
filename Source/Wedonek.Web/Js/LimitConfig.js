$(document).ready(function () {
    _InitLimit();
    $(".TokenLimit").hide();
    $(".TimeLimit").hide();
    $("#LimitType").on("change", function () {
        var type = $(this).val();
        _InitLimitType(type);
    });
    _InitLimitType($("#LimitType").val());
});
function _InitLimitType(type) {
    if (type == "0") {
        $(".TokenLimit").hide();
        $(".TimeLimit").hide();
    }
    else if (type == "3") {
        $(".TokenLimit").show();
        $(".TimeLimit").hide();
    } else {
        $(".TokenLimit").hide();
        $(".TimeLimit").show();
    }
}
function _InitLimit() {
    var serverId = urlTools.GetUrlValue("id");
    SubmitData("LimitConfig/Get", { serverId: serverId }, null, function (data) {
        if (data != null) {
            var isEnable = data.IsEnable;
            var isBucket = data.IsEnableBucket;
            delete data.IsEnable;
            delete data.IsEnableBucket;
            $('#ServerLimit').InitForm(data);
            if (isEnable) {
                $("#IsEnable").attr("checked", "checked");
            }
            if (isBucket) {
                $("#IsEnableBucket").attr("checked", "checked");
            }
            _InitLimitType(data.LimitType);
        } else {
            $("#DropConfig").hide();
        }
    });
}
function _Drop() {
    var serverId = urlTools.GetUrlValue("id");
    var that = this;
    SubmitData("LimitConfig/Drop", { serverId: serverId }, null, function (data) {
        alert("删除成功!");
        $(that).hide();
    });
}
function _SaveLimit() {
    var data = $('#ServerLimit').FormData();
    data.IsEnable = data.IsEnable == "on";
    data.IsEnableBucket = data.IsEnableBucket == "on";
    data.ServerId = urlTools.GetUrlValue("id");
    SubmitData("LimitConfig/Sync", null, data, function () {
        alert("设置成功!");
        $("#DropConfig").show();
    });
}