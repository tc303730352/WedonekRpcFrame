namespace RpcHelper.Validate
{
        internal interface IValidateData
        {
                string AttrName
                {
                        get;
                }
                IValidateAttr[] Attr
                {
                        get;
                }
                IStrFormatFilter FormatFilter
                {
                        get;
                }
                void SyncAttr(IValidateAttr[] attrs);

                bool ValidateData(object data, object root, out string error);
        }
}