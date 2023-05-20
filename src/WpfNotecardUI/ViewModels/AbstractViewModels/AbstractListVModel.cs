using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfNotecardUI.Stores;

namespace WpfNotecardUI.ViewModels.AbstractViewModels
{
    public abstract class AbstractListVModel<T> : ObservableObject
    {
        private readonly NavigationStore _navigationStore;
        private readonly CategoryChildrenStore _categoryChildrenStore;
        private readonly IServiceProvider _serviceProvider;
        public List<T>? CurrentList { get; set; }
        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
        public ICommand GoToStartCommand { get; }
        public ICommand GoToPreviousCommand { get; }

        public AbstractListVModel(NavigationStore navigationStore, IServiceProvider serviceProvider, CategoryChildrenStore categoryChildrenStore)
        {
            _navigationStore = navigationStore;
            _categoryChildrenStore = categoryChildrenStore;
            _serviceProvider = serviceProvider;
            GoToStartCommand = new RelayCommand(GoToStartHandler);
            GoToPreviousCommand = new RelayCommand(GoToPreviousHandler);
        }

        public void GoToStartHandler()
        {

        }
        public void GoToPreviousHandler()
        {

        }

        public abstract void GetDataForList();
    }
}
