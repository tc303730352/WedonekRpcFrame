using System;

namespace RpcModel.Tran
{
        [IRemoteConfig("ApplyTran", "sys.sync", true, true)]
        public class ApplyTran
        {
                /// <summary>
                /// 事务Id
                /// </summary>
                public Guid TranId
                {
                        get;
                        set;
                }
                public Guid? MainTranId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 事务名
                /// </summary>
                public string TranName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 是否为注册的事务
                /// </summary>
                public bool IsReg
                {
                        get;
                        set;
                }
                /// <summary>
                /// 级别
                /// </summary>
                public TranLevel Level
                {
                        get;
                        set;
                }
                /// <summary>
                /// 提交的数据
                /// </summary>
                public string SubmitJson
                {
                        get;
                        set;
                }
                /// <summary>
                /// 过期时间
                /// </summary>
                public int OverTime
                {
                        get;
                        set;
                }
        }
}
