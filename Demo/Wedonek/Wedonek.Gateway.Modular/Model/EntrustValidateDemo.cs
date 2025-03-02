using System;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Validate;

namespace Wedonek.Gateway.Modular.Model
{
    public class EntrustValidateOne
    {
        public string Id { get; set; }

        public EntrustValidateTwo Two
        {
            get;
            set;
        }
    }
    public class EntrustValidateTwo
    {
        [EntrustValidate("_checkUserName")]
        public string UserName { get; set; }

        /// <summary>
        /// 完整方法体
        /// </summary>
        /// <param name="source">当前对象</param>
        /// <param name="parent">上级对象(无这为空,如果无固定类型使用Object替代)</param>
        /// <param name="value">验证的属性值</param>
        /// <param name="proType">验证的属性类型</param>
        /// <exception cref="ErrorException"></exception>
        private void _checkUserName (EntrustValidateTwo source, EntrustValidateOne parent, string value, Type proType)
        {
            if (this.UserName.IsNull())
            {
                throw new ErrorException("rpc.demo.userName.null");
            }
        }
    }
}
