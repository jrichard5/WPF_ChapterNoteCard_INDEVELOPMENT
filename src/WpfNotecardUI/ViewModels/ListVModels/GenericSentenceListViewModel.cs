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
using System.Windows.Input;
using WpfNotecardUI.Models;
using WpfNotecardUI.Services.RealServices;
using WpfNotecardUI.Stores;
using WpfNotecardUI.ViewModels.AbstractViewModels;
using WpfNotecardUI.ViewModels.DialogViewModels;

namespace WpfNotecardUI.ViewModels.ListVModels
{
    public class GenericSentenceListViewModel : AbstractListVMExtra<SentenceNoteCardModel>
    {
        private readonly string _topicName;
        public ICommand DeleteCommand { get; }

        public GenericSentenceListViewModel(string topicName, NavigationStore navigationStore, IServiceProvider serviceProvider)
            : base(navigationStore, serviceProvider)
        {
            DeleteCommand = new RelayCommand<object>(DeleteFunction);
            _topicName = topicName;
            GetDataForList();
        }

        public void DeleteFunction(object? item)
        {
            var selectedNotecard = item as SentenceNoteCardModel;
            if (selectedNotecard == null)
            {
                return;
            }
            var pkForDelete = new SentenceNoteCard()
            {
                ItemQuestion = selectedNotecard.ItemQuestion
            };
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedSP = scope.ServiceProvider;
                var genericRepo = scopedSP.GetRequiredService<IGenericRepo<SentenceNoteCard>>();
                genericRepo.DeleteWithAttach(pkForDelete);
            }
            GetDataForList();
        }

        public override void ExecuteShowDialog()
        {
            AddSentenceViewModel sentenceVM = new AddSentenceViewModel(_topicName, _serviceProvider);
            _dialogService = new DialogServices<AddSentenceViewModel>(sentenceVM);
            //TODO: When adding, I don't re get the count for pages
            _dialogService.ShowDialog(result =>
            {
                GetDataForList();
            });
        }

        public override async void GetDataForList()
        {
            IsLoading = true;
            CurrentList = new List<SentenceNoteCardModel>();
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var sentenceRepo = scopedServiceProvider.GetRequiredService<ISentenceNoteCardRepo>();
                var sentenceList = await sentenceRepo.GetAllWithAChapter(_topicName);
                foreach (var sentence in sentenceList)
                {
                    CurrentList.Add(new SentenceNoteCardModel(sentence));
                }
            }
            GetCountFunction();
            IsLoading = false;
            OnPropertyChanged(nameof(CurrentList));
        }

        public override async void SaveDataFunction(SentenceNoteCardModel? hi)
        {
            if (CurrentList == null)
            {
                return;
            }
            List<SentenceNoteCardModel> changedItems = CurrentList.Where(item => ItemQuestionsThatHaveChanged.Contains(item.ItemQuestion)).ToList();
            List<SentenceNoteCard> backToDb = new List<SentenceNoteCard>();
            foreach (var item in changedItems)
            {
                backToDb.Add(new SentenceNoteCard
                {
                    ItemQuestion = item.ItemQuestion,
                    ItemAnswer = item.ItemAnswer,
                    IsUserWantsToFocusOn = item.IsUserWantsToFocusOn,
                    Hint = item.Hint,
                    LastTimeAccess = item.LastTimeAccessed,
                    MemorizationLevel = item.MemorizationLevel,
                    ChapterSentences = new List<ChapterNoteCardSentenceNoteCard>
                       {
                           new ChapterNoteCardSentenceNoteCard
                           {
                                ChapterNoteCardTopicName = _topicName,
                                SentenceNoteCardItemQuestion = item.ItemQuestion,
                           }
                       }
                });
            }
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var genericRepo = scopedServiceProvider.GetRequiredService<IGenericRepo<SentenceNoteCard>>();
                await genericRepo.BulkUpdateGeneric(backToDb);
            }
            ItemQuestionsThatHaveChanged.Clear();
            SaveData.NotifyCanExecuteChanged();
        }

        protected override async void GetCountFunction()
        {
            IsPageLoading = true;
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var sentRepo = scopedServiceProvider.GetRequiredService<ISentenceNoteCardRepo>();
                MaxPageCount = await sentRepo.CountFromOneChapter(_topicName);
            }
            LastPageNumber = GetCountHelperFunction((int)MaxPageCount);
            IsPageLoading = false;
        }
    }
}
