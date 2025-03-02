namespace WeDonekRpc.Client.Interface
{
        public interface IServerLimit
        {
                /// <summary>
                /// 是否失效
                /// </summary>
                bool IsInvalid { get; }
                /// <summary>
                /// 是否可用
                /// </summary>
                bool IsUsable { get; }
                /// <summary>
                /// 是否限流
                /// </summary>
                /// <returns></returns>
                bool IsLimit();
                /// <summary>
                /// 重置
                /// </summary>
                void Reset();
                /// <summary>
                /// 刷新限流
                /// </summary>
                /// <param name="time">当前时间</param>
                void Refresh(int time);
        }
}
