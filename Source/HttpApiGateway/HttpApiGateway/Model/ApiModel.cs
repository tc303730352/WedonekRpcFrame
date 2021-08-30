using System;
using System.Reflection;

using ApiGateway;
using ApiGateway.Attr;
using ApiGateway.Interface;

namespace HttpApiGateway.Model
{
        internal class ApiModel
        {
                public string ApiUri { get; set; }
                public Type Type
                {
                        get;
                        set;
                }
                public string Prower { get; set; }

                public bool IsAccredit { get; set; }

                public MethodInfo Method
                {
                        get;
                        set;
                }
                public IUpCheck UpCheck { get; set; }
                public ApiType ApiType { get; set; }
        }
}
