using System;

using RpcModel;

namespace RpcSyncService.Model
{
        [System.Serializable]
        internal class TransactionDatum
        {
                public Guid Id
                {
                        get;
                        set;
                }
                public Guid MainTranId
                {
                        get;
                        set;
                }
                public Guid ParentId
                {
                        get;
                        set;
                }
                public long RpcMerId
                {
                        get;
                        set;
                }
                public long ServerId
                {
                        get;
                        set;
                }
                public string SystemType
                {
                        get;
                        set;
                }
                public int RegionId
                {
                        get;
                        set;
                }
                public string TranName
                {
                        get;
                        set;
                }
                public string SubmitJson
                {
                        get;
                        set;
                }
                public string Extend
                {
                        get;
                        set;
                }
                public TransactionStatus TranStatus
                {
                        get;
                        set;
                }
                public bool IsRegTran { get; set; }
                public bool IsEnd { get; set; }
                public short RetryNum { get; set; }

                public long ErrorCode { get; set; }

                public DateTime? EndTime { get; set; }
        }
}
