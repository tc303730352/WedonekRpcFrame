var paging = { Index: 1, Size: 30 };
$(document).ready(function () {
    LoadControl($("#ServerGroup"), $("#ServerType"));
    _Init();
    template.defaults.imports.FormDate = function (date) {
        return new Date(date).Format("yyyy-MM-dd");
    };
    template.defaults.imports.FormatState = function (state) {
        if (state == 0) {
            return "正常";
        } else if (state == 1) {
            return "下线";
        } else {
            return "停用";
        }
    };
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
    if (_count != count || paging.Index == 1) {
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
    paging.Param = $('#QueryForm').FormData();
    SubmitData("Server/Query", null, paging, function (data) {
        var html = template('ListTemplate', data);
        $("#ServerList").html(html);
        _InitPaging(data.count);
    });
}
function _drop(id) {
    if (!window.confirm("确定删除？")) {
        return;
    }
    SubmitData("Server/Drop", { id: id }, null, function (data) {
        _Init();
    });
}
function _setState(id, state) {
    if (!window.confirm("确定进行该项操作？")) {
        return;
    }
    SubmitData("Server/SetState", { id: id, state: state }, null, function (data) {
        _Init();
    });
}