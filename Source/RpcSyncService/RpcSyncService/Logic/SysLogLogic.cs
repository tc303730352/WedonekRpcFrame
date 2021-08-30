using System.Text;

using RpcModel;
using RpcModel.SysError;

using RpcSyncService.Collect;
using RpcSyncService.Model;

using RpcHelper;
namespace RpcSyncService.Logic
{
        internal class SysLogLogic
        {
                public static void SaveSysLog(SysErrorLog[] logs, MsgSource source)
                {
                        SysLog[] sys = logs.ConvertAll(a => new SysLog
                        {
                                AddTime = a.AddTime,
                                Content = a.Content,
                                Exception = a.Exception,
                                GroupId = source.SourceGroupId,
                                ServerId = source.SourceServerId,
                                SystemTypeId = source.SystemTypeId,
                                LogGrade = a.LogGrade,
                                AttrList = a.AttrList,
                                TraceId = a.TraceId,
                                LogGroup = a.LogGroup,
                                LogType = a.LogType,
                                RpcMerId = source.RpcMerId,
                                Title = a.Title,
                                ErrorCode = a.ErrorCode
                        });
                        SysLogCollect.AddLog(sys);
                        _SendErrorLog(sys, source);
                }
                private static void _SendErrorLog(SysErrorLog[] logs, MsgSource source)
                {
                        if (!Config.SyncConfig.IsSendEmail)
                        {
                                return;
                        }
                        else if (!RemoteServerCollect.GetServer(source.SourceServerId, out RemoteServerConfig server))
                        {
                                return;
                        }
                        else
                        {
                                StringBuilder str = new StringBuilder();
                                str.AppendFormat("服务器Id: {0}</br>服务器类型:{1}</br>服务组: {2}</br>集群Id:{3}</br>",
                                             source.SourceServerId,
                                             source.SystemType,
                                             source.GroupTypeVal,
                                             source.RpcMerId);
                                str.AppendFormat("服务器名称: {0}</br> 服务器IP: {1}:{2}</br>内网IP:{3}</br>当前链接IP:{4}</br>",
                                        server.ServerName,
                                        server.RemoteIp,
                                        server.ServerPort,
                                        server.ServerIp,
                                        server.ConIp);
                                str.Append("日志列表:</br>");
                                int num = 0;
                                logs.ForEach(a =>
                                {
                                        if (a.LogType == LogType.信息日志 && a.LogGrade <= Config.SyncConfig.InfoLimitGrade)
                                        {
                                                return;
                                        }
                                        else if (a.LogType == LogType.错误日志 && a.LogGrade <= Config.SyncConfig.ErrorLimitGrade)
                                        {
                                                return;
                                        }
                                        ++num;
                                        if (!string.IsNullOrEmpty(a.Content))
                                        {
                                                a.Content = a.Content.Replace("\r\n", "</br>");
                                        }
                                        str.AppendFormat("类型: {4}</br> 等级: {0}</br> 标题: {1}</br>内容: {2}</br>组别:{3}</br>",
                                                a.LogGrade,
                                                a.Title,
                                                a.Content,
                                                a.LogGroup,
                                                a.LogType);
                                        if (a.LogType == LogType.错误日志 && a.Exception != null)
                                        {
                                                str.Append("错误详细: </br>");
                                                str.AppendFormat("Message: {0}</br> Source: {1}</br> StackTrace: {2}</br>HResult: {3}</br>",
                                                a.Exception.Message,
                                                a.Exception.Source,
                                                a.Exception.StackTrace,
                                                a.Exception.HResult);
                                                if (a.Exception.Data != null)
                                                {
                                                        foreach (string i in a.Exception.Data.Keys)
                                                        {
                                                                str.AppendFormat(" {0}: {1}</br>", i, a.Exception.Data[i]);
                                                        }
                                                }
                                                if (a.Exception.Method != null)
                                                {
                                                        foreach (string i in a.Exception.Method.Keys)
                                                        {
                                                                str.AppendFormat(" {0}: {1}</br>", i, a.Exception.Method[i]);
                                                        }
                                                }
                                        }
                                        str.Append("<p>&nbsp;</p><p>&nbsp;</p>");
                                });

                                if (num == 0)
                                {
                                        return;
                                }
                                EmailModel model = new EmailModel
                                {
                                        DisplayName = Config.SyncConfig.DisplayName,
                                        Reciver = Config.SyncConfig.Reciver,
                                        Content = str.ToString(),
                                        Title = string.Format("服务端信息-数目:{0}", num),
                                        EmailAccount = Config.SyncConfig.EmailAccount,
                                        EmailPwd = Config.SyncConfig.EmailPwd
                                };
                                EmailTools.SendEmail(model);
                        }
                }
        }
}
