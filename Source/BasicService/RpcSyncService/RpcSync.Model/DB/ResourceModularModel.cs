namespace RpcSync.Model.DB
{
    using System;
    using SqlSugar;
    using WeDonekRpc.ModularModel.Resource;

    [SugarTable("ResourceModular")]
    public class ResourceModularModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        public string ModularKey { get; set; }

        public long RpcMerId { get; set; }

        public string SystemType { get; set; }

        public string ModularName { get; set; }

        public string Remark { get; set; }

        public ResourceType ResourceType { get; set; }

        public DateTime AddTime { get; set; }
    }
}
