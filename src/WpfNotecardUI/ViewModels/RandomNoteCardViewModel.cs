using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        public void SwitchToStart()
        {
            _navigationStore.CurrentViewModel = new StartPageViewModel(_navigationStore);
        }

        public RandomNoteCardViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            GoToStartViewModel = new RelayCommand(SwitchToStart);
        }
    }
}
