using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfNotecardUI.ValidationRules
{
    public class IntValidationRule : ValidationRule
    {
        private readonly int _min;
        private readonly int _max;

        public IntValidationRule(int min = Int32.MinValue, int max = Int32.MaxValue)
        {
            _min = min;
            _max = max;

        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if ((value is string stringToTest))
            {
                return new ValidationResult(false, "Please enter in a number");
            }
            if ((int)value < _min)
            {
                return new ValidationResult(false, $"Must be greater than {_min}");
            }
            if ((int)value > _max)
            {
                return new ValidationResult(false, $"Must be less than {_max}");
            }
            return ValidationResult.ValidResult;
        }
    }
}
