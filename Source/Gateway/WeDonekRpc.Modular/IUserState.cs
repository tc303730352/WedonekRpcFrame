using System;
using System.Text.Json.Serialization;

namespace WeDonekRpc.Modular
{
    public interface IUserState
    {
        /// <summary>
        /// 授权码
        /// </summary>
        [JsonIgnore]
        string AccreditId { get; }
        /// <summary>
        /// 创建系统类别
        /// </summary>
        [JsonIgnore]
        string SysGroup { get; }
        /// <summary>
        /// 所属集群
        /// </summary>
        [JsonIgnore]
        long RpcMerId { get; }

        /// <summary>
        /// 索引自定义属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <returns>属性值</returns>
        object this[string name]
        {
            get;
            set;
        }
        /// <summary>
        /// 权限值列表
        /// </summary>
        string[] Prower
        {
            get;
        }
        /// <summary>
        /// 设置权限
        /// </summary>
        /// <param name="prowers"></param>
        void SetPrower (string[] prowers);
        /// <summary>
        /// 检查授权
        /// </summary>
        /// <param name="prower">权限值</param>
        /// <returns>是否授权</returns>
        bool CheckPrower (string[] prower);

        bool CheckPrower (string prower);
        /// <summary>
        /// 获取自定义属性值
        /// </summary>
        /// <typeparam name="T">属性值类型</typeparam>
        /// <param name="name">属性名</param>
        /// <returns>属性值</returns>
        T GetValue<T> (string name);
        /// <summary>
        /// 保存当前状态
        /// </summary>
        /// <param name="upFun">比对原始状态值</param>
        /// <returns>最新状态值</returns>
        IUserState SaveState (Func<IUserState, IUserState, IUserState> upFun);
        /// <summary>
        /// 保存状态
        /// </summary>
        /// <returns>保存是否成功</returns>
        bool SaveState ();

        /// <summary>
        /// 取消授权
        /// </summary>
        void Cancel ();
        /// <summary>
        /// 转JSON
        /// </summary>
        /// <returns></returns>
        string ToJson ();
    }
}
