using RpcModel;

namespace RpcService.Model
{
        [System.Serializable]
        internal class TransmitConfig
        {
                public TransmitType TransmitType
                {
                        get;
                        set;
                }

                public TransmitRange[] Range
                {
                        get;
                        set;
                }
                public string Value { get; set; }
                public int TransmitId { get; set; }
        }
}
