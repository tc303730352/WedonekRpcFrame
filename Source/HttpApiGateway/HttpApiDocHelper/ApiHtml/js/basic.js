function SubmitData(uri, type, data, callback) {
    uri = "/api/helper" + uri;
    $.ajax({
        type: type,
        url: uri,
        data: data,
        cache: false,
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,
        success: function (res, isSuccess, doc) {
            if ("object" == typeof (res)) {
                if (isSuccess) {
                    res = doc.responseText;
                }
            }
            if (res == null || res == "") {
                alert("错误!");
            }
            else {
                eval("res=" + res);
                if (res.errorcode == 0) {
                    callback(res.data);
                }
                else {
                    alert(res.errmsg);
                }
            }
        }
    });
}
function SubmitGetData(uri, callback) {
    this.SubmitData(uri, "GET", null, callback);
}
function SubmitPostData(uri, data, callback) {
    this.SubmitData(uri, "POST", data, callback);
}
var UrlTools = function () {
    this.UrlParams = new Array();
    this.InitUrl = function () {
        if (this.UrlParams.length == 0) {
            var search = window.location.search;
            if (search != null && search.length != 0) {
                var paraString = search.substring(1, search.length).split("&");
                for (var i = 0; i < paraString.length; i++) {
                    var temp = new Object();
                    temp.name = paraString[i].substring(0, paraString[i].indexOf("="));
                    temp.value = paraString[i].substring(paraString[i].indexOf("=") + 1, paraString[i].length);
                    if (temp.name != null && temp.value != null) {
                        this.UrlParams.push(temp);
                    }
                }
            }
        }
    }
    this.GetUrlValue = function (name) {
        this.InitUrl();
        for (var i = 0; i < this.UrlParams.length; i++) {
            var temp = this.UrlParams[i];
            if (temp.name.toLowerCase() == name.toLowerCase()) {
                return temp.value;
            }
        }
    }
}
var urlTools = new UrlTools();