using System;

namespace RpcClient.Model
{
        public class IocBody
        {
                internal IocBody(Type form,Type to,string name)
                {
                        this.Form = form;
                        this.To = to;
                        this.Name = name;
                }
                public Type Form
                {
                        get;
                }
                public Type To
                {
                        get;
                }
                public string Name
                {
                        get;
                        set;
                }
        }
}
