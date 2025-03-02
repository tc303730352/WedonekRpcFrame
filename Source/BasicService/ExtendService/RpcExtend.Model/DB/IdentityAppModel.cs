namespace RpcExtend.Model.DB
{
    using System;
    using SqlSugar;

    [SugarTable("IdentityApp")]
    public class IdentityAppModel
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 应用名
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 应用说明
        /// </summary>
        public string AppShow { get; set; }

        /// <summary>
        /// 应用扩展
        /// </summary>
        [SugarColumn(IsJson = true)]
        public Dictionary<string, string> AppExtend { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
