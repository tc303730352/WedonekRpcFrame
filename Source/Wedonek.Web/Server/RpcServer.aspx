<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RpcServer.aspx.cs" Inherits="Wedonek.Web.Server.RpcServer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/vendors/jsonview/jquery.jsonview.min.css" rel="stylesheet" />
    <link href="/vendors/pagination/pagination.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="page-title">
        <div class="title_left">
            <h3>服务节点信息</h3>
        </div>
    </div>
    <div class="clearfix"></div>
    <div class="row">
        <div class="col-sm-12">
            <ul class="nav nav-tabs bar_tabs" id="myTab" role="tablist">
                <li class="nav-item">
                    <a class="nav-link active" id="home-tab" data-toggle="tab" href="#home" role="tab" aria-controls="home" aria-selected="true">节点资料</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="edit-tab" data-toggle="tab" href="#edit" role="tab" aria-controls="edit" aria-selected="true">编辑节点</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="profile-tab" data-toggle="tab" href="#profile" role="tab" aria-controls="profile" aria-selected="false">节点配置</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="profile-tab" data-toggle="tab" href="#LimitConfig" role="tab" aria-controls="profile" aria-selected="false">节点限流</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="profile-tab" data-toggle="tab" href="#DictateConfig" role="tab" aria-controls="profile" aria-selected="false">接口限流</a>
                </li>
            </ul>
            <div class="tab-content" id="myTabContent">
                <div class="tab-pane fade active show" id="home" role="tabpanel" aria-labelledby="home-tab"></div>
                <div class="tab-pane fade" id="edit" role="tabpanel" aria-labelledby="edit-tab">
                    <form id="RpcServerEdit">
                        <div class="form-horizontal form-label-left">
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                                    系统名<span class="required">*</span>
                                </label>
                                <div class="col-md-6 col-sm-6">
                                    <input type="text" name="ServerName" required="required" class="form-control " placeholder="节点名">
                                </div>
                            </div>
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                                    默认链接Ip<span class="required">*</span>
                                </label>
                                <div class="col-md-6 col-sm-6">
                                    <input type="text" name="ServerIp" required="required" class="form-control " placeholder="内网Ip">
                                </div>
                            </div>
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                                    外网链接Ip<span class="required">*</span>
                                </label>
                                <div class="col-md-6 col-sm-6">
                                    <input type="text" name="RemoteIp" required="required" class="form-control " placeholder="外网Ip">
                                </div>
                            </div>
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                                    节点端口号<span class="required">*</span>
                                </label>
                                <div class="col-md-6 col-sm-6">
                                    <input type="text" name="ServerPort" required="required" class="form-control " placeholder="端口号">
                                </div>
                            </div>
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                                    所属类别<span class="required">*</span>
                                </label>
                                <div class="col-md-6 col-sm-6">
                                    <select class="form-control" id="ServerGroup" name="GroupId"></select>
                                    <select class="form-control" id="ServerType" name="SystemType"></select>
                                </div>
                            </div>
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                                    所在区域<span class="required">*</span>
                                </label>
                                <div class="col-md-6 col-sm-6">
                                    <select class="form-control" id="RegionId" name="RegionId"></select>
                                </div>
                            </div>
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                                    配置优先级
                                </label>
                                <div class="col-md-6 col-sm-6">
                                    <select class="form-control" name="ConfigPrower">
                                        <option value="15" selected="selected">远程优先</option>
                                        <option value="1">本地优先</option>
                                    </select>
                                </div>
                            </div>
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                                    节点MAC<span class="required">*</span>
                                </label>
                                <div class="col-md-6 col-sm-6">
                                    <input type="text" maxlength="17" name="ServerMac" required="required" class="form-control " placeholder="MAC地址">
                                </div>
                            </div>
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                                    节点编号
                                </label>
                                <div class="col-md-6 col-sm-6">
                                    <input type="text" name="ServerIndex" required="required" class="form-control " placeholder="节点编号">
                                </div>
                            </div>
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                                    链接公钥<span class="required">*</span>
                                </label>
                                <div class="col-md-6 col-sm-6">
                                    <input type="text" name="PublicKey" required="required" class="form-control " placeholder="链接公钥">
                                </div>
                            </div>
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                                    权重值
                                </label>
                                <div class="col-md-6 col-sm-6">
                                    <input type="text" name="Weight" required="required" class="form-control " placeholder="Weight" value="1" />
                                </div>
                            </div>
                            <div class="item form-group">
                                <div class="col-sm-12" id="TransmitDiv">
                                </div>
                            </div>
                            <div class="item form-group">
                                <div class="col-md-6 col-sm-6 offset-md-6">
                                    <button type="button" class="btn btn-success" onclick="_Save();">提交</button>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
                <div class="tab-pane fade" id="profile" role="tabpanel" aria-labelledby="profile-tab">
                    <div class="row">
                        <div class="col-sm-12">
                            <form id="QueryForm">
                                <div class="form-inline">
                                    <div class="form-group">
                                        <label for="ex4" class="col-form-label">&nbsp;&nbsp;配置名：</label>
                                        <input type="text" class="form-control text-left" placeholder="配置名" name="ConfigName" />
                                    </div>
                                    <button type="button" id="QueryBtn" class="btn btn-secondary">查询</button>
                                    <a class="btn btn-success" href="AddServerConfig.aspx?id=<%=this.Request.QueryString["Id"] %>">添加配置</a>
                                </div>
                            </form>
                        </div>
                    </div>
                    <div class="table-responsive">
                        <table class="table table-striped jambo_table bulk_action">
                            <thead>
                                <tr class="headings">
                                    <th>#</th>
                                    <th class="column-title">配置名</th>
                                    <th class="column-title">值类型 </th>
                                    <th class="column-title">配置</th>
                                    <th class="column-title">操作</th>
                                </tr>
                            </thead>
                            <tbody id="ConfigList">
                            </tbody>
                        </table>
                        <div id="Pagination" class="m-style pull-right">
                        </div>
                    </div>
                </div>
                <div class="tab-pane fade" id="LimitConfig" role="tabpanel" aria-labelledby="edit-tab">
                    <form id="ServerLimit">
                        <div class="form-horizontal form-label-left">
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                                    是否启用：
                                </label>
                                <div class="col-md-6 col-sm-6 ">
                                    <input type="checkbox" class="flat" name="IsEnable" id="IsEnable" />
                                </div>
                            </div>
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                                    限流方式：
                                </label>
                                <div class="col-md-6 col-sm-6 ">
                                    <select class="form-control" name="LimitType" id="LimitType">
                                        <option value="0">不启用</option>
                                        <option value="1">固定时间窗</option>
                                        <option value="2">流动时间窗</option>
                                        <option value="3">令牌桶</option>
                                    </select>
                                </div>
                            </div>
                            <div class="item form-group TimeLimit">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                                    最大请求量：
                                </label>
                                <div class="col-md-6 col-sm-6 ">
                                    <input class="form-control" type="number" min="1" name="LimitNum" />
                                </div>
                            </div>
                            <div class="item form-group TimeLimit">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                                    限定窗口时间(秒)：
                                </label>
                                <div class="col-md-6 col-sm-6 ">
                                    <input class="form-control" type="number" min="1" name="LimitTime" />
                                </div>
                            </div>
                            <div class="item form-group TokenLimit">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                                    最大令牌数：
                                </label>
                                <div class="col-md-6 col-sm-6 ">
                                    <input class="form-control" type="number" min="1" name="TokenNum" />
                                </div>
                            </div>
                            <div class="item form-group TokenLimit">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                                    每秒新增令牌：
                                </label>
                                <div class="col-md-6 col-sm-6 ">
                                    <input class="form-control" type="number" min="1" name="TokenInNum" />
                                </div>
                            </div>
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                                    启用漏桶：
                                </label>
                                <div class="col-md-6 col-sm-6 ">
                                    <input type="checkbox" class="flat" name="IsEnableBucket" id="IsEnableBucket" />
                                </div>
                            </div>
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                                    漏桶大小：
                                </label>
                                <div class="col-md-6 col-sm-6 ">
                                    <input class="form-control" type="number" min="1" name="BucketSize" />
                                </div>
                            </div>
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                                    漏桶出量：
                                </label>
                                <div class="col-md-6 col-sm-6 ">
                                    <input class="form-control" type="number" min="1" name="BucketOutNum" />
                                </div>
                            </div>
                            <div class="item form-group">
                                <div class="col-md-6 col-sm-6 offset-md-6">
                                    <button type="button" class="btn btn-success" onclick="_SaveLimit();">提交</button>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
                <div class="tab-pane fade" id="DictateConfig" role="tabpanel" aria-labelledby="edit-tab">
                    <div class="row">
                        <div class="col-sm-12">
                            <form id="DictateQuery">
                                <div class="form-inline">
                                    <div class="form-group">
                                        <label for="ex4" class="col-form-label">远程指令：</label>
                                        <input type="text" class="form-control text-left" placeholder="方法名" name="Dictate" />
                                    </div>
                                    <button type="button" id="DictateQueryBtn" class="btn btn-secondary">查询</button>
                                    <a class="btn btn-success" href="AddDictateLimit.aspx?id=<%=this.Request.QueryString["Id"] %>">添加配置</a>
                                </div>
                            </form>
                        </div>
                    </div>
                    <div class="table-responsive">
                        <table class="table table-striped jambo_table bulk_action">
                            <thead>
                                <tr class="headings">
                                    <th>#</th>
                                    <th class="column-title">指令名</th>
                                    <th class="column-title">限制类型 </th>
                                    <th class="column-title">最大请求量</th>
                                    <th class="column-title">限定窗口时间</th>
                                    <th class="column-title">桶大小</th>
                                    <th class="column-title">出桶量</th>
                                    <th class="column-title">最大令牌数</th>
                                    <th class="column-title">每秒新增令牌数</th>
                                    <th class="column-title">操作</th>
                                </tr>
                            </thead>
                            <tbody id="DictateList">
                            </tbody>
                        </table>
                        <div id="Dictate_Pagination" class="m-style pull-right">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="show_Json" tabindex="-1" role="dialog" style="display: none;" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel">配置内容</h4>
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">×</span>
                    </button>
                </div>
                <div class="modal-body">
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="Set_Config" tabindex="-1" role="dialog" style="display: none;" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">设置配置</h4>
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">×</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal form-label-left">
                        <div class="item form-group">
                            <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                                值类型
                            </label>
                            <div class="col-md-6 col-sm-6 ">
                                <select class="form-control" id="ValueType">
                                    <option value="0">字符串</option>
                                    <option value="1">JSON格式</option>
                                </select>
                            </div>
                        </div>
                        <div class="item form-group">
                            <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                                配置值<span class="required">*</span>
                            </label>
                            <div class="col-md-9 col-sm-9 ">
                                <textarea class="resizable_textarea form-control" id="ConfigValue" placeholder=""></textarea>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" onclick="_SaveConfig()">保存</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Js" runat="server">
    <script type="text/javascript" src="/vendors/jsoneditor/jsoneditor.js"></script>
    <script type="text/javascript" src="/vendors/jsonview/jquery.jsonview.min.js"></script>
    <script type="text/javascript" src="/vendors/pagination/jquery.pagination.js"></script>
    <script type="text/javascript" src="/Js/RpcServerInfo.js"></script>
    <script type="text/javascript" src="/Js/LimitConfig.js"></script>
    <script type="text/javascript" src="/Js/DicateLimit.js"></script>
    <script id="DatumTemplate" type="text/html">
        <div class="form-horizontal form-label-left">
            <div class="item form-group">
                <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                    服务名：
                </label>
                <div class="col-md-6 col-sm-6 line33">{{ServerName}}</div>
            </div>
            <div class="item form-group">
                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                    节点内网链接Ip：
                </label>
                <div class="col-md-6 col-sm-6 line33">{{ServerIp}}</div>
            </div>
            <div class="item form-group">
                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                    节点远程链接Ip：
                </label>
                <div class="col-md-6 col-sm-6 line33">{{RemoteIp}}</div>
            </div>
            <div class="item form-group">
                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                    节点端口号：
                </label>
                <div class="col-md-6 col-sm-6 line33">{{ServerPort}}</div>
            </div>
            <div class="item form-group">
                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                    节点类别：
                </label>
                <div class="col-md-6 col-sm-6 line33">{{GroupName}}  -  {{SystemName}}</div>
            </div>
            <div class="item form-group">
                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                    MAC：
                </label>
                <div class="col-md-6 col-sm-6 line33">{{ServerMac}}</div>
            </div>
            <div class="item form-group">
                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                    节点编号：
                </label>
                <div class="col-md-6 col-sm-6 line33">{{ServerIndex}}</div>
            </div>
            <div class="item form-group">
                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                    公钥：
                </label>
                <div class="col-md-6 col-sm-6 line33">{{PublicKey}}</div>
            </div>
            <div class="item form-group">
                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                    负载均衡方式：
                </label>
                <div class="col-md-6 col-sm-6 line33">{{BalancedType}}</div>
            </div>
            <div class="item form-group">
                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                    负载权重：
                </label>
                <div class="col-md-6 col-sm-6 line33">{{Weight}}</div>
            </div>
            <div class="item form-group">
                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                    是否在线：
                </label>
                <div class="col-md-6 col-sm-6 line33">{{IsOnline?"在线":"离线"}}</div>
            </div>
            <div class="item form-group">
                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                    所链接的中控编号：
                </label>
                <div class="col-md-6 col-sm-6 line33">{{BindIndex}}</div>
            </div>
            <div class="item form-group">
                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                    链接IP：
                </label>
                <div class="col-md-6 col-sm-6 line33">{{ConIp}}</div>
            </div>
            <div class="item form-group">
                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                    最后离线日期：
                </label>
                <div class="col-md-6 col-sm-6 line33">{{FormDate(LastOffliceDate)}}</div>
            </div>
            <div class="item form-group">
                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                    添加日期：
                </label>
                <div class="col-md-6 col-sm-6 line33">{{FormDate(AddTime)}}</div>
            </div>
            <div class="col-sm-12">
                <h4>节点负载配置</h4>
                <div class="table-responsive">
                    <table class="table table-striped jambo_table bulk_action">
                        <thead>
                            <tr class="headings">
                                <th>#</th>
                                <th class="column-title">负载方式</th>
                                <th class="column-title">固定值</th>
                                <th class="column-title">负载Id </th>
                                <th class="column-title">范围值</th>
                            </tr>
                        </thead>
                        <tbody>
                            {{each TransmitConfig as a}}
                               <tr>
                                   <th scope="row">{{$index}}</th>
                                   <td>{{FormatTransmitType(a.TransmitType)}}</td>
                                   <td>{{a.Value}}</td>
                                   <td>{{a.TransmitId}}</td>
                                   <td>{{each a.Range as b}}
                                         <p>{{FormatRange(b)}}</p>
                                       {{/each}}</td>
                               </tr>
                            {{/each}}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </script>
    <script id="ConfigsTemplate" type="text/html">
        {{each list as a}}
        <tr>
            <th scope="row">{{$index}}</th>
            <td>{{a.Name}}</td>
            <td>{{GetValueType(a.ValueType)}}</td>
            {{if a.ValueType ==0}}
            <td>{{a.Value}}</td>
            {{else}}
                <td>
                    <button type="button" class="btn btn-sm btn-primary" onclick="_showJson({{a.Id}})">点击查看</button>
                    <div id="Json_{{a.Id}}" style="display: none;">{{a.Value}}</div>
                </td>
            {{/if}}
            <td>
                <a class="btn btn-sm btn-info" href="javascript:_setConfig('{{a.Id}}')">设置</a>
                <a class="btn btn-sm btn-danger" href="javascript:_dropConfig('{{a.Id}}')">删除</a>
            </td>
        </tr>
        {{/each}}
    </script>

    <script id="Dictate_Template" type="text/html">
        {{each list as a}}
        <tr>
            <th scope="row">{{$index}}</th>
            <td>{{a.Dictate}}</td>
            <td>{{GetLimitType(a.LimitType)}}</td>
            <td>{{a.LimitNum}}</td>
            <td>{{a.LimitTime}}</td>
            <td>{{a.BucketSize}}</td>
            <td>{{a.BucketOutNum}}</td>
            <td>{{a.TokenNum}}</td>
            <td>{{a.TokenInNum}}</td>
            <td>
                <a href="EditDictateLimit.aspx?id={{a.Id}}&serverId=<%=this.Request.QueryString["id"] %>" class="btn btn-sm btn-info">编辑</a>
                <a href="javascript:_DropDicate({{a.Id}})" class="btn btn-sm btn-danger">删除</a>
            </td>
        </tr>
        {{/each}}
    </script>
</asp:Content>
