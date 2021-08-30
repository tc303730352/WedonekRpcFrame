<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ServerGroup.aspx.cs" Inherits="Wedonek.Web.ServerGroup.ServerGroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/vendors/pagination/pagination.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="page-title">
        <div class="title_left">
            <h3>节点类型</h3>
        </div>
        <div class="title_right">
            <div class="col-md-5 col-sm-5   form-group pull-right text-right">
                <button type="button" class="btn btn-success" data-toggle="modal" data-target="#Add_Group">添加服务组</button>
                  <button type="button" class="btn btn-success" data-toggle="modal" data-target="#Add_SysType">添加节点类型</button>
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
                        <label for="ex3" class="col-form-label">负载方式：</label>
                        <select class="form-control" name="BalancedType">
                            <option value="">全部</option>
                            <option value="4">平均</option>
                            <option value="0">单例</option>
                            <option value="1">随机</option>
                            <option value="2">权值</option>
                            <option value="3">平均响应时间</option>
                        </select>
                    </div>
                    <div class="form-group">
                        <label for="ex4" class="col-form-label">类型名</label>
                        <input type="text" class="form-control text-left" placeholder="类型名" name="Name" />
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
                        <th class="column-title">服务组</th>
                        <th class="column-title">服务组值</th>
                        <th class="column-title">节点类别值</th>
                        <th class="column-title">类别值 </th>
                        <th class="column-title">负载方式</th>
                        <th class="column-title">操作</th>
                    </tr>
                </thead>
                <tbody id="SysTypeList">
                </tbody>
            </table>
            <div id="Pagination" class="m-style pull-right">
            </div>
        </div>
    </div>
    <div class="modal fade" id="Add_Group" tabindex="-1" role="dialog" style="display: none;" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">添加组别</h4>
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">×</span>
                    </button>
                </div>
                <div class="modal-body">
                    <form id="AddGroup">
                        <div class="form-horizontal form-label-left">
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                                    组别名称：
                                </label>
                                <div class="col-md-6 col-sm-6 ">
                                    <input type="text" name="GroupName" class="form-control " placeholder="组别名称" />
                                </div>
                            </div>
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                                    组别值：
                                </label>
                                <div class="col-md-6 col-sm-6">
                                    <input type="text" name="TypeVal" class="form-control " placeholder="组别值" />
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" onclick="_SaveGroup()">保存</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="Add_SysType" tabindex="-1" role="dialog" style="display: none;" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">添加节点类别</h4>
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">×</span>
                    </button>
                </div>
                <div class="modal-body">
                    <form id="AddSysType">
                        <div class="form-horizontal form-label-left">
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                                    所属组别:
                                </label>
                                <div class="col-md-6 col-sm-6 ">
                                    <select class="form-control" name="GroupId" id="ChoiseGroup"></select>
                                </div>
                            </div>
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                                    类别名称：
                                </label>
                                <div class="col-md-6 col-sm-6 ">
                                    <input type="text" name="SystemName" class="form-control " placeholder="组别名称" />
                                </div>
                            </div>
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                                    组别值：
                                </label>
                                <div class="col-md-6 col-sm-6">
                                    <input type="text" name="TypeVal" class="form-control " placeholder="组别值" />
                                </div>
                            </div>
                            <div class="item form-group">
                                <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                                    值类型
                                </label>
                                <div class="col-md-6 col-sm-6 ">
                                    <select class="form-control" name="BalancedType">
                                        <option value="4" selected="selected">平均</option>
                                        <option value="0">单例</option>
                                        <option value="1">随机</option>
                                        <option value="2">权值</option>
                                        <option value="3">平均响应时间</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" onclick="_SaveSysType()">保存</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Js" runat="server">
    <script type="text/javascript" src="/vendors/pagination/jquery.pagination.js"></script>
    <script type="text/javascript" src="/Js/ServerGroup.js"></script>
    <script id="ListTemplate" type="text/html">
        {{each list as a}}
        <tr>
            <th scope="row">{{$index}}</th>
            <td>{{a.GroupName}}</td>
            <td>{{a.GroupVal}}</td>
            <td>{{a.SystemName}}</td>
            <td>{{a.TypeVal}}</td>
            <td>{{a.BalancedType}}</td>
            <td>
                <a href="GroupConfig.aspx?id={{a.Id}}" class="btn btn-sm btn-info">配置管理</a>
                <a class="btn btn-sm btn-danger" href="javascript:_drop('{{a.Id}}')">删除</a>
            </td>
        </tr>
        {{/each}}
    </script>
</asp:Content>
