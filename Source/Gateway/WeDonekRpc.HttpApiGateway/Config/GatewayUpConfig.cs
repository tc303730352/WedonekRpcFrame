using System;
using Confluent.Kafka;
using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;

namespace WeDonekRpc.HttpApiGateway.Config
{
    /// <summary>
    /// 上传配置
    /// </summary>
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class GatewayUpConfig : IGatewayUpConfig
    {
        private event Action<IGatewayUpConfig> _refEvent;
        public GatewayUpConfig ()
        {
            RpcClient.Config.GetSection("gateway:up").AddRefreshEvent(this._Refresh);
        }

        private void _Refresh ( IConfigSection section, string name )
        {
            this.SingleFileSize = section.GetValue("SingleFileSize", 1024 * 1024 * 10);
            this.LimitFileNum = section.GetValue("LimitFileNum", 1);
            this.BlockUpSize = section.GetValue("BlockUpSize", 10 * 1024 * 1024);
            this.FileExtension = section.GetValue("FileExtension", Array.Empty<string>());
            this.UpBlockNum = section.GetValue("UpBlockNum", 5);
            this.BlockUpUri = section.GetValue("BlockUpUri", "/api/file/block/up").ToLower();
            this.BlockUpState = section.GetValue<RouteSet>("BlockUpState", new RouteSet
            {
                IsAccredit = true,
                RoutePath = "/api/file/block/up/state",
                IsRegex = false
            });
            this.BlockUpUriIsRegex = section.GetValue("BlockUpUriIsRegex", false);
            this.BlockUpOverTime = section.GetValue("BlockUpOverTime", 30);
            this.EnableBlock = section.GetValue("EnableBlock", false);
            this.IsGenerateMd5 = section.GetValue("IsGenerateMd5", true);
            if ( _refEvent != null )
            {
                _refEvent(this);
            }
        }
        /// <summary>
        /// 上传文件是否计算MD5
        /// </summary>
        public bool IsGenerateMd5
        {
            get;
            private set;
        } = true;
        /// <summary>
        /// 单文件上传大小
        /// </summary>
        public int SingleFileSize
        {
            get;
            private set;
        }
        /// <summary>
        /// 允许的扩展
        /// </summary>
        public string[] FileExtension
        {
            get;
            private set;
        }
        /// <summary>
        /// 限定一次请求上传文件数量（0 无限制）
        /// </summary>
        public int LimitFileNum
        {
            get;
            private set;
        }
        /// <summary>
        /// 分块上传大小
        /// </summary>
        public int BlockUpSize
        {
            get;
            private set;
        }
        /// <summary>
        /// 启用分包上传
        /// </summary>
        public bool EnableBlock { get; private set; } = false;
        /// <summary>
        /// 上传块数
        /// </summary>
        public int UpBlockNum { get; private set; } = 5;
        /// <summary>
        /// 分块上传URI地址
        /// </summary>
        public string BlockUpUri
        {
            get;
            private set;
        }
    
        /// <summary>
        /// 分块上传URI地址是否是正则表达式
        /// </summary>
        public bool BlockUpUriIsRegex
        {
            get;
            private set;
        }
        /// <summary>
        /// 分块上传超时时间
        /// </summary>
        public int BlockUpOverTime
        {
            get;
            private set;
        }

        public RouteSet BlockUpState
        {
            get;
            private set;
        }

        public ApiUpSet ToUpSet ()
        {
            return new ApiUpSet
            {
                BlockUpSize = this.BlockUpSize,
                Extension = this.FileExtension,
                LimitFileNum = this.LimitFileNum,
                MaxSize = this.SingleFileSize,
                TempFileSaveDir = HttpService.HttpService.Config.File.TempDirPath,
                IsGenerateMd5 = this.IsGenerateMd5
            };
        }

        public void RefreshEvent ( Action<IGatewayUpConfig> refresh )
        {
            _refEvent += refresh;
            refresh(this);
        }
    }
}
