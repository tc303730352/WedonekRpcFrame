$(document).ready(function () {
    LoadControl($("#ServerGroup"), $("#ServerType"), {
        IsAll: false
    });
});

function _Save() {
    var data = $('#RpcMerConfig').FormData();
    data.RpcMerId = urlTools.GetUrlValue("id");
    data.IsRegionIsolate = data.IsRegionIsolate == "no";
    SubmitData("RpcMerConfig/Add", null, data, function () {
        alert("添加成功!");
        window.location.href = "RpcMerInfo.aspx?id=" + data.RpcMerId;
    });
}
