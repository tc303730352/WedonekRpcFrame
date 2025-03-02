using RpcStore.RemoteModel.TransmitScheme.Model;

namespace RpcStore.Model.TransmitScheme
{
    public class TransmitDto
    {
        /// <summary>
        /// 服务节点组别编号
        /// </summary>
        public string ServerCode { get; set; }
        /// <summary>
        /// 负载配置
        /// </summary>
        [SqlSugar.SugarColumn(IsJson = true)]
        public TransmitConfig[] TransmitConfig
        {
            get;
            set;
        }
    }
}
