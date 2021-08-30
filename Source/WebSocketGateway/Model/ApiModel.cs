using System;
using System.Reflection;

namespace WebSocketGateway.Model
{
        internal class ApiModel
        {
                public Type Type { get;  set; }
                public MethodInfo Method { get;  set; }
                public string Prower { get;  set; }
                public bool IsAccredit { get;  set; }
                public string LocalPath { get;  set; }
        }
}
