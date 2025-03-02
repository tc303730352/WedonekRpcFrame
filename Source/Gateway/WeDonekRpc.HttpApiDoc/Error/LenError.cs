using System;
using WeDonekRpc.Helper.Validate;

namespace WeDonekRpc.HttpApiDoc.Error
{
    internal class LenError : ProErrorMsg
    {
        public LenError () { }
        public LenError (LenValidate len) : base(len.ErrorMsg)
        {
            this.LenShow = len.IsArray
                    ? string.Format("数组长度范围：{0} 到 {1} 位", len.MinLen, len.MaxLen)
                    : string.Format("字符串长度范围：{0} 到 {1} 位 (如是数组检查的是数组成员的长度)", len.MinLen, len.MaxLen);
        }
        public LenError (EnumValidate obj, Type type) : base(obj.ErrorMsg)
        {
            this.LenShow = obj.GetValidateShow(type);
        }
        public LenError (NumValidate num) : base(num.ErrorMsg)
        {
            this.LenShow = num.Format == NumFormat.无
                    ? string.Format("数字范围：{0} 到 {1} 。(如是数组检查的是数组成员的值范围)", num.MinNum, num.MaxNum)
                    : string.Format("格式为：{0}，基数为：{1}。(如是数组检查的是数组成员的值范围)", num.Format.ToString(), num.BaseNum);
        }
        public string LenShow
        {
            get;
        }
    }
}
