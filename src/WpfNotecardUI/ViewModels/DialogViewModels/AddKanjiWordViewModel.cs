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
using WpfNotecardUI.ValidationRules;

namespace WpfNotecardUI.ViewModels.DialogViewModels
{
    public class AddKanjiWordViewModel : ObservableObject, INotifyDataErrorInfo
    {
        Dictionary<string, IList<object>> Errors = new Dictionary<string, IList<object>>();
        Dictionary<string, IList<ValidationRule>> ValidationRules = new Dictionary<string, IList<ValidationRule>>()
        {
            { nameof(TopicName), new List<ValidationRule> {new OneCharacterValdiationRule()} }
        };

        private string _parentName;
        private string topicName;
        public string TopicName
        {
            get { return topicName; }
            set
            {
                bool isValueValid = IsPropertyValid(value);
                if (isValueValid)
                {
                    this.topicName = value;
                    OnPropertyChanged(nameof(TopicName));
                }
            }
        }

        public AddKanjiWordViewModel(string parentName, IServiceProvider serviceLocator)
        {
            _parentName = parentName;

        }

        #region Inotifydataerror
        public bool HasErrors => this.Errors.Any();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
            if (this.Errors.TryGetValue(propertyName, out var errors))
            {
                return errors;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region stackoverflow to manage errors
        //https://stackoverflow.com/questions/56606441/how-to-add-validation-to-view-model-properties-or-how-to-implement-inotifydataer for more info
        public bool IsPropertyValid<TValue>(TValue propertyValue, [CallerMemberName] string propertyName = null) 
        {
            ClearErrors(propertyName);

            if (this.ValidationRules.TryGetValue(propertyName, out var rules))
            {
                IEnumerable<object> errorMessages = rules
                    .Select(validationRule => validationRule.Validate(propertyValue, CultureInfo.CurrentCulture))
                    .Where(result => !result.IsValid)
                    .Select(invalidResult => invalidResult.ErrorContent);
                AddErrorRange(propertyName, errorMessages);
                return !errorMessages.Any();
            }
            return true;
        }

        public bool ClearErrors(string propertyName)
        {
            if (this.Errors.Remove(propertyName))
            {
                OnErrorsChanged(propertyName);
                return true;
            }
            return false;
        }

        private void AddErrorRange(string propertyName, IEnumerable<object> newErrors)
        {
            if (!newErrors.Any())
            {
                return;
            }
            if (!this.Errors.TryGetValue(propertyName, out var pErrors))
            {
                pErrors = new List<object>();
                this.Errors.Add(propertyName, pErrors);
            }
            foreach (var error in newErrors)
            {
                pErrors.Add(error);
            }
            OnErrorsChanged(propertyName);
        }

        protected virtual void OnErrorsChanged(string propertyName)
        {
            this.ErrorsChanged.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        #endregion
    }
}
