using CommunityToolkit.Mvvm.ComponentModel;
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
using WpfNotecardUI.Services.IServices;
using WpfNotecardUI.Services.RealServices;
using WpfNotecardUI.Stores;
using WpfNotecardUI.ViewModels.DialogViewModels;
using WpfNotecardUI.ViewModels.ListVModels;

namespace WpfNotecardUI.ViewModels
{
    public class CategoryListViewModel : ObservableObject
    {
        private readonly NavigationStore _navigationStore;
        private readonly IServiceProvider _serviceProvider;
        private IDialogService _dialogService;
        private bool _isLoading;
        public List<CategoryModel>? DbCategories { get; set; }
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

        private bool isDeleteToggled = false;
        public bool IsDeleteToggled
        {
            get { return isDeleteToggled; }
            set
            {
                isDeleteToggled = value;
                OnPropertyChanged(nameof(IsDeleteToggled));
            }
        }

        public ICommand GoToStartComand { get; }
        public ICommand ToggleDeleteCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand DeleteSelectedCommand { get; }

        public CategoryListViewModel(NavigationStore naviStore, IServiceProvider serviceProvider)
        {
            _navigationStore = naviStore;
            _serviceProvider = serviceProvider;
            GoToStartComand = new RelayCommand(SwitchToStart);
            ToggleDeleteCommand = new RelayCommand(ToggleDeleteFunction);
            AddCommand = new RelayCommand(ExecuteShowDialog);
            DeleteSelectedCommand = new RelayCommand(DeleteSelectedFunction);
            LoadCategories();
        }

        public void DeleteSelectedFunction()
        {
            var pkList = new List<Category>();
            var itemsSelected = DbCategories.Where(item => item.IsSelectedForDeletion == true).ToList();
            foreach (var item in itemsSelected)
            {
                pkList.Add(new Category { Id = item.Id });
            }
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var genericRepo = scopedServiceProvider.GetRequiredService<IGenericRepo<Category>>();
                genericRepo.DeleteByList(pkList);
            }
            LoadCategories();
        }

        public void ToggleDeleteFunction()
        {
            IsDeleteToggled = !IsDeleteToggled;
        }

        public void ExecuteShowDialog()
        {
            var addCategoryVM = new AddCategoryViewModel(_serviceProvider);

            _dialogService = new DialogServices<AddCategoryViewModel>(addCategoryVM);

            //callback Action<string>  Action is a delegate type
            _dialogService.ShowDialog(result =>
            {
                LoadCategories();

            });
        }




        public void SwitchToStart()
        {
            _navigationStore.CurrentViewModel = new StartPageViewModel(_navigationStore, _serviceProvider);
        }

        public void SwitchToChapterView(CategoryModel category)
        {
            _serviceProvider.GetService<CategoryChildrenStore>().ChildrenStack.Push(_navigationStore.CurrentViewModel);
            if (category.CategoryName == "Japanese Vocab")
            {
                _navigationStore.CurrentViewModel = new KanjiListViewModel(category.Id, _navigationStore, _serviceProvider);
            }
            else
            {
                _navigationStore.CurrentViewModel = new GenericChapterListViewModel(category.Id, _navigationStore, _serviceProvider);
            }


        }

        private async void LoadCategories()
        {
            IsLoading = true;
            DbCategories = new List<CategoryModel>();
            using (var scope = _serviceProvider.CreateScope())
            {
                //Guessing it is a scopped serviceProvider >.>
                var scopedServiceProvider = scope.ServiceProvider;
                var categoryRepo = scopedServiceProvider.GetRequiredService<ICategoryRepo>();
                //var fromDb = categoryRepo.GetAll().ContinueWith(task => DbCategories.AddRange(task.Result));
                var fromDb = await categoryRepo.GetAll();
                foreach (var entry in fromDb)
                {
                    DbCategories.Add(new CategoryModel(entry));
                    //DbCategories.AddRange(fromDb);
                }
                OnPropertyChanged(nameof(DbCategories));
            }
            IsLoading = false;
        }
    }
}


//public static CategoryListViewModel CreateCategoryListView(NavigationStore naviStore, IServiceProvider serviceProvider)
//{
//    CategoryListViewModel model = new CategoryListViewModel(naviStore, serviceProvider);
//    model.LoadCategories();
//    return model;
//}

//public ICommand Something { get; }