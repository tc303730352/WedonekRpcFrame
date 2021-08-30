
function _Save() {
    var data = $('#MainForm').FormData();
    if (data.AllowServerIp == "") {
        data.AllowServerIp = new Array();
        data.AllowServerIp.push("*");
    }
    else {
        data.AllowServerIp = data.AllowServerIp.split(",");
    }
    SubmitData("RpcMer/Add", null, data, function (id) {
        window.location = "RpcMerEdit.aspx?id=" + id;
    });
}