namespace Wedonek.RpcStore.Service.Model
{
        public class SetMerConfig
        {
                /// <summary>
                /// 是否隔离
                /// </summary>
                public bool IsRegionIsolate
                {
                        get;
                        set;
                }
                /// <summary>
                /// 隔离级别
                /// </summary>
                public bool IsolateLevel
                {
                        get;
                        set;
                }
        }
}
