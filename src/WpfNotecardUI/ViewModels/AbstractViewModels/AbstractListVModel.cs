using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
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
        protected readonly NavigationStore _navigationStore;
        protected readonly IServiceProvider _serviceProvider;
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

        public AbstractListVModel(NavigationStore navigationStore, IServiceProvider serviceProvider)
        {
            _navigationStore = navigationStore;
            _serviceProvider = serviceProvider;
            GoToStartCommand = new RelayCommand(GoToStartHandler);
            GoToPreviousCommand = new RelayCommand(GoToPreviousHandler);
        }

        public void GoToStartHandler()
        {
            _serviceProvider.GetService<CategoryChildrenStore>().ChildrenStack.Clear();
            _navigationStore.CurrentViewModel = new StartPageViewModel(_navigationStore, _serviceProvider);
        }
        public void GoToPreviousHandler()
        {
            _navigationStore.CurrentViewModel = _serviceProvider.GetService<CategoryChildrenStore>().ChildrenStack.Pop(); ;
        }

        public abstract void GetDataForList();
    }
}
