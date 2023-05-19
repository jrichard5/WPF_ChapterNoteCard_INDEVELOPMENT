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

namespace WpfNotecardUI.ViewModels
{
    public class RandomNoteCardViewModel : ObservableObject
    {
        private readonly NavigationStore _navigationStore;

        public ICommand GoToStartViewModel { get; }

        private readonly IServiceProvider _serviceProvider;

        public void SwitchToStart()
        {
            _navigationStore.CurrentViewModel = new StartPageViewModel(_navigationStore, _serviceProvider);
        }

        public RandomNoteCardViewModel(NavigationStore navigationStore, IServiceProvider serviceProvider)
        {
            _navigationStore = navigationStore;
            GoToStartViewModel = new RelayCommand(SwitchToStart);
            _serviceProvider = serviceProvider;
        }
    }
}
