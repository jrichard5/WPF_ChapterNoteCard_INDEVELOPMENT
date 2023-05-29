using Microsoft.Extensions.DependencyInjection;
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
using DataLayer;

namespace WpfNotecardUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddDiForDbContext();
            services.AddSingleton<NavigationStore>();


            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            NavigationStore store = _serviceProvider.GetService<NavigationStore>();
            if(store == null)
            {
                throw new Exception();
            }

            store.CurrentViewModel = new StartPageViewModel(store, _serviceProvider);

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(store)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
