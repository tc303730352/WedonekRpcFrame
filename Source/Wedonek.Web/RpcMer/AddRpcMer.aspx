<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AddRpcMer.aspx.cs" Inherits="Wedonek.Web.RpcMer.AddRpcMer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="page-title">
        <div class="title_left">
            <h3>添加服务集群资料</h3>
        </div>
    </div>
    <div class="clearfix"></div>
    <div class="row">
        <div class="col-sm-12">
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
                        <input type="text" name="AppId" required="required" value="<%=Guid.NewGuid().ToString("N").ToLower() %>" class="form-control">
                    </div>
                </div>
                <div class="item form-group">
                    <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                        应用秘钥<span class="required">*</span>
                    </label>
                    <div class="col-md-6 col-sm-6 ">
                        <input type="text" name="AppSecret" required="required" value="<%=Guid.NewGuid().ToString("N").ToLower() %>" class="form-control">
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
                <div class="ln_solid"></div>
                <div class="item form-group">
                    <div class="col-md-6 col-sm-6 offset-md-3">
                        <button type="button" class="btn btn-success" onclick="_Save();">提交</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Js" runat="server">
      <script type="text/javascript" src="/Js/AddRpcMer.js"></script>
</asp:Content>
