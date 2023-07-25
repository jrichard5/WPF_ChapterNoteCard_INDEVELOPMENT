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
using WpfNotecardUI.ViewModels.AbstractViewModels;

namespace WpfNotecardUI.ViewModels.DialogViewModels
{
    public class AddCategoryViewModel : AbstractClosableValidationViewModel
    {
        private readonly IServiceProvider _serviceProvider;

        private string _categoryName;
        public string CategoryName
        {
            get { return _categoryName; }
            set
            {
                IsPropertyValid(value);
                this._categoryName = value;
                OnPropertyChanged(nameof(CategoryName));
                AddCategoryCommand.NotifyCanExecuteChanged();
            }
        }

        public RelayCommand AddCategoryCommand { get; }
        public AddCategoryViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            AddCategoryCommand = new RelayCommand(AddCategoryFunction, CanAdd);
        }

        private async void AddCategoryFunction()
        {
            var categoryDataLayer = new Category { CategoryName = _categoryName };

            using (var scope = _serviceProvider.CreateScope())
            {
                var scopeServiceProvider = scope.ServiceProvider;
                var genericCateRepo = scopeServiceProvider.GetRequiredService<IGenericRepo<Category>>();
                try
                {
                    await genericCateRepo.AddAsync(categoryDataLayer);
                    OnRequestClose();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException is not null)
                    {
                        MessageBox.Show(ex.InnerException.Message);
                    }
                }
            }
        }

        public bool CanAdd()
        {
            if (CategoryName == string.Empty)
            {
                return false;
            }
            return !HasErrors;
        }
    }
}
