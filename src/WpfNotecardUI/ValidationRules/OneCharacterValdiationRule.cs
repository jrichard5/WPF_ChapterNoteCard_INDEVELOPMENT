using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfNotecardUI.ValidationRules
{
    public class OneCharacterValdiationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!(value is string topicName))
            {
                return new ValidationResult(false, "Programmer needs this to be a string");
            }

            if (topicName.Count() != 1)
            {
                return new ValidationResult(false, "Must be only one character long");
            }

            return ValidationResult.ValidResult;
        }
    }
}
