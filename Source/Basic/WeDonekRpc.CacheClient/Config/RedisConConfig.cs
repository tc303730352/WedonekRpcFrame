using WeDonekRpc.Helper;

namespace WeDonekRpc.CacheClient.Config
{
    public class RedisConConfig
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConString
        {
            get;
            set;
        }
        public string Connection
        {
            get;
            set;
        }
        public int WriteBuffer { get; set; } = 10240;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 默认数据库
        /// </summary>
        public int Database
        {
            get;
            set;
        } = 0;
        /// <summary>
        ///  异步方式自动使用管道
        /// </summary>
        public bool AsyncPipeline
        {
            get;
            set;
        } = true;
        /// <summary>
        /// 链接池大小
        /// </summary>
        public int PoolSize
        {
            get;
            set;
        } = 10;

        /// <summary>
        /// 连接池中元素的空闲时间（MS），适合连接到远程redis服务器
        /// </summary>
        public int IdleTimeout
        {
            get;
            set;
        } = 60000;
        /// <summary>
        /// 链接超时
        /// </summary>
        public int ConnectTimeout
        {
            get;
            set;
        } = 5000;

        /// <summary>
        ///  发送/接收超时
        /// </summary>
        public int SyncTimeout
        {
            get;
            set;
        } = 10000;

        /// <summary>
        ///  预热连接，接收值，例如Preheat=5 Preheat 5 connections
        /// </summary>
        public int Preheat
        {
            get;
            set;
        } = 5;

        /// <summary>
        ///  跟随系统退出事件自动释放
        /// </summary>
        public bool AutoDispose
        {
            get;
            set;
        } = true;

        // <summary>
        ///   启用加密传输
        /// </summary>
        public bool SSL
        {
            get;
            set;
        } = false;

        // <summary>
        ///   是否尝试集群模式，阿里云、腾讯云集群需要设置此选项为false
        /// </summary>
        public bool Testcluster
        {
            get;
            set;
        } = false;
        /// <summary>
        /// 执行错误，重试次数
        /// </summary>
        public int Tryit
        {
            get;
            set;
        } = 0;
        /// <summary>
        /// 链接名
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        public void Init ()
        {
            if (this.ConString.IsNull() && !this.Connection.IsNull())
            {
                this.ConString = string.Format("{0},defaultDatabase={1},asyncPipeline={2},poolsize={3},idleTimeout={4},connectTimeout={5},syncTimeout={6},preheat={7},autoDispose={8},ssl={9},testcluster={10}&tryit={11},password={12},name={13},writebuffer={14}",
                    this.Connection,
                    this.Database,
                    this.AsyncPipeline,
                    this.PoolSize,
                    this.IdleTimeout,
                    this.ConnectTimeout,
                    this.SyncTimeout,
                    this.Preheat,
                    this.AutoDispose,
                    this.SSL,
                    this.Testcluster,
                    this.Tryit,
                    this.Password,
                    this.Name,
                    this.WriteBuffer
                    );
            }
        }
    }
}
