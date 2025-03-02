using System;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.HttpApiGateway.Idempotent;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;
namespace WeDonekRpc.HttpApiGateway.Config
{

    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class IdempotentConfig : IIdempotentConfig
    {
        private bool _IsLimit = false;
        private string[] _Path;
        public IdempotentConfig (ISysConfig config)
        {
            IConfigSection section = config.GetSection("gateway:idempotent");
            section.AddRefreshEvent(this._Refresh);
        }

        public event Action<IIdempotentConfig> RefreshEvent;

        private void _Init (IConfigSection config)
        {
            this.IdempotentType = config.GetValue<IdempotentType>("Type", IdempotentType.关闭);
            this.TokenRoute = config.GetValue<RouteSet>("Route", new RouteSet
            {
                IsAccredit = true,
                RoutePath = "/api/idempotent/token",
                IsRegex = false
            });
            this.IsEnableRoute = config.GetValue<bool>("IsEnableRoute", true);
            this.Expire = config.GetValue<int>("Expire", 30);
            this.SaveType = config.GetValue<StatusSaveType>("SaveType", StatusSaveType.Local);
            this.ArgName = config.GetValue("ArgName", "tokenId");
            this.RequestMehod = config.GetValue<RequestMehod>("Method", RequestMehod.Head);
            string[] paths = config.GetValue<string[]>("Path");
            if (paths.IsNull())
            {
                this._IsLimit = false;
            }
            else
            {
                this._Path = paths;
                this._IsLimit = true;
            }
        }
        private void _Refresh (IConfigSection config, string name)
        {
            this._Init(config);
            if (name != string.Empty)
            {
                RefreshEvent?.Invoke(this);
            }
        }

        public bool CheckIsLimit (string path)
        {
            return this._IsLimit && this._Path.IsExists(path);
        }

        public IdempotentType IdempotentType
        {
            get;
            private set;
        }
        public string ArgName
        {
            get;
            private set;
        }
        public RequestMehod RequestMehod
        {
            get;
            private set;
        }
        public RouteSet TokenRoute
        {
            get;
            private set;
        }
        /// <summary>
        /// Token过期时间(秒)
        /// </summary>
        public int Expire
        {
            get;
            private set;
        }
        /// <summary>
        /// 存储方式
        /// </summary>
        public StatusSaveType SaveType
        {
            get;
            private set;
        }

        public bool IsEnableRoute
        {
            get;
            private set;
        }
    }
}
