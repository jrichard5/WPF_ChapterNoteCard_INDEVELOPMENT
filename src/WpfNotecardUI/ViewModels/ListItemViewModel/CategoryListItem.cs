using CommunityToolkit.Mvvm.Input;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfNotecardUI.Stores;

namespace WpfNotecardUI.ViewModels.ListItemViewModel
{
    public class CategoryListItem
    {
        private readonly NavigationStore _navigationStore;
        private readonly IServiceProvider _serviceProvider;
        private Category _category;
        public Category Category 
        {
            get { return _category; } 
            set {  _category = value; } 
        }

        public CategoryListItem(Category category, NavigationStore navigationStore, IServiceProvider serviceProvider)
        {
            _category = category;
            _navigationStore = navigationStore;
            _serviceProvider = serviceProvider;
            GoToNextView = new RelayCommand(SwitchViews);

        }

        public ICommand GoToNextView { get; }

        public void SwitchViews()
        {
            Debug.WriteLine("hi");
        }

        //Deconstuctor to test if they get diposed of (Gets called around 1.5 minutes after switching back to main view)
        ~CategoryListItem()
        {
            Debug.WriteLine($"Category {Category.Id} has been deconstructed");
        }
    }
}
