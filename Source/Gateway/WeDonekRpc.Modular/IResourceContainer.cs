using System;

using WeDonekRpc.ModularModel.Resource.Model;

namespace WeDonekRpc.Modular
{
        public interface IResourceContainer : IDisposable
        {
                void Add(ResourceDatum datum);
        }
}
