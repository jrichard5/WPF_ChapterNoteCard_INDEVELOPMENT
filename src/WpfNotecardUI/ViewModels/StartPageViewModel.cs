using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    public class StartPageViewModel : ObservableObject
    {
        public StartPageViewModel(NavigationStore navigationStore, Func<RandomNoteCardViewModel> createRNVM)
        {

            GoToRandomCommand = new NavigateCommand(navigationStore, createRNVM);
        }

        public ICommand GoToRandomCommand { get; }
    }
}
