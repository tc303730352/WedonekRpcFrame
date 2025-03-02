namespace WeDonekRpc.HttpApiDoc.Model
{
    internal class ApiPostBody
    {
        /// <summary>
        /// 数据类型
        /// </summary>
        public ApiRequestType DataType
        {
            get;
            set;
        }
        /// <summary>
        /// 数组成员是否是类
        /// </summary>
        public bool IsClass
        {
            get;
            set;
        }
        /// <summary>
        /// 提交数据是否验证
        /// </summary>
        public bool IsValidate
        {
            set;
            get;
        }
        /// <summary>
        /// 数据说明
        /// </summary>
        public string Show
        {
            get;
            set;
        }
        /// <summary>
        /// 所属类ID
        /// </summary>
        public string ClassId { get; set; }

        /// <summary>
        /// 属性列表
        /// </summary>
        public ApiPostFormat[] ProList { get; set; }
    }
}
