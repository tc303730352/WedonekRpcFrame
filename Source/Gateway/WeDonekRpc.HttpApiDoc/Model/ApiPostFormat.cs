using System;
using System.Collections.Generic;
using System.Reflection;

using WeDonekRpc.HttpApiDoc.Error;
using WeDonekRpc.HttpApiDoc.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Validate;

namespace WeDonekRpc.HttpApiDoc.Model
{
    internal class ApiPostFormat : ApiDataFormat, IApiPostFormat
    {

        public NullError NullCheck { get; set; } = new NullError();


        public List<LenError> LenFormat { get; set; } = [];

        public List<FormatError> DataFormat { get; set; } = [];

        public List<OtherError> OtherFormat { get; set; } = [];

        private static readonly Type _IValidateAttr = typeof(IValidateAttr);
        public void InitField (FieldInfo field)
        {
            object[] attrs = field.GetCustomAttributes(_IValidateAttr, false);
            this._Init(attrs, field.FieldType);
        }
        private void _Init (object[] attrs, Type type)
        {
            if (attrs.Length == 0)
            {
                return;
            }
            attrs.ForEach(a =>
           {
               if (a is NullValidate i)
               {
                   this.NullCheck = new NullError(i.ErrorMsg, i.ToString());
               }
               else if (a is NumValidate num)
               {
                   this.LenFormat.Add(new LenError(num));
               }
               else if (a is LenValidate len)
               {
                   this.LenFormat.Add(new LenError(len));
               }
               else if (a is EnumValidate enumValidate)
               {
                   this.LenFormat.Add(new LenError(enumValidate, type));
               }
               else if (a is FormatValidate format)
               {
                   this.DataFormat.Add(new FormatError(format));
               }
               else if (a is YearValidate year)
               {
                   this.DataFormat.Add(new FormatError(year));
               }
               else if (a is TimeValidate time)
               {
                   this.DataFormat.Add(new FormatError(time));
               }
               else if (a is RegexValidate regex)
               {
                   this.DataFormat.Add(new FormatError(regex));
               }
               else if (a is AreaValidate area)
               {
                   this.DataFormat.Add(new FormatError(area));
               }
               else if (a is EntrustValidate entrust)
               {

               }
               else
               {
                   ValidateAttr attr = (ValidateAttr)a;
                   this.OtherFormat.Add(new OtherError(attr.ErrorMsg, attr.ToString()));
               }
           });
            if (this.NullCheck == null)
            {
                this.NullCheck = new NullError();
            }
        }
        public void InitPro (PropertyInfo pro)
        {
            object[] attrs = pro.GetCustomAttributes(_IValidateAttr, false);
            this._Init(attrs, pro.PropertyType);
        }

        internal void Init (IValidateAttr[] attrs, Type type)
        {
            this._Init(attrs, type);
        }
    }
}
