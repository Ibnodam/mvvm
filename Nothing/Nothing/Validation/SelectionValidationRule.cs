using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Nothing.Validation
{
    internal class SelectionValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return value.ToString() == "0"
          ? new ValidationResult(false, "Please select one")
          : new ValidationResult(true, null);
        }
    }
}
