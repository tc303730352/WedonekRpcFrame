$(document).ready(function () {
    var id = urlTools.GetUrlValue("id");
    _LoadApiInfo(id);
});
function _LoadApiInfo(id) {
    SubmitGetData("/GetClass?id=" + id, function (data) {
        var html = template('ApiInfo', data);
        $("#ApiData").html(html);
    });
}