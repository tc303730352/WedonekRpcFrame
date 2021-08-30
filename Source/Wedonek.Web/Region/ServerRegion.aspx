<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ServerRegion.aspx.cs" Inherits="Wedonek.Web.Region.ServerRegion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="page-title">
        <div class="title_left">
            <h3>节点区域</h3>
        </div>
        <div class="title_right">
            <div class="col-md-5 col-sm-5   form-group pull-right text-right">
                <button class="btn btn-success" onclick="_add();">添加区域</button>
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
                        <th class="column-title">区域Id</th>
                        <th class="column-title">区域名</th>
                        <th class="column-title">操作</th>
                    </tr>
                </thead>
                <tbody id="RegionList">
                </tbody>
            </table>
        </div>
    </div>
     <div class="modal fade" id="Set_Region" tabindex="-1" role="dialog" style="display: none;" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">区域编辑或添加</h4>
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">×</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal form-label-left">
                        <div class="item form-group">
                            <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                                区域名<span class="required">*</span>
                            </label>
                            <div class="col-md-9 col-sm-9 ">
                                 <input type="text" class="form-control text-left" placeholder="区域名" id="RegionName" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" onclick="_save()">保存</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Js" runat="server">
    <script type="text/javascript" src="/Js/ServerRegion.js"></script>
    <script id="ListTemplate" type="text/html">
        {{each list as a}}
        <tr>
            <th scope="row">{{$index}}</th>
            <td>{{a.Id}}</td>
            <td>{{a.RegionName}}</td>
            <td>
                <button class="btn btn-sm btn-danger" onclick="_drop('{{a.Id}}')">删除</button>
                <button class="btn btn-sm btn-info" onclick="_set('{{a.Id}}')">编辑</button>
            </td>
        </tr>
        {{/each}}
    </script>
</asp:Content>
