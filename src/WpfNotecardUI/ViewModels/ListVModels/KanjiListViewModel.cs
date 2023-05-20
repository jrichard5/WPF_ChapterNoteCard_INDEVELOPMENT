using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfNotecardUI.Stores;
using WpfNotecardUI.ViewModels.AbstractViewModels;

namespace WpfNotecardUI.ViewModels.ListVModels
{
    public class KanjiListViewModel : AbstractListVModel<KanjiNoteCard>
    {
        public KanjiListViewModel(NavigationStore navigationStore, IServiceProvider serviceProvider, CategoryChildrenStore categoryChildrenStore)
            : base(navigationStore, serviceProvider, categoryChildrenStore)
        {
            GetDataForList();
        }

        public override void GetDataForList()
        {
            CurrentList = new List<KanjiNoteCard>()
            {
                new KanjiNoteCard() {
                    TopicName = "hi",
                    ChapterNoteCard = new ChapterNoteCard()
                    {
                        TopicName = "hi",
                        Category = new Category()
                        {
                            CategoryName = "Category284795627",
                            Id = 1
                        },
                        CategoryId = 1,
                        ChapterSentences = new List<ChapterNoteCardSentenceNoteCard>(),
                        GradeLevel = 1,
                        LastTimeAccess = DateTime.Now,
                        Sentences = new List<SentenceNoteCard>()
                    },
                    JLPTLevel = 1,
                    KanjiReadings = new List<KanjiReading>(),
                    NewspaperRank = 1,
                }
            };
        }
    }
}
