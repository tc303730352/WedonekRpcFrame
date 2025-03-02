using System;

namespace WeDonekRpc.Helper.Validate
{
    /// <summary>
    /// 检查年份
    /// </summary>
    public class YearValidate : ValidateAttr
    {
        private readonly int _MinYear = 0;
        private readonly int _MaxYear = 0;

        public int MinYear => this._MinYear;

        public int MaxYear => this._MaxYear;

        public YearValidate (string error, int min, int max) : base(error)
        {
            this._MinYear = min;
            this._MaxYear = max;
        }
        public YearValidate (string error, int min) : base(error)
        {
            this._MinYear = min;
            this._MaxYear = int.MaxValue;
        }

        protected override bool _CheckAttr (object source, Type type, object data)
        {
            if (data == null)
            {
                return false;
            }
            int year;
            if (type.Name == PublicDataDic.IntTypeName)
            {
                year = (int)data;
            }
            else if (type.Name == PublicDataDic.StringTypeName && !int.TryParse((string)data, out _))
            {
                return false;
            }
            else
            {
                year = (int)Convert.ChangeType(data, typeof(int));
            }
            return year < this._MinYear || year > this._MaxYear;
        }
    }
}
