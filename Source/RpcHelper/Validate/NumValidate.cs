using System;

namespace RpcHelper.Validate
{
        public enum NumFormat
        {
                无 = 0,
                对数 = 1,
                取余 = 2,
                偶数 = 3,
                奇数 = 4
        }
        /// <summary>
        /// 检查数字格式范围
        /// </summary>
        public class NumValidate : ValidateAttr
        {
                private readonly long _MinNum;
                private readonly long _MaxNum;
                private readonly int _BaseNum = 0;
                private readonly NumFormat _Format = NumFormat.无;

                public long MinNum => this._MinNum;
                public long MaxNum => this._MaxNum;
                public int BaseNum => this._BaseNum;
                public NumFormat Format => this._Format;


                /// <summary>
                /// 数字格式检查
                /// </summary>
                /// <param name="error">错误信息</param>
                /// <param name="min">最小值</param>
                /// <param name="max">最大值</param>
                public NumValidate(string error, long min, long max) : base(error, string.Join("_", min, max))
                {
                        this._MinNum = min;
                        this._MaxNum = max;
                }
                /// <summary>
                /// 数字格式检查
                /// </summary>
                /// <param name="error">错误信息</param>
                /// <param name="num">最小值</param>
                public NumValidate(string error, long min) : base(error, string.Format("{0}_0", min))
                {
                        this._MinNum = min;
                        this._MaxNum = long.MaxValue;
                }
                /// <summary>
                /// 数字格式检查
                /// </summary>
                /// <param name="error">错误信息</param>
                /// <param name="baseNum">基数</param>
                /// <param name="maxNum">最大值</param>
                /// <param name="format">数字格式</param>
                public NumValidate(string error, int baseNum, long maxNum, NumFormat format) : base(error, string.Join("_", baseNum, maxNum, (short)format))
                {
                        this._MinNum = baseNum;
                        this._MaxNum = maxNum;
                        this._BaseNum = baseNum;
                        this._Format = format;
                }

                /// <summary>
                /// 数字格式检查
                /// </summary>
                /// <param name="error">错误信息</param>
                /// <param name="minNum">最小值</param>
                /// <param name="maxNum">最大值</param>
                /// <param name="baseNum">基数</param>
                /// <param name="format">数字格式</param>
                public NumValidate(string error, int minNum, long maxNum, int baseNum, NumFormat format) : base(error, string.Join("_", baseNum, maxNum, (short)format))
                {
                        this._MinNum = minNum;
                        this._MaxNum = maxNum;
                        this._BaseNum = baseNum;
                        this._Format = format;
                }


                protected override bool _CheckAttr(object source, Type type, object data)
                {
                        if (data == null)
                        {
                                return false;
                        }
                        return DataValidate.CheckSize(type, data, this);
                }
        }
}

