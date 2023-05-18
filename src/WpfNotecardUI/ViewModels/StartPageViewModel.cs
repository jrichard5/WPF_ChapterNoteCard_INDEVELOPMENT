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
    public class StartPageViewModel : ObservableObject
    {
        ~StartPageViewModel()
        {
            Console.WriteLine("hi");
        }

        private readonly NavigationStore _navigationStore;
        public StartPageViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            GoToRandomCommand = new RelayCommand(SwitchToRandom);
        }

        public ICommand GoToRandomCommand { get; }

        public void SwitchToRandom()
        {
            _navigationStore.CurrentViewModel = new RandomNoteCardViewModel(_navigationStore);
        }
    }
}
