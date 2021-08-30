$(document).ready(function () {
    LoadControl($("#ServerGroup"), $("#ServerType"), {
        IsAll: false
    });
});

function _Save() {
    var data = $('#RpcConfig').FormData();
    data.RpcMerId = urlTools.GetUrlValue("id");
    SubmitData("SysConfig/Add", null, data, function () {
        alert("添加成功!");
        window.location.href = "RpcMerInfo.aspx?id=" + data.RpcMerId;
    });
}
