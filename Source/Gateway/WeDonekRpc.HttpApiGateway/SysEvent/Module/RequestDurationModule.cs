using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.SysEvent.Model;
using WeDonekRpc.Modular;
using WeDonekRpc.ModularModel;
using WeDonekRpc.ModularModel.SysEvent.Model;

namespace WeDonekRpc.HttpApiGateway.SysEvent.Module
{
    internal class RequestDurationModule : IDisposable
    {
        [ThreadStatic]
        private static Stopwatch _RunTime;

        /// <summary>
        /// 监控配置
        /// </summary>
        private readonly HttpTimeEvent[] _EventList;

        private readonly uint _MinThreshold = 0;
        private readonly string[] _AllowMethod = Array.Empty<string>();

        private readonly IRpcEventLogService _LogService;

        private readonly IGatewayApiService _ApiService;

        public RequestDurationModule ( ServiceSysEvent[] obj, IRpcEventLogService service, IGatewayApiService apiService )
        {
            this._ApiService = apiService;
            this._LogService = service;
            this._EventList = obj.Select(a => new HttpTimeEvent(a)).OrderByDescending(a => a.Threshold).ToArray();
            if ( !this._EventList.IsNull() )
            {
                string[] method = new string[] { "GET", "POST", "OPTIONS" };
                this._AllowMethod = method.FindAll(a => this._EventList.IsExists(c => c.Method.IsNull() || c.Method.Contains(a)));
                this._MinThreshold = this._EventList[^1].Threshold;
            }
            else
            {
                this._AllowMethod = new string[] { "GET", "POST", "OPTIONS" };
            }
        }
        public string Module => "HttpRequestTime";

        public void Dispose ()
        {
            if ( this._EventList.Length > 0 )
            {
                this._ApiService.InitEvent -= this._ApiService_InitEvent;
                this._ApiService.EndEvent -= this._ApiService_EndEvent;
            }
        }

        public void Init ()
        {
            if ( this._EventList.Length > 0 )
            {
                this._ApiService.InitEvent += this._ApiService_InitEvent;
                this._ApiService.EndEvent += this._ApiService_EndEvent;
            }
        }
        private void _ApiService_EndEvent ( IApiHandler hander, IRoute route, IApiService service )
        {
            if ( _RunTime != null )
            {
                _RunTime.Stop();
                long time = _RunTime.ElapsedMilliseconds;
                if ( time >= this._MinThreshold )
                {
                    this._WriteLog((uint)time, hander, route, service);
                }
            }
        }

        private void _ApiService_InitEvent ( IApiHandler hander, IRoute route )
        {
            if ( !this._AllowMethod.IsExists(hander.Request.HttpMethod) )
            {
                return;
            }
            else if ( _RunTime == null )
            {
                _RunTime = new Stopwatch();
                _RunTime.Start();
            }
            else
            {
                _RunTime.Restart();
            }
        }


        private void _WriteLog ( uint time, IApiHandler handler, IRoute route, IApiService service )
        {
            HttpTimeEvent ev = null;
            if ( handler.Request.IsPostFile )
            {
                ev = this._EventList.Find(c => c.IsIgnoreUpFile == false && c.Threshold <= time && !c.IgnoreApi.Contains(route.ApiUri));
            }
            else
            {
                ev = this._EventList.Find(c => c.Threshold <= time && !c.IgnoreApi.Contains(route.ApiUri));
            }
            if ( ev == null )
            {
                return;
            }
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "duration", time.ToString() },
                { "ServiceName", route.ServiceName },
                {"Method",handler.Request.HttpMethod},
                { "ApiUri", route.ApiUri },
                { "StatusCode", handler.Response.StatusCode.ToString()},
                {"ClientIp", handler.Request.ClientIp }
            };
            if ( ev.RecordRange != LogRecordRange.基本 )
            {
                if ( handler.Request.UrlReferrer != null )
                {
                    args.Add("UrlReferrer", handler.Request.UrlReferrer.ToString());
                }
                args.Add("ContentType", handler.Request.ContentType);
                args.Add("ContentLength", handler.Request.ContentLength.ToString());
                if ( handler.Request.HttpMethod == "GET" )
                {
                    args.Add("Query", handler.Request.Url.Query);
                }
                else
                {
                    args.Add("IsPostFile", handler.Request.IsPostFile.ToString());
                    args.Add("Post", handler.Request.PostString);
                    if ( handler.Request.IsPostFile )
                    {
                        StringBuilder str = new StringBuilder();
                        handler.Request.Files.ForEach(c =>
                        {
                            _ = str.AppendFormat("name:{0},size:{1},type:{2},cType:{3},md5:{4}\n", c.FileName, c.FileSize, c.FileType, c.ContentType, c.FileMd5);
                        });
                        args.Add("Files", str.ToString());
                    }
                }
                if ( service != null )
                {
                    args.Add("AccreditId", service.AccreditId);
                    args.Add("IdentityId", service.Identity.IdentityId);
                }
                if ( ev.RecordRange == LogRecordRange.完整 )
                {
                    args.Add("ResponseTxt", handler.Response.ResponseTxt);
                }
            }
            this._LogService.AddLog(ev, args);
        }
    }
}
