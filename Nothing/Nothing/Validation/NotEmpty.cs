using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Nothing.Validation
{
    class NotEmpty : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = ValidationResult.ValidResult;

            if (((string)value).Length == 0)
            {
                result = new ValidationResult(false, "String is empty");
            }
            return result;
        }
    }
}
