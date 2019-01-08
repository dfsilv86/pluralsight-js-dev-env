using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.CodeQuality.Validators
{
    public class ValidationResult
    {
        public ValidationResult(bool success)
        {
            Success = success;
        }

        public ValidationResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; private set; }
        public string Message { get; set; }

        public static implicit operator bool(ValidationResult result)
        {
            return result.Success;
        }

        public static implicit operator ValidationResult(bool success)
        {
            return new ValidationResult(success);
        }
    }
}
