using System;

namespace WeDonekRpc.Helper.Validate
{
    /// <summary>
    /// 空检查(null len)
    /// </summary>
    public class NullValidate : ValidateAttr
    {
        private const int _SortNum = int.MaxValue - 10;
        /// <summary>
        /// 空验证
        /// </summary>
        /// <param name="error">错误信息</param>
        public NullValidate (string error) : base(error, _SortNum)
        {

        }
        private readonly string _TwoAttr = null;
        /// <summary>
        /// 空验证
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="twoAttr">联合空检查的属性名，2个属性需满足其中一个不为空</param>
        public NullValidate (string error, string twoAttr) : base(error, _SortNum)
        {
            this._TwoAttr = twoAttr;
        }

        public override string ToString ()
        {
            if (string.IsNullOrEmpty(this._TwoAttr))
            {
                return null;
            }
            return string.Format("和属性( {0} )联合进行非空检查，2个属性需满足其中一个不为空", this._TwoAttr);
        }
        protected override bool _CheckAttr (object source, Type type, object data)
        {
            if (this._TwoAttr == null)
            {
                return DataValidate.CheckIsNull(type, data);
            }
            else
            {
                object val = Tools.GetObjectAttrVal(source, this._TwoAttr, out Type proType);
                if (proType == null)
                {
                    return DataValidate.CheckIsNull(type, data);
                }
                else
                {
                    return DataValidate.CheckIsNull(proType, val) && DataValidate.CheckIsNull(type, data);
                }
            }
        }
    }
}
