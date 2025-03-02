using System;

using WeDonekRpc.ApiGateway.Model;

namespace WeDonekRpc.ApiGateway.Interface
{
        public interface IApiDocModular : IModular
        {
                string GetApiShow(string path, GatewayType type);
                string GetApiShow(Uri uri);
                void RegModular(string name, Type source, Uri root);
                void RegApi(ApiFuncBody api);
        }
}
