using System;
using System.Collections.Generic;

namespace WeDonekRpc.HttpApiDoc.Model
{
        internal class ApiGroupList
        {
                public int Id
                {
                        get;
                        set;
                }
                public string Name
                {
                        get;
                        set;
                }
                public Uri Root { get; set; }
                public List<ApiModel> ApiList
                {
                        get;
                        set;
                } = new List<ApiModel>();
        }
}
