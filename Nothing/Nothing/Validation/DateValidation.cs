using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Nothing.Validation
{
    public class DateValidationRule : ValidationRule
    {
        public bool AllowFutureDates { get; set; } = false;
        public bool IsRequired { get; set; } = true;
        public string FieldName { get; set; } = "Дата";

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            if (IsRequired && (value == null || string.IsNullOrEmpty(value.ToString())))
            {
                return new ValidationResult(false, $"{FieldName} обязательна для заполнения.");
            }

            if (!IsRequired && (value == null || string.IsNullOrEmpty(value.ToString())))
            {
                return ValidationResult.ValidResult;
            }

            // Проверка типа
            if (!(value is DateTime date))
            {
                if (DateTime.TryParse(value?.ToString(), out DateTime parsedDate))
                {
                    date = parsedDate;
                }
                else
                {
                    return new ValidationResult(false, $"Введите {FieldName} в правильном формате.");
                }
            }

            if (!AllowFutureDates && date > DateTime.Now)
            {
                return new ValidationResult(false, $"{FieldName} не может быть в будущем.");
            }

            return ValidationResult.ValidResult;
        }
    }
    //public class DateValidationRule : ValidationRule
    //{
    //    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    //    {
    //        if (value is DateTime date)
    //        {
    //            if (date > DateTime.Now)
    //            {
    //                return new ValidationResult(false, "Дата не может быть в будущем.");
    //            }
    //        }
    //        else if (value is null)
    //        {
    //            return new ValidationResult(false, "Поле пусто");
    //        }
    //        else
    //        {
    //            return new ValidationResult(false, "Введите дату в правильном формате");

    //        }
    //        return ValidationResult.ValidResult;
    //    }
    //}
}
