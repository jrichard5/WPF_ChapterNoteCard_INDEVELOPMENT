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


        public int lowercasesness { get; set; }
        public string myProperty { get; set; }
        public int uppercasesness { get; set; }
        public string myProperty2 { get; set; }
        public string myProperty3 { get; set; }
        public string myProperty4 { get; set; }
        public string myProperty5 { get; set; }
        public int lintingsuckls { get; set; }
        public string myProperty6 { get; set; }
    }
}
