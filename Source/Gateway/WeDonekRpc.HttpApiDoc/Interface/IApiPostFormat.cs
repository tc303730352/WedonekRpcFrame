using System.Collections.Generic;
using WeDonekRpc.HttpApiDoc.Error;

namespace WeDonekRpc.HttpApiDoc.Interface
{
    internal interface IApiPostFormat : IApiDataFormat
    {
        List<FormatError> DataFormat { get; set; }
        List<LenError> LenFormat { get; set; }
        NullError NullCheck { get; set; }
        List<OtherError> OtherFormat { get; set; }
    }
}