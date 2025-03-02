using System;
using System.ComponentModel.DataAnnotations;

namespace WeDonekRpc.Helper.Validate
{
        /// <summary>
        /// 调用内部方法检查是否验证属性
        /// </summary>
        public class CheckIsValidate : ValidateAttr
        {
                private readonly string _CheckFunName = null;

                /// <summary>
                /// 委托验证
                /// </summary>
                /// <param name="funcName">方法名</param>
                public CheckIsValidate(string funcName) : base(string.Empty, funcName, int.MaxValue)
                {
                        this._CheckFunName = funcName;
                }
                protected override ValidationResult IsValid(object value, ValidationContext context)
                {
                        base.IsContinueValidate = this._CheckAttr(context.ObjectInstance, context.ObjectType.GetProperty(context.DisplayName).PropertyType, value);
                        return ValidationResult.Success;
                }
                protected override bool _CheckAttr(object source, Type type, object data, object root)
                {
                        if (!ValidateMethodCache.GetMethod(source, type, this._CheckFunName, out EntrustMethod method))
                        {
                                return false;
                        }
                        else if (method.CheckAttr(source, type, data, root))
                        {
                                base.IsContinueValidate = true;
                                return false;
                        }
                        else
                        {
                                base.IsContinueValidate = false;
                                return false;
                        }
                }
        }
}
