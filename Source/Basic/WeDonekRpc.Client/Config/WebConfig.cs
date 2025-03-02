using System;
using WeDonekRpc.Client.Helper;
using WeDonekRpc.Client.Queue;
using WeDonekRpc.Client.Queue.Model;

using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.Model.Server;

namespace WeDonekRpc.Client.Config
{
    internal class WebConfig
    {
        public static readonly string ApiVerNum = "1.0.3";

        public static int ApiVer = 0;
        private static readonly LocalEnvironment _Environment;
        static WebConfig ()
        {
            BasicConfig = new RpcBasicConfig();
            RpcConfig = new RpcSysConfig();
            ThreadPool = new ThreadConfig();
            _Environment = new LocalEnvironment
            {
                Mac = LocalConfig.Local.GetValue("rpc:LocalMac", NetworkHelper.GetMac()),
                RunIsAdmin = Tools.IsAdminRun(),
                RunUserIdentity = Tools.GetRunUserIdentity()
            };
        }
        /// <summary>
        /// 获取Kafka配置
        /// </summary>
        /// <returns></returns>
        internal static KafkaConfig GetKafkaConfig ()
        {
            KafkaConfig config = LocalConfig.Local.GetValue<KafkaConfig>("queue:Kafka");
            if (config == null || config.Servers.IsNull())
            {
                return null;
            }
            if (config.Public == null)
            {
                config.Public = new KafkaPublicConfig();
            }
            if (config.Consumer == null)
            {
                config.Consumer = new KafkaSubConfig();
            }
            return config;
        }

        /// <summary>
        /// 系统配置
        /// </summary>
        public static RpcSysConfig RpcConfig
        {
            get;
        }

        public static ThreadConfig ThreadPool
        {
            get;
        }

        /// <summary>
        /// 基础配置
        /// </summary>
        public static RpcBasicConfig BasicConfig
        {
            get;
        }

        #region 基础配置

        public static LocalEnvironment Environment => _Environment;

        public static QueueType QueueType => LocalConfig.Local.GetValue("queue:QueueType", QueueType.RabbitMQ);

        #endregion

        public static RabbitmqConfig GetRabbitmqConfig ()
        {
            RabbitmqConfig config = LocalConfig.Local.GetValue<RabbitmqConfig>("queue:Rabbitmq");
            if (config == null || config.Servers.IsNull())
            {
                return null;
            }
            else if (config.Expiration == 0)
            {
                config.Expiration = RpcConfig.ExpireTime * 1000;
            }
            if (config.ConfirmTimeOut.TotalMilliseconds == 0)
            {
                config.ConfirmTimeOut = new TimeSpan(0, 0, 5, 0);
            }
            return config;
        }
    }
}
