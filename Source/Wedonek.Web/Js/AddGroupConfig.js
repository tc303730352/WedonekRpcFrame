function _Save() {
    var data = $('#RpcConfig').FormData();
    data.SystemTypeId = urlTools.GetUrlValue("id");
    SubmitData("SysConfig/Add", null, data, function () {
        alert("添加成功!");
        window.location.href = "GroupConfig.aspx?id=" + data.SystemTypeId;
    });
}