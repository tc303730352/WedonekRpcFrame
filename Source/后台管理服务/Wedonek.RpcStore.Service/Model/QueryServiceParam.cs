namespace Wedonek.RpcStore.Service.Model
{
        public class QueryServiceParam
        {
                /// <summary>
                /// 服务组Id
                /// </summary>
                public long GroupId
                {
                        get;
                        set;
                }
                public long SystemTypeId
                {
                        get;
                        set;
                }
                public string ServiceName
                {
                        get;
                        set;
                }
                public string ServiceMac
                {
                        get;
                        set;
                }
                public bool? IsOnline
                {
                        get;
                        set;
                }
        }
}
