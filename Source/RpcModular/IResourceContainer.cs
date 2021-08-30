using System;

using RpcModularModel.Resource.Model;

namespace RpcModular
{
        public interface IResourceContainer : IDisposable
        {
                void Add(ResourceDatum datum);
        }
}
