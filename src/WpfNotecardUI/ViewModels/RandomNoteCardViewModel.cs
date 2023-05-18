using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfNotecardUI.Commands;
using WpfNotecardUI.Stores;

namespace WpfNotecardUI.ViewModels
{
    public class RandomNoteCardViewModel : ObservableObject
    {
        private readonly NavigationStore _navigationStore;

        public ICommand GoToStartViewModel { get; }

        public RandomNoteCardViewModel(NavigationStore navigationStore, Func<StartPageViewModel> createStartVM)
        {
            _navigationStore = navigationStore;
            GoToStartViewModel = new NavigateCommand(navigationStore, createStartVM);
        }
    }
}
