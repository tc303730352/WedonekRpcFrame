using WeDonekRpc.Helper;
using System;
using System.Reflection;
using System.Text;

namespace WeDonekRpc.Model
{
    public class RemoteSet
    {
        #region 常量
        private static readonly Type _LockAttr = typeof(RemoteLockAttr);
        private static readonly Type _AppIdentityType = typeof(AppIdentityAttr);
        private static readonly Type _TransmitAttr = typeof(TransmitColumn);
        #endregion

        #region 私有变量
        /// <summary>
        /// 参与事务的成员
        /// </summary>
        private PropertyInfo _TranProperty;
        /// <summary>
        /// 参与远程锁ID生成的成员
        /// </summary>
        private PropertyInfo[] _LockProperty;
        /// <summary>
        /// 标识负载均衡计算数据是否为动态类型
        /// </summary>
        private bool _IsDynamic = false;
        /// <summary>
        /// 标识负载均衡计算数据是否为GUID
        /// </summary>
        private bool _IsGuidType = false;
        /// <summary>
        /// 应用身份属性成员
        /// </summary>
        private PropertyInfo _AppProperty;
        #endregion

        #region 构造函数

        /// <summary>
        /// RPC配置
        /// </summary>
        public RemoteSet ()
        {
        }

        #endregion


        /// <summary>
        /// 负载均衡的计算方式
        /// </summary>
        public TransmitType TransmitType
        {
            get;
            set;
        }
        public bool IsProhibitTrace
        {
            get;
            set;
        }
        /// <summary>
        /// 标识列(计算负载均衡用,计算zoneIndex,hashcode的列名）
        /// </summary>
        public string IdentityColumn { get; set; }
        /// <summary>
        /// 应用标识列
        /// </summary>
        public string AppIdentity { get; set; }
        /// <summary>
        /// 是否同步启动同步锁(解决客户端重复提交问题)
        /// </summary>
        public bool IsEnableLock { get; set; } = false;
        /// <summary>
        /// 锁定用提供标识的列名(用于生成锁的唯一标识)
        /// </summary>
        public string[] LockColumn { get; set; }
        /// <summary>
        /// 是否即刻重置锁状态
        /// </summary>
        public RemoteLockType LockType { get; set; } = RemoteLockType.同步锁;

        /// <summary>
        /// 计算ZIndex的值
        /// </summary>
        public int[] ZIndexBit
        {
            get;
            set;
        }
        /// <summary>
        /// 负载均衡方案
        /// </summary>
        public string Transmit { get; set; }


        /// <summary>
        /// 是否允许重试
        /// </summary>
        public bool IsAllowRetry { get; set; } = true;

        public int? RetryNum { get; set; }
        public int? TimeOut { get; set; }

        public void InitConfig (int retryNum)
        {
            if (!this.RetryNum.HasValue)
            {
                this.RetryNum = retryNum;
            }
            this._IsDynamic = true;
        }
        /// <summary>
        /// 初始化配置
        /// </summary>
        /// <param name="type"></param>
        public void InitConfig (Type type, int retryNum)
        {
            if (!this.RetryNum.HasValue)
            {
                this.RetryNum = retryNum;
            }
            PropertyInfo[] pros = type.GetProperties();
            this._InitTransmit(pros);
            this._InitLock(pros);
            this._InitAppIdentity(pros);
        }
        /// <summary>
        /// 获取请求标识
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void SetAppIdentity (object model, string appId)
        {
            if (this.AppIdentity.IsNull())
            {
                return;
            }
            if (this._IsDynamic)
            {
                this._SetAppIdentity(model, appId);
            }
            else
            {
                this._AppProperty?.SetValue(model, appId);
            }
        }

        /// <summary>
        /// 获取请求标识
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public object GetIdentityVal<T> (T model)
        {
            if (this._IsDynamic)
            {
                return this._GetIdentityVal(model);
            }
            else if (this._TranProperty != null)
            {
                return this._TranProperty.GetValue(model);
            }
            return null;
        }
        /// <summary>
        /// 获取锁ID
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public string GetLockIdVal (DynamicModel dic)
        {
            if (this.LockColumn.Length == 1)
            {
                return dic[this.LockColumn[0]].ToString();
            }
            else
            {
                StringBuilder str = new StringBuilder();
                foreach (string i in this.LockColumn)
                {
                    _ = str.AppendFormat("_{0}", dic[i]);
                }
                _ = str.Remove(0, 1);
                return str.ToString();
            }
        }
        /// <summary>
        /// 获取请求的锁标识
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public string GetLockIdVal<T> (T model)
        {
            if (this._LockProperty.Length == 1)
            {
                return this._LockProperty[0].GetValue(model).ToString();
            }
            else
            {
                StringBuilder str = new StringBuilder();
                foreach (PropertyInfo i in this._LockProperty)
                {
                    _ = str.AppendFormat("_{0}", i.GetValue(model));
                }
                _ = str.Remove(0, 1);
                return str.ToString();
            }
        }

        #region 私有方法
        private void _InitTransmit (PropertyInfo[] pros)
        {
            if (!string.IsNullOrEmpty(this.IdentityColumn))
            {
                this._TranProperty = pros.Find(a => a.Name == this.IdentityColumn);
            }
            else
            {
                foreach (PropertyInfo i in pros)
                {
                    Attribute attr = i.GetCustomAttribute(_TransmitAttr);
                    if (attr != null)
                    {
                        TransmitColumn t = (TransmitColumn)attr;
                        this.TransmitType = t.TransmitType;
                        this.IdentityColumn = i.Name;
                        this.ZIndexBit = t.ZIndexBit;
                        if (t.Transmit != null)
                        {
                            this.Transmit = t.Transmit;
                        }
                        this._TranProperty = i;
                        this._IsGuidType = i.PropertyType == PublicDataDic.GuidType;
                        break;
                    }
                }
            }
            if (this._TranProperty != null && !this._TranProperty.PropertyType.IsValueType && this.TransmitType == TransmitType.Number)
            {
                this._TranProperty = null;
            }
        }

        private void _InitLock (PropertyInfo[] pros)
        {
            if (this.LockColumn != null && this.LockColumn.Length != 0)
            {
                this._LockProperty = pros.FindAll(a => this.LockColumn.IsExists(a.Name));
                this.IsEnableLock = true;
            }
            else
            {
                this._LockProperty = pros.FindAll(a => a.GetCustomAttribute(_LockAttr) != null);
                if (this._LockProperty.Length > 0)
                {
                    this.LockColumn = this._LockProperty.ConvertAll(a => a.Name);
                    this.IsEnableLock = true;
                }
            }
        }
        private object _GetIdentityVal (dynamic model)
        {
            return !string.IsNullOrEmpty(this.IdentityColumn) ? model[this.IdentityColumn] : null;
        }
        private void _SetAppIdentity (dynamic model, string appid)
        {
            model[this.IdentityColumn] = appid;
        }


        private void _InitAppIdentity (PropertyInfo[] pros)
        {
            if (!string.IsNullOrEmpty(this.AppIdentity))
            {
                this._AppProperty = pros.Find(a => a.Name == this.AppIdentity);
            }
            else
            {
                foreach (PropertyInfo i in pros)
                {
                    Attribute attr = i.GetCustomAttribute(_AppIdentityType);
                    if (attr != null)
                    {
                        this._AppProperty = i;
                        this.AppIdentity = i.Name;
                        break;
                    }
                }
            }
        }
        #endregion

    }
}
