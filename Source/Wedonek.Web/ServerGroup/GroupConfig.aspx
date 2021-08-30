<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="GroupConfig.aspx.cs" Inherits="Wedonek.Web.ServerGroup.GroupConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/vendors/jsonview/jquery.jsonview.min.css" rel="stylesheet" />
    <link href="/vendors/pagination/pagination.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="page-title">
        <div class="title_left">
            <h3>节点类别配置管理</h3>
        </div>
    </div>
    <div class="clearfix"></div>
    <div class="row">
        <div class="col-sm-12">
            <form id="QueryForm">
                <div class="form-inline">
                    <div class="form-group">
                        <label for="ex4" class="col-form-label">&nbsp;&nbsp;配置名：</label>
                        <input type="text" class="form-control text-left" placeholder="配置名" name="ConfigName" />
                    </div>
                    <button type="button" id="QueryBtn" class="btn btn-secondary">查询</button>
                    <a class="btn btn-success" href="AddGroupConfig.aspx?id=<%=this.Request.QueryString["id"] %>">添加配置</a>
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
    <script type="text/javascript" src="/vendors/jsonview/jquery.jsonview.min.js"></script>
    <script type="text/javascript" src="/vendors/pagination/jquery.pagination.js"></script>
    <script type="text/javascript" src="/Js/GroupConfig.js"></script>
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
</asp:Content>
