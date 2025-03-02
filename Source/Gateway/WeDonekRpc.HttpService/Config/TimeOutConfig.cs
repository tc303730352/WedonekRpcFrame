using System;

namespace WeDonekRpc.HttpService.Config
{
    public class TimeOutConfig
    {
        /// <summary>
        /// 设置允许在保持的连接上侦听完实体正文的时间
        /// </summary>
        public TimeSpan DrainEntityBody { get; set; } = new TimeSpan(0, 1, 0);
        /// <summary>
        ///设置允许请求实体正文到达的时间。
        /// </summary>
        public TimeSpan EntityBody { get; set; } = new TimeSpan(0, 1, 0);
        /// <summary>
        /// 设置允许分析请求标头的时间。
        /// </summary>
        public TimeSpan HeaderWait { get; set; } = new TimeSpan(0, 1, 0);

        /// <summary>
        /// 设置允许空闲连接的时间。
        /// </summary>
        public TimeSpan IdleConnection { get; set; } = new TimeSpan(0, 1, 0);

        /// <summary>
        /// 设置响应的最低发送速率（以每秒字节数为单位）。
        /// </summary>
        public long MinSendBytesPerSecond { get; set; } = 150;
        /// <summary>
        /// 设置在选取请求前允许请求在请求队列中停留的时间
        /// </summary>
        public TimeSpan RequestQueue { get; set; } = new TimeSpan(0, 1, 0);
    }
}
