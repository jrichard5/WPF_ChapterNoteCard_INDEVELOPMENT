﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfNotecardUI.Stores;
using WpfNotecardUI.ViewModels.ListVModels;

namespace WpfNotecardUI.ViewModels
{
    public class StartPageViewModel : ObservableObject
    {
        ~StartPageViewModel()
        {
            Console.WriteLine("hi");
        }

        private readonly NavigationStore _navigationStore;
        public StartPageViewModel(NavigationStore navigationStore, IServiceProvider serviceProvider)
        {
            _navigationStore = navigationStore;
            GoToRandomCommand = new RelayCommand(SwitchToRandom);
            GoToBrowseAllCommand = new RelayCommand(SwitchToBrowse);
            GoToSettings = new RelayCommand(SwitchToSettings);
            _serviceProvider = serviceProvider;
        }

        public ICommand GoToSettings { get; }
        public void SwitchToSettings()
        {
            _navigationStore.CurrentViewModel = new KanjiListViewModel(_navigationStore, _serviceProvider, new CategoryChildrenStore());
        }
        public ICommand GoToRandomCommand { get; }

        public void SwitchToRandom()
        {
            _navigationStore.CurrentViewModel = new RandomNoteCardViewModel(_navigationStore, _serviceProvider);
        }

        public ICommand GoToBrowseAllCommand { get; }

        private readonly IServiceProvider _serviceProvider;

        public void SwitchToBrowse()
        {
            _navigationStore.CurrentViewModel = new CategoryListViewModel(_navigationStore, _serviceProvider);
            //_navigationStore.CurrentViewModel = CategoryListViewModel.CreateCategoryListView(_navigationStore, _serviceProvider);
        }
    }
}
