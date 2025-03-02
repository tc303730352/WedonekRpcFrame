using System;
using System.Collections.Generic;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.RetryService.Model;
using WeDonekRpc.ExtendModel.RetryTask.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.Client.RetryService.Config
{
    [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.SingleInstance)]
    internal class AutoRetryConfig : IAutoRetryConfig
    {
        private RetrySet _Default;

        private readonly Dictionary<string, RetrySet> _RetryConfig = [];

        private readonly IConfigSection _Config;
        public AutoRetryConfig (IConfigCollect config)
        {
            this._Config = config.GetSection("RetryTask");
            this._Config.AddRefreshEvent(this._load);
        }

        private void _load (IConfigSection section, string name)
        {
            if (name.IsNull())
            {
                IConfigSection def = section.GetSection("default");
                this._Default = this._GetConfig(def);
                section.Keys.ForEach(c =>
                {
                    this._RetryConfig.Add(c, this._GetConfig(section.GetSection(c)));
                });
            }
            else if (name == "default" || name.StartsWith("default:"))
            {
                this._Default = this._GetConfig(section.GetSection("default"));
            }
            else
            {
                string key = section.Keys.Find(c => c == name || name.StartsWith(c + ":"));
                if (key != null)
                {
                    this._RetryConfig.AddOrSet(key, this._GetConfig(section.GetSection(key)));
                }
            }
        }
        private RetrySet _GetConfig (IConfigSection config)
        {
            return new RetrySet
            {
                StartInterval = config.GetValue<int?>("StartInterval"),
                MaxRetryTime = config.GetValue<int?>("MaxRetryTime"),
                MaxRetry = config.GetValue<int?>("MaxRetry", 3),
                Interval = config.GetValue("Interval", new int[] { 1, 2, 3 }),
                StopErrorCode = config.GetValue("ErrorCode", Array.Empty<string>())
            };
        }
        public RetryConfig GetRetrySet (string key)
        {
            RetrySet retry = this._RetryConfig.GetValueOrDefault(key, this._Default);
            return new RetryConfig
            {
                Begin = retry.StartInterval.HasValue ? DateTime.Now.AddSeconds(retry.StartInterval.Value).ToLong() : null,
                End = retry.MaxRetryTime.HasValue ? DateTime.Now.AddSeconds(retry.MaxRetryTime.Value).ToLong() : null,
                Interval = retry.Interval,
                MaxRetry = retry.MaxRetry,
                StopErrorCode = retry.StopErrorCode
            };
        }
    }
}
