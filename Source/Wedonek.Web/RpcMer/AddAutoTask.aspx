<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AddAutoTask.aspx.cs" Inherits="Wedonek.Web.RpcMer.AddAutoTask" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="page-title">
        <div class="title_left">
            <h3>自动任务添加</h3>
        </div>
    </div>
    <div class="clearfix"></div>
    <div class="row">
        <div class="col-sm-12">
            <div class="form-horizontal form-label-left">
                <form id="AutoTaskFrom">
                    <div class="item form-group">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                            任务名<span class="required">*</span>
                        </label>
                        <div class="col-md-6 col-sm-6 ">
                            <input type="text" name="TaskName" required="required" class="form-control " placeholder="任务名">
                        </div>
                    </div>
                    <div class="item form-group">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                            任务类型
                        </label>
                        <div class="col-md-6 col-sm-6">
                            <select class="form-control" id="TaskType" name="TaskType">
                                <option value="0">定时任务</option>
                                <option value="1">间隔任务</option>
                                <option value="2">定时间隔任务</option>
                            </select>
                        </div>
                    </div>
                    <div class="item form-group">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                            任务间隔时间(秒)<span class="required">*</span>
                        </label>
                        <div class="col-md-6 col-sm-6 ">
                            <input type="text" name="TaskTimeSpan" maxlength="8" class="form-control " placeholder="任务间隔时间">
                        </div>
                    </div>
                    <div class="item form-group">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                            优先级<span class="required">*</span>
                        </label>
                        <div class="col-md-6 col-sm-6 ">
                            <input type="text" name="TaskPriority" value="1" class="form-control " placeholder="优先级">
                        </div>
                    </div>
                    <div class="item form-group">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                            发送方式
                        </label>
                        <div class="col-md-6 col-sm-6 ">
                            <select class="form-control" name="SendType" id="SendType">
                                <option value="0">消息</option>
                                <option value="1">HTTP</option>
                                <option value="2">广播</option>
                            </select>
                        </div>
                    </div>
                </form>
                <div class="item form-group">
                    <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                        发送参数配置
                    </label>
                    <div class="col-md-6 col-sm-6 " id="SendParam">
                    </div>
                </div>
                <div class="item form-group">
                    <div class="col-md-6 col-sm-6 offset-md-6">
                        <button type="button" class="btn btn-success" onclick="_Save();">提交</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Js" runat="server">
    <script type="text/javascript" src="/vendors/jsoneditor/jsoneditor.js"></script>
    <script type="text/javascript" src="/js/AddAutoTask.js"></script>
</asp:Content>
