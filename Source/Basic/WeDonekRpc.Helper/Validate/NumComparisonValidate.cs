using System;

namespace WeDonekRpc.Helper.Validate
{
    public enum NumComparisonType
    {
        等于 = 0,
        不等 = 1,
        大于 = 2,
        小于 = 3,
        大于等于 = 4,
        小于等于 = 5
    }
    /// <summary>
    /// 计较两个数字型属性值关系
    /// </summary>
    public class NumComparisonValidate : ValidateAttr
    {
        private readonly NumComparisonType _Type = NumComparisonType.等于;

        private readonly string _TwoAttr = null;
        /// <summary>
        /// 计较两个数字型属性值关系
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="twoAttr">比对的属性</param>
        /// <param name="type">比对方式</param>
        public NumComparisonValidate (string error, string twoAttr, NumComparisonType type) : base(error)
        {
            this._TwoAttr = twoAttr;
            this._Type = type;
        }
        public override string ToString ()
        {
            return string.Format("和属性( {0} )进行比较2个数字之间的关系，当前属性需{1}{0}的值", this._TwoAttr, this._Type.ToString());
        }

        protected override bool _CheckAttr (object source, Type type, object data)
        {
            if (!type.IsValueType || data == null)
            {
                return false;
            }
            object val = Tools.GetObjectAttrVal(source, this._TwoAttr);
            if (val == null)
            {
                return true;
            }
            dynamic num1 = Convert.ToDecimal(data);
            dynamic num2 = Convert.ToDecimal(val);
            if (this._Type == NumComparisonType.等于)
            {
                return num1 != num2;
            }
            else if (this._Type == NumComparisonType.不等)
            {
                return num1 == num2;
            }
            else if (this._Type == NumComparisonType.大于)
            {
                return num1 <= num2;
            }
            else if (this._Type == NumComparisonType.大于等于)
            {
                return num1 < num2;
            }
            else if (this._Type == NumComparisonType.小于)
            {
                return num1 >= num2;
            }
            else
            {
                return num1 > num2;
            }
        }

    }
}
