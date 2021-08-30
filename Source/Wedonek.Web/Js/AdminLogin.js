
$(document).ready(function () {
    DropAccreditId();
    $("#AdminLogin").on("click", _Login);
});
function _Login() {
    var data = $('#LoginForm').FormData();
    SubmitData("Admin/Login", null, data, function (accreditId) {
        SetAccreditId(accreditId);
        window.location = "/Server/ServerIndex.aspx";
    });
}