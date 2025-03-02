using System;
using System.Text.RegularExpressions;

namespace WeDonekRpc.Helper.Validate
{
    /// <summary>
    /// 正则表达式验证
    /// </summary>
    public class RegexValidate : ValidateAttr
    {
        /// <summary>
        /// 是否允许空值 默认：允许
        /// </summary>
        public bool IsAllowEmpty
        {
            get;
            set;
        } = true;
        private readonly Regex _Regex = null;
        /// <summary>
        /// 正则表达式验证
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="regexStr">正则表达式</param>
        /// <param name="isMatch">结果是否取反</param>
        public RegexValidate (string error, string regexStr, bool isMatch = false) : base(error)
        {
            this._Regex = new Regex(regexStr, RegexOptions.IgnoreCase);
            this._IsMatching = isMatch;
        }
        /// <summary>
        /// 正则表达式验证
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="regexStr">正则表达式</param>
        /// <param name="options">提供用于设置正则表达式选项的枚举值</param>
        /// <param name="isMatch">结果是否取反</param>
        public RegexValidate (string error, string regexStr, RegexOptions options, bool isMatch = false) : base(error)
        {
            this._Regex = new Regex(regexStr, options);
            this._IsMatching = isMatch;
        }

        private readonly bool _IsMatching = false;

        public Regex Regex => this._Regex;

        protected override bool _CheckAttr (object source, Type type, object data)
        {
            if (data == null)
            {
                return false;
            }
            bool isValid = DataValidate.CheckString(type, data, this._Regex, this.IsAllowEmpty);
            return this._IsMatching ? !isValid : isValid;
        }
    }
}
