namespace WeDonekRpc.Model.Msg
{
        /// <summary>
        /// 刷新指令节点
        /// </summary>
        [IRemoteBroadcast("RefreshDictateNode", false, "sys.sync", IsCrossGroup = true)]
        public class RefreshDictateNode
        {
        }
}
