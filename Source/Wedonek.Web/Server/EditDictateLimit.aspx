<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="EditDictateLimit.aspx.cs" Inherits="Wedonek.Web.Server.EditDictateLimit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="page-title">
        <div class="title_left">
            <h3>编辑接口限流配置</h3>
        </div>
    </div>
    <div class="clearfix"></div>
    <div class="row">
        <div class="col-sm-12">
            <form id="DictateLimit">
                <div class="form-horizontal form-label-left">
                    <div class="item form-group">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                            接口名：
                        </label>
                        <div class="col-md-6 col-sm-6 ">
                            <input class="form-control" type="text" name="Dictate" />
                        </div>
                    </div>
                    <div class="item form-group">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                            限流方式：
                        </label>
                        <div class="col-md-6 col-sm-6 ">
                            <select class="form-control" name="LimitType" id="LimitType">
                                <option value="1">固定时间窗</option>
                                <option value="2">流动时间窗</option>
                                <option value="3">令牌桶</option>
                                <option value="4">漏桶</option>
                            </select>
                        </div>
                    </div>
                    <div class="item form-group TimeLimit">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                            最大请求量：
                        </label>
                        <div class="col-md-6 col-sm-6 ">
                            <input class="form-control" type="number" min="1" name="LimitNum" />
                        </div>
                    </div>
                    <div class="item form-group TimeLimit">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                            限定窗口时间(秒)：
                        </label>
                        <div class="col-md-6 col-sm-6 ">
                            <input class="form-control" type="number" min="1" name="LimitTime" />
                        </div>
                    </div>
                    <div class="item form-group LeakyLimit">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                            漏桶大小：
                        </label>
                        <div class="col-md-6 col-sm-6 ">
                            <input class="form-control" type="number" min="1" name="BucketSize" />
                        </div>
                    </div>
                    <div class="item form-group LeakyLimit">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                            漏桶出量：
                        </label>
                        <div class="col-md-6 col-sm-6 ">
                            <input class="form-control" type="number" min="1" name="BucketOutNum" />
                        </div>
                    </div>
                    <div class="item form-group TokenLimit">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                            最大令牌数：
                        </label>
                        <div class="col-md-6 col-sm-6 ">
                            <input class="form-control" type="number" min="1" name="TokenNum" />
                        </div>
                    </div>
                    <div class="item form-group TokenLimit">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                            每秒新增令牌：
                        </label>
                        <div class="col-md-6 col-sm-6 ">
                            <input class="form-control" type="number" min="1" name="TokenInNum" />
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
    <script type="text/javascript" src="/Js/EditDictateLimit.js"></script>
</asp:Content>
