<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AddService.aspx.cs" Inherits="Wedonek.Web.Server.AddService" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/vendors/jsonview/jquery.jsonview.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="page-title">
        <div class="title_left">
            <h3>添加服务节点</h3>
        </div>
    </div>
    <div class="clearfix"></div>
    <div class="row">
        <div class="col-sm-12">
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
                            内网链接Ip<span class="required">*</span>
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
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Js" runat="server">
    <script type="text/javascript" src="/vendors/jsoneditor/jsoneditor.js"></script>
    <script type="text/javascript" src="/Js/AddService.js"></script>
</asp:Content>
