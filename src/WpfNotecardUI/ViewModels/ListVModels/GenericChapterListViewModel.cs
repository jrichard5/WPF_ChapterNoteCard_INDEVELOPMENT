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
using System.Windows.Controls;
using System.Windows.Input;
using WpfNotecardUI.Mappers;
using WpfNotecardUI.Models;
using WpfNotecardUI.Services.RealServices;
using WpfNotecardUI.Stores;
using WpfNotecardUI.ViewModels.AbstractViewModels;
using WpfNotecardUI.ViewModels.DialogViewModels;

namespace WpfNotecardUI.ViewModels.ListVModels
{
    public class GenericChapterListViewModel : AbstractListVMExtra<ChapterItemModel>
    {
        private readonly int _categoryId;
        public ICommand DeleteCommand { get; }

        public GenericChapterListViewModel(int categoryId, NavigationStore navigationStore, IServiceProvider serviceProvider)
            : base(navigationStore, serviceProvider)
        {
            DeleteCommand = new RelayCommand<object>(DeleteFunction);
            _categoryId = categoryId;
            GetDataForList();
        }

        public void SwitchToGenericSentenceView(ChapterItemModel item)
        {
            _serviceProvider.GetService<CategoryChildrenStore>().ChildrenStack.Push(_navigationStore.CurrentViewModel);
            _navigationStore.CurrentViewModel = new GenericSentenceListViewModel(item.TopicName, _navigationStore, _serviceProvider);
        }

        public override async void GetDataForList()
        {
            IsLoading = true;
            CurrentList = new List<ChapterItemModel>();
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var chapterRepo = scopedServiceProvider.GetRequiredService<IChapterNoteCardRepo>();
                var chapterList = await chapterRepo.GetPerPageFromOneCategory(_categoryId, pageNumber, NUMBER_PER_PAGE);
                foreach (var chapter in chapterList)
                {
                    CurrentList.Add(new ChapterItemModel(chapter));
                }
                OnPropertyChanged(nameof(CurrentList));
            }
            GetCountFunction();
            IsLoading = false;
        }

        protected override async void GetCountFunction()
        {
            IsPageLoading = true;
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var chapRepo = scopedServiceProvider.GetRequiredService<IChapterNoteCardRepo>();
                MaxPageCount = await chapRepo.CountFromOneCategory(_categoryId);
            }
            LastPageNumber = GetCountHelperFunction((int)MaxPageCount);
            IsPageLoading = false;
        }

        public override async void SaveDataFunction(ChapterItemModel? hi)
        {
            if (CurrentList == null)
            {
                return;
            }
            var list = ItemQuestionsThatHaveChanged;
            List<ChapterItemModel> changedItems = CurrentList.Where(item => ItemQuestionsThatHaveChanged.Contains(item.TopicName)).ToList();
            List<ChapterNoteCard> backToDb = new List<ChapterNoteCard>();
            foreach (var item in changedItems)
            {
                backToDb.Add(ModelToEntityMapper.FromChapterItemToChapterNoteCard(item, _categoryId));
            }
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var genericRepo = scopedServiceProvider.GetRequiredService<IGenericRepo<ChapterNoteCard>>();
                await genericRepo.BulkUpdateGeneric(backToDb);
            }
            ItemQuestionsThatHaveChanged.Clear();
            SaveData.NotifyCanExecuteChanged();
        }

        public override void ExecuteShowDialog()
        {
            AddChapterViewModel chapterVM = new AddChapterViewModel(_categoryId.ToString(), _serviceProvider);
            _dialogService = new DialogServices<AddChapterViewModel>(chapterVM);
            _dialogService.ShowDialog(result =>
            {
                GetDataForList();
            });
        }

        public void DeleteFunction(object? item)
        {
            var selectedNotecard = item as ChapterItemModel;
            if (selectedNotecard == null)
            {
                return;
            }
            //https://stackoverflow.com/questions/43547647/how-to-make-right-click-button-context-menu-in-wpf
            Debug.WriteLine("hi");

            var topicName = selectedNotecard.TopicName;
            var pkForDelete = new ChapterNoteCard()
            {
                TopicName = topicName,
            };
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var genericRepo = scopedServiceProvider.GetRequiredService<IGenericRepo<ChapterNoteCard>>();
                genericRepo.DeleteWithAttach(pkForDelete);
            }
            GetDataForList();
        }
    }
}
