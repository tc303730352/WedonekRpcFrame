using System;
using System.Reflection;
using WeDonekRpc.WebSocketGateway.Interface;

namespace WeDonekRpc.WebSocketGateway.Model
{
    internal class ApiModel : IApiModel
    {
        public Type Type
        {
            get;
            set;
        }
        public MethodInfo Method
        {
            get;
            set;
        }
        public string[] Prower { get; set; }
        public bool IsAccredit { get; set; }
        public string LocalPath { get; set; }
        public Type ApiEventType { get; set; }
        public bool IsEnable { get; set; }
    }
}
