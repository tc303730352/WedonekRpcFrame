using System.Text;
using RpcExtend.Collect;
using RpcExtend.Model;
using RpcExtend.Service.Config;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.ExtendModel.SysError;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Email;
using WeDonekRpc.Model;

namespace RpcExtend.Service
{
    internal static class linqHelper
    {
        public static void SendErrorLog (this SysErrorLog[] logs, MsgSource source, IIocService unity)
        {
            ErrorEmailConfig config = SysConfig.ErrorConfig;
            if (!config.IsSendEmail)
            {
                return;
            }
            RemoteServerConfig server = unity.Resolve<IRemoteServerCollect>().GetServer(source.ServerId);
            StringBuilder str = new StringBuilder();
            _ = str.AppendFormat("来源服务器Id: {0}</br>服务器类型:{1}</br>服务组: {2}</br>集群Id:{3}</br>",
                         source.ServerId,
                         source.SystemType,
                         source.SysGroup,
                         source.RpcMerId);
            _ = str.AppendFormat("服务器名称: {0}</br> 服务器IP: {1}:{2}</br>内网IP:{3}</br>当前链接IP:{4}</br>",
                    server.ServerName,
                    server.RemoteIp,
                    server.ServerPort,
                    server.ServerIp,
                    server.ConIp);
            _ = str.Append("日志列表:</br>");
            int num = 0;
            logs.ForEach(a =>
            {
                if (a.LogType == LogType.信息日志 && a.LogGrade <= config.InfoLimitGrade)
                {
                    return;
                }
                else if (a.LogType == LogType.错误日志 && a.LogGrade <= config.ErrorLimitGrade)
                {
                    return;
                }
                ++num;
                if (!string.IsNullOrEmpty(a.LogShow))
                {
                    a.LogShow = a.LogShow.Replace("\r\n", "</br>");
                }
                _ = str.AppendFormat("类型: {4}</br> 等级: {0}</br> 标题: {1}</br>内容: {2}</br>组别:{3}</br>",
                                    a.LogGrade,
                                    a.LogTitle,
                                    a.LogShow,
                                    a.LogGroup,
                                    a.LogType);
                if (a.LogType == LogType.错误日志 && a.Exception != null)
                {
                    _ = str.Append("错误详细: </br>");
                    _ = str.AppendFormat("Message: {0}</br> Source: {1}</br> StackTrace: {2}</br>HResult: {3}</br>",
                                a.Exception.Message,
                                a.Exception.Source,
                                a.Exception.StackTrace,
                                a.Exception.HResult);
                    if (a.Exception.Data != null)
                    {
                        foreach (string i in a.Exception.Data.Keys)
                        {
                            _ = str.AppendFormat(" {0}: {1}</br>", i, a.Exception.Data[i]);
                        }
                    }
                    if (a.Exception.Method != null)
                    {
                        foreach (string i in a.Exception.Method.Keys)
                        {
                            _ = str.AppendFormat(" {0}: {1}</br>", i, a.Exception.Method[i]);
                        }
                    }
                }
                _ = str.Append("<p>&nbsp;</p><p>&nbsp;</p>");
            });
            if (num == 0)
            {
                return;
            }
            EmailModel model = new EmailModel
            {
                DisplayName = config.DisplayName,
                Reciver = config.Reciver,
                Content = str.ToString(),
                Title = string.Format("服务端信息-数目:{0}", num),
                EmailAccount = config.EmailAccount,
                EmailPwd = config.EmailPwd
            };
            EmailTools.SendEmail(model);
        }
    }
}
