var paging = { Index: 1, Size: 30 };
$(document).ready(function () {
    template.defaults.imports.formatIp = function (ips) {
        if (ips == null || ips.length == 0) {
            return "*";
        }
        else if (ips.length == 1) {
            return ips[0];
        }
        else {
            var str = "";
            var i = 0;
            for (var i in ips) {
                if (++i > 5) {
                    break;
                }
                str += "<p>" + i + "</p>";
            }
            return str;
        }
    };
    _Init();
    $("#QueryBtn").on("click", function () {
        paging.Index = 1;
        _Init();
    });
});
var _count = 0;
function _InitPaging(count) {
    if (count == 0) {
        $("#Pagination").Hide();
        return;
    }
    $("#Pagination").show();
    if (_count != count || paging.Index==1) {
        $("#Pagination").pagination({
            totalData: count,
            showData: paging.Size,
            current: paging.Index,
            homePage: '首页',
            endPage: '末页',
            prevContent: '上页',
            nextContent: '下页',
            coping: true,
            callback: function (e) {
                var index = e.getCurrent();
                if (index != paging.Index) {
                    paging.Index = index;
                    _Init();
                }
            }
        });
    }
}
function _Init() {
    var name = $("#MerName").val();
    SubmitData("RpcMer/Query", { name: name }, paging, function (data) {
        var html = template('MerListTemplate', data);
        $("#MerList").html(html);
        _InitPaging(data.count);
    });
}
function _drop(id) {
    if (!window.confirm("确定删除？")) {
        return;
    }
    SubmitData("RpcMer/Drop", { id: id }, null, function (data) {
        _Init();
    });
}