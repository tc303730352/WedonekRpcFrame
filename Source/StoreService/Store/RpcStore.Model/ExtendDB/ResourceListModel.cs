namespace RpcStore.Model.ExtendDB
{
    using System;
    using RpcStore.RemoteModel;
    using SqlSugar;

    [SugarTable("ResourceList")]
    public class ResourceListModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        public long ModularId { get; set; }

        public string ResourcePath { get; set; }

        public string FullPath { get; set; }

        public string ResourceShow { get; set; }

        public ResourceState ResourceState { get; set; }

        public int VerNum { get; set; }

        public int ResourceVer { get; set; }

        public DateTime? LastTime { get; set; }
        public DateTime AddTime { get; set; }
    }
}
