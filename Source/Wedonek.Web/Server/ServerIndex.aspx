<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ServerIndex.aspx.cs" Inherits="Wedonek.Web.Server.ServerIndex" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/vendors/pagination/pagination.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="page-title">
        <div class="title_left">
            <h3>服务节点</h3>
        </div>
        <div class="title_right">
            <div class="col-md-5 col-sm-5   form-group pull-right text-right">
                <a class="btn btn-success" href="AddService.aspx">添加节点</a>
            </div>
        </div>
    </div>
    <div class="clearfix"></div>
    <div class="row">
        <div class="col-sm-12">
            <form id="QueryForm">
                <div class="form-inline">
                    <div class="form-group">
                        <label for="ex3" class="col-form-label">服务组：</label>
                        <select class="form-control" id="ServerGroup" name="GroupId"></select>
                    </div>
                    <div class="form-group">
                        <label for="ex4" class="col-form-label">节点类型：</label>
                        <select class="form-control" id="ServerType" name="SystemTypeId"></select>
                    </div>
                    <div class="form-group">
                        <label for="ex4" class="col-form-label">是否在线：</label>
                        <select class="form-control" name="IsOnline">
                            <option value="">全部</option>
                            <option value="true">在线</option>
                            <option value="false">离线</option>
                        </select>
                    </div>
                    <div class="form-group">
                        <label for="ex4" class="col-form-label">节点名</label>
                        <input type="text" class="form-control text-left" placeholder="节点名" name="ServiceName" />
                    </div>
                    <button type="button" id="QueryBtn" class="btn btn-secondary">查询</button>
                </div>
            </form>
        </div>
    </div>
    <div class="row">
        <div class="table-responsive">
            <table class="table table-striped jambo_table bulk_action">
                <thead>
                    <tr class="headings">
                        <th>#</th>
                        <th class="column-title">节点名称</th>
                        <th class="column-title">节点类别</th>
                        <th class="column-title">当前是否在线 </th>
                        <th class="column-title">节点状态</th>
                        <th class="column-title">MAC地址</th>
                        <th class="column-title">IP加端口</th>
                        <th class="column-title">节点编号</th>
                        <th class="column-title">负载方式</th>
                        <th class="column-title">权重</th>
                        <th class="column-title">链接公钥</th>
                        <th class="column-title">客户端版本</th>
                        <th class="column-title">最后离线日期</th>
                        <th class="column-title">操作</th>
                    </tr>
                </thead>
                <tbody id="ServerList">
                </tbody>
            </table>
            <div id="Pagination" class="m-style pull-right">
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Js" runat="server">
    <script type="text/javascript" src="/vendors/pagination/jquery.pagination.js"></script>
    <script type="text/javascript" src="/Js/ServerIndex.js"></script>
    <script id="ListTemplate" type="text/html">
        {{each list as a}}
        <tr>
            <th scope="row">{{$index}}</th>
            <td>{{a.ServerName}}</td>
            <td>{{a.SystemName}}</td>
            <td>{{a.IsOnline?"在线":"离线"}}</td>
            <td>{{FormatState(a.ServiceState)}}</td>
            <td>{{a.ServerMac}}</td>
            <td>{{a.ConIp !=""?a.ConIp+":"+a.ServerPort:a.ServerIp+":"+a.ServerPort}}</td>
            <td>{{a.ServerIndex}}</td>
            <td>{{a.BalancedType}}</td>
            <td>{{a.Weight}}</td>
            <td>{{a.PublicKey}}</td>
            <td>{{a.ApiVer}}</td>
            <td>{{FormDate(a.LastOffliceDate)}}</td>
            <td>
                <a href="RpcServer.aspx?id={{a.Id}}" class="btn btn-sm btn-info">管理</a>
                {{if a.IsOnline ==0}}
                <button class="btn btn-sm btn-danger" onclick="_drop('{{a.Id}}')">删除</button>
                {{/if}}
                {{if a.ServiceState ==0}}
                    <button class="btn btn-sm btn-danger" onclick="_setState({{a.Id}},2)">下线</button>
                    <button class="btn btn-sm btn-danger" onclick="_setState({{a.Id}},3)">停用</button>
                {{else}}
                    <button class="btn btn-sm btn-info" onclick="_setState({{a.Id}},0)">启用</button>
                {{/if}}
            </td>
        </tr>
        {{/each}}
    </script>
</asp:Content>
