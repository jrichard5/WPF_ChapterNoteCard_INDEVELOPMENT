using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataLayer.Entities;
using DataLayer.IRepos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfNotecardUI.Models.TreeNodes;
using WpfNotecardUI.Stores;

namespace WpfNotecardUI.ViewModels
{
    public class ChapterSelectionViewModel : ObservableObject
    {
        private readonly NavigationStore _navigationStore;
        private readonly IServiceProvider _serviceProvider;
        public ObservableCollection<CategoryTreeModel> TreeModel { get; set; }

        public ICommand GoToStartComand { get; }
        public ICommand CategoryCheck { get; }
        public ChapterSelectionViewModel(NavigationStore navigationStore, IServiceProvider serviceProvider)
        {
            _navigationStore = navigationStore;
            _serviceProvider = serviceProvider;
            CategoryCheck = new RelayCommand<string>(CategoryCheckFunction);
            GoToStartComand = new RelayCommand(SwitchToStart);
            TreeModel = new ObservableCollection<CategoryTreeModel>();
            GetData();
        }

        public void CategoryCheckFunction(string? categoryName)
        {

        }

        public void SwitchToStart()
        {
            _navigationStore.CurrentViewModel = new StartPageViewModel(_navigationStore, _serviceProvider);
        }

        public async void GetData()
        {
            List<IGrouping<Category, ChapterNoteCard>> groupingByCategory;
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var chapterRepo = scopedServiceProvider.GetRequiredService<IChapterNoteCardRepo>();
                groupingByCategory = await chapterRepo.GroupByCategory();
            }
            foreach (var grouping in groupingByCategory)
            {
                var childrenList = grouping.Select(c => new ChapterTreeModel
                {
                    ChapterName = c.TopicName
                });

                var collection = new ObservableCollection<ChapterTreeModel>(childrenList);

                var cateTreeNode = new CategoryTreeModel
                {
                    CategoryId = grouping.Key.Id,
                    CategoryName = grouping.Key.CategoryName,
                    Children = collection
                };
                TreeModel.Add(cateTreeNode);
            }
            Debug.Write(TreeModel);
        }
    }
}
