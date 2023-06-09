﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataLayer.Entities;
using DataLayer.IRepos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfNotecardUI.Models;
using WpfNotecardUI.Stores;
using WpfNotecardUI.ViewModels.ListVModels;

namespace WpfNotecardUI.ViewModels
{
    public class CategoryListViewModel : ObservableObject
    {
        private readonly NavigationStore _navigationStore;
        private readonly IServiceProvider _serviceProvider;
        private bool _isLoading;
        public List<Category>? DbCategories { get; set; }
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

        public CategoryListViewModel(NavigationStore naviStore, IServiceProvider serviceProvider)
        {
            _navigationStore = naviStore;
            _serviceProvider = serviceProvider;
            GoToStartComand = new RelayCommand(SwitchToStart);
            LoadCategories();
        }

        public static CategoryListViewModel CreateCategoryListView(NavigationStore naviStore, IServiceProvider serviceProvider)
        {
            CategoryListViewModel model = new CategoryListViewModel(naviStore, serviceProvider);
            model.LoadCategories();
            return model;
        }

        //public ICommand Something { get; }

        public ICommand GoToStartComand { get; }
        public void SwitchToStart()
        {
            _navigationStore.CurrentViewModel = new StartPageViewModel(_navigationStore, _serviceProvider);
        }

        public void SwitchToChapterView(Category category)
        {
            _serviceProvider.GetService<CategoryChildrenStore>().ChildrenStack.Push(_navigationStore.CurrentViewModel);
            if (category.CategoryName == "Japanese Vocab")
            {
                _navigationStore.CurrentViewModel = new KanjiListViewModel(_navigationStore, _serviceProvider);
            }
            else
            {
                _navigationStore.CurrentViewModel = new GenericChapterListViewModel(category.Id, _navigationStore, _serviceProvider);
            }
            

        }

        private async void LoadCategories()
        {
            IsLoading = true;
            DbCategories = new List<Category>();
            using (var scope = _serviceProvider.CreateScope())
            {
                //Guessing it is a scopped serviceProvider >.>
                var scopedServiceProvider = scope.ServiceProvider;
                var categoryRepo = scopedServiceProvider.GetRequiredService<ICategoryRepo>();
                //var fromDb = categoryRepo.GetAll().ContinueWith(task => DbCategories.AddRange(task.Result));
                var fromDb = await categoryRepo.GetAll();
                DbCategories.AddRange(fromDb);
                OnPropertyChanged(nameof(DbCategories));
            }
            IsLoading = false;
        }
    }
}
