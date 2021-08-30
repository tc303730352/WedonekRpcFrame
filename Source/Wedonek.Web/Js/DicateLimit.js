var dicate_paging = { Index: 1, Size: 20 };
$(document).ready(function () {
    template.defaults.imports.GetLimitType = function (type) {
        if (type == 0) {
            return "不启用";
        }
        else if (type == 1) {
            return "固定时间窗";
        } else if (type == 2) {
            return "流动时间窗";
        } else if (type == 3) {
            return "令牌桶";
        }
        return "漏斗";
    };
    _InitDicate();
    $("#DictateQueryBtn").on("click", function () {
        dicate_paging.Index = 1;
        _InitDicate();
    });
});
var _dicate_count = 0;
function _InitDictatePaging(count) {
    if (count == 0) {
        $("#Dictate_Pagination").hide();
        return;
    }
    $("#Dictate_Pagination").show();
    if (_dicate_count != count || dicate_paging.Index == 1) {
        _dicate_count = count;
        $("#Dictate_Pagination").pagination({
            totalData: count,
            showData: dicate_paging.Size,
            current: dicate_paging.Index,
            homePage: '首页',
            endPage: '末页',
            prevContent: '上页',
            nextContent: '下页',
            coping: true,
            callback: function (e) {
                var index = e.getCurrent();
                if (index != dicate_paging.Index) {
                    dicate_paging.Index = index;
                    _InitDicate();
                }
            }
        });
    }
}
function _InitDicate() {
    dicate_paging.Param = $('#DictateQuery').FormData();
    dicate_paging.Param.ServerId = urlTools.GetUrlValue("id");
    SubmitData("ServerDictate/Query", null, dicate_paging, function (data) {
        var html = template('Dictate_Template', data);
        $("#DictateList").html(html);
        _InitDictatePaging(data.count);
    });
}
function _DropDicate(id) {
    if (!window.confirm("确定删除？")) {
        return;
    }
    SubmitData("ServerDictate/Drop", { id: id }, null, function (data) {
        _InitDicate();
    });
}