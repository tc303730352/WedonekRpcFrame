var paging = { Index: 1, Size: 10 };
$(document).ready(function () {
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
    SubmitData("Error/Query", null, paging, function (data) {
        var html = template('ListTemplate', data);
        $("#ServerList").html(html);
        _InitPaging(data.count);
    });
}
var _errorId = null;
var _lang = null;
function _Set(id, lang,text) {
    _lang = lang;
    _errorId = id;
    if (text == null) {
        text = "";
    }
    var control = $('#Set_Error');
    $(control).find("#ErrorMsg").val(text);
    $(control).modal('toggle');
}
function _Save() {
    var text = $("#Set_Error").find("#ErrorMsg").val();
    if (text == null || text == "") {
        return;
    }
    SubmitData("Error/SetMsg", null, {
        ErrorId: _errorId,
        Lang: _lang,
        Msg: text
    }, function () {
            _Init();
            $("#Set_Error").modal("hide");
    });
}