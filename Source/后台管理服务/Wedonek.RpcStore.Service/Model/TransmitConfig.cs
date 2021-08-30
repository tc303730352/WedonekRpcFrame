using RpcModel;

namespace Wedonek.RpcStore.Service.Model
{
        [System.Serializable]
        public class TransmitConfig
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
