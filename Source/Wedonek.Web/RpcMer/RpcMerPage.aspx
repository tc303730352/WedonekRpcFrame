<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RpcMerPage.aspx.cs" Inherits="Wedonek.Web.RpcMer.RpcMerPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/vendors/pagination/pagination.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="page-title">
        <div class="title_left">
            <h3>服务集群管理</h3>
        </div>
        <div class="title_right">
            <div class="col-md-5 col-sm-5   form-group pull-right top_search">
                <div class="input-group">
                    <a   class="btn btn-success" href="AddRpcMer.aspx">添加集群</a>
                    <input type="text" class="form-control" placeholder="集群名称" id="MerName" />
                    <span class="input-group-btn">
                        <button class="btn btn-default" id="QueryBtn" type="button">查询</button>
                    </span>
                </div>
            </div>
        </div>
    </div>
    <div class="clearfix"></div>
    <div class="row">
        <div class="table-responsive">
            <table class="table table-striped jambo_table bulk_action">
                <thead>
                    <tr class="headings">
                        <th>#</th>
                        <th class="column-title">系统名</th>
                        <th class="column-title">AppId</th>
                        <th class="column-title">秘钥 </th>
                        <th class="column-title">允许访问地址</th>
                        <th class="column-title">操作</th>
                    </tr>
                </thead>
                <tbody id="MerList">
                </tbody>
            </table>
           <div id="Pagination" class="m-style pull-right">
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Js" runat="server">
    <script type="text/javascript" src="/vendors/pagination/jquery.pagination.js"></script>
    <script type="text/javascript" src="/Js/RpcMer.js"></script>
    <script id="MerListTemplate" type="text/html">
        {{each list as a}}
        <tr>
            <th scope="row">{{$index}}</th>
            <td>{{a.SystemName}}</td>
            <td>{{a.AppId}}</td>
            <td>{{a.AppSecret}}</td>
            <td>{{formatIp(a.AllowServerIp)}}</td>
            <td>
                <a href="RpcMerInfo.aspx?id={{a.Id}}" class="btn btn-sm btn-info">管理</a>
                <a class="btn btn-sm btn-danger" href="javascript:_drop('{{a.Id}}')">删除</a>
            </td>
        </tr>
        {{/each}}
    </script>
</asp:Content>
