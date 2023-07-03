using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WpfNotecardUI.ValidationRules;
using WpfNotecardUI.ViewModels.AbstractViewModels;

namespace WpfNotecardUI.ViewModels.DialogViewModels
{
    public class AddKanjiWordViewModel : AbstractValidationViewModel
    {
        private string _parentName;
        public string ParentName { get { return _parentName; } }
        public DateTime LastTimeAccessed { get; set; } = DateTime.Now;
        private string topicName = string.Empty;
        public string TopicName
        {
            get { return topicName; }
            set
            {
                IsPropertyValid(value);
                this.topicName = value;
                OnPropertyChanged(nameof(TopicName));
            }
        }
        private string topicDefinition = string.Empty;
        public string TopicDefinition
        {
            get { return topicDefinition; }
            set
            {
                IsPropertyValid(value);
                this.topicDefinition = value;
                OnPropertyChanged(nameof(TopicDefinition));
            }
        }
        public RelayCommand AddKanjiCommand { get; }

        public AddKanjiWordViewModel(string parentName, IServiceProvider serviceLocator)
        {
            _parentName = parentName;
            AddKanjiCommand = new RelayCommand(AddKanjiFunction, CanAdd);
            HelpNotify += NotifyCanAddChange;
            //Register the ValidationRules
            ValidationRules.Add(nameof(TopicName), new List<ValidationRule>{ new OneCharacterValdiationRule() });
            ValidationRules.Add(nameof(TopicDefinition), new List<ValidationRule> { new MaxCharacterValidationRule() });

        }

        public void NotifyCanAddChange()
        {
            AddKanjiCommand.NotifyCanExecuteChanged();
        }

        private async void AddKanjiFunction()
        {
            Debug.WriteLine("hi");
        }
        public bool CanAdd()
        {
            if(TopicName == string.Empty
                || TopicDefinition == string.Empty)
            {
                return false;
            }
            return !HasErrors;
        }
    }
}
