using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private bool isDeleteToggled = false;
        public bool IsDeleteToggled
        {
            get { return isDeleteToggled; }
            set
            {
                isDeleteToggled = value;
                OnPropertyChanged(nameof(IsDeleteToggled));
            }
        }
        public ICommand GoToStartCommand { get; }
        public ICommand GoToPreviousCommand { get; }
        public ICommand ToggleDeleteCommand { get; }
        public ICommand DeleteSelectedCommand { get; }

        public AbstractListVModel(NavigationStore navigationStore, IServiceProvider serviceProvider)
        {
            _navigationStore = navigationStore;
            _serviceProvider = serviceProvider;
            GoToStartCommand = new RelayCommand(GoToStartHandler);
            GoToPreviousCommand = new RelayCommand(GoToPreviousHandler);
            ToggleDeleteCommand = new RelayCommand(ToggleDeleteFunction);
            DeleteSelectedCommand = new RelayCommand(DeleteSelectedFunction);
        }

        public virtual void ToggleDeleteFunction()
        {
            IsDeleteToggled = !IsDeleteToggled;
        }

        public virtual void DeleteSelectedFunction()
        {
            throw new NotImplementedException();
        }

        public virtual void GoToStartHandler()
        {
            _serviceProvider.GetService<CategoryChildrenStore>().ChildrenStack.Clear();
            _navigationStore.CurrentViewModel = new StartPageViewModel(_navigationStore, _serviceProvider);
        }
        public virtual void GoToPreviousHandler()
        {
            _navigationStore.CurrentViewModel = _serviceProvider.GetService<CategoryChildrenStore>().ChildrenStack.Pop(); ;
        }

        public abstract void GetDataForList();
    }
}
