namespace Wedonek.RpcStore.Service.Model
{
        public class QuerySysParam
        {
                public long? RpcMerId
                {
                        get;
                        set;
                }
                public long? ServerId
                {
                        get;
                        set;
                }
                public ConfigRange? Range
                {
                        get;
                        set;
                }
                public long? SystemTypeId
                {
                        get;
                        set;
                }
                public string ConfigName
                {
                        get;
                        set;
                }
        }
}
