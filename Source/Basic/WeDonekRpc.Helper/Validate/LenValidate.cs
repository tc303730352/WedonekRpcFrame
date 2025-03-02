using System;

namespace WeDonekRpc.Helper.Validate
{
    /// <summary>
    /// 检查长度(字符串,数值和集合)
    /// </summary>
    public class LenValidate : ValidateAttr
    {
        private readonly int _MinLen;
        private readonly int _MaxLen;

        public int MinLen => this._MinLen;

        public int MaxLen => this._MaxLen;

        public bool IsAllowEmpty
        {
            get;
            set;
        }

        public LenValidate (string error, int min, int max) : base(error)
        {
            this._MinLen = min;
            this._MaxLen = max;
            this.IsArray = false;
            this.IsAllowEmpty = true;
        }
        public LenValidate (string error, int min, int max, bool isArray) : base(error)
        {
            this._MinLen = min;
            this._MaxLen = max;
            this.IsArray = isArray;
            this.IsAllowEmpty = true;
        }
        public LenValidate (string error, int len) : base(error)
        {
            this._MinLen = len;
            this._MaxLen = len;
            this.IsArray = false;
            this.IsAllowEmpty = true;
        }
        public LenValidate (string error, int len, bool isEmpty) : base(error)
        {
            this._MinLen = len;
            this._MaxLen = len;
            this.IsArray = false;
            this.IsAllowEmpty = isEmpty;
        }
        protected override bool _CheckAttr (object source, Type type, object data)
        {
            if (data == null)
            {
                return false;
            }
            return DataValidate.CheckLen(type, data, this._MinLen, this._MaxLen, this.IsArray, this.IsAllowEmpty);
        }
    }
}
