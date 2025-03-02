var blockUp = function (param) {
    this.File = param.file;
    this.AccreditId = param.accreditId;
    this.FormData = param.formData;
    this.UpUri = param.upUri;
    this.UpTaskId;
    this.UpError = param.upError;
    this.FileDatum;
    this.UpProgress = param.upProgress;
    this.BlockConfig;
    var _basic = new basicTools();
    this.UpFile = function () {
        if (this.UpError == null) {
            this.UpError = _upError;
        }
        var that = this;
        this.FileDatum = {
            FileName: this.File.name,
            FileSize: this.File.size
        };
        _basic.CalculateMd5(this.File, function (md5) {
            that.FileDatum.FileMd5 = md5;
            _syncFile(that);
        }, function (error) {
            _upError(-1, error);
        });

    }
    function _upError(code, error) {
        alert(error);
    }
    function _syncFile(that) {
        var uri = _basic.GetUrl(that.UpUri, {
            accreditId: that.AccreditId
        });
        _basic.PostJson(uri, {
            FileName: that.FileDatum.FileName,
            FileSize: that.FileDatum.FileSize,
            FileMd5: that.FileDatum.FileMd5,
            Form: that.FormData
        }, function (taskId) {
            that.UpTaskId = taskId;
            _getUpState(that);
        }, function (code, error) {
            that.UpError(code, error);
        });
    }
    function _getUpState(that) {
        var uri = _basic.GetUrl("BlockUp/GetState", {
            accreditId: that.AccreditId
        });
        _basic.Get(uri, {
            taskId: that.UpTaskId
        }, function (state) {
            _syncUpState(that, state);
        }, function (code, error) {
            that.UpError(code, error);
        });
    }
    function _syncUpState(that, state) {
        if (state.UpState == 0) {//准备中
            that.UpProgress({
                state: 0,
                e: that
            });
            window.setTimeout(function () {
                _getUpState(that);
            }, 200);
        } else if (state.UpState == 1) {//开始上传
            that.BlockConfig = {
                alreadyUp: state.AlreadyUp,
                filesize: state.FileSize,
                tasks: _basic.Map(state.NoUpIndex, function (i) {
                    return {
                        index: i,
                        isEnd: false,
                        retry: 0
                    }
                })
            };
            _progress(that, that.BlockConfig);
            if (val == 100) {
                _getUpState(that);
                return;
            }
            _upFile(that, 0, that.BlockConfig);
        } else if (state.UpState == 3) {//上传完成
            that.UpProgress({
                state: 3,
                e: that,
                result: state.Result
            });
        } else if (state.UpState == 2) {//上传完成校验中
            that.UpProgress({
                state: 3,
                e: that
            });
            window.setTimeout(function () {
                _getUpState(that);
            }, 200);
        }
    }
    function _progress(that, config) {
        var val = Math.round((config.alreadyUp / config.filesize) * 10000) / 100;
        that.UpProgress({
            state: 1,
            e: that,
            Progress: {
                alreadyUp: config.alreadyUp,
                filesize: config.filesize,
                progress: val
            }
        });
    }
    function _upFile(that, i, config) {
        if (i >= config.tasks.length) {
            _getUpState(that);
            return;
        }
        var task = config.tasks[i];
        if (task.isEnd) {
            _upFile(that, i + 1, config);
            return;
        }
        _basic.UpFile("file/block/up", that.File, task.index, config.BlockSize, function (size) {
            task.isEnd = true;
            config.alreadyUp += size;
            _progress(that, config);
            _upFile(that, i + 1, config);
        }, function (errorcode, errmsg) {
            if (task.retry > 3) {
                that.UpError(errorcode, errmsg);
                return;
            }
            task.retry = task.retry + 1;
            _upFile(that, i, config);
        });
    }

}
var basicTools = function () {
    var _basicUri = "http://127.0.0.1:86/api/";
    var chunkSize = 5 * 1024 * 1024;
    var _blobSlice = File.prototype.slice || File.prototype.mozSlice || File.prototype.webkitSlice;
    this.CalculateMd5 = function (file, complate, error) {
        var chunks = Math.ceil(file.size / chunkSize);
        var cur = 0;
        var read = new FileReader();
        var buffer = new SparkMD5.ArrayBuffer();
        read.onload = function (e) {
            buffer.append(e.target.result);
            cur++;
            if (cur < chunks) {
                _readFile(read, file, cur);
            } else {
                var md5 = spark.end();
                complate(md5);
            }
        };
        read.onerror = function () {
            error("文件MD5计算错误!");
        };
        _readFile(read, file, 0);
    }
    this.Map = function (array, action) {
        var list = [];
        for (var i = 0; i < array.length; i++) {
            list.push(action(array[i]));
        }
        return list;
    }
    this.Get = function (uri, data, success, error) {
        var param = {
            Url: _basicUri + uri,
            Type: "GET",
            ContentType: "application/x-www-form-urlencoded"
        };
        param.Url = _getUri(param.Url, data);
        submitApi(param, success, error);
    }

    this.PostJson = function (uri, data, success, error) {
        var param = {
            Url: _basicUri + uri,
            Type: "POST",
            ContentType: "application/json",
            Post = JSON.stringify(data)
        };
        submitApi(param, success, error);
    }
    this.GetUrl = function (uri, param) {
        return _getUri(uri, param);
    }

    this.UpFile = function (uri, file, index, size, success, error) {
        _upFile({
            uri: uri,
            file: file,
            index: index,
            size: size,
            success: success,
            error: error
        });
    }
    function _getUri(uri, param) {
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
    function _readFile(read, file, index) {
        var skip = index * chunkSize;
        var end = skip + chunkSize;
        if (end > file.size) {
            end = file.size;
        }
        read.readAsArrayBuffer(_blobSlice.call(file, skip, end));
    }
    function _upFile(param) {
        var file = param.file;
        var start = param.index * param.size;
        var end = start + param.size;
        if (end > file.size) {
            end = file.size;
        }
        var chunk = file.slice(start, end);
        var formData = new FormData();
        formData.append("file", chunk, file.name);
        $.ajax({
            url: uri,
            type: 'POST',
            cache: false,
            data: formData,
            processData: false,
            contentType: false,
            withCredentials: true,
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            dataType: "json"
        }).done(function (res) {
            if (res.errorcode == 0) {
                param.success();
            } else {
                param.error(res.errorcode, res.errmsg);
            }
        }).fail(function (res) {
            param.error(-2, "请求错误!");
        });
    }
    function submitApi(param, success, error) {
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
                    error(-1, "请求错误!");
                    return;
                }
                else if (res.errorcode == 0) {
                    success(res.data);
                }
                else {
                    error(res.errorcode, res.errmsg);
                }
            },
            error: function (request, textStatus, error) {
                ShowError("请求错误", "请求失败!");
            }
        });
    }
}