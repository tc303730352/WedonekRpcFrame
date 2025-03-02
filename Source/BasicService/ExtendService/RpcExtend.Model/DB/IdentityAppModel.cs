namespace RpcExtend.Model.DB
{
    using System;
    using SqlSugar;

    [SugarTable("IdentityApp")]
    public class IdentityAppModel
    {
        /// <summary>
        /// Ӧ��ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        /// <summary>
        /// Ӧ��ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Ӧ����
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// Ӧ��˵��
        /// </summary>
        public string AppShow { get; set; }

        /// <summary>
        /// Ӧ����չ
        /// </summary>
        [SugarColumn(IsJson = true)]
        public Dictionary<string, string> AppExtend { get; set; }

        /// <summary>
        /// ��Ч��
        /// </summary>
        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// �Ƿ�����
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
