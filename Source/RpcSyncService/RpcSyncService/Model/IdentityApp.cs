using System;

namespace RpcSyncService.Model
{
        public class IdentityApp
        {

                public string AppName
                {
                        get;
                        set;
                }

                public DateTime EffectiveDate
                {
                        get;
                        set;
                }
                public bool IsEnable
                {
                        get;
                        set;
                }
        }
}
