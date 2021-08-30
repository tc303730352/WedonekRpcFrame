using RpcModel;

namespace RpcService.Model
{
        internal class SystemTypeDatum
        {
                public long Id
                {
                        get;
                        set;
                }

                public long GroupId
                {
                        get;
                        set;
                }
                public BalancedType BalancedType { get; set; }
        }
}
