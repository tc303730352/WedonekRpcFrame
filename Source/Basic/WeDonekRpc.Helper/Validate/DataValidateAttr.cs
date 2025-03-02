using System;

namespace WeDonekRpc.Helper.Validate
{
        /// <summary>
        /// 指定外部方法进行验证操作
        /// </summary>
        public class DataValidateAttr : ValidateAttr
        {
                public DataValidateAttr(string error, string routeName) : base(error, 0)
                {
                        this.RouteName = routeName;
                        this.IsArray = false;
                }


                public string RouteName { get; }

                protected override bool _CheckAttr(object source, Type type, object data, object root)
                {
                        if (data == null)
                        {
                                return false;
                        }
                        return DataValidateRoute.ValidateData(this, type, source, data, root);
                }
        }
}
