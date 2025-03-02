using System;
using System.Collections.Concurrent;
using System.Linq;

namespace WeDonekRpc.Helper.Validate
{
    public class EnumFormat
    {
        public EnumFormat (int[] vals)
        {
            this.Values = vals;
            this.Min = vals.Min();
            this.Max = vals.Max();
            this.Sum = vals.Sum(a => a % 2 != 0 ? 0 : a);
        }
        public int[] Values
        {
            get;
        }
        public int Min
        {
            get;
        }
        public int Max
        {
            get;
        }
        public int Sum
        {
            get;
        }
        public override string ToString ()
        {
            return string.Format("数值范围：{0} ,最大值：{1}，最小值：{2}。", string.Join(",", this.Values), this.Max, this.Min);
        }
        public bool CheckEnum (int val, bool isContain)
        {
            if (isContain && val != 0)
            {
                return val <= this.Sum && val >= this.Min && this.Values.IsExists(a => a != 0 && ( a & val ) == a);
            }
            return this.Values.IsExists(a => a == val);
        }
    }
    /// <summary>
    /// 枚举验证
    /// </summary>
    public class EnumValidate : ValidateAttr
    {
        private static readonly ConcurrentDictionary<string, EnumFormat> _EnumDic = new ConcurrentDictionary<string, EnumFormat>();

        private static string _AddEnum (Type type)
        {
            string name = type.FullName;
            if (!_EnumDic.ContainsKey(name))
            {
                int[] vals = EnumHelper.GetValues(type).ToArray<int>();
                _ = _EnumDic.TryAdd(name, new EnumFormat(vals));
            }
            return name;
        }

        private static bool _CheckEnumVal (string name, int val, bool isContain)
        {
            if (_EnumDic.TryGetValue(name, out EnumFormat obj))
            {
                return obj.CheckEnum(val, isContain);
            }
            return false;
        }
        /// <summary>
        /// 枚举格式检查
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="type">枚举类型</param>
        /// <param name="isContain">是否允许 | 运算</param>
        public EnumValidate (string error, Type type, bool isContain = false) : base(error, type.FullName)
        {
            this.IsContain = isContain;
            this._EnumName = _AddEnum(type);
        }

        /// <summary>
        /// 枚举格式检查
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="type">枚举类型</param>
        /// <param name="filterNum">过滤掉的值</param>
        public EnumValidate (string error, Type type, params int[] filterNum) : base(error, type.FullName)
        {
            this._Filter = filterNum;
            this._EnumName = _AddEnum(type);
        }
        public EnumValidate (string error, params int[] filterNum) : base(error)
        {
            this._Filter = filterNum;
        }
        /// <summary>
        /// 数字格式检查
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="isContain">是否取余</param>
        public EnumValidate (string error, bool isContain = false) : base(error)
        {
            this.IsContain = isContain;
        }
        private readonly int[] _Filter = null;
        private string _EnumName = null;
        /// <summary>
        /// 是否取余
        /// </summary>
        public bool IsContain { get; set; }


        protected override bool _CheckAttr (object source, Type type, object data)
        {
            if (data == null)
            {
                return false;
            }
            else if (this._EnumName == null)
            {
                this._EnumName = _AddEnum(type);
            }
            int val = (int)data;
            if (this._Filter != null && Array.FindIndex(this._Filter, a => a == val) != -1)
            {
                return true;
            }
            return !_CheckEnumVal(this._EnumName, val, this.IsContain);
        }
        public string GetValidateShow (Type type)
        {
            if (string.IsNullOrEmpty(this._EnumName))
            {
                this._EnumName = _AddEnum(type);
            }
            return this.ToString();
        }
        public override string ToString ()
        {
            if (_EnumDic.TryGetValue(this._EnumName, out EnumFormat obj))
            {
                if (this.IsContain)
                {
                    return string.Format("{0}是否支持取余：是，取余范围:2-{1}， 排除的数字：{2}。", obj.ToString(), obj.Sum, this._Filter.Join(","));
                }
                return string.Format("{0}是否支持取余：否，排除的数字：{1}。", obj.ToString(), this._Filter.Join(","));
            }
            return null;
        }
    }
}
