using System;
using System.Reflection;

namespace WeDonekRpc.Helper.Validate
{

    internal class EntrustMethod
    {
        private readonly MethodInfo _Method = null;
        private readonly Type _SourceType = null;
        private readonly Type _AttrType = null;
        private readonly int[] _ParamT = null;
        private readonly bool _IsVoid = false;
        private readonly int _ReturnIndex = -1;
        private readonly int _ParamLen = 0;
        public EntrustMethod (MethodInfo method, Type source, Type attrType)
        {
            this._SourceType = source;
            this._AttrType = attrType;
            ParameterInfo[] param = method.GetParameters();
            this._IsVoid = method.ReturnType == PublicDataDic.VoidType;
            this._ParamT = Array.ConvertAll(param, a =>
            {
                if (a.ParameterType.FullName == this._SourceType.FullName)
                {
                    return 0;
                }
                else if (a.ParameterType.FullName == this._AttrType.FullName)
                {
                    return 1;
                }
                else if (a.ParameterType == typeof(Type))
                {
                    return 2;
                }
                else if (a.IsOut)
                {
                    return 3;
                }
                else
                {
                    return 4;
                }
            });
            this._ReturnIndex = Array.FindIndex(this._ParamT, a => a == 3);
            this._Method = method;
            this._ParamLen = param.Length;
        }

        /// <summary>
        /// 检查属性
        /// </summary>
        /// <param name="source">源数据</param>
        /// <param name="type">类型</param>
        /// <param name="data">属性值</param>
        /// <param name="root">包含源数据的上一级对象（单级为空）</param>
        /// <param name="error">错误码</param>
        /// <returns>是否通过验证</returns>
        public bool CheckAttr (object source, Type type, object data, object root, out string error)
        {
            if (this._IsVoid && this._ParamLen == 0)
            {
                error = null;
                _ = this._Method.Invoke(source, null);
                return true;
            }
            else if (this._ParamLen == 0)
            {
                error = null;
                return (bool)this._Method.Invoke(source, null);
            }
            object[] arg = this._ParamT.ConvertAll(a =>
            {
                if (a == 0)
                {
                    return source;
                }
                else if (a == 1)
                {
                    return data;
                }
                else if (a == 2)
                {
                    return type;
                }
                else if (a == 4)
                {
                    return root;
                }
                return null;
            });
            if (this._IsVoid)
            {
                _ = this._Method.Invoke(source, arg);
                error = null;
                return true;
            }
            else if ((bool)this._Method.Invoke(source, arg))
            {
                error = null;
                return true;
            }
            else if (this._ReturnIndex != -1)
            {
                error = (string)arg[this._ReturnIndex];
            }
            else
            {
                error = null;
            }
            return false;
        }

        internal bool CheckAttr (object source, Type type, object data, object root)
        {
            if (this._ParamLen == 0)
            {
                return (bool)this._Method.Invoke(source, null);
            }
            object[] arg = Array.ConvertAll(this._ParamT, a =>
            {
                if (a == 0)
                {
                    return source;
                }
                else if (a == 1)
                {
                    return data;
                }
                else if (a == 2)
                {
                    return type;
                }
                else if (a == 4)
                {
                    return root;
                }
                return null;
            });
            if ((bool)this._Method.Invoke(source, arg))
            {
                return true;
            }
            return false;
        }
    }
    /// <summary>
    /// 委托指定方法验证数据
    /// </summary>
    public class EntrustValidate : ValidateAttr
    {
        private readonly string _CheckFunName = null;
        /// <summary>
        /// 委托验证
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="funcName">方法名</param>
        public EntrustValidate (string error, string funcName) : base(error, int.MaxValue)
        {
            this._CheckFunName = funcName;
            this.IsContinueValidate = true;
        }
        public EntrustValidate (string funcName) : base(string.Empty, int.MaxValue)
        {
            this._CheckFunName = funcName;
            this.IsContinueValidate = true;
        }
        public EntrustValidate (string error, string funcName, int sortNum) : base(error, sortNum)
        {
            this._CheckFunName = funcName;
            this.IsContinueValidate = true;
        }


        public string CheckFunName => this._CheckFunName;


        public override bool CheckAttr (object source, Type type, object data, object root, out string error)
        {
            if (!ValidateMethodCache.GetMethod(source, type, this._CheckFunName, out EntrustMethod method))
            {
                error = null;
                return true;
            }
            else
            {
                return method.CheckAttr(source, type, data, root, out error);
            }
        }

    }
}
