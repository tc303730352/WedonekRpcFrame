<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AddMerConfig.aspx.cs" Inherits="Wedonek.Web.RpcMer.AddMerConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="page-title">
        <div class="title_left">
            <h3>跨区域链接配置</h3>
        </div>
    </div>
    <div class="clearfix"></div>
    <div class="row">
        <div class="col-sm-12">
            <form id="RpcMerConfig">
                <div class="form-horizontal form-label-left">
                    <div class="item form-group">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                            节点类型
                        </label>
                        <div class="col-md-6 col-sm-6">
                            <select class="form-control" id="ServerGroup"></select>
                            <select class="form-control" id="ServerType" name="SystemTypeId"></select>
                        </div>
                    </div>
                    <div class="item form-group">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                            是否隔离
                        </label>
                        <div class="col-md-6 col-sm-6 text-left">
                            <input type="checkbox" style="width:50px" class="form-control" checked="checked" name="IsRegionIsolate" />
                        </div>
                    </div>
                    <div class="item form-group">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                            隔离级别
                        </label>
                        <div class="col-md-6 col-sm-6 ">
                            <select class="form-control" name="IsolateLevel">
                                <option value="false">完全隔离</option>
                                <option value="true" selected="selected">区域隔离</option>
                            </select>
                            <p>*完全隔离: 只访问同机房的节点</p>
                            <p>*区域隔离: 首先访问同机房的节点，都不可用时访问其它区域节点</p>
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
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Js" runat="server">
     <script type="text/javascript" src="/Js/AddRpcMerConfig.js"></script>
</asp:Content>
