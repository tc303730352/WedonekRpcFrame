$(document).ready(function () {
    template.defaults.imports.GetApiTypeName = function (type) {
        if (type == 1) {
            return "文件上传";
        }
        else if (type == 2) {
            return "数据流";
        } else if (type == 3) {
            return "WebSocket接口";
        }
        else {
            return "接口";
        }
    };
    _LoadApiList();
});

function _LoadApiList() {
    SubmitGetData("/GetApiGroup", function (data) {
        var obj = new Object();
        obj.list = data;
        var html = template('ApiTr', obj);
        $("#ApiList").html(html);
    });
}