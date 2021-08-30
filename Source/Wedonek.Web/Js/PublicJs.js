var _BaseUri = "http://127.0.0.1:89/api/";
function _CheckLogin() {
    if (window.location.href.endsWith("Login.html")) {
        return;
    }
    var id = GetAccreditId();
    if (id == null || id == "") {
        window.location = "/Login.html";
    }
}
$(document).ready(function () {
    _CheckLogin();
});
function SubmitData(uri, get, post, success) {
    var param = {
        Url: _BaseUri + uri,
        Type: "GET",
        ContentType: "application/x-www-form-urlencoded"
    };
    if (get == null) {
        get = {};
    }
    get.accreditId = GetAccreditId();
    param.Url = GetUri(param.Url, get);
    if (post != null) {
        param.Type = "POST";
        param.Post = JSON.stringify(post);
        param.ContentType = "application/json";
    }
    Submit(param, success);
}
function Submit(param, success) {
    $.ajax({
        type: param.Type,
        url: param.Url,
        cache: false,
        contentType: param.ContentType,
        dataType: "json",
        data: param.Post,
        withCredentials: true,
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,
        success: function (res) {
            if (res == null) {
                ShowError("错误", "服务器繁忙!");
                return;
            }
            else if (res.errorcode == 0) {
                success(res.data);
            }
            else {
                ShowError("错误", res.errmsg);
            }
        },
        error: function (request, textStatus, error) {
            ShowError("请求错误", "请求失败!");
        }
    });
}

function ShowError(title, text) {
    new PNotify({
        title: title,
        text: text,
        type: 'error',
        styling: 'bootstrap3'
    });
}
function GetAccreditId() {
    return $.cookie('AccreditId');
}
function DropAccreditId() {
    return $.removeCookie('AccreditId', { path: '/' });
}
function SetAccreditId(accreditId) {
    $.cookie('AccreditId', accreditId, { expires: 1, path: '/' });
}
function GetUri(uri, param) {
    var arg = "";
    for (var i in param) {
        if (param[i] != null) {
            arg += "&" + i + "=" + param[i];
        }
    }
    if (arg == "") {
        return uri;
    }
    if (uri.indexOf('?') == -1) {
        return uri + "?" + arg.substring(1, arg.length);
    }
    return uri + arg;
}
$.fn.FormData = function () {
    var ct = this.serializeArray();
    var obj = {};
    $.each(ct, function () {
        if (obj[this.name] !== undefined) {
            if (!obj[this.name].push) {
                obj[this.name] = [obj[this.name]];
            }
            obj[this.name].push(this.value || "");
        } else {
            obj[this.name] = this.value || "";
        }
    });
    return obj;
};

$.fn.InitForm = function (data) {
    for (var i in data) {
        var control = $(this).find("[name=" + i + "]");
        if (control != null) {
            $(control).val(data[i]);
        }
    }
};
$.fn.Join = function (spears) {
    var str = "";
    for (var i = 0; i < this.length; i++) {
        str += spears;
        str += this[i];
    }
    if (str.length == 0) {
        return str;
    }
    return str.substring(1, str.length);
};
var UrlTools = function () {
    this.IsLoad = false;
    this.UrlParams = new Array();
    this.UrlSegments = new Array();
    this.InitUrl = function () {
        if (this.IsLoad) {
            return;
        }
        this.IsLoad = true;
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
        if (this.UrlSegments.length == 0) {
            this.UrlSegments = this.GetUrlSegmentList();
        }
    }
    this.GetNewUrl = function (url) {
        this.InitUrl();
        if (this.UrlParams.length != 0) {
            if (url.indexOf('?') == -1) {
                url += "?";
            } else {
                url += "&";
            }
            for (var i = 0; i < this.UrlParams.length; i++) {
                if (i != 0) {
                    url += "&";
                }
                var temp = this.UrlParams[i];
                url += temp.name + "=" + temp.value;
            }
        }
        return url;
    }
    this.CheckIsLocalUrl = function (url) {
        if (this.IsURL(url)) {
            var mySementList = this.GetUrlSegmentList(url);
            var localSementList = this.GetUrlSegmentList();
            if (localSementList[1] == mySementList[1]) {
                return true;
            }
            return false;
        }
        return true;
    }
    this.GetUrlSegmentList = function (url) {
        url = this.GetUrl(url);
        url = url.replace("://", "/");
        return url.split('/');
    }
    this.IsURL = function (url) {
        var strRegex = /^(http:|https:){1}\/\/(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?.*$/g;
        return strRegex.test(url);
    }
    this.GetUrlSegments = function (num) {
        this.InitUrl();
        if (num == null && this.UrlSegments.length != 0) {
            num = this.UrlSegments.length - 1;
        }
        if (this.UrlSegments.length > num) {
            return this.UrlSegments[num];
        }
        return null;
    }
    this.GetUrlSegmentsLen = function () {
        this.InitUrl();
        return this.UrlSegments.length;
    }
    this.GetUrlAddressType = function () {
        var urlS = this.GetUrlSegments();
        var regex = /^(\w+){1}[_]([?a-zA-Z0-9]{8}-[?a-zA-Z0-9]{4}-[?a-zA-Z0-9]{4}-[?a-zA-Z0-9]{4}-[?a-zA-Z0-9]{12}){1}([?].*)*$/;
        if (regex.test(urlS)) {
            var matchs = urlS.match(regex);
            return matchs[1];
        }
        regex = /^(\w+){1}([?].*)*$/;
        if (regex.test(urlS)) {
            var matchs = urlS.match(regex);
            return matchs[1];
        }
        regex = /^(\w+){1}[.]\w+([?].*)*$/;
        if (regex.test(urlS)) {
            var matchs = urlS.match(regex);
            return matchs[1];
        }
        return null;
    }
    this.JumpUrl = function () {
        this.InitUrl();
        var url = this.GetUrl(null);
        if (this.UrlParams.length != 0) {
            url += "?";
            for (var i = 0; i < this.UrlParams.length; i++) {
                if (i != 0) {
                    url += "&";
                }
                var temp = this.UrlParams[i];
                url += temp.name + "=" + temp.value;
            }
        }
        window.location.href = url;
    }
    this.GetUrl = function (url) {
        if (url == null) {
            url = window.location.href;
        }
        if (url.indexOf("?") > -1) {
            return url.substring(0, url.indexOf("?"))
        } else {
            return url
        }
    }
    this.GetUrlHead = function () {
        this.InitUrl();
        return this.UrlSegments[0] + "://" + this.UrlSegments[1];
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
    this.UpdateUrlValue = function (name, newValue) {
        this.InitUrl();
        for (var i = 0; i < this.UrlParams.length; i++) {
            var temp = this.UrlParams[i];
            if (temp.name.toLowerCase() == name.toLowerCase()) {
                this.UrlParams[i].value = newValue;
                return;
            }
        }
        var newData = new Object();
        newData.name = name;
        newData.value = newValue;
        this.UrlParams.push(newData);
    }
    this.RemoveUrlParam = function (name) {
        this.InitUrl();
        var index = -1;
        for (var i = 0; i < this.UrlParams.length; i++) {
            var temp = this.UrlParams[i];
            if (temp.name.toLowerCase() == name.toLowerCase()) {
                index = i;
                break;
            }
        }
        if (index != -1) {
            this.UrlParams.splice(index, 1);
        }
    }
}
var urlTools = new UrlTools();

