<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AddConRelation.aspx.cs" Inherits="Wedonek.Web.Server.AddConRelation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="page-title">
        <div class="title_left">
            <h3>设定服务节点链接关系</h3>
        </div>
        <div class="title_right">
            <div class="col-md-5 col-sm-5   form-group pull-right text-right">
                <a class="btn btn-success" href="RpcServer.aspx?id=<%=this.Request.QueryString["Id"] %>">返回</a>
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
                        <label for="ex4" class="col-form-label">节点类型</label>
                        <select class="form-control" id="ServerType" name="SystemTypeId"></select>
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
                        <th class="column-title">MAC地址</th>
                        <th class="column-title">IP加端口</th>
                        <th class="column-title">是否使用远端IP</th>
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
    <script type="text/javascript" src="/Js/AddConRelation.js"></script>
    <script id="ListTemplate" type="text/html">
        {{each list as a}}
        <tr>
            <th scope="row">{{$index}}</th>
            <td>{{a.ServerName}}</td>
            <td>{{a.GroupName}} - {{a.SystemName}}</td>
            <td>{{a.IsOnline?"在线":"离线"}}</td>
            <td>{{a.ServerMac}}</td>
            <td>{{a.ConIp !=""?a.ConIp+":"+a.ServerPort:a.ServerIp+":"+a.ServerPort}}</td>
            <td>{{a.IsRemote?"是":"否"}}</td>
            <td>{{if a.Id !=serverId}}
                {{if a.IsRemote ==false}}
                <a href="javascript:_bind({{a.Id}})" class="btn btn-sm btn-info">绑定</a>
                {{else}}
                <a href="javascript:_unbind({{a.Id}})" class="btn btn-sm btn-danger">解绑</a>
                {{/if}}
                    {{/if}}
            </td>
        </tr>
        {{/each}}
    </script>
</asp:Content>
