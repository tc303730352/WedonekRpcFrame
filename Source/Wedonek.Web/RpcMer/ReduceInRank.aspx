<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ReduceInRank.aspx.cs" Inherits="Wedonek.Web.RpcMer.ReduceInRank" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="page-title">
        <div class="title_left">
            <h3>熔断降级配置</h3>
        </div>
    </div>
    <div class="clearfix"></div>
    <div class="row">
        <div class="col-sm-12">
            <form id="ReduceInRank">
                <div class="form-horizontal form-label-left">
                        <div class="item form-group">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                            熔断触发错误数：
                        </label>
                        <div class="col-md-6 col-sm-6 ">
                            <input type="text" name="FusingErrorNum" value="1" class="form-control " placeholder="熔断触发错误数"/>
                        </div>
                    </div>
                      <div class="item form-group">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                            启用降级：
                        </label>
                        <div class="col-md-6 col-sm-6 text-left">
                            <input type="checkbox" name="IsEnable" style="width:30px;"  class="form-control " checked="checked" />
                        </div>
                    </div>
                    <div class="item form-group">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="first-name">
                            触发降级错误数：
                        </label>
                        <div class="col-md-6 col-sm-6 ">
                            <input type="text" name="LimitNum"  value="10" class="form-control " placeholder="触发限制错误数"/>
                        </div>
                    </div>
                    <div class="item form-group">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                            刷新统计数的时长(秒)：
                        </label>
                        <div class="col-md-6 col-sm-6">
                            <input type="text" name="RefreshTime"  value="5" class="form-control " placeholder="触发降级熔断时长"/>
                        </div>
                    </div>
                    <div class="item form-group">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                            降级熔断起始时长(秒)：
                        </label>
                        <div class="col-md-6 col-sm-6">
                            <input type="text" name="BeginDuration" value="1" class="form-control " placeholder="起始秒数" />
                        </div>
                    </div>   
                    <div class="item form-group">
                        <label class="col-form-label col-md-3 col-sm-3 label-align" for="last-name">
                            降级熔断结束时长(秒)：
                        </label>
                        <div class="col-md-6 col-sm-6">
                            <input type="text" name="EndDuration"  class="form-control " value="3" placeholder="结束秒数"/>
                        </div>
                    </div>
                    <div class="item form-group">
                        <div class="col-md-6 col-sm-6 offset-md-6">
                            <button type="button" class="btn btn-success" onclick="_Save();">保 存</button>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Js" runat="server">
       <script type="text/javascript" src="/Js/ReduceInRank.js"></script>
</asp:Content>
