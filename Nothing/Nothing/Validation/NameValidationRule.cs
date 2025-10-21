using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Nothing.Validation
{
    class NameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null) return new ValidationResult(false, "Поле пустое");
            string name = value.ToString()!;
            if (name.All(c => char.IsLetter(c) || c == ' '))
                return new ValidationResult(true, null);
            return new ValidationResult(false, "Должны быть только буквы");
        }
    }
}
