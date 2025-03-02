using System;

namespace WeDonekRpc.Helper.Validate
{
        public interface IValidateAttr : IComparable<IValidateAttr>
        {
                /// <summary>
                /// 是否检查数组内数据
                /// </summary>
                bool IsArray { get; }
                string AttrName { get; }
                int SortNum { get; }

                string ErrorMsg { get; }

                bool RequiresValidationContext { get; }
                bool CheckAttr(object source, Type type, object data, object root, out string error);


        }
}