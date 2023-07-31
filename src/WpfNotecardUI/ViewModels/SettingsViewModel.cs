using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WpfNotecardUI.Stores;
using WpfNotecardUI.ValidationRules;
using WpfNotecardUI.ViewModels.AbstractViewModels;

namespace WpfNotecardUI.ViewModels
{
    public class SettingsViewModel : AbstractValidationViewModel
    {
        private string _numbersPerPage = Properties.Settings.Default.NumberPerPage.ToString();
        private NavigationStore _navigationStore;
        private readonly IServiceProvider _serviceProvider;

        public string NumbersPerPage
        {
            get { return _numbersPerPage; }
            set
            {
                IsPropertyValid(value);
                _numbersPerPage = value;
                OnPropertyChanged(nameof(NumbersPerPage));
            }
        }

        private bool _onlyFocus = Properties.Settings.Default.OnlyUseOnFocus;
        public bool OnOnlyFocus
        {
            get { return _onlyFocus; }
            set
            {
                _onlyFocus = value;
                OnPropertyChanged(nameof(OnOnlyFocus));
            }
        }
        public RelayCommand SaveCommand { get; }
        public ICommand GoToStartCommand { get; }
        public SettingsViewModel(NavigationStore navigationStore, IServiceProvider serviceProvider)
        {
            _navigationStore = navigationStore;
            _serviceProvider = serviceProvider;
            SaveCommand = new RelayCommand(SaveSettingsFunction, CanSave);
            ValidationRules.Add(nameof(NumbersPerPage), new List<ValidationRule> { new StringMustBeInt(0, 256) });
            GoToStartCommand = new RelayCommand(GoToStartFunction);
        }

        public void GoToStartFunction()
        {
            _navigationStore.CurrentViewModel = new StartPageViewModel(_navigationStore, _serviceProvider);
        }

        public void SaveSettingsFunction()
        {
            if(!Int32.TryParse(NumbersPerPage, out int result))
            {
                throw new ArgumentException();
            }
            Properties.Settings.Default.NumberPerPage = result;
            Properties.Settings.Default.OnlyUseOnFocus = OnOnlyFocus;

            Properties.Settings.Default.Save();

        }

        public bool CanSave()
        {
            return !HasErrors;
        }
    }
}
