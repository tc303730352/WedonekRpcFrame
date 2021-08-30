<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RpcMerInfo.aspx.cs" Inherits="Wedonek.Web.RpcMer.RpcMerInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/vendors/jsonview/jquery.jsonview.min.css" rel="stylesheet" />
    <link href="/vendors/pagination/pagination.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="page-title">
        <div class="title_left">
            <h3>集群信息</h3>
        </div>
    </div>
    <div class="clearfix"></div>
    <div class="row">
        <div class="col-sm-12">
            <ul class="nav nav-tabs bar_tabs" id="myTab" role="tablist">
                <li class="nav-item">
                    <a class="nav-link active" id="home-tab" data-toggle="tab" href="#home" role="tab" aria-controls="home" aria-selected="true">编辑资料</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="profile-tab" data-toggle="tab" href="#profile" role="tab" aria-controls="profile" aria-selected="false">集群节点</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="contact-tab" data-toggle="tab" href="#contact" role="tab" aria-controls="contact" aria-selected="false">集群配置</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="task-tab" data-toggle="tab" href="#task" role="tab" aria-controls="task" aria-selected="false">任务配置</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="MerConfig-tab" data-toggle="tab" href="#MerConfig" role="tab" aria-controls="MerConfig" aria-selected="false">跨机房配置</a>
                </li>
            </ul>
            <div class="tab-content" id="myTabContent">
                <div class="tab-pane fade active show" id="home" role="tabpanel" aria-labelledby="home-tab">
                    <form id="RpcMerEdit">
                        <div class="form-horizontal form-label-left">
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                                    系统名<span class="required">*</span>
                                </label>
                                <div class="col-md-6 col-sm-6 ">
                                    <input type="text" name="SystemName" required="required" class="form-control " placeholder="系统名称">
                                </div>
                            </div>
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                                    应用AppId<span class="required">*</span>
                                </label>
                                <div class="col-md-6 col-sm-6 ">
                                    <input type="text" name="AppId" readonly="readonly" value="" class="form-control">
                                </div>
                            </div>
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                                    应用秘钥<span class="required">*</span>
                                </label>
                                <div class="col-md-6 col-sm-6 ">
                                    <input type="text" name="AppSecret" required="required" class="form-control">
                                </div>
                            </div>
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                                    允许链接的Ip
                                </label>
                                <div class="col-md-6 col-sm-6 ">
                                    <textarea class="resizable_textarea form-control" name="AllowServerIp" placeholder="允许链接的IP地址多个逗号分隔！"></textarea>
                                    <p>允许链接的IP地址多个逗号分隔！*代表允许全部</p>
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
                    <div class="col-sm-12 text-right">
                        <a class="btn btn-success" href="BindServer.aspx?id=<%=this.Request.QueryString["Id"] %>">编辑节点</a>
                    </div>
                    <div class="table-responsive">
                        <table class="table table-striped jambo_table bulk_action">
                            <thead>
                                <tr class="headings">
                                    <th>#</th>
                                    <th class="column-title">节点名称</th>
                                    <th class="column-title">节点类别</th>
                                    <th class="column-title">当前是否在线 </th>
                                    <th class="column-title">权重</th>
                                    <th class="column-title">操作</th>
                                </tr>
                            </thead>
                            <tbody id="ServerList">
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="tab-pane fade" id="contact" role="tabpanel" aria-labelledby="contact-tab">
                    <div class="row">
                        <div class="col-sm-12">
                            <form id="QueryForm">
                                <div class="form-inline">
                                    <div class="form-group">
                                        <label for="ex3" class="col-form-label">服务组：</label>
                                        <select class="form-control" id="ServerGroup"></select>
                                    </div>
                                    <div class="form-group">
                                        <label for="ex4" class="col-form-label">&nbsp;&nbsp;节点类型：</label>
                                        <select class="form-control" id="ServerType" name="SystemTypeId"></select>
                                    </div>
                                    <div class="form-group">
                                        <label for="ex4" class="col-form-label">&nbsp;&nbsp;有效范围：</label>
                                        <select class="form-control" name="Range">
                                            <option value="">全部</option>
                                            <option value="0">全局</option>
                                            <option value="1">节点类型</option>
                                        </select>
                                    </div>
                                    <div class="form-group">
                                        <label for="ex4" class="col-form-label">&nbsp;&nbsp;配置名：</label>
                                        <input type="text" class="form-control text-left" placeholder="配置名" name="ConfigName" />
                                    </div>
                                    <button type="button" id="QueryBtn" class="btn btn-secondary">查询</button>
                                    <a class="btn btn-success" href="AddConfig.aspx?id=<%=this.Request.QueryString["Id"] %>">添加配置</a>
                                </div>
                            </form>
                        </div>
                    </div>
                    <div class="table-responsive">
                        <table class="table table-striped jambo_table bulk_action">
                            <thead>
                                <tr class="headings">
                                    <th>#</th>
                                    <th class="column-title">有效范围</th>
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
                <div class="tab-pane fade" id="task" role="tabpanel" aria-labelledby="contact-tab">
                    <div class="row">
                        <div class="col-sm-12">
                            <form id="TaskQueryForm">
                                <div class="form-inline">
                                    <div class="form-group">
                                        <label for="ex4" class="col-form-label">&nbsp;&nbsp;任务类型：</label>
                                        <select class="form-control" name="Range">
                                            <option value="">全部</option>
                                            <option value="0">定时任务</option>
                                            <option value="1">间隔任务</option>
                                            <option value="2">定时间隔任务</option>
                                        </select>
                                    </div>
                                    <div class="form-group">
                                        <label for="ex4" class="col-form-label">&nbsp;&nbsp;任务名：</label>
                                        <input type="text" class="form-control text-left" placeholder="任务名" name="TaskName" />
                                    </div>
                                    <button type="button" id="TaskQueryBtn" class="btn btn-secondary">查询</button>
                                    <a class="btn btn-success" href="AddAutoTask.aspx?id=<%=this.Request.QueryString["Id"] %>">添加任务</a>
                                </div>
                            </form>
                        </div>
                    </div>
                    <div class="table-responsive">
                        <table class="table table-striped jambo_table bulk_action">
                            <thead>
                                <tr class="headings">
                                    <th>#</th>
                                    <th class="column-title">名称</th>
                                    <th class="column-title">类型</th>
                                    <th class="column-title">间隔 </th>
                                    <th class="column-title">优先级</th>
                                    <th class="column-title">发送类型 </th>
                                    <th class="column-title">版本号</th>
                                    <th class="column-title">操作</th>
                                </tr>
                            </thead>
                            <tbody id="TaskList">
                            </tbody>
                        </table>
                        <div id="Task_Pagination" class="m-style pull-right">
                        </div>
                    </div>
                </div>

                <div class="tab-pane fade" id="MerConfig" role="tabpanel" aria-labelledby="contact-tab">
                    <div class="row">
                        <div class="text-right col-sm-12">
                            <a class="btn btn-success" href="AddMerConfig.aspx?id=<%=this.Request.QueryString["Id"] %>">添加</a>
                        </div>
                    </div>
                    <div class="table-responsive">
                        <table class="table table-striped jambo_table bulk_action">
                            <thead>
                                <tr class="headings">
                                    <th>#</th>
                                    <th class="column-title">节点类型</th>
                                    <th class="column-title">是否隔离</th>
                                    <th class="column-title">隔离级别 </th>
                                    <th class="column-title">操作</th>
                                </tr>
                            </thead>
                            <tbody id="MerConfigList">
                            </tbody>
                        </table>
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
    <div class="modal fade" id="Set_Weight" tabindex="-1" role="dialog" style="display: none;" aria-hidden="true">
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
                            <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                                权重值<span class="required">*</span>
                            </label>
                            <div class="col-md-9 col-sm-9 ">
                                <input type="text" class="form-control text-left" placeholder="权重值" id="WeightNum" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" onclick="_SaveWeight()">保存</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="Set_MerConfig" tabindex="-1" role="dialog" style="display: none;" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">修改跨区域链接配置</h4>
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">×</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal form-label-left">
                        <div class="item form-group">
                            <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                                是否隔离
                            </label>
                            <div class="col-md-6 col-sm-6 text-left">
                                <input type="checkbox" style="width: 50px" class="form-control" id="IsRegionIsolate" />
                            </div>
                        </div>
                        <div class="item form-group">
                            <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                                隔离级别
                            </label>
                            <div class="col-md-6 col-sm-6 ">
                                <select class="form-control" id="IsolateLevel">
                                    <option value="false">完全隔离</option>
                                    <option value="true">区域隔离</option>
                                </select>
                                <p>*完全隔离: 只访问同机房的节点</p>
                                <p>*区域隔离: 首先访问同机房的节点，都不可用时访问其它区域节点</p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" onclick="_SaveMerConfig()">保存</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Js" runat="server">
    <script type="text/javascript" src="/vendors/jsonview/jquery.jsonview.min.js"></script>
    <script type="text/javascript" src="/vendors/pagination/jquery.pagination.js"></script>
    <script type="text/javascript" src="/Js/RpcMerInfo.js"></script>
    <script id="ListTemplate" type="text/html">
        {{each list as a}}
        <tr>
            <th scope="row">{{$index}}</th>
            <td>{{a.ServerName}}</td>
            <td>{{a.SystemName}}</td>
            <td>{{a.IsOnline?"在线":"离线"}}</td>
            <td>{{a.Weight}}</td>
            <td>
                <a class="btn btn-sm btn-danger" href="javascript:_drop('{{a.Id}}')">解绑</a>
                <a class="btn btn-sm  btn-secondary" href="javascript:_setWeight('{{a.Id}}')">设置权重</a>
                <a class="btn btn-sm btn-info" href="ReduceInRank.aspx?merId=<%=this.Request.QueryString["id"] %>&serverId={{a.ServerId}}">降级和熔断</a>
                <a class="btn btn-sm btn-info" href="ClientLimitConfig.aspx?merId=<%=this.Request.QueryString["id"] %>&serverId={{a.ServerId}}">限流配置</a>
            </td>
        </tr>
        {{/each}}
    </script>
    <script id="MerConfigListTemplate" type="text/html">
        {{each list as a}}
        <tr>
            <th scope="row">{{$index}}</th>
            <td>{{a.SystemName}}</td>
            <td>{{a.IsRegionIsolate?"是":"否"}}</td>
            <td>{{a.IsolateLevel?"区域优先":"完全隔离"}}</td>
            <td>
                <a class="btn btn-sm btn-danger" href="javascript:_dropMerConfig('{{a.Id}}')">删除</a>
                <a class="btn btn-sm  btn-secondary" href="javascript:_setMerConfig('{{a.Id}}')">设置</a>
            </td>
        </tr>
        {{/each}}
    </script>
    <script id="ConfigsTemplate" type="text/html">
        {{each list as a}}
        <tr>
            <th scope="row">{{$index}}</th>
            <td>{{a.Range}}</td>
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
    <script id="TaskTemplate" type="text/html">
        {{each list as a}}
        <tr>
            <th scope="row">{{$index}}</th>
            <td>{{a.TaskName}}</td>
            <td>{{GetTaskType(a.TaskType)}}</td>
            <td>{{a.TaskTimeSpan}} 秒</td>
            <td>{{a.TaskPriority}}</td>
            <td>{{GetSendType(a.SendType)}}</td>
            <td>{{a.VerNum}}</td>
            <td>
                <a class="btn btn-sm btn-info" href="SetAutoTask.aspx?id={{a.Id}}&rid=<%=this.Request.QueryString["id"] %>">编辑</a>
                <a class="btn btn-sm btn-danger" href="javascript:_dropTask('{{a.Id}}')">删除</a>
            </td>
        </tr>
        {{/each}}
    </script>
</asp:Content>
