using System.Collections.Generic;

namespace WeDonekRpc.HttpService.Rewrite
{
    internal class RouteParam
    {
        public RouteParam(Dictionary<string, string> args,string action)
        {
            this.Action = action;
            this.Args = args;
        }
        public RouteParam(string action)
        {
            this.Action = action;
        }
        public string Action
        {
            get;
        }
        public Dictionary<string,string> Args
        {
            get;
        }
    }
}
