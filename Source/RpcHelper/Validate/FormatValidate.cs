using System;

namespace RpcHelper.Validate
{
        /// <summary>
        /// 格式校验
        /// </summary>
        public class FormatValidate : ValidateAttr
        {
                private readonly ValidateFormat _FormatType = ValidateFormat.Email;
                /// <summary>
                /// 格式校验
                /// </summary>
                /// <param name="error">错误信息</param>
                /// <param name="format">格式类型</param>
                /// <param name="isMatch">是否取相反的结果</param>
                public FormatValidate(string error, ValidateFormat format, bool isMatch = false) : base(error)
                {
                        this._FormatType = format;
                        this._IsMatching = isMatch;
                }
                public FormatValidate(string error, ValidateFormat format, string str, bool isMatch = false) : base(error)
                {
                        this._FormatType = format;
                        this._ContainStr = str;
                        this._IsMatching = isMatch;
                }
                /// <summary>
                /// 格式校验
                /// </summary>
                /// <param name="error">错误信息</param>
                /// <param name="str">包含特定字符</param>
                /// <param name="isMatch">是否取相反的结果</param>
                public FormatValidate(string error, string str, bool isMatch = false) : base(error)
                {
                        this._FormatType = ValidateFormat.包含特定字符;
                        this._IsMatching = isMatch;
                        this._ContainStr = str;
                }


                private readonly string _ContainStr = null;
                private readonly bool _IsMatching = false;

                public ValidateFormat FormatType => this._FormatType;

                public string ContainStr => this._ContainStr;

                protected override bool _CheckAttr(object source, Type type, object data)
                {
                        if (data == null)
                        {
                                return false;
                        }
                        string str;
                        if (type.Name != DataValidate.StrTypeName)
                        {
                                if (type.Name == PublicDataDic.UriTypeName)
                                {
                                        str = data.ToString();
                                }
                                else
                                {
                                        return false;
                                }
                        }
                        else
                        {
                                str = (string)data;
                        }
                        if (string.IsNullOrEmpty(str))
                        {
                                return false;
                        }
                        bool isValid = DataValidate.CheckString(str, this._FormatType, this._ContainStr);
                        return this._IsMatching ? !isValid : isValid;
                }
        }
}
