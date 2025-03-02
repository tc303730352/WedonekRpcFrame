using System;

namespace WeDonekRpc.Helper.Validate
{
        /// <summary>
        /// 比较两个时间的关系
        /// </summary>
        public class TimeComparisonValidate : ValidateAttr
        {
                private readonly NumComparisonType _Type = NumComparisonType.等于;

                private readonly string _TwoAttr = null;
                /// <summary>
                /// 计较两个数字型属性值关系
                /// </summary>
                /// <param name="error">错误信息</param>
                /// <param name="twoAttr">比对的属性</param>
                /// <param name="type">比对方式</param>
                public TimeComparisonValidate(string error, string twoAttr, NumComparisonType type) : base(error)
                {
                        this._TwoAttr = twoAttr;
                        this._Type = type;
                }
                public override string ToString()
                {
                        return string.Format("和属性( {0} )进行比较2个时间类型的关系，当前属性需{1}{0}的值", this._TwoAttr, this._Type.ToString());
                }

                private static long _GetDateTime(Type type, object data)
                {
                        if (type.Name == PublicDataDic.LongTypeName)
                        {
                                return (long)data;
                        }
                        else if (type.Name == PublicDataDic.StringTypeName)
                        {
                                return Tools.GetTimeSpan(DateTime.Parse((string)data));
                        }
                        else
                        {
                                return Tools.GetTimeSpan((DateTime)data);
                        }
                }
                protected override bool _CheckAttr(object source, Type type, object data)
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
                        long num1 = _GetDateTime(type, data);
                        long num2 = _GetDateTime(type, val);
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

