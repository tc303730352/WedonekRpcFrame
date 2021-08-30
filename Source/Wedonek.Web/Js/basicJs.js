function LoadSysGroup(selects,isAll=true) {
    SubmitData("ServerGroup/GetGroups", null, null, function (list) {
        var html = "";
        if (isAll) {
            html="<option value=\"0\">所有</option>";
        }
        for (var i = 0; i < list.length; i++) {
            html += "<option value=\"" + list[i].Id + "\">" + list[i].GroupName + "</option>";
        }
        $(selects).html(html);
    });
}
function LoadSysType(groupId, selects, arg) {
    if (groupId == 0) {
        if (arg.IsAll) {
            $(selects).html("<option value=\"0\">所有</option>");
        }
        else {
            $(selects).hide();
        }
        return;
    }
    SubmitData("ServerType/Gets", { groupid: groupId }, null, function (list) {
        $(selects).show();
        var html = "";
        if (arg.IsAll) {
            html = "<option value=\"0\">所有</option>";
        }
        for (var i = 0; i < list.length; i++) {
            if (list[i].Id == arg.SystemType) {
                html += "<option value=\"" + list[i].Id + "\" selected=\"selected\">" + list[i].SystemName + "</option>";
            } else {
                html += "<option value=\"" + list[i].Id + "\">" + list[i].SystemName + "</option>";
            }
        }
        $(selects).html(html);
    });
}
function LoadServerRegion(selects, isAll = true) {
    SubmitData("ServerRegion/Gets", null, null, function (list) {
        var html = "";
        if (isAll) {
            html = "<option value=\"0\">所有</option>";
        }
        for (var i = 0; i < list.length; i++) {
            html += "<option value=\"" + list[i].Id + "\">" + list[i].RegionName + "</option>";
        }
        $(selects).html(html);
    });
}
Date.prototype.Format = function (formatStr) {
    var str = formatStr;
    var Week = ['日', '一', '二', '三', '四', '五', '六'];

    str = str.replace(/yyyy|YYYY/, this.getFullYear());
    str = str.replace(/yy|YY/, (this.getYear() % 100) > 9 ? (this.getYear() % 100).toString() : '0' + (this.getYear() % 100));

    str = str.replace(/MM/, this.getMonth() >= 9 ? (this.getMonth() + 1).toString() : '0' + (this.getMonth() + 1));
    str = str.replace(/M/g, (this.getMonth() + 1));

    str = str.replace(/w|W/g, Week[this.getDay()]);

    str = str.replace(/dd|DD/, this.getDate() > 9 ? this.getDate().toString() : '0' + this.getDate());
    str = str.replace(/d|D/g, this.getDate());

    str = str.replace(/hh|HH/, this.getHours() > 9 ? this.getHours().toString() : '0' + this.getHours());
    str = str.replace(/h|H/g, this.getHours());
    str = str.replace(/mm/, this.getMinutes() > 9 ? this.getMinutes().toString() : '0' + this.getMinutes());
    str = str.replace(/m/g, this.getMinutes());

    str = str.replace(/ss|SS/, this.getSeconds() > 9 ? this.getSeconds().toString() : '0' + this.getSeconds());
    str = str.replace(/s|S/g, this.getSeconds());

    return str;
}
function LoadControl(group, sysType, arg) {
    if (arg == null) {
        arg = {
            IsAll: true,
            IsGroup: true,
            GroupId: 0,
            SystemType: 0
        };
    }
    SubmitData("ServerGroup/GetGroups", null, null, function (list) {
        var html = "";
        if (arg.IsGroup) {
            html = "<option value=\"0\">所有</option>";
        }
        for (var i = 0; i < list.length; i++) {
            if (list[i].Id == arg.GroupId) {
                html += "<option value=\"" + list[i].Id + "\" selected=\"selected\">" + list[i].GroupName + "</option>";
            } else {
                html += "<option value=\"" + list[i].Id + "\">" + list[i].GroupName + "</option>";
            }
        }
        $(group).html(html);
        var val = $(group).val();
        LoadSysType(val, sysType, arg);
        $(group).on("change", function (e) {
            val = $(this).val();
            LoadSysType(val, sysType, arg);
        });
    });
}