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
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
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
        public ICommand ChapterCheck { get; }
        public ChapterSelectionViewModel(NavigationStore navigationStore, IServiceProvider serviceProvider)
        {
            _navigationStore = navigationStore;
            _serviceProvider = serviceProvider;
            CategoryCheck = new RelayCommand<int>(CategoryCheckFunction);
            ChapterCheck = new RelayCommand<string>(ChapterCheckFunction);
            GoToStartComand = new RelayCommand(SwitchToStart);
            TreeModel = new ObservableCollection<CategoryTreeModel>();
            GetData();
        }

        public void ChapterCheckFunction(string? chapterName)
        {
            //Should only be one because chapter Name is the primary key
            var category = TreeModel.First(c => c.Children.Any(c => c.ChapterName == chapterName));
            var chapter = category.Children.First(c => c.ChapterName == chapterName);

            if (chapter is null)
            {
                throw new ArgumentException();
            }
            if (chapter.IsFocused)
            {
                chapter.IsFocused = false;
                category.IsFocused = false;
            }
            else
            {
                chapter.IsFocused = true;
            }
            Properties.Settings.Default.ChaptersJSON = JsonSerializer.Serialize(TreeModel);
            Properties.Settings.Default.Save();
        }

        public void CategoryCheckFunction(int categoryId)
        {
            var results = TreeModel.First(node => node.CategoryId == categoryId);
            if (results.IsFocused)
            {
                foreach (var child in results.Children)
                {
                    child.IsFocused = false;
                }
                results.IsFocused = false;
            }
            else
            {
                foreach (var child in results.Children)
                {
                    child.IsFocused = true;
                }
                results.IsFocused = true;

            }
            OnPropertyChanged(nameof(TreeModel));
            Properties.Settings.Default.ChaptersJSON = JsonSerializer.Serialize(TreeModel);
            Properties.Settings.Default.Save();

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
            CheckSavedData();
            Debug.Write(TreeModel);
        }


        private void CheckSavedData()
        {
            string saveJSON = Properties.Settings.Default.ChaptersJSON.ToString();
            ObservableCollection<CategoryTreeModel> oldTree = null;
            try
            {
                oldTree = JsonSerializer.Deserialize<ObservableCollection<CategoryTreeModel>>(saveJSON);
            }
            catch (JsonException ex)
            {
                Debug.WriteLine(ex.Message);

            }

            if (oldTree is null)
            {
                return;
            }
            foreach (var category in oldTree)
            {
                if (category.IsFocused)
                {
                    CategoryCheckFunction(category.CategoryId);
                }
                else
                {
                    foreach (var child in category.Children)
                    {
                        if (child.IsFocused)
                        {
                            if (!(TreeModel.Any(c => c.Children.Any(c => c.ChapterName == child.ChapterName))))
                            {
                                continue;
                            }
                            ChapterCheckFunction(child.ChapterName);
                        }
                    }
                }
            }
        }
    }
}
