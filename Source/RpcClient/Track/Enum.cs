namespace RpcClient.Track
{
        public enum TraceType
        {
                Zipkin = 0
        }
        public enum TrackRange
        {
                ALL = 14,
                Gateway = 2,
                RpcMsg = 4,
                RpcQueue = 8
        }
        public enum TrackDepth
        {
                基本 = 0,
                发起的参数 = 2,
                响应的数据 = 4,
                接收的数据 = 8,
                返回的数据 = 16
        }
        public enum TrackStage
        {
                /// <summary>
                /// 客户端发起请求
                /// </summary>
                cs,
                /// <summary>
                ///  服务器接受请求，开始处理
                /// </summary>
                sr,
                /// <summary>
                /// 服务器完成处理，给客户端应答；
                /// </summary>
                ss,
                /// <summary>
                /// 客户端接受应答从服务器
                /// </summary>
                cr
        }
}
