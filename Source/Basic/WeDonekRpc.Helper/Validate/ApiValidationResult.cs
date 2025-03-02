using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeDonekRpc.Helper.Validate
{
        public class ApiValidationResult : ValidationResult
        {
                public ApiValidationResult(string errorMessage) : base(errorMessage)
                {

                }
                public ApiValidationResult(string errorMessage, IEnumerable<string> memberNames) : base(errorMessage, memberNames)
                {

                }
                public bool IgnoreValidation
                {
                        get;
                }
                public ApiValidationResult(bool ignoreValidation) : base(string.Empty)
                {
                        this.IgnoreValidation = ignoreValidation;
                }
        }
}
