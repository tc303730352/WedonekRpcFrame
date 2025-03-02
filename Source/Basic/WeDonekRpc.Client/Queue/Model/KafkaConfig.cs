
using Confluent.Kafka;

namespace WeDonekRpc.Client.Queue.Model
{
    public class KafkaConfig
    {
        /// <summary>
        /// 服务器链接
        /// </summary>
        public QueueCon[] Servers { get; set; }
        /// <summary>
        /// 强制刷新元数据时间
        /// </summary>
        public int MetadataMaxAgeMs { get; set; } = 300000;


        /// <summary>
        /// 发送缓冲区大小
        /// </summary>
        public int SendBufferSize { get; set; }

        /// <summary>
        /// 接收缓冲区大小
        /// </summary>
        public int ReceiveBufferSize { get; set; }

        /// <summary>
        /// 连接失败后，尝试连接Kafka的时间间隔
        /// </summary>
        public int ReconnectBackoffMs { get; set; } = 50;
        /// <summary>
        /// 尝试连接到Kafka，生产者客户端等待的最大时间
        /// </summary>
        public int ReconnectBackoffMaxMs { get; set; } = 1000;
        /// <summary>
        /// 请求超时时间
        /// </summary>
        public int SocketTimeoutMs { get; set; } = 20000;
        /// <summary>
        /// 启用长链接
        /// </summary>
        public bool SocketKeepaliveEnable { get; set; } = true;


        /// <summary>
        /// 启用幂等
        /// </summary>
        public bool EnableIdempotence { get; set; } = false;


        /// <summary>
        /// 空连接的超时限制
        /// </summary>
        public int ConnectionsMaxIdleMs { get; set; } = 600000;
        /// <summary>
        /// 取消请求之前的最大时间长度（以毫秒为单位）
        /// </summary>
        public int CancellationDelayMaxMs { get; set; } = 100;
        /// <summary>
        /// 如果启用，librdkafka将在第一次调用rd_kafka_new（）时使用srand（current_time.ms）初始化PRNG（只有在平台上没有rand_r（）时才需要）。
        /// 如果禁用，应用程序必须在调用rd_kafka_new（）之前调用srand（）。
        /// </summary>
        public bool EnableRandomSeed { get; set; } = true;
        /// <summary>
        /// 如果未设置OAuthBearner \u refresh\u cb，
        /// 则启用内置的不安全JWT OAuthBearner令牌处理程序。
        /// 此内置处理程序只能用于开发或测试，不能用于生产。
        /// </summary>
        public bool EnableSaslOauthbearerUnsecureJwt { get; set; } = false;
        /// <summary>
        /// 是否启用证书验证
        /// </summary>
        public bool EnableSslCertificateVerification { get; set; } = false;
        /// <summary>
        /// 指示在ApiVersionRequest失败的情况下使用'broker.version.fallback'回退的时间
        /// ApiVersionRequest仅在与代理建立新连接时发出（例如在升级之后）。
        /// </summary>
        public int ApiVersionFallbackMs { get; set; } = 0;
        /// <summary>
        /// 请求代理支持的API版本，以根据可用的协议功能调整功能。
        /// </summary>
        public bool ApiVersionRequest { get; set; } = false;
        /// <summary>
        /// 请求代理支持的API版本超时时间
        /// </summary>
        public int ApiVersionRequestTimeoutMs { get; set; } = 10000;
        /// <summary>
        /// Kerberos密钥表文件的路径。
        /// 此配置属性仅用作`sasl.kerberos.kinit.cmd`as``中的变量-t“{sasl.kerberos.keytab}”`。
        /// </summary>
        public string SaslKerberosKeytab { get; set; }
        /// <summary>
        /// 链接地址格式
        /// </summary>
        public BrokerAddressFamily BrokerAddressFamily { get; set; } = BrokerAddressFamily.Any;
        /// <summary>
        /// 缓存代理地址解析结果的时间（毫秒）
        /// </summary>
        public int BrokerAddressTtl { get; set; } = 1000;
        /// <summary>
        /// 每个代理连接的最大请求数
        /// </summary>
        public int MaxInFlight { get; set; } = 1000000;
        /// <summary>
        /// 要复制到缓冲区的消息的最大大小。大于此值的消息将通过引用（零拷贝）传递，代价是较大的IOVEC。
        /// </summary>
        public int MessageCopyMaxBytes { get; set; } = 65535;
        /// <summary>
        /// SSL识别方式
        /// </summary>
        public SslEndpointIdentificationAlgorithm SslEndpointIdentificationAlgorithm { get; set; } = SslEndpointIdentificationAlgorithm.None;
        /// <summary>
        ///   Shell command to refresh or acquire the client's Kerberos ticket. This command is executed on client creation and every sasl.kerberos.min.time.before.relogin (0=disable). %{config.prop.name} is replaced by corresponding config object value.
        /// </summary>
        public string SaslKerberosKinitCmd { get; set; }


        public string SslCaLocation { get; set; }

        public string SslCertificatePem { get; set; }

        public string SslCertificateLocation { get; set; }

        public string SslKeyPem { get; set; }
        public string SslKeyPassword { get; set; }
        public string SslKeyLocation { get; set; }
        public string SslSigalgsList { get; set; }
        public string SslCurvesList { get; set; }

        public string SslCipherSuites { get; set; }
        public SecurityProtocol SecurityProtocol { get; set; } = SecurityProtocol.Plaintext;
        public string BrokerVersionFallback { get; set; }


        public string SslCaCertificateStores { get; set; }

        public string SslCrlLocation { get; set; }

        public string SslKeystorePassword { get; set; }

        public string SslEngineLocation { get; set; }

        public string SslEngineId { get; set; }



        public string SaslKerberosServiceName { get; set; }

        public string SaslKerberosPrincipal { get; set; }

        public int? SaslKerberosMinTimeBeforeRelogin { get; set; }

        public string SaslUsername { get; set; }

        public string SaslPassword { get; set; }

        public string SaslOauthbearerConfig { get; set; }

        public string SslKeystoreLocation { get; set; }


        public bool LogConnectionClose { get; set; } = true;

        public string PluginLibraryPaths { get; set; }

        public SaslMechanism SaslMechanism { get; set; } = Confluent.Kafka.SaslMechanism.Gssapi;


        public int MessageMaxBytes { get; set; } = 1000000;

        public int ReceiveMessageMaxBytes { get; set; } = 100000000;

        public int TopicMetadataRefreshIntervalMs { get; set; } = 300000;

        public int TopicMetadataRefreshFastIntervalMs { get; set; } = 250;

        public bool TopicMetadataRefreshSparse { get; set; } = true;

        public int TopicMetadataPropagationMaxMs { get; set; } = 30000;

        public string TopicBlacklist { get; set; }

        public int InternalTerminationSignal { get; set; } = 0;

        public string Debug { get; set; }

        public int SocketSendBufferBytes { get; set; } = 0;

        public int SocketReceiveBufferBytes { get; set; } = 0;
        public bool SocketNagleDisable { get; set; } = false;

        public int SocketMaxFails { get; set; } = 1;

        public int StatisticsIntervalMs { get; set; } = 0;

        public bool LogQueue { get; set; } = false;

        public bool LogThreadName { get; set; } = true;
        /// <summary>
        /// 生产者配置
        /// </summary>
        public KafkaPublicConfig Public
        {
            get;
            set;
        }
        /// <summary>
        /// 消费者配置
        /// </summary>
        public KafkaSubConfig Consumer
        {
            get;
            set;
        }
        /// <summary>
        /// 过期时间(毫秒)
        /// </summary>
        public int Expiration { get; set; }
    }
}
