using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfNotecardUI.ValidationRules
{
    public class StringMustBeInt : ValidationRule
    {
        private readonly int _min;
        private readonly int _max;

        public StringMustBeInt(int min = 0, int max = Int32.MaxValue)
        {
            _max = max;
            _min = min;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is not string stringToTest)
            {
                return new ValidationResult(false, "Please enter in a string");
            }
            if (!Int32.TryParse(stringToTest, out int number))
            {
                return new ValidationResult(false, "Please enter in a number");
            }
            if (number < _min)
            {
                return new ValidationResult(false, $"Must be greater than {_min}");
            }
            if (number > _max)
            {
                return new ValidationResult(false, $"Must be less than {_min}");
            }
            return ValidationResult.ValidResult;
        }
    }
}
