using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfNotecardUI.ViewModels.AbstractViewModels
{
    public class AbstractValidationViewModel : ObservableObject, INotifyDataErrorInfo
    {

        protected Dictionary<string, IList<object>> Errors = new Dictionary<string, IList<object>>();
        protected Dictionary<string, IList<ValidationRule>> ValidationRules = new Dictionary<string, IList<ValidationRule>>();

        #region INotifyDataErrorInfo
        public bool HasErrors => this.Errors.Any();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public event Action HelpNotify;

        public void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            HelpNotify?.Invoke();
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            if( propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
            if(this.Errors.TryGetValue(propertyName, out var errors))
            {
                return errors;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region AmazingGenericValidationCodeFromStackOverflow

        //https://stackoverflow.com/questions/56606441/how-to-add-validation-to-view-model-properties-or-how-to-implement-inotifydataer for more info
        public bool IsPropertyValid<TValue>(TValue propertyValue, [CallerMemberName] string propertyName = null)
        {
            ClearErrors(propertyName);

            if (this.ValidationRules.TryGetValue(propertyName, out var validationRules))
            {
                IEnumerable<object> errorMessages = validationRules
                        .Select(rule => rule.Validate(propertyValue, CultureInfo.CurrentCulture))
                        .Where(result => !result.IsValid)
                        .Select(validationResult => validationResult.ErrorContent);
                AddRangeErrors(propertyName, errorMessages);
                return !errorMessages.Any();

            }
            return true;

        }

        private void AddRangeErrors(string propertyName, IEnumerable<object> errorMessages)
        {
            if (!errorMessages.Any())
            {
                return;
            }
            if(!Errors.TryGetValue(propertyName, out var errors))
            {
               errors = new List<object>();
                Errors.Add(propertyName, errors);
            }
            foreach (var error in errorMessages)
            {
                errors.Add(error);
            }
            OnErrorsChanged(propertyName);
        }

        private bool ClearErrors(string propertyName)
        {
            if (Errors.Remove(propertyName))
            {
                OnErrorsChanged(propertyName);
                return true;
            }
            return false;
        }
        #endregion
    }
}
