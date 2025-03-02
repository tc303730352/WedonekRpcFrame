using System;
using System.ComponentModel.DataAnnotations;
namespace WeDonekRpc.Helper.Validate
{
    [AttributeUsage((AttributeTargets)2440, AllowMultiple = true)]
    public class ValidateAttr : ValidationAttribute, IValidateAttr
    {
        public ValidateAttr(string error, string key, int sortNum = 10)
        {
            this._ErrorMsg = error;
            this.AttrName = Tools.GetMD5(string.Join("_", this.GetType().Name, error, key));
            this.SortNum = sortNum;
            this.IsArray = true;
        }
        public ValidateAttr(string error, int sortNum = 10)
        {
            this._ErrorMsg = error;
            this.AttrName = Tools.GetMD5(string.Join("_", this.GetType().Name, error));
            this.SortNum = sortNum;
            this.IsArray = true;
        }

        private readonly string _ErrorMsg = null;
        public bool IsArray { get; set; }
        public string AttrName
        {
            get;
        }

        /// <summary>
        /// 排序位
        /// </summary>
        public int SortNum { get; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg => this._ErrorMsg;

        protected new string ErrorMessageString => this._ErrorMsg;
        public new string ErrorMessage => this._ErrorMsg;

        private volatile bool _IsContinueValidate = true;

        public int CompareTo(IValidateAttr other)
        {
            return other.SortNum.CompareTo(this.SortNum);
        }

        protected virtual bool _CheckAttr(object source, Type type, object data)
        {
            return true;
        }
        protected virtual bool _CheckAttr(object source, Type type, object data, object root)
        {
            return this._CheckAttr(source, type, data);
        }
        public virtual bool CheckAttr(object source, Type type, object data, object root, out string error)
        {
            if (this._CheckAttr(source, type, data, root))
            {
                error = this.ErrorMsg;
                return false;
            }
            error = null;
            return true;
        }
        public override string FormatErrorMessage(string name)
        {
            return this.ErrorMessage;
        }

        public override bool RequiresValidationContext => this._IsContinueValidate;

        protected bool IsContinueValidate { get => this._IsContinueValidate; set => this._IsContinueValidate = value; }


        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            object obj = context.ObjectInstance;
            if (this.CheckAttr(obj, context.ObjectType.GetProperty(context.DisplayName).PropertyType, value, obj, out string error))
            {
                return ValidationResult.Success;
            }
            return new ApiValidationResult(error);
        }
        public override bool IsValid(object value)
        {
            return base.IsValid(value);
        }
    }
}
