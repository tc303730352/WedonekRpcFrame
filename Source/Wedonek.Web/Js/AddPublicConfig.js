function _Save() {
    var data = $('#RpcConfig').FormData();
    SubmitData("SysConfig/Add", null, data, function () {
        alert("添加成功!");
        window.location.href = "ConfigIndex.aspx";
    });
}
