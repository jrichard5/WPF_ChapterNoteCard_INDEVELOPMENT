using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfNotecardUI.ValidationRules
{
    public class MaxCharacterValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(!(value is string stringToTest))
            {
                return new ValidationResult(false, "Must be string");
            }
            if (stringToTest.Length > 52) 
            {
                return new ValidationResult(false, "Cannot be more than 52 characters");
            }
            return ValidationResult.ValidResult;
        }
    }
}
