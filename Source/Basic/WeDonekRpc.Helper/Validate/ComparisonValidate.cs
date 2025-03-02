using System;

namespace WeDonekRpc.Helper.Validate
{
    /// <summary>
    /// 比对2个属性的关系
    /// </summary>
    public class ComparisonValidate : ValidateAttr
    {
        private readonly bool _IsEqual = false;

        private readonly string _TwoAttr = null;
        /// <summary>
        /// 计较两个属性值关系
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="twoAttr">比对的属性</param>
        /// <param name="isEqual">是否相等</param>
        public ComparisonValidate (string error, string twoAttr, bool isEqual = false) : base(error)
        {
            this._TwoAttr = twoAttr;
            this._IsEqual = isEqual;
        }

        public override string ToString ()
        {
            return string.Format("和属性( {0} )进行相等关系比较，当前属性需{1}{0}的值", this._TwoAttr, this._IsEqual ? "等于" : "不等于");
        }
        protected override bool _CheckAttr (object source, Type type, object data)
        {
            if (data == null)
            {
                return false;
            }
            object val = Tools.GetObjectAttrVal(source, this._TwoAttr);
            return this._IsEqual ? !data.Equals(val) : data.Equals(val);
        }

    }
}

