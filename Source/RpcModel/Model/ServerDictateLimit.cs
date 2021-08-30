using System;

using RpcHelper.Validate;

namespace RpcModel.Model
{
        /// <summary>
        /// 服务指令限流配置
        /// </summary>
        [Serializable]
        public class ServerDictateLimit
        {
                /// <summary>
                /// 限流指令
                /// </summary>
                [NullValidate("rpc.dictate.null")]
                [LenValidate("rpc.dictate.len", 1, 50)]
                public string Dictate
                {
                        get;
                        set;
                }
                /// <summary>
                /// 限流类型
                /// </summary>
                [EnumValidate("rpc.limit.type.error", typeof(ServerLimitType))]
                [EntrustValidate("_Check")]
                public ServerLimitType LimitType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 最大流量
                /// </summary>
                [NumValidate("rpc.limit.num.error", 0, int.MaxValue)]
                public int LimitNum
                {
                        get;
                        set;
                }
                /// <summary>
                /// 窗口大小（秒）
                /// </summary>
                [NumValidate("rpc.limit.time.error", 0, short.MaxValue)]
                public short LimitTime
                {
                        get;
                        set;
                }


                /// <summary>
                /// 桶大小
                /// </summary>
                [NumValidate("rpc.bucket.size.error", 0, short.MaxValue)]
                public short BucketSize
                {
                        get;
                        set;
                }
                /// <summary>
                /// 桶溢出速度
                /// </summary>
                [NumValidate("rpc.bucket.outNum.error", 0, short.MaxValue)]
                public short BucketOutNum
                {
                        get;
                        set;
                }
                /// <summary>
                /// 令牌最大数
                /// </summary>
                [NumValidate("rpc.token.num.error", 0, short.MaxValue)]
                public short TokenNum
                {
                        get;
                        set;
                }
                /// <summary>
                /// 每秒添加令牌数
                /// </summary>
                [NumValidate("rpc.token.inNum.error", 0, short.MaxValue)]
                public short TokenInNum
                {
                        get;
                        set;
                }
                private static bool _Check(ServerDictateLimit obj, out string error)
                {
                        if (obj.LimitType == ServerLimitType.不启用)
                        {
                                error = null;
                                return true;
                        }
                        else if (obj.LimitType == ServerLimitType.令牌桶)
                        {
                                if (obj.TokenNum == 0)
                                {
                                        error = "rpc.token.num.error";
                                        return false;
                                }
                                else if (obj.TokenInNum == 0)
                                {
                                        error = "rpc.token.inNum.error";
                                        return false;
                                }
                        }
                        else if (obj.LimitType == ServerLimitType.漏桶)
                        {
                                if (obj.BucketSize == 0)
                                {
                                        error = "rpc.bucket.size.error";
                                        return false;
                                }
                                else if (obj.BucketOutNum == 0)
                                {
                                        error = "rpc.bucket.outNum.error";
                                        return false;
                                }
                        }
                        else if (obj.LimitNum == 0)
                        {
                                error = "rpc.limit.num.error";
                                return false;
                        }
                        else if (obj.LimitTime == 0)
                        {
                                error = "rpc.limit.time.error";
                                return false;
                        }
                        error = null;
                        return true;
                }
        }
}
