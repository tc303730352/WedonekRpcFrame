<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AddServerConfig.aspx.cs" Inherits="Wedonek.Web.Server.AddServerConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="page-title">
        <div class="title_left">
            <h3>服务节点配置添加</h3>
        </div>
    </div>
    <div class="clearfix"></div>
    <div class="row">
        <div class="col-sm-12">
            <form id="RpcConfig">
                <div class="form-horizontal form-label-left">
                    <div class="item form-group">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                            配置名<span class="required">*</span>
                        </label>
                        <div class="col-md-6 col-sm-6 ">
                            <input type="text" name="Name" required="required" class="form-control " placeholder="配置名">
                        </div>
                    </div>
                    <div class="item form-group">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                            值类型
                        </label>
                        <div class="col-md-6 col-sm-6 ">
                            <select class="form-control" name="ValueType">
                                <option value="0">字符串</option>
                                <option value="1">JSON格式</option>
                            </select>
                        </div>
                    </div>
                    <div class="item form-group">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                            配置值
                        </label>
                        <div class="col-md-6 col-sm-6 ">
                            <textarea class="resizable_textarea form-control" name="Value" placeholder=""></textarea>
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
    <script type="text/javascript">
        function _Save() {
            var data = $('#RpcConfig').FormData();
            data.ServerId = urlTools.GetUrlValue("id");
            SubmitData("SysConfig/Add", null, data, function () {
                alert("添加成功!");
                window.location.href = "RpcServer.aspx?id=" + data.ServerId;
            });
        }

    </script>
</asp:Content>
