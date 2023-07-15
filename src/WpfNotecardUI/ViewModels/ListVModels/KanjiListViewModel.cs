using CommunityToolkit.Mvvm.Input;
using DataLayer.Entities;
using DataLayer.IRepos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfNotecardUI.Mappers;
using WpfNotecardUI.Models;
using WpfNotecardUI.Services.IServices;
using WpfNotecardUI.Services.RealServices;
using WpfNotecardUI.Stores;
using WpfNotecardUI.ViewModels.AbstractViewModels;
using WpfNotecardUI.ViewModels.DialogViewModels;
using WpfNotecardUI.Views.Dialogs;

namespace WpfNotecardUI.ViewModels.ListVModels
{
    public class KanjiListViewModel : AbstractListVMExtra<KanjiListItemModel>
    {
        public KanjiListViewModel(NavigationStore navigationStore, IServiceProvider serviceProvider)
            : base(navigationStore, serviceProvider)
        {
            GetDataForList();
            GetCountFunction();
            
            //_dialogService = new DialogServices<KanjiWordDialog, AddKanjiWordViewModel>("Japanese Vocab", _serviceProvider);
        }

        public override void DeleteSelectedFunction()
        {
            var pkList = new List<ChapterNoteCard>();
            var ItemsSelected = CurrentList.Where(item => item.IsSelectedForDeletion == true).ToList();
            foreach (var item in ItemsSelected)
            {
                //Since the kanji notecard inherits from chapter, need to delete chapter
                pkList.Add(ModelToEntityMapper.ToChapterNoteCardPrimaryKey(item));
            }
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var genericRepo = scopedServiceProvider.GetRequiredService<IGenericRepo<ChapterNoteCard>>();
                genericRepo.DeleteByList(pkList);
            }
                Debug.WriteLine("hi");
        }

        public override void ExecuteShowDialog()
        {
            AddKanjiWordViewModel kanjiVM = new AddKanjiWordViewModel("Japanese Vocab", _serviceProvider);

            _dialogService = new DialogServices<AddKanjiWordViewModel>(kanjiVM);

            //callback Action<string>  Action is a delegate type
            _dialogService.ShowDialog(result =>
            {
                var teset = result;
                GetDataForList();
            });
        }


        public override async void GetDataForList()
        {
            IsLoading = true;
            CurrentList = new List<KanjiListItemModel>();
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var kanjiRepo = scopedServiceProvider.GetRequiredService<IKanjiNoteCardRepo>();
                var kanjiList = await kanjiRepo.GetAllWithAllInfo();
                foreach(var kanji in kanjiList)
                {
                    CurrentList.Add(new KanjiListItemModel(kanji));
                }
                OnPropertyChanged(nameof(CurrentList));
            }
            IsLoading = false;
        }

        public override void SaveDataFunction(KanjiListItemModel? hi)
        {
            throw new NotImplementedException();
        }

        public void SwitchToJapaneseWordView(KanjiListItemModel item)
        {
            _serviceProvider.GetService<CategoryChildrenStore>().ChildrenStack.Push(_navigationStore.CurrentViewModel);
            _navigationStore.CurrentViewModel = new JapaneseWordListViewModel(item, _navigationStore, _serviceProvider);
        }

        protected override async void GetCountFunction()
        {
            IsPageLoading = true;
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var genericRepo = scopedServiceProvider.GetRequiredService<IChapterNoteCardRepo>();
                MaxPageCount = await genericRepo.GetCountByCategoryName("Japanese Vocab");

            }
            LastPageNumber = GetCountHelperFunction((int)MaxPageCount);
            IsPageLoading = false;
        }
    }
}

//CurrentList = new List<KanjiNoteCard>()
//{
//    new KanjiNoteCard() {
//        TopicName = "hi",
//        ChapterNoteCard = new ChapterNoteCard()
//        {
//            TopicName = "hi",
//            Category = new Category()
//            {
//                CategoryName = "Category284795627",
//                Id = 1
//            },
//            CategoryId = 1,
//            ChapterSentences = new List<ChapterNoteCardSentenceNoteCard>(),
//            GradeLevel = 1,
//            LastTimeAccess = DateTime.Now,
//            Sentences = new List<SentenceNoteCard>()
//        },
//        JLPTLevel = 1,
//        KanjiReadings = new List<KanjiReading>(),
//        NewspaperRank = 1,
//    }
//};