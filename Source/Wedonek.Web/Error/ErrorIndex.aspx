<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ErrorIndex.aspx.cs" Inherits="Wedonek.Web.Error.ErrorIndex" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="/vendors/pagination/pagination.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="page-title">
        <div class="title_left">
            <h3>错误管理</h3>
        </div>
    </div>
    <div class="clearfix"></div>
    <div class="row">
        <div class="col-sm-12">
            <form id="QueryForm">
                <div class="form-inline">
                    <div class="form-group">
                        <label for="ex4" class="col-form-label">错误码：</label>
                        <input type="text" class="form-control text-left" placeholder="错误码" name="ErrorCode" />
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
                        <th class="column-title">错误码</th>
                        <th class="column-title">中文说明</th>
                        <th class="column-title">英文说明</th>
                    </tr>
                </thead>
                <tbody id="ServerList">
                </tbody>
            </table>
            <div id="Pagination" class="m-style pull-right">
            </div>
        </div>
    </div>
    <div class="modal fade" id="Set_Error" tabindex="-1" role="dialog" style="display: none;" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel">友好提示</h4>
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">×</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal form-label-left">
                        <div class="item form-group">
                            <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                                友好提示<span class="required">*</span>
                            </label>
                            <div class="col-md-6 col-sm-6 ">
                                <input type="text" id="ErrorMsg" maxlength="100" class="form-control " placeholder="友好提示">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" onclick="_Save()">保存</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Js" runat="server">
    <script type="text/javascript" src="/vendors/pagination/jquery.pagination.js"></script>
    <script type="text/javascript" src="/Js/ErrorIndex.js"></script>
    <script id="ListTemplate" type="text/html">
        {{each list as a}}
        <tr>
            <th scope="row">{{$index}}</th>
            <td>{{a.ErrorCode}}</td>
            <td>{{if a.Zh=="" || a.Zh==null}}
                    <button class="btn btn-sm btn-info" onclick="_Set('{{a.Id}}','zh')">设置</button>
                {{else}}
                   {{a.Zh}}<button class="btn btn-sm btn-secondary" onclick="_Set('{{a.Id}}','zh',' {{a.Zh}}')">设置</button>
                {{/if}}
            </td>
            <td>{{if a.En=="" || a.En==null}}
                    <button class="btn btn-sm btn-info" onclick="_Set('{{a.Id}}','en')">设置</button>
                {{else}}
                 <button class="btn btn-sm btn-secondary" onclick="_Set('{{a.Id}}','en')">{{a.En}}</button>
                {{/if}}</td>
        </tr>
        {{/each}}
    </script>
</asp:Content>
