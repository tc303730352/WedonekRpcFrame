using System;

namespace RpcSyncService.Model
{
        internal class DictateNode
        {
                public Guid Id
                {
                        get;
                        set;
                }
                public Guid ParentId
                {
                        get;
                        set;
                }

                public string Dictate
                {
                        get;
                        set;
                }

                public bool IsEndpoint
                {
                        get;
                        set;
                }
        }
}
