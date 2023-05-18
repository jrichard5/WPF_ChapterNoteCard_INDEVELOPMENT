using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfNotecardUI.Stores;
using WpfNotecardUI.ViewModels;

namespace WpfNotecardUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private readonly NavigationStore _navigationStore;

        public App()
        {
            _navigationStore = new NavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationStore.CurrentViewModel = CreateStartViewModel();

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

        private StartPageViewModel CreateStartViewModel()
        {
            return new StartPageViewModel(_navigationStore, CreateRandomNotecardViewModel);
        }

        private RandomNoteCardViewModel CreateRandomNotecardViewModel()
        {
            return new RandomNoteCardViewModel(_navigationStore, CreateStartViewModel);
        }
    }
}
