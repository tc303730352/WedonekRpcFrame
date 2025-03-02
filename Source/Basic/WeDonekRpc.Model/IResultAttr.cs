using System;

namespace WeDonekRpc.Model
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class IResultAttr : Attribute
    {
        public IResultAttr (string dictate, string sysType)
        {
            this.ApiName = string.Join(".", sysType, dictate).ToLower();
        }
        public string ApiName
        {
            get;
        }
    }
}
