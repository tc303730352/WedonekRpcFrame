using System.Collections.Generic;

using WeDonekRpc.ModularModel.Resource;
using WeDonekRpc.ModularModel.Resource.Model;

namespace WeDonekRpc.Modular.Service
{
    internal class ResourceContainer : IResourceContainer
    {
        public ResourceContainer(string name, ResourceType type)
        {
            this._Name = name;
            this._ResourceType = type;
        }
        private readonly string _Name = null;
        private readonly List<ResourceDatum> _ResourceList = new List<ResourceDatum>();
        private readonly ResourceType _ResourceType = ResourceType.API接口;
        private void _Commit()
        {
            if (this._ResourceList.Count > 0)
            {
                ResourceService.Submit(this._Name, this._ResourceType, this._ResourceList.ToArray());
                this._ResourceList.Clear();
            }
        }

        public void Add(ResourceDatum datum)
        {
            this._ResourceList.Add(datum);
        }

        public void Dispose()
        {
            this._Commit();
        }
    }
}
