using System;

namespace WeDonekRpc.Helper.Validate
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
        private readonly decimal _MinNum;
        private readonly decimal _MaxNum;
        private readonly int _BaseNum = 0;
        private readonly NumFormat _Format = NumFormat.无;

        public decimal MinNum => this._MinNum;
        public decimal MaxNum => this._MaxNum;
        public int BaseNum => this._BaseNum;
        public NumFormat Format => this._Format;


        /// <summary>
        /// 数字格式检查
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public NumValidate (string error, double min, double max) : base(error, string.Join("_", min, max))
        {
            this._MinNum = new decimal(min);
            this._MaxNum = new decimal(max);
        }
        /// <summary>
        /// 数字格式检查
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public NumValidate (string error, int min, int max) : base(error, string.Join("_", min, max))
        {
            this._MinNum = new decimal(min);
            this._MaxNum = new decimal(max);
        }
        /// <summary>
        /// 数字格式检查
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="min">最小值</param>
        public NumValidate (string error, double min) : base(error, string.Format("{0}_0", min))
        {
            this._MinNum = new decimal(min);
            this._MaxNum = decimal.MaxValue;
        }
        /// <summary>
        /// 数字格式检查
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="min">最小值</param>
        public NumValidate (string error, int min) : base(error, string.Format("{0}_0", min))
        {
            this._MinNum = new decimal(min);
            this._MaxNum = decimal.MaxValue;
        }
        /// <summary>
        /// 数字格式检查
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="baseNum">基数</param>
        /// <param name="max">最大值</param>
        /// <param name="format">数字格式</param>
        public NumValidate (string error, int baseNum, double max, NumFormat format) : base(error, string.Join("_", baseNum, max, (short)format))
        {
            this._MinNum = baseNum;
            this._MaxNum = new decimal(max);
            this._BaseNum = baseNum;
            this._Format = format;
        }

        /// <summary>
        /// 数字格式检查
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="baseNum">基数</param>
        /// <param name="format">数字格式</param>
        public NumValidate (string error, double min, double max, int baseNum, NumFormat format) : base(error, string.Join("_", baseNum, max, (short)format))
        {
            this._MinNum = new decimal(min);
            this._MaxNum = new decimal(max);
            this._BaseNum = baseNum;
            this._Format = format;
        }

        /// <summary>
        /// 数字格式检查
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="baseNum">基数</param>
        /// <param name="max">最大值</param>
        /// <param name="format">数字格式</param>
        public NumValidate (string error, int baseNum, int max, NumFormat format) : base(error, string.Join("_", baseNum, max, (short)format))
        {
            this._MinNum = baseNum;
            this._MaxNum = new decimal(max);
            this._BaseNum = baseNum;
            this._Format = format;
        }

        /// <summary>
        /// 数字格式检查
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="baseNum">基数</param>
        /// <param name="format">数字格式</param>
        public NumValidate (string error, int min, int max, int baseNum, NumFormat format) : base(error, string.Join("_", baseNum, max, (short)format))
        {
            this._MinNum = new decimal(min);
            this._MaxNum = new decimal(max);
            this._BaseNum = baseNum;
            this._Format = format;
        }
        protected override bool _CheckAttr (object source, Type type, object data)
        {
            if (data == null)
            {
                return false;
            }
            return DataValidate.CheckSize(type, data, this);
        }
    }
}

