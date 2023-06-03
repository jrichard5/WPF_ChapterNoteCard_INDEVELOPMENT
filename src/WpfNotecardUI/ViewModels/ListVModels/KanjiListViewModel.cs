using DataLayer.Entities;
using DataLayer.IRepos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfNotecardUI.Models;
using WpfNotecardUI.Stores;
using WpfNotecardUI.ViewModels.AbstractViewModels;

namespace WpfNotecardUI.ViewModels.ListVModels
{
    public class KanjiListViewModel : AbstractListVModel<KanjiListItemModel>
    {

        public KanjiListViewModel(NavigationStore navigationStore, IServiceProvider serviceProvider)
            : base(navigationStore, serviceProvider)
        {
            GetDataForList();
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

        public void SwitchToJapaneseWordView(KanjiListItemModel item)
        {
            _navigationStore.CurrentViewModel = new JapaneseWordListViewModel(item, _navigationStore, _serviceProvider);
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